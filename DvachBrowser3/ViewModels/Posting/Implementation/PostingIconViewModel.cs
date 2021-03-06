﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using DvachBrowser3.Makaba;
using DvachBrowser3.Posting;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Иконка.
    /// </summary>
    public sealed class PostingIconViewModel : PostingFieldViewModelBase<IPostingIconElement>, IPostingIconViewModel
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
            Value = DefaultIcon;
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

        /// <summary>
        /// Получить данные постинга.
        /// </summary>
        /// <returns>Данные постинга.</returns>
        public override KeyValuePair<PostingFieldSemanticRole, object>? GetValueData()
        {
            var idx = Value?.Value;
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
        /// <param name="flush">Вызвать сохранение данных.</param>
        public override void SetValueData(object data, bool flush = true)
        {
            if (data == null)
            {
                objValue = DefaultIcon;
            }
            else
            {
                var v = (int)data;
                objValue = Icons.FirstOrDefault(i => i.Value == v) ?? DefaultIcon;
            }
            // ReSharper disable once ExplicitCallerInfoArgument
            RaisePropertyChanged(nameof(Value));
            if (flush)
            {
                OnValueChange();
            }
        }

        /// <summary>
        /// Заполнить значение по умолчанию.
        /// </summary>
        /// <param name="flush">Вызвать сохранение данных.</param>
        public override void SetDefaultValueData(bool flush = true)
        {
            SetValueData(null, flush);
        }

        /// <summary>
        /// Очистить данные.
        /// </summary>
        /// <param name="flush">Вызвать сохранение данных.</param>
        public override Task Clear(bool flush = true)
        {
            SetValueData(null, flush);
            return Task.CompletedTask;
        }
    }
}