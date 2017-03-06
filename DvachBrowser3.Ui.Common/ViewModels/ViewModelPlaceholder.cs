using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Core;

namespace DvachBrowser3.Ui.ViewModels
{
    /// <summary>
    /// Инициализируемая по требованию модель представления.
    /// </summary>
    /// <typeparam name="TKey">Ключ.</typeparam>
    /// <typeparam name="T">Тип модели.</typeparam>
    public class ViewModelPlaceholder<TKey, T> : DispatchedObjectBase, IViewModelPlaceholder<T> where T : class
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="dispatcher">Диспетчер.</param>
        /// <param name="key">Ключ.</param>
        /// <param name="placeholderModelSource">Источник моделей.</param>
        /// <param name="comparer">Средство сравнения.</param>
        public ViewModelPlaceholder(CoreDispatcher dispatcher, TKey key, IViewModelPlaceholderModelSource<TKey, T> placeholderModelSource, IEqualityComparer<TKey> comparer = null) : base(dispatcher)
        {
            if (placeholderModelSource == null) throw new ArgumentNullException(nameof(placeholderModelSource));
            Key = key;
            Comparer = comparer;
            PlaceholderModelSource = placeholderModelSource;
            PlaceholderModelSource.ClearSuggested += PlaceholderModelSourceOnClearSuggested;
        }

        private async void PlaceholderModelSourceOnClearSuggested(object sender, TKey key)
        {
            try
            {
                var comparer = Comparer ?? EqualityComparer<TKey>.Default;
                if (comparer.Equals(key, Key))
                {
                    await Clear();
                }
            }
            catch (Exception ex)
            {
                DebugHelper.BreakOnError(ex);
            }
        }

        /// <summary>
        /// Источник моделей.
        /// </summary>
        protected IViewModelPlaceholderModelSource<TKey, T> PlaceholderModelSource { get; }

        /// <summary>
        /// Ключ.
        /// </summary>
        protected TKey Key { get; }

        /// <summary>
        /// Средство сравнения.
        /// </summary>
        protected IEqualityComparer<TKey> Comparer { get; }

        private T _model;

        /// <summary>
        /// Модель.
        /// </summary>
        public T Model
        {
            get
            {
                var model = Interlocked.CompareExchange(ref _model, null, null);
                if (model == null)
                {
                    TriggerCreateModel();
                }
                return model;
            }
        }

        /// <summary>
        /// Установить модель.
        /// </summary>
        /// <param name="model">Модель.</param>
        /// <returns>Таск.</returns>
        protected Task SetModel(T model)
        {
            Interlocked.Exchange(ref _model, model);
            return Dispatcher.DispatchAction(() =>
            {
                OnPropertyChanged(nameof(Model));
                OnPropertyChanged(nameof(IsAvailable));
            });
        }

        /// <summary>
        /// Модель действительна.
        /// </summary>
        public bool IsAvailable => Model != null;

        private int _isCreating = 0;

        /// <summary>
        /// Заставить модель обновиться.
        /// </summary>
        protected async void TriggerCreateModel()
        {
            try
            {
                await CreateModel();
            }
            catch (Exception ex)
            {
                DebugHelper.BreakOnError(ex);
            }
        }

        /// <summary>
        /// Создать модель.
        /// </summary>
        /// <returns>Таск.</returns>
        public async Task CreateModel()
        {
            if (Interlocked.Exchange(ref _isCreating, 1) == 0)
            {
                try
                {
                    await DoCreateModel();
                }
                finally
                {
                    Interlocked.Exchange(ref _isCreating, 0);
                }
            }
        }

        /// <summary>
        /// Создать модель.
        /// </summary>
        /// <returns>Таск.</returns>
        protected virtual async Task DoCreateModel()
        {
            var data = await PlaceholderModelSource.CreateModel(Dispatcher, Key);
            await SetModel(data);
        }

        /// <summary>
        /// Сбросить модель (если на модель назначен биндинг - она будет пересоздана).
        /// </summary>
        /// <returns>Таск.</returns>
        public async Task Clear()
        {
            if (Interlocked.CompareExchange(ref _clearCount, 0, 0) <= 0)
            {
                await DoClear();
            }
        }

        /// <summary>
        /// Сбросить модель (если на модель назначен биндинг - она будет пересоздана).
        /// </summary>
        /// <returns>Таск.</returns>
        protected virtual async Task DoClear()
        {
            await SetModel(null);
        }

        private int _clearCount;

        /// <summary>
        /// Заблокировать очистку.
        /// </summary>
        /// <param name="autoCreate">Автоматически создать.</param>
        public void LockClear(bool autoCreate)
        {
            var count = Interlocked.Increment(ref _clearCount);
            var model = Interlocked.CompareExchange(ref _model, null, null);
            if (count > 0 && model == null)
            {
                var unawaitedTask = Dispatcher.DispatchAction(async () =>
                {
                    try
                    {
                        await CreateModel();
                    }
                    catch (Exception ex)
                    {
                        DebugHelper.BreakOnError(ex);
                    }
                });
            }
        }

        /// <summary>
        /// Разблокировать очистку.
        /// </summary>
        /// <param name="autoClear">Автоматически очистить модель.</param>
        public void UnlockClear(bool autoClear)
        {
            var count = Interlocked.Decrement(ref _clearCount);
            var model = Interlocked.CompareExchange(ref _model, null, null);
            if (count <= 0 && model != null)
            {
                var unawaitedTask = Dispatcher.DispatchAction(async () =>
                {
                    try
                    {
                        await DoClear();
                    }
                    catch (Exception ex)
                    {
                        DebugHelper.BreakOnError(ex);
                    }
                });
            }
        }
    }
}