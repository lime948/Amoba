using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amoba
{
    internal class Program
    {
        static int games = 0;
        static string[,] board = new string[3, 3];
        static void Main(string[] args)
        {
            Console.WriteLine("=== AMŐBA ===");

            byte turn = 0;
            string empty = "e";

            Display();
            Winconditions();
            if (games >= 1)
                Replay();

            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    board[i, j] = empty;
                }
            }
            do
            {
                GameLoop();
            } while (FreeSpace());

            if (!FreeSpace())
            {
                Console.WriteLine("Nincs több üres mező! Döntetlen!");
            }
            void GameLoop()
            {
                for (int i = 0; i < board.GetLength(0); i++)
                {
                    for (int j = 0; j < board.GetLength(1); j++)
                    {
                        Console.Write($"{board[i, j]} ");
                    }
                    Console.WriteLine();
                }

                char player = (turn % 2 == 0 ? 'X' : 'O');
                Console.WriteLine($"Következő játékos: {player}");

                Console.Write("Add meg a sor számát (1-10): ");
                byte row = (Byte)(Convert.ToInt32(Console.ReadLine()) - 1);
                Console.Write("Add meg az oszlop számát (1-10): ");
                byte col = (Byte)(Convert.ToInt32(Console.ReadLine()) - 1);

                if (row < 0 || row > board.GetLength(0) - 1 || col < 0 || col > board.GetLength(1) - 1)
                {
                    Console.Clear();
                    Console.WriteLine("Hibás koordináta! Próbáld újra.");
                    GameLoop();
                }
                else if (board[row, col] != empty)
                {
                    Console.Clear();
                    Console.WriteLine("Ez a mező már foglalt! Próbáld újra.");
                    GameLoop();
                }
                else
                {
                    board[row, col] = $"{player} ";
                    turn++;
                    Console.Clear();
                    GameLoop();
                }
                
            }

            bool FreeSpace()
            {
                for (int i = 0; i < board.GetLength(0); i++)
                {
                    for (int j = 0; j < board.GetLength(1); j++)
                    {
                        if (board[i, j] == empty)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
            bool Replay()
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
            void Display()
            {

            }

            void Winconditions()
            {
               
                //nyerés és vsztés után games++;
            }
        }
    }
}