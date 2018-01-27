using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rotation.Common
{
    public readonly struct Time
    {
        private static readonly Random rand = new Random();

        public int Value { get; }

        public Time(int value)
        {
            this.Value = value;
        }

        public Time(int min, double sec)
        {
            this.Value = min * 600 + (int)(sec * 10);
        }

        public static implicit operator Time(int value)
        {
            return new Time(value);
        }

        public static Time Seconds(double sec) => (int)(sec * 10);

        public static Time Minutes(double min) => Seconds(min * 60);

        public static Time Rand(int min, int max) => (int)(rand.NextDouble() * (max - min) + min);
    }
}
