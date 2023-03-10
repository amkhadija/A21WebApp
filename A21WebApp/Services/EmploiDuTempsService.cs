using A21WebApp.Models;
using Microsoft.Net.Http.Headers;

namespace A21WebApp.Services
{
    public class EmploiDuTempsService : IEmploiDuTempsService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _httpClient;
        private List<string> _erreurs;

        public EmploiDuTempsService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _httpClient = _httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7109/");
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
            _erreurs = new List<string>();
        }

        public async Task<EmploiTemps> GetEmploiduTemps(int id)
        {
            return await _httpClient.GetFromJsonAsync<EmploiTemps>($"api/EmploiTemps/{id}");
        }

        public async Task<EmploiTemps> GetEmploiduTempsAvecCrenoHoraires(int id)
        {
            var empt = await _httpClient.GetFromJsonAsync<EmploiTemps>($"api/EmploiTemps/{id}");
            empt.CrenoHoraires = await _httpClient.GetFromJsonAsync<IEnumerable<CrenoHoraire>>($"api/CrenoHoraires/EmploiTemps/{id}");
            return empt;
        }

        public async Task<IEnumerable<EmploiTemps>> GetListeEmploiduTemps()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<EmploiTemps>>("api/EmploiTemps");
        }

        public async Task<EmploiTemps> SaveEmploiduTemps(EmploiTemps emploiTemps)
        {
            var response = await _httpClient.PostAsJsonAsync<EmploiTemps>("api/EmploiTemps", emploiTemps);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                //var respContent1 = await response.Content.ReadAsStringAsync();
                var empt = await response.Content.ReadFromJsonAsync<EmploiTemps>();
                return empt;
            }

            var respContent = await response.Content.ReadAsStringAsync();
            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                _erreurs.Add(respContent);
            }
            //var requestContent = await response.RequestMessage.Content.ReadAsStringAsync();
            //var empt =  await response.Content.ReadFromJsonAsync<EmploiTemps>();
            //empt.CrenoHoraires = await _httpClient.GetFromJsonAsync<IEnumerable<CrenoHoraire>>($"api/CrenoHoraires/EmploiTemps/{emploiTemps.ID}");
            return null;
        }

        public async Task<EmploiTemps> CreateEmploiduTemps(EmploiTemps emploiTemps)
        {
            var response = await _httpClient.PostAsJsonAsync<EmploiTemps>("api/EmploiTemps", emploiTemps);
            var empt = await response.Content.ReadFromJsonAsync<EmploiTemps>();
            empt.CrenoHoraires = await _httpClient.GetFromJsonAsync<IEnumerable<CrenoHoraire>>($"api/CrenoHoraires/EmploiTemps/{emploiTemps.ID}");
            return empt;
        }

        public async Task<bool> DeleteEmploiduTemps(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/EmploiTemps/{id}");
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }

        public List<string> getErrors()
        {
            return _erreurs;
        }
    }
}