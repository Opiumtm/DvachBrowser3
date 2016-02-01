using System;
using System.Threading;
using System.Threading.Tasks;
using DvachBrowser3.Engines;
using DvachBrowser3.Links;
using DvachBrowser3.Logic.NetworkLogic;
using DvachBrowser3.Posting;

namespace DvachBrowser3.Logic
{
    /// <summary>
    /// Операция постинга.
    /// </summary>
    public sealed class PostingOperation : NetworkLogicOperation<IPostingResult, PostingArgument>
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="services">Сервисы.</param>
        /// <param name="parameter">Параметр.</param>
        public PostingOperation(IServiceProvider services, PostingArgument parameter) : base(services, parameter)
        {
        }

        /// <summary>
        /// Выполнить операцию.
        /// </summary>
        /// <param name="token">Токен отмены операции.</param>
        /// <returns>Таск.</returns>
        public override async Task<IPostingResult> Complete(CancellationToken token)
        {
            var engines = ServiceLocator.Current.GetServiceOrThrow<INetworkEngines>();
            var logic = ServiceLocator.Current.GetServiceOrThrow<INetworkLogic>();
            var engine = engines.FindEngine(Parameter.Data.Link.Engine);
            var captcha = Parameter.Captcha;
            if (captcha == null)
            {
                SignalProcessing("Проверка возможности постинга...", "CAPTCHA CHECK");
                if ((engine.Capability & EngineCapability.NoCaptcha) != 0)
                {
                    var ncOperation = engine.CheckNoCaptchaAbility(Parameter.Data.Link);
                    var nc = await ncOperation.Complete(token);
                    if (nc.CanPost)
                    {
                        captcha = new NoCaptchaPostingData();
                    }
                }
            }
            if (captcha == null)
            {
                var keyOperation = engine.GetCaptchaKeys(Parameter.Data.Link, Parameter.CaptchaType ?? engine.DefaultCaptchaType);
                var key = await keyOperation.Complete(token);
                if (!key.NeedCaptcha)
                {
                    captcha = new SkipCaptchaPostingData();
                }
                else
                {
                    return new Result()
                    {
                        Kind = PostingResultKind.NeedCaptcha,
                        CaptchaType = key.Keys.Kind,
                        Engine = Parameter.Data.Link.Engine                        
                    };
                }
            }
            var postOperation = logic.Post(Parameter.Data, captcha, 0);
            postOperation.Progress += (sender, e) => OnProgress(e);
            var link = await postOperation.Complete(token);
            return new Result()
            {
                Kind = PostingResultKind.Success,
                RedirectLink = link
            };
        }

        private sealed class Result : IPostingResult
        {
            /// <summary>
            /// Тип результата.
            /// </summary>
            public PostingResultKind Kind { get; set; }

            /// <summary>
            /// Тип капчи.
            /// </summary>
            public CaptchaType CaptchaType { get; set; }

            /// <summary>
            /// Движок.
            /// </summary>
            public string Engine { get; set; }

            /// <summary>
            /// Ссылка для перенаправления.
            /// </summary>
            public BoardLinkBase RedirectLink { get; set; }
        }
    }
}