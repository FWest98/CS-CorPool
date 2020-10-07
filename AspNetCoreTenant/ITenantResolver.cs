using System;
using System.Threading.Tasks;

namespace Corpool.AspNetCoreTenant {
    public interface ITenantResolver<TTenant> where TTenant : ITenant {
        Task<TTenant> ResolveTentantAsync(string identifier);
    }

    internal class TenantResolver<TTenant> : ITenantResolver<TTenant> where TTenant : class, ITenant {
        private readonly Func<string, Task<TTenant>> _resolveAsync;

        public TenantResolver(Func<string, Task<TTenant>> resolveAsync) {
            _resolveAsync = resolveAsync;
        }

        public Task<TTenant> ResolveTentantAsync(string identifier) => _resolveAsync(identifier);
    }
}
