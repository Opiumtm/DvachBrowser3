using System;

namespace DvachBrowser3
{
    /// <summary>
    /// Канал слабо связанных событий.
    /// </summary>
    public interface IWeakEventChannel
    {
        /// <summary>
        /// Зарегистрировать обратныйй вызов.
        /// </summary>
        /// <param name="callback">Обратный вызов.</param>
        /// <returns>Токен обратного вызова.</returns>
        Guid AddCallback(IWeakEventCallback callback);

        /// <summary>
        /// Удалить обратный вызов.
        /// </summary>
        /// <param name="token">Токен.</param>
        void RemoveCallback(Guid token);

        /// <summary>
        /// Вызвать событие.
        /// </summary>
        /// <param name="sender">Отправитель.</param>
        /// <param name="e">Параметр события.</param>
        void RaiseEvent(object sender, object e);
    }
}