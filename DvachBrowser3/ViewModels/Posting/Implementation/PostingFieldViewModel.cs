using DvachBrowser3.Posting;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Обычное поле постинга.
    /// </summary>
    /// <typeparam name="T">Тип поля.</typeparam>
    public sealed class PostingFieldViewModel<T> : PostingFieldViewModelBase<T>
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="parent">Родительская модель.</param>
        /// <param name="isSupported">Поддерживается.</param>
        /// <param name="role">Роль.</param>
        public PostingFieldViewModel(IPostingFieldsViewModel parent, bool isSupported, PostingFieldSemanticRole role) : base(parent, isSupported, role)
        {
        }
    }
}