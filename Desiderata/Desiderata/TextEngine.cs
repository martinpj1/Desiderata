using System;
using System.Collections.Generic;
using System.Threading;
using static Desiderata.Program;
using static Desiderata.Player;
using static Desiderata.Menu;

namespace Desiderata
{
    //TODO: fix multiple row choices refreshing correctly
    //TODO: find a more elegant way of handling the HUD other than overloading the Display methods
    public static class TextEngine
    {
        static int sleepTime = 25;
        static ScrollingSpeed _scrollSpeed = ScrollingSpeed.Normal;
        public static ScrollingSpeed scrollSpeed {
            get { return _scrollSpeed; }
            set {
                _scrollSpeed = value;
                switch (_scrollSpeed)
                {
                    case ScrollingSpeed.Slow:
                        sleepTime = 50; 
                        break;
                    case ScrollingSpeed.Normal:
                        sleepTime = 25; 
                        break;
                    case ScrollingSpeed.Fast:
                        sleepTime = 15;
                        break;
                    case ScrollingSpeed.Instant:
                        sleepTime = 0;
                        break;
                }

            } }

        public static void DisplayParagraph()
        {
            ConsoleKey pressedKey;

            foreach (string line in Paragraph)
            {
                //Console.WriteLine(line);
                foreach (char c in line)
                {
                    Console.Write(c);
                    Thread.Sleep(sleepTime);
                }
                Console.Write("\n");
            }
            Console.Write("\n");
            Console.WriteLine("Press <Enter> to continue... ");
            Console.WriteLine("I C T");

            while (Console.KeyAvailable) { Console.ReadKey(true); }

            do
            {
                pressedKey = Console.ReadKey(true).Key;
                if (pressedKey == ConsoleKey.I)
                {
                    ShowInventory();
                }
                else if (pressedKey == ConsoleKey.C)
                {
                    ShowCharacterStats();
                }
                else if (pressedKey == ConsoleKey.T)
                {
                    ShowTranscript();
                }
            } while (pressedKey != ConsoleKey.Enter);

            Console.Clear();
            Paragraph.Clear();
            RefreshHUD();
        }
        public static void DisplayParagraph(bool suppressHUD)
        {
            ConsoleKey pressedKey;

            foreach (string line in Paragraph)
            {
                //Console.WriteLine(line);
                foreach (char c in line)
                {
                    Console.Write(c);
                    Thread.Sleep(sleepTime);
                }
                Console.Write("\n");
            }
            Console.Write("\n");
            Console.WriteLine("Press <Enter> to continue... ");
            Console.WriteLine("I C T");

            while (Console.KeyAvailable) { Console.ReadKey(true); }

            do
            {
                pressedKey = Console.ReadKey(true).Key;
                if (pressedKey == ConsoleKey.I)
                {
                    ShowInventory();
                }
                else if (pressedKey == ConsoleKey.C)
                {
                    ShowCharacterStats();
                }
                else if (pressedKey == ConsoleKey.T)
                {
                    ShowTranscript();
                }
            } while (pressedKey != ConsoleKey.Enter);

            Console.Clear();
            Paragraph.Clear();
            if(!suppressHUD)
                RefreshHUD();
        }


        public static void DisplayChoices()
        {
            LinkedListNode<Choice> SelectedChoice = Choices.First;
            ConsoleKey pressedKey;

            foreach (string line in Paragraph)
            {
                //Console.WriteLine(line);
                foreach (char c in line)
                {
                    Console.Write(c);
                    Thread.Sleep(sleepTime);
                }
                Console.Write("\n");
            }
            Console.Write("\n");

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
        public static void DisplayChoices(bool suppressHUD)
        {
            LinkedListNode<Choice> SelectedChoice = Choices.First;
            ConsoleKey pressedKey;

            foreach (string line in Paragraph)
            {
                //Console.WriteLine(line);
                foreach (char c in line)
                {
                    Console.Write(c);
                    Thread.Sleep(sleepTime);
                }
                Console.Write("\n");
            }
            Console.Write("\n");

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
            if(!suppressHUD)
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
            var healthBar = $"{Health}/100";
            centerString(ref healthBar, 20);

            Console.SetCursorPosition(5, 20);
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            for (int i = 0; i < Health / 5; i++)
            {
                Console.Write(healthBar[i]);
            }
            Console.BackgroundColor = ConsoleColor.Black;
            for (int i = Health / 5; i < 20; i++)
            {
                Console.Write(healthBar[i]);
            }

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
        public static void centerString(ref string stringToCenter, int totalWidth)
        {
            stringToCenter = new string(' ', spacesToPrefix(stringToCenter.Length, totalWidth)) + stringToCenter + new string(' ', spacesToAppend(stringToCenter.Length, totalWidth));
        }
        public static int spacesToPrefix(int stringLength, int totalWidth)
        {
            int extraSpace = (totalWidth - stringLength);
            return extraSpace / 2;
        }
        public static int spacesToAppend(int stringLength, int totalWidth)
        {
            int extraSpace = (totalWidth - stringLength);
            if (extraSpace % 2 == 0)
                return extraSpace / 2;
            else
                return extraSpace / 2 + 1;
        }
    }

    public enum ScrollingSpeed
    {
        Slow,
        Normal,
        Fast,
        Instant
    }

}
