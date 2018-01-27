using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rotation.Data
{
    public struct OutputDataRow
    {
        public int Velocity { get; set; }

        public int Time { get; set; }

        public string ToCsvLine()
        {
            return $"{this.Time},{(this.Velocity >= 0 ? 0 : 1)},{Math.Abs(this.Velocity)}\n";
        }

        public byte[] ToBluetoothSignal()
        {
            var v = (byte)((byte)(uint)Math.Abs(this.Velocity) | (this.Velocity < 0 ? 0x80 : 0x00));
            return new byte[] { 0x02, 0x01, v, };
        }
    }

    public static class OutputUtil
    {
        public static int ToOutputVelocity(double velocity)
        {
            return (int)(velocity * 100);
        }
    }
}
