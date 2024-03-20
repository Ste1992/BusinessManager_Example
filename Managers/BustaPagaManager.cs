using System.Globalization;
using BusinessManager.Models.DipendenteModels;
using BusinessManager.Services;

namespace BusinessManager.Managers
{
    public class BustaPagaManager
    {
        
        // Elenco buste paga per dipendente
        public List<BustaPaga> ListaBustePaga()
        {
            using (var dbContext = new MyDbContext())
            {
                DipendentiManager dm = new();
                while (true)
                {
                    Console.WriteLine("Inserisci l'ID per consultare le buste paga relative al dipendente:");
                    if (int.TryParse(Console.ReadLine(), out int dipendenteId))
                    {
                        var dipendente = dm.GetDipendenteById(dipendenteId);
                        if (dipendente == null)
                        {
                            Console.WriteLine("Dipendente non trovato. Inserire un ID valido.");
                            continue;
                        }
                        // Se il dipendente esiste, restituisci la lista delle buste paga relative a quel dipendente
                        return dbContext.BustaPaga.Where(bp => bp.DipendenteId == dipendenteId).ToList();
                    }
                    Console.WriteLine("ID non valido. Inserire un numero intero.");
                }
            }
        }

        public void GeneraBustaPaga()
        {
            using (var dbContext = new MyDbContext())
            {
                DipendentiManager dm = new();
                while (true)
                {
                    // Recupera il dipendente per ID
                    Console.WriteLine($"Inserire ID dipendente: ");
                    if (int.TryParse(Console.ReadLine(), out int ID))
                    {
                        var dipendente = dm.GetDipendenteById(ID);
                        if (dipendente != null)
                        {
                            // Mostra nome e cognome dipendente
                            Console.WriteLine("Nome:");
                            Console.WriteLine(dipendente.Nome);

                            Console.WriteLine("Cognome:");
                            Console.WriteLine(dipendente.Cognome);

                            Console.WriteLine("Paga base:");
                            // Definizione di una mappa per memorizzare le paghe orarie per ogni livello di contratto
                            Dictionary<int, decimal> pagheOrarie = new()
                            {
                                { 6, 6 }, // Lvl, paga oraria
                                { 5, 7 },
                                { 4, 8 },
                                { 3, 10}
                            };

                            dipendente.BustaPaga = new BustaPaga(); // Inizializza la busta paga

                            // Calcola la busta paga
                            decimal pagaOraria = dipendente.BustaPaga.PagaOraria; // Inizialmente impostata dalla busta paga del dipendente

                            // Se il livello di contratto del dipendente è presente nel dizionario delle paghe orarie, aggiorna la paga oraria
                            if (pagheOrarie.TryGetValue(dipendente.LivelloContratto, out decimal pagaOrariaDaDizionario))
                            {
                                pagaOraria = pagaOrariaDaDizionario; // Aggiorna la paga oraria
                            }
                            else
                            {
                                // Gestione dell'eventuale errore o valore di default
                                Console.WriteLine("Livello contratto non valido.");
                            }
//------------------------->COMPRENDERE BENE 
                            // Calcola le ore lavorate nel mese corrente
                            DateOnly oggi = DateOnly.FromDateTime(DateTime.Today);
                            DateTime inizioMese = new(oggi.Year, oggi.Month, 1); // Ottiene il primo giorno del mese corrente
                            DateTime fineMese = inizioMese.AddMonths(1).AddDays(-1); // Ottiene l'ultimo giorno del mese corrente

                            // Recupera tutte le timbrature del dipendente nel mese corrente e le ordina per data
                            var timbratureMese = dbContext.Timbrature
                                .Where(t => t.DipendenteId == ID && t.Timestamp >= inizioMese && t.Timestamp <= fineMese)
                                .OrderBy(t => t.Timestamp)
                                .ToList();

                            TimeSpan oreLavorate = TimeSpan.Zero; // Variabile per memorizzare le ore lavorate
                            TimeSpan oreStraordinario = TimeSpan.Zero; // Variabile per memorizzare le ore di straordinario

                            // Ciclo attraverso le timbrature per calcolare le ore lavorate e le ore di straordinario
                            for (int i = 0; i < timbratureMese.Count; i += 2)
                            {
                                var entrata = timbratureMese[i];
                                var uscita = (i + 1 < timbratureMese.Count) ? timbratureMese[i + 1] : null;

                                if (uscita != null)
                                {
                                    TimeSpan durataLavoro = uscita.Timestamp - entrata.Timestamp; // Calcola la durata del lavoro tra entrata e uscita

                                    // Se la durata del lavoro è inferiore o uguale a 8 ore, aggiunge le ore lavorate
                                    if (durataLavoro.TotalHours <= 8)
                                    {
                                        oreLavorate += durataLavoro;
                                    }
                                    else // Altrimenti, aggiunge 8 ore come lavoro normale e le ore restanti vanno nello straordinario
                                    {
                                        oreLavorate += TimeSpan.FromHours(8);
                                        oreStraordinario += durataLavoro - TimeSpan.FromHours(8);
                                    }
                                }
                            }

                            // Calcola la busta paga basata sulle ore lavorate e le ore di straordinario
                            decimal pagaStraordinario = pagaOraria * 1.5m; // Supponiamo che le ore di straordinario vengano pagate 50% in più

                            decimal totalePaga = (decimal)oreLavorate.TotalHours * pagaOraria + // Calcola la paga per le ore lavorate
                                                (decimal)oreStraordinario.TotalHours * pagaStraordinario; // Calcola la paga per le ore di straordinario
//------------------------->.............
                            GetTimbrature(ID);
                            Console.WriteLine($"Ore lavorate: {oreLavorate}");
                            Console.WriteLine($"Ore straordinario: {oreStraordinario}");
                            Console.WriteLine($"Totale paga: {totalePaga}");
                            Console.WriteLine($"Data emissione: {dipendente.BustaPaga.DataEmissione = oggi}");
                            Console.WriteLine($"Inquadramento: {dipendente.Inquadramento}");
                            Console.WriteLine($"Residenza: {dipendente.Indirizzo}, {dipendente.NumeroCivico}, {dipendente.Cap}, {dipendente.Comune}, {dipendente.Provincia}");

                            dbContext.BustaPaga.Add(new BustaPaga{
                                DipendenteId = ID,
                                OreLavorate = oreLavorate,
                                PagaOraria = pagaOraria,
                                OreStraordinario = oreStraordinario,
                                PagaStraordinario = pagaStraordinario,
                                TotalePaga = totalePaga,
                                DataEmissione = oggi,
                            });

                            dbContext.SaveChanges();
                            Console.WriteLine("Busta paga salvata con successo.");
                        }
                        else
                        {
                            Console.WriteLine("Dipendente non trovato.");
                        }
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Input errato. Inserire un ID valido.");
                    }
                }
            }
        }

        public void EditBustaPaga()
        {
            using (var dbContext = new MyDbContext())
            {
                while (true)
                {
                    Console.WriteLine("Inserire l'ID del dipendente:");
                    if (int.TryParse(Console.ReadLine(), out int ID))
                    {
                        var dipendente = dbContext.Dipendenti.FirstOrDefault(d => d.ID == ID);
                        if (dipendente != null)
                        {
                            var bustePaga = dbContext.BustaPaga.Where(bp => bp.DipendenteId == ID).ToList();
                            if (bustePaga.Count > 0)
                            {
                                for (int i = 0; i < bustePaga.Count; i++)
                                {
                                    Console.WriteLine($"{i}. ID: {bustePaga[i].DipendenteId}, Ore Lavorate: {bustePaga[i].OreLavorate}, Ore Straordinario: {bustePaga[i].OreStraordinario}, Totale Paga: {bustePaga[i].TotalePaga}");
                                }

                                while (true)
                                {
                                    Console.WriteLine("Selezionare la busta paga da modificare:");
                                    if(int.TryParse(Console.ReadLine(), out int input) && input >= 0 && input < bustePaga.Count)
                                    {
                                        var bustaSelezionata = bustePaga[input];
                                        Console.WriteLine("Ore lavorate ordinarie (formato hh:mm:ss):");
                                        
                                        string inputOre = Console.ReadLine();
                                        if (!string.IsNullOrEmpty(inputOre) && TimeSpan.TryParseExact(inputOre, @"hh\:mm\:ss", CultureInfo.InvariantCulture, out TimeSpan oreLavorate))
                                        {
                                            // L'input è nel formato corretto, puoi assegnarlo alla busta paga selezionata
                                            bustaSelezionata.OreLavorate = oreLavorate;
                                        }
                                        
                                        Console.WriteLine("Ore straordinario (formato hh:mm:ss)");
                                        string inputStraordinario = Console.ReadLine();
                                        if (!string.IsNullOrEmpty(inputStraordinario) && TimeSpan.TryParseExact(inputStraordinario, @"hh\:mm\:ss", CultureInfo.InvariantCulture, out TimeSpan oreStraordinario))
                                        {
                                            bustaSelezionata.OreStraordinario = oreStraordinario;
                                        }

                                        Console.WriteLine("Data emissione (formato dd\\MM\\yyyy)");
                                        string dataInput = Console.ReadLine();
                                        if (!string.IsNullOrEmpty(dataInput) && DateTime.TryParseExact(dataInput, @"dd\MM\yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dataEmissione))
                                        {
                                            DateOnly dataEmissioneDateOnly = new(dataEmissione.Year, dataEmissione.Month, dataEmissione.Day);
                                            bustaSelezionata.DataEmissione = dataEmissioneDateOnly;
                                        }

                                        dbContext.SaveChanges();
                                        Console.WriteLine("Modifiche apportate con successo.");
                                        break;
                                    }
                                    else
                                    {
                                        Console.WriteLine("Input errato. Inserire un valore valido.");
                                    }
                                }
                                
                            }
                            else
                            {
                                Console.WriteLine("Nessuna busta paga trovata per l'ID speicificato.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Nessuna dipendente trovato per l'ID selezionato");
                        }
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Input errato. Inserire un ID valido.");
                    }
                }
            }  
        }

        public void EliminaBustaPaga()
        {
            using (var dbContext = new MyDbContext())
            {
                Console.WriteLine("Inserire l'ID del dipendente:");
                if (int.TryParse(Console.ReadLine(), out int ID))
                {
                    var dipendente = dbContext.Dipendenti.FirstOrDefault(d => d.ID == ID);
                    if (dipendente != null)
                    {
                        var bustePaga = dbContext.BustaPaga.Where(bp => bp.DipendenteId == ID).ToList();
                        if (bustePaga.Count > 0)
                        {
                            for (int i = 0; i < bustePaga.Count; i++)
                            {
                                Console.WriteLine($"{i}. ID: {bustePaga[i].DipendenteId}, Data emissione: {bustePaga[i].DataEmissione}");
                            }

                            while (true)
                            {
                                Console.WriteLine("Selezionare la busta paga da eliminare:");
                                if (int.TryParse(Console.ReadLine(), out int input) && input >= 0 && input < bustePaga.Count)
                                {
                                    var bustaDaEliminare = dbContext.BustaPaga.FirstOrDefault(bp => bp.Index == input);
                                    if (bustaDaEliminare != null)
                                    {
                                        dbContext.BustaPaga.Remove(bustaDaEliminare);
                                        dbContext.SaveChanges();
                                        Console.WriteLine("Busta paga eliminata con successo.");
                                        break;
                                    }
                                    else
                                    {
                                        Console.WriteLine("Nessuna busta paga trovata all'indice specificato.");
                                    }
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("All'ID specificato non corrispondono buste paga.");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Nessun dipendente trovato per l'ID specificato.");
                }
            }
        }

        private void GetTimbrature(int id)
        {
            using (var dbContext = new MyDbContext())
            {
                if (int.TryParse(Console.ReadLine(), out int ID))
                {
                    DipendentiManager dm = new DipendentiManager();
                    var dipendente = dm.GetDipendenteById(id);
                    // Recupera le timbrature del dipendente
                    var timbrature = dbContext.Timbrature
                        .Where(t => t.DipendenteId == ID)
                        .OrderBy(t => t.Timestamp)
                        .ToList();
                    
                    // Inizializza le variabili per il calcolo delle ore
                    TimeSpan oreLavorate = TimeSpan.Zero;
                    TimeSpan oreStraordinario = TimeSpan.Zero;

                    // Itera sulle timbrature per calcolare le ore lavorate
                    for (int i = 0; i < timbrature.Count - 1; i += 2)
                    {
                        var entrata = timbrature[i];
                        var uscita = timbrature[i + 1];

                        // Calcola la durata del lavoro tra entrata e uscita
                        TimeSpan durataLavoro = uscita.Timestamp - entrata.Timestamp;

                        // Aggiorna le ore lavorate
                        oreLavorate += durataLavoro;

                        // Se le ore lavorate superano le 8 ore, calcola le ore straordinario
                        if (oreLavorate.TotalHours > 8)
                        {
                            oreStraordinario += durataLavoro;
                        }

                        // Aggiorna le buste paga con le ore lavorate e straordinario
                        dipendente.BustaPaga.OreLavorate = oreLavorate;
                        dipendente.BustaPaga.OreStraordinario = oreStraordinario;

                        dbContext.SaveChanges();

                        Console.WriteLine($"Ore lavorate: {oreLavorate.TotalHours}");
                        Console.WriteLine($"Ore straordinario: {oreStraordinario.TotalHours}");
                        Console.WriteLine("Busta paga aggiornata.");
                    }
                }
            }
        }
    
    }
}
