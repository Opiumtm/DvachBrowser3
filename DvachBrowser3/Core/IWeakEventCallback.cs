namespace DvachBrowser3
{
    /// <summary>
    /// Обратный вызов слабо связанного события.
    /// </summary>
    public interface IWeakEventCallback
    {
        /// <summary>
        /// Получить событие.
        /// </summary>
        /// <param name="sender">Отправитель.</param>
        /// <param name="e">Параметр события.</param>
        void ReceiveWeakEvent(object sender, object e);
    }
}