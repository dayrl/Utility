using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace Zdd.Utility
{
    public class ImageProcessor
    {
        /// <summary>
        /// 异步锁
        /// </summary>
        private static readonly object AsyncLock = new object();

        /// <summary>
        /// 根据指定的大小按比例缩放图片
        /// </summary>
        /// <param name="ImgStream">The img stream.</param>
        /// <param name="outWidth">Width of the out.</param>
        /// <param name="outHeight">Height of the out.</param>
        /// <param name="fillColor">Color of the fill.</param>
        /// <returns></returns>
        public static Image GetZoomImage(Stream ImgStream, int outWidth, int outHeight, Color fillColor)
        {
            if (ImgStream == null || ImgStream.Length == 0)
                throw new ArgumentNullException("ImgStream");

            //读取图片
            Bitmap readImg = new Bitmap(ImgStream);

            //判断是否需要缩放
            if (readImg.Height == outHeight && readImg.Width == outWidth)
            {
                return readImg;
            }

            //缩放处理
            int tmpHeight;//图片缩小后的高度
            int tmpWidth;//图片缩小后的宽度
            double widthProportion;//宽度缩小比
            double heightProportion;//高度缩小比

            widthProportion = (double)outWidth / readImg.Width;
            heightProportion = (double)outHeight / readImg.Height;

            if(outWidth <= 0)
            {
                widthProportion = 1;
            }
            if (outHeight <= 0)
            {
                heightProportion = 1;
            }
            if (widthProportion < heightProportion)
            {
                tmpHeight = (int)(widthProportion * readImg.Height);
                tmpWidth = (int)(widthProportion * readImg.Width);
            }
            else
            {
                tmpHeight = (int)(heightProportion * readImg.Height);
                tmpWidth = (int)(heightProportion * readImg.Width);
            }

            //判断只限制宽 或者 只限制高
            if (outWidth <= 0)
                outWidth = tmpWidth;
            if (outHeight <= 0)
                outHeight = tmpHeight;

            /*//最原始的缩小方法，效果不好
            return readImg.GetThumbnailImage(outWidth, outHeight, null, IntPtr.Zero);
            */

            //以下为改良方法
            Bitmap img = new Bitmap(outWidth, outHeight);
            img.SetResolution(72f, 72f);
            Graphics gdiobj = Graphics.FromImage(img);
            gdiobj.CompositingQuality = CompositingQuality.HighQuality;
            gdiobj.SmoothingMode = SmoothingMode.HighQuality;
            gdiobj.InterpolationMode = InterpolationMode.HighQualityBicubic;
            gdiobj.PixelOffsetMode = PixelOffsetMode.HighQuality;
            gdiobj.FillRectangle(new SolidBrush(fillColor), 0, 0, outWidth, outHeight);
            //确定矩形的位置
            int x = (outWidth - tmpWidth) / 2;
            int y = (outHeight - tmpHeight) / 2;
            Rectangle destrect = new Rectangle(x, y, tmpWidth, tmpHeight);

            gdiobj.DrawImage(readImg, destrect, 0, 0, readImg.Width, readImg.Height, GraphicsUnit.Pixel);

            return img;
        }

        /// <summary>
        /// Convert and save the image
        /// </summary>
        /// <param name="fileStream">The file stream.</param>
        /// <param name="iWidth">Width</param>
        /// <param name="iHeight">Height</param>
        /// <param name="iQuality">quality.</param>
        /// <param name="fillColor">Color of the fill.</param>
        /// <param name="sSavePath">The save path.</param>
        public static void ZoomAndSaveImage(Stream fileStream, int iWidth, int iHeight, int iQuality, Color fillColor, string sSavePath)
        {
            Image img = GetZoomImage(fileStream, iWidth, iHeight, fillColor);
            SaveImage(sSavePath, img, iQuality);
        }

        /// <summary>
        /// 将图片存储到指定路径
        /// </summary>
        /// <param name="strPath"></param>
        /// <param name="img"></param>
        /// <param name="quality">质量,最高100 </param>
        public static void SaveImage(string strPath, Image img, int quality)
        {
            lock (AsyncLock)
            {
                EncoderParameters ep = new EncoderParameters(1);
                ep.Param[0] = new EncoderParameter(Encoder.Quality, quality);
                ImageCodecInfo ici = GetEncoderInfo("image/jpeg");
                if (ici != null)
                {
                    img.Save(strPath, ici, ep);
                }
                else
                {
                    img.Save(strPath, ImageFormat.Jpeg);
                }
            }
        }

        /// <summary>
        /// 返回指定图片类型的解码器
        /// </summary>
        /// <param name="mimeType"></param>
        /// <returns></returns>
        private static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            ImageCodecInfo[] encoders = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo encoder in encoders)
            {
                if (encoder.MimeType == mimeType)
                    return encoder;
            }
            return null;
        }

        /// <summary>
        /// Adds the sign pic.
        /// </summary>
        /// <param name="ImgStream">The img stream.</param>
        /// <param name="watermarkFilename">The watermark filename.</param>
        /// <param name="watermarkStatus">The watermark status. 0=不使用 1=左上 2=中上 3=右上 4=左中  9=右下</param>
        /// <param name="watermarkTransparency">The watermark transparency. 1--10 10为不透明</param>
        /// <returns></returns>
        public static Image AddSignPic(Stream ImgStream, string watermarkFilename, int watermarkStatus, int watermarkTransparency)
        {
            if (ImgStream == null || ImgStream.Length == 0)
                throw new ArgumentNullException("ImgStream");

            //读取图片
            Bitmap img = new Bitmap(ImgStream);

            //根据原图创建绘图地板
            Graphics g = Graphics.FromImage(img);
            ////设置高质量插值法
            //g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            ////设置高质量,低速度呈现平滑程度
            //g.SmoothingMode = SmoothingMode.HighQuality;

            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            //读取水印图片
            Image watermark = new Bitmap(watermarkFilename);

            //判断水印图片大小 如果水印图片大于地板图片 就不处理
            if (watermark.Height >= img.Height || watermark.Width >= img.Width)
            {
                return img;
            }

            ImageAttributes imageAttributes = new ImageAttributes();
            ColorMap colorMap = new ColorMap();
            colorMap.OldColor = Color.FromArgb(255, 0, 255, 0);
            colorMap.NewColor = Color.FromArgb(0, 0, 0, 0);
            ColorMap[] remapTable = {colorMap};
            imageAttributes.SetRemapTable(remapTable, ColorAdjustType.Bitmap);

            //设置透明度
            float transparency = 0.5F;
            if (watermarkTransparency >= 1 && watermarkTransparency <= 10)
            {
                transparency = (watermarkTransparency/10.0F);
            }
            float[][] colorMatrixElements = {
                                                new float[] {1.0f, 0.0f, 0.0f, 0.0f, 0.0f},
                                                new float[] {0.0f, 1.0f, 0.0f, 0.0f, 0.0f},
                                                new float[] {0.0f, 0.0f, 1.0f, 0.0f, 0.0f},
                                                new float[] {0.0f, 0.0f, 0.0f, transparency, 0.0f},
                                                new float[] {0.0f, 0.0f, 0.0f, 0.0f, 1.0f}
                                            };
            ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);
            imageAttributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

            //设置水印位置
            int xpos = 0;
            int ypos = 0;
            switch (watermarkStatus)
            {
                case 1:
                    xpos = (int) (img.Width*(float) .01);
                    ypos = (int) (img.Height*(float) .01);
                    break;
                case 2:
                    xpos = (int) ((img.Width*(float) .50) - (watermark.Width/2));
                    ypos = (int) (img.Height*(float) .01);
                    break;
                case 3:
                    xpos = (int) ((img.Width*(float) .99) - (watermark.Width));
                    ypos = (int) (img.Height*(float) .01);
                    break;
                case 4:
                    xpos = (int) (img.Width*(float) .01);
                    ypos = (int) ((img.Height*(float) .50) - (watermark.Height/2));
                    break;
                case 5:
                    xpos = (int) ((img.Width*(float) .50) - (watermark.Width/2));
                    ypos = (int) ((img.Height*(float) .50) - (watermark.Height/2));
                    break;
                case 6:
                    xpos = (int) ((img.Width*(float) .99) - (watermark.Width));
                    ypos = (int) ((img.Height*(float) .50) - (watermark.Height/2));
                    break;
                case 7:
                    xpos = (int) (img.Width*(float) .01);
                    ypos = (int) ((img.Height*(float) .99) - watermark.Height);
                    break;
                case 8:
                    xpos = (int) ((img.Width*(float) .50) - (watermark.Width/2));
                    ypos = (int) ((img.Height*(float) .99) - watermark.Height);
                    break;
                case 9:
                    xpos = (int) ((img.Width*(float) .99) - (watermark.Width));
                    ypos = (int) ((img.Height*(float) .99) - watermark.Height);
                    break;
                default:
                    break;
            }

            g.DrawImage(watermark, new Rectangle(xpos, ypos, watermark.Width, watermark.Height), 0, 0, watermark.Width,
                        watermark.Height, GraphicsUnit.Pixel, imageAttributes);

            //g.Dispose();
            //watermark.Dispose();
            //imageAttributes.Dispose();

            return img;
        }
        
        /// <summary>
        /// Adds the sign text.
        /// </summary>
        /// <param name="img">The img.</param>
        /// <param name="filename">The filename.</param>
        /// <param name="watermarkText">The watermark text.</param>
        /// <param name="watermarkStatus">The watermark status. 0=不使用 1=左上 2=中上 3=右上 4=左中  9=右下</param>
        /// <param name="quality">The quality.</param>
        /// <param name="fontname">The fontname.</param>
        /// <param name="fontsize">The fontsize.</param>
        public static void AddSignText(Image img, string filename, string watermarkText, int watermarkStatus, int quality, string fontname, int fontsize)
        {
            //System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(img);
            //    .FromFile(filename);
            Graphics g = Graphics.FromImage(img);
            Font drawFont = new Font(fontname, fontsize, FontStyle.Regular, GraphicsUnit.Pixel);
            SizeF crSize;
            crSize = g.MeasureString(watermarkText, drawFont);

            float xpos = 0;
            float ypos = 0;

            switch (watermarkStatus)
            {
                case 1:
                    xpos = img.Width*(float) .01;
                    ypos = img.Height*(float) .01;
                    break;
                case 2:
                    xpos = (img.Width*(float) .50) - (crSize.Width/2);
                    ypos = img.Height*(float) .01;
                    break;
                case 3:
                    xpos = (img.Width*(float) .99) - crSize.Width;
                    ypos = img.Height*(float) .01;
                    break;
                case 4:
                    xpos = img.Width*(float) .01;
                    ypos = (img.Height*(float) .50) - (crSize.Height/2);
                    break;
                case 5:
                    xpos = (img.Width*(float) .50) - (crSize.Width/2);
                    ypos = (img.Height*(float) .50) - (crSize.Height/2);
                    break;
                case 6:
                    xpos = (img.Width*(float) .99) - crSize.Width;
                    ypos = (img.Height*(float) .50) - (crSize.Height/2);
                    break;
                case 7:
                    xpos = img.Width*(float) .01;
                    ypos = (img.Height*(float) .99) - crSize.Height;
                    break;
                case 8:
                    xpos = (img.Width*(float) .50) - (crSize.Width/2);
                    ypos = (img.Height*(float) .99) - crSize.Height;
                    break;
                case 9:
                    xpos = (img.Width*(float) .99) - crSize.Width;
                    ypos = (img.Height*(float) .99) - crSize.Height;
                    break;
                default:
                    break;
            }

            //            System.Drawing.StringFormat StrFormat = new System.Drawing.StringFormat();
            //            StrFormat.Alignment = System.Drawing.StringAlignment.Center;
            //
            //            g.DrawString(watermarkText, drawFont, new System.Drawing.SolidBrush(System.Drawing.Color.White), xpos + 1, ypos + 1, StrFormat);
            //            g.DrawString(watermarkText, drawFont, new System.Drawing.SolidBrush(System.Drawing.Color.Black), xpos, ypos, StrFormat);
            g.DrawString(watermarkText, drawFont, new SolidBrush(Color.White), xpos + 1, ypos + 1);
            g.DrawString(watermarkText, drawFont, new SolidBrush(Color.Black), xpos, ypos);

            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            ImageCodecInfo ici = null;
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.MimeType.IndexOf("jpeg") > -1)
                {
                    ici = codec;
                }
            }
            EncoderParameters encoderParams = new EncoderParameters();
            long[] qualityParam = new long[1];
            if (quality < 0 || quality > 100)
            {
                quality = 80;
            }
            qualityParam[0] = quality;

            EncoderParameter encoderParam = new EncoderParameter(Encoder.Quality, qualityParam);
            encoderParams.Param[0] = encoderParam;

            if (ici != null)
            {
                img.Save(filename, ici, encoderParams);
            }
            else
            {
                img.Save(filename);
            }
            g.Dispose();
            //bmp.Dispose();
            img.Dispose();
        }
    }
}
