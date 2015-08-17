namespace DvachBrowser3.TextRender
{
    /// <summary>
    /// Формирователь команд.
    /// </summary>
    public interface ITextRenderCommandFormer
    {
        /// <summary>
        /// Добавить элемент.
        /// </summary>
        /// <param name="element">Элемент.</param>
        /// <returns>true, если добавлено успешно. false, если нужно вызвать Flush</returns>
        bool AddElement(IRenderProgramElement element);

        /// <summary>
        /// Получить команду.
        /// </summary>
        /// <returns>Команда (null - нет команды).</returns>
        ITextRenderCommand GetCommand();

        /// <summary>
        /// Очистить.
        /// </summary>
        void Clear();

        /// <summary>
        /// Очистка текстового буфера.
        /// </summary>
        void Flush();
    }
}