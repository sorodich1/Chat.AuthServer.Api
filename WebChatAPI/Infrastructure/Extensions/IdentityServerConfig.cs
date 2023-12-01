using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace WebChatAPI.Infrastructure.Extensions
{
    public class IdentityServerConfig
    {
        public static IEnumerable<Client> GetClients() =>
            new List<Client>
            {
                new Client()
                {
                    ClientId = "microservice1",
                    AllowAccessTokensViaBrowser = true,
                    IdentityTokenLifetime = 21600,
                    AuthorizationCodeLifetime = 2600,
                    AccessTokenLifetime = 2600,
                    AllowOfflineAccess = true,
                    RefreshTokenUsage = TokenUsage.ReUse,
                    RefreshTokenExpiration = TokenExpiration.Sliding,
                    SlidingRefreshTokenLifetime = 1296000,
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    RequireClientSecret = false,
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.Address,
                        "api1"
                    }
                },
                 new Client
                {
                    ClientId = "microservice2",
                    AllowedGrantTypes = GrantTypes.Code,
                    RequireClientSecret = false,
                    RequireConsent = false,
                    RequirePkce = false,
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.Address,
                        "api1"
                    },
                     RedirectUris = {"https://localhost:1000/authentication/login_callback"},
                     PostLogoutRedirectUris = {"https://localhost:1000"},
                }
            };

        public static IEnumerable<IdentityResource> GetIdentityResources() => new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Email(),
            new IdentityResources.Profile()
        };

        public static IEnumerable<ApiResource> GetApiResources() => new List<ApiResource>
        {
            new ApiResource("api1", "API Default")
        };

        public static IEnumerable<ApiScope> GetApiScopes() 
        {
            yield return new ApiScope("api1");
        }
    }
}
