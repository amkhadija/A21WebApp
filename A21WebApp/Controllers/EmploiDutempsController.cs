using A21WebApp.Models;
using A21WebApp.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Diagnostics;
using System.Net.Http.Headers;

namespace A21WebApp.Controllers
{
    public class EmploiDutempsController : Controller
    {
        private readonly ILogger<EmploiDutempsController> _logger;

        string baseURL = "https://localhost:7109/";

        private readonly IEmploiDuTempsService _serviceEmploiduTemps;


        public EmploiDutempsController(ILogger<EmploiDutempsController> logger, IEmploiDuTempsService serviceEmploiduTemps)
        {
            _logger = logger;
            _serviceEmploiduTemps = serviceEmploiduTemps;
        }

        public async Task<IActionResult> Index()
        {
            ViewData.Model= await _serviceEmploiduTemps.GetListeEmploiduTemps();
            return View();
        }

        public async Task<IActionResult> Detail(int id)
        {
            ViewData.Model = await _serviceEmploiduTemps.GetEmploiduTempsAvecCrenoHoraires(id);
            return View();
        }

        public async Task<IActionResult> Edit(int id)
        {
            ViewData.Model = await _serviceEmploiduTemps.GetEmploiduTempsAvecCrenoHoraires(id);
            return View();
        }
        public async Task<IActionResult> Create(int id)
        {
            var t = getEmploiDuTempsFromForm();
            var emp = new EmploiTemps();
            var empt = await _serviceEmploiduTemps.SaveEmploiduTemps(emp);
            ViewData.Model = await _serviceEmploiduTemps.GetEmploiduTempsAvecCrenoHoraires(id);
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Save(int id)
        {
            //if (!ModelState.IsValid)
            var empt = getEmploiDuTempsFromForm();
            empt = await _serviceEmploiduTemps.SaveEmploiduTemps(empt);
            ViewData.Model = empt;//await _serviceEmploiduTemps.GetEmploiduTempsAvecCrenoHoraires(id);
            return RedirectToAction("Detail", new RouteValueDictionary { { "id", id } });
        }

        private EmploiTemps getEmploiDuTempsFromForm()
        {
            EmploiTemps emp = getEmploiTempsFromFromH();
            var liste = new List<CrenoHoraire>();

            for (int i = 1; i <= 25; i++)
            {
                var crenoPrperties = Request.Form.Keys.Where(x => x.EndsWith($"_{i}")).ToList();
                var ch = getCreno(crenoPrperties);
                liste.Add(ch);
            }
            emp.CrenoHoraires = liste;
            return emp;
        }

        private EmploiTemps getEmploiTempsFromFromH()
        {
            var emp = new EmploiTemps();
            var frm = Request.Form;
            var v = "";
            var prp = Request.Form.Keys.FirstOrDefault(x => x == "ID");
            if (prp != null)
            {
                v = Request.Form[prp].First();
                emp.ID = int.Parse(v);
            }
            prp = Request.Form.Keys.FirstOrDefault(x => x == "Annee_Scolaire");
            if (prp != null)
            {
                v = Request.Form[prp].First();
                emp.Annee_Scolaire = v;
            }

            prp = Request.Form.Keys.FirstOrDefault(x => x == "Locale");
            if (prp != null)
            {
                v = Request.Form[prp].First();
                emp.Locale = int.Parse(v);
            }

            prp = Request.Form.Keys.FirstOrDefault(x => x == "Groupe");
            if (prp != null)
            {
                v = Request.Form[prp].First();
                emp.Groupe = int.Parse(v);
            }
            prp = Request.Form.Keys.FirstOrDefault(x => x == "Nom_Ecole");
            if (prp != null)
            {
                v = Request.Form[prp].First();
                emp.Nom_Ecole = v;
            }

            return emp;
        }

        private CrenoHoraire getCreno(List<string> crenoPrperties)
        {
            var creno = new CrenoHoraire();
            crenoPrperties.ForEach(p =>
            {
                var pName = p.Split("_")[0];
                var value = Request.Form[p].First();
                if ( !string.IsNullOrWhiteSpace(value))
                {
                    switch (pName)
                    {
                        case "ID":
                            creno.ID = int.Parse(value);
                            break;
                        case "EnseignantID":
                            creno.EnseignantID = int.Parse(value);
                            break;
                        case "Jours":
                            creno.Jours = value;
                            break;
                        case "EmploiTempsID":
                            creno.EmploiTempsID = int.Parse(value);
                            break;
                        case "Periode":
                            creno.Periode = int.Parse(value);
                            break;
                    }
                }
            });
            return creno;
        }

        public async Task<IActionResult> getEnseigants(int id)
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
        //public async Task<IActionResult> getListEnseigants()
        //{
        //    ViewData.Model = await _serviceEmploiduTemps.GetListeEmploiduTemps();
        //    return View();
        //}

    }
}