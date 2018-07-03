using System;
using System.Drawing;

namespace Zdd.Utility.Graphs
{
    /// <summary>
    /// Summary description for Legend.
    /// </summary>
    public class Legend
    {
        private LegendEntryCollection legendEntryCollection = null;
        private int border = 2;
        private int columnCount = 0;
        private int columnGap = 2;
        private Size size = Size.Empty;
        private Color color = Color.White;
        private string text = "Legend";
        private double factorWidth = 5;
        private double factorHeight = 2;

        /// <summary>
        /// Initializes a new instance of the <see cref="Legend"/> class.
        /// </summary>
        /// <param name="size">The size.</param>
        public Legend(Size size)
        {
            this.size = size;
            legendEntryCollection = new LegendEntryCollection();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Legend"/> class.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public Legend(int width, int height)
        {
            Size size = new Size(width, height);
            this.size = size;
            legendEntryCollection = new LegendEntryCollection();
        }

        /// <summary>
        /// ��ȡ������LegendEntryCollection
        /// </summary>
        /// <value></value>
        public LegendEntryCollection LegendEntryCollection
        {
            get { return legendEntryCollection; }
            set { legendEntryCollection = value; }
        }

        /// <summary>
        /// ��ȡ������Border
        /// </summary>
        /// <value></value>
        public int Border
        {
            get { return border; }
            set { border = value; }
        }

        /// <summary>
        /// ��ȡ������ColumnCount
        /// </summary>
        /// <value></value>
        public int ColumnCount
        {
            get
            {
                try
                {
                    if (columnCount == 0)
                    {
                        if (Size == Size.Empty || LegendEntryCollection == null)
                        {
                            return 1;
                        }
                        else if (LegendEntryCollection.Count == 0)
                        {
                            return 1;
                        }
                        else
                        {
                            double area = Size.Width*Size.Height/LegendEntryCollection.Count;
                            double x = Math.Sqrt(area/(factorWidth*factorHeight));

                            if (x < 0.0)
                                x = x*(-1);

                            return (int) (Size.Width/(x*factorWidth));
                        }
                    }
                    else
                        return columnCount;
                }
                catch
                {
                    return 1;
                }
            }
            set { columnCount = value; }
        }

        /// <summary>
        /// ��ȡ������ColumnGap
        /// </summary>
        /// <value></value>
        public int ColumnGap
        {
            get { return columnGap; }
            set { columnGap = value; }
        }

        /// <summary>
        /// ��ȡ������Size
        /// </summary>
        /// <value></value>
        public Size Size
        {
            get { return size; }
            set { size = value; }
        }

        /// <summary>
        /// ��ȡ������Color
        /// </summary>
        /// <value></value>
        public Color Color
        {
            get { return color; }
            set { color = value; }
        }

        /// <summary>
        /// ��ȡ������Text
        /// </summary>
        /// <value></value>
        public string Text
        {
            get { return text; }
            set { text = value; }
        }
    }
}