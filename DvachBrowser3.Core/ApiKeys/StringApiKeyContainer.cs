using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;
using Newtonsoft.Json;

namespace DvachBrowser3.ApiKeys
{
    /// <summary>
    /// Строковый контейнер ключей.
    /// </summary>
    public sealed class StringApiKeyContainer : IApiKeyContainer
    {
        private readonly byte[] c;
        private readonly byte[] i;
        private readonly byte[] p;

        public StringApiKeyContainer(byte[] c, byte[] i, byte[] p)
        {
            this.c = c;
            this.i = i;
            this.p = p;
        }

        /// <summary>
        /// Ключи.
        /// </summary>
        /// <returns>Ключи.</returns>
        public IReadOnlyDictionary<string, IApiKey> GetKeys()
        {
            var prov = SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.AesCbcPkcs7);
            var aes = prov.CreateSymmetricKey(CryptographicBuffer.CreateFromByteArray(p));
            var iv = CryptographicBuffer.CreateFromByteArray(i);
            var dataBuf = CryptographicBuffer.CreateFromByteArray(c);
            var plainBuf = CryptographicEngine.Decrypt(aes, dataBuf, iv);
            var plainBytes = plainBuf.ToArray();
            var plainText = Encoding.UTF8.GetString(plainBytes, 0, plainBytes.Length);
            var obj = JsonConvert.DeserializeObject<KeyContainer>(plainText);
            var res = new Dictionary<string, IApiKey>();
            foreach (var key in obj.Keys)
            {
                res[key.Id] = new ApiKey(key.Value);
            }
            return new ReadOnlyDictionary<string, IApiKey>(res);
        }

        public class KeyContainer
        {
            [JsonProperty("vid")]
            public int Version { get; set; }

            [JsonProperty("cuid")]
            public string UniqueId { get; set; }

            [JsonProperty("keys")]
            public KeyContainerKey[] Keys { get; set; }
        }

        public class KeyContainerKey
        {
            [JsonProperty("k")]
            public string Id { get; set; }

            [JsonProperty("v")]
            public string Value { get; set; }
        }
    }
}