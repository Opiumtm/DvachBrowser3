using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices.ComTypes;
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
        /// Капча.
        /// </summary>
        CaptchaPostingData Captcha { get; set; }

        /// <summary>
        /// Время сохранения.
        /// </summary>
        string SaveTime { get; }

        /// <summary>
        /// Необходимо установить капчу.
        /// </summary>
        event NeedSetCaptchaEventHandler NeedSetCaptcha;

        /// <summary>
        /// Удачный постинг.
        /// </summary>
        event PostingSuccessEventHandler PostingSuccess;

        /// <summary>
        /// Начать постинг.
        /// </summary>
        void Post();
    }
}