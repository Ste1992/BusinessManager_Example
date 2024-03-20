using System.Globalization;
using BusinessManager.Models;
using BusinessManager.Services;

namespace BusinessManager.Managers
{
    public class TimbraturaManager
    {

        // ELIMINA LA TIMBRATURA
        public void DeleteTimbratura()
        {
            using (var dbContext = new MyDbContext())
            {
                Console.WriteLine("Inserisci l'ID del dipendente:");
                if (int.TryParse(Console.ReadLine(), out int ID))
                {
                    var dipendente = dbContext.Dipendenti.FirstOrDefault(d => d.ID == ID);
                    if (dipendente != null)
                    {
                        Console.WriteLine("Inserisci la data (formato dd-MM-yyyy):");
                        if (DateTime.TryParseExact(Console.ReadLine(), "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateTime))
                        {
                            // MOSTRA LA LISTA DELLE TIMBRATURE
                            var timbrature = dbContext.Timbrature.Where(t => t.DipendenteId == ID && t.Timestamp.Date == dateTime.Date).ToList();
                            if (timbrature.Count > 0)
                            {
                                foreach (var t in timbrature)
                                {
                                    Console.WriteLine($"{t.Id}. Dipendente: {t.DipendenteId}, Timbratura: {t.Timestamp}");
                                }

                                Console.WriteLine("Indica l'indice da eliminare:");
                                if (int.TryParse(Console.ReadLine(), out int index))
                                {
                                    // RECUPERA LA TIMBRATURA SELEZIONATA
                                    var timbraturaDaCancellare = dbContext.Timbrature.Where(tb => tb.Id == index);
                                    if (timbraturaDaCancellare != null)
                                    {
                                        Console.WriteLine("Cancellare la timbratura? Y/N");
                                        ConsoleKeyInfo key = Console.ReadKey();
                                        char inputChar = char.ToLower(key.KeyChar);

                                        if (inputChar == 'y')
                                        {
                                            dbContext.Timbrature.RemoveRange(timbraturaDaCancellare);
                                            dbContext.SaveChanges();
                                            Console.WriteLine("Timbratura eliminata correttamente.");
                                        }
                                        else if (inputChar == 'n')
                                        {
                                            Console.WriteLine("Operazione annullata.");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Input invalido.");
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("Nessuna timbratura trovata all'indice indicato.");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("L'indice indicato non è corretto. Indicare un indice valido.");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Timbratura non presente per la data specificata.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Data non corretta.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Nessun dipendente trovato con l'ID specificato.");
                    }
                }
                else
                {
                    Console.WriteLine("Input non corretto. Inserire un ID valido.");
                }
            }
        }

        public void EditTimbratura()
        {
            using (var dbContext = new MyDbContext())
            {
                Console.WriteLine("Inserisci l'ID del dipendente:");
                if (int.TryParse(Console.ReadLine(), out int ID))
                {

                    var dipendente = dbContext.Dipendenti.FirstOrDefault(d => d.ID == ID);
                    if (dipendente != null)
                    {

                        Console.WriteLine("Inserisci la data della timbratura (formato DD-MM-YYYY):");
                        if (DateTime.TryParseExact(Console.ReadLine(), "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime datestamp))
                        {

                            var timbratura = dbContext.Timbrature.Where(t => t.Timestamp.Date == datestamp.Date).ToList();
                            if (timbratura.Count != 0)
                            {
                                Console.WriteLine("Timbrature trovate per la data specificata:");
                                foreach (var t in timbratura)
                                {
                                    Console.WriteLine($"ID dipendente: {t.DipendenteId}, Timestamp: {t.Timestamp}");
                                }

                                EditTimbraturaTool(ID);
                            }
                            else
                            {
                                Console.WriteLine("Formato data non valido.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Nessun dipendente trovato con l'ID specificato");
                        }
                    }
                    else
                    {
                        Console.WriteLine("ID del dipendente non trovato.");
                    }
                }
            }
        }


        private void EditTimbraturaTool(int ID)
        {
            using (var dbContext = new MyDbContext())
            {
                // Modifica la timbratura
                Console.WriteLine("Selezionare la timbratura da modificare:\n1. Entrata, 2. Uscita");
                if (int.TryParse(Console.ReadLine(), out int scelta))
                {
                    switch(scelta)
                    {
                        case 1: // Modifica dell'entrata
                            var entrata = dbContext.Timbrature.FirstOrDefault(t => t.DipendenteId == ID && t.TipoPresenza == TipoPresenza.Entrata);
                            if (entrata != null)
                            {
                                Console.WriteLine("Nuova marca temporale (formato HH:mm:ss):");
                                if (TimeSpan.TryParseExact(Console.ReadLine(), "hh\\:mm\\:ss", CultureInfo.InvariantCulture, out TimeSpan entrataTimestamp))
                                {
                                    entrata.Timestamp += entrataTimestamp;

                                    dbContext.SaveChanges();
                                    Console.WriteLine("Timbratura modificata con successo.");
                                }
                                else
                                {
                                    Console.WriteLine("Formato ora non valido.");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Nessuna timbratura di ingresso trovata per la data specificata.");
                            }
                            break;

                        case 2: // Modifica dell'uscita
                            var uscita = dbContext.Timbrature.FirstOrDefault(t => t.DipendenteId == ID && t.TipoPresenza == TipoPresenza.Uscita);
                            if (uscita != null)
                            {
                                Console.WriteLine("Nuova marca temporale (formato HH-mm-ss)");
                                if (TimeSpan.TryParseExact(Console.ReadLine(), "hh\\:mm\\:ss", CultureInfo.InvariantCulture, out TimeSpan uscitaTimestamp))
                                {
                                    uscita.Timestamp += uscitaTimestamp;

                                    dbContext.SaveChanges();
                                    Console.WriteLine("Timbratura modificata con successo.");
                                }
                                else
                                {
                                    Console.WriteLine("Formato ora non valido.");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Nessuna timbratura di uscita trovata per la data specificata.");
                            }
                            break;

                        default:
                            Console.WriteLine("Scelta errata.");
                            break;
                    }
                }
            }
        }
        
        
    }
}
