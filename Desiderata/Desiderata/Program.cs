using System;
using System.Collections.Generic;
using static Desiderata.TextEngine; 

namespace Desiderata
{
    class Program
    {
        static int _coin = 0;
        public static int Coin { get { return _coin; } set { _coin = value; RefreshHUD(); } }

        static int _fiatus = 20;
        public static int Fiatus { get { return _fiatus; } set { _fiatus = value; RefreshHUD(); } }

        static int _health = 100;
        public static int Health { get { return _health; } set { _health = value; RefreshHUD(); } }

        static int _martial = 50;
        public static int Martial { get { return _martial; } set { _martial = value; RefreshHUD(); } }

        static int _mind = 100;
        public static int Mind { get { return _mind; } set { _mind = value; RefreshHUD(); } }

        static int _power = 100;
        public static int Power { get { return _power; } set { _power = value; RefreshHUD(); } }

        static int _strength = 1;
        public static int Strength { get { return _strength; } set { _strength = value; RefreshHUD(); } }

        static int _virtue = 1;
        public static int Virtue { get { return _virtue; } set { _virtue = value; RefreshHUD(); } }

        public static LinkedList<Choice> Choices = new LinkedList<Choice>();
        public static List<string> Paragraph = new List<string>();
        
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



