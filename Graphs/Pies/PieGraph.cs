using System.Drawing;

namespace Zdd.Utility.Graphs
{
    /// <summary>
    /// Summary description for PieGraph.
    /// </summary>
    public class PieGraph : GraphBase
    {
        private PieSliceCollection pieSliceCollection = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="PieGraph"/> class.
        /// </summary>
        public PieGraph()
        {
            pieSliceCollection = new PieSliceCollection();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PieGraph"/> class.
        /// </summary>
        /// <param name="size">The size.</param>
        public PieGraph(Size size)
            : base(size)
        {
            pieSliceCollection = new PieSliceCollection();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PieGraph"/> class.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public PieGraph(int width, int height)
            : base(width, height)
        {
            pieSliceCollection = new PieSliceCollection();
        }

        /// <summary>
        /// 获取或设置Slices
        /// </summary>
        /// <value></value>
        public PieSliceCollection Slices
        {
            get { return pieSliceCollection; }
            set { pieSliceCollection = value; }
        }
    }
}