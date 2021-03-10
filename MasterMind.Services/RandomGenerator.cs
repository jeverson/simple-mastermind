using System;

namespace MasterMind.Services
{
    public class RandomGenerator : IRandom
    {
        private readonly Random random;

        public RandomGenerator()
        {
            random = new Random();
        }

        public int Next(int min, int max)
        {
            return random.Next(min, max);
        }
    }
}
