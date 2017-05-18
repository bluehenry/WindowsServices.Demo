using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Topshelf.Logging;

namespace FileConverterService
{
    class ConverterService
    {
        private FileSystemWatcher _watcher;
        private static readonly LogWriter _log = HostLogger.Get<ConverterService>();
        public bool Start()
        {
            string path = @"c:\temp\a";
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                _watcher = new FileSystemWatcher(path, "*_in.txt");

                _watcher.Created += FileCreated;

                _watcher.IncludeSubdirectories = false;

                _watcher.EnableRaisingEvents = true;

            }
            catch (Exception exception)
            {
                _log.InfoFormat(exception.ToString());
            }


            return true;
        }

        private void FileCreated(object sender, FileSystemEventArgs e)
        {
            try
            {
                // Sleep 5 seconds, otherwise get System.IO.IOException: The process cannot access the file ... because it is being used by another process
                Thread.Sleep(5000);

                _log.InfoFormat("Starting conversion of '{0}'", e.FullPath);

                if (e.FullPath.Contains("bad_in"))
                {
                    throw new NotSupportedException("Cannot convert");
                }

                string content = File.ReadAllText(e.FullPath);

                string upperContent = content.ToUpperInvariant();

                var dir = Path.GetDirectoryName(e.FullPath);

                var convertedFileName = Path.GetFileName(e.FullPath) + ".converted";

                var convertedPath = Path.Combine(dir, convertedFileName);
                                

                File.WriteAllText(convertedPath, upperContent);
            }
            catch (Exception exception)
            {
                _log.InfoFormat(exception.ToString());
            }
        }

        public bool Stop()
        {
            _watcher.Dispose();

            return true;
        }

        public bool Pause()
        {
            _watcher.EnableRaisingEvents = false;
            return true;
        }

        public bool Continue()
        {
            _watcher.EnableRaisingEvents = true;
            return true;
        }

        public void CustomCommand(int commandNumber)
        {
            _log.InfoFormat("Got the command number '{0}'", commandNumber);
        }
    }
}
