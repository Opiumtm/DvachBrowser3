using System;
using System.Collections.Generic;

namespace DvachBrowser3
{
    /// <summary>
    /// Основные константы.
    /// </summary>
    public static class CoreConstants
    {
        /// <summary>
        /// Пространство имён для XML.
        /// </summary>
        public const string DvachBrowserNamespace = "urn:boethiah:dvachbrowser3";

        /// <summary>
        /// Максимальное количество постов в контейнере.
        /// </summary>
        public const int PostContainerMaxCount = 50;

        /// <summary>
        /// Максимальное количество посещённых тредов.
        /// </summary>
        public const int MaxVisitedThreads = 30;

        /// <summary>
        /// Максимальное количество параллельных загрузок маленьких изображений.
        /// </summary>
        public const int MaxParallelSmallImageDownloads = 4;

        /// <summary>
        /// Движок.
        /// </summary>
        // ReSharper disable MemberHidesStaticFromOuterClass
        public static class Engine
        {
            /// <summary>
            /// Движок Makaba.
            /// </summary>
            public const string Makaba = "makaba";            
        }
        // ReSharper restore MemberHidesStaticFromOuterClass

        /// <summary>
        /// Константы макабы.
        /// </summary>
        public static class Makaba
        {
            /// <summary>
            /// Взрослые доски.
            /// </summary>
            public static readonly HashSet<string> AdultBoards = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "b", "fag", "fg", "fur", "g", "ga", "h", "ho", "sex", "fet", "e", "hc", "gb", "ya", "r34", "hm", "guro",
                "vn", "hg", "es"
            };            
        }
    }
}