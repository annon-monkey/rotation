using System;
using System.Collections.Generic;
using System.Text;
using rotation.Entities;
using Rotation.Forms.Models.Editor.Values;

namespace Rotation.Forms.Models.Editor.Elements
{
    class StopElement : PointElement
    {
        public override ElementType ElementType => ElementType.Stop;

        public StopElement()
        {
            this.Velocity.Value = 0;
            this.Velocity.IsTakeOver = false;
        }

        protected override void UpdateDescription()
        {
            this.Description = $"Stop   [{this.Velocity.ToListText()}/ {TimeUtil.ToListText(this.DuringTime)}]";
            this.IsError = this.DuringTime <= 0;
        }
    }
}
