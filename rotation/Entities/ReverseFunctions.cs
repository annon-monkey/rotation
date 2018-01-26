using rotation.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rotation.Entities
{
    interface IReverseFunction
    {
        bool IsReverse(int time);
    }

    /// <summary>
    /// 一定間隔でオンオフを繰り返す
    /// </summary>
    class RegularReverseFunction : IReverseFunction
    {
        /// <summary>
        /// オンにかかる時間、またはオフにかかる時間
        /// </summary>
        public Time HalfCycleTime { get; set; }

        public RegularReverseFunction() { }

        public RegularReverseFunction(Time during)
        {
            this.HalfCycleTime = during;
        }

        public bool IsReverse(int time)
        {
            return (time / this.HalfCycleTime.Value) % 2 == 1;
        }
    }

    /// <summary>
    /// 少しずつオンオフの時間が長くなる・短くなる
    /// </summary>
    class LineReverseFunction : IReverseFunction
    {
        /// <summary>
        /// 変化してもいい最大の時間。オン・オフの時間がこれより長くなることはない。
        /// 超えたらまたFirstTimeからやり直す
        /// </summary>
        public Time LimitTime { get; set; }

        /// <summary>
        /// 最初の時間
        /// </summary>
        public Time FirstTime { get; set; }

        /// <summary>
        /// 最初の時間に加算する時間
        /// </summary>
        public Time DeltaTime { get; set; }

        public bool IsReverse(int time)
        {
            return this.GetId(time) % 2 == 1;
        }

        private int GetId(int time)
        {
            var id = -1;
            var t = 0;
            var d = this.FirstTime.Value;
            while (t < time)
            {
                t += d;

                d += this.DeltaTime.Value;
                if (this.DeltaTime.Value >= 0 ? d > this.LimitTime.Value : d < this.LimitTime.Value)
                {
                    d = this.FirstTime.Value;
                }
                id++;
            }

            return id < 0 ? 0 : id;
        }
    }

    class RandomReverseFunction : IReverseFunction
    {
        private int[] randomTimes;
        private int sumTime;

        public Time MaxTime { get; }

        public Time MinTime { get; }

        public RandomReverseFunction(Time min, Time max)
        {
            this.MaxTime = max;
            this.MinTime = min;

            var size = 128;
            this.randomTimes = new int[size];
            var rand = new Random();
            for (int i = 0; i < size; i++)
            {
                this.randomTimes[i] = (int)(rand.NextDouble() * (max.Value - min.Value) + min.Value);
            }
            this.sumTime = this.randomTimes.Sum();
        }

        public bool IsReverse(int time)
        {
            var t = time % this.sumTime;
            var id = 0;
            foreach (var tt in this.randomTimes)
            {
                t -= tt;
                if (t < 0)
                {
                    break;
                }
                id++;
            }

            return id % 2 == 1;
        }
    }
}
