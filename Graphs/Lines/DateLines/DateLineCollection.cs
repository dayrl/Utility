using System;
using System.Collections;

namespace Zdd.Utility.Graphs
{
    /// <summary>
    /// DateLineCollection
    /// </summary>
    [Serializable()]
    public class DateLineCollection : CollectionBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DateLineCollection"/> class.
        /// </summary>
        public DateLineCollection()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DateLineCollection"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public DateLineCollection(DateLineCollection value)
        {
            AddRange(value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DateLineCollection"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public DateLineCollection(DateLine[] value)
        {
            AddRange(value);
        }

        /// <summary>
        /// Gets or sets the <see cref="Utility.Lines.DateLines.DateLine"/> at the specified index.
        /// </summary>
        /// <value></value>
        public DateLine this[int index]
        {
            get { return ((DateLine) (List[index])); }
            set { List[index] = value; }
        }

        /// <summary>
        /// Adds the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public int Add(DateLine value)
        {
            return List.Add(value);
        }

        /// <summary>
        /// Adds the range.
        /// </summary>
        /// <param name="value">The value.</param>
        public void AddRange(DateLine[] value)
        {
            for (int i = 0; (i < value.Length); i = (i + 1))
            {
                Add(value[i]);
            }
        }

        /// <summary>
        /// Adds the range.
        /// </summary>
        /// <param name="value">The value.</param>
        public void AddRange(DateLineCollection value)
        {
            for (int i = 0; (i < value.Count); i = (i + 1))
            {
                Add(value[i]);
            }
        }

        /// <summary>
        /// Determines whether [contains] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// 	<c>true</c> if [contains] [the specified value]; otherwise, <c>false</c>.
        /// </returns>
        public bool Contains(DateLine value)
        {
            return List.Contains(value);
        }

        /// <summary>
        /// Copies to.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="index">The index.</param>
        public void CopyTo(DateLine[] array, int index)
        {
            List.CopyTo(array, index);
        }

        /// <summary>
        /// Indexes the of.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public int IndexOf(DateLine value)
        {
            return List.IndexOf(value);
        }

        /// <summary>
        /// Inserts the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="value">The value.</param>
        public void Insert(int index, DateLine value)
        {
            List.Insert(index, value);
        }

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns></returns>
        public new DateLineEnumerator GetEnumerator()
        {
            return new DateLineEnumerator(this);
        }

        /// <summary>
        /// Removes the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        public void Remove(DateLine value)
        {
            List.Remove(value);
        }

        /// <summary>
        /// DateLineEnumerator
        /// </summary>
        public class DateLineEnumerator : object, IEnumerator
        {
            private IEnumerable temp;
            private IEnumerator baseEnumerator;

            /// <summary>
            /// Initializes a new instance of the <see cref="DateLineEnumerator"/> class.
            /// </summary>
            /// <param name="mappings">The mappings.</param>
            public DateLineEnumerator(DateLineCollection mappings)
            {
                temp = ((IEnumerable) (mappings));
                baseEnumerator = temp.GetEnumerator();
            }

            /// <summary>
            /// 获取集合中的当前元素。
            /// </summary>
            /// <value></value>
            /// <returns>集合中的当前元素。</returns>
            public DateLine Current
            {
                get { return ((DateLine) (baseEnumerator.Current)); }
            }

            /// <summary>
            /// 获取集合中的当前元素。
            /// </summary>
            /// <value></value>
            /// <returns>集合中的当前元素。</returns>
            object IEnumerator.Current
            {
                get { return baseEnumerator.Current; }
            }

            /// <summary>
            /// 将枚举数推进到集合的下一个元素。
            /// </summary>
            /// <returns>
            /// 如果枚举数成功地推进到下一个元素，则为 true；如果枚举数越过集合的结尾，则为 false。
            /// </returns>
            public bool MoveNext()
            {
                return baseEnumerator.MoveNext();
            }

            /// <summary>
            /// 将枚举数推进到集合的下一个元素。
            /// </summary>
            /// <returns>
            /// 如果枚举数成功地推进到下一个元素，则为 true；如果枚举数越过集合的结尾，则为 false。
            /// </returns>
            bool IEnumerator.MoveNext()
            {
                return baseEnumerator.MoveNext();
            }

            /// <summary>
            /// 将枚举数设置为其初始位置，该位置位于集合中第一个元素之前。
            /// </summary>
            public void Reset()
            {
                baseEnumerator.Reset();
            }

            /// <summary>
            /// 将枚举数设置为其初始位置，该位置位于集合中第一个元素之前。
            /// </summary>
            void IEnumerator.Reset()
            {
                baseEnumerator.Reset();
            }
        }
    }
}