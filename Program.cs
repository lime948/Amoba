using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amoba
{
    internal class Program
    {
        static int games = 0;
        static void Main(string[] args)
        {
            
            Console.WriteLine("=== AMŐBA ===");
            GameLoop();
            Display();
            Winconditions();
            if (games >= 1)
                Replay();
            
        }
        static bool Replay()
        {
            bool ujra = false;
            do
            {
                Console.WriteLine("Szeretnél újra játszani? (i/n)");
                string valasz = Console.ReadLine().ToLower();
                if (valasz == "i")
                {
                    Console.WriteLine("Új játék indul...");
                    ujra = true;
                    GameLoop();
                    Display();
                    Winconditions();
                    break;
                }
                else if (valasz == "n")
                {
                    ujra = false;
                    Console.WriteLine("Köszönöm, hogy játszottál!");
                    break;
                }
                else
                {
                    Console.WriteLine("Hibás válasz, kérlek írd be újra!");
                }
            } while (ujra);
            return ujra;
        }

        static void GameLoop()
        {
           
        }

        static void Display()
        {

        }

        static void Winconditions()
        {
            //nyerés és vsztés után games += 1;
        }
    }
}
