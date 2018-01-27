using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using rotation.Data;

namespace rotation.Entities
{
    class LoopEntity : EntityBase
    {
        public List<IEntity> Entities { get; } = new List<IEntity>();

        public int LoopCount { get; set; }

        public override double LastVelocity => this.Entities.Last().LastVelocity;

        public override IEnumerable<RotationDataRow> Output()
        {
            var loop = this.Entities.SelectMany(e => e.Output()).ToArray();
            var result = Enumerable.Empty<RotationDataRow>();
            for (var i = 0; i < this.LoopCount; i++)
            {
                result = result.Concat(loop);
            }
            return result;
        }
    }
}
