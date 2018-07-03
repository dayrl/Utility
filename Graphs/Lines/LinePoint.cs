namespace Zdd.Utility.Graphs
{
    /// <summary>
    /// Summary description for LinePoint.
    /// </summary>
    public class LinePoint
    {
        private double xValue = 0.0;
        private double yValue = 0.0;
        private string text = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="LinePoint"/> class.
        /// </summary>
        public LinePoint()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LinePoint"/> class.
        /// </summary>
        /// <param name="xValue">The x value.</param>
        /// <param name="yValue">The y value.</param>
        public LinePoint(double xValue, double yValue)
        {
            this.xValue = xValue;
            this.yValue = yValue;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LinePoint"/> class.
        /// </summary>
        /// <param name="xValue">The x value.</param>
        /// <param name="yValue">The y value.</param>
        /// <param name="text">The text.</param>
        public LinePoint(double xValue, double yValue, string text)
        {
            this.xValue = xValue;
            this.yValue = yValue;
            this.text = text;
        }

        /// <summary>
        /// 获取或设置XValue
        /// </summary>
        /// <value></value>
        public double XValue
        {
            get { return xValue; }
            set { xValue = value; }
        }

        /// <summary>
        /// 获取或设置YValue
        /// </summary>
        /// <value></value>
        public double YValue
        {
            get { return yValue; }
            set { yValue = value; }
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
    }
}