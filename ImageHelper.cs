using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;

/********************************************************************
	created:	2017/03/08
	filename: 	ImageHelper.cs
	file base:	ImageHelper
	file ext:	cs
	author:		ZDD
	purpose:	Image Process Utils,
    modify              date
     init version      2017/03/08
*********************************************************************/
namespace Zdd.Utility
{
    public class ImageHelper
    {
        /// <summary>
        /// 创建图片
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static Image ReBit(string url)
        {
            try
            {
                //string strCompress = "compress.jpg";
                //bool bCompress = GetThumbnail(url, strCompress, 10);
                if (true)
                {
                    using (BinaryReader reader = new BinaryReader(File.Open(url, FileMode.Open, FileAccess.Read, FileShare.Read)))
                    {
                        try
                        {
                            //FileInfo fi = new FileInfo(url);
                            //byte[] bytes = reader.ReadBytes((int)fi.Length);
                            //reader.Close();
                            //reader.Dispose();
                            using (MemoryStream ms = new MemoryStream())
                            {
                                byte[] buffer = new byte[10240];
                                while (true)
                                {
                                    int read = reader.Read(buffer, 0, 10240);
                                    if (read == 0)
                                        break;
                                    ms.Write(buffer, 0, read);
                                }
                                Image img = Image.FromStream(ms);
                                return img;
                            }

                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                }
                else
                {
                    //Log.Kernel.LogError("压缩失败.");
                }
            }
            catch (Exception ee)
            {
                throw ee;
            }
        }
        /// <summary>
        /// 按比例缩小图片，自动计算高度
        /// </summary>
        /// <param name="strOldPic">源图文件名(包括路径)</param>
        /// <param name="strNewPic">缩小后保存为文件名(包括路径)</param>
        /// <param name="intWidth">缩小至宽度</param>
        public static void SmallPicWidth(string strOldPic, string strNewPic, int intWidth)
        {

            System.Drawing.Bitmap objPic, objNewPic;
            try
            {
                decimal deWidth = (decimal)intWidth;
                objPic = new System.Drawing.Bitmap(strOldPic);
                decimal deHeight = (decimal)objPic.Height;
                decimal deWidth2 = (decimal)objPic.Width;
                int intHeight = (int)Math.Round((deWidth / deWidth2) * deHeight, 2);

                objNewPic = new System.Drawing.Bitmap(objPic, intWidth, intHeight);
                objNewPic.Save(strNewPic);
                objNewPic.Dispose();

            }
            catch { }
            finally
            {
                objPic = null;
                objNewPic = null;
            }
        }
        /// <summary>
        /// 压缩图像
        /// </summary>
        /// <param name="srcImg">源路径</param>
        /// <param name="dstImg">目的</param>
        /// <param name="flag">压缩比例1-100</param>
        /// <returns></returns>
        public static bool GetThumbnail(string srcImg, string dstImg, int flag)
        {
            try
            {
                using (Image iSource = Image.FromFile(srcImg))
                {
                    ImageFormat format = iSource.RawFormat;
                    EncoderParameters ep = new EncoderParameters();
                    long[] qy = new long[1];
                    qy[0] = flag;
                    EncoderParameter eParm = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qy);
                    ep.Param[0] = eParm;

                    ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();
                    ImageCodecInfo jpegICIinfo = null;
                    for (int x = 0; x < arrayICI.Length; x++)
                    {
                        if (arrayICI[x].FormatDescription.Equals("JPEG"))
                        {
                            jpegICIinfo = arrayICI[x];
                            break;
                        }
                    }
                    if (null != jpegICIinfo)
                    {
                        iSource.Save(dstImg, jpegICIinfo, ep);
                    }
                    else
                    {
                        iSource.Save(dstImg, format);
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 合成图像
        /// </summary>
        /// <param name="image1"></param>
        /// <param name="image2"></param>
        /// <param name="way">true:横向合成;false:纵向合成</param>
        /// <returns></returns>
        public static Bitmap GetMegerImage(Image image1, Image image2, bool way = true)
        {
            int width = image1.Width + image2.Width;
            int height = image1.Height + image2.Height;
            Bitmap bitmap;
            if (way)
            {
                if (image1.Height > image2.Height)
                {
                    height = image1.Height;
                }
                else
                {
                    height = image2.Height;
                }
                bitmap = new Bitmap(width, height);
                Graphics graphics = Graphics.FromImage(bitmap);
                Rectangle destRect = new Rectangle(0, 0, image1.Width, image1.Height);
                graphics.DrawImage(image1, destRect, 0, 0, image1.Width, image1.Height, GraphicsUnit.Pixel);
                Rectangle destRect2 = new Rectangle(image1.Width, 0, image2.Width, image2.Height);
                graphics.DrawImage(image2, destRect2, 0, 0, image2.Width, image2.Height, GraphicsUnit.Pixel);
                graphics.Flush();
                graphics.Dispose();
            }
            else
            {
                if (image1.Width > image2.Height)
                {
                    width = image1.Width;
                }
                else
                {
                    width = image2.Width;
                }
                bitmap = new Bitmap(width, height);
                Graphics graphics = Graphics.FromImage(bitmap);
                Rectangle destRect = new Rectangle(0, 0, image1.Width, image1.Height);
                graphics.DrawImage(image1, destRect, 0, 0, image1.Width, image1.Height, GraphicsUnit.Pixel);
                Rectangle destRect2 = new Rectangle(0, image1.Height, image2.Width, image2.Height);
                graphics.DrawImage(image2, destRect2, 0, 0, image2.Width, image2.Height, GraphicsUnit.Pixel);
                graphics.Flush();
                graphics.Dispose();
            }
            return bitmap;
        }

        /// <summary>
        /// 毫米换算成像素
        /// </summary>
        /// <param name="millimeter"></param>
        /// <param name="isWidth"></param>
        /// <returns></returns>
        public static double MillimetersToPixels(double millimeter, bool isWidth = true)
        {
            double dResult = 0;
            /*
            SetProcessDPIAware(); //重要
            IntPtr screenDC = GetDC(IntPtr.Zero);
            int dpi_x = GetDeviceCaps(screenDC, LOGPIXELSX);
            int dpi_y = GetDeviceCaps(screenDC, LOGPIXELSY);
            //象素数 / DPI = 英寸数
            //英寸数 * 25.4 = 毫米数
            ReleaseDC(IntPtr.Zero, screenDC);
            if (isWidth)
            {
                dResult = millimeter * dpi_x / 25.4;
            }
            else
            {
                dResult = millimeter * dpi_y / 25.4;
            }*/
            using (Graphics gfx = Graphics.FromHwnd(IntPtr.Zero))
            {
                if (isWidth)
                {
                    dResult = (gfx.DpiX / 25.4f * millimeter);
                }
                else
                {
                    dResult = (gfx.DpiY / 25.4f * millimeter);
                }
            }
            return dResult;
        }
        /// <summary>
        /// 像素换算成毫米
        /// </summary>
        /// <param name="Pixels">像素</param>
        /// <param name="isWidth">是否宽度</param>
        /// <returns></returns>
        public static double PixelsToMillimeters(double Pixels, bool isWidth = true)
        {
            double dResult = 0;
            using (Graphics gfx = Graphics.FromHwnd(IntPtr.Zero))
            {
                if (isWidth)
                {
                    dResult = Pixels * 25.4f / gfx.DpiX;
                }
                else
                {
                    dResult = Pixels * 25.4f / gfx.DpiY;
                }
            }
            return dResult;
        }
        #region [native api]

        [DllImport("gdi32.dll")]
        private static extern int GetDeviceCaps(IntPtr hdc, int nIndex);
        [DllImport("user32.dll")]
        static extern IntPtr GetDC(IntPtr ptr);
        [DllImport("user32.dll", EntryPoint = "ReleaseDC")]
        public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDc);
        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateDC(
        string lpszDriver, // driver name
        string lpszDevice, // device name
        string lpszOutput, // not used; should be NULL
        Int64 lpInitData // optional printer data
        );
        [DllImport("user32.dll")]
        internal static extern bool SetProcessDPIAware();
        #region const
        const int DRIVERVERSION = 0;
        const int TECHNOLOGY = 2;
        const int HORZSIZE = 4;
        const int VERTSIZE = 6;
        const int HORZRES = 8;
        const int VERTRES = 10;
        const int BITSPIXEL = 12;
        const int PLANES = 14;
        const int NUMBRUSHES = 16;
        const int NUMPENS = 18;
        const int NUMMARKERS = 20;
        const int NUMFONTS = 22;
        const int NUMCOLORS = 24;
        const int PDEVICESIZE = 26;
        const int CURVECAPS = 28;
        const int LINECAPS = 30;
        const int POLYGONALCAPS = 32;
        const int TEXTCAPS = 34;
        const int CLIPCAPS = 36;
        const int RASTERCAPS = 38;
        const int ASPECTX = 40;
        const int ASPECTY = 42;
        const int ASPECTXY = 44;
        const int SHADEBLENDCAPS = 45;
        const int LOGPIXELSX = 88;
        const int LOGPIXELSY = 90;
        const int SIZEPALETTE = 104;
        const int NUMRESERVED = 106;
        const int COLORRES = 108;
        const int PHYSICALWIDTH = 110;
        const int PHYSICALHEIGHT = 111;
        const int PHYSICALOFFSETX = 112;
        const int PHYSICALOFFSETY = 113;
        const int SCALINGFACTORX = 114;
        const int SCALINGFACTORY = 115;
        const int VREFRESH = 116;
        const int DESKTOPVERTRES = 117;
        const int DESKTOPHORZRES = 118;
        const int BLTALIGNMENT = 119;
        #endregion
        #endregion

        /*
         * 平移 旋转 缩放
         */
        /// <summary>
        /// 对一个坐标点按照一个中心进行旋转
        /// </summary>
        /// <param name="center">中心点</param>
        /// <param name="p1">要旋转的点</param>
        /// <param name="angle">旋转角度，笛卡尔直角坐标</param>
        /// <returns></returns>
        public static Point PointRotate(Point center, Point p1, double angle)
        {
            Point tmp = new Point();
            double angleHude = angle * Math.PI / 180;/*角度变成弧度*/
            double x1 = (p1.X - center.X) * Math.Cos(angleHude) - (p1.Y - center.Y) * Math.Sin(angleHude) + center.X;
            double y1 = -(p1.X - center.X) * Math.Sin(angleHude) + (p1.Y - center.Y) * Math.Cos(angleHude) + center.Y;
            tmp.X = Convert.ToInt32(x1);
            tmp.Y = Convert.ToInt32(y1);
            return tmp;
        }
        /// <summary>
        /// 屏幕坐标与迪卡尔坐标互相转换
        /// </summary>
        /// <param name="pt">要转换的点</param>
        /// <param name="nMaxWidth">最大宽度X</param>
        /// <param name="nMaxHeight">最大高度Y</param>
        /// <returns></returns>
        public static Point ScreenToCartesian(Point pt, int nMaxWidth, int nMaxHeight)
        {
            //屏幕坐标以左上角为原点
            //迪卡尔坐标以左下角为原点
            int ptx = pt.X;
            int pty = nMaxHeight - pt.Y;
            return new Point(ptx, pty);
        }
        /// <summary>
        /// 坐标移动、旋转
        /// </summary>
        /// <param name="centerPt"></param>
        /// <param name="rotatePt"></param>
        /// <param name="angle">角度</param>
        /// <returns></returns>
        public static Point RotatePoint(Point centerPt, Point rotatePt, double angle)
        {
            if (angle == 0.000000F)
            {
                return rotatePt;
            }
            var radian = 180.0 * Math.PI / angle;
            int x = (int)((double)(rotatePt.X - centerPt.X) * Math.Cos(radian) - (double)(rotatePt.Y - centerPt.Y) * Math.Sin(radian));
            int y = (int)((double)(rotatePt.Y - centerPt.Y) * Math.Cos(radian) + (double)(rotatePt.X - centerPt.X) * Math.Sin(radian));
            Point point = new Point(centerPt.X + x, centerPt.Y + y);
            return point;
        }
        /// <summary>
        /// 点旋转
        /// </summary>
        /// <param name="centerPt">中心点</param>
        /// <param name="rotatePt">旋转的点</param>
        /// <param name="angle">旋转角度</param>
        /// <param name="outPt">返回点</param>
        /// <returns></returns>
        public static void RotatePoint(Point centerPt, Point rotatePt, double angle, out Point outPt)
        {
            if (angle == 0.000000F)
            {
                outPt = rotatePt;
                return;
            }
            var radian = angle;//180.0 * Math.PI / angle;
            int newx = (int)((double)(rotatePt.X - centerPt.X) * Math.Cos(radian) - (double)(rotatePt.Y - centerPt.Y) * Math.Sin(radian));
            int newy = (int)((double)(rotatePt.Y - centerPt.Y) * Math.Cos(radian) + (double)(rotatePt.X - centerPt.X) * Math.Sin(radian));
            Point newPt = new Point(centerPt.X + newx, centerPt.Y + newy);
            outPt = newPt;
        }
        /// <summary>
        /// 根据图像获取光标
        /// </summary>
        /// <param name="cursor"></param>
        /// <param name="hotPoint"></param>
        /// <returns></returns>
        public  static Cursor GetCursor(Bitmap cursor, Point hotPoint)
        {
            int hotX = hotPoint.X - 40;
            int hotY = hotPoint.Y - 38;
            Bitmap myNewCursor = new Bitmap(64, 64);
            Graphics g = Graphics.FromImage(myNewCursor);
            g.Clear(Color.FromArgb(0, 0, 0, 0));
            g.DrawImage(cursor, hotX, hotY, cursor.Width, cursor.Height);
            Cursor  mycursor= new Cursor(myNewCursor.GetHicon());
            g.Dispose();
            myNewCursor.Dispose();
            return mycursor;
        }
       /// <summary>
       /// 旋转图像
       /// </summary>
       /// <param name="srcImage">源图像</param>
       /// <param name="angle">旋转角度</param>
       /// <param name="x">旋转中心点x坐标</param>
       /// <param name="y">旋转中心点y坐标</param>
       /// <param name="width">目标宽度</param>
       /// <param name="height">目标高高度</param>
       /// <param name="dstName">目的图像文件名</param>
       /// <returns></returns>
        public static  Image RotateImage(Image srcImage, float angle, int x, int y, int width, int height, string dstName)
        {
            Bitmap dsImage = new Bitmap(srcImage.Width * 2, srcImage.Height * 2);
            Graphics g = Graphics.FromImage(dsImage);
            g.InterpolationMode = InterpolationMode.Bilinear;
            g.SmoothingMode = SmoothingMode.HighQuality;
            Rectangle rect = new Rectangle(0, 0, srcImage.Width * 2, srcImage.Height * 2);
            Point center = new Point(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);
            g.TranslateTransform((float)center.X, (float)center.Y);
            g.RotateTransform(360f - angle);
            g.TranslateTransform((float)(-(float)x), (float)(-(float)y));
            Rectangle rect2 = new Rectangle(0, 0, srcImage.Width, srcImage.Height);
            g.DrawImage(srcImage, rect2);
            g.ResetTransform();
            g.Save();
            g.Dispose();
            dsImage.Save("RotateImage.jpg", ImageFormat.Jpeg);
            double r = Math.Sqrt((double)(x * x + y * y));
            double vsin = (double)y / r;
            double vangle = Math.Asin(vsin);
            double vangle2 = Math.Abs(vangle * 180.0 / Math.PI);
            Rectangle rectCut = new Rectangle(srcImage.Width, srcImage.Height, width, height);
            Bitmap bitmapcut = dsImage.Clone(rectCut, PixelFormat.Format24bppRgb);
            bitmapcut.RotateFlip(RotateFlipType.Rotate180FlipNone);
            bitmapcut.Save(dstName, ImageFormat.Jpeg);
            return dsImage;
        }
        /// <summary>
        /// 获取两点之前的距离
        /// </summary>
        /// <param name="P1"></param>
        /// <param name="P2"></param>
        /// <returns></returns>
        public static double GetPointDistance(Point P1, Point P2)
        {
            return Math.Sqrt(
                Math.Pow((double)(Math.Max(P2.X, P1.X) - Math.Min(P2.X, P1.X)), 2.0) +
                Math.Pow((double)(Math.Max(P2.Y, P1.Y) - Math.Min(P2.Y, P1.Y)), 2.0));
        }
        /// <summary>
        /// 点旋转、移动
        /// </summary>
        /// <param name="angle">旋转角度</param>
        /// <param name="center">中心点坐标</param>
        /// <param name="move">移动的坐标</param>
        /// <returns></returns>
        public static Point RotatePoint(double angle, Point center, Point move)
        {
            double radian = angle / 180.0 * Math.PI;
            int newx = (int)((double)(move.X - center.X) * Math.Cos(radian) - (double)(move.Y - center.Y) * Math.Sin(radian));
            int newy = (int)((double)(move.Y - center.Y) * Math.Cos(radian) + (double)(move.X - center.X) * Math.Sin(radian));
            Point newPt = new Point(center.X + newx, center.Y + newy);
            return newPt;
        }
        /// <summary>
        /// 计算用印坐标
        /// </summary>
        /// <param name="mousex">原图用印坐标x</param>
        /// <param name="mousey">原图用印坐标y</param>
        /// <param name="ptOut">输出坐标</param>
        /// <param name="verifyPoists">校准点坐标 左上，右上，右下，左下，中间</param>
        /// <returns>成功返回0, 
        /// -1 x too big
        /// -2 x too small
        /// -3 y too big
        /// -4 y too small
        /// </returns>
        public static int GetSealPoint(int mousex, int mousey, out Point ptOut, List<Point> verifyPoists = null,
            int devMinX = 3, int devMinY = 60, int devMaxX = 260, int devMaxY = 230)
        {
            bool bNeedRotate = true;//需要旋转角度
            ptOut = new Point(0, 0);
            #region [设备物理坐标区间]
            int iDevXMin = devMinX;
            int iDevXMax = devMaxX;
            int iDevYMin = devMinY;
            int iDevYMax = devMaxY;
            #endregion
            //默认校准点数据
            Point p1 = new Point(290, 228);//左上角
            Point p2 = new Point(2421, 229);//右上角
            Point p3 = new Point(297, 1649);//左下角
            Point p4 = new Point(2437, 1648);//右下角
            
            if (null != verifyPoists)
            {
                if (verifyPoists.Count >= 4
                    && verifyPoists[0] != null
                    && verifyPoists[1] != null
                    && verifyPoists[2] != null
                    && verifyPoists[3] != null
                    )
                {
                    //校准点不能小于等于0
                    var count = verifyPoists.Count(p => p.X <= 0 || p.Y <= 0);
                    if (count == 0)
                    {//左上，右上，右下，左下，中间
                        p1 = verifyPoists[0];//左上角
                        p2 = verifyPoists[1];//右上角
                        p3 = verifyPoists[3];//左下角
                        p4 = verifyPoists[2];//右下角
                    }
                }
            }
            Point px = new Point(mousex, mousey);
            int result = 0;
            int error = 0;
            if (mousex >= Math.Max(p2.X, p4.X))//
            {
                //return -1;
                error = -1;
                //px.X = Math.Max(p2.X, p4.X);//x too big
            }
            if (mousex <= Math.Min(p1.X, p3.X))
            {
                //return -2;
                error = -2;
                //px.X = Math.Min(p1.X, p3.X);//x too little
            }
            if (mousey <= Math.Min(p1.Y, p2.Y))
            {
                //return -3;
                error = -3;
                //px.Y = Math.Min(p1.Y, p2.Y);// y too little
            }
            if (mousey >= Math.Max(p3.Y, p4.Y))
            {
                error = -4;
                //return -4;
                //px.Y = Math.Max(p3.Y, p4.Y);//y too big
            }
            Point ptRT = p2;//新右上角
            Point ptLD=p3;//新左下角
            Point ptRB=p4;//新右下角
            Point ptNew=px;//新用印坐标点

            if (bNeedRotate)
            {
                //p1 p2之间的距离
                double dWidth = Math.Sqrt((double)(Math.Abs(p1.X - p2.X) * Math.Abs(p1.X - p2.X) + Math.Abs(p1.Y - p2.Y) * Math.Abs(p1.Y - p2.Y)));
                double dsin = (double)Math.Abs(p2.Y - p1.Y) / dWidth;//正弦=股长/弦长
                double dangle = Math.Asin(dsin);//角度
                if (p1.Y >= p2.Y)//
                {
                    dangle = -dangle;
                }
                RotatePoint(p1, p2, dangle, out ptRT);
                RotatePoint(p1, p3, dangle, out ptLD);
                RotatePoint(p1, p4, dangle, out ptRB);
                RotatePoint(p1, px, dangle, out ptNew);
            }
            if (error != 0)
            {//坐标越界,使用比例计算
                var ratex = (double)(iDevXMax - iDevXMin) / (double)Math.Abs(ptRT.X - p1.X);
                var ratey = (double)(iDevYMax - iDevYMin) / (double)Math.Abs(ptLD.Y - p1.Y);

                var tmpx = (double)Math.Abs(ptNew.X - ptRT.X) * ratex;
                var tmpy = (double)Math.Abs(ptNew.Y - p1.Y) * ratey;
                var x = iDevXMin + tmpx;
                var y = iDevYMin + tmpy;
                ptOut = new Point(Convert.ToInt32(x), Convert.ToInt32(y));
                return result;
            }

            //dXPercent=(用印点x-右上角x)/(右上角x-左上角x)宽
            double dXPercent = (double)Math.Abs(ptNew.X - ptRT.X) / (double)Math.Abs(ptRT.X - p1.X);
            //dYPercent=y方向比例(用印点y-左上角y)/(左下角y-左上角y)高
            double dYPercent = (double)Math.Abs(ptNew.Y - p1.Y) / (double)Math.Abs(ptLD.Y - p1.Y);
            int iDevX = iDevXMin + (int)((double)(iDevXMax - iDevXMin) * dXPercent);
            int iDevY = iDevYMin + (int)((double)(iDevYMax - iDevYMin) * dYPercent);
            ptOut = new Point(iDevX, iDevY);
            return result;
        }//end GetSealPoint
        #region [extra]
        public static bool IsGrayscale(Bitmap image)
        {
            bool ret = false;

            // check pixel format
            if (image.PixelFormat == PixelFormat.Format8bppIndexed)
            {
                ret = true;
                // check palette
                ColorPalette cp = image.Palette;
                Color c;
                // init palette
                for (int i = 0; i < 256; i++)
                {
                    c = cp.Entries[i];
                    if ((c.R != i) || (c.G != i) || (c.B != i))
                    {
                        ret = false;
                        break;
                    }
                }
            }
            return ret;
        }

        public static Bitmap CreateGrayscaleImage(int width, int height)
        {
            // create new image
            Bitmap image = new Bitmap(width, height, PixelFormat.Format8bppIndexed);
            // set palette to grayscale
            SetGrayscalePalette(image);
            // return new image
            return image;
        }
        public static void SetGrayscalePalette(Bitmap image)
        {
            // check pixel format
            if (image.PixelFormat != PixelFormat.Format8bppIndexed)
                throw new Exception("Source image is not 8 bpp image.");

            // get palette
            ColorPalette cp = image.Palette;
            // init palette
            for (int i = 0; i < 256; i++)
            {
                cp.Entries[i] = Color.FromArgb(i, i, i);
            }
            // set palette back
            image.Palette = cp;
        }
        public static Bitmap Clone(Bitmap source, PixelFormat format)
        {
            // copy image if pixel format is the same
            if (source.PixelFormat == format)
                return Clone(source);

            int width = source.Width;
            int height = source.Height;

            // create new image with desired pixel format
            Bitmap bitmap = new Bitmap(width, height, format);

            // draw source image on the new one using Graphics
            Graphics g = Graphics.FromImage(bitmap);
            g.DrawImage(source, 0, 0, width, height);
            g.Dispose();

            return bitmap;
        }
        public static Bitmap Clone(Bitmap source)
        {
            // lock source bitmap data
            BitmapData sourceData = source.LockBits(
                new Rectangle(0, 0, source.Width, source.Height),
                ImageLockMode.ReadOnly, source.PixelFormat);

            // create new image
            Bitmap destination = Clone(sourceData);

            // unlock source image
            source.UnlockBits(sourceData);

            //
            if (
                (source.PixelFormat == PixelFormat.Format1bppIndexed) ||
                (source.PixelFormat == PixelFormat.Format4bppIndexed) ||
                (source.PixelFormat == PixelFormat.Format8bppIndexed) ||
                (source.PixelFormat == PixelFormat.Indexed))
            {
                ColorPalette srcPalette = source.Palette;
                ColorPalette dstPalette = destination.Palette;

                int n = srcPalette.Entries.Length;

                // copy pallete
                for (int i = 0; i < n; i++)
                {
                    dstPalette.Entries[i] = srcPalette.Entries[i];
                }

                destination.Palette = dstPalette;
            }

            return destination;
        }
        public static Bitmap Clone(BitmapData sourceData)
        {
            // get source image size
            int width = sourceData.Width;
            int height = sourceData.Height;

            // create new image
            Bitmap destination = new Bitmap(width, height, sourceData.PixelFormat);

            // lock destination bitmap data
            BitmapData destinationData = destination.LockBits(
                new Rectangle(0, 0, width, height),
                ImageLockMode.ReadWrite, destination.PixelFormat);

            SystemTools.CopyUnmanagedMemory(destinationData.Scan0, sourceData.Scan0, height * sourceData.Stride);

            // unlock destination image
            destination.UnlockBits(destinationData);

            return destination;
        }
        public static void FormatImage(ref Bitmap image)
        {
            if (
                (image.PixelFormat != PixelFormat.Format24bppRgb) &&
                (image.PixelFormat != PixelFormat.Format32bppRgb) &&
                (image.PixelFormat != PixelFormat.Format32bppArgb) &&
                (image.PixelFormat != PixelFormat.Format48bppRgb) &&
                (image.PixelFormat != PixelFormat.Format64bppArgb) &&
                (image.PixelFormat != PixelFormat.Format16bppGrayScale) &&
                (IsGrayscale(image) == false)
                )
            {
                Bitmap tmp = image;
                // convert to 24 bits per pixel
                image = Clone(tmp, PixelFormat.Format24bppRgb);
                // delete old image
                tmp.Dispose();
            }
        }
        public static System.Drawing.Bitmap FromFile(string fileName)
        {
            Bitmap loadedImage = null;
            FileStream stream = null;

            try
            {
                // read image to temporary memory stream
                stream = File.OpenRead(fileName);
                MemoryStream memoryStream = new MemoryStream();

                byte[] buffer = new byte[10000];
                while (true)
                {
                    int read = stream.Read(buffer, 0, 10000);

                    if (read == 0)
                        break;

                    memoryStream.Write(buffer, 0, read);
                }

                loadedImage = (Bitmap)Bitmap.FromStream(memoryStream);
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                    stream.Dispose();
                }
            }

            return loadedImage;
        }

        public static Bitmap Convert16bppTo8bpp(Bitmap bimap)
        {
            Bitmap newImage = null;
            int layers = 0;

            // get image size
            int width = bimap.Width;
            int height = bimap.Height;

            // create new image depending on source image format
            switch (bimap.PixelFormat)
            {
                case PixelFormat.Format16bppGrayScale:
                    // create new grayscale image
                    newImage = CreateGrayscaleImage(width, height);
                    layers = 1;
                    break;

                case PixelFormat.Format48bppRgb:
                    // create new color 24 bpp image
                    newImage = new Bitmap(width, height, PixelFormat.Format24bppRgb);
                    layers = 3;
                    break;

                case PixelFormat.Format64bppArgb:
                    // create new color 32 bpp image
                    newImage = new Bitmap(width, height, PixelFormat.Format32bppArgb);
                    layers = 4;
                    break;

                case PixelFormat.Format64bppPArgb:
                    // create new color 32 bpp image
                    newImage = new Bitmap(width, height, PixelFormat.Format32bppPArgb);
                    layers = 4;
                    break;

                default:
                    throw new Exception("Invalid pixel format of the source image.");
            }

            // lock both images
            BitmapData sourceData = bimap.LockBits(new Rectangle(0, 0, width, height),
                ImageLockMode.ReadOnly, bimap.PixelFormat);
            BitmapData newData = newImage.LockBits(new Rectangle(0, 0, width, height),
                ImageLockMode.ReadWrite, newImage.PixelFormat);

            unsafe
            {
                // base pointers
                byte* sourceBasePtr = (byte*)sourceData.Scan0.ToPointer();
                byte* newBasePtr = (byte*)newData.Scan0.ToPointer();
                // image strides
                int sourceStride = sourceData.Stride;
                int newStride = newData.Stride;

                for (int y = 0; y < height; y++)
                {
                    ushort* sourcePtr = (ushort*)(sourceBasePtr + y * sourceStride);
                    byte* newPtr = (byte*)(newBasePtr + y * newStride);

                    for (int x = 0, lineSize = width * layers; x < lineSize; x++, sourcePtr++, newPtr++)
                    {
                        *newPtr = (byte)(*sourcePtr >> 8);
                    }
                }
            }

            // unlock both image
            bimap.UnlockBits(sourceData);
            newImage.UnlockBits(newData);

            return newImage;
        }
        public static Bitmap Convert8bppTo16bpp(Bitmap bimap)
        {
            Bitmap newImage = null;
            int layers = 0;

            // get image size
            int width = bimap.Width;
            int height = bimap.Height;

            // create new image depending on source image format
            switch (bimap.PixelFormat)
            {
                case PixelFormat.Format8bppIndexed:
                    // create new grayscale image
                    newImage = new Bitmap(width, height, PixelFormat.Format16bppGrayScale);
                    layers = 1;
                    break;

                case PixelFormat.Format24bppRgb:
                    // create new color 48 bpp image
                    newImage = new Bitmap(width, height, PixelFormat.Format48bppRgb);
                    layers = 3;
                    break;

                case PixelFormat.Format32bppArgb:
                    // create new color 64 bpp image
                    newImage = new Bitmap(width, height, PixelFormat.Format64bppArgb);
                    layers = 4;
                    break;

                case PixelFormat.Format32bppPArgb:
                    // create new color 64 bpp image
                    newImage = new Bitmap(width, height, PixelFormat.Format64bppPArgb);
                    layers = 4;
                    break;

                default:
                    throw new Exception("Invalid pixel format of the source image.");
            }

            // lock both images
            BitmapData sourceData = bimap.LockBits(new Rectangle(0, 0, width, height),
                ImageLockMode.ReadOnly, bimap.PixelFormat);
            BitmapData newData = newImage.LockBits(new Rectangle(0, 0, width, height),
                ImageLockMode.ReadWrite, newImage.PixelFormat);

            unsafe
            {
                // base pointers
                byte* sourceBasePtr = (byte*)sourceData.Scan0.ToPointer();
                byte* newBasePtr = (byte*)newData.Scan0.ToPointer();
                // image strides
                int sourceStride = sourceData.Stride;
                int newStride = newData.Stride;

                for (int y = 0; y < height; y++)
                {
                    byte* sourcePtr = (byte*)(sourceBasePtr + y * sourceStride);
                    ushort* newPtr = (ushort*)(newBasePtr + y * newStride);

                    for (int x = 0, lineSize = width * layers; x < lineSize; x++, sourcePtr++, newPtr++)
                    {
                        *newPtr = (ushort)(*sourcePtr << 8);
                    }
                }
            }

            // unlock both image
            bimap.UnlockBits(sourceData);
            newImage.UnlockBits(newData);

            return newImage;
        }
        #endregion
    }

}
