using A21WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using System.Diagnostics;
using System.Net.Http.Headers;

namespace A21WebApp.Controllers
{
    public class HomeController_ : Controller
    {
        private readonly ILogger<HomeController> _logger;

        string baseURL = "https://localhost:7109/";

        public HomeController_(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            //Calling the web API and populating the data in view using DataTable
            DataTable dt = new DataTable();
            using(var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage getData = await client.GetAsync("EmploiTemps");

                if(getData.IsSuccessStatusCode) 
                {
                    string results =getData.Content.ReadAsStringAsync().Result;
                    dt = JsonConvert.DeserializeObject<DataTable>(results);
                }
                else
                {
                    Console.WriteLine("Error calling web API");
                }
                ViewData.Model= dt;
            }

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

        public async Task<IActionResult> IndexEmploiTemps()
        {
            //calling the web API and populating the data in view usinsing dataTable
            DataTable dt = new DataTable();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage getData = await client.GetAsync("api/EmploiTemps");
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