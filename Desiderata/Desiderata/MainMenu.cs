using System;
using static Desiderata.Program;
using static Desiderata.TextEngine;
using static System.Console; 

namespace Desiderata
{
    public static class MainMenu
    {
        public static void DisplayMainMenu()
        {
            var title = "DESIDERATA";
            var newGame = "NEW GAME";
            var loadGame = "LOAD GAME"; 
            var options = "OPTIONS";
            var exit = "EXIT";

            centerString(ref title, WindowWidth);
            centerString(ref newGame, WindowWidth);
            centerString(ref loadGame, WindowWidth); 
            centerString(ref options, WindowWidth);
            centerString(ref exit, WindowWidth);

            Paragraph.Add(title);
            Choices.AddFirst(new Choice(newGame, AManApproaches));
            Choices.AddAfter(Choices.Last, new Choice(loadGame, () => { })); 
            Choices.AddAfter(Choices.Last, new Choice(options, OptionsMenu));
            Choices.AddAfter(Choices.Last, new Choice(exit, () => { }));
            DisplayChoices(true);
        }
        
        private static void OptionsMenu()
        {
            Paragraph.Add("Set scroll speed");
            Choices.AddFirst(new Choice("SLOW", () => scrollSpeed = ScrollingSpeed.Slow));
            Choices.AddAfter(Choices.Last, new Choice("NORMAL", () => scrollSpeed = ScrollingSpeed.Normal));
            Choices.AddAfter(Choices.Last, new Choice("FAST", () => scrollSpeed = ScrollingSpeed.Fast));
            Choices.AddAfter(Choices.Last, new Choice("INSTANT", () => scrollSpeed = ScrollingSpeed.Instant));
            DisplayChoices(true);
            DisplayMainMenu();
        }
    }
}