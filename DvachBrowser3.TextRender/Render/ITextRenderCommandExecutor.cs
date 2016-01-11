using System.Threading.Tasks;

namespace DvachBrowser3.TextRender
{
    /// <summary>
    /// Исполнение команды текстового рендеринга.
    /// </summary>
    public interface ITextRenderCommandExecutor
    {
        /// <summary>
        /// Выполнить команду.
        /// </summary>
        /// <param name="command">Команда.</param>
        /// <returns>Остаток для последующего вызова.</returns>
        ITextRenderCommand ExecuteCommand(ITextRenderCommand command);

        /// <summary>
        /// Всего линий отрисовано.
        /// </summary>
        int Lines { get; }

        /// <summary>
        /// Очистить.
        /// </summary>
        void Clear();
    }
}