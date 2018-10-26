using System;

namespace Äventyrspel_v2
{

    class Program
    {

        //The main entry point
        static void Main(string[] args)
        {

            //Sets the colors of the console
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;

            //Sets the size of the console
            Console.WindowWidth = 95;
            Console.WindowHeight = 20;

            Console.Clear();

            //Create the new Game object
            Game Game = new Game();

            //Starts the game
            Game.StartGame();
        }

    }

    class Print
    {

        public static void PrintColorText(string text, ConsoleColor color)
        {

            Console.ForegroundColor = color;
            Console.Write(text);

            Console.ForegroundColor = ConsoleColor.Black;

        }

        public static void PrintMiddle(string text, bool line, int offsetY, int offsetX)
        {

            Console.SetCursorPosition((Console.WindowWidth - text.Length - offsetX) / 2, Console.CursorTop + offsetY);

            if (line)
            {
                Console.WriteLine(text);
            }
            else
            {
                Console.Write(text);
            }
        }

    }

}