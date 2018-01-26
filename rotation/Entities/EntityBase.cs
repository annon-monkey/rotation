using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using rotation.Data;

namespace rotation.Entities
{
    abstract class EntityBase : IEntity
    {
        public abstract double LastVelocity { get; }

        public abstract IEnumerable<RotationDataRow> Output();
    }
}
