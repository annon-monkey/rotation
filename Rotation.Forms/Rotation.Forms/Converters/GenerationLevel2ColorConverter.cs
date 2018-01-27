using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace Rotation.Forms.Converters
{
    class GenerationLevel2ColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int i)
            {
                switch (i)
                {
                    case 0:
                        return Color.Transparent;
                    case 1:
                        return new Color(0.8, 0.8, 0.8);
                    case 2:
                        return new Color(0.6, 0.6, 0.6);
                    case 3:
                        return new Color(0.4, 0.4, 0.4);
                    case 4:
                        return new Color(0.2, 0.2, 0.2);
                    default:
                        return Color.Black;
                }
            }
            throw new NotSupportedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
