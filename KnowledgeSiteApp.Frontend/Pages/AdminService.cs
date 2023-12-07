using System.Net.Http.Json;
using Blazored.SessionStorage;
using KnowledgeSiteApp.Models.Dto;
using KnowledgeSiteApp.Models.ErrorHandling;
using KnowledgeSiteApp.Models.Entities;

namespace KnowledgeSiteApp.Frontend.Pages
{
    public class AdminService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<AdminService> _logger;
        private readonly ISessionStorageService _sessionStorage;

        public User User { get; private set; } = new User();
        public bool IsAuthenticated { get; private set; }
        public string ErrorMessage { get; private set; }

        public AdminService(HttpClient httpClient, ILogger<AdminService> logger, ISessionStorageService sessionStorage)
        {
            _httpClient = httpClient;
            _logger = logger;
            _sessionStorage = sessionStorage;
        }

        public async Task Login(LoginUserDto loginUser)
        {
            try
            {
                var response = await SendAuthorizedRequest(HttpMethod.Post, "https://localhost:7095/api/User/Login", JsonContent.Create(loginUser));

                if (response.IsSuccessStatusCode)
                {
                    User = await response.Content.ReadFromJsonAsync<User>();
                    IsAuthenticated = true;
                }
                else
                {
                    var errorResponse = await response.Content.ReadFromJsonAsync<ErrorDetails>();
                    ErrorMessage = errorResponse.Message;
                    IsAuthenticated = false;
                }
            }
            catch (Exception e)
            {
                ErrorMessage = $"An error occurred during login: {e.Message}";
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

        public async Task SaveUserIdAsync(int userId)
        {
            await _sessionStorage.SetItemAsync("AdminId", userId);
        }

        public async Task<int?> GetUserIdAsync()
        {
            return await _sessionStorage.GetItemAsync<int?>("AdminId");
        }

        public async Task<bool> IsUserAuthenticatedAsync()
        {
            var userId = await GetUserIdAsync();
            return userId.HasValue;
        }
    }
}
