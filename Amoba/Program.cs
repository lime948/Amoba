using System;
using System.Collections.Generic;

namespace Amoba
{
    internal class Program
    {
        static void Main(string[] _)
        {
            // Változók deklarálása
            Console.Title = "Amőba";
            byte boardSize = 0;
            byte row = 0;
            byte col = 0;
            byte turn = 0;
            string empty = " ";
            bool success = false;
            byte selector = 0;
            bool selected = false;
            string[,] board;

            // Nehézség bekérése
            do
            {
                Difficulty();
            } while (!selected);


            FillBoard();

            // Fő játékciklus
            do
            {
                GameLoop();
            }
            while (FreeSpace() && NoWinner());

            if (!FreeSpace())
            {
                WriteCentered("Nincs több üres mező! Döntetlen!");
                Replay();
            }

            void Replay()
            {
                string input;
                WriteCentered("Szeretnéd újra játszani? (I/N)");
                do
                {
                    Console.CursorLeft = (Console.WindowWidth / 2);
                    input = Console.ReadLine().ToUpper();
                    if (input == "I")
                    {
                        turn = 0;
                        selector = 0;
                        selected = false;
                        Console.Clear();
                        do
                        {
                            Difficulty();
                        } while (!selected);
                        FillBoard();
                        GameLoop();
                    }
                    else if (input == "N")
                    {
                        Environment.Exit(0);
                    }
                    else
                    {
                        WriteCentered("Hibás válasz! Kérlek, válaszolj I vagy N betűvel.");
                    }
                } while (input != "I"); // Ha Input = N akkor amúgyis kilép
            }

            void FillBoard()
            // Tábla feltöltése
            {
                board = new string[boardSize, boardSize];
                for (int i = 0; i < board.GetLength(0); i++)
                {
                    for (int j = 0; j < board.GetLength(1); j++)
                    {
                        board[i, j] = empty;
                    }
                }
            }

            void GameLoop()
            {
                DrawBoard();

                // Játékosváltás
                Console.WriteLine();
                char player = (turn % 2 == 0 ? 'X' : 'O');
                WriteCentered($"Következő játékos: {player}");

                // Pozíciók bekérése, Try-Catch hogy ne crasheljen a program
                do
                {
                    WriteCentered($"Add meg a sor számát (1-{board.GetLength(0)}): ");
                    Console.CursorLeft = (Console.WindowWidth / 2);
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
                    Console.CursorLeft = (Console.WindowWidth / 2);
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
                        Console.WriteLine();
                        WriteCentered($"Gratulálok {player}, nyertél!");
                        Replay();
                    }
                }

                void DrawBoard()
                // Tábla kirajzolása (Zétény írta ezt a részt, én csak egy szóközt mozgattam)
                {
                    Console.Clear();
                    var lines = new List<string>();

                    // Top border
                    var top = "┌";
                    for (int k = 0; k < board.GetLength(0) - 1; k++)
                        top += "───┬";
                    top += "───┐";
                    lines.Add(top);

                    // Board rows
                    for (int i = 0; i < board.GetLength(0); i++)
                    {
                        var rows = "│";
                        for (int j = 0; j < board.GetLength(1); j++)
                        {
                            rows += $" {board[i, j]} │";
                        }
                        lines.Add(rows);

                        if (i < board.GetLength(0) - 1)
                        {
                            var mid = "├";
                            for (int k = 1; k <= board.GetLength(0) - 1; k++)
                                mid += "───┼";
                            mid += "───┤";
                            lines.Add(mid);
                        }
                    }

                    // Bottom border
                    var bottom = "└";
                    for (int k = 0; k < board.GetLength(0) - 1; k++)
                        bottom += "───┴";
                    bottom += "───┘";
                    lines.Add(bottom);

                    // Print each line centered
                    foreach (var line in lines)
                        WriteCentered(line);
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

            void Difficulty()
            // Nehézség, ez csak a tábla mérete
            {
                Console.Clear();
                Console.ResetColor();
                WriteCentered("Kérem válasszon táblaméretet: ");

                if (selector == 0)
                {
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                }
                WriteCentered("10x10");

                if (selector == 1)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                }
                WriteCentered("11x11");

                if (selector == 2)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                }
                WriteCentered("12x12");

                if (selector == 3)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                }
                WriteCentered("13x13");

                if (selector == 4)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                }
                WriteCentered("14x14");

                if (selector == 5)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                }
                WriteCentered("15x15");

                if (selector == 6)
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                }
                WriteCentered("Random (a fentebbiek közül)");
                Console.CursorLeft = (Console.WindowWidth / 2);

                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.Enter:
                        selected = true;
                        break;
                    case ConsoleKey.UpArrow:
                        if (selector > 0)
                        {
                            selector -= 1;
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        if (selector < 6)
                        {
                            selector += 1;
                        }
                        break;
                }

                switch (selector)
                {
                    case 0:
                        boardSize = 10;
                        break;
                    case 1:
                        boardSize = 11;
                        break;
                    case 2:
                        boardSize = 12;
                        break;
                    case 3:
                        boardSize = 13;
                        break;
                    case 4:
                        boardSize = 14;
                        break;
                    case 5:
                        boardSize = 15;
                        break;
                    case 6:
                        Random randomSize = new Random();
                        boardSize = (byte)randomSize.Next(10, 16);
                        Console.ResetColor();
                        break;
                }
            }
        }

        static void WriteCentered(string text)
        // Középre írás
        {
            int width = Console.WindowWidth;
            int leftPadding = (width - text.Length) / 2;
            if (leftPadding < 0) leftPadding = 0;
            Console.WriteLine(new string(' ', leftPadding) + text);
        }
    }
}