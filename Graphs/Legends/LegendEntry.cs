using System.Drawing;

namespace Zdd.Utility.Graphs
{
    /// <summary>
    /// Summary description for Legend.
    /// </summary>
    public class LegendEntry
    {
        private Color color;
        private string text;

        /// <summary>
        /// Initializes a new instance of the <see cref="LegendEntry"/> class.
        /// </summary>
        public LegendEntry()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LegendEntry"/> class.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <param name="text">The text.</param>
        public LegendEntry(Color color, string text)
        {
            this.color = color;
            this.text = text;
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