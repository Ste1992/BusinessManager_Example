using BusinessManager.Interfaces;
using BusinessManager.Managers;

namespace BusinessManager.Menus
{
    public class BustaPagaMenu : IMenu
    {

        public void VisualizzaMenu()
        {
            Console.WriteLine("0. Menù Principale");
            Console.WriteLine("1. Storico Buste Paga Dipendente");
            Console.WriteLine("2. Genera Busta Paga");
            Console.WriteLine("3. Modifica Busta Paga");
            Console.WriteLine("4. Elimina Busta Paga");
        }

        public void EseguiScelta(int scelta)
        {
            BustaPagaManager bp = new();

            switch(scelta)
            {
                case 1:
                    bp.ListaBustePaga();
                    break;

                case 2:
                    bp.GeneraBustaPaga();
                    break;

                case 3:
                    bp.EditBustaPaga();
                    break;

                case 4:
                    bp.EliminaBustaPaga();
                    break;

                default:
                    Console.WriteLine("Scelta errata");
                    break;
            }
        }

        public static void Menu()
        {
            BustaPagaMenu bustaPagaMenu = new();

            bool continua = true;
            while (continua)
            {
                Console.Clear();
                bustaPagaMenu.VisualizzaMenu();
                Console.WriteLine("Scelta:");
                if (int.TryParse(Console.ReadLine(), out int scelta))
                {
                    bustaPagaMenu.EseguiScelta(scelta);
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