using BusinessManager.Interfaces;
using BusinessManager.Managers;

namespace BusinessManager.Menus
{
    public class AnagraficaMenu : IMenu
    {

        public void VisualizzaMenu()
        {
            Console.WriteLine("0. Menù Principale");
            Console.WriteLine("1. Aggiungi dipendente");
            Console.WriteLine("2. Modifica dipendente");
            Console.WriteLine("3. Elimina dipendente");
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
                    dm.AddDipendente();
                    break;

                case 2:
                    dm.EditDipendente();
                    break;

                case 3:
                    dm.DeleteDipendente();
                    break;
            }
        }

        public static void Menu()
        {
            AnagraficaMenu anagraficaMenu = new();

            bool continua = true;
            while (continua)
            {
                Console.Clear();
                anagraficaMenu.VisualizzaMenu();
                Console.WriteLine("Scelta:");
                if (int.TryParse(Console.ReadLine(), out int scelta))
                {
                    anagraficaMenu.EseguiScelta(scelta);
                }
                else
                {
                    Console.WriteLine("Input non valido. Inserire un numero corrispondente all'opzione desiderata.");
                }

                Console.WriteLine("Premi un tasto per continuare...");
                Console.ReadKey();
            }
        }

    }
}
