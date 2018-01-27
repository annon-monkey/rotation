using Rotation.Forms.Models.Editor.Values;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rotation.Forms.Models.Editor.Elements
{
    interface IElement
    {
        ElementType ElementType { get; }

        string Description { get; }

        bool CanHaveChildren { get; }

        bool CanNest { get; }

        int GenerationLevel { get; set; }

        bool IsError { get; }
    }
}
