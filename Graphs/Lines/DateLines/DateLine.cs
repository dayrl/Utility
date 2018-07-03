using System;
using System.Drawing;

namespace Zdd.Utility.Graphs
{
    /// <summary>
    /// Summary description for DateLine.
    /// </summary>
    public class DateLine : Line
    {
        private DateLinePointCollection dateLinePointCollection = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="DateLine"/> class.
        /// </summary>
        public DateLine()
            : base()
        {
            dateLinePointCollection = new DateLinePointCollection();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DateLine"/> class.
        /// </summary>
        /// <param name="color">The color.</param>
        public DateLine(Color color)
            : base(color)
        {
            dateLinePointCollection = new DateLinePointCollection();
        }

        /// <summary>
        /// 获取或设置DatePoints
        /// </summary>
        /// <value></value>
        public DateLinePointCollection DatePoints
        {
            get { return dateLinePointCollection; }
            set { dateLinePointCollection = value; }
        }

        /// <summary>
        /// Adds the date line point.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="value">The value.</param>
        public void AddDateLinePoint(DateTime date, double value)
        {
            dateLinePointCollection.Add(new DateLinePoint(date, value));
        }
    }
}