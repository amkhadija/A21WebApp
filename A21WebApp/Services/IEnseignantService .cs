using A21WebApp.Models;
using System.Net.Http.Headers;

namespace A21WebApp.Services
{
    public interface IEnseignantService
    {

        public  Task<IEnumerable<Enseignant>> GetEnseignants();
        
    }
}
