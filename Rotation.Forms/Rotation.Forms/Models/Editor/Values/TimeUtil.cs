using System;
using System.Collections.Generic;
using System.Text;

namespace Rotation.Forms.Models.Editor.Values
{
    static class TimeUtil
    {
        public static string ToListText(int time)
        {
            return $"{time / 10}.{time % 10}秒";
        }
    }
}
