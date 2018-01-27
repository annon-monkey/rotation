using rotation.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rotation.Entities
{
    public interface IEntity
    {
        double LastVelocity { get; }

        IEnumerable<RotationDataRow> Output();
    }
}
