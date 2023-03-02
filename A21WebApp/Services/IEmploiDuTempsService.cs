using A21WebApp.Models;
using System.Net.Http.Headers;

namespace A21WebApp.Services
{
    public interface IEmploiDuTempsService
    {

        public  Task<IEnumerable<EmploiTemps>> GetListeEmploiduTemps();
        public  Task<EmploiTemps> GetEmploiduTemps(int id);
        public  Task<EmploiTemps> GetEmploiduTempsAvecCrenoHoraires(int id);
        
        public  Task<EmploiTemps> SaveEmploiduTemps(EmploiTemps emploiTemps);
    }
}
