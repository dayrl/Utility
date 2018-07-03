using System;
using System.Drawing;

namespace Zdd.Utility.Graphs
{
    /// <summary>
    /// MultiBarDisplayStyle
    /// </summary>
    public enum MultiBarDisplayStyle
    {
        /// <summary>
        /// SingleBar
        /// </summary>
        SingleBar = 1,

        /// <summary>
        /// SeparateBars
        /// </summary>
        SeparateBars = 2
    }

    /// <summary>
    /// GridGraphBase
    /// </summary>
    public class BarGraph : GridGraphBase
    {
        private BarSliceCollection barSliceCollection = null;
        private int barGap = 2;
        private int maxBarSliceWidth = 15;
        private bool showBarSliceText = false;
        private double cutOff = Double.MinValue;
        private MultiBarDisplayStyle multiBarDisplayStyle = MultiBarDisplayStyle.SingleBar;


        /// <summary>
        /// Initializes a new instance of the <see cref="BarGraph"/> class.
        /// </summary>
        public BarGraph()
        {
            barSliceCollection = new BarSliceCollection();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BarGraph"/> class.
        /// </summary>
        /// <param name="size">The size.</param>
        public BarGraph(Size size)
            : base(size)
        {
            barSliceCollection = new BarSliceCollection();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BarGraph"/> class.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public BarGraph(int width, int height)
            : base(width, height)
        {
            barSliceCollection = new BarSliceCollection();
        }

        /// <summary>
        /// 获取或设置BarSliceCollection
        /// </summary>
        /// <value></value>
        public BarSliceCollection BarSliceCollection
        {
            get { return barSliceCollection; }
            set { barSliceCollection = value; }
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
                {
                    if (barSliceCollection != null && barSliceCollection.Count > 0)
                    {
                        return new Size(500, (maxBarSliceWidth + barGap)*(barSliceCollection.Count + 1) + 20);
                    }
                    else
                        return new Size(500, 500);
                }
                else
                    return base.Size;
            }
            set { base.Size = value; }
        }

        /// <summary>
        /// 获取或设置BarGap
        /// </summary>
        /// <value></value>
        public int BarGap
        {
            get { return barGap; }
            set { barGap = value; }
        }

        /// <summary>
        /// Gets or sets the width of the max bar slice.
        /// </summary>
        /// <value>The width of the max bar slice.</value>
        public int MaxBarSliceWidth
        {
            get { return maxBarSliceWidth; }
            set { maxBarSliceWidth = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [show bar slice text].
        /// </summary>
        /// <value><c>true</c> if [show bar slice text]; otherwise, <c>false</c>.</value>
        public bool ShowBarSliceText
        {
            get { return showBarSliceText; }
            set { showBarSliceText = value; }
        }

        /// <summary>
        /// 获取或设置CutOff
        /// </summary>
        /// <value></value>
        public double CutOff
        {
            get { return cutOff; }
            set { cutOff = value; }
        }

        /// <summary>
        /// 获取或设置MultiBarDisplayStyle
        /// </summary>
        /// <value></value>
        public MultiBarDisplayStyle MultiBarDisplayStyle
        {
            get { return multiBarDisplayStyle; }
            set { multiBarDisplayStyle = value; }
        }
    }
}