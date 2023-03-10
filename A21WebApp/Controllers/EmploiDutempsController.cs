using A21WebApp.Models;
using A21WebApp.Services;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace A21WebApp.Controllers
{
    public class EmploiDutempsController : Controller
    {
        private readonly ILogger<EmploiDutempsController> _logger;

        private string baseURL = "https://localhost:7109/";

        private readonly IEmploiDuTempsService _serviceEmploiduTemps;
        private readonly IEnseignantService _serviceEnseigant;

        public EmploiDutempsController(ILogger<EmploiDutempsController> logger, IEmploiDuTempsService serviceEmploiduTemps, IEnseignantService serviceEnseigant)
        {
            _logger = logger;
            _serviceEmploiduTemps = serviceEmploiduTemps;
            _serviceEnseigant = serviceEnseigant;
        }

        public async Task<IActionResult> Index()
        {
            ViewData.Model = await _serviceEmploiduTemps.GetListeEmploiduTemps();
            return View();
        }

        public async Task<IActionResult> Detail(int id)
        {
            ViewData.Model = await _serviceEmploiduTemps.GetEmploiduTempsAvecCrenoHoraires(id);
            ViewData["Enseignants"] = await _serviceEnseigant.GetEnseignants();
            return View();
        }

        public async Task<IActionResult> Edit(int id)
        {
            ViewData.Model = await _serviceEmploiduTemps.GetEmploiduTempsAvecCrenoHoraires(id);
            ViewData["Enseignants"] = await _serviceEnseigant.GetEnseignants();
            return View();
        }

        //Methode pour afficher le formulaire de creation EmploiTemps
        //return view  Edit (modifier) car la create and update utilise le meme formulaire
        public async Task<IActionResult> Create()
        {
            var emp = new EmploiTemps();
            var crs = new List<CrenoHoraire>();
            string[] JoursArray = new string[] { "Lundi", "Mardi", "Mercredi", "Jeudi", "Vendredi" };

            // Add CrenoHoraires to the new EmploiTemps object

            for (int i = 0; i < 25; i++)
            {
                var crenoHoraire = new CrenoHoraire
                {
                    Jours = JoursArray[i / 5],
                    Periode = (i % 5) + 1,
                    ID = i + 1
                };
                crs.Add(crenoHoraire);
            }

            emp.CrenoHoraires = crs;
            ViewData.Model = emp;
            ViewData["Enseignants"] = await _serviceEnseigant.GetEnseignants();
            return View("Edit");
        }

        //Methode save permet l'enregistrement d'un new EploiTemps ou d'un update
        [HttpPost]
        public async Task<IActionResult> Save(int id)
        {
            //if (!ModelState.IsValid)
            var empt = getEmploiDuTempsFromForm();
            var emptResult = await _serviceEmploiduTemps.SaveEmploiduTemps(empt);
            if (emptResult != null)
            {
                ViewData["Errors"] = "";
                ViewData.Model = emptResult;
                return RedirectToAction("Detail", new RouteValueDictionary { { "id", empt.ID } });
            }
            else
            {
                ViewData.Model = empt;
                ViewData["Enseignants"] = await _serviceEnseigant.GetEnseignants();
                ViewData["Errors"] = _serviceEmploiduTemps.getErrors();
                return View("Edit");
            }
        }

        //Methode pour créer un objet EmploiTemps à partir de formulaire de update/Create
        private EmploiTemps getEmploiDuTempsFromForm()
        {
            EmploiTemps emp = getEmploiTempsFromFromH();
            var liste = new List<CrenoHoraire>();

            var crenoIds = Request.Form.Keys.Where(x => x.StartsWith("ID_")).Select(x => x.Split("_")[1]).ToList();
            crenoIds.ForEach(id =>
            {
                var crenoPrperties = Request.Form.Keys.Where(x => x.EndsWith($"_{id}")).ToList();
                var ch = getCreno(crenoPrperties);
                liste.Add(ch);
            });
            emp.CrenoHoraires = liste;
            return emp;
        }

        // return emploiTemps sans les crenoHoraires (Header)
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
                if (!string.IsNullOrWhiteSpace(value))
                {
                    switch (pName)
                    {
                        case "ID":
                            creno.ID = int.Parse(value);
                            break;

                        case "EnseignantID":
                            creno.EnseignantID = int.Parse(value) > 0 ? int.Parse(value) : null;
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

        public async Task<IActionResult> Delete(int id)
        {
            var result = await _serviceEmploiduTemps.DeleteEmploiduTemps(id);
            return RedirectToAction("Index");
        }
    }
}