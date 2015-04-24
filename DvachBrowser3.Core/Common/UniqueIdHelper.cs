using System;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;

namespace DvachBrowser3
{
    /// <summary>
    /// Помощник получения уникальных ID.
    /// </summary>
    public static class UniqueIdHelper
    {
        /// <summary>
        /// Получить уникальный ID.
        /// </summary>
        /// <param name="src">Данные.</param>
        /// <returns>ID.</returns>
        public static Guid CreateId(byte[] src)
        {
            var prov = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Sha1);
            var hash = prov.CreateHash();
            hash.Append(CryptographicBuffer.CreateFromByteArray(src));
            var result = hash.GetValueAndReset();
            var bytes = result.ToArray();
            var uid = new Guid(bytes.Take(16).ToArray());
            return uid;
        }

        /// <summary>
        /// Получить уникальный ID.
        /// </summary>
        /// <param name="keyString">Исходная строка.</param>
        /// <returns>ID.</returns>
        public static Guid CreateId(string keyString)
        {
            var src = Encoding.UTF8.GetBytes(keyString);
            return CreateId(src);
        }
    }
}