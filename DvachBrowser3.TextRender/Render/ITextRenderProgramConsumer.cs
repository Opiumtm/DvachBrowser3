namespace DvachBrowser3.TextRender
{
    /// <summary>
    /// ���������� ���������.
    /// </summary>
    public interface ITextRenderProgramConsumer
    {
        /// <summary>
        /// �������� �������.
        /// </summary>
        /// <param name="element">�������.</param>
        /// <returns>����� ��� ��������� �������.</returns>
        bool PushProgramElement(IRenderProgramElement element);

        /// <summary>
        /// ��������.
        /// </summary>
        void Clear();

        /// <summary>
        /// ��������� ������������������.
        /// </summary>
        void Flush();
    }
}