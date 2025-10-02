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
            string empty = " ";
            bool success = false;
            WriteCentered("=== Amőba ===");

            // Tábla feltöltése, ez csak a legelején fut le egyszer
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    board[i, j] = empty;
                }
            }

            void Replay()
            {
                WriteCentered("Szeretnéd újra játszani? (I/N)");
                string input = Console.ReadLine().ToUpper();
                if (input == "I")
                {
                    Console.Clear();
                    // Tábla újrafeltöltése
                    for (int i = 0; i < board.GetLength(0); i++)
                    {
                        for (int j = 0; j < board.GetLength(1); j++)
                        {
                            board[i, j] = empty;
                        }
                    }
                    turn = 0;
                    GameLoop();
                }
                else if (input == "N")
                {
                    Environment.Exit(0);
                }
                else
                {
                    WriteCentered("Hibás válasz! Kérlek, válaszolj I vagy N betűvel.");
                    Replay();
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
                WriteCentered("Nincs több üres mező! Döntetlen!");
                // Enterrel restart
            }

            void GameLoop()
            {
                DrawBoard();

                // Játékosváltás
                char player = (turn % 2 == 0 ? 'X' : 'O');
                Console.Write("\n");
                WriteCentered($"Következő játékos: {player}");

                // Pozíciók bekérése, Try-Catch hogy ne crasheljen a program
                do
                {
                    WriteCentered($"Add meg a sor számát (1-{board.GetLength(0)}): ");
                    try
                    {
                        row = (Byte)(Convert.ToInt32(Console.ReadLine()) - 1);
                        success = Byte.TryParse(row.ToString(), out row);
                    }
                    catch (Exception)
                    {
                        WriteCentered("A koordináta csak szám lehet!");
                    }
                }
                while (!success);

                do
                {
                    WriteCentered($"Add meg az oszlop számát (1-{board.GetLength(1)}): ");
                    try
                    {
                        col = (Byte)(Convert.ToInt32(Console.ReadLine()) - 1);
                        success = Byte.TryParse(col.ToString(), out col);

                    }
                    catch (Exception)
                    {
                        WriteCentered("A koordináta csak szám lehet!");
                    }
                }
                while (!success);

                // Koordináták ellenőrzése
                if (row < 0 || row > board.GetLength(0) - 1 || col < 0 || col > board.GetLength(1) - 1)
                {
                    Console.Clear();
                    WriteCentered("Hibás koordináta! Próbáld újra.");
                }
                else if (board[row, col] != empty)
                {
                    Console.Clear();
                    WriteCentered("Ez a mező már foglalt! Próbáld újra.");
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
                        WriteCentered($"Gratulálok {player}, nyertél!");
                        Replay();
                    }
                }

                void DrawBoard()
                {
                    Console.Write("┌");
                    for (int k = 0; k < board.GetLength(0) - 1; k++)
                    {
                        Console.Write("───┬");
                    }
                    Console.Write("───┐");
                    Console.Write($"\n");
                    // Tábla kiírása
                    for (int i = 1; i < board.GetLength(0); i++)
                    {
                        Console.Write("│");
                        for (int j = 0; j < board.GetLength(1); j++)
                        {
                            Console.Write($"{board[i, j]}");
                            Console.Write("  │");
                        }
                        //"─, │, ┌, ┐, └, ┘, ├, ┤, ┬, ┴, ┼"
                        Console.Write($"\n");
                        Console.Write("├");
                        for (int k = 1; k <= board.GetLength(0) - 1; k++)
                        {
                            Console.Write("───┼");
                        }
                        Console.Write("───┤");
                        Console.WriteLine();
                    }
                    Console.Write("│");
                    for (int k = 0; k <= board.GetLength(1) - 1; k++)
                    {
                        Console.Write("   │");
                    }
                    Console.Write("\n");
                    Console.Write("└");
                    for (int k = 0; k < board.GetLength(0) - 1; k++)
                    {
                        Console.Write("───┴");
                    }
                    Console.Write("───┘");
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
        static void WriteCentered(string text)
        {
            int width = Console.WindowWidth;
            int leftPadding = (width - text.Length) / 2;
            if (leftPadding < 0) leftPadding = 0;
            Console.WriteLine(new string(' ', leftPadding) + text);
        }

    }
}
