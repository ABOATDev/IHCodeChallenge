using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace IHCode
{
    public static class UsefulColors
    {

        public static Color DARK_BACKROUND { get; } =  Colors.Transparent; 
        public static Color BRIGHT_CODE_COLOR { get; } = Color.FromRgb(228, 229, 225);

        public static Color DARKER_BACKGROUND { get; } = Color.FromRgb(35, 39, 42);

        public static Color FILE_BUTTONS_COLOR { get; } = Color.FromRgb(48, 82, 105);

        public static SolidColorBrush GetBrush(this Color color)
        {

            return new SolidColorBrush(color);

        }

    }
}
