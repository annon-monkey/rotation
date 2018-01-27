using rotation.Entities;
using Rotation.Forms.Models.Editor.Values;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rotation.Forms.Models.Editor.Elements
{
    class LinearMutualElement : ElementBase
    {
        public override ElementType ElementType => ElementType.LinearMutual;

        public int FirstTime
        {
            get => this._firstTime;
            set
            {
                if (this._firstTime != value)
                {
                    this._firstTime = value;
                    this.OnPropertyChanged();
                    this.UpdateDescription();
                }
            }
        }
        private int _firstTime;

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

        public int DeltaTime
        {
            get => this._deltaTime;
            set
            {
                if (this._deltaTime != value)
                {
                    this._deltaTime = value;
                    this.OnPropertyChanged();
                    this.UpdateDescription();
                }
            }
        }
        private int _deltaTime;

        public LinearMutualElement()
        {
            this.CanHaveChildren = true;
            this.UpdateDescription();
        }

        private void UpdateDescription()
        {
            this.Description = $"Linear Mutual   [{TimeUtil.ToListText(this.FirstTime)} -> {TimeUtil.ToListText(this.MaxTime)} ( d = {TimeUtil.ToListText(this.DeltaTime)})]";
            this.IsError = this.DeltaTime == 0 || (this.DeltaTime > 0 ? !(this.FirstTime < this.MaxTime) : !(this.MaxTime < this.FirstTime));
        }

        public override IEntity ToEntity(IEntity before)
        {
            throw new NotSupportedException();
        }

        public override string ToSerializedText()
        {
            return base.ToSerializedText() + $"|{this.FirstTime}|{this.MaxTime}|{this.DeltaTime}";
        }

        public override void LoadSerializedText(string text)
        {
            var data = text.Split('|');
            base.LoadSerializedText(data[0]);
            this.FirstTime = int.Parse(data[1]);
            this.MaxTime = int.Parse(data[2]);
            this.DeltaTime = int.Parse(data[3]);
        }
    }
}
