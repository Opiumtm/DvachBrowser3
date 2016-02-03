namespace DvachBrowser3.TextRender
{
    /// <summary>
    /// Средство размещения элементов.
    /// </summary>
    public interface ITextRender2MeasureMapper
    {
        /// <summary>
        /// Создать карту.
        /// </summary>
        /// <param name="program">Программа.</param>
        /// <param name="width">Ширина.</param>
        /// <param name="fontSize">Размер шрифта.</param>
        /// <param name="maxLines">Максимальное число линий.</param>
        /// <returns>Карта.</returns>
        ITextRender2MeasureMap CreateMap(ITextRender2RenderProgram program, double width, double fontSize, int? maxLines);
    }
}