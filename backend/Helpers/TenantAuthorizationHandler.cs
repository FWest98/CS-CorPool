using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CorPool.BackEnd.Options;
using Microsoft.Extensions.Options;

namespace CorPool.BackEnd.Helpers {
    public class TenantAuthorizationHandler : AuthorizationHandler<TenantAuthorizationRequirement> {
        private readonly ITenantAccessor _tenantAccessor;
        private readonly IOptionsMonitor<AuthenticationOptions> _authOptions;

        public TenantAuthorizationHandler(ITenantAccessor tenantAccessor, IOptionsMonitor<AuthenticationOptions> authOptions) {
            _tenantAccessor = tenantAccessor;
            _authOptions = authOptions;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, TenantAuthorizationRequirement requirement) {
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
