using Rotation.Forms.Models.Editor.Values;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rotation.Forms.Models.Editor.Elements
{
    class LineElement : ElementBase
    {
        public override ElementType ElementType => ElementType.Line;

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

        public Velocity StartVelocity { get; } = new Velocity();

        public Velocity EndVelocity { get; } = new Velocity();

        public LineElement()
        {
            this.StartVelocity.PropertyChanged += (sender, e) => this.UpdateDescription();
            this.EndVelocity.PropertyChanged += (sender, e) => this.UpdateDescription();
            this.UpdateDescription();
        }

        private void UpdateDescription()
        {
            this.Description = $"Line   [{this.StartVelocity.ToListText()} -> {this.EndVelocity.ToListText()}/ {TimeUtil.ToListText(this.DuringTime)}]";
            this.IsError = this.DuringTime <= 0;
        }
    }
}
