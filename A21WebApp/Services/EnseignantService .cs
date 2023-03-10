using A21WebApp.Models;
using Microsoft.Net.Http.Headers;

namespace A21WebApp.Services
{
    public class EnseignantService : IEnseignantService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _httpClient;

        public EnseignantService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _httpClient = _httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7109/");
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
        }

        public async Task<IEnumerable<Enseignant>> GetEnseignants()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<Enseignant>>("api/Enseignants");
        }
    }
}