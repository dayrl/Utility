using System;

namespace Zdd.Utility.Graphs
{
    /// <summary>
    /// DateMode
    /// </summary>
    public enum DateMode
    {
        /// <summary>
        /// HalfDay
        /// </summary>
        HalfDay = 1,
        /// <summary>
        /// Day
        /// </summary>
        Day = 2,
        /// <summary>
        /// Weeks
        /// </summary>
        Weeks = 3,
        /// <summary>
        /// Months
        /// </summary>
        Months = 4,
        /// <summary>
        /// Years
        /// </summary>
        Years = 5
    }

    /// <summary>
    /// Summary description for DateLineGraph.
    /// </summary>
    public class DateLineGraph : LineGraph
    {
        private DateTime startDate = DateTime.MaxValue;
        private DateTime endDate = DateTime.MinValue;
        private DateMode dateMode = DateMode.Weeks;
        private DateLineCollection dateLineCollection = null;
        private DateLine trendLine = null;
        private DateXAxisTextCollection dateXAxisTextCollection = null;
        private DateXAxisTextCollection datePhaseLines = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="DateLineGraph"/> class.
        /// </summary>
        public DateLineGraph()
            : base()
        {
            dateLineCollection = new DateLineCollection();
            dateXAxisTextCollection = new DateXAxisTextCollection();
            datePhaseLines = new DateXAxisTextCollection();
            trendLine = new DateLine();
            trendLine.Width = 2.0F;
            MarginForTextOnAxis = 15;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DateLineGraph"/> class.
        /// </summary>
        /// <param name="dateMode">The date mode.</param>
        public DateLineGraph(DateMode dateMode)
            : base()
        {
            this.dateMode = dateMode;
            dateLineCollection = new DateLineCollection();
            dateXAxisTextCollection = new DateXAxisTextCollection();
            datePhaseLines = new DateXAxisTextCollection();
            trendLine = new DateLine();
            trendLine.Width = 2.0F;
            MarginForTextOnAxis = 15;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DateLineGraph"/> class.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="dateMode">The date mode.</param>
        public DateLineGraph(DateTime startDate, DateTime endDate, DateMode dateMode)
            : base()
        {
            this.startDate = startDate;
            this.endDate = endDate;
            this.dateMode = dateMode;
            dateLineCollection = new DateLineCollection();
            dateXAxisTextCollection = new DateXAxisTextCollection();
            datePhaseLines = new DateXAxisTextCollection();
            trendLine = new DateLine();
            trendLine.Width = 2.0F;
            MarginForTextOnAxis = 15;
        }

        /// <summary>
        /// 获取或设置DateMode
        /// </summary>
        /// <value></value>
        public DateMode DateMode
        {
            get { return dateMode; }
            set { dateMode = value; }
        }

        /// <summary>
        /// 获取或设置StartDate
        /// </summary>
        /// <value></value>
        public DateTime StartDate
        {
            get { return startDate; }
            set { startDate = value; }
        }

        /// <summary>
        /// 获取或设置EndDate
        /// </summary>
        /// <value></value>
        public DateTime EndDate
        {
            get { return endDate; }
            set { endDate = value; }
        }

        /// <summary>
        /// 获取或设置DateLines
        /// </summary>
        /// <value></value>
        public DateLineCollection DateLines
        {
            get { return dateLineCollection; }
            set { dateLineCollection = value; }
        }

        /// <summary>
        /// 获取或设置DateTrendLine
        /// </summary>
        /// <value></value>
        public DateLine DateTrendLine
        {
            get { return trendLine; }
            set { trendLine = value; }
        }

        /// <summary>
        /// 获取或设置DateXAxisTextCollection
        /// </summary>
        /// <value></value>
        public DateXAxisTextCollection DateXAxisTextCollection
        {
            get { return dateXAxisTextCollection; }
            set { dateXAxisTextCollection = value; }
        }

        /// <summary>
        /// 获取或设置DatePhaseLines
        /// </summary>
        /// <value></value>
        public DateXAxisTextCollection DatePhaseLines
        {
            get { return datePhaseLines; }
            set { datePhaseLines = value; }
        }
    }
}