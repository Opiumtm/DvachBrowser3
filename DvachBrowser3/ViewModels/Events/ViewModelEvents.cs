using System;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// События моделей представления.
    /// </summary>
    public static class ViewModelEvents
    {
        /// <summary>
        /// Обновление списка борд.
        /// </summary>
        public static readonly Guid BoardListRefreshId = new Guid("{FBFCE0B3-974B-41A8-ACC5-FF1415B0BFE1}");

        /// <summary>
        /// Попадение по ссылке.
        /// </summary>
        public static readonly Guid LinkClickId = new Guid("{9628E2F2-9117-4A04-8ACD-111159E686C3}");

        /// <summary>
        /// Список избранного изменился.
        /// </summary>
        public static readonly Guid FavoritesListRefreshedId = new Guid("{B43A3474-11B7-4B49-8FA5-273EB45688F8}");

        /// <summary>
        /// Список посещённых изменился.
        /// </summary>
        public static readonly Guid VisitedListRefreshedId = new Guid("{818785E5-775B-4150-BBF6-9888AE3E2AE5}");

        /// <summary>
        /// Обновление списка борд.
        /// </summary>
        public static readonly IWeakEventChannel BoardListRefresh = new WeakEventChannel(BoardListRefreshId);

        /// <summary>
        /// Попадение по ссылке.
        /// </summary>
        public static readonly IWeakEventChannel LinkClick = new WeakEventChannel(LinkClickId);

        /// <summary>
        /// Список избранного изменился.
        /// </summary>
        public static readonly IWeakEventChannel FavoritesListRefreshed = new WeakEventChannel(FavoritesListRefreshedId);

        /// <summary>
        /// Список посещённых изменился.
        /// </summary>
        public static readonly IWeakEventChannel VisitedListRefreshed = new WeakEventChannel(VisitedListRefreshedId);
    }
}