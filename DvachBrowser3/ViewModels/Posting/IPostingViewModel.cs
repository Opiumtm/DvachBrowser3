using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using DvachBrowser3.Engines;
using DvachBrowser3.Links;
using DvachBrowser3.Logic;
using DvachBrowser3.Posting;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Модель представления постинга.
    /// </summary>
    public interface IPostingViewModel : INotifyPropertyChanged, IStartableViewModel
    {
        /// <summary>
        /// Ссылка, куда постинг.
        /// </summary>
        BoardLinkBase PostingLink { get; }

        /// <summary>
        /// Операция постинга.
        /// </summary>
        IOperationViewModel Posting { get; }

        /// <summary>
        /// Поля.
        /// </summary>
        IPostingFieldsViewModel Fields { get; }

        /// <summary>
        /// Тип капчи.
        /// </summary>
        CaptchaType CaptchaType { get; }

        /// <summary>
        /// Время сохранения.
        /// </summary>
        string StorageDate { get; }

        /// <summary>
        /// Капча.
        /// </summary>
        CaptchaPostingData Captcha { get; set; }

        /// <summary>
        /// Необходимо установить капчу.
        /// </summary>
        event NeedSetCaptchaEventHandler NeedSetCaptcha;

        /// <summary>
        /// Начать постинг.
        /// </summary>
        void Post();
    }
}