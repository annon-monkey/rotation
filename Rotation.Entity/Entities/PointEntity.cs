using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using rotation.Common;
using rotation.Data;

namespace rotation.Entities
{
    class PointEntity : EntityBase
    {
        public Time DuringTime { get; set; }

        public double Velocity { get; set; }

        public override double LastVelocity => this.Velocity;

        public override IEnumerable<RotationDataRow> Output()
        {
            yield return new RotationDataRow
            {
                DuringTime = this.DuringTime,
                Velocity = this.Velocity,
            };
        }
    }
}
