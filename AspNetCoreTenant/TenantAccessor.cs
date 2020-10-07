using Microsoft.AspNetCore.Http;

namespace Corpool.AspNetCoreTenant {
    public interface ITenantAccessor : ITenantAccessor<ITenant> {}
    public interface ITenantAccessor<out TTenant> where TTenant : ITenant {
        TTenant Tenant { get; }
    }

    public class TenantAccessor : TenantAccessor<ITenant>, ITenantAccessor {
        public TenantAccessor(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor) { }
    }

    public class TenantAccessor<TTenant> : ITenantAccessor<TTenant> where TTenant : class, ITenant {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public TenantAccessor(IHttpContextAccessor httpContextAccessor) {
            _httpContextAccessor = httpContextAccessor;
        }

        public TTenant Tenant => _httpContextAccessor.HttpContext.GetTenant<TTenant>();
    }
}
