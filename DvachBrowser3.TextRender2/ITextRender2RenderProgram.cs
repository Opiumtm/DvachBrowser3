using System.Collections.Generic;

namespace DvachBrowser3.TextRender
{
    /// <summary>
    /// Программа рендеринга.
    /// </summary>
    public interface ITextRender2RenderProgram
    {
        /// <summary>
        /// Получить команды.
        /// </summary>
        /// <returns>Команды.</returns>
        IReadOnlyList<ITextRenderCommand> GetCommands();
    }
}