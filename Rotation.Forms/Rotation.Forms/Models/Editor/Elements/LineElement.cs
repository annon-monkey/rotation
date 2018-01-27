using rotation.Entities;
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

        public override IEntity ToEntity(IEntity before)
        {
            if (this.StartVelocity.IsTakeOver)
            {
                return before.Line(this.DuringTime, this.EndVelocity.ToEntityValue());
            }
            else
            {
                return before.From(this.StartVelocity.ToEntityValue())
                             .Line(this.DuringTime, this.EndVelocity.ToEntityValue());
            }
        }

        public override string ToSerializedText()
        {
            return base.ToSerializedText() + $"|{this.DuringTime}|{this.StartVelocity.ToSerializedText()}|{(this.EndVelocity.ToSerializedText())}";
        }

        public override void LoadSerializedText(string text)
        {
            var data = text.Split('|');
            base.LoadSerializedText(data[0]);
            this.DuringTime = int.Parse(data[1]);
            this.StartVelocity.LoadSerializedText(data[2]);
            this.EndVelocity.LoadSerializedText(data[3]);
        }
    }
}
