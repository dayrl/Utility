using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace Zdd.Utility
{
    public class Crop : BaseTransformationFilter
    {
        private Rectangle rect;

        // format translation dictionary
        private Dictionary<PixelFormat, PixelFormat> formatTranslations = new Dictionary<PixelFormat, PixelFormat>();

        /// <summary>
        /// Format translations dictionary.
        /// </summary>
        public override Dictionary<PixelFormat, PixelFormat> FormatTranslations
        {
            get { return formatTranslations; }
        }

        /// <summary>
        /// Rectangle to crop.
        /// </summary>
        public Rectangle Rectangle
        {
            get { return rect; }
            set { rect = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Crop"/> class.
        /// </summary>
        /// 
        /// <param name="rect">Rectangle to crop.</param>
        /// 
        public Crop(Rectangle rect)
        {
            this.rect = rect;

            formatTranslations[PixelFormat.Format8bppIndexed] = PixelFormat.Format8bppIndexed;
            formatTranslations[PixelFormat.Format24bppRgb] = PixelFormat.Format24bppRgb;
            formatTranslations[PixelFormat.Format32bppRgb] = PixelFormat.Format32bppRgb;
            formatTranslations[PixelFormat.Format32bppArgb] = PixelFormat.Format32bppArgb;
            formatTranslations[PixelFormat.Format16bppGrayScale] = PixelFormat.Format16bppGrayScale;
            formatTranslations[PixelFormat.Format48bppRgb] = PixelFormat.Format48bppRgb;
            formatTranslations[PixelFormat.Format64bppArgb] = PixelFormat.Format64bppArgb;
        }

        /// <summary>
        /// Calculates new image size.
        /// </summary>
        /// 
        /// <param name="sourceData">Source image data.</param>
        /// 
        /// <returns>New image size - size of the destination image.</returns>
        /// 
        protected override System.Drawing.Size CalculateNewImageSize(UnmanagedImage sourceData)
        {
            return new Size(rect.Width, rect.Height);
        }

        /// <summary>
        /// Process the filter on the specified image.
        /// </summary>
        /// 
        /// <param name="sourceData">Source image data.</param>
        /// <param name="destinationData">Destination image data.</param>
        /// 
        protected override unsafe void ProcessFilter(UnmanagedImage sourceData, UnmanagedImage destinationData)
        {
            // validate rectangle
            Rectangle srcRect = rect;
            srcRect.Intersect(new Rectangle(0, 0, sourceData.Width, sourceData.Height));

            int xmin = srcRect.Left;
            int ymin = srcRect.Top;
            int ymax = srcRect.Bottom - 1;
            int copyWidth = srcRect.Width;

            int srcStride = sourceData.Stride;
            int dstStride = destinationData.Stride;
            int pixelSize = Image.GetPixelFormatSize(sourceData.PixelFormat) / 8;
            int copySize = copyWidth * pixelSize;

            // do the job
            byte* src = (byte*)sourceData.ImageData.ToPointer() + ymin * srcStride + xmin * pixelSize;
            byte* dst = (byte*)destinationData.ImageData.ToPointer();

            if (rect.Top < 0)
            {
                dst -= dstStride * rect.Top;
            }
            if (rect.Left < 0)
            {
                dst -= pixelSize * rect.Left;
            }

            // for each line
            for (int y = ymin; y <= ymax; y++)
            {
                SystemTools.CopyUnmanagedMemory(dst, src, copySize);
                src += srcStride;
                dst += dstStride;
            }
        }
    }
}
