using System;
using System.Drawing;

namespace Zdd.Utility.Graphs
{
    /// <summary>
    /// Summary description for DateLineGraphRenderer.
    /// </summary>
    internal class DateLineGraphRenderer
    {
        private DateLineGraph dateLineGraph;
        private DateTime startDate;
        private DateTime endDate;
        private int totalDays;
      //  private int totalHours;
        private TimeSpan timeSpan;
        private DateMode dateMode;

        /// <summary>
        /// Initializes a new instance of the <see cref="DateLineGraphRenderer"/> class.
        /// </summary>
        public DateLineGraphRenderer()
        {
            startDate = new DateTime();
            endDate = new DateTime();
        }

        /// <summary>
        /// Draws the graph.
        /// </summary>
        /// <param name="dateLineGraph">The date line graph.</param>
        /// <returns></returns>
        public Image DrawGraph(DateLineGraph dateLineGraph)
        {
            try
            {
                if (dateLineGraph == null)
                    return null;

                this.dateLineGraph = dateLineGraph;
                CalculateValues();
                SetValues();

                LineGraphRenderer lgr = new LineGraphRenderer();
                return lgr.DrawGraph(this.dateLineGraph);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Calculates the values.
        /// </summary>
        private void CalculateValues()
        {
            dateMode = dateLineGraph.DateMode;
            startDate = GetMinimumDate();
            endDate = GetMaximumDate();
            timeSpan = GetTimeSpan();
            totalDays = timeSpan.Days;
           // totalHours = timeSpan.Hours;
        }

        /// <summary>
        /// Gets the time span.
        /// </summary>
        /// <returns></returns>
        private TimeSpan GetTimeSpan()
        {
            TimeSpan ts = endDate - startDate;
            return ts;
        }

        /// <summary>
        /// Gets the minimum date.
        /// </summary>
        /// <returns></returns>
        private DateTime GetMinimumDate()
        {
            DateTime retVal = DateTime.MaxValue;

            if (dateLineGraph.DateLines == null)
                return DateTime.MinValue;

            for (int i = 0; i < dateLineGraph.DateLines.Count; i++)
            {
                DateLine dateLine = dateLineGraph.DateLines[i];

                for (int j = 0; j < dateLine.DatePoints.Count; j++)
                {
                    if (DateTime.Compare(dateLine.DatePoints[j].Date, retVal) < 0)
                        retVal = dateLine.DatePoints[j].Date;
                }
            }

            if (dateLineGraph.DateTrendLine != null && dateLineGraph.DateTrendLine.DatePoints != null &&
                dateLineGraph.DateTrendLine.DatePoints.Count > 0)
            {
                for (int j = 0; j < dateLineGraph.DateTrendLine.DatePoints.Count; j++)
                {
                    if (DateTime.Compare(dateLineGraph.DateTrendLine.DatePoints[j].Date, retVal) < 0)
                        retVal = dateLineGraph.DateTrendLine.DatePoints[j].Date;
                }
            }

            if (DateTime.Compare(dateLineGraph.StartDate, retVal) < 0)
                return dateLineGraph.StartDate;
            else
                return retVal;
        }

        private DateTime GetMaximumDate()
        {
            DateTime retVal = DateTime.MinValue;

            if (dateLineGraph.DateLines == null)
                return DateTime.MinValue;

            for (int i = 0; i < dateLineGraph.DateLines.Count; i++)
            {
                DateLine dateLine = dateLineGraph.DateLines[i];

                for (int j = 0; j < dateLine.DatePoints.Count; j++)
                {
                    if (DateTime.Compare(dateLine.DatePoints[j].Date, retVal) > 0)
                        retVal = dateLine.DatePoints[j].Date;
                }
            }

            if (dateLineGraph.DateTrendLine != null && dateLineGraph.DateTrendLine.DatePoints != null &&
                dateLineGraph.DateTrendLine.DatePoints.Count > 0)
            {
                for (int j = 0; j < dateLineGraph.DateTrendLine.DatePoints.Count; j++)
                {
                    if (DateTime.Compare(dateLineGraph.DateTrendLine.DatePoints[j].Date, retVal) > 0)
                        retVal = dateLineGraph.DateTrendLine.DatePoints[j].Date;
                }
            }

            if (DateTime.Compare(dateLineGraph.EndDate, retVal) > 0)
                return dateLineGraph.EndDate;
            else
                return retVal;
        }

        private void SetValues()
        {
            switch (dateLineGraph.DateMode)
            {
                case DateMode.Day:
                    SetValuesForDaysMode();
                    break;

                case DateMode.Weeks:
                    SetValuesForWeeksMode();
                    break;

                case DateMode.Months:
                    SetValuesForMonthsMode();
                    break;

                case DateMode.Years:
                    SetValuesForYearsMode();
                    break;

                default:
                    break;
            } // switch
            MapAndAddLines();
            MapAndAddTrendLine();
            MapAndAddPhaseLines();
        }

        private void SetValuesForDaysMode()
        {
            dateLineGraph.XAxisTextCollection = new XAxisTextCollection();
            dateLineGraph.TotalXAxisIntervals = timeSpan.Hours;
            dateLineGraph.XAxisIntervalValue = 1;

            // Set X Axis Text
            int hours = 0;
            DateTime date = startDate;

            while (hours < timeSpan.Hours)
            {
                //this.dateLineGraph.AddXAxisText(days, date.ToShortDateString());
                dateLineGraph.AddXAxisText(hours, date.Day.ToString() + "/" + date.TimeOfDay.Hours.ToString());
                date = date.AddHours(24);
                hours += 24;
            }

            //this.dateLineGraph.AddXAxisText(this.totalDays, this.endDate.ToShortDateString());
            dateLineGraph.AddXAxisText(totalDays, endDate.Day.ToString() + "/" + endDate.TimeOfDay.Hours.ToString());
        }

        private void SetValuesForWeeksMode()
        {
            dateLineGraph.XAxisTextCollection = new XAxisTextCollection();
            dateLineGraph.TotalXAxisIntervals = totalDays;
            dateLineGraph.XAxisIntervalValue = 1;

            // Set X Axis Text
            int days = 0;
            DateTime date = startDate;
            while (days < totalDays)
            {
                //this.dateLineGraph.AddXAxisText(days, date.ToShortDateString());
                dateLineGraph.AddXAxisText(days, date.Month.ToString() + "/" + date.Day.ToString());
                date = date.AddDays(7);
                days += 7;
            }
            //this.dateLineGraph.AddXAxisText(this.totalDays, this.endDate.ToShortDateString());
            dateLineGraph.AddXAxisText(totalDays, endDate.Month.ToString() + "/" + endDate.Day.ToString());
        }

        private void SetValuesForMonthsMode()
        {
            dateLineGraph.XAxisTextCollection = new XAxisTextCollection();
            dateLineGraph.TotalXAxisIntervals = totalDays/28;
            dateLineGraph.XAxisIntervalValue = 7;

            // Set X Axis Text
            int days = 0;
            DateTime date = startDate;

            while (days < totalDays)
            {
                //this.dateLineGraph.AddXAxisText(days, date.ToShortDateString());
                dateLineGraph.AddXAxisText(days, date.Month.ToString() + "/" + date.Day.ToString());
                date = date.AddDays(28);
                days += 28;
            }

            //this.dateLineGraph.AddXAxisText(this.totalDays, this.endDate.ToShortDateString());
            dateLineGraph.AddXAxisText(totalDays, endDate.Month.ToString() + "/" + endDate.Day.ToString());
        }

        private void SetValuesForYearsMode()
        {
            dateLineGraph.XAxisTextCollection = new XAxisTextCollection();
            dateLineGraph.TotalXAxisIntervals = totalDays/(12*28);
            dateLineGraph.XAxisIntervalValue = 28;

            // Set X Axis Text
            int days = 0;
            DateTime date = startDate;

            while (days < totalDays)
            {
                //this.dateLineGraph.AddXAxisText(days, date.ToShortDateString());
                dateLineGraph.AddXAxisText(days, date.Month.ToString() + "/" + date.Year.ToString());
                date = date.AddDays(12*28);
                days += (12*28);
            }
            //this.dateLineGraph.AddXAxisText(this.totalDays, this.endDate.ToShortDateString());
            dateLineGraph.AddXAxisText(totalDays, endDate.Month.ToString() + "/" + endDate.Year.ToString());
        }

        private void MapAndAddLines()
        {
            dateLineGraph.Lines = new LineCollection();

            // Map dates to X Values
            for (int i = 0; i < dateLineGraph.DateLines.Count; i++)
            {
                DateLine dateLine = dateLineGraph.DateLines[i];
                dateLine.Points = new LinePointCollection();
                for (int j = 0; j < dateLine.DatePoints.Count; j++)
                {
                    DateLinePoint point = dateLine.DatePoints[j];
                    DateTime date = point.Date;
                    TimeSpan ts = date - startDate;

                    if (dateMode == DateMode.Day || dateMode == DateMode.HalfDay)
                        point.XValue = ts.Hours;
                    else
                        point.XValue = ts.Days;

                    dateLine.Points.Add(point);
                }

                dateLineGraph.Lines.Add(dateLine);
            }
        }

        private void MapAndAddTrendLine()
        {
            if (dateLineGraph.DateTrendLine == null || dateLineGraph.DateTrendLine.DatePoints == null ||
                dateLineGraph.DateTrendLine.DatePoints.Count <= 0)
                return;

            DateLine trendLine = dateLineGraph.DateTrendLine;
            trendLine.Points = new LinePointCollection();

            for (int j = 0; j < trendLine.DatePoints.Count; j++)
            {
                DateLinePoint point = trendLine.DatePoints[j];
                DateTime date = point.Date;
                TimeSpan ts = date - startDate;

                if (dateMode == DateMode.Day || dateMode == DateMode.HalfDay)
                    point.XValue = ts.Hours;
                else
                    point.XValue = ts.Days;

                trendLine.Points.Add(point);
            }

            dateLineGraph.TrendLine = trendLine;
        }

        private void MapAndAddPhaseLines()
        {
            if (dateLineGraph.DatePhaseLines == null || dateLineGraph.DatePhaseLines.Count <= 0)
                return;

            for (int i = 0; i < dateLineGraph.DatePhaseLines.Count; i++)
            {
                DateXAxisText datePhaseLine = dateLineGraph.DatePhaseLines[i];
                XAxisText phaseLine = new XAxisText();

                // Start value
                DateTime startDate = datePhaseLine.StartDate;
                TimeSpan ts = startDate - this.startDate;

                if (dateMode == DateMode.Day || dateMode == DateMode.HalfDay)
                    phaseLine.XValueStart = ts.Hours;
                else
                    phaseLine.XValueStart = ts.Days;

                // End value
                DateTime endDate = datePhaseLine.EndDate;
                ts = endDate - this.startDate;

                if (dateMode == DateMode.Day || dateMode == DateMode.HalfDay)
                    phaseLine.XValueEnd = ts.Hours;
                else
                    phaseLine.XValueEnd = ts.Days;

                //Text
                phaseLine.Text = datePhaseLine.Text;
                dateLineGraph.PhaseLines.Add(phaseLine);
            }
        }
    }
}