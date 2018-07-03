using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Windows.Forms;

namespace Zdd.Utility.Graphs
{
    /// <summary>
    /// Summary description for LineGraphRenderer.
    /// </summary>
    public class LineGraphRenderer
    {
        private double widthPixels;
        private double heightPixels;
        private double drawingAreaWidthPixels;
        private double drawingAreaHeightPixels;
        private double minimumYValue;
        private double maximumYValue;
        private double totalYValue;
        private double positiveHeightPixels; // In Drawing Area
        private double negativeHeightPixels; // In Drawing Area
        private double gridHeightValue;
        private double gridHeightPixels;
        private double titleHeightPixels;
       // private double totalXAxisIntervals;
        private double maxXValue;
        private double xAxisIntervalValue;

        private Rectangle totalRectangle;
        private Rectangle drawingAreaRectangle;
       // private Rectangle titleRectangle;

        private LineGraph lineGraph;

        private int marginForTextOnXAxis;
        private double marginForPhaseLinesText;
        private int marginForNumbersOnYAxis = 15;
        private double maxTitleHeightPixels = 15.0;

        /// <summary>
        /// Initializes a new instance of the <see cref="LineGraphRenderer"/> class.
        /// </summary>
        public LineGraphRenderer()
        {
        }

        /// <summary>
        /// Draws the graph.
        /// </summary>
        /// <param name="lineGraph">The line graph.</param>
        /// <returns></returns>
        public Image DrawGraph(LineGraph lineGraph)
        {
            try
            {
                if (lineGraph == null)
                    return null;

                this.lineGraph = lineGraph;

                Bitmap bMap = new Bitmap(lineGraph.Size.Width, lineGraph.Size.Height, PixelFormat.Format64bppPArgb);
                Graphics g = Graphics.FromImage(bMap);

                g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.Clear(Color.White);
                DrawVerticalBottomGraph(ref g);
                switch (lineGraph.Alignment)
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

        /// <summary>
        /// Draws the vertical bottom graph.
        /// </summary>
        /// <param name="g">The g.</param>
        private void DrawVerticalBottomGraph(ref Graphics g)
        {
            try
            {
                CalculateValues();
                //DrawBackColor(ref g);
                DrawBackColorGradient(lineGraph.Color, lineGraph.ColorGradient, ref g);
                //DrawCustomBackColorGradient(Color.White, Color.DarkGray, ref g);
                DrawXAxisText(ref g);
                DrawPhaseLines(ref g);
                DrawGrid(ref g);
                for (int i = 0; i < lineGraph.Lines.Count; i++)
                {
                    DrawLineGraph(lineGraph.Lines[i], ref g);
                }
                DrawTrendLine(ref g);
                DrawXAxisSectionMarks(ref g);
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
            marginForTextOnXAxis = lineGraph.MarginForTextOnAxis;

            widthPixels = lineGraph.Size.Width;
            heightPixels = lineGraph.Size.Height;

            drawingAreaWidthPixels = widthPixels - (2*lineGraph.Border) - marginForNumbersOnYAxis;
            drawingAreaHeightPixels = heightPixels - (2*lineGraph.Border) - marginForTextOnXAxis;
            if (lineGraph.Text != null && lineGraph.Text != String.Empty)
            {
                titleHeightPixels = drawingAreaHeightPixels*0.1;
                if (titleHeightPixels > maxTitleHeightPixels)
                    titleHeightPixels = maxTitleHeightPixels;
                drawingAreaHeightPixels = drawingAreaHeightPixels - titleHeightPixels;
            }

            if (lineGraph.PhaseLines != null && lineGraph.PhaseLines.Count > 0)
            {
                marginForPhaseLinesText = drawingAreaHeightPixels*0.05;
                if (marginForPhaseLinesText > 10)
                    marginForPhaseLinesText = 10;
                drawingAreaHeightPixels = drawingAreaHeightPixels - marginForPhaseLinesText;
            }

            minimumYValue = GetMinimumYValue();
            maximumYValue = GetMaximumYValue();
            totalYValue = GetTotalYValue();

            positiveHeightPixels = GetPositiveHeightPixels();
            negativeHeightPixels = GetNegativeHeightPixels();

            gridHeightValue = GetGridHeightValue();
            gridHeightPixels = GetGridHeightPixels();

            //totalXAxisIntervals = lineGraph.TotalXAxisIntervals;
            maxXValue = GetMaxXValue();
            xAxisIntervalValue = lineGraph.XAxisIntervalValue;

            totalRectangle = new Rectangle(0, 0, (int) widthPixels, (int) heightPixels);
            drawingAreaRectangle =
                new Rectangle((int) lineGraph.Border + marginForNumbersOnYAxis,
                              (int) lineGraph.Border + (int) titleHeightPixels + (int) marginForPhaseLinesText,
                              (int) drawingAreaWidthPixels, (int) drawingAreaHeightPixels);
          //  titleRectangle =
               // new Rectangle((int) lineGraph.Border, (int) lineGraph.Border, (int) drawingAreaWidthPixels,
                             // (int) titleHeightPixels);
        }

        private double GetMinimumYValue()
        {
            double minValue = Double.MaxValue;
            for (int i = 0; i < lineGraph.Lines.Count; i++)
            {
                Line line = lineGraph.Lines[i];
                for (int j = 0; j < line.Points.Count; j++)
                {
                    LinePoint p = line.Points[j];
                    if (p.YValue < minValue)
                        minValue = p.YValue;
                }
            }

            if ((lineGraph.TrendLine != null) && (lineGraph.TrendLine.Points != null) &&
                (lineGraph.TrendLine.Points.Count > 0))
            {
                for (int j = 0; j < lineGraph.TrendLine.Points.Count; j++)
                {
                    LinePoint p = lineGraph.TrendLine.Points[j];

                    if (p.YValue < minValue)
                        minValue = p.YValue;
                }
            }

            if (minValue > 0.0)
                minValue = 0.0;

            if (lineGraph.RoundOffGridHeight)
                if (minValue < 0.0 && minValue > -10.0)
                    minValue = -10.0;

            return minValue;
        }

        private double GetMaximumYValue()
        {
            double maxValue = Double.MinValue;
            for (int i = 0; i < lineGraph.Lines.Count; i++)
            {
                Line line = lineGraph.Lines[i];
                for (int j = 0; j < line.Points.Count; j++)
                {
                    LinePoint p = line.Points[j];
                    if (p.YValue > maxValue)
                        maxValue = p.YValue;
                }
            }

            if ((lineGraph.TrendLine != null) && (lineGraph.TrendLine.Points != null) &&
                (lineGraph.TrendLine.Points.Count > 0))
            {
                for (int j = 0; j < lineGraph.TrendLine.Points.Count; j++)
                {
                    LinePoint p = lineGraph.TrendLine.Points[j];

                    if (p.YValue > maxValue)
                        maxValue = p.YValue;
                }
            }

            if (lineGraph.RoundOffGridHeight)
                if (maxValue < 10.0)
                    maxValue = 10.0;

            if (lineGraph.HonorScale && maxValue < lineGraph.MaxScaleValue)
                return lineGraph.MaxScaleValue;
            else
                return maxValue;
        }

        private double GetTotalYValue()
        {
            double retVal = maximumYValue - minimumYValue;
            if (retVal < 0.0)
                retVal = retVal*(-1.0);

            return retVal;
        }

        private double GetMinimumXValue()
        {
            double minValue = Double.MaxValue;

            for (int i = 0; i < lineGraph.Lines.Count; i++)
            {
                Line line = lineGraph.Lines[i];

                for (int j = 0; j < line.Points.Count; j++)
                {
                    LinePoint p = line.Points[j];

                    if (p.XValue < minValue)
                        minValue = p.XValue;
                }
            }

            return minValue;
        }

        private double GetMaximumXValue()
        {
            double maxValue = Double.MinValue;

            for (int i = 0; i < lineGraph.Lines.Count; i++)
            {
                Line line = lineGraph.Lines[i];

                for (int j = 0; j < line.Points.Count; j++)
                {
                    LinePoint p = line.Points[j];

                    if (p.XValue > maxValue)
                        maxValue = p.XValue;
                }
            }

            return maxValue;
        }

        private double GetPositiveHeightPixels()
        {
            if (minimumYValue >= 0.0 && maximumYValue >= 0)
                return drawingAreaHeightPixels;

            if (minimumYValue <= 0.0 && maximumYValue <= 0)
                return 0.0;

            if (minimumYValue > maximumYValue)
                throw new Exception("Error: Minimum value is more than Maximum value");

            return (drawingAreaHeightPixels*maximumYValue)/totalYValue;
        }

        private double GetNegativeHeightPixels()
        {
            if (minimumYValue >= 0.0 && maximumYValue >= 0)
                return 0.0;

            if (minimumYValue <= 0.0 && maximumYValue <= 0)
                return drawingAreaHeightPixels;

            if (minimumYValue > maximumYValue)
                throw new Exception("Error: Minimum value is more than Maximum value");

            double retVal = (drawingAreaHeightPixels*minimumYValue)/totalYValue;
            if (retVal < 0.0)
                retVal = retVal*(-1);
            return retVal;
        }

        private double GetGridHeightValue()
        {
            if (lineGraph.GridSpacingValue != 0)
                return lineGraph.GridSpacingValue;

            double retVal;
            double max = maximumYValue;
            double min = minimumYValue;

            if (maximumYValue < 0.0)
                max = maximumYValue*(-1);

            if (minimumYValue < 0.0)
                min = minimumYValue*(-1);

            if (max > min)
                retVal = max/10;
            else
                retVal = min/10;

            if (lineGraph.RoundOffGridHeight)
                retVal = Math.Ceiling(retVal);

            if (retVal >= 3.0)
            {
                double temp = retVal%5;
                retVal = retVal + (5.0 - temp);
            }

            return retVal;
        }

        private double GetGridHeightPixels()
        {
            return (gridHeightValue*drawingAreaHeightPixels)/totalYValue;
        }

        // The return value is to be used as is. It is calculated w.r.t. drawingAreaRectangle.Left
        private double MappedXCoordinatePixels(double value)
        {
            return drawingAreaRectangle.Left + value;
        }

        // The return value is to be used as is. It is calculated w.r.t. drawingAreaRectangle.Left
        private double MappedXCoordinateValue(double value)
        {
            if (value < 0)
                return drawingAreaRectangle.Left;

            double retVal = drawingAreaRectangle.Left + (drawingAreaWidthPixels*value/maxXValue);

            if (retVal > drawingAreaRectangle.Right)
                retVal = drawingAreaRectangle.Right;

            return retVal;
        }

        // The return value is to be used as is. It is calculated w.r.t. drawingAreaRectangle.Top
        private double MappedYCoordinateValue(double value)
        {
            if (value == 0.0)
                return drawingAreaRectangle.Top + positiveHeightPixels;

            double retVal;

            if (value > 0.0) // value is positive
            {
                if (minimumYValue >= 0.0 && maximumYValue >= 0) // both positive
                {
                    double diff = totalYValue;
                    retVal = (value*drawingAreaHeightPixels)/diff;
                    retVal = drawingAreaHeightPixels - retVal;
                }
                else if (minimumYValue <= 0.0 && maximumYValue <= 0) // both negative
                {
                    throw new Exception("Error: Value is positive while both Max and Min values are negative");
                }
                else if (minimumYValue > maximumYValue) // error
                {
                    throw new Exception("Error: Minimum value is more than Maximum value");
                }
                else // Max positive and Min negative
                {
                    double diff = totalYValue;
                    retVal = (value*drawingAreaHeightPixels)/diff;
                    retVal = positiveHeightPixels - retVal;
                }
            }
            else // value is negative
            {
                value = value*(-1);
                if (minimumYValue >= 0.0 && maximumYValue >= 0) // both positive
                {
                    throw new Exception("Error: Value is negative while both Max and Mix values are positive");
                }
                else if (minimumYValue <= 0.0 && maximumYValue <= 0) // both negative
                {
                    double diff = totalYValue;
                    retVal = (value*drawingAreaHeightPixels)/diff;
                }
                else if (minimumYValue > maximumYValue) // error
                {
                    throw new Exception("Error: Minimum value is more than Maximum value");
                }
                else // Max positive and Min negative
                {
                    double diff = totalYValue;
                    retVal = (value*drawingAreaHeightPixels)/diff;
                    retVal = positiveHeightPixels + retVal;
                }
            }

            return drawingAreaRectangle.Top + retVal;
        }

        private void DrawLineGraph(Line line, ref Graphics g)
        {
            Pen gPen = new Pen(line.Color, line.Width);
            DrawLineGraph(line, gPen, ref g);
        }

        private void DrawLineGraph(Line line, Pen gPen, ref Graphics g)
        {
            if (line.Points.Count == 0)
                return;

            double pointSpacing = drawingAreaWidthPixels/(line.Points.Count - 1);
            double oldX = MappedXCoordinateValue(line.Points[0].XValue);
            double oldY = MappedYCoordinateValue(line.Points[0].YValue);
            double newX = 0;
            double newY = 0;

            for (int i = 1; i < line.Points.Count; i++)
            {
                newX = MappedXCoordinateValue(line.Points[i].XValue);
                newY = MappedYCoordinateValue(line.Points[i].YValue);
                g.DrawLine(gPen, (float) oldX, (float) oldY, (float) newX, (float) newY);
                oldX = newX;
                oldY = newY;
            }

            // Draw Projected Line
            if (lineGraph.ShowProjectedTrend && gPen.DashStyle == DashStyle.Solid) // Means Line is not a Trend Line
            {
                Pen gDashPen = new Pen(gPen.Color, gPen.Width);
                gDashPen.DashStyle = DashStyle.DashDot;
                gDashPen.DashCap = DashCap.Triangle;

                double deltaX = line.Points[line.Points.Count - 1].XValue - line.Points[0].XValue;
                double deltaY = line.Points[line.Points.Count - 1].YValue - line.Points[0].YValue;
                double progress = deltaY/deltaX;
                double deltaProjectedProgress = (maxXValue - deltaX)*progress;
                double finalYValue = progress + deltaProjectedProgress;

                newX = MappedXCoordinateValue(maxXValue);
                newY = MappedYCoordinateValue(finalYValue);

                // Guard against the boundaries
                double m = (newY - oldY)/(newX - oldX); // (y2-y1)/(x2-x1)

                if (newY < drawingAreaRectangle.Top)
                {
                    newY = drawingAreaRectangle.Top;
                    newX = (drawingAreaRectangle.Top - oldY)/m + oldX;
                }
                else if (newY > drawingAreaRectangle.Bottom)
                {
                    newY = drawingAreaRectangle.Bottom;
                    newX = (drawingAreaRectangle.Bottom - oldY)/m + oldX;
                }

                g.DrawLine(gDashPen, (float) oldX, (float) oldY, (float) newX, (float) newY);
            }
        }


        private void DrawGrid(ref Graphics g)
        {
            // Set up Font
            StringFormat format = new StringFormat(StringFormatFlags.NoClip);

            format.Alignment = StringAlignment.Far;
            format.LineAlignment = StringAlignment.Center;

            float fontSize = (float) 0.5*(float) gridHeightPixels;

            if (fontSize > (float) 10.0)
                fontSize = (float) 10.0;

            if (fontSize < (float) 1.0)
                fontSize = (float) 1.0;

            SolidBrush gBrush = new SolidBrush(Color.Black);

            // Center line
            Pen gPen = new Pen(Color.Black, (float) 2.0F);
            if (minimumYValue <= 0.0 && maximumYValue >= 0.0)
            {
                g.DrawLine(gPen, (float) MappedXCoordinatePixels(0.0), (float) MappedYCoordinateValue(0.0),
                           (float) MappedXCoordinatePixels(drawingAreaWidthPixels), (float) MappedYCoordinateValue(0.0));
                g.DrawString("0", new Font("Tahoma", fontSize), gBrush, (float) MappedXCoordinatePixels(-2),
                             (float) MappedYCoordinateValue(0), format);
            }

            if (!lineGraph.ShowGrid)
                return;

            // +ve grid lines
            gPen = new Pen(Color.Gray, (float) 0.03);
            int gridCount = (int) (positiveHeightPixels/gridHeightPixels);
            if (gridCount > 0)
                gridCount++;
            for (int i = 1; i <= gridCount; i++)
            {
                if (MappedYCoordinateValue(gridHeightValue*i) > drawingAreaRectangle.Bottom ||
                    MappedYCoordinateValue(gridHeightValue*i) < drawingAreaRectangle.Top)
                    continue;
                g.DrawLine(gPen, (float) MappedXCoordinatePixels(0.0), (float) MappedYCoordinateValue(gridHeightValue*i),
                           (float) MappedXCoordinatePixels(drawingAreaWidthPixels),
                           (float) MappedYCoordinateValue(gridHeightValue*i));
                float value = (float) gridHeightValue*i;
                g.DrawString(value.ToString(), new Font("Tahoma", fontSize), gBrush, (float) MappedXCoordinatePixels(-2),
                             (float) MappedYCoordinateValue(value), format);
            }

            // -ve grid lines
            gridCount = (int) (negativeHeightPixels/gridHeightPixels);
            if (gridCount > 0)
                gridCount++;
            for (int i = 1; i <= gridCount; i++)
            {
                if (MappedYCoordinateValue(gridHeightValue*i*(-1)) > drawingAreaRectangle.Bottom ||
                    MappedYCoordinateValue(gridHeightValue*i) < drawingAreaRectangle.Top)
                    continue;
                g.DrawLine(gPen, (float) MappedXCoordinatePixels(0.0),
                           (float) MappedYCoordinateValue(gridHeightValue*i*(-1)),
                           (float) MappedXCoordinatePixels(drawingAreaWidthPixels),
                           (float) MappedYCoordinateValue(gridHeightValue*i*(-1)));
                float value = (float) gridHeightValue*i*(-1);
                g.DrawString(value.ToString(), new Font("Tahoma", fontSize), gBrush, (float) MappedXCoordinatePixels(-2),
                             (float) MappedYCoordinateValue(value), format);
            }
        }

        private void DrawTitle(ref Graphics g)
        {
            if (lineGraph.Text == null || lineGraph.Text == String.Empty)
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
            g.DrawString(lineGraph.Text, new Font("Tahoma", fontSize, FontStyle.Bold), gBrush, 0, 0, format);
            g.TranslateTransform(-Middle.X, -Middle.Y);
        }

        private void DrawXAxisText(ref Graphics g)
        {
            if (lineGraph.XAxisTextCollection == null)
                return;

            if (lineGraph.XAxisTextCollection.Count == 0) return;

            // Set up Font
            StringFormat format = new StringFormat(StringFormatFlags.NoClip);

            SolidBrush gBrushFont = new SolidBrush(Color.Black);

            // Set up Pen and Brush for lines
            Pen gPen = new Pen(Color.Gray, (float) 0.03);
            SolidBrush gBrush = new SolidBrush(lineGraph.Color);

            double xStart;
            double xEnd;

            for (int i = 0; i < lineGraph.XAxisTextCollection.Count; i++)
            {
                xStart = lineGraph.XAxisTextCollection[i].XValueStart;
                xEnd = lineGraph.XAxisTextCollection[i].XValueEnd;
                g.DrawLine(gPen, (int) MappedXCoordinateValue(xStart), (int) drawingAreaRectangle.Top,
                           (int) MappedXCoordinateValue(xStart), (int) drawingAreaRectangle.Bottom);
                g.DrawLine(gPen, (int) MappedXCoordinateValue(xEnd), (int) drawingAreaRectangle.Top,
                           (int) MappedXCoordinateValue(xEnd), (int) drawingAreaRectangle.Bottom);
                if (xStart == xEnd)
                {
                    float fontSize = (float) 0.5*(float) gridHeightPixels;

                    if (fontSize > (float) 9.0)
                        fontSize = (float) 9.0;

                    if (fontSize < (float) 1.0)
                        fontSize = (float) 1.0;

                    //Draw Text
                    Point Middle =
                        new Point((int) MappedXCoordinateValue(xStart), (int) drawingAreaRectangle.Bottom + 4);

                    format.Alignment = StringAlignment.Far;
                    format.LineAlignment = StringAlignment.Center;

                    g.TranslateTransform(Middle.X, Middle.Y);
                    g.RotateTransform(35*(-1));
                    g.DrawString(lineGraph.XAxisTextCollection[i].Text.Trim(), new Font("Tahoma", fontSize), gBrushFont,
                                 0, 0, format);
                    g.RotateTransform(35);
                    g.TranslateTransform(-Middle.X, -Middle.Y);
                }
                else
                {
                    float fontSize = (float) 0.5*(float) gridHeightPixels;

                    if (fontSize > (float) 10.0)
                        fontSize = (float) 10.0;

                    if (fontSize < (float) 1.0)
                        fontSize = (float) 1.0;

                    format.Alignment = StringAlignment.Center;
                    format.LineAlignment = StringAlignment.Near;

                    //Draw Text
                    Point Middle =
                        new Point((int) ((MappedXCoordinateValue(xStart) + MappedXCoordinateValue(xEnd))/2),
                                  (int) drawingAreaRectangle.Bottom + 2);

                    g.TranslateTransform(Middle.X, Middle.Y);
                    g.DrawString(lineGraph.XAxisTextCollection[i].Text.Trim(), new Font("Tahoma", fontSize), gBrushFont,
                                 0, 0, format);
                    g.TranslateTransform(-Middle.X, -Middle.Y);
                }
            }
        }

        private void DrawPhaseLines(ref Graphics g)
        {
            if (lineGraph.PhaseLines == null)
                return;

            if (lineGraph.PhaseLines.Count <= 0)
                return;

            // Set up Font
            StringFormat format = new StringFormat(StringFormatFlags.NoClip);
            SolidBrush gBrushFont = new SolidBrush(Color.Red);

            // Set up Pen and Brush for lines
            Pen gPen = new Pen(Color.Red, (float) 0.03);
            double xStart;
            double xEnd;

            for (int i = 0; i < lineGraph.PhaseLines.Count; i++)
            {
                xStart = lineGraph.PhaseLines[i].XValueStart;
                xEnd = lineGraph.PhaseLines[i].XValueEnd;
                g.DrawLine(gPen, (int) MappedXCoordinateValue(xStart), (int) drawingAreaRectangle.Top,
                           (int) MappedXCoordinateValue(xStart), (int) drawingAreaRectangle.Bottom);
                g.DrawLine(gPen, (int) MappedXCoordinateValue(xEnd), (int) drawingAreaRectangle.Top,
                           (int) MappedXCoordinateValue(xEnd), (int) drawingAreaRectangle.Bottom);
                if (xStart == xEnd)
                {
                    float fontSize = (float) 0.9*(float) marginForPhaseLinesText;

                    if (fontSize > (float) 8.0)
                        fontSize = (float) 8.0;

                    if (fontSize < (float) 1.0)
                        fontSize = (float) 1.0;

                    //Draw Text
                    Point Middle = new Point((int) MappedXCoordinateValue(xEnd), (int) (drawingAreaRectangle.Top - 1));

                    format.Alignment = StringAlignment.Center;
                    format.LineAlignment = StringAlignment.Far;
                    g.TranslateTransform(Middle.X, Middle.Y);
                    g.DrawString(lineGraph.PhaseLines[i].Text.Trim(), new Font("Tahoma", fontSize), gBrushFont, 0, 0,
                                 format);
                    g.TranslateTransform(-Middle.X, -Middle.Y);
                }
                else
                {
                    float fontSize = (float) 0.9*(float) marginForPhaseLinesText;

                    if (fontSize > (float) 8.0)
                        fontSize = (float) 8.0;

                    if (fontSize < (float) 1.0)
                        fontSize = (float) 1.0;

                    format.Alignment = StringAlignment.Center;
                    format.LineAlignment = StringAlignment.Far;

                    //Draw Text
                    Point Middle =
                        new Point((int) ((MappedXCoordinateValue(xStart) + MappedXCoordinateValue(xEnd))/2),
                                  (int) (drawingAreaRectangle.Top - 1));

                    g.TranslateTransform(Middle.X, Middle.Y);
                    g.DrawString(lineGraph.PhaseLines[i].Text.Trim(), new Font("Tahoma", fontSize), gBrushFont, 0, 0,
                                 format);
                    g.TranslateTransform(-Middle.X, -Middle.Y);
                }
            }
        }


        private void DrawBackColor(ref Graphics g)
        {
            Pen gPen = new Pen(Color.Black, (float) 0.03);
            SolidBrush gBrush = new SolidBrush(lineGraph.Color);

            g.FillRectangle(gBrush, drawingAreaRectangle);
            g.DrawRectangle(gPen, drawingAreaRectangle);
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

        private void DrawCustomBackColorGradient(Color startColor, Color endColor, ref Graphics g)
        {
            int startR = (int) startColor.R;
            int startG = (int) startColor.G;
            int startB = (int) startColor.B;
            int endR = (int) endColor.R;
            int endG = (int) endColor.G;
            int endB = (int) endColor.B;
            double deltaR = (endR - startR)/drawingAreaWidthPixels;
            double deltaG = (endG - startG)/drawingAreaWidthPixels;
            double deltaB = (endB - startB)/drawingAreaWidthPixels;
            int currentR = startR;
            int currentG = startG;
            int currentB = startB;

            Pen gPen = new Pen(Color.Black, (float) 0.03);

            for (long  i = 0; i < drawingAreaWidthPixels; i++)
            {
                gPen.Color =
                    Color.FromArgb(currentR + (int) (deltaR*i), currentG + (int) (deltaG*i), currentB + (int) (deltaB*i));
                g.DrawLine(gPen, (int) drawingAreaRectangle.Left + i, (int) drawingAreaRectangle.Top,
                           (int) drawingAreaRectangle.Left + i, (int) drawingAreaRectangle.Bottom);
            }

            gPen.Color = Color.Black;
            g.DrawRectangle(gPen, drawingAreaRectangle);
        }

        private void DrawTrendLine(ref Graphics g)
        {
            if (lineGraph.TrendLine == null || lineGraph.TrendLine.Points == null ||
                lineGraph.TrendLine.Points.Count <= 0)
                return;

            Pen gPen = new Pen(lineGraph.TrendLine.Color, lineGraph.TrendLine.Width);
            gPen.DashStyle = DashStyle.Dash;
            gPen.DashCap = DashCap.Triangle;
            DrawLineGraph(lineGraph.TrendLine, gPen, ref g);
        }

        private void DrawBorder(ref Graphics g)
        {
            Pen gPen = new Pen(Color.Gray, (float) 0.3);
            g.DrawRectangle(gPen, (int) totalRectangle.Left, (int) totalRectangle.Top, (int) widthPixels - 1,
                            (int) heightPixels - 1);
        }

        private double GetMaxXValue()
        {
            if (lineGraph.XAxisTextCollection == null)
                return 0.0;

            if (lineGraph.XAxisTextCollection.Count == 0)
                return 0.0;

            double maxValue = Double.MinValue;

            for (int i = 0; i < lineGraph.XAxisTextCollection.Count; i++)
            {
                double x;

                if (lineGraph.XAxisTextCollection[i].XValueStart == lineGraph.XAxisTextCollection[i].XValueEnd)
                    x = lineGraph.XAxisTextCollection[i].XValueStart;
                else
                    x = (lineGraph.XAxisTextCollection[i].XValueStart > lineGraph.XAxisTextCollection[i].XValueEnd)
                            ? lineGraph.XAxisTextCollection[i].XValueStart
                            : lineGraph.XAxisTextCollection[i].XValueEnd;

                if (x > maxValue)
                    maxValue = x;
            }

            return maxValue;
        }

        private void DrawXAxisSectionMarks(ref Graphics g)
        {
            // Set up Pen and Brush for lines
            Pen gPen = new Pen(Color.Gray, (float) 0.03);
            double currentX = 0.0;
            double height = gridHeightPixels/10;

            if (height > 5.0)
                height = 5.0;

            while (currentX <= drawingAreaWidthPixels)
            {
                g.DrawLine(gPen, (int) MappedXCoordinateValue(currentX), (int) (drawingAreaRectangle.Bottom - height),
                           (int) MappedXCoordinateValue(currentX), (int) drawingAreaRectangle.Bottom);
                currentX += xAxisIntervalValue;
            }
        }
    } // class
} // namespace