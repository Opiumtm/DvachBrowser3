namespace DvachBrowser3.Styles
{
    /// <summary>
    /// Текcт.
    /// </summary>
    public sealed class StyleManagerText : StyleManagerObjectBase, IStyleManagerText
    {
        /// <summary>
        /// Назначить значения.
        /// </summary>
        protected override void SetValues()
        {
            if (!IsNarrowView)
            {
                NormalFontSize = 14.5;
                SmallFontSize = 12;
                TitleFontSize = 18;
                PostFontSize = 14.5;
                ThreadPreviewPostLines = 5;
                ListHeaderFontSize = 24;
                ProgressFontSize = 14.5;
            }
            else
            {
                NormalFontSize = 13.5;
                SmallFontSize = 11.5;
                TitleFontSize = 16;
                PostFontSize = 13.5;
                ThreadPreviewPostLines = 7;
                ListHeaderFontSize = 18;
                ProgressFontSize = 13.5;
            }
        }

        private double normalFontSize;

        /// <summary>
        /// Нормальный размер шрифтаю
        /// </summary>
        public double NormalFontSize
        {
            get { return normalFontSize; }
            private set
            {
                normalFontSize = value;
                OnPropertyChanged();
            }
        }

        private double smallFontSize;

        /// <summary>
        /// Маленький размер шрифта.
        /// </summary>
        public double SmallFontSize
        {
            get { return smallFontSize; }
            private set
            {
                smallFontSize = value;
                OnPropertyChanged();
            }
        }

        private double titleFontSize;

        /// <summary>
        /// Размер шрифта заголовков.
        /// </summary>
        public double TitleFontSize
        {
            get { return titleFontSize; }
            private set
            {
                titleFontSize = value;
                OnPropertyChanged();
            }
        }

        private double postFontSize;

        /// <summary>
        /// Размер текста поста.
        /// </summary>
        public double PostFontSize
        {
            get { return postFontSize; }
            private set
            {
                postFontSize = value;
                OnPropertyChanged();
            }
        }

        private int threadPreviewPostLines;

        /// <summary>
        /// Количество линий в превью поста.
        /// </summary>
        public int ThreadPreviewPostLines
        {
            get { return threadPreviewPostLines; }
            private set
            {
                threadPreviewPostLines = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Размер хидера списка борд.
        /// </summary>
        private double listHeaderFontSize;

        /// <summary>
        /// Размер хидера списка борд.
        /// </summary>
        public double ListHeaderFontSize
        {
            get { return listHeaderFontSize; }
            private set
            {
                listHeaderFontSize = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Размер шрифта прогресса.
        /// </summary>
        private double progressFontSize;

        /// <summary>
        /// Размер шрифта прогресса.
        /// </summary>
        public double ProgressFontSize
        {
            get { return progressFontSize; }
            set
            {
                progressFontSize = value;
                OnPropertyChanged();
            }
        }
    }
}