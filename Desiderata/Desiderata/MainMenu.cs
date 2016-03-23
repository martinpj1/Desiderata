using System;
using static Desiderata.Program;
using static Desiderata.TextEngine;

namespace Desiderata
{
    public static class MainMenu
    {
        private static int mainMenuWidth = 50;

        public static void DisplayMainMenu()
        {
            Console.CursorVisible = false;
            var title = "DESIDERATA";
            var newGame = "NEW GAME";
            var options = "OPTIONS";

            centerString(ref title, mainMenuWidth);
            centerString(ref newGame, mainMenuWidth);
            centerString(ref options, mainMenuWidth);

            Paragraph.Add(title);
            Choices.AddFirst(new Choice(newGame, () => AManApproaches()));
            Choices.AddAfter(Choices.Last, new Choice(options, () => OptionsMenu()));
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