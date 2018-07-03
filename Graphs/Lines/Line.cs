using System.Drawing;

namespace Zdd.Utility.Graphs
{
    /// <summary>
    /// Summary description for Line.
    /// </summary>
    public class Line
    {
        private Color color = Color.Black;
        private LinePointCollection points = null;
        private float width = 1.0F;

        /// <summary>
        /// Initializes a new instance of the <see cref="Line"/> class.
        /// </summary>
        public Line()
        {
            points = new LinePointCollection();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Line"/> class.
        /// </summary>
        /// <param name="color">The color.</param>
        public Line(Color color)
        {
            this.color = color;
            points = new LinePointCollection();
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
        /// 获取或设置Points
        /// </summary>
        /// <value></value>
        public LinePointCollection Points
        {
            get { return points; }
            set { points = value; }
        }

        /// <summary>
        /// 获取或设置Width
        /// </summary>
        /// <value></value>
        public float Width
        {
            get { return width; }
            set { width = value; }
        }

        /// <summary>
        /// Adds the point.
        /// </summary>
        /// <param name="xvalue">The xvalue.</param>
        /// <param name="yValue">The y value.</param>
        public void AddPoint(double xvalue, double yValue)
        {
            Points.Add(new LinePoint(xvalue, yValue, null));
        }

        /// <summary>
        /// Adds the point.
        /// </summary>
        /// <param name="xvalue">The xvalue.</param>
        /// <param name="yValue">The y value.</param>
        /// <param name="text">The text.</param>
        public void AddPoint(double xvalue, double yValue, string text)
        {
            Points.Add(new LinePoint(xvalue, yValue, text));
        }
    }
}