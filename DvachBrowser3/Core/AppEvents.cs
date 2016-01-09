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

        /// <summary>
        /// Приостановка приложения.
        /// </summary>
        public static readonly Guid AppSuspendId = new Guid("{58A2586F-E429-47DC-99D3-E2DD06631A71}");

        /// <summary>
        /// Приостановка приложения.
        /// </summary>
        public static readonly IWeakEventChannel AppSuspend = new WeakEventChannel(AppSuspendId);
    }
}