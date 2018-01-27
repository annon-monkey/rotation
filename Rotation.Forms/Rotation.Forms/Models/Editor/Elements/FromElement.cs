using System;
using System.Collections.Generic;
using System.Text;
using rotation.Entities;
using Rotation.Forms.Models.Editor.Values;

namespace Rotation.Forms.Models.Editor.Elements
{
    class FromElement : ElementBase
    {
        public override ElementType ElementType => ElementType.From;

        public Velocity Velocity { get; } = new Velocity();

        public FromElement()
        {
            this.Velocity.PropertyChanged += (sender, e) => this.UpdateDescription();
            this.UpdateDescription();
        }

        public override IEntity ToEntity(IEntity before)
        {
            return before.From(this.Velocity.ToEntityValue());
        }

        private void UpdateDescription()
        {
            this.Description = $"From   [{this.Velocity.ToListText()}]";
        }

        public override string ToSerializedText()
        {
            return base.ToSerializedText() + $"|{this.Velocity.ToSerializedText()}";
        }

        public override void LoadSerializedText(string text)
        {
            var data = text.Split('|');
            base.LoadSerializedText(data[0]);
            this.Velocity.LoadSerializedText(data[1]);
        }
    }
}
