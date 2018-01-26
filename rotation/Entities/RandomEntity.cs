using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using rotation.Common;
using rotation.Data;

namespace rotation.Entities
{
    class RandomEntity : EntityBase
    {
        public int LoopCount { get; set; }

        public Time SingleDuringTime { get; set; }

        public double MaxVelocity { get; set; }

        public double MinVelocity { get; set; }

        public override double LastVelocity => this.MaxVelocity;

        public override IEnumerable<RotationDataRow> Output()
        {
            var rand = new Random();

            for (int i = 0; i < this.LoopCount - 1; i++)
            {
                yield return new RotationDataRow
                {
                    DuringTime = this.SingleDuringTime,
                    Velocity = rand.NextDouble() * (this.MaxVelocity - this.MinVelocity) + this.MinVelocity,
                };
            }
            yield return new RotationDataRow
            {
                DuringTime = this.SingleDuringTime,
                Velocity = this.MaxVelocity,
            };
        }
    }
}
