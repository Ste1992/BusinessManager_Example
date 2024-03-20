using BusinessManager.Interfaces;

namespace BusinessManager.Menus
{
    public class MenuPrincipale : IMenu
    {

        public void VisualizzaMenu()
        {
            Console.WriteLine("Benvenuto in BusinessManager");
            Console.WriteLine("Menù:");
            Console.WriteLine("1. Dipendenti");
            Console.WriteLine("2. Timbrature");
            Console.WriteLine("3. Buste Paga");
        }

        public void EseguiScelta(int scelta)
        {
            switch(scelta)
            {
                case 1:
                    DipendentiMenu.Menu();
                    break;

                case 2:
                    CartellinoMenu.Menu();
                    break;

                case 3:
                    BustaPagaMenu.Menu();
                    break;

                default:
                    Console.WriteLine("Scelta errata");
                    break;
            }
        }

        public static void Menu()
        {
            MenuPrincipale menuPrincipale = new();

            bool continua = true;
            while (continua)
            {
                Console.Clear();
                menuPrincipale.VisualizzaMenu();
                Console.WriteLine("Scelta:");
                if (int.TryParse(Console.ReadLine(), out int scelta))
                {
                    menuPrincipale.EseguiScelta(scelta);
                }
                else
                {
                    Console.WriteLine("Input non valido. Inserisci un numero corrispondente all'opzione desiderata.");
                }

                Console.WriteLine("Vuoi uscire dal programma? (s/n)");
                string risposta = Console.ReadLine();
                if (risposta.ToLower() == "s")
                {
                    continua = false; // Imposta la variabile di controllo a false per uscire dal ciclo
                }
            }
        }

    }
}
