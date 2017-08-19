using System;
using System.Collections.Generic;
using static Desiderata.CombatEngine;
using static Desiderata.MainMenu;
using static Desiderata.Player;
using static Desiderata.TextEngine;
using static System.Console;
using static ConsoleExtender.ConsoleHelper;
using System.Media;
using System.Threading;

namespace Desiderata
{
    internal class Program
    {
        //TODO: flush the input buffer regularly
        public static LinkedList<Choice> Choices = new LinkedList<Choice>();
        public static List<string> Paragraph = new List<string>();

        static SoundPlayer mainMenuMusic = new SoundPlayer(Environment.CurrentDirectory + "\\Vikings.wav"); 
        private static void Main(string[] args)
        {
            Title = "Desiderata";
            
            CursorVisible = false;
            DisableQuickEdit(); 
            SetConsoleFont(16);
            SetFullScreen();
            ChangeResolution(1280, 720);
            mainMenuMusic.Play();

            //This is the entry point for the actual gameplay
            DisplayMainMenu();
            
            //Change the screen resolution back to what it started as
            ChangeResolution(retainWidth, retainHeight);
          //160, 45
          //1280, 720
        }

        static System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
        public static void Exp()
        {
            stopwatch.Start();
            BackgroundColor = ConsoleColor.DarkRed; 
            for (int i = 0; i < WindowWidth; i++)
                for(int j = 0; j < WindowHeight; j++)
                {
                    Write(' '); 
                }
            CursorLeft = 0;
            CursorTop = 0;
            BackgroundColor = ConsoleColor.Black;
            stopwatch.Stop();
        }



        public static void AManApproaches()
        {
            RefreshHUD();
          
            Paragraph.Add("A man approches you upon the road.");
            Paragraph.Add("He is haggerd and old. His skin glistens");
            Paragraph.Add("in a cold sweat and the moon shines like mercury");
            Paragraph.Add("in the reflection of his eyes.");
            DisplayParagraph();

            Paragraph.Add("The lines run deep in his face and the hair upon his head hints at wisdom untold, or is it the stresses of madness that has taken their color and left them bleached in silver?");
            Paragraph.Add("I wonder if I should talk with the man...");
            Choices.AddFirst(new Choice("Excuse me sir, but might I bare your load for a mile?", () => BareTheLoad()));
            Choices.AddAfter(Choices.Last, new Choice("Give me all you have! I know that look in one's eye and I trust not your perplextion for it speaks of inner madness untold! *you raise your fists*", () => FightTheOldMan()));
            DisplayChoices();
        }

        private static void BareTheLoad()
        {
            Paragraph.Add("You have chosen to bare the old man's load and walk with him for a mile");
            DisplayParagraph();
            {
                Mind += 15;
                Strength += 3;

                Paragraph.Add("As you walk with the old man, he tells you of stories of great battles, concepts of wealth creation, and concepts that boggle the mind.");
                Paragraph.Add("You think to yourself, “Truly this wisdom was well worth the burden of his load. I only wish we weren’t parting ways at this fork.”");
                Paragraph.Add("As the man disappears in the distance you now know what it is like to be in the presence of a truly wise man.");

            }
        }

        private static void FightTheOldMan()
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

        private static void TakeTheCoins()
        {
            Coin += 10;
        }
    }
}