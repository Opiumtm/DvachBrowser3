using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Media.Imaging;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Статический кэш изображений.
    /// </summary>
    public sealed class StaticImageCache
    {
        /// <summary>
        /// Кэши.
        /// </summary>
        private static readonly Dictionary<string, StaticImageCache> Caches = new Dictionary<string, StaticImageCache>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Кэш иконок и флагов.
        /// </summary>
        public static readonly StaticImageCacheDesc IconsAndFlags = new StaticImageCacheDesc()
        {
            Name = "IconsAndFlags",
            MaxSize = 50
        };

        /// <summary>
        /// Маленькие картинки для предварительного просмотра.
        /// </summary>
        public static readonly StaticImageCacheDesc Thumbnails = new StaticImageCacheDesc()
        {
            Name = "Thumbnails",
            MaxSize = 20
        };

        /// <summary>
        /// Получить кэш.
        /// </summary>
        /// <param name="cache">Описание кэша.</param>
        /// <returns>Кэш.</returns>
        public static StaticImageCache GetCache(StaticImageCacheDesc cache)
        {
            lock (Caches)
            {
                if (!Caches.ContainsKey(cache.Name))
                {
                    Caches[cache.Name] = new StaticImageCache(cache.MaxSize);
                }
                return Caches[cache.Name];
            }
        }

        private readonly MaxSizeDictionary<string, BitmapImage> cache;

        /// <summary>
        /// Кэш статических изображений.
        /// </summary>
        /// <param name="maxSize">Максимальный размер.</param>
        public StaticImageCache(int maxSize = 15)
        {
            cache = new MaxSizeDictionary<string, BitmapImage>(maxSize);
        }

        /// <summary>
        /// Попробовать получить.
        /// </summary>
        /// <param name="key">Ключ.</param>
        /// <returns>Изображение.</returns>
        public BitmapImage TryGet(string key)
        {
            lock (cache)
            {
                if (key == null)
                {
                    return null;
                }
                if (cache.ContainsKey(key))
                {
                    return cache[key];
                }
                return null;
            }
        }

        /// <summary>
        /// Установить.
        /// </summary>
        /// <param name="key">Ключ.</param>
        /// <param name="image">Изображение.</param>
        public void Set(string key, BitmapImage image)
        {
            lock (cache)
            {
                if (key != null && image != null)
                {
                    cache[key] = image;
                }
            }
        }
    }

    /// <summary>
    /// Описание статического кэша.
    /// </summary>
    public struct StaticImageCacheDesc
    {
        /// <summary>
        /// Имя.
        /// </summary>
        public string Name;

        /// <summary>
        /// Максимальный размер.
        /// </summary>
        public int MaxSize;
    }
}