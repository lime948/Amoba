using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Amoba
{
    internal class Program
    {
        static void Main(string[] _)
        {
            // Változók deklarálása
            Console.Title = "Amőba";
            string[,] board = new string[10, 10];
            byte row = 0;
            byte col = 0;
            byte turn = 0;
            string empty = "e";
            bool success = false;

            // Tábla feltöltése, ez csak a legelején fut le egyszer
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    board[i, j] = empty;
                }
            }

            // Fő játékciklus
            do
            {
                GameLoop();
            }
            while (FreeSpace() && NoWinner());

            if (!FreeSpace())
            {
                Console.WriteLine("Nincs több üres mező! Döntetlen!");
                // Enterrel restart
            }

            void GameLoop()
            {
                DrawBoard();

                // Játékosváltás
                char player = (turn % 2 == 0 ? 'X' : 'O');
                Console.WriteLine($"Következő játékos: {player}");

                // Pozíciók bekérése, Try-Catch hogy ne crasheljen a program
                do
                {
                    Console.Write($"Add meg a sor számát (1-{board.GetLength(0)}): ");
                    try
                    {
                        row = (Byte)(Convert.ToInt32(Console.ReadLine()) - 1);
                        success = Byte.TryParse(row.ToString(), out row);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("A koordináta csak szám lehet!");
                    }
                }
                while (!success);

                do
                {
                    Console.Write($"Add meg az oszlop számát (1-{board.GetLength(1)}): ");
                    try
                    {
                        col = (Byte)(Convert.ToInt32(Console.ReadLine()) - 1);
                        success = Byte.TryParse(col.ToString(), out col);

                    }
                    catch (Exception)
                    {
                        Console.WriteLine("A koordináta csak szám lehet!");
                    }
                }
                while (!success);

                // Koordináták ellenőrzése
                if (row < 0 || row > board.GetLength(0) - 1 || col < 0 || col > board.GetLength(1) - 1)
                {
                    Console.Clear();
                    Console.WriteLine("Hibás koordináta! Próbáld újra.");
                }
                else if (board[row, col] != empty)
                {
                    Console.Clear();
                    Console.WriteLine("Ez a mező már foglalt! Próbáld újra.");
                }
                else
                {
                    board[row, col] = $"{player}";
                    if (NoWinner())
                    {
                        turn++;
                        Console.Clear();
                    }
                    else
                    {
                        Console.Clear();
                        DrawBoard();
                        Console.WriteLine($"Gratulálok {player}, nyertél!");
                        // Enterrel restart
                    }
                }

                void DrawBoard()
                {
                    // Tábla kiírása
                    for (int i = 0; i < board.GetLength(0); i++)
                    {
                        for (int j = 0; j < board.GetLength(1); j++)
                        {
                            Console.Write($"{board[i, j]} ");
                        }
                        Console.WriteLine();
                    }
                }
            }

            bool FreeSpace()
            {
                // Üres helyek ellenőrzése
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

            bool NoWinner()
            // Ellenőrzi, hogy van-e nyertes
            {
                for (int i = 0; i < board.GetLength(0); i++)
                {
                    for (int j = 0; j < board.GetLength(1); j++)
                    {
                        // Vízszintes ellenőrzés
                        if (j + 4 < board.GetLength(1) &&
                            board[i, j] != empty &&
                            board[i, j] == board[i, j + 1] &&
                            board[i, j] == board[i, j + 2] &&
                            board[i, j] == board[i, j + 3] &&
                            board[i, j] == board[i, j + 4])
                        {
                            return false;
                        }

                        // Függőleges ellenőrzés
                        if (i + 4 < board.GetLength(0) &&
                            board[i, j] != empty &&
                            board[i, j] == board[i + 1, j] &&
                            board[i, j] == board[i + 2, j] &&
                            board[i, j] == board[i + 3, j] &&
                            board[i, j] == board[i + 4, j])
                        {
                            return false;
                        }

                        // Átlós ellenőrzés (\) 
                        if (i + 4 < board.GetLength(0) && j + 4 < board.GetLength(1) &&
                            board[i, j] != empty &&
                            board[i, j] == board[i + 1, j + 1] &&
                            board[i, j] == board[i + 2, j + 2] &&
                            board[i, j] == board[i + 3, j + 3] &&
                            board[i, j] == board[i + 4, j + 4])
                        {
                            return false;
                        }

                        // Átlós ellenőrzés (/)
                        if (i - 4 >= 0 && j + 4 < board.GetLength(1) &&
                            board[i, j] != empty &&
                            board[i, j] == board[i - 1, j + 1] &&
                            board[i, j] == board[i - 2, j + 2] &&
                            board[i, j] == board[i - 3, j + 3] &&
                            board[i, j] == board[i - 4, j + 4])
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
        }
    }
}