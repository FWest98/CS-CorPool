using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CorPool.BackEnd.Helpers;
using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace CorPool.BackEnd.DatabaseModels {
    public class UserStore : 
        IUserStore<User>, 
        IUserPasswordStore<User>, 
        IUserEmailStore<User>, 
        IUserPhoneNumberStore<User>, 
        IQueryableUserStore<User>, 
        IUserSecurityStampStore<User> {

        private readonly DatabaseContext _database;
        private readonly ILookupNormalizer _normalizer;
        private readonly ITenantAccessor _tenantAccessor;
        private readonly ReplaceOptions _upsert = new ReplaceOptions { IsUpsert = true };

        // UsersQ is a tenanted queryable
        private IMongoQueryable<User> UsersQ => Users.Tenanted(_tenantAccessor.Tenant);
        private IMongoCollection<User> Users => _database.Users;

        public UserStore(DatabaseContext database, ILookupNormalizer normalizer, ITenantAccessor tenantAccessor) {
            _database = database;
            _normalizer = normalizer;
            _tenantAccessor = tenantAccessor;
        }

        public async Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken) {
            if(user == null) throw new ArgumentNullException(nameof(user));
            cancellationToken.ThrowIfCancellationRequested();

            // Check existing user
            if (await UsersQ.AnyAsync(s => s.UserName == user.UserName, cancellationToken)) {
                return IdentityResult.Failed(new IdentityError { Code = "Username already in use" });
            }

            // Insert new user
            user.TenantId = _tenantAccessor.Tenant.Id;
            await Users.InsertOneAsync(user, null, cancellationToken);

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(User user, CancellationToken cancellationToken) {
            if(user == null) throw new ArgumentNullException(nameof(user));
            cancellationToken.ThrowIfCancellationRequested();

            await Users.DeleteOneAsync(s => s.Id == user.Id, cancellationToken);
            return IdentityResult.Success;
        }

        public async Task<User> FindByIdAsync(string userId, CancellationToken cancellationToken) {
            cancellationToken.ThrowIfCancellationRequested();

            return await UsersQ.FirstOrDefaultAsync(s => s.Id == userId, cancellationToken);
        }

        public async Task<User> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken) {
            cancellationToken.ThrowIfCancellationRequested();

            return await UsersQ.FirstOrDefaultAsync(s => s.NormalizedUserName == normalizedUserName, cancellationToken);
        }

        public Task<string> GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken) {
            if(user == null) throw new ArgumentNullException(nameof(user));
            cancellationToken.ThrowIfCancellationRequested();

            return Task.FromResult(user.NormalizedUserName ?? _normalizer.NormalizeName(user.UserName));
        }

        public Task<string> GetUserIdAsync(User user, CancellationToken cancellationToken) {
            if(user == null) throw new ArgumentNullException(nameof(user));
            cancellationToken.ThrowIfCancellationRequested();

            return Task.FromResult(user.Id);
        }

        public Task<string> GetUserNameAsync(User user, CancellationToken cancellationToken) {
            if(user == null) throw new ArgumentNullException(nameof(user));
            cancellationToken.ThrowIfCancellationRequested();

            return Task.FromResult(user.UserName);
        }

        public async Task SetNormalizedUserNameAsync(User user, string normalizedName, CancellationToken cancellationToken) {
            if(user == null) throw new ArgumentNullException(nameof(user));
            cancellationToken.ThrowIfCancellationRequested();

            user.NormalizedUserName = normalizedName ?? _normalizer.NormalizeName(user.UserName);

            await Users.ReplaceOneAsync(s => s.Id == user.Id, user, _upsert, cancellationToken);
        }

        public async Task SetUserNameAsync(User user, string userName, CancellationToken cancellationToken) {
            if (user == null) throw new ArgumentNullException(nameof(user));
            cancellationToken.ThrowIfCancellationRequested();

            user.UserName = userName;
            user.NormalizedUserName = _normalizer.NormalizeName(userName);

            await Users.ReplaceOneAsync(s => s.Id == user.Id, user, _upsert, cancellationToken);
        }

        public async Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken) {
            if (user == null) throw new ArgumentNullException(nameof(user));
            cancellationToken.ThrowIfCancellationRequested();

            user.TenantId = _tenantAccessor.Tenant.Id;
            await Users.ReplaceOneAsync(s => s.Id == user.Id, user, _upsert, cancellationToken);

            return IdentityResult.Success;
        }

        public Task<string> GetPasswordHashAsync(User user, CancellationToken cancellationToken) {
            if (user == null) throw new ArgumentNullException(nameof(user));
            cancellationToken.ThrowIfCancellationRequested();

            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(User user, CancellationToken cancellationToken) {
            if (user == null) throw new ArgumentNullException(nameof(user));
            cancellationToken.ThrowIfCancellationRequested();

            return Task.FromResult(user.PasswordHash != null);
        }

        public async Task SetPasswordHashAsync(User user, string passwordHash, CancellationToken cancellationToken) {
            if (user == null) throw new ArgumentNullException(nameof(user));
            cancellationToken.ThrowIfCancellationRequested();

            user.PasswordHash = passwordHash;
            await Users.ReplaceOneAsync(s => s.Id == user.Id, user, _upsert, cancellationToken);
        }

        public async Task<User> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken) {
            cancellationToken.ThrowIfCancellationRequested();

            return await UsersQ.FirstOrDefaultAsync(s => s.NormalizedEmail == normalizedEmail, cancellationToken);
        }

        public Task<string> GetEmailAsync(User user, CancellationToken cancellationToken) {
            if (user == null) throw new ArgumentNullException(nameof(user));
            cancellationToken.ThrowIfCancellationRequested();

            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(User user, CancellationToken cancellationToken) {
            if (user == null) throw new ArgumentNullException(nameof(user));
            cancellationToken.ThrowIfCancellationRequested();

            return Task.FromResult(user.EmailConfirmed);
        }

        public Task<string> GetNormalizedEmailAsync(User user, CancellationToken cancellationToken) {
            if (user == null) throw new ArgumentNullException(nameof(user));
            cancellationToken.ThrowIfCancellationRequested();

            return Task.FromResult(user.NormalizedEmail);
        }

        public async Task SetEmailAsync(User user, string email, CancellationToken cancellationToken) {
            if (user == null) throw new ArgumentNullException(nameof(user));
            cancellationToken.ThrowIfCancellationRequested();

            user.Email = email;
            user.NormalizedEmail = _normalizer.NormalizeEmail(email);

            await Users.ReplaceOneAsync(s => s.Id == user.Id, user, _upsert, cancellationToken);
        }

        public async Task SetEmailConfirmedAsync(User user, bool confirmed, CancellationToken cancellationToken) {
            if (user == null) throw new ArgumentNullException(nameof(user));
            cancellationToken.ThrowIfCancellationRequested();

            user.EmailConfirmed = confirmed;

            await Users.ReplaceOneAsync(s => s.Id == user.Id, user, _upsert, cancellationToken);
        }

        public async Task SetNormalizedEmailAsync(User user, string normalizedEmail, CancellationToken cancellationToken) {
            if (user == null) throw new ArgumentNullException(nameof(user));
            cancellationToken.ThrowIfCancellationRequested();

            user.NormalizedEmail = normalizedEmail;

            await Users.ReplaceOneAsync(s => s.Id == user.Id, user, _upsert, cancellationToken);
        }

        public Task<string> GetPhoneNumberAsync(User user, CancellationToken cancellationToken) {
            if (user == null) throw new ArgumentNullException(nameof(user));
            cancellationToken.ThrowIfCancellationRequested();

            return Task.FromResult(user.PhoneNumber);
        }

        public Task<bool> GetPhoneNumberConfirmedAsync(User user, CancellationToken cancellationToken) {
            if (user == null) throw new ArgumentNullException(nameof(user));
            cancellationToken.ThrowIfCancellationRequested();

            return Task.FromResult(user.PhoneNumberConfirmed);
        }

        public async Task SetPhoneNumberAsync(User user, string phoneNumber, CancellationToken cancellationToken) {
            if (user == null) throw new ArgumentNullException(nameof(user));
            cancellationToken.ThrowIfCancellationRequested();

            user.PhoneNumber = phoneNumber;

            await Users.ReplaceOneAsync(s => s.Id == user.Id, user, _upsert, cancellationToken);
        }

        public async Task SetPhoneNumberConfirmedAsync(User user, bool confirmed, CancellationToken cancellationToken) {
            if (user == null) throw new ArgumentNullException(nameof(user));
            cancellationToken.ThrowIfCancellationRequested();

            user.PhoneNumberConfirmed = confirmed;

            await Users.ReplaceOneAsync(s => s.Id == user.Id, user, _upsert, cancellationToken);
        }

        IQueryable<User> IQueryableUserStore<User>.Users => UsersQ;

        public Task<string> GetSecurityStampAsync(User user, CancellationToken cancellationToken) {
            if (user == null) throw new ArgumentNullException(nameof(user));
            cancellationToken.ThrowIfCancellationRequested();

            return Task.FromResult(user.SecurityStamp);
        }

        public async Task SetSecurityStampAsync(User user, string stamp, CancellationToken cancellationToken) {
            if (user == null) throw new ArgumentNullException(nameof(user));
            cancellationToken.ThrowIfCancellationRequested();

            user.SecurityStamp = stamp;

            await Users.ReplaceOneAsync(s => s.Id == user.Id, user, _upsert, cancellationToken);
        }

        public void Dispose() {}
    }
}
