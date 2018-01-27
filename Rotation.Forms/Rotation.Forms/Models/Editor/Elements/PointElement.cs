using Rotation.Forms.Models.Editor.Values;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rotation.Forms.Models.Editor.Elements
{
    class PointElement : ElementBase
    {
        public override ElementType ElementType => ElementType.Point;

        public int DuringTime
        {
            get => this._duringTime;
            set
            {
                if (this._duringTime != value)
                {
                    this._duringTime = value;
                    this.OnPropertyChanged();
                    this.UpdateDescription();
                }
            }
        }
        private int _duringTime;

        public Velocity Velocity { get; } = new Velocity();

        public PointElement()
        {
            this.Velocity.PropertyChanged += (sender, e) => this.UpdateDescription();
            this.UpdateDescription();
        }

        private void UpdateDescription()
        {
            this.Description = $"Point   [{this.Velocity.ToListText()}/ {TimeUtil.ToListText(this.DuringTime)}]";
            this.IsError = this.DuringTime <= 0;
        }
    }
}
