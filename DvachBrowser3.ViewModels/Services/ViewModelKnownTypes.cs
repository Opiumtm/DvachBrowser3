using System;
using System.Collections.Generic;
using DvachBrowser3.Links;

namespace DvachBrowser3
{
    /// <summary>
    /// Известные типы.
    /// </summary>
    public static class ViewModelKnownTypes
    {
        /// <summary>
        /// Известные типы.
        /// </summary>
        public static IEnumerable<Type> KnownTypes
        {
            get { yield return typeof (BoardLinkBase); }
        }
    }
}