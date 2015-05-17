using Windows.UI;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Модель представления постера.
    /// </summary>
    public interface IPosterViewModel
    {
        /// <summary>
        /// Родительская модель.
        /// </summary>
        IPostViewModel Parent { get; }

        /// <summary>
        /// Имя.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Трипкод.
        /// </summary>
        string Tripcode { get; }

        /// <summary>
        /// Цвет.
        /// </summary>
        Color Color { get; }

        /// <summary>
        /// Есть цвет.
        /// </summary>
        bool HasColor { get; }
    }
}