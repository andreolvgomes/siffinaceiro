using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SIF.Commom
{
    public class RedimensionarImage : IDisposable
    {
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public byte[] ImageResize(byte[] photoBytes, int width, int height)
        {
            //int thumbnailSize = 150;
            using (MemoryStream photoStream = new MemoryStream(photoBytes))
            {
                var photo = ReadBitmapFrame(photoStream);
                //int width, height;
                //if (photo.Width > photo.Height)
                //{
                //    width = thumbnailSize;
                //    height = 200; // (int)(photo.Height * ThumbnailSize / photo.Width);
                //}
                //else
                //{
                //    width = (int)(photo.Width * thumbnailSize / photo.Height);
                //    height = thumbnailSize;
                //}
                var resized = Resize(photo, width, height);
                return ToByteArray(resized);
            }
        }

        private byte[] ToByteArray(BitmapFrame targetFrame)
        {
            byte[] targetBytes = null;
            using (var memoryStream = new MemoryStream())
            {
                var targetEncoder = new PngBitmapEncoder();
                targetEncoder.Frames.Add(targetFrame);
                targetEncoder.Save(memoryStream);
                targetBytes = memoryStream.ToArray();
            }
            return targetBytes;
        }

        private BitmapFrame ReadBitmapFrame(MemoryStream photoStream)
        {
            BitmapDecoder photoDecoder = BitmapDecoder.Create(photoStream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.None);
            return photoDecoder.Frames[0];
        }

        private BitmapFrame Resize(BitmapFrame photo, int width, int height)
        {
            return Resize(photo, width, height, BitmapScalingMode.HighQuality);
        }

        private BitmapFrame Resize(BitmapFrame photo, int width, int height, BitmapScalingMode scalingMode)
        {
            return Draw(width, height, scalingMode, new ImageDrawing(photo, new Rect(0, 0, width, height)));
        }

        private BitmapFrame Draw(int width, int height, BitmapScalingMode scalingMode, params Drawing[] drawings)
        {
            DrawingVisual targetVisual = new DrawingVisual();
            var targetContext = targetVisual.RenderOpen();
            DrawingGroup group = new DrawingGroup();
            RenderOptions.SetBitmapScalingMode(group, scalingMode);
            foreach (var drawing in drawings)
            {
                group.Children.Add(drawing);
            }
            targetContext.DrawDrawing(group);
            RenderTargetBitmap target = new RenderTargetBitmap(width, height, 96, 96, PixelFormats.Default); targetContext.Close();
            target.Render(targetVisual);
            BitmapFrame targetFrame = BitmapFrame.Create(target);
            return targetFrame;
        }
    }
}
