using System;
using DvachBrowser3.Links;
using DvachBrowser3.Makaba;
using Template10.Mvvm;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Информация об иконке.
    /// </summary>
    public sealed class MakabaBoardInfoIcon : ViewModelBase, IBoardInfoIcon
    {
        private readonly MakabaIconReference icon;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="icon">Иконка.</param>
        /// <param name="engine">Движок.</param>
        public MakabaBoardInfoIcon(MakabaIconReference icon, string engine)
        {
            if (icon == null) throw new ArgumentNullException(nameof(icon));
            if (engine == null) throw new ArgumentNullException(nameof(engine));
            this.icon = icon;
            Icon = new ImageSourceViewModel(new MediaLink() { Engine = engine, IsAbsolute = false, RelativeUri = icon.Url});
            Icon.Load.Start();
        }

        /// <summary>
        /// Имя.
        /// </summary>
        public string Name => icon.Name ?? "";

        public IImageSourceViewModel Icon { get; }
    }
}