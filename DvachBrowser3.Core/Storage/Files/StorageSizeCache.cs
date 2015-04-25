using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
                var sizes = await file.PoliteRead(DoLoad, TimeSpan.FromSeconds(5));
                Sizes.Clear();
                foreach (var kv in sizes)
                {
                    Sizes[kv.Key] = kv.Value;
                }
            }
            catch
            {
                Sizes.Clear();
                if (Debugger.IsAttached)
                {
                    Debugger.Break();
                }
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
            catch
            {
                if (Debugger.IsAttached)
                {
                    Debugger.Break();
                }
            }
        }

        private async Task<List<KeyValuePair<string, StorageSizeCacheItem>>> DoLoad(IRandomAccessStream istr)
        {
            var result = new List<KeyValuePair<string, StorageSizeCacheItem>>();
            if (istr.Size > 0)
            {
                using (var rd = new DataReader(istr) { UnicodeEncoding = UnicodeEncoding.Utf8 })
                {
                    await rd.LoadAsync((uint)istr.Size);
                    var sz = rd.ReadUInt32();
                    for (UInt32 i = 0; i < sz; i++)
                    {
                        var strSz = rd.ReadUInt32();
                        var name = rd.ReadString(strSz);
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
            return result;
        }

        private async Task DoSave(IRandomAccessStream istr)
        {
            using (var wr = new DataWriter(istr) {UnicodeEncoding = UnicodeEncoding.Utf8})
            {
                var arr = Sizes.ToArray();
                wr.WriteUInt32((uint)arr.Length);
                foreach (var kv in arr)
                {
                    var strSz = wr.MeasureString(kv.Key);
                    wr.WriteUInt32(strSz);
                    wr.WriteString(kv.Key);
                    wr.WriteUInt64(kv.Value.Size);
                    wr.WriteInt64(kv.Value.Date.Ticks);
                    wr.WriteInt64(kv.Value.Date.Offset.Ticks);
                    await wr.StoreAsync();
                }
                await wr.FlushAsync();
            }
        }
    }
}