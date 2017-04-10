using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml.Data;

namespace DvachBrowser3.Ui.ViewModels
{
    /// <summary>
    /// Источник моделей.
    /// </summary>
    /// <typeparam name="TKey">Ключ.</typeparam>
    /// <typeparam name="T">Тип модели.</typeparam>
    public class PlaceholderViewModelSource<TKey, T> : IViewModelPlaceholderModelSource<TKey, T> where T : class
    {
        private readonly IEqualityComparer<TKey> _comparer;

        private readonly Func<CoreDispatcher, TKey[], Task<KeyValuePair<TKey, T>[]>> _factory;

        private readonly int? _cacheSize;

        private readonly Dictionary<TKey, T> _cachedModels;

        private readonly List<TKey> _cache = new List<TKey>();

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="factory">Фабрика моделей.</param>
        /// <param name="cacheSize">Размер кэша.</param>
        /// <param name="comparer">Средство сравнения.</param>
        public PlaceholderViewModelSource(Func<CoreDispatcher, TKey[], Task<KeyValuePair<TKey, T>[]>> factory, int? cacheSize = null, IEqualityComparer<TKey> comparer = null)
        {
            if (factory == null) throw new ArgumentNullException(nameof(factory));
            _comparer = comparer;
            _factory = factory;
            _cacheSize = cacheSize;
            _cachedModels = new Dictionary<TKey, T>(Comparer);
        }

        /// <summary>
        /// Создать модель.
        /// </summary>
        /// <param name="dispatcher">Диспетчер.</param>
        /// <param name="key">Ключ.</param>
        /// <returns>Модель.</returns>
        public async Task<T> CreateModel(CoreDispatcher dispatcher, TKey key)
        {
            if (_cacheSize != null && _cacheSize > 1)
            {
                return await dispatcher.DispatchTask(() => DoCreateModel(dispatcher, key));
            }
            return await CreateOneElement(dispatcher, key);
        }

        /// <summary>
        /// Создать один элемент.
        /// </summary>
        /// <param name="dispatcher">Диспетчер.</param>
        /// <param name="key">Ключ.</param>
        /// <returns>Результат.</returns>
        protected async Task<T> CreateOneElement(CoreDispatcher dispatcher, TKey key)
        {
            return (await _factory(dispatcher, new[] {key})).Where(kv => Comparer.Equals(kv.Key, key)).Select(kv => kv.Value).FirstOrDefault();
        }

        private async Task<T> DoCreateModel(CoreDispatcher dispatcher, TKey key)
        {
            if (_cachedModels.ContainsKey(key))
            {
                var m = _cachedModels[key];
                _cache.RemoveAll(v => Comparer.Equals(v, key));
                _cache.Insert(0, key);
                return m;
            }
            else
            {
                var m = await CreateOneElement(dispatcher, key);
                _cachedModels[key] = m;
                _cache.RemoveAll(v => Comparer.Equals(v, key));
                _cache.Insert(0, key);
                AdjustCacheSize(dispatcher);
                return m;
            }
        }

        private void AdjustCacheSize(CoreDispatcher dispatcher)
        {
            while (_cache.Count > _cacheSize)
            {
                var k = _cache[_cache.Count - 1];
                _cachedModels.Remove(k);
                _cache.RemoveAt(_cache.Count - 1);
                var unawaitedTask = dispatcher.RunAsync(CoreDispatcherPriority.Low, () =>
                {
                    try
                    {
                        OnClearSuggested(k);
                    }
                    catch (Exception ex)
                    {
                        DebugHelper.BreakOnError(ex);
                    }
                });
            }
        }

        /// <summary>
        /// Предлагается очистка модели.
        /// </summary>
        public event EventHandler<TKey> ClearSuggested;

        /// <summary>
        /// Прогрузить элементы в кэш.
        /// </summary>
        /// <param name="dispatcher">Диспетчер.</param>
        /// <param name="keys">Ключи.</param>
        /// <returns>Таск.</returns>
        public async Task PreloadItems(CoreDispatcher dispatcher, IEnumerable<TKey> keys)
        {
            if (_cacheSize != null && _cacheSize > 1)
            {
                await DoPreloadItems(dispatcher, keys, _cacheSize.Value);
            }
        }

        /// <summary>
        /// Прогрузить элементы в кэш.
        /// </summary>
        /// <param name="dispatcher">Диспетчер.</param>
        /// <param name="keys">Ключи.</param>
        /// <param name="maxSize">Максимальный размер.</param>
        /// <returns>Таск.</returns>
        protected virtual async Task DoPreloadItems(CoreDispatcher dispatcher, IEnumerable<TKey> keys, int maxSize)
        {
            await dispatcher.DispatchAction(async () =>
            {
                var toCreate = new HashSet<TKey>(Comparer);
                foreach (var key in keys)
                {
                    if (toCreate.Count >= maxSize)
                    {
                        break;
                    }

                    if (!_cachedModels.ContainsKey(key))
                    {
                        toCreate.Add(key);
                    }
                }
                var elements = (await _factory(dispatcher, toCreate.ToArray())).Deduplicate(kv => kv.Key, Comparer).ToArray();
                var toUpdate = new HashSet<TKey>(elements.Select(kv => kv.Key));
                _cache.RemoveAll(v => toUpdate.Contains(v));
                foreach (var kv in elements)
                {
                    _cachedModels[kv.Key] = kv.Value;
                    _cache.Insert(0, kv.Key);
                }

                AdjustCacheSize(dispatcher);
            });
        }

        private IEqualityComparer<TKey> Comparer => _comparer ?? EqualityComparer<TKey>.Default;

        private void OnClearSuggested(TKey e)
        {
            ClearSuggested?.Invoke(this, e);
        }
    }
}