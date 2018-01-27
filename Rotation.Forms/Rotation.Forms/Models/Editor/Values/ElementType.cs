using System;
using System.Collections.Generic;
using System.Text;

namespace Rotation.Forms.Models.Editor.Values
{
    enum ElementType : int
    {
        CollectionSetting = -1,
        None = 0,
        Point = 1,
        Line = 2,
        Mutual = 3,
        Loop = 4,
        LinearMutual = 5,
        RandomMutual = 6,
        Random = 7,
        Trigonometric = 8,
        From = 9,
        Stop = 10,
        Comment = 11,
    }
}
