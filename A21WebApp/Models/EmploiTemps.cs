using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace A21WebApp.Models
{
    public class EmploiTemps
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public string? Annee_Scolaire { get; set; }
        public string? Nom_Ecole { get; set; }
        public int? Groupe { get; set; }
        public int? Locale { get; set; }

        public IEnumerable<CrenoHoraire> CrenoHoraires { get; set; }

        public EmploiTemps()
        {
            CrenoHoraires = new List<CrenoHoraire>();
        }
    }
}