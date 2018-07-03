using System;
using System.Collections;

namespace Zdd.Utility.Graphs
{
    /// <summary>
    /// LinePointCollection
    /// </summary>
    [Serializable()]
    public class LinePointCollection : CollectionBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LinePointCollection"/> class.
        /// </summary>
        public LinePointCollection()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LinePointCollection"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public LinePointCollection(LinePointCollection value)
        {
            AddRange(value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LinePointCollection"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public LinePointCollection(LinePoint[] value)
        {
            AddRange(value);
        }

        /// <summary>
        /// Gets or sets the <see cref="Utility.Lines.LinePoint"/> at the specified index.
        /// </summary>
        /// <value></value>
        public LinePoint this[int index]
        {
            get { return ((LinePoint) (List[index])); }
            set { List[index] = value; }
        }

        /// <summary>
        /// Adds the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public int Add(LinePoint value)
        {
            return List.Add(value);
        }

        /// <summary>
        /// Adds the range.
        /// </summary>
        /// <param name="value">The value.</param>
        public void AddRange(LinePoint[] value)
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
        public void AddRange(LinePointCollection value)
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
        public bool Contains(LinePoint value)
        {
            return List.Contains(value);
        }

        /// <summary>
        /// Copies to.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="index">The index.</param>
        public void CopyTo(LinePoint[] array, int index)
        {
            List.CopyTo(array, index);
        }

        /// <summary>
        /// Indexes the of.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public int IndexOf(LinePoint value)
        {
            return List.IndexOf(value);
        }

        /// <summary>
        /// Inserts the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="value">The value.</param>
        public void Insert(int index, LinePoint value)
        {
            List.Insert(index, value);
        }

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns></returns>
        public new LinePointEnumerator GetEnumerator()
        {
            return new LinePointEnumerator(this);
        }

        /// <summary>
        /// Removes the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        public void Remove(LinePoint value)
        {
            List.Remove(value);
        }

        /// <summary>
        /// LinePointEnumerator
        /// </summary>
        public class LinePointEnumerator : object, IEnumerator
        {
            private IEnumerable temp;
            private IEnumerator baseEnumerator;

            /// <summary>
            /// Initializes a new instance of the <see cref="LinePointEnumerator"/> class.
            /// </summary>
            /// <param name="mappings">The mappings.</param>
            public LinePointEnumerator(LinePointCollection mappings)
            {
                temp = ((IEnumerable) (mappings));
                baseEnumerator = temp.GetEnumerator();
            }

            /// <summary>
            /// 获取集合中的当前元素。
            /// </summary>
            /// <value></value>
            /// <returns>集合中的当前元素。</returns>
            public LinePoint Current
            {
                get { return ((LinePoint) (baseEnumerator.Current)); }
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