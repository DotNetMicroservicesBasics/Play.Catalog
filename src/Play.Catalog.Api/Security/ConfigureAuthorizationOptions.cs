using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Play.Catalog.Api.Consts;

namespace Play.Catalog.Api.Security
{
    public class ConfigureAuthorizationOptions : IConfigureNamedOptions<AuthorizationOptions>
    {
        public void Configure(string? name, AuthorizationOptions options)
        {
            options.AddPolicy(Policies.Read, policy =>
            {
                policy.RequireRole(Roles.Admin);
                policy.RequireClaim("scope", Claims.ReadAccess, Claims.Fullaccess);
            });

            options.AddPolicy(Policies.Write, policy =>
            {
                policy.RequireRole(Roles.Admin);
                policy.RequireClaim("scope", Claims.WriteAccess, Claims.Fullaccess);
            });
        }

        public void Configure(AuthorizationOptions options)
        {
            Configure(options);
        }
    }
}