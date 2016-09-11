using System;
using System.ComponentModel;
using DvachBrowser3.ViewModels;

namespace DvachBrowser3
{
    /// <summary>
    /// Класс-помощник для слабо связанных событий без подписки.
    /// </summary>
    public static class LiteWeakEventHelper
    {
        /// <summary>
        /// Создать слабо связанный обработчик.
        /// </summary>
        /// <typeparam name="T">Тип аргумента события.</typeparam>
        /// <typeparam name="TSub">Тип подписчика.</typeparam>
        /// <param name="handle">Слабая ссылка на подписчика.</param>
        /// <param name="action">Действие.</param>
        /// <returns>Обработчик события.</returns>
        public static EventHandler<T> CreateHandler<T, TSub>(WeakReference<TSub> handle, Action<TSub, object, T> action) where TSub : class
        {
            if (handle == null) throw new ArgumentNullException(nameof(handle));
            return (sender, e) =>
            {
                TSub obj;
                if (handle.TryGetTarget(out obj))
                {
                    action?.Invoke(obj, sender, e);
                }
            };
        }

        /// <summary>
        /// Создать слабо связанный обработчик.
        /// </summary>
        /// <typeparam name="TSub">Тип подписчика.</typeparam>
        /// <param name="handle">Слабая ссылка на подписчика.</param>
        /// <param name="action">Действие.</param>
        /// <returns>Обработчик события.</returns>
        public static EventHandler CreateHandler<TSub>(WeakReference<TSub> handle, Action<TSub, object, EventArgs> action) where TSub : class
        {
            if (handle == null) throw new ArgumentNullException(nameof(handle));
            return (sender, e) =>
            {
                TSub obj;
                if (handle.TryGetTarget(out obj))
                {
                    action?.Invoke(obj, sender, e);
                }
            };
        }

        /// <summary>
        /// Создать слабо связанный обработчик.
        /// </summary>
        /// <typeparam name="TSub">Тип подписчика.</typeparam>
        /// <param name="handle">Слабая ссылка на подписчика.</param>
        /// <param name="action">Действие.</param>
        /// <returns>Обработчик события.</returns>
        public static PropertyChangedEventHandler CreatePropertyHandler<TSub>(WeakReference<TSub> handle, Action<TSub, object, PropertyChangedEventArgs> action) where TSub : class
        {
            if (handle == null) throw new ArgumentNullException(nameof(handle));
            return (sender, e) =>
            {
                TSub obj;
                if (handle.TryGetTarget(out obj))
                {
                    action?.Invoke(obj, sender, e);
                }
            };
        }

        /// <summary>
        /// Создать слабо связанный обработчик.
        /// </summary>
        /// <typeparam name="TSub">Тип подписчика.</typeparam>
        /// <param name="handle">Слабая ссылка на подписчика.</param>
        /// <param name="action">Действие.</param>
        /// <returns>Обработчик события.</returns>
        public static OperationProgressFinishedEventHandler CreateProgressFinishedHandler<TSub>(WeakReference<TSub> handle, Action<TSub, object, OperationProgressFinishedEventArgs> action) where TSub : class
        {
            if (handle == null) throw new ArgumentNullException(nameof(handle));
            return (sender, e) =>
            {
                TSub obj;
                if (handle.TryGetTarget(out obj))
                {
                    action?.Invoke(obj, sender, e);
                }
            };
        }

        /// <summary>
        /// Создать слабо связанный обработчик.
        /// </summary>
        /// <typeparam name="TSub">Тип подписчика.</typeparam>
        /// <param name="handle">Слабая ссылка на подписчика.</param>
        /// <param name="action">Действие.</param>
        /// <returns>Обработчик события.</returns>
        public static ImageSourceGotEventHandler CreateImageGotHandler<TSub>(WeakReference<TSub> handle, Action<TSub, object, ImageSourceGotEventArgs> action) where TSub : class
        {
            if (handle == null) throw new ArgumentNullException(nameof(handle));
            return (sender, e) =>
            {
                TSub obj;
                if (handle.TryGetTarget(out obj))
                {
                    action?.Invoke(obj, sender, e);
                }
            };
        }

        /// <summary>
        /// Создать слабо связанный обработчик.
        /// </summary>
        /// <typeparam name="TSub">Тип подписчика.</typeparam>
        /// <param name="handle">Слабая ссылка на подписчика.</param>
        /// <param name="action">Действие.</param>
        /// <returns>Обработчик события.</returns>
        public static PostingSuccessEventHandler CreatePostingSuccessHandler<TSub>(WeakReference<TSub> handle, Action<TSub, object, PostingSuccessEventArgs> action) where TSub : class
        {
            if (handle == null) throw new ArgumentNullException(nameof(handle));
            return (sender, e) =>
            {
                TSub obj;
                if (handle.TryGetTarget(out obj))
                {
                    action?.Invoke(obj, sender, e);
                }
            };
        }

        /// <summary>
        /// Создать слабо связанный обработчик.
        /// </summary>
        /// <typeparam name="TSub">Тип подписчика.</typeparam>
        /// <param name="handle">Слабая ссылка на подписчика.</param>
        /// <param name="action">Действие.</param>
        /// <returns>Обработчик события.</returns>
        public static NeedSetCaptchaEventHandler CreateNeedSetCaptchaHandler<TSub>(WeakReference<TSub> handle, Action<TSub, object, NeedSetCaptchaEventArgs> action) where TSub : class
        {
            if (handle == null) throw new ArgumentNullException(nameof(handle));
            return (sender, e) =>
            {
                TSub obj;
                if (handle.TryGetTarget(out obj))
                {
                    action?.Invoke(obj, sender, e);
                }
            };
        }
    }
}