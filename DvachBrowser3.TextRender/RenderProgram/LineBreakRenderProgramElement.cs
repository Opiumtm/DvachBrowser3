namespace DvachBrowser3.TextRender
{
    /// <summary>
    /// Элемент программы с переводом строки.
    /// </summary>
    public sealed class LineBreakRenderProgramElement : IRenderProgramElement
    {
        /// <summary>
        /// Идентификатор.
        /// </summary>
        public string Id
        {
            get { return CommonRenderProgramElements.LineBreak; }
        }
    }
}