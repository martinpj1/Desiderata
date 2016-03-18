using System;
using System.Collections.Generic;
using System.Threading;
using static Desiderata.Program;

namespace Desiderata
{
    public static class TextEngine
    {
        public static void DisplayParagraph()
        {
            foreach (string line in Paragraph)
            {
                //Console.WriteLine(line);
                foreach (char c in line)
                {
                    Console.Write(c);
                    Thread.Sleep(25);
                }
                Console.Write("\n");
            }
            Console.Write("Press <Enter> to continue... ");
            while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }
            Console.Clear();
            Paragraph.Clear();
            RefreshHUD();
        }

        //TODO: fix multiple row choices refreshing correctly
        public static void DisplayChoices()
        {
            LinkedListNode<Choice> SelectedChoice = Choices.First;
            ConsoleKey pressedKey;

            foreach (var line in Paragraph)
                Console.WriteLine(line);
            Console.WriteLine("");

            foreach (var choice in Choices)
            {
                choice.Row = Console.CursorTop;
                Console.WriteLine(choice.DisplayText);
            }

            Console.CursorTop = SelectedChoice.Value.Row;
            Console.WriteLine("> " + SelectedChoice.Value.DisplayText);

            do
            {
                pressedKey = Console.ReadKey(true).Key;
                if (pressedKey == ConsoleKey.UpArrow && SelectedChoice != Choices.First)
                {
                    Console.CursorTop = SelectedChoice.Value.Row;
                    ClearCurrentConsoleLine();
                    Console.WriteLine(SelectedChoice.Value.DisplayText);
                    SelectedChoice = SelectedChoice.Previous;
                }
                else if (pressedKey == ConsoleKey.DownArrow && SelectedChoice != Choices.Last)
                {
                    Console.CursorTop = SelectedChoice.Value.Row;
                    ClearCurrentConsoleLine();
                    Console.WriteLine(SelectedChoice.Value.DisplayText);
                    SelectedChoice = SelectedChoice.Next;
                }

                Console.CursorTop = SelectedChoice.Value.Row;
                Console.WriteLine("> " + SelectedChoice.Value.DisplayText);

            } while (pressedKey != ConsoleKey.Enter);

            Paragraph.Clear();
            Choices.Clear();
            Console.Clear();
            RefreshHUD();
            SelectedChoice.Value.Method.Invoke();
        }

        public static void RefreshHUD()
        {
            int currentLineCursor = Console.CursorTop;
            ConsoleColor prevForegroundColor = Console.ForegroundColor;
            ConsoleColor prevBackgroundColor = Console.BackgroundColor;

            Console.SetCursorPosition(0, 20);
            ClearCurrentConsoleLine();
            Console.SetCursorPosition(0, 21);
            ClearCurrentConsoleLine();

            //HEALTH (5-24,20)
            var healthBars = Health / 5;
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.SetCursorPosition(5, 20);
            Console.WriteLine(new string(' ', healthBars));

            //STRENGTH (26,20) && (22-29,21)
            Console.BackgroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(26, 20);
            Console.WriteLine(Strength.ToString());
            Console.SetCursorPosition(22, 21);
            Console.WriteLine("STRENGTH");

            //COIN (31,20) && (30-33,21)
            Console.BackgroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(31, 20);
            Console.WriteLine(Coin.ToString());
            Console.SetCursorPosition(30, 21);
            Console.WriteLine("COIN");

            Console.CursorTop = currentLineCursor;
            Console.ForegroundColor = prevForegroundColor;
            Console.BackgroundColor = prevBackgroundColor;
        }

        public static void ClearCurrentConsoleLine()
        {
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);
        }
    }
}
