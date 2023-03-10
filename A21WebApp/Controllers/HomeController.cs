using A21WebApp.Models;
using A21WebApp.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using System.Diagnostics;
using System.Net.Http.Headers;

namespace A21WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private string baseURL = "https://localhost:7109/";

        private readonly IEmploiDuTempsService _serviceEmploiduTemps;

        public HomeController(ILogger<HomeController> logger, IEmploiDuTempsService serviceEmploiduTemps)
        {
            _logger = logger;
            _serviceEmploiduTemps = serviceEmploiduTemps;
        }

        public async Task<IActionResult> Index()
        {
            ViewData.Model = await _serviceEmploiduTemps.GetListeEmploiduTemps();
            return View();
        }

        public async Task<IActionResult> IndexEnseignant()
        {
            //calling the web API and populating the data in view usinsing dataTable
            DataTable dt = new DataTable();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage getData = await client.GetAsync("api/Enseignants");
                if (getData.IsSuccessStatusCode)
                {
                    string results = getData.Content.ReadAsStringAsync().Result;
                    dt = JsonConvert.DeserializeObject<DataTable>(results);
                }
                else
                {
                    Console.WriteLine("Erreur calling web API");
                }
                ViewData.Model = dt;
            }
            return View();
        }

        public async Task<IActionResult> IndexCrenoHoraire()
        {
            //calling the web API and populating the data in view usinsing dataTable
            DataTable dt = new DataTable();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage getData = await client.GetAsync("api/CrenoHoraires");
                if (getData.IsSuccessStatusCode)
                {
                    string results = getData.Content.ReadAsStringAsync().Result;
                    dt = JsonConvert.DeserializeObject<DataTable>(results);
                }
                else
                {
                    Console.WriteLine("Erreur calling web API");
                }
                ViewData.Model = dt;
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}