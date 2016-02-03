namespace DvachBrowser3.TextRender
{
    /// <summary>
    /// Формирователь программы.
    /// </summary>
    public interface ITextRender2ProgramFormer : ITextRenderProgramConsumer
    {
        /// <summary>
        /// Формирователь команд.
        /// </summary>
        ITextRenderCommandFormer CommandFormer { get; }

        /// <summary>
        /// Получить программу.
        /// </summary>
        /// <returns></returns>
        ITextRender2RenderProgram GetProgram();
    }
}