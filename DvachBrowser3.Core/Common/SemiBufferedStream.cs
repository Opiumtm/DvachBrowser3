using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;

namespace DvachBrowser3
{
    /// <summary>
    /// Частично буферизованный поток.
    /// </summary>
    public sealed class SemiBufferedStream : Stream
    {
        private readonly long maxSize;

        private Stream currentStream;

        private StorageFile tempFile;

        private IRandomAccessStream rtStream;

        public SemiBufferedStream(long maxSize)
        {
            currentStream = new MemoryStream();
            this.maxSize = maxSize;
        }

        public override void Flush()
        {
            currentStream.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return currentStream.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return currentStream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            currentStream.SetLength(value);
            CheckStreamSize();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            currentStream.Write(buffer, offset, count);
            CheckStreamSize();
        }

        public override bool CanRead
        {
            get { return currentStream.CanRead; }
        }

        public override bool CanSeek
        {
            get { return currentStream.CanSeek; }
        }

        public override bool CanWrite
        {
            get { return currentStream.CanWrite; }
        }

        public override long Length
        {
            get { return currentStream.Length; }
        }

        private void CheckStreamSize()
        {
            if (tempFile != null)
            {
                return;
            }
            if (currentStream.Length > maxSize)
            {
                var tf = CreateTempFile();
                tf.Wait();
            }
        }

        private async Task CreateTempFile()
        {
            tempFile = await ApplicationData.Current.TemporaryFolder.CreateFileAsync(Guid.NewGuid() + ".tmp", CreationCollisionOption.GenerateUniqueName);
            rtStream = await tempFile.OpenAsync(FileAccessMode.ReadWrite);
            var newStream = rtStream.AsStream();
            var position = currentStream.Position;
            currentStream.Position = 0;
            await currentStream.CopyToAsync(newStream);
            newStream.Position = position;
            currentStream.Dispose();
            currentStream = newStream;
        }

        public override long Position
        {
            get { return currentStream.Position; }
            set { currentStream.Position = value; }
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (rtStream != null)
                {
                    rtStream.Dispose();
                }
                currentStream.Dispose();
                if (tempFile != null)
                {
                    var tmpName = tempFile.Name;
                    tempFile = null;
                    var task = Task.Factory.StartNew(new Action(async () =>
                    {
                        try
                        {
                            var tempFile2 = await ApplicationData.Current.TemporaryFolder.GetFileAsync(tmpName);
                            await tempFile2.DeleteAsync();
                        }
                        catch (Exception ex)
                        {
                            DebugHelper.BreakOnError(ex);
                        }
                    }));
                    task.Wait();
                }                
            }
            base.Dispose(disposing);
        }
    }
}