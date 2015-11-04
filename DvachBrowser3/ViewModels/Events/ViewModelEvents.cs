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
        public static IWeakEventChannel BoardListRefresh = new WeakEventChannel();
    }
}