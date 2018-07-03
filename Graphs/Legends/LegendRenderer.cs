using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;

namespace Zdd.Utility.Graphs
{
    /// <summary>
    /// Summary description for LegendRenderer.
    /// </summary>
    internal class LegendRenderer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LegendRenderer"/> class.
        /// </summary>
        public LegendRenderer()
        {
        }

        /// <summary>
        /// Draws the legend.
        /// </summary>
        /// <param name="legend">The legend.</param>
        /// <returns></returns>
        public Image DrawLegend(Legend legend)
        {
            Bitmap bMap = new Bitmap(legend.Size.Width, legend.Size.Height, PixelFormat.Format64bppPArgb);
            Graphics g = Graphics.FromImage(bMap);
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            g.Clear(legend.Color);
            FillUpLegend(legend, ref g);
            return bMap;
        }

        /// <summary>
        /// Fills the up legend.
        /// </summary>
        /// <param name="legend">The legend.</param>
        /// <param name="g">The g.</param>
        private void FillUpLegend(Legend legend, ref Graphics g)
        {
            if (legend.ColumnCount <= 0)
                return;

            if (legend.LegendEntryCollection == null)
                return;

            if (legend.LegendEntryCollection.Count <= 0)
                return;

            Pen gPen = new Pen(Color.Black, (float) 0.03);
            SolidBrush gBrush = new SolidBrush(legend.Color);

            int surfaceWidth = legend.Size.Width - (2*legend.Border);
            int surfaceHeight = legend.Size.Height - (2*legend.Border);

            int mainTextHeight = 0;
            int mainTextWidth = 0;

            if (legend.Text != null && legend.Text != String.Empty)
            {
                mainTextHeight = (int) ((float) surfaceHeight*(float) 0.2);
                mainTextWidth = (int) ((float) surfaceWidth*(float) 0.8);
                surfaceHeight = (int) ((float) surfaceHeight*(float) 0.8);
            }

            Rectangle[] columnRect = new Rectangle[legend.ColumnCount];
            int columnWidth = (surfaceWidth - ((legend.ColumnCount - 1)*legend.ColumnGap))/legend.ColumnCount;
            int columnHeight = surfaceHeight;
            columnRect[0] = new Rectangle(legend.Border, legend.Border + mainTextHeight, columnWidth, columnHeight);
            for (int i = 1; i < legend.ColumnCount; i++)
            {
                columnRect[i] =
                    new Rectangle(columnRect[i - 1].Right + legend.ColumnGap, legend.Border + mainTextHeight,
                                  columnWidth, columnHeight);
            }

            int totalEntries = legend.LegendEntryCollection.Count;
            int rows;
            if (totalEntries%legend.ColumnCount == 0)
                rows = totalEntries/legend.ColumnCount;
            else
                rows = totalEntries/legend.ColumnCount + 1;

            int entryWidth = columnWidth;
            int entryHeight = columnHeight/rows;

            int boxWidth = (int) ((float) entryWidth*(float) 0.2);
            int boxTextGap = (int) ((float) entryWidth*(float) 0.15);

            if (boxTextGap > 2)
                boxTextGap = 2;

            int textWidth = (int) ((float) entryWidth*(float) 0.65);

            int width1 = (int) ((float) boxWidth*(float) 0.7);
            int width2 = (int) ((float) entryHeight*(float) 0.6);
            int boxSide;
            if (width1 > width2)
                boxSide = width2;
            else
                boxSide = width1;
            if (boxSide > 12)
                boxSide = 12;

            int textHeight = (int) ((float) entryHeight*(float) 0.7);

            int rowNo = 0;
            int columnNo = 0;
            float fontSize;
            for (int i = 0; i < totalEntries; i++)
            {
                Rectangle boxRect =
                    new Rectangle(columnRect[columnNo].Left + boxWidth/2 - boxSide/2,
                                  columnRect[columnNo].Top + entryHeight*rowNo + entryHeight/2 - boxSide/2, boxSide,
                                  boxSide);
                gBrush.Color = legend.LegendEntryCollection[i].Color;
                g.FillRectangle(gBrush, boxRect);
                g.DrawRectangle(gPen, boxRect);

                fontSize = (float) boxSide*(float) 0.7;
                if (fontSize < (float) 1.0)
                    fontSize = (float) 1.0;
                if (fontSize > (float) 10.0)
                    fontSize = (float) 10.0;

                RectangleF textRect =
                    new RectangleF(columnRect[columnNo].Left + boxWidth + boxTextGap,
                                   columnRect[columnNo].Top + entryHeight*rowNo + entryHeight/2 - textHeight/2,
                                   textWidth, textHeight);
                StringFormat sf = new StringFormat(StringFormatFlags.NoClip);
                sf.Alignment = StringAlignment.Near;
                sf.LineAlignment = StringAlignment.Center;
                //g.DrawString (legend.LegendEntryCollection[i].Text, new Font ("Tahoma", fontSize), new SolidBrush (Color.Black), textRect.Left, textRect.Top + textRect.Height/2 - fontSize/2);
                g.DrawString(legend.LegendEntryCollection[i].Text, new Font("Tahoma", fontSize),
                             new SolidBrush(Color.Black), textRect, sf);

                columnNo++;
                if (columnNo >= legend.ColumnCount)
                    columnNo = 0;

                if ((i + 1)%legend.ColumnCount == 0)
                    rowNo++;
            }

            fontSize = (float) mainTextHeight*(float) 0.4;
            if (fontSize < (float) 1.0)
                fontSize = (float) 1.0;
            if (fontSize > (float) 11.0)
                fontSize = (float) 11.0;

            Rectangle mainTextRect = new Rectangle(legend.Border, legend.Border, mainTextWidth, mainTextHeight);
            g.TranslateTransform(surfaceWidth/2, legend.Border + mainTextHeight/2);
            StringFormat format = new StringFormat(StringFormatFlags.NoClip);
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;
            g.DrawString(legend.Text, new Font("Tahoma", fontSize, FontStyle.Bold), new SolidBrush(Color.Black), 0, 0,
                         format);
        }
    }
}