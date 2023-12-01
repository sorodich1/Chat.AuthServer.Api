using System.Net;
using System.Text.Json;

namespace ChatClient.Forms.Token
{
    public static class TokenLoader
    {
        public static Task<SecurityToken?> RequestToken(string userName, string password, string tokenServerUrl)
        {
            var context = GetContent(userName, password);
            return GetTokenAsync(context, tokenServerUrl);
        }

        private static FormUrlEncodedContent GetContent(string userName, string password)
        {
            var values = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string> ( "grant_type", "password" ),
                new KeyValuePair<string, string> ("username", userName ),
                new KeyValuePair<string, string> ( "password", password ),
                new KeyValuePair<string, string> ( "client_secret", "secret" ),
                new KeyValuePair<string, string> ("client_id", "microservice1"),
                 new KeyValuePair<string, string> ( "scope", "api1" ),
            };
            return new FormUrlEncodedContent(values);
        }

        private static async Task<SecurityToken?> GetTokenAsync(FormUrlEncodedContent content, string tokenServerUrl)
        {
            string responseResult;
            using (var client = new HttpClient())
            {
                var response = await client.PostAsync($"{tokenServerUrl}", content);
                if (response.StatusCode == HttpStatusCode.BadRequest || response.StatusCode == HttpStatusCode.InternalServerError)
                {
                    var responseText = await response.Content.ReadAsStringAsync();
                    if (!string.IsNullOrEmpty(responseText))
                    {
                        Console.WriteLine(responseText);
                        return null;
                    }
                }
                response.EnsureSuccessStatusCode();
                responseResult = await response.Content.ReadAsStringAsync();
            }
            try
            {
                if (!string.IsNullOrEmpty(responseResult))
                {
                    var yy = JsonSerializer.Deserialize<SecurityToken>(responseResult);
                    return yy;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }
    }
}
