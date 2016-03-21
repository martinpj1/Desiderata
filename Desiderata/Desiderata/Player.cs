using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Desiderata.TextEngine;

namespace Desiderata
{
    public static class Player
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
    }
}
