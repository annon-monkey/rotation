using Rotation.Forms.Models.Editor.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rotation.Forms.Models.Editor.Elements
{
    static class Element
    {
        public static IElement FromSerializedText(string text)
        {
            var k = new string(text.TakeWhile(c => c != '$').ToArray());
            if (int.TryParse(k, out int keyInt))
            {
                var key = (ElementType)keyInt;
                var element = FromElementType(key);

                try
                {
                    element?.LoadSerializedText(text);
                }
                catch
                {
                    return null;
                }
                return element;
            }

            return null;
        }

        public static IElement FromElementType(ElementType type)
        {
            IElement element = null;
            switch (type)
            {
                case ElementType.Point:
                    element = new PointElement();
                    break;
                case ElementType.Line:
                    element = new LineElement();
                    break;
                case ElementType.Mutual:
                    element = new MutualElement();
                    break;
                case ElementType.Loop:
                    element = new LoopElement();
                    break;
                case ElementType.LinearMutual:
                    element = new LinearMutualElement();
                    break;
                case ElementType.RandomMutual:
                    element = new RandomMutualElement();
                    break;
                case ElementType.Random:
                    element = new RandomElement();
                    break;
                case ElementType.Trigonometric:
                    element = new TrigonometricElement();
                    break;
                case ElementType.From:
                    element = new FromElement();
                    break;
                case ElementType.Stop:
                    element = new StopElement();
                    break;
            }

            return element;
        }
    }
}
