using System;

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

            // Nehézség bekérése
            do
            {
                Difficulty();
            } while (!selected);

            string[,] board = new string[boardSize, boardSize];

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
                Console.WriteLine();
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
                // Tábla kirajzolása (Zétény írta ezt a részt, én csak egy szóközzel hotfixeltem)
                {
                    Console.Write("┌");
                    for (int k = 0; k < board.GetLength(0) - 1; k++)
                    {
                        Console.Write("───┬");
                    }
                    Console.Write("───┐");
                    Console.Write($"\n");
                    // Tábla kiírása
                    for (int i = 0; i < board.GetLength(0); i++)
                    {
                        Console.Write("│");
                        for (int j = 0; j < board.GetLength(1); j++)
                        {
                            Console.Write($" {board[i, j]}");
                            Console.Write(" │");
                        }
                        //"─, │, ┌, ┐, └, ┘, ├, ┤, ┬, ┴, ┼"
                        if (i < board.GetLength(0) - 1)
                        {
                            Console.Write($"\n");
                            Console.Write("├");
                            for (int k = 1; k <= board.GetLength(0) - 1; k++)
                            {
                                Console.Write("───┼");
                            }
                            Console.Write("───┤");
                        }
                        Console.WriteLine();
                    }
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

            void Difficulty()
            // Nehézség, ez csak a tábla mérete
            {
                Console.Clear();
                Console.ResetColor();
                Console.WriteLine("Kérem válasszon táblaméretet: ");

                if (selector == 0)
                {
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                }
                Console.WriteLine("10x10");

                if (selector == 1)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                }
                Console.WriteLine("11x11");

                if (selector == 2)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                }
                Console.WriteLine("12x12");

                if (selector == 3)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                }
                Console.WriteLine("13x13");

                if (selector == 4)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                }
                Console.WriteLine("14x14");

                if (selector == 5)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                }
                Console.WriteLine("15x15");

                if (selector == 6)
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                }
                Console.WriteLine("Random (a fentebbiek közül)");

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
    }
}