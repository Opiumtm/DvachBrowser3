using System;
using System.Collections.Generic;
using DvachBrowser3.Links;

namespace DvachBrowser3.Logic
{
    /// <summary>
    /// Сервис хэша ссылок.
    /// </summary>
    public class LinkHashService : ServiceBase, ILinkHashService
    {
        private Dictionary<Type, Func<BoardLinkBase, string>> typeFuncs;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="services">Сервисы.</param>
        public LinkHashService(IServiceProvider services) : base(services)
        {
        }

        public string GetLinkHash(BoardLinkBase link)
        {
            var t = link.GetType();
            if (typeFuncs.ContainsKey(t))
            {
                return typeFuncs[t](link);
            }
            throw new ArgumentException(string.Format("Неизвестный тип ссылки {0}", t.FullName));
        }
    }
}