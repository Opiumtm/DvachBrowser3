using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Newtonsoft.Json;

namespace DvachBrowser3.ApiKeys
{
    /// <summary>
    /// Строковый контейнер ключей.
    /// </summary>
    public sealed class StringApiKeyContainer : IApiKeyContainer
    {
        private readonly Lazy<string> containerStr;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="containerStr">Строковое представление.</param>
        public StringApiKeyContainer(Func<string> containerStr)
        {
            this.containerStr = new Lazy<string>(containerStr);
        }

        /// <summary>
        /// Ключи.
        /// </summary>
        /// <returns>Ключи.</returns>
        public IReadOnlyDictionary<string, IApiKey> GetKeys()
        {
            var parts = containerStr.Value.Split('|');
            if (parts.Length != 3)
            {
                throw new InvalidOperationException("Неправильно задан контейнер ключей");
            }
            var uniqueId = parts[0];
            var password = parts[1];
            var data = parts[2];
            var passwordHash = GetPasswordHash(password);
            var prov = SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.AesCbcPkcs7);
            var aes = prov.CreateSymmetricKey(CryptographicBuffer.CreateFromByteArray(passwordHash.Take(16).ToArray()));
            var iv = CryptographicBuffer.DecodeFromBase64String(uniqueId);
            var dataBuf = CryptographicBuffer.DecodeFromBase64String(data);
            var plainBuf = CryptographicEngine.Decrypt(aes, dataBuf, iv);
            var plainBytes = plainBuf.ToArray();
            var plainText = Encoding.UTF8.GetString(plainBytes, 0, plainBytes.Length);
            var obj = JsonConvert.DeserializeObject<KeyContainer>(plainText);
            if (obj.UniqueId != uniqueId)
            {
                throw new InvalidOperationException("Не совпадает уникальный идентификатор контейнера ключей");
            }
            if (obj.Version != 0x010000)
            {
                throw new InvalidOperationException("Не совпадает версия контейнера ключей");
            }
            var res = new Dictionary<string, IApiKey>();
            foreach (var key in obj.Keys)
            {
                res[key.Id] = new ApiKey(key.Value);
            }
            return new ReadOnlyDictionary<string, IApiKey>(res);
        }

        private byte[] GetPasswordHash(string password)
        {
            var src = Encoding.UTF8.GetBytes(password);
            var prov = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Sha1);
            var hash = prov.CreateHash();
            hash.Append(CryptographicBuffer.CreateFromByteArray(src));
            var result = hash.GetValueAndReset();
            return result.ToArray();
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