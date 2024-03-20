using BusinessManager.Interfaces;
using BusinessManager.Managers;
using BusinessManager.Services;

namespace BusinessManager.Menus
{
    public class CartellinoMenu : IMenu
    {

        public void VisualizzaMenu()
        {
            Console.WriteLine("0. Menù Principale");
            Console.WriteLine("1. Registra Entrata");
            Console.WriteLine("2. Registra Uscita");
            Console.WriteLine("3. Storico Cartellino");
            Console.WriteLine("4. Modifica Timbratura");
            Console.WriteLine("5. Cancella Timbratura");
        }

        public void EseguiScelta(int scelta)
        {
            DipendentiManager dm = new();
            TimbraturaManager tm = new();

            string? id;
            switch(scelta)
            {
                case 0:
                    MenuPrincipale.Menu();
                    break;

                case 1: // ENTRATA
                    Console.WriteLine("Inserisci ID: ");
                    id = Console.ReadLine();

                    if (int.TryParse(id, out int x))
                    {
                        dm.RegistraEntrata(x);
                    }
                    break;

                case 2: // USCITA
                    Console.WriteLine("Inserisci ID: ");
                    id = Console.ReadLine();

                    if (int.TryParse(id, out int y))
                    {
                        dm.RegistraUscita(y);
                    }
                    break;

                case 3: // STORICO
                    using (var dbContext = new MyDbContext())
                    {
                        var storicoPresenze = dm.StoricoTimbratureDipendente();
                        foreach (var presenze in storicoPresenze)
                        {
                            Console.WriteLine($"{presenze.DipendenteId} {presenze.Timestamp} {presenze.TipoPresenza}");
                        }
                        break;
                    }

                case 4: // MODIFICA
                    tm.EditTimbratura();
                    break;

                case 5: // CANCELLA
                    tm.DeleteTimbratura();
                    break;

                default:
                    Console.WriteLine("Scelta errata");
                    break;
            }
        }

        public static void Menu()
        {
            CartellinoMenu cartellinoMenu = new();

            bool continua = true;
            while (continua)
            {
                Console.Clear();
                cartellinoMenu.VisualizzaMenu();
                Console.WriteLine("Scelta:");
                if (int.TryParse(Console.ReadLine(), out int scelta))
                {
                    cartellinoMenu.EseguiScelta(scelta);
                }
                else
                {
                    Console.WriteLine("Input non valido. Inserisci un numero corrispondente all'opzione desiderata.");
                }

                Console.WriteLine("Premi un tasto per continuare...");
                Console.ReadKey();
            }
        }

    }
}
