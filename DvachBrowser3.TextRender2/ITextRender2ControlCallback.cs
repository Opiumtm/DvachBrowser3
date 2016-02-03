namespace DvachBrowser3.TextRender
{
    /// <summary>
    /// Обратный вызов контрола.
    /// </summary>
    public interface ITextRender2ControlCallback : ITextRender2RenderCallback
    {
        /// <summary>
        /// Получить программу рендеринга.
        /// </summary>
        /// <returns>Программа рендеринга.</returns>
        ITextRender2RenderProgram GetRenderProgram();
    }
}