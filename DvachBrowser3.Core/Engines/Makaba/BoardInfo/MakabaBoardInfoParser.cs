﻿using System;
using System.Collections.Generic;
using System.Linq;
using DvachBrowser3.Board;
using DvachBrowser3.Engines.Makaba.Json;
using DvachBrowser3.Links;
using DvachBrowser3.Makaba;
using DvachBrowser3.Posting;

namespace DvachBrowser3.Engines.Makaba.BoardInfo
{
    /// <summary>
    /// Парсер информации о борде.
    /// </summary>
    public sealed class MakabaBoardInfoParser : ServiceBase, IMakabaBoardInfoParser
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="services">Сервисы.</param>
        public MakabaBoardInfoParser(IServiceProvider services) : base(services)
        {
        }

        private static readonly PostingFieldSemanticRole[] CommonRoles = new PostingFieldSemanticRole[]
        {
            PostingFieldSemanticRole.Captcha, 
            PostingFieldSemanticRole.Comment, 
            PostingFieldSemanticRole.Email, 
            PostingFieldSemanticRole.Icon,
            PostingFieldSemanticRole.MediaFile, 
            PostingFieldSemanticRole.OpFlag, 
            PostingFieldSemanticRole.PosterName, 
            PostingFieldSemanticRole.PosterTrip, 
            PostingFieldSemanticRole.SageFlag, 
            PostingFieldSemanticRole.Title, 
            PostingFieldSemanticRole.WatermarkFlag,            
        };

        /// <summary>
        /// Парсить информацию о борде.
        /// </summary>
        /// <param name="category">Категория.</param>
        /// <param name="b">Информация.</param>
        /// <returns>Ссылка на борду.</returns>
        public BoardReference Parse(string category, MobileBoardInfo b)
        {
            var board = new BoardReference()
            {
                Category = category,
                DisplayName = b.Name,
                Link = new BoardLink() {Engine = CoreConstants.Engine.Makaba, Board = b.Id},
                ShortName = b.Id,
                Extensions = new List<BoardReferenceExtension>(),
                IsAdult = "Взрослым".Equals(category) || CoreConstants.Makaba.AdultBoards.Contains(b.Id)
            };
            var makabaExtension = new MakabaBoardReferenceExtension()
            {
                Bumplimit = b.BumpLimit,
                DefaultName = b.DefaultName,
                Icons = (b.Icons ?? new BoardIcon2[0]).Select(i => new MakabaIconReference()
                {
                    Name = i.Name,
                    Number = i.NumberInt,
                    Url = i.Url
                }).ToList(),
                Pages = b.Pages,
                MaxComment = null,
                Sage = b.Sage != 0,
                Tripcodes = b.Tripcodes != 0
            };
            board.Extensions.Add(makabaExtension);
            var postingExtension = new BoardReferencePostingExtension()
            {
                Capabilities = new List<PostingCapability>()
            };
            foreach (var cr in CommonRoles)
            {
                switch (cr)
                {
                    case PostingFieldSemanticRole.Comment:
                        postingExtension.Capabilities.Add(new PostingCommentCapability()
                        {
                            MaxLength = null,
                            MarkupType = PostingMarkupType.Makaba,
                            Role = PostingFieldSemanticRole.Comment
                        });
                        break;
                    case PostingFieldSemanticRole.Captcha:
                        postingExtension.Capabilities.Add(new PostingCaptchaCapability()
                        {
                            CaptchaTypes = CaptchaTypes.Recaptcha | CaptchaTypes.Yandex
                        });
                        break;
                    case PostingFieldSemanticRole.Icon:
                        if (makabaExtension.Icons.Count > 0)
                        {
                            postingExtension.Capabilities.Add(new PostingIconCapability()
                            {
                                Icons = makabaExtension.Icons.Select(i => new PostingCapabilityIcon()
                                {
                                    Name = i.Name,
                                    Number = i.Number
                                }).ToList(),
                                Role = PostingFieldSemanticRole.MediaFile
                            });
                        }
                        break;
                    case PostingFieldSemanticRole.MediaFile:
                        postingExtension.Capabilities.Add(new PostingMediaFileCapability()
                        {
                            MaxFileCount = 4,
                            Role = PostingFieldSemanticRole.MediaFile
                        });
                        break;
                    /*
                    case PostingFieldSemanticRole.SageFlag:
                        if (makabaExtension.Sage)
                        {
                            postingExtension.Capabilities.Add(new PostingCapability()
                            {
                                Role = PostingFieldSemanticRole.SageFlag
                            });
                        }
                        break;*/
                    default:
                        postingExtension.Capabilities.Add(new PostingCapability()
                        {
                            Role = cr
                        });
                        break;
                }
            }
            board.Extensions.Add(postingExtension);
            return board;
        }

        public BoardReference Default(string category, string boardId)
        {
            return Parse(category, new MobileBoardInfo()
            {
                BumpLimit = 500,
                Category = category,
                DefaultName = "Аноним",
                EnablePosting = true,
                Icons = new BoardIcon2[0],
                Id = boardId,
                Name = "/" + boardId + "/",
                Pages = 5,
                Sage = 1,
                Tripcodes = 1
            });
        }
    }
}