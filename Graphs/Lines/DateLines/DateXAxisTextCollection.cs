using System;
using System.Collections;

namespace Zdd.Utility.Graphs
{
    /// <summary>
    /// DateXAxisTextCollection
    /// </summary>
    [Serializable()]
    public class DateXAxisTextCollection : CollectionBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DateXAxisTextCollection"/> class.
        /// </summary>
        public DateXAxisTextCollection()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DateXAxisTextCollection"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public DateXAxisTextCollection(DateXAxisTextCollection value)
        {
            AddRange(value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DateXAxisTextCollection"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public DateXAxisTextCollection(DateXAxisText[] value)
        {
            AddRange(value);
        }

        /// <summary>
        /// Gets or sets the <see cref="Utility.Lines.DateLines.DateXAxisText"/> at the specified index.
        /// </summary>
        /// <value></value>
        public DateXAxisText this[int index]
        {
            get { return ((DateXAxisText) (List[index])); }
            set { List[index] = value; }
        }

        /// <summary>
        /// Adds the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public int Add(DateXAxisText value)
        {
            return List.Add(value);
        }

        /// <summary>
        /// Adds the range.
        /// </summary>
        /// <param name="value">The value.</param>
        public void AddRange(DateXAxisText[] value)
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
        public void AddRange(DateXAxisTextCollection value)
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
        public bool Contains(DateXAxisText value)
        {
            return List.Contains(value);
        }

        /// <summary>
        /// Copies to.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="index">The index.</param>
        public void CopyTo(DateXAxisText[] array, int index)
        {
            List.CopyTo(array, index);
        }

        /// <summary>
        /// Indexes the of.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public int IndexOf(DateXAxisText value)
        {
            return List.IndexOf(value);
        }

        /// <summary>
        /// Inserts the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="value">The value.</param>
        public void Insert(int index, DateXAxisText value)
        {
            List.Insert(index, value);
        }

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns></returns>
        public new DateXAxisTextEnumerator GetEnumerator()
        {
            return new DateXAxisTextEnumerator(this);
        }

        /// <summary>
        /// Removes the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        public void Remove(DateXAxisText value)
        {
            List.Remove(value);
        }

        /// <summary>
        /// DateXAxisTextEnumerator
        /// </summary>
        public class DateXAxisTextEnumerator : object, IEnumerator
        {
            private IEnumerable temp;
            private IEnumerator baseEnumerator;

            /// <summary>
            /// Initializes a new instance of the <see cref="DateXAxisTextEnumerator"/> class.
            /// </summary>
            /// <param name="mappings">The mappings.</param>
            public DateXAxisTextEnumerator(DateXAxisTextCollection mappings)
            {
                temp = ((IEnumerable) (mappings));
                baseEnumerator = temp.GetEnumerator();
            }

            /// <summary>
            /// ��ȡ�����еĵ�ǰԪ�ء�
            /// </summary>
            /// <value></value>
            /// <returns>�����еĵ�ǰԪ�ء�</returns>
            public DateXAxisText Current
            {
                get { return ((DateXAxisText) (baseEnumerator.Current)); }
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