using System;
using System.IO;
using System.Threading;

namespace Layoutize.Tests
{
    public sealed class WorkingDirectoryFixture : IDisposable
    {
        public void Dispose()
        {
            if (WorkingDirectory.Exists)
            {
                WorkingDirectory.Delete(true);
            }
        }

        internal DirectoryInfo GetNewWorkingDirectory()
        {
            return WorkingDirectory.CreateSubdirectory(Interlocked.Increment(ref _id).ToString());
        }

        private readonly DirectoryInfo WorkingDirectory = Directory.CreateDirectory(Path.Combine(Path.GetTempPath(), "LayoutizeTests"));

        private int _id;
    }
}
