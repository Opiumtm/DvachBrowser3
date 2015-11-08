using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Media;
using DvachBrowser3.Board;
using DvachBrowser3.Engines;
using DvachBrowser3.Makaba;
using Template10.Mvvm;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Сводная информация о борде.
    /// </summary>
    public sealed class SummaryBoardInfo : ViewModelBase, ISummaryBoardInfo
    {
        private readonly BoardReference boardReference;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="boardReference">Ссылка на борду.</param>
        public SummaryBoardInfo(BoardReference boardReference)
        {
            if (boardReference == null) throw new ArgumentNullException(nameof(boardReference));
            this.boardReference = boardReference;
            if (boardReference.Extensions != null)
            {
                var makaba = boardReference.Extensions.OfType<MakabaBoardReferenceExtension>().FirstOrDefault();
                if (makaba != null)
                {
                    MakabaInfo = new MakabaBoardInfo(makaba, boardReference.Link?.Engine);
                }
                var posting = boardReference.Extensions.OfType<BoardReferencePostingExtension>().FirstOrDefault();
                if (posting != null)
                {
                    Posting = new PostingBoardInfo(posting);
                }
            }
            var engines = ServiceLocator.Current.GetServiceOrThrow<INetworkEngines>();
            if (boardReference.Link?.Engine != null)
            {
                if (engines.ListEngines().Any(e => StringComparer.OrdinalIgnoreCase.Equals(e, boardReference.Link.Engine)))
                {
                    var engine = engines.GetEngineById(boardReference.Link.Engine);
                    if ((engine.Capability & EngineCapability.PartialThreadRequest) != 0)
                    {
                        EngineCapabilities.Add(new BoardInfoString("Загрузка только новых сообщений в треде при обновлении"));
                    }
                    if ((engine.Capability & EngineCapability.ThreadStatusRequest) != 0)
                    {
                        EngineCapabilities.Add(new BoardInfoString("Получение количества постов и даты обновления без загрузки треда"));
                    }
                    if ((engine.Capability & EngineCapability.BoardsListRequest) != 0)
                    {
                        EngineCapabilities.Add(new BoardInfoString("Получение списка досок"));
                    }
                    if ((engine.Capability & EngineCapability.SearchRequest) != 0)
                    {
                        EngineCapabilities.Add(new BoardInfoString("Поиск по доске"));
                    }
                    if ((engine.Capability & EngineCapability.TopPostsRequest) != 0)
                    {
                        EngineCapabilities.Add(new BoardInfoString("Топ тредов на доске"));
                    }
                    if ((engine.Capability & EngineCapability.LastModifiedRequest) != 0)
                    {
                        EngineCapabilities.Add(new BoardInfoString("Быстрая проверка изменений в треде"));
                    }
                    if ((engine.Capability & EngineCapability.NoCaptcha) != 0)
                    {
                        EngineCapabilities.Add(new BoardInfoString("Отправка сообщений в тред без ввода капчи"));
                    }
                }
            }
        }

        /// <summary>
        /// Короткое имя.
        /// </summary>
        public string ShortName => boardReference.ShortName ?? "";

        /// <summary>
        /// Отображаемое имя.
        /// </summary>
        public string DisplayName => boardReference.DisplayName ?? "";

        /// <summary>
        /// Категория.
        /// </summary>
        public string Category => boardReference.Category ?? "";

        /// <summary>
        /// Ресурс.
        /// </summary>
        public string Resource => BoardListBoardViewModelsHelper.GetResourceName(boardReference.Link?.Engine);

        /// <summary>
        /// Движок.
        /// </summary>
        public string Engine => BoardListBoardViewModelsHelper.GetEngineName(boardReference.Link?.Engine);

        /// <summary>
        /// Логотип.
        /// </summary>
        public ImageSource EngineLogo => BoardListBoardViewModelsHelper.GetLogo(boardReference.Link?.Engine);

        /// <summary>
        /// Не для работы.
        /// </summary>
        public bool NotSafeForWork => boardReference.IsAdult;

        /// <summary>
        /// Информация по макабе.
        /// </summary>
        public IMakabaBoardInfo MakabaInfo { get; }

        /// <summary>
        /// Постинг.
        /// </summary>
        public IPostingBoardInfo Posting { get; }

        /// <summary>
        /// Возможности движка.
        /// </summary>
        public IList<IBoadInfoString> EngineCapabilities { get; } = new List<IBoadInfoString>();
    }
}