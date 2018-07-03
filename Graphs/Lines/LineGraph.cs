using System.Drawing;

namespace Zdd.Utility.Graphs
{
    /// <summary>
    /// Summary description for LineGraph.
    /// </summary>
    public class LineGraph : GridGraphBase
    {
        private LineCollection lineCollection = null;
        private Line trendLine = null;
        private double totalXAxisIntervals = 1.0;
        private double xAxisIntervalValue = 1.0;
        private XAxisTextCollection xAxisTextCollection = null;
        private XAxisTextCollection phaseLines = null;
        private bool showProjectedTrend = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="LineGraph"/> class.
        /// </summary>
        public LineGraph()
            : base()
        {
            lineCollection = new LineCollection();
            xAxisTextCollection = new XAxisTextCollection();
            phaseLines = new XAxisTextCollection();
            trendLine = new Line();
            trendLine.Width = 2.0F;
            MarginForTextOnAxis = 10;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LineGraph"/> class.
        /// </summary>
        /// <param name="size">The size.</param>
        public LineGraph(Size size)
            : base(size)
        {
            lineCollection = new LineCollection();
            xAxisTextCollection = new XAxisTextCollection();
            phaseLines = new XAxisTextCollection();
            trendLine = new Line();
            trendLine.Width = 2.0F;
            MarginForTextOnAxis = 10;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LineGraph"/> class.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public LineGraph(int width, int height)
            : base(width, height)
        {
            lineCollection = new LineCollection();
            xAxisTextCollection = new XAxisTextCollection();
            phaseLines = new XAxisTextCollection();
            trendLine = new Line();
            trendLine.Width = 2.0F;
            MarginForTextOnAxis = 10;
        }

        /// <summary>
        /// 获取或设置Size
        /// </summary>
        /// <value></value>
        public override Size Size
        {
            get
            {
                if (base.Size == Size.Empty)
                    return new Size(500, 500);
                else
                    return base.Size;
            }
            set { base.Size = value; }
        }

        /// <summary>
        /// 获取或设置Lines
        /// </summary>
        /// <value></value>
        public LineCollection Lines
        {
            get { return lineCollection; }
            set { lineCollection = value; }
        }

        /// <summary>
        /// 获取或设置XAxisTextCollection
        /// </summary>
        /// <value></value>
        public XAxisTextCollection XAxisTextCollection
        {
            get { return xAxisTextCollection; }
            set { xAxisTextCollection = value; }
        }

        /// <summary>
        /// 获取或设置PhaseLines
        /// </summary>
        /// <value></value>
        public XAxisTextCollection PhaseLines
        {
            get { return phaseLines; }
            set { phaseLines = value; }
        }

        /// <summary>
        /// 获取或设置TrendLine
        /// </summary>
        /// <value></value>
        public Line TrendLine
        {
            get { return trendLine; }
            set { trendLine = value; }
        }

        /// <summary>
        /// 获取或设置TotalXAxisIntervals
        /// </summary>
        /// <value></value>
        public double TotalXAxisIntervals
        {
            get { return totalXAxisIntervals; }
            set { totalXAxisIntervals = value; }
        }

        /// <summary>
        /// 获取或设置XAxisIntervalValue
        /// </summary>
        /// <value></value>
        public double XAxisIntervalValue
        {
            get { return xAxisIntervalValue; }
            set { xAxisIntervalValue = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [show projected trend].
        /// </summary>
        /// <value><c>true</c> if [show projected trend]; otherwise, <c>false</c>.</value>
        public bool ShowProjectedTrend
        {
            get { return showProjectedTrend; }
            set { showProjectedTrend = value; }
        }

        /// <summary>
        /// Adds the X axis text.
        /// </summary>
        /// <param name="xValue">The x value.</param>
        /// <param name="text">The text.</param>
        public void AddXAxisText(double xValue, string text)
        {
            XAxisTextCollection.Add(new XAxisText(xValue, text));
        }

        /// <summary>
        /// Adds the X axis text.
        /// </summary>
        /// <param name="xValueStart">The x value start.</param>
        /// <param name="xValueEnd">The x value end.</param>
        /// <param name="text">The text.</param>
        public void AddXAxisText(double xValueStart, double xValueEnd, string text)
        {
            XAxisTextCollection.Add(new XAxisText(xValueStart, xValueEnd, text));
        }
    }
}