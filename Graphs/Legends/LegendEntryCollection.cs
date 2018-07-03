using System;
using System.Collections;

namespace Zdd.Utility.Graphs
{
    /// <summary>
    /// LegendEntryCollection
    /// </summary>
    [Serializable()]
    public class LegendEntryCollection : CollectionBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LegendEntryCollection"/> class.
        /// </summary>
        public LegendEntryCollection()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LegendEntryCollection"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public LegendEntryCollection(LegendEntryCollection value)
        {
            AddRange(value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LegendEntryCollection"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public LegendEntryCollection(LegendEntry[] value)
        {
            AddRange(value);
        }

        /// <summary>
        /// Gets or sets the <see cref="Utility.Legends.LegendEntry"/> at the specified index.
        /// </summary>
        /// <value></value>
        public LegendEntry this[int index]
        {
            get { return ((LegendEntry) (List[index])); }
            set { List[index] = value; }
        }

        /// <summary>
        /// Adds the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public int Add(LegendEntry value)
        {
            return List.Add(value);
        }

        /// <summary>
        /// Adds the range.
        /// </summary>
        /// <param name="value">The value.</param>
        public void AddRange(LegendEntry[] value)
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
        public void AddRange(LegendEntryCollection value)
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
        public bool Contains(LegendEntry value)
        {
            return List.Contains(value);
        }

        /// <summary>
        /// Copies to.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="index">The index.</param>
        public void CopyTo(LegendEntry[] array, int index)
        {
            List.CopyTo(array, index);
        }

        /// <summary>
        /// Indexes the of.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public int IndexOf(LegendEntry value)
        {
            return List.IndexOf(value);
        }

        /// <summary>
        /// Inserts the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="value">The value.</param>
        public void Insert(int index, LegendEntry value)
        {
            List.Insert(index, value);
        }

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns></returns>
        public new LegendEntryEnumerator GetEnumerator()
        {
            return new LegendEntryEnumerator(this);
        }

        /// <summary>
        /// Removes the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        public void Remove(LegendEntry value)
        {
            List.Remove(value);
        }

        /// <summary>
        /// LegendEntryEnumerator
        /// </summary>
        public class LegendEntryEnumerator : object, IEnumerator
        {
            private IEnumerable temp;
            private IEnumerator baseEnumerator;

            /// <summary>
            /// Initializes a new instance of the <see cref="LegendEntryEnumerator"/> class.
            /// </summary>
            /// <param name="mappings">The mappings.</param>
            public LegendEntryEnumerator(LegendEntryCollection mappings)
            {
                temp = ((IEnumerable) (mappings));
                baseEnumerator = temp.GetEnumerator();
            }

            /// <summary>
            /// ��ȡ�����еĵ�ǰԪ�ء�
            /// </summary>
            /// <value></value>
            /// <returns>�����еĵ�ǰԪ�ء�</returns>
            public LegendEntry Current
            {
                get { return ((LegendEntry) (baseEnumerator.Current)); }
            }

            /// <summary>
            /// ��ȡ�����еĵ�ǰԪ�ء�
            /// </summary>
            /// <value></value>
            /// <returns>�����еĵ�ǰԪ�ء�</returns>
            object IEnumerator.Current
            {
                get { return baseEnumerator.Current; }
            }

            /// <summary>
            /// ��ö�����ƽ������ϵ���һ��Ԫ�ء�
            /// </summary>
            /// <returns>
            /// ���ö�����ɹ����ƽ�����һ��Ԫ�أ���Ϊ true�����ö����Խ�����ϵĽ�β����Ϊ false��
            /// </returns>
            public bool MoveNext()
            {
                return baseEnumerator.MoveNext();
            }

            /// <summary>
            /// ��ö�����ƽ������ϵ���һ��Ԫ�ء�
            /// </summary>
            /// <returns>
            /// ���ö�����ɹ����ƽ�����һ��Ԫ�أ���Ϊ true�����ö����Խ�����ϵĽ�β����Ϊ false��
            /// </returns>
            bool IEnumerator.MoveNext()
            {
                return baseEnumerator.MoveNext();
            }

            /// <summary>
            /// ��ö��������Ϊ���ʼλ�ã���λ��λ�ڼ����е�һ��Ԫ��֮ǰ��
            /// </summary>
            public void Reset()
            {
                baseEnumerator.Reset();
            }

            /// <summary>
            /// ��ö��������Ϊ���ʼλ�ã���λ��λ�ڼ����е�һ��Ԫ��֮ǰ��
            /// </summary>
            void IEnumerator.Reset()
            {
                baseEnumerator.Reset();
            }
        }
    }
}