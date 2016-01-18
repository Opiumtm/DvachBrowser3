using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using DvachBrowser3.Makaba;
using DvachBrowser3.Posting;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Иконка.
    /// </summary>
    public sealed class PostingIconViewModel : PostingFieldViewModelBase<Empty>, IPostingIconViewModel
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="parent">Родительская модель.</param>
        /// <param name="isSupported">Поддерживается.</param>
        /// <param name="role">Роль.</param>
        /// <param name="extension">Расширение данных борды.</param>
        /// <param name="engine">Движок.</param>
        public PostingIconViewModel(IPostingFieldsViewModel parent, bool isSupported, PostingFieldSemanticRole role, MakabaBoardReferenceExtension extension, string engine) : base(parent, isSupported && extension?.Icons != null, role)
        {
            Icons.Add(DefaultIcon);
            SelectedIcon = DefaultIcon;
            var iconsSrc = extension?.Icons;
            if (iconsSrc != null)
            {
                foreach (var i in iconsSrc.OrderBy(i => i.Number))
                {
                    Icons.Add(new PostingIconElement(i, engine));
                }
            }
        }

        /// <summary>
        /// Иконки.
        /// </summary>
        public IList<IPostingIconElement> Icons { get; } = new ObservableCollection<IPostingIconElement>();

        /// <summary>
        /// Иконка по умолчанию.
        /// </summary>
        public IPostingIconElement DefaultIcon { get; } = new PostingIconElement(null, "Без иконки", null);

        private IPostingIconElement selectedIcon;

        /// <summary>
        /// Выбранная иконка.
        /// </summary>
        public IPostingIconElement SelectedIcon
        {
            get { return selectedIcon; }
            set
            {
                selectedIcon = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Получить данные постинга.
        /// </summary>
        /// <returns>Данные постинга.</returns>
        public override KeyValuePair<PostingFieldSemanticRole, object>? GetValueData()
        {
            var idx = SelectedIcon?.Value;
            if (idx == null)
            {
                return null;
            }
            return new KeyValuePair<PostingFieldSemanticRole, object>(Role, idx.Value);
        }

        /// <summary>
        /// Заполнить значение.
        /// </summary>
        /// <param name="data">Значение.</param>
        public override void SetValueData(object data)
        {
            if (data == null)
            {
                SelectedIcon = DefaultIcon;
            }
            else
            {
                var v = (int)data;
                SelectedIcon = Icons.FirstOrDefault(i => i.Value == v) ?? DefaultIcon;
            }
        }
    }
}