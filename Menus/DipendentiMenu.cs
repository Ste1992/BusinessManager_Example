using BusinessManager.Interfaces;
using BusinessManager.Managers;

namespace BusinessManager.Menus
{
    public class DipendentiMenu : IMenu
    {

        public void VisualizzaMenu()
        {
            Console.WriteLine("0. Menù Principale");
            Console.WriteLine("1. Lista dipendenti");
            Console.WriteLine("2. Gestione Anagrafica");
            Console.WriteLine("3. Gestione Cartellini");
        }

        public void EseguiScelta(int scelta)
        {
            DipendentiManager dm = new();

            switch(scelta)
            {
                case 0:
                    MenuPrincipale.Menu();
                    break;

                case 1:
                    var dipendenti = dm.ListaDipendenti();
                    foreach (var dipendente in dipendenti)
                    {
                        Console.WriteLine($"{dipendente.ID} {dipendente.Nome} {dipendente.Cognome} {dipendente.Login}");
                    }
                    break;

                case 2:
                    AnagraficaMenu.Menu();
                    break;

                case 3:
                    CartellinoMenu.Menu();
                    break;
                
                default:
                    Console.WriteLine("Scelta non valida");
                    break;
            }
        }

        public static void Menu()
        {
            DipendentiMenu dipendentiMenu = new();

            bool continua = true;
            while (continua)                    
            {
                Console.Clear();
                dipendentiMenu.VisualizzaMenu();
                Console.WriteLine("Scelta:");
                if (int.TryParse(Console.ReadLine(), out int scelta))
                {
                    dipendentiMenu.EseguiScelta(scelta);
                }
                else
                {
                    Console.WriteLine("Input non valido. Inserire un valore corrispondente all'opzione desiderata.");
                }

                Console.WriteLine("Premi un tasto per continuare...");
                Console.ReadKey();
            }
        }

    }
}
