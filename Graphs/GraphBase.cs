using System.Drawing;

namespace Zdd.Utility.Graphs
{
    /// <summary>
    /// Alignment
    /// </summary>
    public enum Alignment
    {
        /// <summary>
        /// VerticalBottom
        /// </summary>
        VerticalBottom,
        /// <summary>
        /// VerticalTop
        /// </summary>
        VerticalTop,
        /// <summary>
        /// HorizontalLeft
        /// </summary>
        HorizontalLeft,
        /// <summary>
        /// HorizontalRight
        /// </summary>
        HorizontalRight
    }

    /// <summary>
    /// Summary description for Class1.
    /// </summary>
    public abstract class GraphBase
    {
        private Size size = Size.Empty;
        private int border = 15;
        private Color color = SystemColors.Info;
        private Color colorGradient = Color.Empty;
        private string text = "Graph";
        private Alignment alignment = Alignment.VerticalBottom;
        private bool roundOffGridHeight = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphBase"/> class.
        /// </summary>
        public GraphBase()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphBase"/> class.
        /// </summary>
        /// <param name="size">The size.</param>
        public GraphBase(Size size)
        {
            this.size = size;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphBase"/> class.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public GraphBase(int width, int height)
        {
            size = new Size(width, height);
        }

        /// <summary>
        /// 获取或设置Size
        /// </summary>
        /// <value></value>
        public virtual Size Size
        {
            get
            {
                if (size == Size.Empty)
                    return new Size(500, 500);
                else
                    return size;
            }
            set { size = value; }
        }

        /// <summary>
        /// 获取或设置Border
        /// </summary>
        /// <value></value>
        public int Border
        {
            get { return border; }
            set { border = value; }
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
        /// 获取或设置Alignment
        /// </summary>
        /// <value></value>
        public Alignment Alignment
        {
            get { return alignment; }
            set { alignment = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [round off grid height].
        /// </summary>
        /// <value><c>true</c> if [round off grid height]; otherwise, <c>false</c>.</value>
        public bool RoundOffGridHeight
        {
            get { return roundOffGridHeight; }
            set { roundOffGridHeight = value; }
        }
    }
}