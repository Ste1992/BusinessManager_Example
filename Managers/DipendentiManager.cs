using BusinessManager.Models;
using BusinessManager.Services;

namespace BusinessManager.Managers
{
    public class DipendentiManager
    {

        // GESTIONE ANAGRAFICA DIPENDENTE
        public List<Dipendente> ListaDipendenti()
        {
            using (var dbContext = new MyDbContext())
            {
                return dbContext.Dipendenti.ToList();
            }
        }

        public void AddDipendente()
        {
            using (var db = new MyDbContext())
            {
                string? nome = "";
                while (string.IsNullOrEmpty(nome))
                {
                    Console.WriteLine("Nome:");
                    nome = Console.ReadLine();
                }

                string? cognome = "";
                while (string.IsNullOrEmpty(cognome))
                {
                    Console.WriteLine("Cognome:");
                    cognome = Console.ReadLine();
                }

                Console.WriteLine("ID:");
                int id = IdGenerator();
                Console.WriteLine(id);

                string? login = "";
                while (string.IsNullOrEmpty(login))
                {
                    Console.WriteLine("Login:");
                    login = Console.ReadLine();
                }

                string? inquadramento = "";
                while (string.IsNullOrEmpty(inquadramento))
                {
                    Console.WriteLine("Inquadramento:");
                    inquadramento = Console.ReadLine();
                }

                string? indirizzo = "";
                while (string.IsNullOrEmpty(indirizzo))
                {
                    Console.WriteLine("Indirizzo:");
                    indirizzo = Console.ReadLine();
                }

                string numeroCivico;
                while (true)
                {
                    Console.WriteLine("Numero civico:");
                    numeroCivico = Console.ReadLine();
                    if (int.TryParse(numeroCivico, out _))
                    {  
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Input errato. Inserire un valore valido.");
                    }
                }
                int numeroCivicoInt = int.Parse(numeroCivico);

                string cap;
                while (true)
                {
                    Console.WriteLine("CAP:");
                    cap = Console.ReadLine();
                    if (int.TryParse(cap, out _))
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Input errato. Inserire un valore valido.");
                    }
                }
                int capInt = int.Parse(cap);

                string? comune = "";
                while (string.IsNullOrEmpty(comune))
                {
                    Console.WriteLine("Comune:");
                    comune = Console.ReadLine();
                }

                string? provincia = "";
                while (string.IsNullOrEmpty(provincia))
                {
                    Console.WriteLine("Provincia:");
                    provincia = Console.ReadLine();
                }

                Console.WriteLine("Livello contratto:");
                while (true)
                {
                    if (int.TryParse(Console.ReadLine(), out int LivelloContratto))
                    {
                        db.Dipendenti.Add(new Dipendente { 
                            Nome = nome,
                            Cognome = cognome,
                            ID = id,
                            Login = login,
                            Inquadramento = inquadramento,
                            Indirizzo = indirizzo,
                            NumeroCivico = numeroCivicoInt,
                            Cap = capInt,
                            Comune = comune,
                            Provincia = provincia,
                            LivelloContratto = LivelloContratto
                        });
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Inserire un input corretto");
                    }
                }

                db.SaveChanges();
                Console.WriteLine("Dipendente aggiunto con successo!");
            }
        }

        private int IdGenerator()
        {
            Random random = new Random();
            int id;

            using (var dbContext = new MyDbContext())
            {
                do
                {
                    id = random.Next(1000, 10000); // Genera un codice a 4 cifre
                } while (dbContext.Dipendenti.Any(d => d.ID == id)); // Controlla se l'ID esiste già nel database
            }
            return id;
        }

        public void DeleteDipendente()
        {
            using (var dbContext = new MyDbContext())
            {
                try
                {
                    Console.WriteLine("Cerca dipendente per ID:");
                    if (int.TryParse(Console.ReadLine(), out int ID))
                    {
                        var dipendente = GetDipendenteById(ID);
                    
                        if (dipendente != null)
                        {
                            Console.WriteLine("Risultato ricerca:");
                            Console.WriteLine($"{dipendente.Nome} {dipendente.Cognome}, ID {dipendente.ID}, Login {dipendente.Login}");

                            Console.WriteLine("Eliminare? Y/N");
                            ConsoleKeyInfo key = Console.ReadKey();
                            char inputChar = char.ToLower(key.KeyChar);

                            if (inputChar == 'y')
                            {
                                dbContext.Dipendenti.Remove(dipendente);
                                dbContext.SaveChanges();
                                Console.WriteLine("Dipendente ID rimosso con successo");
                            }

                            else if (inputChar == 'n')
                            {
                                Console.WriteLine("Operazione annullata");
                            }
                            
                            else
                            {
                                Console.WriteLine("Input errato.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Input errato.");
                        }
                    }
                }
                catch (FormatException e)
                {
                    Console.WriteLine(e);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Errore generico: " + e);
                }
            }
        }

        public void EditDipendente()
        {
            Console.WriteLine("Cerca l'ID del dipendente da modificare:");            
            if (int.TryParse(Console.ReadLine(), out int ID))
            {
                EditTool(ID);
            }
        }

        private void EditTool(int ID)
        {
            using (var dbContext = new MyDbContext())
            {
                var dipendente = dbContext.Dipendenti.FirstOrDefault(d => d.ID == ID);

                if (dipendente != null)
                {
                    Console.WriteLine("Nome (Premere Invio per mantenere il valore attuale): ");
                    string nome = Console.ReadLine();
                    if (!string.IsNullOrEmpty(nome))
                    {
                        dipendente.Nome = nome;
                    }
                    
                    Console.WriteLine("Cognome (Premere Invio per mantenere il valore attuale): ");
                    string cognome = Console.ReadLine();
                    if (!string.IsNullOrEmpty(cognome))
                    {
                        dipendente.Cognome = cognome;
                    }
                    
                    Console.WriteLine("Login:");
                    string login = Console.ReadLine();
                    if (!string.IsNullOrEmpty(login))
                    {
                        dipendente.Login = login;
                    }

                    Console.WriteLine("Inquadramento:");
                    string inquadramento = Console.ReadLine();
                    if (!string.IsNullOrEmpty(inquadramento))
                    {
                        dipendente.Inquadramento = inquadramento;
                    }

                    Console.WriteLine("Indirizzo:");
                    string indirizzo = Console.ReadLine();
                    if (!string.IsNullOrEmpty(indirizzo))
                    {
                        dipendente.Indirizzo = indirizzo;
                    }

                    Console.WriteLine("Numero civico:");
                    string numeroCivico = Console.ReadLine();
                    if (!string.IsNullOrEmpty(numeroCivico))
                    {
                        if (int.TryParse(numeroCivico, out int civico))
                        {
                            dipendente.NumeroCivico = civico;
                        }
                    }

                    Console.WriteLine("Cap:");
                    string cap = Console.ReadLine();
                    if (!string.IsNullOrEmpty(cap))
                    {
                        if (int.TryParse(cap, out int c))
                        {
                            dipendente.Cap = c;
                        }
                    }

                    Console.WriteLine("Comune:");
                    string comune = Console.ReadLine();
                    if (!string.IsNullOrEmpty(comune))
                    {
                        dipendente.Comune = comune;
                    }

                    Console.WriteLine("Provincia:");
                    string provincia = Console.ReadLine();
                    if (!string.IsNullOrEmpty(provincia))
                    {
                        dipendente.Provincia = provincia;
                    }

                    dbContext.SaveChanges();
                    Console.WriteLine("Modifiche salvate con successo!");
                }
            }
        }

        public Dipendente GetDipendenteById(int ID)
        {
            using (var dbContext = new MyDbContext())
            {
                var dipendente = dbContext.Dipendenti.FirstOrDefault(d => d.ID == ID);

                if (dipendente != null)
                {
                    return dipendente;
                }
                return null;
            }
        }

        // GESTIONE CARTELLINO
        public void RegistraEntrata(int dipendenteId)
        {
            using (var dbContext = new MyDbContext())
            {
                var oraAttuale = DateTime.Now.Date;

                var timbraturaEsistente = dbContext.Timbrature
                .Any(t => t.DipendenteId == dipendenteId && t.Timestamp.Date == oraAttuale && t.TipoPresenza == TipoPresenza.Entrata);

                if (!timbraturaEsistente)
                {
                    var presenza = new Timbratura
                    {
                        DipendenteId = dipendenteId,
                        Timestamp = DateTime.Now,
                        TipoPresenza = TipoPresenza.Entrata
                    };

                    dbContext.Timbrature.Add(presenza);
                    dbContext.SaveChanges();
                    Console.WriteLine("Ingresso registrato con successo!");
                }
                else
                {
                    Console.WriteLine("Ingresso già registrato per questo dipendente.");
                }
            }
        }

        public void RegistraUscita(int dipendenteId)
        {
            using (var dbContext = new MyDbContext())
            {
                var oraAttuale = DateTime.Now.Date;

                var timbraturaEsistente = dbContext.Timbrature
                .Any(t => t.DipendenteId == dipendenteId && t.Timestamp.Date == oraAttuale && t.TipoPresenza == TipoPresenza.Uscita);

                if (!timbraturaEsistente)
                {
                    var presenza = new Timbratura
                    {
                        DipendenteId = dipendenteId,
                        Timestamp = DateTime.Now,
                        TipoPresenza = TipoPresenza.Uscita
                    };

                    dbContext.Timbrature.Add(presenza);
                    dbContext.SaveChanges();
                    Console.WriteLine("Uscita registrata con successo!");
                }
                else
                {
                    Console.WriteLine("Uscita già registrata per questo dipendente.");
                }
            }
        }

        public List<Timbratura> StoricoTimbratureDipendente()
        {
            using (var dbContext = new MyDbContext())
            {
                Console.WriteLine("Inserisci ID: ");
                string? id = Console.ReadLine();

                if (int.TryParse(id, out int ID))
                {
                    return dbContext.Timbrature
                        .Where(p => p.DipendenteId == ID)
                        .OrderBy(p => p.Timestamp)
                        .ToList();
                }
                return null;
            }
        }

    }
}
