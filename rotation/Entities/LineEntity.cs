using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using rotation.Common;
using rotation.Data;

namespace rotation.Entities
{
    class LineEntity : EntityBase
    {
        public Time DuringTime { get; set; }

        public double StartVelocity { get; set; }

        public double EndVelocity { get; set; }

        public override double LastVelocity => this.EndVelocity;

        public override IEnumerable<RotationDataRow> Output()
        {
            var frames = this.DuringTime.Value;
            for (var f = 0; f < frames; f++)
            {
                yield return new RotationDataRow
                {
                    DuringTime = new Time(1),
                    Velocity = (this.EndVelocity - this.StartVelocity) / frames * f + this.StartVelocity,
                };
            }
        }
    }
}
