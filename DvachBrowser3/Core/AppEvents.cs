using System;

namespace DvachBrowser3
{
    /// <summary>
    /// События приложения.
    /// </summary>
    public static class AppEvents
    {
        /// <summary>
        /// Возобновление приложения.
        /// </summary>
        public static readonly Guid AppResumeId = new Guid("{407E852B-5EB1-4C23-880D-F32ED1BA24A1}");

        /// <summary>
        /// Возобновление приложения.
        /// </summary>
        public static readonly IWeakEventChannel AppResume = new WeakEventChannel(AppResumeId);
    }
}