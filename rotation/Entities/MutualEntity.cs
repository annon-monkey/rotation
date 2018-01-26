using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using rotation.Common;
using rotation.Data;

namespace rotation.Entities
{
    class MutualEntity : EntityBase
    {
        public EntityTimeline Timeline { get; } = new EntityTimeline();

        public IReverseFunction ReverseFunction { get; set; } = new RegularReverseFunction
        {
            HalfCycleTime = new Time(12),
        };

        public override double LastVelocity => throw new NotImplementedException();

        public override IEnumerable<RotationDataRow> Output()
        {
            var temp = new RotationDataRow();

            var length = this.Timeline.LengthTime.Value;
            for (var i = 0; i < length; i++)
            {
                var isReverse = this.ReverseFunction.IsReverse(i);
                temp.DuringTime = new Time(1);
                temp.Velocity = this.Timeline.GetRow(i).Velocity * (isReverse ? -1 : 1);
                yield return temp;
            }
        }
    }
}
