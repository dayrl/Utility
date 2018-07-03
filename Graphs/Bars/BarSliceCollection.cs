using System;
using System.Collections;

namespace Zdd.Utility.Graphs
{
    /// <summary>
    /// CollectionBase
    /// </summary>
    [Serializable()]
    public class BarSliceCollection : CollectionBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BarSliceCollection"/> class.
        /// </summary>
        public BarSliceCollection()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BarSliceCollection"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public BarSliceCollection(BarSliceCollection value)
        {
            AddRange(value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BarSliceCollection"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public BarSliceCollection(BarSlice[] value)
        {
            AddRange(value);
        }

        /// <summary>
        /// Gets or sets the <see cref="Utility.Bars.BarSlice"/> at the specified index.
        /// </summary>
        /// <value></value>
        public BarSlice this[int index]
        {
            get { return ((BarSlice) (List[index])); }
            set { List[index] = value; }
        }

        /// <summary>
        /// Adds the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public int Add(BarSlice value)
        {
            return List.Add(value);
        }

        /// <summary>
        /// Adds the range.
        /// </summary>
        /// <param name="value">The value.</param>
        public void AddRange(BarSlice[] value)
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
        public void AddRange(BarSliceCollection value)
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
        public bool Contains(BarSlice value)
        {
            return List.Contains(value);
        }

        /// <summary>
        /// Copies to.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="index">The index.</param>
        public void CopyTo(BarSlice[] array, int index)
        {
            List.CopyTo(array, index);
        }

        /// <summary>
        /// Indexes the of.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public int IndexOf(BarSlice value)
        {
            return List.IndexOf(value);
        }

        /// <summary>
        /// Inserts the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="value">The value.</param>
        public void Insert(int index, BarSlice value)
        {
            List.Insert(index, value);
        }

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns></returns>
        public new BarSliceEnumerator GetEnumerator()
        {
            return new BarSliceEnumerator(this);
        }

        /// <summary>
        /// Removes the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        public void Remove(BarSlice value)
        {
            List.Remove(value);
        }

        /// <summary>
        /// BarSliceEnumerator class
        /// </summary>
        public class BarSliceEnumerator : object, IEnumerator
        {
            private IEnumerator baseEnumerator;

            private IEnumerable temp;

            /// <summary>
            /// Initializes a new instance of the <see cref="BarSliceEnumerator"/> class.
            /// </summary>
            /// <param name="mappings">The mappings.</param>
            public BarSliceEnumerator(BarSliceCollection mappings)
            {
                temp = ((IEnumerable) (mappings));
                baseEnumerator = temp.GetEnumerator();
            }

            /// <summary>
            /// 获取集合中的当前元素。
            /// </summary>
            /// <value></value>
            /// <returns>集合中的当前元素。</returns>
            public BarSlice Current
            {
                get { return ((BarSlice) (baseEnumerator.Current)); }
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