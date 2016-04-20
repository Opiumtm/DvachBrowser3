namespace DvachBrowser3.Styles
{
    /// <summary>
    /// Иконки.
    /// </summary>
    public sealed class StyleManagerIcons : StyleManagerObjectBase, IStyleManagerIcons
    {
        /// <summary>
        /// Назначить значения.
        /// </summary>
        protected override void SetValues()
        {
            if (!IsNarrowView)
            {
                PoIconSize = 16;
                HeaderIconSize = 16;
                FlagIconHeight = 16;
                FlagIconWidth = 16*1.5;
            }
            else
            {
                PoIconSize = 14;
                HeaderIconSize = 14;
                FlagIconHeight = 14;
                FlagIconWidth = 14 * 1.5;
            }
        }

        private double poIconSize;

        /// <summary>
        /// Размер флага/иконки.
        /// </summary>
        public double PoIconSize
        {
            get { return poIconSize; }
            private set
            {
                poIconSize = value;
                OnPropertyChanged();
            }
        }

        private double headerIconSize;

        /// <summary>
        /// Размер иконки хидера.
        /// </summary>
        public double HeaderIconSize
        {
            get { return headerIconSize; }
            private set
            {
                headerIconSize = value;
                OnPropertyChanged();
            }
        }

        private double flagIconWidth;

        /// <summary>
        /// Ширина флага.
        /// </summary>
        public double FlagIconWidth
        {
            get { return flagIconWidth; }
            private set
            {
                flagIconWidth = value;
                OnPropertyChanged();
            }
        }

        private double flagIncoHeight;

        /// <summary>
        /// Ширина флага.
        /// </summary>
        public double FlagIconHeight
        {
            get { return flagIncoHeight; }
            private set
            {
                flagIncoHeight = value;
                OnPropertyChanged();
            }
        }
    }
}