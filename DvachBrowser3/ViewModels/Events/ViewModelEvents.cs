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
        /// Обновление списка борд.
        /// </summary>
        public static readonly IWeakEventChannel BoardListRefresh = new WeakEventChannel(BoardListRefreshId);

        /// <summary>
        /// Попадение по ссылке.
        /// </summary>
        public static readonly IWeakEventChannel LinkClick = new WeakEventChannel(LinkClickId);
    }
}