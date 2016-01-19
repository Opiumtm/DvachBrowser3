using System;

namespace DvachBrowser3
{
    /// <summary>
    /// Пустое значение.
    /// </summary>
    public struct Empty : IEquatable<Empty>
    {
        /// <summary>
        /// Пустое значение.
        /// </summary>
        public static readonly Empty Value = new Empty();

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(Empty other)
        {
            return true;
        }

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <returns>
        /// true if <paramref name="obj"/> and this instance are the same type and represent the same value; otherwise, false. 
        /// </returns>
        /// <param name="obj">The object to compare with the current instance. </param>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Empty && Equals((Empty) obj);
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer that is the hash code for this instance.
        /// </returns>
        public override int GetHashCode()
        {
            return -1;
        }

        public static bool operator ==(Empty left, Empty right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Empty left, Empty right)
        {
            return !left.Equals(right);
        }
    }
}