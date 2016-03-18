using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
