using System.Drawing;

namespace Zdd.Utility.Graphs
{
    /// <summary>
    /// Summary description for PieSlice.
    /// </summary>
    public class PieSlice
    {
        private Color color = Color.Blue;
        private double value;

        /// <summary>
        /// Initializes a new instance of the <see cref="PieSlice"/> class.
        /// </summary>
        public PieSlice()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PieSlice"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="color">The color.</param>
        public PieSlice(double value, Color color)
        {
            this.value = value;
            this.color = color;
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
        /// 获取或设置Value
        /// </summary>
        /// <value></value>
        public double Value
        {
            get { return value; }
            set { this.value = value; }
        }
    }
}