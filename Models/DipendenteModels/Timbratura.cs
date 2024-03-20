using System.ComponentModel.DataAnnotations;

namespace BusinessManager.Models
{
    public class Timbratura
    {
        [Key]
        public int Id { get; set; }
        
        public int DipendenteId { get; set; } // Riferimento al dipendente
        public DateTime Timestamp { get; set; } // Marca temporale per l'entrata o l'uscita del dipendente
        public TipoPresenza TipoPresenza { get; set; } // Enumerazione per distinguere l'entrata e l'uscita

        // Proprietà di navigazione per rappresentare la relazione con il dipendente
        public Dipendente Dipendente { get; set; }
    }

    public enum TipoPresenza
    {
        Entrata,
        Uscita
    }
}