using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Imaging;
using System.Drawing;

namespace Zdd.Utility
{
    public abstract class BaseTransformationFilter : IFilter, IFilterInformation
    {
        public abstract Dictionary<PixelFormat, PixelFormat> FormatTranslations { get; }
        public Bitmap Apply(Bitmap image)
        {
            // lock source bitmap data
            BitmapData srcData = image.LockBits(
                new Rectangle(0, 0, image.Width, image.Height),
                ImageLockMode.ReadOnly, image.PixelFormat);

            Bitmap dstImage = null;

            try
            {
                // apply the filter
                dstImage = Apply(srcData);
                if ((image.HorizontalResolution > 0) && (image.VerticalResolution > 0))
                {
                    dstImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);
                }
            }
            finally
            {
                // unlock source image
                image.UnlockBits(srcData);
            }

            return dstImage;
        }
        public Bitmap Apply(BitmapData imageData)
        {
            // check pixel format of the source image
            CheckSourceFormat(imageData.PixelFormat);

            // destination image format
            PixelFormat dstPixelFormat = FormatTranslations[imageData.PixelFormat];

            // get new image size
            Size newSize = CalculateNewImageSize(new UnmanagedImage(imageData));

            // create new image of required format
            Bitmap dstImage = (dstPixelFormat == PixelFormat.Format8bppIndexed) ?
                ImageHelper.CreateGrayscaleImage(newSize.Width, newSize.Height) :
                new Bitmap(newSize.Width, newSize.Height, dstPixelFormat);

            // lock destination bitmap data
            BitmapData dstData = dstImage.LockBits(
                new Rectangle(0, 0, newSize.Width, newSize.Height),
                ImageLockMode.ReadWrite, dstPixelFormat);

            try
            {
                // process the filter
                ProcessFilter(new UnmanagedImage(imageData), new UnmanagedImage(dstData));
            }
            finally
            {
                // unlock destination images
                dstImage.UnlockBits(dstData);
            }

            return dstImage;
        }
        public UnmanagedImage Apply(UnmanagedImage image)
        {
            // check pixel format of the source image
            CheckSourceFormat(image.PixelFormat);

            // get new image size
            Size newSize = CalculateNewImageSize(image);

            // create new destination image
            UnmanagedImage dstImage = UnmanagedImage.Create(newSize.Width, newSize.Height, FormatTranslations[image.PixelFormat]);

            // process the filter
            ProcessFilter(image, dstImage);

            return dstImage;
        }
        public void Apply(UnmanagedImage sourceImage, UnmanagedImage destinationImage)
        {
            // check pixel format of the source and destination images
            CheckSourceFormat(sourceImage.PixelFormat);

            // ensure destination image has correct format
            if (destinationImage.PixelFormat != FormatTranslations[sourceImage.PixelFormat])
            {
                throw new Exception("Destination pixel format is specified incorrectly.");
            }

            // get new image size
            Size newSize = CalculateNewImageSize(sourceImage);

            // ensure destination image has correct size
            if ((destinationImage.Width != newSize.Width) || (destinationImage.Height != newSize.Height))
            {
                throw new Exception("Destination image must have the size expected by the filter.");
            }

            // process the filter
            ProcessFilter(sourceImage, destinationImage);
        }
        protected abstract System.Drawing.Size CalculateNewImageSize(UnmanagedImage sourceData);
        protected abstract unsafe void ProcessFilter(UnmanagedImage sourceData, UnmanagedImage destinationData);
        private void CheckSourceFormat(PixelFormat pixelFormat)
        {
            if (!FormatTranslations.ContainsKey(pixelFormat))
                throw new Exception("Source pixel format is not supported by the filter.");
        }
    }
}
