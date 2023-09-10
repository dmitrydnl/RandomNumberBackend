using System;

namespace RandomNumberBackend.Game
{
    public class NumberGenerator : INumberGenerator
    {
        private readonly Random random;
        private readonly int min;
        private readonly int max;

        public NumberGenerator(int min, int max)
        {
            random = new Random();
            this.min = min;
            this.max = max;
        }

        public int Generate()
        {
            return random.Next(min, max + 1);
        }
    }
}
