using System;
using Windows.Storage;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// �������� ������� ��������� �����������.
    /// </summary>
    public sealed class ImageSourceGotEventArgs : EventArgs
    {
        /// <summary>
        /// �����������.
        /// </summary>
        /// <param name="cacheUri">URI ����.</param>
        /// <param name="file">����.</param>
        public ImageSourceGotEventArgs(Uri cacheUri, StorageFile file)
        {
            CacheUri = cacheUri;
            File = file;
        }

        /// <summary>
        /// URI ����.
        /// </summary>
        public Uri CacheUri { get; }

        /// <summary>
        /// ����.
        /// </summary>
        public StorageFile File { get; }
    }
}