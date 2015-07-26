using System;
using System.Threading.Tasks;
using Windows.UI.Core;

namespace DvachBrowser3
{
    /// <summary>
    /// Асинхронная операция получения и буферизации значения.
    /// </summary>
    /// <typeparam name="T">Тип значения.</typeparam>
    public sealed class ViewModelLazy<T>
    {
        /// <summary>
        /// Загружено.
        /// </summary>
        public event EventHandler Loaded;

        private readonly T defaultValue;

        private readonly CoreDispatcher dispatcher;

        private readonly bool defaultOnError;

        private readonly Func<Task<T>> updateFunc;

        private void OnLoaded()
        {
            EventHandler handler = Loaded;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        private bool isGot = false;

        private bool isGetting = false;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="dispatcher">Диспетчер.</param>
        /// <param name="updateFunc">Функция обновления.</param>
        /// <param name="defaultValue">Значение по умолчанию.</param>
        /// <param name="defaultOnError">Значение по умолчанию по ошибке.</param>
        public ViewModelLazy(CoreDispatcher dispatcher, Func<Task<T>> updateFunc, T defaultValue = default(T), bool defaultOnError = true)
        {
            if (dispatcher == null) throw new ArgumentNullException("dispatcher");
            if (updateFunc == null) throw new ArgumentNullException("updateFunc");
            this.dispatcher = dispatcher;
            this.defaultOnError = defaultOnError;
            this.updateFunc = updateFunc;
            this.defaultValue = defaultValue;
            this.value = defaultValue;
        }

        private T value;

        public T Value
        {
            get
            {
                if (!isGot && !isGetting)
                {
                    dispatcher.RunAsync(CoreDispatcherPriority.Normal, GetValue);
                }
                return value;
            }
        }

        private async void GetValue()
        {
            if (isGetting)
            {
                return;
            }
            isGetting = true;
            try
            {
                value = await updateFunc();
            }
            catch (Exception ex)
            {
                if (!defaultOnError)
                {
                    throw;
                }
                DebugHelper.BreakOnError(ex);
                value = defaultValue;
            }
            finally
            {
                isGetting = false;
            }
            isGot = true;
            dispatcher.RunAsync(CoreDispatcherPriority.Normal, OnLoaded);
        }

        /// <summary>
        /// Стереть значение.
        /// </summary>
        public void ClearValue()
        {
            isGot = false;
        }
    }
}