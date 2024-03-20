using System.ComponentModel.DataAnnotations;
using BusinessManager.Models.DipendenteModels;

namespace BusinessManager.Models
{
    public class Dipendente
    {
        [Key]
        public int ID { get; set; }
        public string Nome { get; set; }
        public string Cognome { get; set; }
        public string Login { get; set; }
        public string Inquadramento { get; set; }
        public string Indirizzo { get; set; }
        public int NumeroCivico { get; set; }
        public int Cap { get; set; }
        public string Comune { get; set; }
        public string Provincia { get; set; }
        public int LivelloContratto { get; set; }

        public BustaPaga BustaPaga { get; set; }

        public List<Timbratura> Timbrature { get; set; }
    }
}
