using System;
using System.Collections.Generic;

namespace DvachBrowser3.TextRender
{
    /// <summary>
    /// Формирователь программы.
    /// </summary>
    public sealed class TextRender2ProgramFormer : ITextRender2ProgramFormer
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="commandFormer">Формирователь команд.</param>
        public TextRender2ProgramFormer(ITextRenderCommandFormer commandFormer)
        {
            if (commandFormer == null) throw new ArgumentNullException(nameof(commandFormer));
            CommandFormer = commandFormer;
        }

        /// <summary>
        /// Формирователь команд.
        /// </summary>
        public ITextRenderCommandFormer CommandFormer { get; }

        private readonly List<ITextRenderCommand> commands = new List<ITextRenderCommand>();

        /// <summary>
        /// Получить программу.
        /// </summary>
        /// <returns></returns>
        public ITextRender2RenderProgram GetProgram()
        {
            return new RenderProgram(commands.ToArray());
        }

        /// <summary>
        /// Добавить элемент.
        /// </summary>
        /// <param name="element">Элемент.</param>
        /// <returns>Можно ещё выполнять команды.</returns>
        public bool PushProgramElement(IRenderProgramElement element)
        {
            if (!CommandFormer.AddElement(element))
            {
                Flush();
                CommandFormer.AddElement(element);
            }
            return true;
        }

        /// <summary>
        /// Очистить.
        /// </summary>
        public void Clear()
        {
            CommandFormer.Clear();
            commands.Clear();
        }

        /// <summary>
        /// Завершить последовательность.
        /// </summary>
        public void Flush()
        {
            var command = CommandFormer.GetCommand();
            if (command != null)
            {
                commands.Add(command);
            }
            CommandFormer.Flush();
        }

        private sealed class RenderProgram : ITextRender2RenderProgram
        {
            private readonly ITextRenderCommand[] commands;

            /// <summary>
            /// Инициализирует новый экземпляр класса <see cref="T:System.Object"/>.
            /// </summary>
            public RenderProgram(ITextRenderCommand[] commands)
            {
                if (commands == null) throw new ArgumentNullException(nameof(commands));
                this.commands = commands;
            }

            /// <summary>
            /// Получить команды.
            /// </summary>
            /// <returns>Команды.</returns>
            public IReadOnlyList<ITextRenderCommand> GetCommands()
            {
                return commands;
            }
        }
    }
}