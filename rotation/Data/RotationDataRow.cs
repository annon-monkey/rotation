using rotation.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rotation.Data
{
    struct RotationDataRow
    {
        public Time DuringTime { get; set; }
        
        public double Velocity
        {
            get => this._velocity;
            set
            {
                if (value < -1 || 1 < value)
                {
                    throw new ArgumentException("速度は-1から1までの間にしてください");
                }
                this._velocity = value;
            }
        }
        private double _velocity;
    }
}
