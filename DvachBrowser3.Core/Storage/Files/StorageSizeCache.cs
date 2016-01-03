using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using UnicodeEncoding = Windows.Storage.Streams.UnicodeEncoding;

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
                var sizes = await file.PoliteRead(DoLoad, TimeSpan.FromSeconds(1));
                Sizes.Clear();
                foreach (var kv in sizes)
                {
                    Sizes[kv.Key] = kv.Value;
                }
            }
            catch (Exception ex)
            {
                Sizes.Clear();
                DebugHelper.BreakOnError(ex);
            }
        }

        /// <summary>
        /// Сохранить.
        /// </summary>
        /// <param name="file">Файл.</param>
        /// <param name="parentFolder">Родительская директория.</param>
        /// <returns>Таск.</returns>
        public async Task Save(StorageFile file, StorageFolder parentFolder)
        {
            try
            {
                await file.ReplaceContent(parentFolder, DoSave);
            }
            catch (Exception ex)
            {
                DebugHelper.BreakOnError(ex);
            }
        }

        private async Task<List<KeyValuePair<string, StorageSizeCacheItem>>> DoLoad(IRandomAccessStream istr)
        {
            var result = new List<KeyValuePair<string, StorageSizeCacheItem>>();
            if (istr.Size > 0)
            {
                using (var mem = new MemoryStream())
                {
                    using (var istr2 = istr.AsStreamForRead())
                    {
                        await istr2.CopyToAsync(mem);
                        mem.Position = 0;
                        using (var rd = new BinaryReader(mem, Encoding.UTF8))
                        {
                            var sz = rd.ReadUInt32();
                            for (UInt32 i = 0; i < sz; i++)
                            {
                                var strSz = rd.ReadUInt32();
                                var name = strSz > 0 ? new string(rd.ReadChars((int)strSz)) : "";
                                var size = rd.ReadUInt64();
                                var dticks = rd.ReadInt64();
                                var oticks = rd.ReadInt64();
                                result.Add(new KeyValuePair<string, StorageSizeCacheItem>(name, new StorageSizeCacheItem
                                {
                                    Size = size,
                                    Date = new DateTimeOffset(dticks, new TimeSpan(oticks))
                                }));
                            }
                        }
                    }
                }
            }
            return result;
        }

        private async Task DoSave(IRandomAccessStream istr)
        {
            using (var mem = new MemoryStream())
            {
                using (var wr = new BinaryWriter(mem, Encoding.UTF8))
                {
                    var arr = Sizes.ToArray();
                    wr.Write((uint)arr.Length);
                    foreach (var kv in arr)
                    {
                        var strSz = (uint)kv.Key.Length;
                        wr.Write(strSz);
                        if (strSz > 0)
                        {
                            wr.Write(kv.Key.ToCharArray());
                        }
                        wr.Write(kv.Value.Size);
                        wr.Write(kv.Value.Date.Ticks);
                        wr.Write(kv.Value.Date.Offset.Ticks);
                    }
                    wr.Flush();
                    mem.Position = 0;
                    using (var istr2 = istr.AsStreamForWrite())
                    {
                        await mem.CopyToAsync(istr2);
                    }
                }
            }
        }

        /// <summary>
        /// Клонировать объект.
        /// </summary>
        /// <returns>Клон объекта.</returns>
        public StorageSizeCache Clone()
        {
            var result = new StorageSizeCache();
            foreach (var kv in Sizes)
            {
                result.Sizes[kv.Key] = kv.Value;
            }
            return result;
        }
    }
}