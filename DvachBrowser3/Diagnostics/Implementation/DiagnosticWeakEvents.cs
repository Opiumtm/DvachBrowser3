using System.Globalization;
using System.Threading.Tasks;

namespace DvachBrowser3.Diagnostics
{
    /// <summary>
    /// Диагностика слабо связанных событий.
    /// </summary>
    public sealed class DiagnosticWeakEvents : IDiagnosticWeakEvents
    {
        public IDiagnosticValue LastCleanupRun { get; } = new LastCleanupRunValue();

        public IDiagnosticValue ActiveDictionaries { get; } = new ActiveDictionariesValue();

        public IDiagnosticValue ActiveCallbacks { get; } = new ActiveCallbacksValue();

        /// <summary>
        /// Очистить.
        /// </summary>
        public void Cleanup()
        {
            AppHelpers.DispatchAction(DoCleanup);
        }

        private async Task DoCleanup()
        {
            await WeakEventChannel.TriggerCleanup();
            Invalidate();
        }

        private void Invalidate()
        {
            ((DiagnosticValueBase)LastCleanupRun).TriggerChange();
            ((DiagnosticValueBase)ActiveDictionaries).TriggerChange();
            ((DiagnosticValueBase)ActiveCallbacks).TriggerChange();
        }

        private sealed class LastCleanupRunValue : DiagnosticValueBase
        {
            public override string Name => "Последний запуск очистки";

            public override async Task<string> QueryAsync()
            {
                var v = await WeakEventChannel.GetLastRun();
                if (v == null)
                {
                    return "Никогда";
                }
                return v.Value.ToString(CultureInfo.CurrentCulture);
            }
        }

        private sealed class ActiveDictionariesValue : DiagnosticValueBase
        {
            public override string Name => "Контейнеров обработчиков";

            public override async Task<string> QueryAsync()
            {
                var v = await WeakEventChannel.GetDictionariesCount();
                return v.ToString();
            }
        }

        private sealed class ActiveCallbacksValue : DiagnosticValueBase
        {
            public override string Name => "Всего обработчиков";

            public override async Task<string> QueryAsync()
            {
                var v = await WeakEventChannel.GetCallbacksCount();
                return v.ToString();
            }
        }
    }
}