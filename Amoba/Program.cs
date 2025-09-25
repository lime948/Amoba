using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amoba
{
    internal class Program
    {
        static void Main(string[] args)
        {
        }

        static void GameLoop()
        {
            Console.WriteLine("=== AMŐBA ===");
            bool ujra = false;
            do
            {
                Console.WriteLine("Szeretnél újra játszani? (i/n)");
                string valasz = Console.ReadLine().ToLower();
                if (valasz == "i")
                {
                    ujra = true;
                }
                else if (valasz == "n")
                {
                    ujra = false;
                }
                else
                {
                    Console.WriteLine("Hibás válasz, kérlek írd be újra!");
                }
            } while (ujra);
        }

        static void Display()
        {
        }

        static void Winconditions()
        {
        }
    }
}
