using System;

namespace DvachBrowser3.TextRender
{
    /// <summary>
    /// Логика рендеринга текста.
    /// </summary>
    public sealed class TextRenderLogic : ITextRenderLogic
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="former">Формирователь команд.</param>
        /// <param name="executor">Исполнитель команд.</param>
        public TextRenderLogic(ITextRenderCommandFormer former, ITextRenderCommandExecutor executor)
        {
            if (former == null) throw new ArgumentNullException("former");
            if (executor == null) throw new ArgumentNullException("executor");
            Executor = executor;
            Former = former;
        }

        /// <summary>
        /// Формирователь команд.
        /// </summary>
        public ITextRenderCommandFormer Former { get; private set; }

        /// <summary>
        /// Исполнитель команд.
        /// </summary>
        public ITextRenderCommandExecutor Executor { get; private set; }

        /// <summary>
        /// Максимальное количество линий.
        /// </summary>
        public int? MaxLines { get; set; }

        /// <summary>
        /// Добавить элемент.
        /// </summary>
        /// <param name="element">Элемент.</param>
        /// <returns>Можно ещё выполнять команды.</returns>
        public bool PushProgramElement(IRenderProgramElement element)
        {
            if (ExceedLines)
            {
                return false;
            }
            if (!Former.AddElement(element))
            {
                Flush();
                Former.AddElement(element);
            }
            return true;
        }

        private bool ExceedLines
        {
            get
            {
                if (MaxLines != null)
                {
                    if (Executor.Lines >= MaxLines.Value)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// Очистить.
        /// </summary>
        public void Clear()
        {
            Former.Clear();
            Executor.Clear();
        }

        /// <summary>
        /// Завершить последовательность.
        /// </summary>
        public void Flush()
        {
            Execute(Former.GetCommand());
            Former.Flush();
        }

        private void Execute(ITextRenderCommand command)
        {
            int counter = 1000;
            while (command != null && counter > 0 && !ExceedLines)
            {
                counter--;
                command = Executor.ExecuteCommand(command);
            }
        }
    }
}