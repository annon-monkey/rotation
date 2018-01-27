using System;
using System.Collections.Generic;
using System.Text;

namespace Rotation.Forms.Models.Editor.Values
{
    class PickerItem<ID>
    {
        public ID Identity { get; set; }
        public string Text { get; set; }
        public override string ToString() => this.Text;
    }
}
