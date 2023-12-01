using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;
using Client = IdentityServer4.Models.Client;

namespace Server.Auth.Helpers
{
    public class IdentityServerConfig
    {
        public static IEnumerable<Client> GetClients()
        {
           var client = new List<Client>
                {
                    new Client
                    {
                        ClientId = "HironMicroservice",
                        AllowAccessTokensViaBrowser = true,
                        IdentityTokenLifetime = 21600,
                        AuthorizationCodeLifetime = 21600,
                        AccessTokenLifetime = 21600,
                        AllowOfflineAccess =  true,
                        RefreshTokenUsage = TokenUsage.ReUse,
                        RefreshTokenExpiration = TokenExpiration.Sliding,
                        SlidingRefreshTokenLifetime = 1296000, //in seconds = 15 days
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
                    }
                };
            Program.Logger.Info($"Создано {client.Count} клиентов");
            return client;
        }
           

        public static IEnumerable<IdentityResource> GetResource()
        {
            var identityResource = new List<IdentityResource>()
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Email(),
                new IdentityResources.Profile()
            };
            Program.Logger.Info($"Создано {identityResource.Count} ресурсов");
            return identityResource;
        }

        public static IEnumerable<ApiResource> GetApiResource()
        {
            var apiResource = new List<ApiResource>()
            {
                new ApiResource("api1", "API Default")
            };
            Program.Logger.Info($"Создано {apiResource.Count} api ресурсов");
            return apiResource;
        }

        public static IEnumerable<ApiScope> GetScope()
        {
            yield return new ApiScope("api1");
        }
    }
}
