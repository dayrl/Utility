using System;
using System.Collections;

namespace Zdd.Utility.Graphs
{
    /// <summary>
    /// XAxisTextCollection
    /// </summary>
    [Serializable()]
    public class XAxisTextCollection : CollectionBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="XAxisTextCollection"/> class.
        /// </summary>
        public XAxisTextCollection()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XAxisTextCollection"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public XAxisTextCollection(XAxisTextCollection value)
        {
            AddRange(value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XAxisTextCollection"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public XAxisTextCollection(XAxisText[] value)
        {
            AddRange(value);
        }

        /// <summary>
        /// Gets or sets the <see cref="Utility.Lines.XAxisText"/> at the specified index.
        /// </summary>
        /// <value></value>
        public XAxisText this[int index]
        {
            get { return ((XAxisText) (List[index])); }
            set { List[index] = value; }
        }

        /// <summary>
        /// Adds the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public int Add(XAxisText value)
        {
            return List.Add(value);
        }

        /// <summary>
        /// Adds the range.
        /// </summary>
        /// <param name="value">The value.</param>
        public void AddRange(XAxisText[] value)
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
        public void AddRange(XAxisTextCollection value)
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
        public bool Contains(XAxisText value)
        {
            return List.Contains(value);
        }

        /// <summary>
        /// Copies to.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="index">The index.</param>
        public void CopyTo(XAxisText[] array, int index)
        {
            List.CopyTo(array, index);
        }

        /// <summary>
        /// Indexes the of.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public int IndexOf(XAxisText value)
        {
            return List.IndexOf(value);
        }

        /// <summary>
        /// Inserts the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="value">The value.</param>
        public void Insert(int index, XAxisText value)
        {
            List.Insert(index, value);
        }

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns></returns>
        public new XAxisTextEnumerator GetEnumerator()
        {
            return new XAxisTextEnumerator(this);
        }

        /// <summary>
        /// Removes the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        public void Remove(XAxisText value)
        {
            List.Remove(value);
        }

        /// <summary>
        /// 
        /// </summary>
        public class XAxisTextEnumerator : object, IEnumerator
        {
            private IEnumerable temp;
            private IEnumerator baseEnumerator;

            /// <summary>
            /// Initializes a new instance of the <see cref="XAxisTextEnumerator"/> class.
            /// </summary>
            /// <param name="mappings">The mappings.</param>
            public XAxisTextEnumerator(XAxisTextCollection mappings)
            {
                temp = ((IEnumerable) (mappings));
                baseEnumerator = temp.GetEnumerator();
            }

            /// <summary>
            /// ��ȡ�����еĵ�ǰԪ�ء�
            /// </summary>
            /// <value></value>
            /// <returns>�����еĵ�ǰԪ�ء�</returns>
            public XAxisText Current
            {
                get { return ((XAxisText) (baseEnumerator.Current)); }
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