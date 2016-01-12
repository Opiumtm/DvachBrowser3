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
            }
            else
            {
                PoIconSize = 14;
                HeaderIconSize = 14;
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
    }
}