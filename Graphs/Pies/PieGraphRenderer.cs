using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Windows.Forms;

namespace Zdd.Utility.Graphs
{
    /// <summary>
    /// Summary description for PieGraphRenderer.
    /// </summary>
    public class PieGraphRenderer
    {
        private double widthPixels;
        private double heightPixels;
        private double drawingAreaWidthPixels;
        private double drawingAreaHeightPixels;
        private double totalValue;
        private double titleHeightPixels;

        private Rectangle totalRectangle;
        private Rectangle drawingAreaRectangle;
        private Rectangle pieGraphRectangle;
       //private Rectangle titleRectangle;

        private PieGraph pieGraph;

        private double maxTitleHeightPixels = 15.0;
        private double margin = 5.0;

        /// <summary>
        /// Initializes a new instance of the <see cref="PieGraphRenderer"/> class.
        /// </summary>
        public PieGraphRenderer()
        {
        }

        /// <summary>
        /// Draws the graph.
        /// </summary>
        /// <param name="pieGraph">The pie graph.</param>
        /// <returns></returns>
        public Image DrawGraph(PieGraph pieGraph)
        {
            try
            {
                if (pieGraph == null)
                    return null;

                this.pieGraph = pieGraph;

                Bitmap bMap = new Bitmap(pieGraph.Size.Width, pieGraph.Size.Height, PixelFormat.Format64bppPArgb);
                Graphics g = Graphics.FromImage(bMap);

                g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.Clear(Color.White);
                DrawVerticalBottomGraph(ref g);
                switch (pieGraph.Alignment)
                {
                    case Alignment.HorizontalLeft:
                        bMap.RotateFlip(RotateFlipType.Rotate270FlipXY);
                        break;

                    case Alignment.HorizontalRight:
                        bMap.RotateFlip(RotateFlipType.Rotate90FlipX);
                        break;

                    case Alignment.VerticalTop:
                        bMap.RotateFlip(RotateFlipType.Rotate180FlipX);
                        break;

                    default:
                        break;
                }
                return bMap;
            }
            catch
            {
                return null;
            }
        }

        private void DrawVerticalBottomGraph(ref Graphics g)
        {
            try
            {
                CalculateValues();
                DrawBackColorGradient(pieGraph.Color, pieGraph.ColorGradient, ref g);
                DrawPieGraph(ref g);
                DrawTitle(ref g);
                DrawBorder(ref g);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occured: \n" + ex.ToString(), "Graphing Message", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
        }

        private void CalculateValues()
        {
            widthPixels = pieGraph.Size.Width;
            heightPixels = pieGraph.Size.Height;
            drawingAreaWidthPixels = widthPixels - (2*pieGraph.Border);
            drawingAreaHeightPixels = heightPixels - (2*pieGraph.Border);
            if (pieGraph.Text != null && pieGraph.Text != String.Empty)
            {
                titleHeightPixels = drawingAreaHeightPixels*0.1;
                if (titleHeightPixels > maxTitleHeightPixels)
                    titleHeightPixels = maxTitleHeightPixels;

                drawingAreaHeightPixels = drawingAreaHeightPixels - titleHeightPixels;
            }
            totalValue = GetTotalValue();
            totalRectangle = new Rectangle(0, 0, (int) widthPixels, (int) heightPixels);
            drawingAreaRectangle =
                new Rectangle((int) pieGraph.Border, (int) pieGraph.Border + (int) titleHeightPixels,
                              (int) drawingAreaWidthPixels, (int) drawingAreaHeightPixels);
            pieGraphRectangle =
                new Rectangle((int) (pieGraph.Border + margin), (int) (pieGraph.Border + titleHeightPixels + margin),
                              (int) (drawingAreaWidthPixels - 2*margin), (int) (drawingAreaHeightPixels - 2*margin));
          //  titleRectangle =
                //new Rectangle((int) pieGraph.Border, (int) pieGraph.Border, (int) drawingAreaWidthPixels,
                              //(int) titleHeightPixels);
        }

        private double GetTotalValue()
        {
            if (pieGraph.Slices == null)
                return 0;

            if (pieGraph.Slices.Count == 0)
                return 0;

            double retVal = 0;

            for (int i = 0; i < pieGraph.Slices.Count; i++)
            {
                retVal += pieGraph.Slices[i].Value;
            }

            return retVal;
        }

        private void DrawBackColorGradient(Color startColor, Color endColor, ref Graphics g)
        {
            Pen gPen = new Pen(Color.Black, (float) 0.03);
            LinearGradientBrush lgBrush =
                new LinearGradientBrush(drawingAreaRectangle, startColor, endColor, (float) (45*(-1)), true);

            g.FillRectangle(lgBrush, drawingAreaRectangle);
            gPen.Color = Color.Black;
            g.DrawRectangle(gPen, drawingAreaRectangle);
        }

        private void DrawBorder(ref Graphics g)
        {
            Pen gPen = new Pen(Color.Gray, (float) 0.3);

            g.DrawRectangle(gPen, (int) totalRectangle.Left, (int) totalRectangle.Top, (int) widthPixels - 1,
                            (int) heightPixels - 1);
        }

        private void DrawTitle(ref Graphics g)
        {
            if (pieGraph.Text == null || pieGraph.Text == String.Empty)
                return;

            // Set up Font
            StringFormat format = new StringFormat(StringFormatFlags.NoClip);

            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Near;

            float fontSize = (float) 0.8*(float) titleHeightPixels;

            if (fontSize > (float) 10.0)
                fontSize = (float) 10.0;

            if (fontSize < (float) 1.0)
                fontSize = (float) 1.0;

            SolidBrush gBrush = new SolidBrush(Color.Black);

            //Draw Title
            Point Middle = new Point((int) MappedXCoordinatePixels(drawingAreaWidthPixels/2), (int) titleHeightPixels/2);

            g.TranslateTransform(Middle.X, Middle.Y);
            g.DrawString(pieGraph.Text, new Font("Tahoma", fontSize, FontStyle.Bold), gBrush, 0, 0, format);
            g.TranslateTransform(-Middle.X, -Middle.Y);
        }

        // The return value is to be used as is. It is calculated w.r.t. drawingAreaRectangle.Left
        private double MappedXCoordinatePixels(double value)
        {
            return drawingAreaRectangle.Left + value;
        }

        // The return value is to be used as is. It is calculated w.r.t. center of pieGraphRectangle
        private double MappedXCoordinateCenterPixels(double value)
        {
            return (pieGraphRectangle.Left + pieGraphRectangle.Right)/2 + value;
        }

        // The return value is to be used as is. It is calculated w.r.t. center of pieGraphRectangle
        private double MappedYCoordinateCenterPixels(double value)
        {
            return (pieGraphRectangle.Top + pieGraphRectangle.Bottom)/2 + value;
        }

        private float Round(float num, int precision)
        {
            float pow = 1;
            for (int i = 0; i < precision; i++)
            {
                pow *= 10;
            }
            if (num > 0)
                return (int) (num*pow + .5)/pow;
            return (int) (num*pow - .5)/pow;
        }

        private void DrawPieGraph(ref Graphics g)
        {
            if (pieGraph.Slices == null)
                return;

            if (pieGraph.Slices.Count == 0)
                return;

            // Set up Font
            StringFormat format = new StringFormat(StringFormatFlags.NoClip);

            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Near;

            float fontSize = (float) 0.51*(float) titleHeightPixels;

            if (fontSize > (float) 10.0)
                fontSize = (float) 10.0;

            if (fontSize < (float) 1.0)
                fontSize = (float) 1.0;

            Pen gPen = new Pen(Color.Black, (float) 0.03);
            double startAngle = 0;

            if (pieGraph.Slices.Count == 1)
            {
                SolidBrush gBrush = new SolidBrush(pieGraph.Slices[0].Color);
                format.LineAlignment = StringAlignment.Center;

                // Fill Pie Slice
                g.FillEllipse(gBrush, pieGraphRectangle);
                g.DrawEllipse(gPen, pieGraphRectangle);

                // Write Text
                gBrush = new SolidBrush(Color.White);
                string text = pieGraph.Slices[0].Value.ToString() + "\n(100%)";
                double x = MappedXCoordinateCenterPixels(0);
                double y = MappedYCoordinateCenterPixels(0);
                Point Middle = new Point((int) x, (int) y);
                g.TranslateTransform(Middle.X, Middle.Y);
                g.DrawString(text, new Font("Tahoma", fontSize, FontStyle.Bold), gBrush, 0, 0, format);
                g.TranslateTransform(-Middle.X, -Middle.Y);
                return;
            }

            for (int i = 0; i < pieGraph.Slices.Count; i++)
            {
                double sweepAngle = pieGraph.Slices[i].Value*360/totalValue;
                SolidBrush gBrush = new SolidBrush(pieGraph.Slices[i].Color);

                // Fill Pie Slice
                g.FillPie(gBrush, pieGraphRectangle, (float) startAngle, (float) sweepAngle);
                g.DrawPie(gPen, pieGraphRectangle, (float) startAngle, (float) sweepAngle);

                // Write Text in Pie Slice
                // x = a * cos(theta)
                // y = b * sin (theta)
                gBrush = new SolidBrush(Color.White);
                double angle = startAngle + sweepAngle/2;
                double x;
                double y;

                if (sweepAngle <= 20.0)
                {
                    x = MappedXCoordinateCenterPixels((pieGraphRectangle.Width*0.8/2)*Math.Cos(angle*Math.PI/180));
                    y = MappedYCoordinateCenterPixels((pieGraphRectangle.Height*0.8/2)*Math.Sin(angle*Math.PI/180));
                }
                else
                {
                    x = MappedXCoordinateCenterPixels((pieGraphRectangle.Width*0.6/2)*Math.Cos(angle*Math.PI/180));
                    y = MappedYCoordinateCenterPixels((pieGraphRectangle.Height*0.6/2)*Math.Sin(angle*Math.PI/180));
                }

                Point Middle = new Point((int) x, (int) y);
                g.TranslateTransform(Middle.X, Middle.Y);

                float percent = Round((float) (pieGraph.Slices[i].Value*100/totalValue), 1);
                string text = String.Empty;

                if (sweepAngle <= 20.0)
                    text = pieGraph.Slices[i].Value.ToString() + " (" + percent.ToString() + "%)";
                else
                    text = pieGraph.Slices[i].Value.ToString() + "\n(" + percent.ToString() + "%)";
                g.DrawString(text, new Font("Tahoma", fontSize, FontStyle.Bold), gBrush, 0, 0, format);
                g.TranslateTransform(-Middle.X, -Middle.Y);

                startAngle += sweepAngle;
            }
        }
    } // class
} // namespace