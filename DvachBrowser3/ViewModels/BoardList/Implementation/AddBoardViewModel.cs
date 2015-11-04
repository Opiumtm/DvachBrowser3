using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using DvachBrowser3.Board;
using DvachBrowser3.Engines;
using DvachBrowser3.Logic;
using DvachBrowser3.Storage;
using Template10.Common;
using Template10.Mvvm;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Модель представления для добавления борды.
    /// </summary>
    public sealed class AddBoardViewModel : ViewModelBase, IAddBoardViewModel
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public AddBoardViewModel()
        {
            var engineSrv = ServiceLocator.Current.GetServiceOrThrow<INetworkEngines>();
            var engineIds = engineSrv.ListEngines();
            foreach (var id in engineIds)
            {
                Engines.Add(engineSrv.GetEngineById(id));
            }
            if (Engines.Count > 0)
            {
                SelectedEngine = Engines[0];
            }
            this.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName.Equals(nameof(SelectedEngine), StringComparison.OrdinalIgnoreCase) || e.PropertyName.Equals(nameof(ShortName), StringComparison.OrdinalIgnoreCase))
                {
                    UpdateDescription();
                }
            };
        }

        /// <summary>
        /// Движки.
        /// </summary>
        public IList<INetworkEngine> Engines { get; } = new ObservableCollection<INetworkEngine>();

        private INetworkEngine selectedEngine;

        /// <summary>
        /// Выбранный движок.
        /// </summary>
        public INetworkEngine SelectedEngine
        {
            get { return selectedEngine; }
            set
            {
                selectedEngine = value;
                RaisePropertyChanged();
            }
        }

        private string shortName = "";

        /// <summary>
        /// Короткое имя.
        /// </summary>
        public string ShortName
        {
            get { return shortName; }
            set
            {
                shortName = value;
                RaisePropertyChanged();
            }
        }

        private string description = "";

        /// <summary>
        /// Описание.
        /// </summary>
        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Обновить значения.
        /// </summary>
        /// <param name="selectedEngine1">Выбранный движок.</param>
        /// <param name="shortName1">Короткое имя.</param>
        /// <param name="description1">Описание.</param>
        public void ResetValues(object selectedEngine1, string shortName1, string description1)
        {
            selectedEngine = selectedEngine1 as INetworkEngine;
            shortName = shortName1;
            description = description1;
        }

        /// <summary>
        /// Получить модель борды.
        /// </summary>
        /// <returns>Модель борды.</returns>
        public IBoardListBoardViewModel GetBoardModel()
        {
            if (SelectedEngine == null)
            {
                return null;
            }
            if (string.IsNullOrWhiteSpace(ShortName))
            {
                return null;
            }
            return new BoardListBoardDataViewModel(SelectedEngine.CreateBoardLink(ShortName), Description, "", false, false);
        }

        private string cachedEngineId;

        private BoardReferences references;

        async void UpdateDescription()
        {
            try
            {
                var link = SelectedEngine?.RootLink;
                if (SelectedEngine?.EngineId != cachedEngineId)
                {
                    cachedEngineId = SelectedEngine?.EngineId;
                    if (link != null)
                    {
                        references = await ServiceLocator.Current.GetServiceOrThrow<IStorageService>().ThreadData.LoadBoardReferences(link);
                    }
                    else
                    {
                        references = null;
                    }
                }
            }
            catch
            {
                references = null;
            }
            if (string.IsNullOrWhiteSpace(ShortName))
            {
                Description = "";
            }
            else
            {
                var sn = ShortName.Trim();
                var lts = ServiceLocator.Current.GetServiceOrThrow<ILinkTransformService>();
                var board = (references?.References ?? new List<BoardReference>()).FirstOrDefault(b => sn.Equals(lts.GetBoardShortName(b.Link), StringComparison.OrdinalIgnoreCase));
                if (board != null)
                {
                    Description = board.DisplayName;
                }
                else
                {
                    Description = null;
                }
            }
        }
    }
}