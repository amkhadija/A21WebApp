using A21WebApp.Models;

namespace A21WebApp.Services
{
    public interface IEnseignantService
    {
        public Task<IEnumerable<Enseignant>> GetEnseignants();
    }
}