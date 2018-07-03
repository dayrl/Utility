using System;

namespace Zdd.Utility.Graphs
{
    /// <summary>
    /// Summary description for DateXAxisText.
    /// </summary>
    public class DateXAxisText : XAxisText
    {
        private DateTime startDate;
        private DateTime endDate;

        /// <summary>
        /// Initializes a new instance of the <see cref="DateXAxisText"/> class.
        /// </summary>
        public DateXAxisText()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DateXAxisText"/> class.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="text">The text.</param>
        public DateXAxisText(DateTime date, string text)
            : base()
        {
            startDate = date;
            endDate = date;
            base.Text = text;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DateXAxisText"/> class.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="text">The text.</param>
        public DateXAxisText(DateTime startDate, DateTime endDate, string text)
            : base()
        {
            this.startDate = startDate;
            this.endDate = endDate;
            base.Text = text;
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
    }
}