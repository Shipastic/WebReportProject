using System.Text.Json;

namespace DAPManSWebReports.Domain.IdentityService.TokenServise
{
    /// <summary>
    /// Сервис для создания токена используя фабрику
    /// </summary>
    public  class JwtHandler
    {
        private readonly HttpClient _httpClient;
        private readonly string _clientId;
        private readonly string secretKey;
        private readonly string _keycloakUrl;

        /// <summary>
        /// Generate jwt-token from token generator
        /// </summary>
        /// <param name="username"></param>
        /// <param name="secretKey"></param>
        /// <param name="issuer"></param>
        /// <param name="audience"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public async Task<string> GetTokenAsync(string username,    
                                                string secretKey, 
                                                string issuer, 
                                                string audience,
                                                string role)
        {
            var tokenGenerator = TokenFactory.CreateTokenGenerator(role);

            return await tokenGenerator.GenerateTokenAsync(username, secretKey, issuer, audience);
        }

        /// <summary>
        /// Get token from keycloak service
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="_clientId"></param>
        /// <param name="secretKey"></param>
        /// <returns></returns>
        /// <exception cref="HttpRequestException"></exception>
        public async Task<string> GetTokenAsync(string username, string password, string _clientId, string secretKey)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, $"{_keycloakUrl}/protocol/openid-connect/token");
            var content = new[]
            {
                new KeyValuePair<string, string>("username", username),
                new KeyValuePair<string, string> ("password", password),
                new KeyValuePair<string, string> ("client_id", _clientId),
                new KeyValuePair<string, string> ("client_secret", secretKey),
                new KeyValuePair<string, string> ("grand_type", "password")
            };

            request.Content = new FormUrlEncodedContent(content);

            var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Failed to get token: {response.ReasonPhrase}");
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var jsonDocument = JsonDocument.Parse(responseContent);
            return jsonDocument.RootElement.GetProperty("access_token").GetString();
        }
    }
}
