namespace Zdd.Utility.Graphs
{
    /// <summary>
    /// Summary description for XAxisText.
    /// </summary>
    public class XAxisText
    {
        private double xValueStart = 0.0;
        private double xValueEnd = 0.0;
        private string text = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="XAxisText"/> class.
        /// </summary>
        public XAxisText()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XAxisText"/> class.
        /// </summary>
        /// <param name="xValue">The x value.</param>
        /// <param name="text">The text.</param>
        public XAxisText(double xValue, string text)
        {
            xValueStart = xValue;
            xValueEnd = xValue;
            this.text = text;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XAxisText"/> class.
        /// </summary>
        /// <param name="xValueStart">The x value start.</param>
        /// <param name="xValueEnd">The x value end.</param>
        /// <param name="text">The text.</param>
        public XAxisText(double xValueStart, double xValueEnd, string text)
        {
            this.xValueStart = xValueStart;
            this.xValueEnd = xValueEnd;
            this.text = text;
        }

        /// <summary>
        /// 获取或设置XValueStart
        /// </summary>
        /// <value></value>
        public double XValueStart
        {
            get { return xValueStart; }
            set { xValueStart = value; }
        }

        /// <summary>
        /// 获取或设置XValueEnd
        /// </summary>
        /// <value></value>
        public double XValueEnd
        {
            get { return xValueEnd; }
            set { xValueEnd = value; }
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