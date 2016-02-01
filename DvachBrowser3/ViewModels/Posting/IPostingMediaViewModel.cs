using System.ComponentModel;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using DvachBrowser3.Posting;
using DvachBrowser3.Styles;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Изображение постинга.
    /// </summary>
    public interface IPostingMediaViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Родительская модель.
        /// </summary>
        IPostingMediaCollectionViewModel Parent { get; }

        /// <summary>
        /// Идентификатор.
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Изображение.
        /// </summary>
        ImageSource Image { get; }

        /// <summary>
        /// Есть изображение.
        /// </summary>
        bool HasImage { get; }

        /// <summary>
        /// Строка с размером.
        /// </summary>
        string SizeStr { get; }

        /// <summary>
        /// Размер файла.
        /// </summary>
        ulong FileSize { get; }

        /// <summary>
        /// Оригинальное имя.
        /// </summary>
        string OriginalName { get; }

        /// <summary>
        /// Добавлять уникальный ID.
        /// </summary>
        bool AddUniqueId { get; set; }

        /// <summary>
        /// Изменять размер.
        /// </summary>
        bool Resize { get; set; }

        /// <summary>
        /// Можно изменять размер.
        /// </summary>
        bool CanResize { get; }

        /// <summary>
        /// Можно показывать предпросмотр.
        /// </summary>
        bool CanPreview { get; }

        /// <summary>
        /// Создать описание файла.
        /// </summary>
        /// <returns>Описание файла.</returns>
        PostingMediaFile CreateData();

        /// <summary>
        /// Удалить файл.
        /// </summary>
        Task Delete();

        /// <summary>
        /// Менеджер стилей.
        /// </summary>
        IStyleManager StyleManager { get; }
    }
}