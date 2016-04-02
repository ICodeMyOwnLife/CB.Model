using System;
using System.Collections.Generic;


namespace CB.Model.Common
{
    public struct Key<T1, T2>: IEquatable<Key<T1, T2>>, IComparable<Key<T1, T2>>
    {
        #region Fields & Properties
        public T1 Key1 { get; }

        public T2 Key2 { get; }
        #endregion


        #region Constructors
        public Key(T1 key1, T2 key2)
        {
            Key1 = key1;
            Key2 = key2;
        }
        #endregion


        #region Methods
        public bool Equals(Key<T1, T2> other)
        {
            return EqualityComparer<T1>.Default.Equals(Key1, other.Key1) &&
                   EqualityComparer<T2>.Default.Equals(Key2, other.Key2);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Key<T1, T2> && Equals((Key<T1, T2>)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (EqualityComparer<T1>.Default.GetHashCode(Key1) * 397) ^
                       EqualityComparer<T2>.Default.GetHashCode(Key2);
            }
        }

        public int CompareTo(Key<T1, T2> other)
        {
            var compareT1 = Comparer<T1>.Default.Compare(Key1, other.Key1);
            return compareT1 != 0 ? compareT1 : Comparer<T2>.Default.Compare(Key2, other.Key2);
        }

        public override string ToString()
        {
            return $"{{Key1={Key1},Key2={Key2}}}";
        }
        #endregion
    }
}