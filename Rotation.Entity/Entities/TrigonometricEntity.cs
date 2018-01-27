using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using rotation.Common;
using rotation.Data;

namespace rotation.Entities
{
    class TrigonometricEntity : EntityBase
    {
        public TrigonometricFunction Function { get; set; } = TrigonometricFunction.Start;

        public double StartVelocity { get; set; }

        public double EndVelocity { get; set; }

        public Time DuringTime { get; set; }

        public override double LastVelocity => this.EndVelocity;

        public override IEnumerable<RotationDataRow> Output()
        {
            Func<int, double> f = default;
            switch (this.Function)
            {
                case TrigonometricFunction.Start:
                    f = (t) => Math.Sin(Math.PI / 2 * ((double)t / this.DuringTime.Value));
                    break;
                case TrigonometricFunction.End:
                    f = (t) => Math.Cos(Math.PI / 2 * ((double)t / this.DuringTime.Value)) * -1;
                    break;
            }

            var based = this.Function == TrigonometricFunction.Start ? this.StartVelocity : this.EndVelocity;
            for (var t = 0; t < this.DuringTime.Value; t++)
            {
                var v = f(t) * (this.EndVelocity - this.StartVelocity) + based;
                yield return new RotationDataRow
                {
                    DuringTime = 1,
                    Velocity = v,
                };
            }
        }
    }

    public enum TrigonometricFunction
    {
        /// <summary>
        /// 最初に加速または減速する
        /// </summary>
        Start = 1,

        /// <summary>
        /// 最後に加速または減速する
        /// </summary>
        End = 2,
    }
}
