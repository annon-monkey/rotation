using System;
using System.Collections.Generic;
using System.Text;
using rotation.Entities;
using Rotation.Forms.Models.Editor.Values;

namespace Rotation.Forms.Models.Editor.Elements
{
    class LoopElement : ElementBase
    {
        public override ElementType ElementType => ElementType.Loop;

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

        public LoopElement()
        {
            this.CanHaveChildren = true;
            this.CanNest = true;
            this.UpdateDescription();
        }

        private void UpdateDescription()
        {
            this.Description = $"Loop   [{this.LoopCount}回]";
            this.IsError = this.LoopCount <= 0;
        }

        public override IEntity ToEntity(IEntity before)
        {
            throw new NotSupportedException();
        }

        public override string ToSerializedText()
        {
            return base.ToSerializedText() + $"|{this.LoopCount}";
        }

        public override void LoadSerializedText(string text)
        {
            var data = text.Split('|');
            base.LoadSerializedText(data[0]);
            this.LoopCount = int.Parse(data[1]);
        }
    }
}
