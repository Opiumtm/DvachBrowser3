using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace DvachBrowser3.Storage.Files
{
    /// <summary>
    /// Кэш размеров хранилища.
    /// </summary>
    public sealed class StorageSizeCache
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public StorageSizeCache()
        {
            Sizes = new Dictionary<string, StorageSizeCacheItem>(StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Размеры.
        /// </summary>
        public Dictionary<string, StorageSizeCacheItem> Sizes { get; private set; }

        /// <summary>
        /// Загрузить.
        /// </summary>
        /// <param name="file">Файл.</param>
        /// <returns>Таск.</returns>
        public async Task Load(StorageFile file)
        {
            try
            {
                using (var str = await file.OpenStreamForReadAsync())
                {
                    using (var rd = new BinaryReader(str, Encoding.Unicode))
                    {
                        Sizes.Clear();
                        var output = new List<KeyValuePair<string, StorageSizeCacheItem>>();
                        await Task.Factory.StartNew(() =>
                        {
                            // ReSharper disable AccessToDisposedClosure
                            while (str.Position < str.Length)
                            {
                                var sz = rd.ReadInt32();
                                var chars = sz > 0 ? rd.ReadChars(sz) : new char[0];
                                var name = new string(chars);
                                var size = rd.ReadUInt64();
                                var dateTicks = rd.ReadInt64();
                                var offsetTicks = rd.ReadInt64();
                                var date = new DateTimeOffset(dateTicks, TimeSpan.FromTicks(offsetTicks));
                                output.Add(new KeyValuePair<string, StorageSizeCacheItem>(name, new StorageSizeCacheItem() { Size = size, Date = date}));
                            }
                            // ReSharper restore AccessToDisposedClosure
                        });
                        foreach (var r in output)
                        {
                            Sizes[r.Key] = r.Value;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Sizes.Clear();
            }
        }

        /// <summary>
        /// Сохранить.
        /// </summary>
        /// <param name="file">Файл.</param>
        /// <returns>Таск.</returns>
        public async Task Save(StorageFile file)
        {
            try
            {
                using (var tr = await file.OpenTransactedWriteAsync())
                {
                    using (var str = tr.Stream.AsStreamForWrite())
                    {
                        using (var wr = new BinaryWriter(str, Encoding.Unicode))
                        {
                            await Task.Factory.StartNew(() =>
                            {
                                // ReSharper disable AccessToDisposedClosure
                                foreach (var kv in Sizes)
                                {
                                    var chars = kv.Key.ToCharArray();
                                    wr.Write(chars.Length);
                                    if (chars.Length > 0) wr.Write(chars);
                                    wr.Write(kv.Value.Size);
                                    wr.Write(kv.Value.Date.Ticks);
                                    wr.Write(kv.Value.Date.Offset.Ticks);
                                }
                                // ReSharper restore AccessToDisposedClosure
                            });
                        }
                    }
                    await tr.CommitAsync();
                }
            }
            catch
            {
                if (Debugger.IsAttached)
                {
                    Debugger.Break();
                }
            }
        }
    }
}