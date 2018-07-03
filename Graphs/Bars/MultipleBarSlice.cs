using System;
using System.Drawing;

namespace Zdd.Utility.Graphs
{
    /// <summary>
    /// Summary description for MultipleBarSlice.
    /// </summary>
    public class MultipleBarSlice : BarSlice
    {
        private double[] partialValues = null;
        private Color[] partialColors = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="MultipleBarSlice"/> class.
        /// </summary>
        public MultipleBarSlice()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultipleBarSlice"/> class.
        /// </summary>
        /// <param name="partialValues">The partial values.</param>
        /// <param name="partialColors">The partial colors.</param>
        public MultipleBarSlice(double[] partialValues, Color[] partialColors)
        {
            this.partialColors = partialColors;
            this.partialValues = partialValues;
            if (partialValues != null)
            {
                double totalValue = 0;

                for (int i = 0; i < partialValues.Length; i++)
                {
                    totalValue += partialValues[i];
                }

                value = totalValue;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultipleBarSlice"/> class.
        /// </summary>
        /// <param name="partialValues">The partial values.</param>
        /// <param name="partialColors">The partial colors.</param>
        /// <param name="text">The text.</param>
        public MultipleBarSlice(double[] partialValues, Color[] partialColors, string text)
        {
            this.partialColors = partialColors;
            this.partialValues = partialValues;
            if (partialValues != null)
            {
                double totalValue = 0;

                for (int i = 0; i < partialValues.Length; i++)
                {
                    totalValue += partialValues[i];
                }

                value = totalValue;
            }

            this.text = text;
        }

        /// <summary>
        /// 获取或设置PartialValues
        /// </summary>
        /// <value></value>
        public double[] PartialValues
        {
            get { return partialValues; }
            set
            {
                partialValues = value;
                if (partialValues != null)
                {
                    double totalValue = 0;

                    for (int i = 0; i < partialValues.Length; i++)
                    {
                        totalValue += partialValues[i];
                    }

                    Value = totalValue;
                }
            }
        }

        /// <summary>
        /// 获取或设置PartialColors
        /// </summary>
        /// <value></value>
        public Color[] PartialColors
        {
            get { return partialColors; }
            set { partialColors = value; }
        }

        /// <summary>
        /// 获取或设置MaxValue
        /// </summary>
        /// <value></value>
        public double MaxValue
        {
            get
            {
                if (PartialValues == null)
                    return 0;

                if (PartialValues.Length == 0)
                    return 0;

                double retVal = Double.MinValue;

                for (int i = 0; i < PartialValues.Length; i++)
                {
                    if (PartialValues[i] > retVal)
                        retVal = PartialValues[i];
                }

                return retVal;
            }
        }

        /// <summary>
        /// 获取或设置MinValue
        /// </summary>
        /// <value></value>
        public double MinValue
        {
            get
            {
                if (PartialValues == null)
                    return 0;

                if (PartialValues.Length == 0)
                    return 0;

                double retVal = Double.MaxValue;

                for (int i = 0; i < PartialValues.Length; i++)
                {
                    if (PartialValues[i] < retVal)
                        retVal = PartialValues[i];
                }

                return retVal;
            }
        }
    }
}