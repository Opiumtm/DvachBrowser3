using System;
using System.Collections.Generic;
using DvachBrowser3.Posting;

namespace DvachBrowser3.Markup
{
    /// <summary>
    /// Сервис разметки.
    /// </summary>
    public sealed class MarkupService : ServiceBase, IMarkupService
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="services">Сервисы.</param>
        public MarkupService(IServiceProvider services) : base(services)
        {
        }

        private readonly Dictionary<PostingMarkupType, IMarkupProvider> providers = new Dictionary<PostingMarkupType, IMarkupProvider>();

        /// <summary>
        /// Получить провайдер разметки.
        /// </summary>
        /// <param name="markupType">Тип разметки.</param>
        /// <returns>Провайдер.</returns>
        public IMarkupProvider GetProvider(PostingMarkupType markupType)
        {
            lock (providers)
            {
                if (!providers.ContainsKey(markupType))
                {
                    throw new ArgumentException("Провайдер разметки не найден", nameof(markupType));
                }
                return providers[markupType];
            }
        }

        /// <summary>
        /// Зарегистрировать провайдер.
        /// </summary>
        /// <param name="markupProvider">Провайдер.</param>
        public void RegisterProvider(IMarkupProvider markupProvider)
        {
            lock (providers)
            {
                providers[markupProvider.MarkupType] = markupProvider;
            }
        }
    }
}