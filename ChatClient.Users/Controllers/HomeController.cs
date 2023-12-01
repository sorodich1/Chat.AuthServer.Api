using IdentityModel.Client;

using Microsoft.AspNetCore.Mvc;

namespace Authorization.Users.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public HomeController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetOrders()
        {
            var authClient = _httpClientFactory.CreateClient();
            var discoveryDocument = await authClient.GetDiscoveryDocumentAsync("https://localhost:1000");

            var tokenResponse = await authClient.RequestClientCredentialsTokenAsync(
                new ClientCredentialsTokenRequest
                {
                    Address = discoveryDocument.TokenEndpoint,
                    ClientId = "microservice1",
                    //ClientSecret = "client_secret",
                    Scope = "api1"
                });

            var ordersClient = _httpClientFactory.CreateClient();

            ordersClient.SetBearerToken(tokenResponse.AccessToken);

            var response = await ordersClient.GetAsync("https://localhost:7001/Home/GetSecrets");

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Message = response.StatusCode.ToString();
                return View();
            }

            var message = await response.Content.ReadAsStringAsync();

            ViewBag.Message = message;

            return View();
        }
    }
}