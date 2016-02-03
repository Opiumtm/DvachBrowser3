namespace DvachBrowser3.TextRender
{
    /// <summary>
    /// Получатель программы.
    /// </summary>
    public interface ITextRenderProgramConsumer
    {
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