using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;

namespace Zdd.Utility.Graphs
{
    /// <summary>
    /// Summary description for GraphRendered.
    /// </summary>
    public class GraphRenderer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GraphRenderer"/> class.
        /// </summary>
        public GraphRenderer()
        {
        }

        /// <summary>
        /// Draws the legend.
        /// </summary>
        /// <param name="legend">The legend.</param>
        /// <returns></returns>
        public static Image DrawLegend(Legend legend)
        {
            LegendRenderer lr = new LegendRenderer();
            return lr.DrawLegend(legend);
        }

        /// <summary>
        /// Draws the graph.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <returns></returns>
        public static Image DrawGraph(GraphBase graph)
        {
            Image graphImage = null;

            if (graph.GetType().Name == "BarGraph")
                graphImage = DrawGraph((BarGraph) graph);
            else if (graph.GetType().Name == "PieGraph")
                graphImage = DrawGraph((PieGraph) graph);
            else if (graph.GetType().Name == "DateLineGraph")
                graphImage = DrawGraph((DateLineGraph) graph);
            else
                graphImage = DrawGraph((LineGraph) graph);

            return graphImage;
        }

        /// <summary>
        /// Draws the graph.
        /// </summary>
        /// <param name="barGraph">The bar graph.</param>
        /// <returns></returns>
        public static Image DrawGraph(BarGraph barGraph)
        {
            BarGraphRenderer bgr = new BarGraphRenderer();
            return bgr.DrawGraph(barGraph);
        }

        /// <summary>
        /// Draws the graph.
        /// </summary>
        /// <param name="lineGraph">The line graph.</param>
        /// <returns></returns>
        public static Image DrawGraph(LineGraph lineGraph)
        {
            LineGraphRenderer lgr = new LineGraphRenderer();
            return lgr.DrawGraph(lineGraph);
        }

        /// <summary>
        /// Draws the graph.
        /// </summary>
        /// <param name="dateLineGraph">The date line graph.</param>
        /// <returns></returns>
        public static Image DrawGraph(DateLineGraph dateLineGraph)
        {
            DateLineGraphRenderer dlgr = new DateLineGraphRenderer();
            return dlgr.DrawGraph(dateLineGraph);
        }

        /// <summary>
        /// Draws the graph.
        /// </summary>
        /// <param name="pieGraph">The pie graph.</param>
        /// <returns></returns>
        public static Image DrawGraph(PieGraph pieGraph)
        {
            PieGraphRenderer pgr = new PieGraphRenderer();
            return pgr.DrawGraph(pieGraph);
        }

        /// <summary>
        /// Joins the bit maps.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="legend">The legend.</param>
        /// <returns></returns>
        public static Image JoinBitMaps(Image graph, Image legend)
        {
            int width = graph.Width;
            if (legend.Width > graph.Width)
                width = legend.Width;
            Size totalSize = new Size(width, graph.Height + legend.Height);
            return JoinBitMaps(graph, legend, totalSize);
        }

        /// <summary>
        /// Joins the bit maps.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="legend">The legend.</param>
        /// <param name="totalSize">The total size.</param>
        /// <returns></returns>
        public static Image JoinBitMaps(Image graph, Image legend, Size totalSize)
        {
            Bitmap bMap = new Bitmap(totalSize.Width, totalSize.Height, PixelFormat.Format64bppPArgb);
            Graphics g = Graphics.FromImage(bMap);

            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.Clear(Color.White);

            Rectangle graphRect = new Rectangle(0, 0, totalSize.Width, (int) (totalSize.Height*0.92));
            Rectangle legendRect =
                new Rectangle(0, graphRect.Height, totalSize.Width, totalSize.Height - graphRect.Height);
            if (legendRect.Height > legend.Height)
            {
                legendRect = new Rectangle(0, totalSize.Height - legend.Height, totalSize.Width, legend.Height);
                graphRect = new Rectangle(0, 0, totalSize.Width, (int) (totalSize.Height - legendRect.Height));
            }
            g.DrawImage(graph, graphRect);
            g.DrawImage(legend, legendRect);
            Pen gPen = new Pen(Color.Gray, (float) 0.3);
            g.DrawRectangle(gPen, (int) 0, (int) 0, (int) totalSize.Width - 1, (int) totalSize.Height - 1);
            return bMap;
        }

        /// <summary>
        /// Draws the graph and legend.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="legend">The legend.</param>
        /// <returns></returns>
        public static Image DrawGraphAndLegend(GraphBase graph, Legend legend)
        {
            Image graphImage = null;

            if (graph.GetType().Name == "BarGraph")
                graphImage = DrawGraph((BarGraph) graph);
            else
                graphImage = DrawGraph((LineGraph) graph);

            Image legendImage = DrawLegend(legend);
            return JoinBitMaps(graphImage, legendImage);
        }

        /// <summary>
        /// Draws the graph and legend.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="legend">The legend.</param>
        /// <param name="totalSize">The total size.</param>
        /// <returns></returns>
        public static Image DrawGraphAndLegend(GraphBase graph, Legend legend, Size totalSize)
        {
            Image graphImage = null;
            Rectangle graphRect = new Rectangle(0, 0, totalSize.Width, (int) (totalSize.Height*0.92));
            Rectangle legendRect =
                new Rectangle(0, graphRect.Height, totalSize.Width, totalSize.Height - graphRect.Height);

            if (legendRect.Height > legend.Size.Height)
            {
                legendRect =
                    new Rectangle(0, totalSize.Height - legend.Size.Height, totalSize.Width, legend.Size.Height);
                graphRect = new Rectangle(0, 0, totalSize.Width, (int) (totalSize.Height - legendRect.Height));
            }

            graph.Size = graphRect.Size;
            legend.Size = legendRect.Size;

            if (graph.GetType().Name == "BarGraph")
                graphImage = DrawGraph((BarGraph) graph);
            else if (graph.GetType().Name == "PieGraph")
                graphImage = DrawGraph((PieGraph) graph);
            else if (graph.GetType().Name == "DateLineGraph")
                graphImage = DrawGraph((DateLineGraph) graph);
            else
                graphImage = DrawGraph((LineGraph) graph);

            Image legendImage = DrawLegend(legend);
            return JoinBitMaps(graphImage, legendImage, totalSize);
        }
    }
}