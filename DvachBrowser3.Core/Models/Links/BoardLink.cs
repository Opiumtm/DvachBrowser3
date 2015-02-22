using System;
using System.Runtime.Serialization;

namespace DvachBrowser3.Links
{
    /// <summary>
    /// Ссылка на борде.
    /// </summary>
    [DataContract(Namespace = CoreConstants.DvachBrowserNamespace)]
    public class BoardLink : BoardLinkBase, IEquatable<BoardLink>
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
        public int? PostNumber { get; set; }
        
        public bool Equals(BoardLink other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Engine, other.Engine, StringComparison.OrdinalIgnoreCase) && string.Equals(Board, other.Board, StringComparison.OrdinalIgnoreCase) && ThreadNumber == other.ThreadNumber && PostNumber == other.PostNumber;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((BoardLink) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = (Board != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(Board) : 0);
                hashCode = (hashCode*397) ^ ThreadNumber;
                hashCode = (hashCode*397) ^ PostNumber.GetHashCode();
                hashCode = (hashCode*397) ^ (Engine != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(Engine) : 0);
                return hashCode;
            }
        }

        public static bool operator ==(BoardLink left, BoardLink right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(BoardLink left, BoardLink right)
        {
            return !Equals(left, right);
        }
    }
}