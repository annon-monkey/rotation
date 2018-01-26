using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rotation.Data
{
    struct OutputDataRow
    {
        public int Velocity { get; set; }

        public int Time { get; set; }

        public override string ToString()
        {
            return $"{this.Time},{(this.Velocity >= 0 ? 0 : 1)},{Math.Abs(this.Velocity)}";
        }
    }

    static class OutputUtil
    {
        public static int ToOutputVelocity(double velocity)
        {
            return (int)(velocity * 100);
        }
    }
}
