using System;
using System.Collections.Generic;
using System.Text;
using rotation.Entities;
using Rotation.Forms.Models.Editor.Values;

namespace Rotation.Forms.Models.Editor.Elements
{
    class MutualElement : ElementBase
    {
        public override ElementType ElementType => ElementType.Mutual;

        public int HalfFrequencyTime
        {
            get => this._halfFrequencyTime;
            set
            {
                if (this._halfFrequencyTime != value)
                {
                    this._halfFrequencyTime = value;
                    this.OnPropertyChanged();
                }
            }
        }
        private int _halfFrequencyTime;

        public MutualElement()
        {
            this.CanHaveChildren = true;
            this.UpdateDescription();
        }

        private void UpdateDescription()
        {
            this.Description = $"Mutual   [{TimeUtil.ToListText(this.HalfFrequencyTime)}]";
            this.IsError = this.HalfFrequencyTime <= 0;
        }

        public override IEntity ToEntity(IEntity before)
        {
            throw new NotSupportedException();
        }

        public override string ToSerializedText()
        {
            return base.ToSerializedText() + $"|{this.HalfFrequencyTime}";
        }

        public override void LoadSerializedText(string text)
        {
            var data = text.Split('|');
            base.LoadSerializedText(data[0]);
            this.HalfFrequencyTime= int.Parse(data[1]);
        }
    }
}
