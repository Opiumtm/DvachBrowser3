using System;
using System.Runtime.Serialization;

namespace DvachBrowser3.Posts
{
    /// <summary>
    /// Ссылка на пост на борде.
    /// </summary>
    [DataContract(Namespace = CoreConstants.DvachBrowserNamespace)]
    public class PostNodeBoardLink : PostNodeBase, IEquatable<PostNodeBoardLink>
    {
        /// <summary>
        /// Борда.
        /// </summary>
        [DataMember]
        public string Board { get; set; }

        /// <summary>
        /// Номер треда.
        /// </summary>
        [DataMember]
        public int ThreadNumber { get; set; }

        /// <summary>
        /// Номер поста.
        /// </summary>
        [DataMember]
        public int PostNumber { get; set; }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(PostNodeBoardLink other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Board, other.Board) && ThreadNumber == other.ThreadNumber && PostNumber == other.PostNumber;
        }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>; otherwise, false.
        /// </returns>
        /// <param name="obj">The <see cref="T:System.Object"/> to compare with the current <see cref="T:System.Object"/>. </param>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((PostNodeBoardLink)obj);
        }

        /// <summary>
        /// Serves as a hash function for a particular type. 
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"/>.
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = (Board != null ? Board.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ ThreadNumber;
                hashCode = (hashCode * 397) ^ PostNumber;
                return hashCode;
            }
        }         
    }
}