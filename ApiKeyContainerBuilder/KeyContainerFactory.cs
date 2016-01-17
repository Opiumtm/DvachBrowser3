using System;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;
using Newtonsoft.Json;

namespace ApiKeyContainerBuilder
{
    public class KeyContainerFactory
    {
        /// <summary>
        /// ID приложения.
        /// </summary>
        private const string KeyApplicationId = "appid";

        /// <summary>
        /// Секретный ключ.
        /// </summary>
        private const string KeySecretKey = "secretkey";

        private readonly string privateKey;
        private readonly string publicKey;
        private readonly string password;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="T:System.Object"/>.
        /// </summary>
        public KeyContainerFactory(string privateKey, string publicKey, string password)
        {
            this.privateKey = privateKey;
            this.publicKey = publicKey;
            this.password = password;
        }

        public void GenerateContainerString()
        {
            var passwordHash = GetPasswordHash();
            var prov = SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.AesCbcPkcs7);
            var aes = prov.CreateSymmetricKey(CryptographicBuffer.CreateFromByteArray(passwordHash.Take(16).ToArray()));
            var iv = CryptographicBuffer.GenerateRandom(aes.KeySize / 8);
            UniqueId = CryptographicBuffer.EncodeToBase64String(iv);
            var plainText = GetPlainTextString();
            var plainBuf = CryptographicBuffer.CreateFromByteArray(Encoding.UTF8.GetBytes(plainText));
            var r = CryptographicEngine.Encrypt(aes, plainBuf, iv);
            ContainerStr = CryptographicBuffer.EncodeToBase64String(r);
        }

        private string GetPlainTextString()
        {
            var container = new KeyContainer()
            {
                Version = 0x010000,
                UniqueId = UniqueId,
                Keys = new []
                {                    
                    new KeyContainerKey() { Id = KeyApplicationId, Value = publicKey }, 
                    new KeyContainerKey() { Id = KeySecretKey, Value = privateKey}, 
                }
            };
            return JsonConvert.SerializeObject(container, Formatting.None);
        }

        private byte[] GetPasswordHash()
        {
            var src = Encoding.UTF8.GetBytes(password);
            var prov = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Sha1);
            var hash = prov.CreateHash();
            hash.Append(CryptographicBuffer.CreateFromByteArray(src));
            var result = hash.GetValueAndReset();
            return result.ToArray();
        }

        /// <summary>
        /// Уникальный ID.
        /// </summary>
        public string UniqueId { get; private set; }

        /// <summary>
        /// Строка контейнера.
        /// </summary>
        public string ContainerStr { get; private set; }
    }

    public class EncryptedString
    {
        private readonly string original;
        private readonly string password;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="T:System.Object"/>.
        /// </summary>
        public EncryptedString(string original, string password)
        {
            this.original = original;
            this.password = password;
        }
        private byte[] GetPasswordHash(string salt)
        {
            var src = Encoding.UTF8.GetBytes($"{password}+{salt}");
            var prov = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Sha1);
            var hash = prov.CreateHash();
            hash.Append(CryptographicBuffer.CreateFromByteArray(src));
            var result = hash.GetValueAndReset();
            return result.ToArray();
        }

        public string Encrypt()
        {
            var passwordHash = GetPasswordHash("pwd");
            var ivHash = GetPasswordHash("iv");
            var prov = SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.AesCbcPkcs7);
            var aes = prov.CreateSymmetricKey(CryptographicBuffer.CreateFromByteArray(passwordHash.Take(16).ToArray()));
            var iv = CryptographicBuffer.CreateFromByteArray(ivHash.Take(16).ToArray());
            var plainBuf = CryptographicBuffer.CreateFromByteArray(Encoding.UTF8.GetBytes(original));
            var r = CryptographicEngine.Encrypt(aes, plainBuf, iv);
            //return CryptographicBuffer.EncodeToBase64String(r);
            var sb = new StringBuilder();
            sb.Append("private static readonly byte[] C = new byte[] {");
            bool f = true;
            foreach (var b in r.ToArray())
            {
                if (!f) sb.Append(",");
                f = false;
                sb.Append("0x" + b.ToString("X2"));
            }
            sb.AppendLine("};");

            sb.Append("private static readonly byte[] I = new byte[] {");
            f = true;
            foreach (var b in ivHash.Take(16).ToArray())
            {
                if (!f) sb.Append(",");
                f = false;
                sb.Append("0x" + b.ToString("X2"));
            }
            sb.AppendLine("};");

            sb.Append("private static readonly byte[] P = new byte[] {");
            f = true;
            foreach (var b in passwordHash.Take(16).ToArray())
            {
                if (!f) sb.Append(",");
                f = false;
                sb.Append("0x" + b.ToString("X2"));
            }
            sb.AppendLine("};");
            return sb.ToString();
        }
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