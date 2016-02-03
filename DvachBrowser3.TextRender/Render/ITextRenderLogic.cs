namespace DvachBrowser3.TextRender
{
    /// <summary>
    /// Логика рендеринга текста.
    /// </summary>
    public interface ITextRenderLogic : ITextRenderProgramConsumer
    {
        /// <summary>
        /// Формирователь команд.
        /// </summary>
        ITextRenderCommandFormer Former { get; }

        /// <summary>
        /// Исполнитель команд.
        /// </summary>
        ITextRenderCommandExecutor Executor { get; }

        /// <summary>
        /// Максимальное количество линий.
        /// </summary>
        int? MaxLines { get; set; }

        /// <summary>
        /// Превышено количество линий.
        /// </summary>
        bool ExceedLines { get; }
    }
}