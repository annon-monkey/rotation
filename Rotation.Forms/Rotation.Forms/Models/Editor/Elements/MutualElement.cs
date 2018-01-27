using System;
using System.Collections.Generic;
using System.Text;
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
    }
}
