using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace taleworlds_minigame{
    internal class RandomTW {
        private static Random _random = new Random();
        public static int Next(int min, int max) {
            return _random.Next(min, max + 1);
        }

        public static double NextDouble() {
            return _random.NextDouble();
        }
}
}
