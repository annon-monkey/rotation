using rotation.Entities;
using Rotation.Forms.Models.Editor.Values;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rotation.Forms.Models.Editor.Elements
{
    class RandomMutualElement : ElementBase
    {
        public override ElementType ElementType => ElementType.RandomMutual;

        public int MaxTime
        {
            get => this._maxTime;
            set
            {
                if (this._maxTime != value)
                {
                    this._maxTime = value;
                    this.OnPropertyChanged();
                    this.UpdateDescription();
                }
            }
        }
        private int _maxTime;

        public int MinTime
        {
            get => this._minTime;
            set
            {
                if (this._minTime != value)
                {
                    this._minTime = value;
                    this.OnPropertyChanged();
                    this.UpdateDescription();
                }
            }
        }
        private int _minTime;

        public RandomMutualElement()
        {
            this.CanHaveChildren = true;
            this.UpdateDescription();
        }

        private void UpdateDescription()
        {
            this.Description = $"Random Mutual   [{TimeUtil.ToListText(this.MinTime)} <-> {TimeUtil.ToListText(this.MaxTime)}]";
            this.IsError = this.MinTime <= 0 || this.MaxTime <= 0;
        }

        public override IEntity ToEntity(IEntity before)
        {
            throw new NotSupportedException();
        }

        public override string ToSerializedText()
        {
            return base.ToSerializedText() + $"|{this.MinTime}|{this.MaxTime}";
        }

        public override void LoadSerializedText(string text)
        {
            var data = text.Split('|');
            base.LoadSerializedText(data[0]);
            this.MaxTime = int.Parse(data[1]);
            this.MinTime = int.Parse(data[2]);
        }
    }
}
