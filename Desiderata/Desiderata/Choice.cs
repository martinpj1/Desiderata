using System;

namespace Desiderata
{
    public class Choice
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
}