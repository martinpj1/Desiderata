using System.Collections.Generic;
using static Desiderata.CombatEngine;
using static Desiderata.MainMenu;
using static Desiderata.Player;
using static Desiderata.TextEngine;

namespace Desiderata
{
    class Program
    {
        public static LinkedList<Choice> Choices = new LinkedList<Choice>();
        public static List<string> Paragraph = new List<string>();
        
        static void Main(string[] args)
        {
            DisplayMainMenu();
        }

        public static void AManApproaches()
        {
            RefreshHUD();

            Paragraph.Add("A man approches you upon the road.");
            Paragraph.Add("He is haggerd and old. His skin glistens");
            Paragraph.Add("in a cold sweat and the moon shines like mercury in the reflection of his eyes.");
            DisplayParagraph();


            Paragraph.Add("The lines run deep in his face and the hair upon his head hints at wisdom untold, or is it the stresses of madness that has taken their color and left them bleached in silver?");
            Paragraph.Add("I wonder if I should talk with the man...");
            Choices.AddFirst(new Choice("Excuse me sir, but might I bare your load for a mile?", () => BareTheLoad()));
            Choices.AddAfter(Choices.Last, new Choice("Give me all you have! I know that look in one's eye and I trust not your perplextion for it speaks of inner madness untold! *you raise your fists*", () => FightTheOldMan()));
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
                Paragraph.Add("The man lays unconscious upon the ground. His body twitches ever so slightly, but you are sure he is beaten");
                Paragraph.Add("The satchel has fallen from his back and its contents lay naked upon the ground.");
                Paragraph.Add("A great leather book lays on the dusty trail with a bag of coins lying upon it.");

                Choices.AddFirst(new Choice("Take his coins ", () => TakeTheCoins()));
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

    }
}



