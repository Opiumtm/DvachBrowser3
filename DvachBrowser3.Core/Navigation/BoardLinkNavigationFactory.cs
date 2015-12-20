using System;
using System.Collections.Generic;

namespace DvachBrowser3.Navigation
{
    /// <summary>
    /// Фабрика ключей навигации.
    /// </summary>
    public sealed class BoardLinkNavigationFactory : INavigationKeyFactory
    {
        private static readonly Dictionary<string, Func<string, INavigationKey>> LinkTypes = new Dictionary<string, Func<string, INavigationKey>>(StringComparer.OrdinalIgnoreCase)
        {
            { BoardNavigationKey.TypeNameConst, BoardNavigationKey.Deserialize },
            { BoardPageNavigationKey.TypeNameConst, BoardPageNavigationKey.Deserialize },
            { ThreadNavigationKey.TypeNameConst, ThreadNavigationKey.Deserialize },
            { PostNavigationKey.TypeNameConst, PostNavigationKey.Deserialize },
            { BoardMediaNavigationKey.TypeNameConst, BoardMediaNavigationKey.Deserialize },
            { MediaNavigationKey.TypeNameConst, MediaNavigationKey.Deserialize },
            { YoutubeNavigationKey.TypeNameConst, YoutubeNavigationKey.Deserialize },
            { RootNavigationKey.TypeNameConst, RootNavigationKey.Deserialize },
            { ThreadTagNavigationKey.TypeNameConst, ThreadTagNavigationKey.Deserialize },
            { BoardCatalogNavigationKey.TypeNameConst, BoardCatalogNavigationKey.Deserialize },
        };

        /// <summary>
        /// Имена типов ключей.
        /// </summary>
        public IEnumerable<string> TypeNames
        {
            get { return LinkTypes.Keys; }
        }

        /// <summary>
        /// Десериализовать ключ.
        /// </summary>
        /// <param name="typeName">Тип ключа.</param>
        /// <param name="data">Строка.</param>
        /// <returns>Ключ.</returns>
        public INavigationKey Deserialize(string typeName, string data)
        {
            if (LinkTypes.ContainsKey(typeName))
            {
                return LinkTypes[typeName](data);
            }
            throw new ArgumentException(string.Format("Тип ключа навигации не поддерживается BoardLinkNavigationFactory:{0}", typeName));
        }
    }
}