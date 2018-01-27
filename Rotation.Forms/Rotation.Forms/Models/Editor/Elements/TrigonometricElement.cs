using rotation.Entities;
using Rotation.Forms.Models.Editor.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rotation.Forms.Models.Editor.Elements
{
    class TrigonometricElement : ElementBase
    {
        public override ElementType ElementType => ElementType.Trigonometric;

        private static readonly IEnumerable<PickerItem<TrigonometricFunction>> _functions = new PickerItem<TrigonometricFunction>[]
        {
            new PickerItem<TrigonometricFunction>
            {
                Identity = TrigonometricFunction.Start,
                Text = "はじめに大きく加速／減速",
            },
            new PickerItem<TrigonometricFunction>
            {
                Identity = TrigonometricFunction.End,
                Text = "おわりに大きく加速／減速",
            },
        };
        public IEnumerable<PickerItem<TrigonometricFunction>> Functions => _functions;

        public PickerItem<TrigonometricFunction> Function
        {
            get => this._function;
            set
            {
                if (this._function != value)
                {
                    this._function = value;
                    this.OnPropertyChanged();
                }
            }
        }
        private PickerItem<TrigonometricFunction> _function;

        public int DuringTime
        {
            get => this._duringTime;
            set
            {
                if (this._duringTime != value)
                {
                    this._duringTime = value;
                    this.OnPropertyChanged();
                }
            }
        }
        private int _duringTime;

        public Velocity StartVelocity { get; } = new Velocity();

        public Velocity EndVelocity { get; } = new Velocity();

        public TrigonometricElement()
        {
            this.StartVelocity.PropertyChanged += (sender, e) => this.UpdateDescription();
            this.EndVelocity.PropertyChanged += (sender, e) => this.UpdateDescription();
            this.Function = this.Functions.First();
            this.UpdateDescription();
        }

        private void UpdateDescription()
        {
            this.Description = $"Trigonometric   [{this.StartVelocity.ToListText()} -> {this.EndVelocity.ToListText()}, {TimeUtil.ToListText(this.DuringTime)}, func: {(this.Function?.Identity == TrigonometricFunction.Start ? "始" : "終" ?? "？")}]";
            this.IsError = this.DuringTime <= 0 || this.Function == null;
        }

        public override IEntity ToEntity(IEntity before)
        {
            if (!this.StartVelocity.IsTakeOver)
            {
                before = before.From(this.StartVelocity.ToEntityValue());
            }

            return before.Trigonometric(this.DuringTime, this.EndVelocity.ToEntityValue(), this.Function?.Identity ?? TrigonometricFunction.Start);
        }

        public override string ToSerializedText()
        {
            return base.ToSerializedText() + $"|{((int?)this.Function?.Identity ?? 0)}|{this.StartVelocity.ToSerializedText()}|{this.EndVelocity.ToSerializedText()}|{this.DuringTime}";
        }

        public override void LoadSerializedText(string text)
        {
            var data = text.Split('|');
            base.LoadSerializedText(data[0]);
            var fid = int.Parse(data[1]);
            this.Function = this.Functions.SingleOrDefault(f => (int)f.Identity == fid);
            this.StartVelocity.LoadSerializedText(data[2]);
            this.EndVelocity.LoadSerializedText(data[3]);
            this.DuringTime = int.Parse(data[4]);
        }
    }
}
