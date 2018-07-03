using System;
using System.Drawing;

namespace Zdd.Utility
{
    public class RGB
    {
        /// <summary>
        /// Index of red component.
        /// </summary>
        public const short R = 2;

        /// <summary>
        /// Index of green component.
        /// </summary>
        public const short G = 1;

        /// <summary>
        /// Index of blue component.
        /// </summary>
        public const short B = 0;

        /// <summary>
        /// Index of alpha component for ARGB images.
        /// </summary>
        public const short A = 3;

        /// <summary>
        /// Red component.
        /// </summary>
        public byte Red;

        /// <summary>
        /// Green component.
        /// </summary>
        public byte Green;

        /// <summary>
        /// Blue component.
        /// </summary>
        public byte Blue;

        /// <summary>
        /// Alpha component.
        /// </summary>
        public byte Alpha;

        /// <summary>
        /// <see cref="System.Drawing.Color">Color</see> value of the class.
        /// </summary>
        public System.Drawing.Color Color
        {
            get { return Color.FromArgb(Alpha, Red, Green, Blue); }
            set
            {
                Red = value.R;
                Green = value.G;
                Blue = value.B;
                Alpha = value.A;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RGB"/> class.
        /// </summary>
        public RGB()
        {
            Red = 0;
            Green = 0;
            Blue = 0;
            Alpha = 255;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RGB"/> class.
        /// </summary>
        /// 
        /// <param name="red">Red component.</param>
        /// <param name="green">Green component.</param>
        /// <param name="blue">Blue component.</param>
        /// 
        public RGB(byte red, byte green, byte blue)
        {
            this.Red = red;
            this.Green = green;
            this.Blue = blue;
            this.Alpha = 255;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RGB"/> class.
        /// </summary>
        /// 
        /// <param name="red">Red component.</param>
        /// <param name="green">Green component.</param>
        /// <param name="blue">Blue component.</param>
        /// <param name="alpha">Alpha component.</param>
        /// 
        public RGB(byte red, byte green, byte blue, byte alpha)
        {
            this.Red = red;
            this.Green = green;
            this.Blue = blue;
            this.Alpha = alpha;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RGB"/> class.
        /// </summary>
        /// 
        /// <param name="color">Initialize from specified <see cref="System.Drawing.Color">color.</see></param>
        /// 
        public RGB(System.Drawing.Color color)
        {
            this.Red = color.R;
            this.Green = color.G;
            this.Blue = color.B;
            this.Alpha = color.A;
        }
    }
}
