﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Web.Http;
using Windows.Web.Http.Filters;
using DvachBrowser3.ApiKeys;
using DvachBrowser3.Engines.Makaba.Json;
using DvachBrowser3.Links;
using DvachBrowser3.Posting;
using Newtonsoft.Json;

namespace DvachBrowser3.Engines.Makaba.Operations
{
    /// <summary>
    /// Постинг Makaba.
    /// </summary>
    public sealed class MakabaPostOperation : HttpPostEngineOperationBase<IPostingResult, PostEntryData>
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="parameter">Параметр.</param>
        /// <param name="services">Сервисы.</param>
        public MakabaPostOperation(PostEntryData parameter, IServiceProvider services) : base(parameter, services)
        {
            apiKeys = new Lazy<ApiKey>(GetAppId);
        }

        protected override Uri GetRequestUri()
        {
            return Services.GetServiceOrThrow<IMakabaUriService>().GetPostingUri();
        }

        protected override async Task<IHttpContent> GetPostContent()
        {
            var pr = GetPostingRef();
            var result = new HttpMultipartFormDataContent();
            var stringData = new List<KeyValuePair<string, string>>();
            stringData.Add(new KeyValuePair<string, string>("task", "post"));
            stringData.Add(new KeyValuePair<string, string>("thread", pr.Thread.ToString(CultureInfo.InvariantCulture)));
            stringData.Add(new KeyValuePair<string, string>("board", pr.Board));

            if (Parameter.CommonData.ContainsKey(PostingFieldSemanticRole.WatermarkFlag))
            {
                var wm = (bool)Parameter.CommonData[PostingFieldSemanticRole.WatermarkFlag];
                if (wm)
                {
                    stringData.Add(new KeyValuePair<string, string>("water_mark", "on"));
                }
            }

            if (Parameter.CommonData.ContainsKey(PostingFieldSemanticRole.OpFlag))
            {
                var wm = (bool)Parameter.CommonData[PostingFieldSemanticRole.OpFlag];
                if (wm)
                {
                    stringData.Add(new KeyValuePair<string, string>("op_mark", "on"));
                }
            }

            if (Parameter.CommonData.ContainsKey(PostingFieldSemanticRole.Icon))
            {
                var wm = (int)Parameter.CommonData[PostingFieldSemanticRole.Icon];
                stringData.Add(new KeyValuePair<string, string>("icon", wm.ToString(CultureInfo.CurrentCulture)));
            }

            if (Parameter.CommonData.ContainsKey(PostingFieldSemanticRole.PosterName))
            {
                var val = (string)Parameter.CommonData[PostingFieldSemanticRole.PosterName];
                string trip1 = null;
                string trip2 = null;

                if (Parameter.CommonData.ContainsKey(PostingFieldSemanticRole.PosterTrip))
                {
                    trip1 = (string) Parameter.CommonData[PostingFieldSemanticRole.PosterTrip];
                }
                if (Parameter.CommonData.ContainsKey(PostingFieldSemanticRole.PosterTrip2))
                {
                    trip2 = (string)Parameter.CommonData[PostingFieldSemanticRole.PosterTrip2];
                }

                trip1 = trip1?.Replace("#", "");
                trip2 = trip2?.Replace("#", "");

                if (!string.IsNullOrWhiteSpace(trip1))
                {
                    val = val + "#" + trip1;
                }
                if (!string.IsNullOrWhiteSpace(trip2))
                {
                    val = val + "##" + trip2;
                }

                stringData.Add(new KeyValuePair<string, string>("name", val));
            }

            bool isSage = false;
            if (Parameter.CommonData.ContainsKey(PostingFieldSemanticRole.SageFlag))
            {
                var wm = (bool)Parameter.CommonData[PostingFieldSemanticRole.SageFlag];
                if (wm)
                {
                    stringData.Add(new KeyValuePair<string, string>("email", "sage"));
                    isSage = true;
                }                
            }

            if (Parameter.CommonData.ContainsKey(PostingFieldSemanticRole.Email) && !isSage)
            {
                var val = (string)Parameter.CommonData[PostingFieldSemanticRole.Email];
                stringData.Add(new KeyValuePair<string, string>("email", val));
            }
            
            if (Parameter.CommonData.ContainsKey(PostingFieldSemanticRole.Title))
            {
                var val = (string)Parameter.CommonData[PostingFieldSemanticRole.Title];
                stringData.Add(new KeyValuePair<string, string>("subject", val));
            }

            if (Parameter.CommonData.ContainsKey(PostingFieldSemanticRole.ThreadTag))
            {
                var val = (string)Parameter.CommonData[PostingFieldSemanticRole.ThreadTag];
                stringData.Add(new KeyValuePair<string, string>("tags", val));
            }

            var comment = "";

            if (Parameter.CommonData.ContainsKey(PostingFieldSemanticRole.Comment))
            {
                var val = (string)Parameter.CommonData[PostingFieldSemanticRole.Comment] ?? "";
                var correctionService = Services.GetServiceOrThrow<INetworkEngines>().GetEngineById(CoreConstants.Engine.Makaba).PostCorrection;
                if (correctionService != null)
                {
                    val = correctionService.CorrectPostText(val);
                }
                comment = val;
            }
            stringData.Add(new KeyValuePair<string, string>("comment", comment));

            if (Parameter.Captcha != null)
            {
                if (Parameter.Captcha is DvachCaptchaPostingData)
                {
                    var c = Parameter.Captcha as DvachCaptchaPostingData;
                    stringData.Add(new KeyValuePair<string, string>("captcha_type", "2chaptcha"));
                    stringData.Add(new KeyValuePair<string, string>("2chaptcha_id", c.Key));
                    stringData.Add(new KeyValuePair<string, string>("2chaptcha_value", c.CaptchaEntry));
                }
                if (Parameter.Captcha is YandexCaptchaPostingData)
                {
                    var c = Parameter.Captcha as YandexCaptchaPostingData;
                    stringData.Add(new KeyValuePair<string, string>("captcha", c.Key));
                    stringData.Add(new KeyValuePair<string, string>("captcha_value", c.CaptchaEntry));
                }
                if (Parameter.Captcha is RecaptchaCaptchaPostingData)
                {
                    var c = Parameter.Captcha as RecaptchaCaptchaPostingData;
                    stringData.Add(new KeyValuePair<string, string>("captcha_type", "recaptcha"));
                    stringData.Add(new KeyValuePair<string, string>("g-recaptcha-response", c.RecaptchaHash));
                }
                if (Parameter.Captcha is RecaptchaV1CaptchaPostingData)
                {
                    var c = Parameter.Captcha as RecaptchaV1CaptchaPostingData;
                    stringData.Add(new KeyValuePair<string, string>("captcha_type", "recaptchav1"));
                    stringData.Add(new KeyValuePair<string, string>("recaptcha_challenge_field", c.Challenge));
                    stringData.Add(new KeyValuePair<string, string>("recaptcha_response_field", c.CaptchaEntry));
                }
                if (Parameter.Captcha is NoCaptchaPostingData)
                {
                    await AddNoCaptchaData(stringData, comment);
                }
                if (Parameter.Captcha is SkipCaptchaPostingData)
                {
                    // Не добавляем капчу - не нужно
                }
            }
            
            foreach (var sd in stringData)
            {
                result.Add(new HttpStringContent(sd.Value ?? ""), sd.Key);
            }

            for (int i = 0; i < Parameter.MediaFiles.Length; i++)
            {
                var parName = "image" + (i + 1).ToString(CultureInfo.InvariantCulture);
                var str = await Parameter.MediaFiles[i].TempFile.OpenReadAsync();
                result.Add(new HttpStreamContent(str), parName, Parameter.MediaFiles[i].FileName);
            }

            return result;
        }

        private async Task AddNoCaptchaData(List<KeyValuePair<string, string>> stringData, string comment)
        {
            var sessionKey = await GetApiSessionKey();
            var signature = GetApiSignature(sessionKey);
            stringData.Add(new KeyValuePair<string, string>("captcha_type", "app"));
            stringData.Add(new KeyValuePair<string, string>("app_response_id", sessionKey));
            stringData.Add(new KeyValuePair<string, string>("app_response", signature));
            //stringData.Add(new KeyValuePair<string, string>("app_signature", randomKey));
        }

        private string GetApiSignature(string id)
        {
            var contentToSign = $"{id}|{apiKeys.Value.Key}";
            var hash = GetSha1Hash(contentToSign);
            var sb = new StringBuilder();
            foreach (var b in hash)
            {
                sb.AppendFormat("{0:X2}", b);
            }
            return sb.ToString().ToLower();
        }

        private async Task<string> GetApiSessionKey()
        {
            var client = await CreateClient();
            var uri = Services.GetServiceOrThrow<IMakabaUriService>().GetNocaptchaUri(false, apiKeys.Value.Id);
            var str = await client.GetStringAsync(uri);
            var message = JsonConvert.DeserializeObject<CaptchaV2IdResult>(str);
            switch (message.Result)
            {
                // 1 - Запрос удовлетворён успешно.
                case 1:
                    if (message.Id == null)
                    {
                        throw new WebException("Неправильный ответ с сервера (captcha.id = null)");
                    }
                    return message.Id;
                // 0 - При выполнении запроса возникла ошибка. Код ошибки находится в переменной error, описание ошибки в переменной description.
                case 0:
                    throw new WebException($"Ошибка получения капчи {message.Error}: {message.ErrorDescription}");
                default:
                    throw new WebException("Неправильный ответ с сервера (captcha.result invalid)");
            }
        }

        private readonly Lazy<ApiKey> apiKeys;

        /// <summary>
        /// Получить ID приложения.
        /// </summary>
        /// <returns>ID приложения.</returns>
        private ApiKey GetAppId()
        {
            var api = Services.GetServiceOrThrow<IApiKeyService>();
            var container = api.Find(CoreConstants.ApiKeys.Containers.MakabaPosting);
            var keys = container?.GetKeys();
            if (keys == null)
            {
                throw new InvalidOperationException("Нет ключей приложения");
            }
            if (!keys.ContainsKey(CoreConstants.ApiKeys.ApplicationId) || !keys.ContainsKey(CoreConstants.ApiKeys.SecretKey))
            {
                throw new InvalidOperationException("Нет ключей приложения");
            }
            return new ApiKey()
            {
                Id = keys[CoreConstants.ApiKeys.ApplicationId].Get(),
                Key = keys[CoreConstants.ApiKeys.SecretKey].Get(),
            };
        }

        private byte[] GetSha1Hash(string contentToSign)
        {
            var prov = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Sha256);
            var sha1 = prov.CreateHash();
            var buf = Encoding.UTF8.GetBytes(contentToSign);
            sha1.Append(CryptographicBuffer.CreateFromByteArray(buf));
            var hashBuf = sha1.GetValueAndReset();
            return hashBuf.ToArray();
        }

        private PostingRef GetPostingRef()
        {
            if (Parameter.Link is BoardLink)
            {
                var l = Parameter.Link as BoardLink;
                return new PostingRef() { Board = l.Board, Thread = 0 };
            }
            if (Parameter.Link is ThreadLink)
            {
                var l = Parameter.Link as ThreadLink;
                return new PostingRef() { Board = l.Board, Thread = l.Thread };
            }
            if (Parameter.Link is PostLink)
            {
                var l = Parameter.Link as PostLink;
                return new PostingRef() { Board = l.Board, Thread = l.Thread };
            }
            throw new ArgumentException("Неправильный формат ссылки (post)");
        }

        protected override async Task<IPostingResult> DoComplete(HttpResponseMessage message, CancellationToken token)
        {
            var pr = GetPostingRef();
            message.EnsureSuccessStatusCode();
            ulong length;
            var hasLength = message.Content.TryComputeLength(out length);
            ulong? length1 = hasLength ? (ulong?) length : null;
            var str = await message.Content.ReadAsStringAsync().AsTask(token, new Progress<ulong>(l => DownloadProgress(length1, l)));
            string error = null;
            try
            {
                var obj = JsonConvert.DeserializeObject<PostingJsonResult>(str);
                if ("OK".Equals(obj.Status, StringComparison.OrdinalIgnoreCase) || "Redirect".Equals(obj.Status, StringComparison.OrdinalIgnoreCase))
                {
                    if (!string.IsNullOrWhiteSpace(obj.Target) && "Redirect".Equals(obj.Status, StringComparison.OrdinalIgnoreCase))
                    {
                        int n;
                        if (int.TryParse(obj.Target, out n))
                        {
                            return new OperationResult()
                            {
                                RedirectLink = new ThreadLink()
                                {
                                    Engine = CoreConstants.Engine.Makaba,
                                    Board = pr.Board,
                                    Thread = n
                                }
                            };
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(obj.Num) && "OK".Equals(obj.Status, StringComparison.OrdinalIgnoreCase) && pr.Thread > 0)
                    {
                        int n;
                        if (int.TryParse(obj.Num, out n))
                        {
                            return new OperationResult()
                            {
                                PostLink = new PostLink()
                                {
                                    Engine = CoreConstants.Engine.Makaba,
                                    Board = pr.Board,
                                    Thread = pr.Thread,
                                    Post = n
                                }
                            };
                        }                        
                    }
                    return new OperationResult();
                }
                error = obj.Reason;
            }
            catch (Exception ex)
            {
                throw new WebException("Сервер вернул неправильный ответ");
            }
            throw new WebException(error ?? "Сервер вернул неправильный ответ");
        }

        /// <summary>
        /// Установить хидеры.
        /// </summary>
        /// <param name="client">Клиент.</param>
        /// <param name="filter">Фильтр.</param>
        /// <returns>Хидеры.</returns>
        protected override async Task SetHeaders(HttpClient client, IHttpFilter filter)
        {
            await base.SetHeaders(client, filter);
            await MakabaHeadersHelper.SetClientHeaders(Services, client, filter);
        }

        private class OperationResult : IPostingResult
        {
            public BoardLinkBase RedirectLink { get; set; }
            public BoardLinkBase PostLink { get; set; }
        }

        private class PostingRef
        {
            public string Board { get; set; }
            public int Thread { get; set; }
        }

        private struct ApiKey
        {
            public string Id;
            public string Key;
        }
    }
}