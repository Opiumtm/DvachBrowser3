﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using DvachBrowser3.Logic;

namespace DvachBrowser3.Links
{
    /// <summary>
    /// Базовый класс для ссылки на борде.
    /// </summary>
    [DataContract(Namespace = CoreConstants.DvachBrowserNamespace)]

    [KnownType(typeof(BoardLink))]
    [KnownType(typeof(BoardPageLink))]
    [KnownType(typeof(ThreadLink))]
    [KnownType(typeof(ThreadPartLink))]
    [KnownType(typeof(PostLink))]
    [KnownType(typeof(BoardMediaLink))]
    [KnownType(typeof(MediaLink))]
    [KnownType(typeof(YoutubeLink))]
    [KnownType(typeof(RootLink))]
    [KnownType(typeof(ThreadTagLink))]
    [KnownType(typeof(BoardCatalogLink))]

    [ServiceDependencyInfo(typeof(ILinkHashService))]
    [ServiceDependencyInfo(typeof(ILinkTransformService))]
    public abstract class BoardLinkBase : IDeepCloneable<BoardLinkBase>
    {
        /// <summary>
        /// Движок.
        /// </summary>
        [DataMember]
        public string Engine { get; set; }

        /// <summary>
        /// Тип ссылки.
        /// </summary>
        [IgnoreDataMember]
        public BoardLinkKind LinkKind
        {
            get { return GetLinkKind(); }
        }

        /// <summary>
        /// Получить тип ссылки.
        /// </summary>
        /// <returns>Тип ссылки.</returns>
        protected abstract BoardLinkKind GetLinkKind();

        /// <summary>
        /// Клонировать.
        /// </summary>
        /// <returns>Клон.</returns>
        public abstract BoardLinkBase DeepClone();
    }
}