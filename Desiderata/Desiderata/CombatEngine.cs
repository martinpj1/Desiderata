using System;
using System.Collections.Generic;
using System.Threading;
using static Desiderata.Program;

namespace Desiderata
{
    public static class CombatEngine
    {
        public static bool BattleResult(double defenderStrength)
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