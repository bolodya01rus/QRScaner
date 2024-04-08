using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Interop;

namespace QRScaner.Converter
{
    internal class ImageConverter : Converter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Imaging.CreateBitmapSourceFromBitmap((Bitmap)value);
        }
    }
}
