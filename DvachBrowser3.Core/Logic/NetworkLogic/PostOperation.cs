using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using DvachBrowser3.Engines;
using DvachBrowser3.Links;
using DvachBrowser3.Other;
using DvachBrowser3.Posting;
using DvachBrowser3.Storage;

namespace DvachBrowser3.Logic.NetworkLogic
{
    /// <summary>
    /// Операция постинга.
    /// </summary>
    public class PostOperation : NetworkLogicOperation<BoardLinkBase, PostOperationParameter>
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="services">Сервисы.</param>
        /// <param name="parameter">Параметр.</param>
        public PostOperation(IServiceProvider services, PostOperationParameter parameter) : base(services, parameter)
        {
        }


        /// <summary>
        /// Выполнить операцию.
        /// </summary>
        /// <param name="token">Токен отмены операции.</param>
        /// <returns>Таск.</returns>
        public override async Task<BoardLinkBase> Complete(CancellationToken token)
        {
            List<Func<Task>> disposeActions = new List<Func<Task>>();
            try
            {
                SignalProcessing("Подготовка к отправке...", "PREPARE");
                if (Parameter.Data is DraftPostingData)
                {
                    throw new ArgumentException("Прямой постинг черновиков не поддерживается");
                }
                if (Parameter.Data.Link == null)
                {
                    throw new ArgumentException("Не указано, куда отправлять сообщение");
                }
                var engine = GetEngine(Parameter.Data.Link);
                var storage = Services.GetServiceOrThrow<IStorageService>();
                var linkTransformService = Services.GetServiceOrThrow<ILinkTransformService>();
                var linkHashService = Services.GetServiceOrThrow<ILinkHashService>();
                var mediaIds = new PostingMediaFile[0];
                var deleteData = (Parameter.Mode & PostingMode.DeleteFromStorage) != 0;
                if (Parameter.Data.FieldData.ContainsKey(PostingFieldSemanticRole.MediaFile))
                {
                    var d = Parameter.Data.FieldData[PostingFieldSemanticRole.MediaFile] as PostingMediaFiles;
                    if (d != null && d.Files != null)
                    {
                        mediaIds = d.Files.Where(f => f != null).ToArray();
                    }
                }
                var mediaFiles = new List<MediaFilePostingData>();
                foreach (var id in mediaIds)
                {
                    var file = await GetMediaTempFile(id, storage);
                    if (file != null)
                    {
                        mediaFiles.Add(file);
                        var tmpFile = file.TempFile;
                        disposeActions.Add(async () =>
                        {
                            await tmpFile.DeleteAsync();
                        });
                    }
                }
                var entryData = new PostEntryData()
                {
                    Link = Parameter.Data.Link,
                    CommonData = new Dictionary<PostingFieldSemanticRole, object>(),
                    Captcha = Parameter.Captcha,
                    MediaFiles = mediaFiles.ToArray()
                };
                foreach (var p in Parameter.Data.FieldData)
                {
                    if (p.Key != PostingFieldSemanticRole.MediaFile)
                    {
                        entryData.CommonData[p.Key] = p.Value;
                    }
                }
                var postOperation = engine.Post(entryData);
                postOperation.Progress += (sender, e) => OnProgress(e);
                var result = await postOperation.Complete(token);

                if (deleteData)
                {
                    try
                    {
                        await storage.PostData.DeletePostingData(Parameter.Data.Link);
                    }
                    catch (Exception ex)
                    {
                        DebugHelper.BreakOnError(ex);
                    }
                    foreach (var f in mediaIds)
                    {
                        try
                        {
                            await storage.PostData.MediaStorage.DeleteMediaFile(f.MediaFileId);
                        }
                        catch (Exception ex)
                        {
                            DebugHelper.BreakOnError(ex);
                        }
                    }
                }

                if (result.PostLink != null)
                {
                    if (Parameter.Data.Link.LinkKind == BoardLinkKind.Thread)
                    {
                        var threadLink = linkTransformService.ThreadLinkFromThreadPartLink(Parameter.Data.Link);
                        if (threadLink != null)
                        {
                            var myPosts = await storage.ThreadData.LoadMyPostsInfo(threadLink);
                            if (myPosts == null)
                            {
                                myPosts = new MyPostsInfo() { Link = threadLink, MyPosts = new List<BoardLinkBase>() };
                            }
                            if (myPosts.MyPosts == null)
                            {
                                myPosts.MyPosts = new List<BoardLinkBase>();
                            }
                            myPosts.MyPosts.Add(result.PostLink);
                            myPosts.MyPosts = myPosts.MyPosts.Distinct(linkHashService.GetComparer()).ToList();
                            await storage.ThreadData.SaveMyPostsInfo(myPosts);
                        }
                    }
                    return result.PostLink;
                }
                if (result.RedirectLink != null)
                {
                    return result.RedirectLink;
                }
                return null;
            }
            finally
            {
                foreach (var da in disposeActions)
                {
                    try
                    {
                        await da();
                    }
                    catch (Exception ex)
                    {
                        DebugHelper.BreakOnError(ex);
                    }
                }
            }
        }

        private async Task<MediaFilePostingData> GetMediaTempFile(PostingMediaFile media, IStorageService storage)
        {
            var file = await storage.PostData.MediaStorage.GetMediaFile(media.MediaFileId);
            if (file != null)
            {
                var tempId = Guid.NewGuid();
                var tempFile = await ApplicationData.Current.TemporaryFolder.CreateFileAsync(tempId + ".tmp", CreationCollisionOption.GenerateUniqueName);
                try
                {
                    var originalName = media.OriginalName;
                    var ext = Path.GetExtension(originalName);
                    bool isSized = false;
                    if (media.Resize && !".webm".Equals(ext, StringComparison.OrdinalIgnoreCase))
                    {
                        using (var src = await file.Value.File.OpenReadAsync())
                        {
                            var decoder = await BitmapDecoder.CreateAsync(src);
                            var sz = new Size(decoder.OrientedPixelWidth, decoder.OrientedPixelHeight);
                            if (sz.Width > 800 || sz.Height > 800)
                            {
                                var sz2 = sz.ScaleTo(new Size(800, 800));
                                var newH = (uint)sz2.Height;
                                var newW = (uint)sz2.Width;
                                if (newH < 1) newH = 1;
                                if (newW < 1) newW = 1;
                                if (newH > 800) newH = 800;
                                if (newW > 800) newW = 800;
                                var transform = new BitmapTransform() { ScaledHeight = newH, ScaledWidth = newW };
                                var pixelData = await decoder.GetPixelDataAsync(BitmapPixelFormat.Rgba8, BitmapAlphaMode.Straight, transform, ExifOrientationMode.RespectExifOrientation, ColorManagementMode.DoNotColorManage);
                                using (var outStr = await tempFile.OpenAsync(FileAccessMode.ReadWrite))
                                {
                                    var encoder = await BitmapEncoder.CreateAsync(decoder.DecoderInformation.CodecId, outStr);
                                    encoder.SetPixelData(BitmapPixelFormat.Rgba8, BitmapAlphaMode.Premultiplied, newW, newH, 96, 96, pixelData.DetachPixelData());
                                    await encoder.FlushAsync();
                                    isSized = true;
                                }
                            }
                        }
                    }
                    if (!isSized)
                    {
                        await file.Value.File.CopyAndReplaceAsync(tempFile);
                    }
                    if (media.AddUniqueId)
                    {
                        using (var outFile = await tempFile.OpenStreamForWriteAsync())
                        {
                            outFile.Seek(0, SeekOrigin.End);
                            var uid = Guid.NewGuid().ToByteArray();
                            await outFile.WriteAsync(uid, 0, uid.Length);
                        }
                    }
                    return new MediaFilePostingData()
                    {
                        TempFile = tempFile,
                        FileName = media.OriginalName
                    };
                }
                catch
                {
                    try
                    {
                        await tempFile.DeleteAsync();
                    }
                    catch (Exception ex)
                    {
                        DebugHelper.BreakOnError(ex);
                    }
                    throw;
                }
            }
            return null;
        }
    }
}