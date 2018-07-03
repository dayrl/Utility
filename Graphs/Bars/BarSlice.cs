using System.Drawing;

namespace Zdd.Utility.Graphs
{
    /// <summary>
    /// Summary description for BarSlice.
    /// </summary>
    public class BarSlice
    {
        /// <summary>
        /// color
        /// </summary>
        protected Color color;

        /// <summary>
        /// colorGradient
        /// </summary>
        protected Color colorGradient;

        /// <summary>
        /// text
        /// </summary>
        protected string text;

        /// <summary>
        /// value
        /// </summary>
        protected double value;

        /// <summary>
        /// maxWidth
        /// </summary>
        protected int maxWidth = 15;

        /// <summary>
        /// Initializes a new instance of the <see cref="BarSlice"/> class.
        /// </summary>
        public BarSlice()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BarSlice"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="color">The color.</param>
        public BarSlice(double value, Color color)
        {
            this.value = value;
            this.color = color;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BarSlice"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="color">The color.</param>
        /// <param name="colorGradient">The color gradient.</param>
        public BarSlice(double value, Color color, Color colorGradient)
        {
            this.value = value;
            this.color = color;
            this.colorGradient = colorGradient;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BarSlice"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="color">The color.</param>
        /// <param name="text">The text.</param>
        public BarSlice(double value, Color color, string text)
        {
            this.value = value;
            this.color = color;
            this.text = text;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BarSlice"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="color">The color.</param>
        /// <param name="colorGradient">The color gradient.</param>
        /// <param name="text">The text.</param>
        public BarSlice(double value, Color color, Color colorGradient, string text)
        {
            this.value = value;
            this.color = color;
            this.text = text;
            this.colorGradient = colorGradient;
        }

        /// <summary>
        /// 获取或设置Color
        /// </summary>
        /// <value></value>
        public Color Color
        {
            get { return color; }
            set { color = value; }
        }

        /// <summary>
        /// 获取或设置ColorGradient
        /// </summary>
        /// <value></value>
        public Color ColorGradient
        {
            get
            {
                if (colorGradient == Color.Empty)
                    return color;
                else
                    return colorGradient;
            }
            set { colorGradient = value; }
        }

        /// <summary>
        /// 获取或设置Text
        /// </summary>
        /// <value></value>
        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        /// <summary>
        /// 获取或设置Value
        /// </summary>
        /// <value></value>
        public double Value
        {
            get { return value; }
            set { this.value = value; }
        }

        /// <summary>
        /// Gets or sets the width of the max.
        /// </summary>
        /// <value>The width of the max.</value>
        public int MaxWidth
        {
            get { return maxWidth; }
            set { maxWidth = value; }
        }
    }
}