using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Desiderata
{
    class Program
    {
        static int _coin = 0;
        static int Coin { get { return _coin; } set { _coin = value; RefreshHUD(); } }
        static int Fiatus = 100;

        static int _health = 100;
        static int Health { get { return _health; } set { _health = value; RefreshHUD(); } }

        static int Martial = 100;
        static int Mind = 100;
        static int Power = 100;

        static int _strength = 1;
        static int Strength { get { return _strength; } set { _strength = value; RefreshHUD(); } }

        static int Virtue = 100;

        static LinkedList<Choice> Choices = new LinkedList<Choice>();
        static List<string> Paragraph = new List<string>();

        class Choice
        {
            public Choice(string text, Action method)
            {
                DisplayText = text;
                Method = method;
            }
            public string DisplayText;
            public Action Method;
            public int Row;
        }


        static void Main(string[] args)
        {
            RefreshHUD();
            Console.CursorVisible = false;

            Paragraph.Add("A man approches you upon the road.");
            Paragraph.Add("He is haggerd and old. His skin glistens");
            Paragraph.Add("in a cold sweat and the moon shines like mercury in the reflection of his eyes.");
            DisplayParagraph();


            Paragraph.Add("The lines run deep in his face and the hair upon his head hints at wisdom untold, or is it the stresses of madness that has taken their color and left them bleached in silver?");
            Paragraph.Add("I wonder if I should talk with the man...");
            Choices.AddFirst(new Choice("Excuse me sir, but might I bare your load for a mile?", () => BareTheLoad()));
            Choices.AddAfter(Choices.Last, new Choice("Give me all you have! I know that look in one's eye and I trust not your perplexion for it speaks of inner maddness untold! *you raise your fists*", () => FightTheOldMan()));
            DisplayChoices();

        }

        static void BareTheLoad()
        {
            Paragraph.Add("You have chosen to bare the old man's load and walk with him for a mile");
            DisplayParagraph();
        }
        static void FightTheOldMan()
        {
            Paragraph.Add("You have chosen to bare your fists and fight the old man");
            DisplayParagraph();
            if (BattleResult(0.07))
            {
                Strength += 10;
                Paragraph.Add("You licked the man... Not literally");
                Paragraph.Add("The man lays unconsious upon the ground. His body twitches ever so slightly, but you are sure he is beaten");
                Paragraph.Add("The sachel has fallen from his back and it's contents lay naked upon the ground.");
                Paragraph.Add("A great leather book lays on the dusty trail with a bag of coins lying upon it.");

                Choices.AddFirst(new Choice("Plunder his booty... ummmm ", () => TakeTheCoins()));
                DisplayChoices();
            }
            else
            {
                Health -= 70;
                Paragraph.Add("He stuck his cane through your eye socket... Ouch!");
                DisplayParagraph();
            }

        }

        static void TakeTheCoins()
        {
            Coin += 10;
        }



        static void RefreshHUD()
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


        static void DisplayParagraph()
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
        static void DisplayChoices()
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

        static void ClearCurrentConsoleLine()
        {
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);
        }


        static bool BattleResult(double defenderStrength)
        {
            Random r = new Random();
            double randomNumber = r.NextDouble();

            if (randomNumber > defenderStrength)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}



