using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessManager.Models.DipendenteModels
{
    public class BustaPaga
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Index { get; set; }

        // Chiave esterna per la relazione con Dipendente
        [ForeignKey("Dipendente")]
        public int DipendenteId { get; set; }

        public TimeSpan OreLavorate { get; set; }
        public decimal PagaOraria { get; set; }
        public TimeSpan OreStraordinario { get; set; }
        public decimal PagaStraordinario { get; set; }
        public decimal TotalePaga { get; set; }
        public DateOnly DataEmissione { get; set; }

        // Proprietà di navigazione per la relazione con Dipendente
        public Dipendente Dipendente { get; set; }
    }
}
