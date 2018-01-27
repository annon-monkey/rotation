using System;
using System.Collections.Generic;
using System.Text;
using rotation.Entities;
using Rotation.Forms.Models.Editor.Values;

namespace Rotation.Forms.Models.Editor.Elements
{
    class RandomElement : ElementBase
    {
        public override ElementType ElementType => ElementType.Random;

        public Velocity MaxVelocity { get; } = new Velocity();

        public Velocity MinVelocity { get; } = new Velocity();

        public int SingleDuringTime
        {
            get => this._singleDuringTime;
            set
            {
                if (this._singleDuringTime != value)
                {
                    this._singleDuringTime = value;
                    this.OnPropertyChanged();
                    this.UpdateDescription();
                }
            }
        }
        private int _singleDuringTime;

        public int LoopCount
        {
            get => this._loopCount;
            set
            {
                if (this._loopCount != value)
                {
                    this._loopCount = value;
                    this.OnPropertyChanged();
                    this.UpdateDescription();
                }
            }
        }
        private int _loopCount;

        public RandomElement()
        {
            this.MaxVelocity.PropertyChanged += (sender, e) => this.UpdateDescription();
            this.MinVelocity.PropertyChanged += (sender, e) => this.UpdateDescription();
            this.UpdateDescription();
        }

        private void UpdateDescription()
        {
            this.Description = $"Random   [{this.MinVelocity.ToListText()} <-> {this.MaxVelocity.ToListText()}, {TimeUtil.ToListText(this.SingleDuringTime)}毎に変化, {this.LoopCount}回]";
            this.IsError = this.SingleDuringTime <= 0 || this.LoopCount <= 0;
        }

        public override IEntity ToEntity(IEntity before)
        {
            return before.Random(this.MinVelocity.ToEntityValue(), this.MaxVelocity.ToEntityValue(), this.SingleDuringTime, this.LoopCount);
        }

        public override string ToSerializedText()
        {
            return base.ToSerializedText() + $"|{this.MaxVelocity.ToSerializedText()}|{this.MinVelocity.ToSerializedText()}|{this.SingleDuringTime}|{this.LoopCount}";
        }

        public override void LoadSerializedText(string text)
        {
            var data = text.Split('|');
            base.LoadSerializedText(data[0]);
            this.MaxVelocity.LoadSerializedText(data[1]);
            this.MinVelocity.LoadSerializedText(data[2]);
            this.SingleDuringTime = int.Parse(data[3]);
            this.LoopCount = int.Parse(data[4]);
        }
    }
}
