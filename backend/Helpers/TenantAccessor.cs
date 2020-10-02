using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CorPool.BackEnd.DatabaseModels;
using Microsoft.AspNetCore.Http;

namespace CorPool.BackEnd.Helpers {
    public interface ITenantAccessor {
        Tenant Tenant { get; }
    }

    public class TenantAccessor : ITenantAccessor {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public TenantAccessor(IHttpContextAccessor httpContextAccessor) {
            _httpContextAccessor = httpContextAccessor;
        }

        public Tenant Tenant => _httpContextAccessor.HttpContext.GetTenant();
    }
}
