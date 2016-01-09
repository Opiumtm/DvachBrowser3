using System;
using DvachBrowser3.ViewModels;

namespace DvachBrowser3.Views.Partial
{
    /// <summary>
    /// Аргумент события по нажатию на тред.
    /// </summary>
    public sealed class ThreadPreviewTappedEventArgs : EventArgs
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="thread">Тред.</param>
        public ThreadPreviewTappedEventArgs(IThreadPreviewViewModel thread)
        {
            Thread = thread;
        }

        /// <summary>
        /// Тред.
        /// </summary>
        public IThreadPreviewViewModel Thread { get; }
    }
}