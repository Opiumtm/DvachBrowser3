namespace DvachBrowser3.TextRender
{
    /// <summary>
    /// Логика рендеринга текста.
    /// </summary>
    public interface ITextRenderLogic
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
        /// Добавить элемент.
        /// </summary>
        /// <param name="element">Элемент.</param>
        /// <returns>Можно ещё выполнять команды.</returns>
        bool PushProgramElement(IRenderProgramElement element);

        /// <summary>
        /// Очистить.
        /// </summary>
        void Clear();

        /// <summary>
        /// Завершить последовательность.
        /// </summary>
        void Flush();
    }
}