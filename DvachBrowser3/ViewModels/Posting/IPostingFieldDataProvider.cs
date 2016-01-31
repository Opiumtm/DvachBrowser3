using System.Collections.Generic;
using System.Threading.Tasks;
using DvachBrowser3.Posting;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Провайдер данных постинга.
    /// </summary>
    public interface IPostingFieldDataProvider
    {
        /// <summary>
        /// Роль.
        /// </summary>
        PostingFieldSemanticRole Role { get; }

        /// <summary>
        /// Получить данные постинга.
        /// </summary>
        /// <returns>Данные постинга.</returns>
        KeyValuePair<PostingFieldSemanticRole, object>? GetValueData();

        /// <summary>
        /// Заполнить значение.
        /// </summary>
        /// <param name="data">Значение.</param>
        /// <param name="flush">Вызвать сохранение данных.</param>
        void SetValueData(object data, bool flush = true);

        /// <summary>
        /// Заполнить значение по умолчанию.
        /// </summary>
        /// <param name="flush">Вызвать сохранение данных.</param>
        void SetDefaultValueData(bool flush = true);

        /// <summary>
        /// Очистить данные.
        /// </summary>
        /// <param name="flush">Вызвать сохранение данных.</param>
        Task Clear(bool flush = true);

        /// <summary>
        /// Вызвать событие по изменению.
        /// </summary>
        void RaiseChanged();
    }
}