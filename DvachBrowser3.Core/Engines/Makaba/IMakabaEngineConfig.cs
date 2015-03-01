﻿using System;
using System.Collections.Generic;
using System.Linq;
using DvachBrowser3.Posting;

namespace DvachBrowser3.Engines.Makaba
{
    /// <summary>
    /// Конфигурация движка Makaba.
    /// </summary>
    public interface IMakabaEngineConfig : IConfiguration
    {
        /// <summary>
        /// Базовый URI.
        /// </summary>
        Uri BaseUri { get; set; }

        /// <summary>
        /// Куки.
        /// </summary>
        Dictionary<string, string> Cookies { get; set; }

        /// <summary>
        /// Тип капчи.
        /// </summary>
        CaptchaType CaptchaType { get; set; }
    }
}