using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using DvachBrowser3.Markup;
using DvachBrowser3.Posting;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Поля постинга.
    /// </summary>
    public interface IPostingFieldsViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Родительская модель.
        /// </summary>
        IPostingViewModel Parent { get; }

        /// <summary>
        /// Заголовок.
        /// </summary>
        IPostingFieldViewModel<string> Title { get; }

        /// <summary>
        /// Комментарий.
        /// </summary>
        IPostingFieldViewModel<string> Comment { get; }

        /// <summary>
        /// Адрес почты.
        /// </summary>
        IPostingFieldViewModel<string> Email { get; }

        /// <summary>
        /// Имя постера.
        /// </summary>
        IPostingFieldViewModel<string> PosterName { get; }


        /// <summary>
        /// Трипкод (первая часть).
        /// </summary>
        IPostingFieldViewModel<string> PosterTrip1 { get; }

        /// <summary>
        /// Трипкод (вторая часть).
        /// </summary>
        IPostingFieldViewModel<string> PosterTrip2 { get; }

        /// <summary>
        /// Иконка.
        /// </summary>
        IPostingIconViewModel Icon { get; }

        /// <summary>
        /// Имя постера.
        /// </summary>
        IPostingFieldViewModel<bool> SageFlag { get; }

        /// <summary>
        /// Вотермарка.
        /// </summary>
        IPostingFieldViewModel<bool> WatermarkFlag { get; }

        /// <summary>
        /// Вотермарка.
        /// </summary>
        IPostingFieldViewModel<bool> OpFlag { get; }

        /// <summary>
        /// Медиа файлы.
        /// </summary>
        IPostingMediaCollectionViewModel Media { get; }

        /// <summary>
        /// Тэг треда.
        /// </summary>
        IPostingFieldViewModel<string> ThreadTag { get; }

        /// <summary>
        /// Сохранить хранилище полей.
        /// </summary>
        /// <param name="data">Данные.</param>
        void Save(IDictionary<PostingFieldSemanticRole, object> data);

        /// <summary>
        /// Загрузить хранилище полей.
        /// </summary>
        /// <param name="data">Данные.</param>
        void Load(IDictionary<PostingFieldSemanticRole, object> data);

        /// <summary>
        /// Сохранить данные.
        /// </summary>
        /// <param name="immediate">Сохранить немедленно.</param>
        Task Flush(bool immediate = false);

        /// <summary>
        /// Пометить как сохранённый.
        /// </summary>
        void MarkAsSent();

        /// <summary>
        /// Очистить данные.
        /// </summary>
        /// <returns></returns>
        Task Clear();

        /// <summary>
        /// Установить значения по умолчанию.
        /// </summary>
        void SetDefault();

        /// <summary>
        /// Вызвать событие по изменению.
        /// </summary>
        void RaiseChanged();

        /// <summary>
        /// Сохранено.
        /// </summary>
        event EventHandler Flushed;

        /// <summary>
        /// Тип разметки.
        /// </summary>
        PostingMarkupType? MarkupType { get; }

        /// <summary>
        /// Провайдер 
        /// </summary>
        IMarkupProvider MarkupProvider { get; }

        /// <summary>
        /// Инициализировано.
        /// </summary>
        event EventHandler Initialized;

        /// <summary>
        /// Инициализировать.
        /// </summary>
        void Initialize();
    }
}