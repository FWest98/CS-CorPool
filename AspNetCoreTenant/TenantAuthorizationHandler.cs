using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Corpool.AspNetCoreTenant {
    public class TenantAuthorizationHandler<TOptions> : AuthorizationHandler<TenantAuthorizationRequirement>
        where TOptions : ITenantAuthOptions {
        private readonly ITenantAccessor _tenantAccessor;
        private readonly IOptionsMonitor<TOptions> _authOptions;

        public TenantAuthorizationHandler(ITenantAccessor tenantAccessor, IOptionsMonitor<TOptions> authOptions) {
            _tenantAccessor = tenantAccessor;
            _authOptions = authOptions;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            TenantAuthorizationRequirement requirement) {
            // If no valid tenant is set, unauthorized
            if (_tenantAccessor.Tenant == null) {
                context.Fail();
                return Task.CompletedTask;
            }

            // Check whether claim tenant matches
            if (context.User.HasClaim(s =>
                s.Type == "Tenant" &&
                s.Value == _tenantAccessor.Tenant.Id &&
                s.Issuer == _authOptions.CurrentValue.Authority
            )) {
                context.Succeed(requirement);
            } else {
                context.Fail();
            }

            return Task.CompletedTask;
        }
    }
}
