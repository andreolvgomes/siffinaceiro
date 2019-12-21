using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace SIF.Commom.Converters
{
    public class ConvertBytesToImageBrush : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                byte[] byteArrayIn = value as byte[];
                using (MemoryStream stream = new MemoryStream())
                {
                    stream.Write(byteArrayIn, 0, byteArrayIn.Length);
                    stream.Position = 0;
                    System.Drawing.Image img = System.Drawing.Image.FromStream(stream);
                    BitmapImage returnImage = new BitmapImage();
                    returnImage.BeginInit();
                    using (MemoryStream ms = new MemoryStream())
                    {
                        img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                        ms.Seek(0, SeekOrigin.Begin);
                        returnImage.StreamSource = ms;
                        returnImage.EndInit();

                        return returnImage;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
