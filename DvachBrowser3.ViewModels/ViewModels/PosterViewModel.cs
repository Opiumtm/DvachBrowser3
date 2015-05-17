using System;
using Windows.UI;
using DvachBrowser3.Posts;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Модель представления постера.
    /// </summary>
    public sealed class PosterViewModel : ViewModelBase, IPosterViewModel
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="data">Данные.</param>
        /// <param name="parent">Родительская модель.</param>
        public PosterViewModel(PostTreePosterExtension data, IPostViewModel parent)
        {
            if (data == null) throw new ArgumentNullException("data");
            if (parent == null) throw new ArgumentNullException("parent");
            this.data = data;
            Parent = parent;
        }

        private readonly PostTreePosterExtension data;

        /// <summary>
        /// Родительская модель.
        /// </summary>
        public IPostViewModel Parent { get; private set; }

        /// <summary>
        /// Имя.
        /// </summary>
        public string Name
        {
            get { return data.Name; }
        }

        /// <summary>
        /// Трипкод.
        /// </summary>
        public string Tripcode
        {
            get { return data.Tripcode; }
        }

        /// <summary>
        /// Цвет.
        /// </summary>
        public Color Color
        {
            get { return data.NameColor != null ? data.NameColor.Value : Colors.Transparent; }
        }

        /// <summary>
        /// Есть цвет.
        /// </summary>
        public bool HasColor
        {
            get { return data.NameColor != null; }
        }
    }
}