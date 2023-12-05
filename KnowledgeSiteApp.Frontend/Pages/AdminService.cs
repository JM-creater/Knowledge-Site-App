using System.Net.Http.Json;
using KnowledgeSiteApp.Models.Dto;

namespace KnowledgeSiteApp.Frontend.Pages
{
    public class AdminService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<AdminService> _logger;

        public Models.Entities.User User { get; private set; } = new Models.Entities.User();
        public bool IsAuthenticated { get; private set; }

        public AdminService(HttpClient httpClient, ILogger<AdminService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task Login(LoginUserDto loginUser)
        {
            try
            {
                var response = await SendAuthorizedRequest(HttpMethod.Post, "https://localhost:7095/api/User/Login", JsonContent.Create(loginUser));

                if (response.IsSuccessStatusCode)
                {
                    User = await response.Content.ReadFromJsonAsync<Models.Entities.User>();
                    IsAuthenticated = true;
                }
                else
                {
                    _logger.LogError($"Login failed with status code: {response.StatusCode}");
                    IsAuthenticated = false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred during login: {ex.Message}");
                IsAuthenticated = false;
            }
        }

        private async Task<HttpResponseMessage> SendAuthorizedRequest(HttpMethod method, string url, HttpContent content = null)
        {
            var request = new HttpRequestMessage(method, url);
            request.Headers.Add("X-API-KEY", "KNOWLEDGESITEAPPAPI");

            if (content != null)
            {
                request.Content = content;
            }

            return await _httpClient.SendAsync(request);
        }
    }
}
