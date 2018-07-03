using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Windows.Forms;

namespace Zdd.Utility.Graphs
{
    /// <summary>
    /// Summary description for BarGraphRenderer.
    /// </summary>
    internal class BarGraphRenderer
    {
        private double widthPixels;
        private double heightPixels;
        private double drawingAreaWidthPixels;
        private double drawingAreaHeightPixels;
        private double minimumValue;
        private double maximumValue;
        private double totalValue;
        private double positiveHeightPixels; // In Drawing Area
        private double negativeHeightPixels; // In Drawing Area
        private double gridHeightValue;
        private double gridHeightPixels;
        private double titleHeightPixels;
        private int totalSlices;
        private double barSliceAllotedWidth;
        private double barGap;
        private double barSliceWidth;

        private Rectangle totalRectangle;
        private Rectangle drawingAreaRectangle;

        private BarGraph barGraph;

        private int marginForTextOnXAxis;
        private readonly int marginForNumbersOnYAxis = 15;
        private readonly double maxTitleHeightPixels = 15.0;
        private readonly float maxFontSizeVerticalBottom = 10.0F;
        private readonly float maxFontSizeHorizontalLeft = 7.0F;
        private float maxFontSize;

        /// <summary>
        /// Initializes a new instance of the <see cref="BarGraphRenderer"/> class.
        /// </summary>
        public BarGraphRenderer()
        {
        }

        /// <summary>
        /// Draws the graph.
        /// </summary>
        /// <param name="barGraph">The bar graph.</param>
        /// <returns></returns>
        public Image DrawGraph(BarGraph barGraph)
        {
            if (barGraph == null)
                return null;

            this.barGraph = barGraph;

            Bitmap bMap = null;
            if (this.barGraph.Alignment == Alignment.VerticalBottom)
                bMap = new Bitmap(barGraph.Size.Width, barGraph.Size.Height, PixelFormat.Format64bppPArgb);
            else if (this.barGraph.Alignment == Alignment.HorizontalLeft)
                bMap = new Bitmap(barGraph.Size.Height, barGraph.Size.Width, PixelFormat.Format64bppPArgb);

            Graphics g = Graphics.FromImage(bMap);

            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.Clear(Color.White);
            DrawVerticalBottomGraph(ref g);
            switch (barGraph.Alignment)
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
                DrawBackColorGradient(barGraph.Color, barGraph.ColorGradient, ref g);
                //DrawCustomBackColorGradient(Color.White, Color.DarkGray, ref g);
                DrawBarGraph(ref g);
                DrawGrid(ref g);
                DrawCutOff(ref g);
                DrawTitle(ref g);
                DrawBorder(ref g);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occured: \n" + ex.ToString(), "Graphing Message", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Calculates the values.
        /// </summary>
        private void CalculateValues()
        {
            marginForTextOnXAxis = barGraph.MarginForTextOnAxis;

            if (barGraph.Alignment == Alignment.VerticalBottom)
            {
                widthPixels = barGraph.Size.Width;
                heightPixels = barGraph.Size.Height;
            }
            else if (barGraph.Alignment == Alignment.HorizontalLeft)
            {
                widthPixels = barGraph.Size.Height;
                heightPixels = barGraph.Size.Width;
            }

            drawingAreaWidthPixels = widthPixels - (2*barGraph.Border) - marginForNumbersOnYAxis;
            drawingAreaHeightPixels = heightPixels - (2*barGraph.Border) - marginForTextOnXAxis;
            if (barGraph.Text != null && barGraph.Text != String.Empty)
            {
                if (barGraph.Alignment == Alignment.VerticalBottom)
                {
                    titleHeightPixels = drawingAreaHeightPixels*0.1;
                    if (titleHeightPixels > maxTitleHeightPixels)
                        titleHeightPixels = maxTitleHeightPixels;
                    drawingAreaHeightPixels = drawingAreaHeightPixels - titleHeightPixels;
                }
                else if (barGraph.Alignment == Alignment.HorizontalLeft)
                {
                    titleHeightPixels = drawingAreaWidthPixels*0.1;
                    if (titleHeightPixels > maxTitleHeightPixels)
                        titleHeightPixels = maxTitleHeightPixels;
                    drawingAreaWidthPixels = drawingAreaWidthPixels - titleHeightPixels;
                }
            }

            minimumValue = GetMinimumValue();
            maximumValue = GetMaximumValue();
            totalValue = GetTotalValue();

            positiveHeightPixels = GetPositiveHeightPixels();
            negativeHeightPixels = GetNegativeHeightPixels();

            gridHeightValue = GetGridHeightValue();
            gridHeightPixels = GetGridHeightPixels();

            totalSlices = GetTotalSlices();
            barSliceAllotedWidth = GetBarSliceAllotedWidth();
            barGap = barGraph.BarGap;
            barSliceWidth = GetBarSliceWidth();

            totalRectangle = new Rectangle(0, 0, (int) widthPixels, (int) heightPixels);

            if (barGraph.Alignment == Alignment.VerticalBottom)
            {
                drawingAreaRectangle =
                    new Rectangle((int) barGraph.Border + marginForNumbersOnYAxis,
                                  (int) barGraph.Border + (int) titleHeightPixels, (int) drawingAreaWidthPixels,
                                  (int) drawingAreaHeightPixels);
                new Rectangle((int) barGraph.Border, (int) barGraph.Border, (int) drawingAreaWidthPixels,
                              (int) titleHeightPixels);
            }
            else if (barGraph.Alignment == Alignment.HorizontalLeft)
            {
                drawingAreaRectangle =
                    new Rectangle((int) barGraph.Border + marginForNumbersOnYAxis, (int) barGraph.Border,
                                  (int) drawingAreaWidthPixels, (int) drawingAreaHeightPixels);
                new Rectangle((int) drawingAreaRectangle.Right, (int) barGraph.Border, (int) titleHeightPixels,
                              (int) drawingAreaHeightPixels);
            }

            maxFontSize = GetMaxFontSize();
        }

        /// <summary>
        /// Gets the size of the max font.
        /// </summary>
        /// <returns></returns>
        private float GetMaxFontSize()
        {
            switch (barGraph.Alignment)
            {
                case Alignment.VerticalBottom:
                    return maxFontSizeVerticalBottom;
                case Alignment.HorizontalLeft:
                    return maxFontSizeHorizontalLeft;
                default:
                    return 8.0F;
            }
        }

        /// <summary>
        /// Gets the minimum value.
        /// </summary>
        /// <returns></returns>
        private double GetMinimumValue()
        {
            double minValue = Double.MaxValue;

            BarSliceCollection barSliceCollection = barGraph.BarSliceCollection;

            if (barSliceCollection == null || barSliceCollection.Count == 0)
                return 0.0;

            if (barGraph.MultiBarDisplayStyle == MultiBarDisplayStyle.SingleBar)
            {
                foreach (BarSlice barSlice in barSliceCollection)
                {
                    if (barSlice.Value < minValue)
                        minValue = barSlice.Value;
                }
            }
            else
            {
                foreach (BarSlice barSlice in barSliceCollection)
                {
                    if (barSlice.GetType().Name == "BarSlice")
                    {
                        if (barSlice.Value < minValue)
                            minValue = barSlice.Value;
                    }
                    else
                    {
                        if (((MultipleBarSlice) barSlice).MinValue < minValue)
                            minValue = ((MultipleBarSlice) barSlice).MinValue;
                    }
                }
            }

            if (minValue > 0)
                minValue = 0;

            if (barGraph.RoundOffGridHeight)
                if (minValue < 0.0 && minValue > -10.0)
                    minValue = -10.0;

            return minValue;
        }

        /// <summary>
        /// Gets the maximum value.
        /// </summary>
        /// <returns></returns>
        private double GetMaximumValue()
        {
            double maxValue = Double.MinValue;

            BarSliceCollection barSliceCollection = barGraph.BarSliceCollection;

            if (barSliceCollection == null || barSliceCollection.Count == 0)
                return 0;

            if (barGraph.MultiBarDisplayStyle == MultiBarDisplayStyle.SingleBar)
            {
                foreach (BarSlice barSlice in barSliceCollection)
                {
                    if (barSlice.Value > maxValue)
                        maxValue = barSlice.Value;
                }
            }
            else
            {
                foreach (BarSlice barSlice in barSliceCollection)
                {
                    if (barSlice.GetType().Name == "BarSlice")
                    {
                        if (barSlice.Value > maxValue)
                            maxValue = barSlice.Value;
                    }
                    else
                    {
                        if (((MultipleBarSlice) barSlice).MaxValue > maxValue)
                            maxValue = ((MultipleBarSlice) barSlice).MaxValue;
                    }
                }
            }

            if (barGraph.RoundOffGridHeight)
                if (maxValue < 10.0)
                    maxValue = 10.0;

            if (barGraph.HonorScale && maxValue < barGraph.MaxScaleValue) return barGraph.MaxScaleValue;
            else
                return maxValue;
        }

        /// <summary>
        /// Gets the total value.
        /// </summary>
        /// <returns></returns>
        private double GetTotalValue()
        {
            double retVal = maximumValue - minimumValue;

            if (retVal < 0.0)
                retVal = retVal*(-1.0);

            return retVal;
        }

        private double GetPositiveHeightPixels()
        {
            if (minimumValue >= 0.0 && maximumValue >= 0)
                return drawingAreaHeightPixels;

            if (minimumValue <= 0.0 && maximumValue <= 0)
                return 0.0;

            if (minimumValue > maximumValue)
                throw new Exception("Error: Minimum value is more than Maximum value");

            return (drawingAreaHeightPixels*maximumValue)/totalValue;
        }

        private double GetNegativeHeightPixels()
        {
            if (minimumValue >= 0.0 && maximumValue >= 0)
                return 0.0;

            if (minimumValue <= 0.0 && maximumValue <= 0)
                return drawingAreaHeightPixels;

            if (minimumValue > maximumValue)
                throw new Exception("Error: Minimum value is more than Maximum value");

            double retVal = (drawingAreaHeightPixels*minimumValue)/totalValue;

            if (retVal < 0.0)
                retVal = retVal*(-1);

            return retVal;
        }

        private double GetGridHeightValue()
        {
            if (barGraph.GridSpacingValue != 0)
                return barGraph.GridSpacingValue;

            double retVal;
            double max = maximumValue;
            double min = minimumValue;

            if (maximumValue < 0.0)
                max = maximumValue*(-1);

            if (minimumValue < 0.0)
                min = minimumValue*(-1);

            if (max > min)
                retVal = max/10;
            else
                retVal = min/10;

            if (barGraph.RoundOffGridHeight)
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
            return (gridHeightValue*drawingAreaHeightPixels)/totalValue;
        }

        // The return value is to be used as is. It is calculated w.r.t. _drawingAreaRectangle.Left
        private double MappedXCoordinate(double value)
        {
            return drawingAreaRectangle.Left + value;
        }

        // The return value is to be used as is. It is calculated w.r.t. _drawingAreaRectangle.Top
        private double MappedYCoordinate(double value)
        {
            if (value == 0.0)
                return drawingAreaRectangle.Top + positiveHeightPixels;

            double retVal;

            if (value > 0.0) // value is positive
            {
                if (minimumValue >= 0.0 && maximumValue >= 0) // both positive
                {
                    double diff = totalValue;

                    retVal = (value*drawingAreaHeightPixels)/diff;
                    retVal = drawingAreaHeightPixels - retVal;
                }
                else if (minimumValue <= 0.0 && maximumValue <= 0) // both negative
                {
                    throw new Exception("Error: Value is positive while both Max and Mix values are negative");
                }
                else if (minimumValue > maximumValue) // error
                {
                    throw new Exception("Error: Minimum value is more than Maximum value");
                }
                else // Max positive and Min negative
                {
                    double diff = totalValue;

                    retVal = (value*drawingAreaHeightPixels)/diff;
                    retVal = positiveHeightPixels - retVal;
                }
            }
            else // value is negative
            {
                value = value*(-1);
                if (minimumValue >= 0.0 && maximumValue >= 0) // both positive
                {
                    throw new Exception("Error: Value is negative while both Max and Mix values are positive");
                }
                else if (minimumValue <= 0.0 && maximumValue <= 0) // both negative
                {
                    double diff = totalValue;

                    retVal = (value*drawingAreaHeightPixels)/diff;
                }
                else if (minimumValue > maximumValue) // error
                {
                    throw new Exception("Error: Minimum value is more than Maximum value");
                }
                else // Max positive and Min negative
                {
                    double diff = totalValue;

                    retVal = (value*drawingAreaHeightPixels)/diff;
                    retVal = positiveHeightPixels + retVal;
                }
            }

            return drawingAreaRectangle.Top + retVal;
        }

        private int GetTotalSlices()
        {
            if (barGraph.BarSliceCollection == null)
                return 0;
            else
                return barGraph.BarSliceCollection.Count;
        }

        private double GetBarSliceAllotedWidth()
        {
            if (totalSlices == 0.0)
                return drawingAreaWidthPixels;
            else
                return drawingAreaWidthPixels/totalSlices;
        }

        private double GetBarSliceWidth()
        {
            double retVal = barSliceAllotedWidth - barGap;
            if (retVal <= 0.0) // -ve
            {
                if (barSliceAllotedWidth >= 1.0)
                {
                    retVal = 1.0;
                    barGap = barSliceAllotedWidth - 1.0;
                    return retVal;
                }
                else
                {
                    throw new Exception("Bar Slice Width is less than or equal to 1. Please increase the BarGraph Width");
                }
            }
            else // +ve
            {
                /*if(retVal<this.barGraph.MaxBarSliceWidth)
					return retVal;
				else
					return this.barGraph.MaxBarSliceWidth;*/
                if (retVal > barGraph.MaxBarSliceWidth)
                    return barGraph.MaxBarSliceWidth;
                else
                    return retVal;
            }
        }

        private void DrawBarGraph(ref Graphics g)
        {
            if (totalSlices == 0)
                return;

            for (int i = 0; i < totalSlices; i++)
            {
                DrawBarSlice(i, ref g);
                DrawText(i, ref g);
            }
        }

        private void DrawBarSlice(int index, ref Graphics g)
        {
            BarSlice bar = barGraph.BarSliceCollection[index];
            double width = barSliceWidth;
            if (width > bar.MaxWidth)
                width = bar.MaxWidth;

            Pen gPen = new Pen(Color.Black, (float) 0.03);
            SolidBrush gBrush = new SolidBrush(barGraph.Color);
            LinearGradientBrush lgBrush = null;

            int startX = (int) ((barSliceAllotedWidth*index) + (barSliceAllotedWidth/2) - (width/2));
            Rectangle rect;
            Rectangle shadowRect;

            if (bar.GetType().Name != "MultipleBarSlice")
            {
                if (bar.Value >= 0)
                {
                    rect =
                        new Rectangle((int) MappedXCoordinate(startX), (int) MappedYCoordinate(bar.Value), (int) width,
                                      (int) ((bar.Value*drawingAreaHeightPixels)/totalValue));
                    shadowRect =
                        new Rectangle((int) MappedXCoordinate(startX) + 2, (int) MappedYCoordinate(bar.Value) - 3,
                                      (int) width, (int) ((bar.Value*drawingAreaHeightPixels)/totalValue));
                    try
                    {
                        lgBrush = new LinearGradientBrush(rect, bar.Color, bar.ColorGradient, (float) (45*(-1)), true);
                    }
                    catch
                    {
                    }
                    ;
                }
                else
                {
                    rect =
                        new Rectangle((int) MappedXCoordinate(startX), (int) MappedYCoordinate(0.0), (int) width,
                                      (int) ((bar.Value*(-1)*drawingAreaHeightPixels)/totalValue));
                    shadowRect =
                        new Rectangle((int) MappedXCoordinate(startX) + 2, (int) MappedYCoordinate(0.0) + 3, (int) width,
                                      (int) ((bar.Value*(-1)*drawingAreaHeightPixels)/totalValue));
                    try
                    {
                        lgBrush = new LinearGradientBrush(rect, bar.Color, bar.ColorGradient, (float) (45), true);
                    }
                    catch
                    {
                    }
                    ;
                }

                if (lgBrush != null)
                {
                    gBrush.Color = Color.LightGray;
                    g.FillRectangle(gBrush, shadowRect);
                    gBrush.Color = bar.Color;

                    //g.FillRectangle(gBrush, rect);
                    g.FillRectangle(lgBrush, rect);
                    g.DrawRectangle(gPen, rect);
                }
            }
            else
            {
                MultipleBarSlice mbs = (MultipleBarSlice) bar;
                if (mbs.PartialValues == null)
                    return;
                if (mbs.PartialValues.Length == 0)
                    return;

                if (barGraph.MultiBarDisplayStyle == MultiBarDisplayStyle.SeparateBars) // SeparateBars
                {
                    double partialValue = 0;
                    double cumulativeValue = 0;

                    for (int i = 0; i < mbs.PartialValues.Length; i++)
                    {
                        partialValue = mbs.PartialValues[i];
                        cumulativeValue += mbs.PartialValues[i];
                        if (partialValue >= 0)
                        {
                            rect =
                                new Rectangle((int) MappedXCoordinate(startX + (i*width/mbs.PartialValues.Length)),
                                              (int) MappedYCoordinate(partialValue),
                                              (int) (width/mbs.PartialValues.Length),
                                              (int) ((partialValue*drawingAreaHeightPixels)/totalValue));
                            shadowRect =
                                new Rectangle((int) MappedXCoordinate(startX + (i*width/mbs.PartialValues.Length)) + 2,
                                              (int) MappedYCoordinate(partialValue) - 3,
                                              (int) (width/mbs.PartialValues.Length),
                                              (int) ((partialValue*drawingAreaHeightPixels)/totalValue));
                        }
                        else
                        {
                            rect =
                                new Rectangle((int) MappedXCoordinate(startX + (i*width/mbs.PartialValues.Length)),
                                              (int) MappedYCoordinate(0), (int) (width/mbs.PartialValues.Length),
                                              (int) ((partialValue*(-1)*drawingAreaHeightPixels)/totalValue));
                            shadowRect =
                                new Rectangle((int) MappedXCoordinate(startX + (i*width/mbs.PartialValues.Length)) + 2,
                                              (int) MappedYCoordinate(0) + 3, (int) (width/mbs.PartialValues.Length),
                                              (int) ((partialValue*(-1)*drawingAreaHeightPixels)/totalValue));
                        }

                        gBrush.Color = Color.LightGray;
                        g.FillRectangle(gBrush, shadowRect);
                        gBrush.Color = mbs.PartialColors[i];
                        g.FillRectangle(gBrush, rect);
                        g.DrawRectangle(gPen, rect);
                    } // for
                } // if SeparateBars

                else // SingleBar
                {
                    double partialValue = 0;
                    double cumulativeValue = 0;

                    for (int i = 0; i < mbs.PartialValues.Length; i++)
                    {
                        partialValue = mbs.PartialValues[i];
                        cumulativeValue += mbs.PartialValues[i];
                        if (partialValue >= 0)
                        {
                            rect =
                                new Rectangle((int) MappedXCoordinate(startX), (int) MappedYCoordinate(cumulativeValue),
                                              (int) width, (int) ((partialValue*drawingAreaHeightPixels)/totalValue));
                            shadowRect =
                                new Rectangle((int) MappedXCoordinate(startX) + 2,
                                              (int) MappedYCoordinate(cumulativeValue) - 3, (int) width,
                                              (int) ((partialValue*drawingAreaHeightPixels)/totalValue));
                        }
                        else
                        {
                            rect =
                                new Rectangle((int) MappedXCoordinate(startX),
                                              (int) MappedYCoordinate(cumulativeValue - partialValue), (int) width,
                                              (int) ((partialValue*(-1)*drawingAreaHeightPixels)/totalValue));
                            shadowRect =
                                new Rectangle((int) MappedXCoordinate(startX) + 2,
                                              (int) MappedYCoordinate(cumulativeValue - partialValue) + 3, (int) width,
                                              (int) ((partialValue*(-1)*drawingAreaHeightPixels)/totalValue));
                        }

                        gBrush.Color = Color.LightGray;
                        g.FillRectangle(gBrush, shadowRect);
                        gBrush.Color = mbs.PartialColors[i];
                        g.FillRectangle(gBrush, rect);
                        g.DrawRectangle(gPen, rect);
                    } // for
                } // else SingleBar
            } // else -> MultipleBarSlice
        } // DrawBarSlice function

        private void DrawText(int index, ref Graphics g)
        {
            // Set up Font
            SolidBrush gBrush = new SolidBrush(Color.Black);
            StringFormat format = new StringFormat(StringFormatFlags.NoClip);

            float fontSize = (float) 0.5*(float) gridHeightPixels;

            if (fontSize > (float) 7.0)
                fontSize = (float) 7.0;

            if (fontSize < (float) 1.0)
                fontSize = (float) 1.0;

            BarSlice bar = barGraph.BarSliceCollection[index];
            double width = barSliceWidth;
            if (width > bar.MaxWidth)
                width = bar.MaxWidth;

            Rectangle rect;
            Rectangle shadowRect;
            int startX = (int) ((barSliceAllotedWidth*index) + (barSliceAllotedWidth/2) - (width/2));
            if (bar.Value >= 0)
            {
                rect =
                    new Rectangle((int) MappedXCoordinate(startX), (int) MappedYCoordinate(bar.Value), (int) width,
                                  (int) ((bar.Value*drawingAreaHeightPixels)/totalValue));
                shadowRect =
                    new Rectangle((int) MappedXCoordinate(startX) + 2, (int) MappedYCoordinate(bar.Value) - 3,
                                  (int) width, (int) ((bar.Value*drawingAreaHeightPixels)/totalValue));

                if (barGraph.Alignment == Alignment.VerticalBottom)
                {
                    //Draw Text
                    format.Alignment = StringAlignment.Far;
                    format.LineAlignment = StringAlignment.Center;

                    Point Middle = new Point(rect.Left + (int) (rect.Width/2), rect.Bottom + 4);

                    g.TranslateTransform(Middle.X, Middle.Y);
                    g.RotateTransform((float) 45.0*-1);
                    g.DrawString(bar.Text, new Font("Tahoma", fontSize), gBrush, 0, 0, format);
                    g.RotateTransform((float) 45.0);
                    g.TranslateTransform(-Middle.X, -Middle.Y);
                }
                else if (barGraph.Alignment == Alignment.HorizontalLeft)
                {
                    //Draw Text
                    format.Alignment = StringAlignment.Far;
                    format.LineAlignment = StringAlignment.Center;

                    Point Middle = new Point(rect.Left + (int) (rect.Width/2), rect.Bottom + 4);

                    g.TranslateTransform(Middle.X, Middle.Y);
                    g.RotateTransform((float) 245.0);
                    g.DrawString(bar.Text, new Font("Tahoma", fontSize), gBrush, 0, 0, format);
                    g.RotateTransform((float) 245.0*-1);
                    g.TranslateTransform(-Middle.X, -Middle.Y);
                }
            }
            else
            {
                rect =
                    new Rectangle((int) MappedXCoordinate(startX), (int) MappedYCoordinate(0.0), (int) width,
                                  (int) ((bar.Value*(-1)*drawingAreaHeightPixels)/totalValue));
                shadowRect =
                    new Rectangle((int) MappedXCoordinate(startX) + 2, (int) MappedYCoordinate(0.0) + 3, (int) width,
                                  (int) ((bar.Value*(-1)*drawingAreaHeightPixels)/totalValue));

                if (barGraph.Alignment == Alignment.VerticalBottom)
                {
                    //Draw Text
                    format.Alignment = StringAlignment.Near;
                    format.LineAlignment = StringAlignment.Center;

                    Point Middle = new Point(rect.Left + (int) (rect.Width/2), rect.Top - 4);

                    g.TranslateTransform(Middle.X, Middle.Y);
                    g.RotateTransform((float) 45.0*-1);
                    g.DrawString(bar.Text, new Font("Tahoma", fontSize), gBrush, 0, 0, format);
                    g.RotateTransform((float) 45.0);
                    g.TranslateTransform(-Middle.X, -Middle.Y);
                }
                else if (barGraph.Alignment == Alignment.HorizontalLeft)
                {
                    //Draw Text
                    format.Alignment = StringAlignment.Near;
                    format.LineAlignment = StringAlignment.Center;

                    Point Middle = new Point(rect.Left + (int) (rect.Width/2), rect.Top - 4);

                    g.TranslateTransform(Middle.X, Middle.Y);
                    g.RotateTransform((float) 245.0);
                    g.DrawString(bar.Text, new Font("Tahoma", fontSize), gBrush, 0, 0, format);
                    g.RotateTransform((float) 245.0*-1);
                    g.TranslateTransform(-Middle.X, -Middle.Y);
                }
            }
        }

        private void DrawGrid(ref Graphics g)
        {
            // Center line
            Pen gPen = new Pen(Color.Black, (float) 2.0F);

            if (minimumValue <= 0.0 && maximumValue >= 0.0)
            {
                g.DrawLine(gPen, (float) MappedXCoordinate(0.0), (float) MappedYCoordinate(0.0),
                           (float) MappedXCoordinate(drawingAreaWidthPixels), (float) MappedYCoordinate(0.0));
                DrawNumberNextToGrid(0, Color.Black, ref g);
            }

            if (!barGraph.ShowGrid)
                return;

            // +ve grid lines
            gPen = new Pen(Color.Gray, (float) 0.03);

            int gridCount = (int) (positiveHeightPixels/gridHeightPixels);

            if (gridCount > 0)
                gridCount++;

            for (int i = 1; i <= gridCount; i++)
            {
                if (MappedYCoordinate(gridHeightValue*i) > drawingAreaRectangle.Bottom ||
                    MappedYCoordinate(gridHeightValue*i) < drawingAreaRectangle.Top)
                    continue;

                g.DrawLine(gPen, (float) MappedXCoordinate(0.0), (float) MappedYCoordinate(gridHeightValue*i),
                           (float) MappedXCoordinate(drawingAreaWidthPixels),
                           (float) MappedYCoordinate(gridHeightValue*i));
                float value = (float) gridHeightValue*i;
                DrawNumberNextToGrid(value, Color.Black, ref g);
            }

            // -ve grid lines
            gridCount = (int) (negativeHeightPixels/gridHeightPixels);
            if (gridCount > 0)
                gridCount++;

            for (int i = 1; i <= gridCount; i++)
            {
                if (MappedYCoordinate(gridHeightValue*i*(-1)) > drawingAreaRectangle.Bottom ||
                    MappedYCoordinate(gridHeightValue*i) < drawingAreaRectangle.Top)
                    continue;

                g.DrawLine(gPen, (float) MappedXCoordinate(0.0), (float) MappedYCoordinate(gridHeightValue*i*(-1)),
                           (float) MappedXCoordinate(drawingAreaWidthPixels),
                           (float) MappedYCoordinate(gridHeightValue*i*(-1)));
                float value = (float) gridHeightValue*i*(-1);
                DrawNumberNextToGrid(value, Color.Black, ref g);
            }
        }

        private void DrawNumberNextToGrid(float value, Color color, ref Graphics g)
        {
            // Set up Font
            StringFormat format = new StringFormat(StringFormatFlags.NoClip);
            float fontSize = (float) 0.5*(float) gridHeightPixels;

            if (fontSize > (float) maxFontSize)
                fontSize = (float) maxFontSize;

            if (fontSize < (float) 1.0)
                fontSize = (float) 1.0;

            SolidBrush gBrush = new SolidBrush(color);

            if (barGraph.Alignment == Alignment.VerticalBottom)
            {
                format.Alignment = StringAlignment.Far;
                format.LineAlignment = StringAlignment.Center;

                Point Middle = new Point((int) MappedXCoordinate(-2), (int) MappedYCoordinate(value));

                g.TranslateTransform(Middle.X, Middle.Y);
                g.RotateTransform((float) 0.0*-1);
                g.DrawString(value.ToString(), new Font("Tahoma", fontSize), gBrush, 0, 0, format);
                g.RotateTransform((float) 0.0);
                g.TranslateTransform(-Middle.X, -Middle.Y);
            }
            else if (barGraph.Alignment == Alignment.HorizontalLeft)
            {
                format.Alignment = StringAlignment.Center;
                format.LineAlignment = StringAlignment.Center;

                Point Middle = new Point((int) MappedXCoordinate(fontSize*(-1)), (int) MappedYCoordinate(value));

                g.TranslateTransform(Middle.X, Middle.Y);
                g.RotateTransform((float) 90.0*-1);
                g.DrawString(value.ToString(), new Font("Tahoma", fontSize), gBrush, 0, 0, format);
                g.RotateTransform((float) 90.0);
                g.TranslateTransform(-Middle.X, -Middle.Y);
            }
        }

        private void DrawTitle(ref Graphics g)
        {
            if (barGraph.Text == null || barGraph.Text == String.Empty)
                return;

            // Set up Font
            StringFormat format = new StringFormat(StringFormatFlags.NoClip);

            float fontSize = (float) 0.8*(float) titleHeightPixels;

            if (fontSize > (float) 10.0)
                fontSize = (float) 10.0;

            if (fontSize < (float) 1.0)
                fontSize = (float) 1.0;

            SolidBrush gBrush = new SolidBrush(Color.Black);

            if (barGraph.Alignment == Alignment.VerticalBottom)
            {
                format.Alignment = StringAlignment.Center;
                format.LineAlignment = StringAlignment.Near;

                //Draw Title
                Point Middle = new Point((int) MappedXCoordinate(drawingAreaWidthPixels/2), (int) titleHeightPixels/2);

                g.TranslateTransform(Middle.X, Middle.Y);
                g.DrawString(barGraph.Text, new Font("Tahoma", fontSize, FontStyle.Bold), gBrush, 0, 0, format);
                g.TranslateTransform(-Middle.X, -Middle.Y);
            }
            else if (barGraph.Alignment == Alignment.HorizontalLeft)
            {
                format.Alignment = StringAlignment.Center;
                format.LineAlignment = StringAlignment.Near;

                //Draw Title
                Point Middle =
                    new Point((int) MappedXCoordinate(drawingAreaWidthPixels + titleHeightPixels/2),
                              (int) drawingAreaHeightPixels/2);

                g.TranslateTransform(Middle.X, Middle.Y);
                g.RotateTransform((float) (90*-1));
                g.DrawString(barGraph.Text, new Font("Tahoma", fontSize, FontStyle.Bold), gBrush, 0, 0, format);
                g.RotateTransform((float) 90);
                g.TranslateTransform(-Middle.X, -Middle.Y);
            }
        }

        private void DrawBackColor(ref Graphics g)
        {
            Pen gPen = new Pen(Color.Black, (float) 0.03);
            SolidBrush gBrush = new SolidBrush(barGraph.Color);
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

        private void DrawCutOff(ref Graphics g)
        {
            if (barGraph.CutOff == Double.MinValue)
                return;

            Pen gPen = new Pen(Color.Red, (float) 1.5);
            g.DrawLine(gPen, (int) drawingAreaRectangle.Left, (int) MappedYCoordinate(barGraph.CutOff),
                       (int) drawingAreaRectangle.Right, (int) MappedYCoordinate(barGraph.CutOff));
            DrawNumberNextToGrid((float) barGraph.CutOff, Color.Red, ref g);
        }

        private void DrawBorder(ref Graphics g)
        {
            Pen gPen = new Pen(Color.Gray, (float) 0.3);
            g.DrawRectangle(gPen, (int) totalRectangle.Left, (int) totalRectangle.Top, (int) widthPixels - 1,
                            (int) heightPixels - 1);
        }
    }
}