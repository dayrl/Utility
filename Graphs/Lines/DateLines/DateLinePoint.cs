using System;

namespace Zdd.Utility.Graphs
{
    /// <summary>
    /// Summary description for DatePoint.
    /// </summary>
    public class DateLinePoint : LinePoint
    {
        private DateTime date = DateTime.MinValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="DateLinePoint"/> class.
        /// </summary>
        public DateLinePoint()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DateLinePoint"/> class.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="value">The value.</param>
        public DateLinePoint(DateTime date, double value)
            : base()
        {
            this.date = date;
            base.YValue = value;
        }

        /// <summary>
        /// 获取或设置Date
        /// </summary>
        /// <value></value>
        public DateTime Date
        {
            get { return date; }
            set { date = value; }
        }
    }
}