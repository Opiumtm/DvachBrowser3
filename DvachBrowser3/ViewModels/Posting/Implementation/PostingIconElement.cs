using System;
using DvachBrowser3.Links;
using DvachBrowser3.Makaba;
using Template10.Mvvm;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Иконка.
    /// </summary>
    public sealed class PostingIconElement : ViewModelBase, IPostingIconElement
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="icon">Иконка.</param>
        /// <param name="engine">Движок.</param>
        public PostingIconElement(MakabaIconReference icon, string engine)
        {
            if (icon == null) throw new ArgumentNullException(nameof(icon));
            if (engine == null) throw new ArgumentNullException(nameof(engine));
            Value = icon.Number;
            Name = icon.Name;
            Image = new ImageSourceViewModel(new MediaLink() { Engine = engine, RelativeUri = icon.Url, IsAbsolute = false });
            Image.Load.Start();
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="value">Значение.</param>
        /// <param name="name">Имя.</param>
        /// <param name="iconLink">Ссылка.</param>
        public PostingIconElement(int? value, string name, BoardLinkBase iconLink)
        {
            Value = value;
            Name = name;
            if (iconLink != null)
            {
                Image = new ImageSourceViewModel(iconLink);
                Image.Load.Start();
            }
        }

        /// <summary>
        /// Значение.
        /// </summary>
        public int? Value { get; }

        /// <summary>
        /// Имя.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Изображение.
        /// </summary>
        public IImageSourceViewModel Image { get; }
    }
}