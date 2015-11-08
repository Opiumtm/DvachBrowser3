using System;
using System.Collections.Generic;
using System.Linq;
using DvachBrowser3.Makaba;
using Template10.Mvvm;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Информация о макабе.
    /// </summary>
    public class MakabaBoardInfo : ViewModelBase, IMakabaBoardInfo
    {
        private readonly MakabaBoardReferenceExtension extension;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="extension">Расширение.</param>
        /// <param name="engine">Движок.</param>
        public MakabaBoardInfo(MakabaBoardReferenceExtension extension, string engine)
        {
            if (extension == null) throw new ArgumentNullException(nameof(extension));
            this.extension = extension;
            if (engine != null && extension.Icons != null)
            {
                foreach (var icon in extension.Icons.Where(i => i != null).OrderBy(i => i.Number))
                {
                    Icons.Add(new MakabaBoardInfoIcon(icon, engine));
                }
            }
        }

        /// <summary>
        /// Иконки.
        /// </summary>
        public IList<IBoardInfoIcon> Icons { get; } = new List<IBoardInfoIcon>();

        /// <summary>
        /// Есть иконки.
        /// </summary>
        public bool HasIcons => Icons.Count > 0;

        /// <summary>
        /// Бамп лимит.
        /// </summary>
        public int? BumpLimit => extension.Bumplimit;

        /// <summary>
        /// Имя по умолчанию.
        /// </summary>
        public string DefaultName => extension.DefaultName ?? "";

        /// <summary>
        /// Количество страниц.
        /// </summary>
        public int Pages => extension.Pages;

        /// <summary>
        /// Поддержка сажи.
        /// </summary>
        public bool Sage => extension.Sage;

        /// <summary>
        /// Поддержка трипкодов.
        /// </summary>
        public bool TripCodes => extension.Tripcodes;

        /// <summary>
        /// Максимальный размер поста.
        /// </summary>
        public int? MaxCommentSize => extension.MaxComment;
    }
}