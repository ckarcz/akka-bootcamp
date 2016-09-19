using System;
using System.IO;
using System.Text;
using Akka.Actor;

namespace WinTail.Actors
{

	public class FileTailActor : UntypedActor
	{
		private readonly IActorRef _reporterActor;
		private FileObserver _observer;
		private Stream _fileStream;
		private StreamReader _fileStreamReader;

		private static class Message
		{
			public class FileWrite
			{
				public FileWrite(string fileName)
				{
					FileName = fileName;
				}

				public string FileName { get; private set; }
			}

			public class FileError
			{
				public FileError(string fileName, string reason)
				{
					FileName = fileName;
					Reason = reason;
				}

				public string FileName { get; private set; }

				public string Reason { get; private set; }
			}

			public class InitialRead
			{
				public InitialRead(string fileName, string text)
				{
					FileName = fileName;
					Text = text;
				}

				public string FileName { get; private set; }
				public string Text { get; private set; }
			}
		}

		public FileTailActor(IActorRef reporterActor, string filePath)
		{
			_reporterActor = reporterActor;
			FilePath = filePath;
		}

		public string FilePath { get; private set; }

		protected override void PreStart()
		{
			_observer = new FileObserver(Self, Path.GetFullPath(FilePath));
			_observer.Start();

			_fileStream = new FileStream(Path.GetFullPath(FilePath), FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
			_fileStreamReader = new StreamReader(_fileStream, Encoding.UTF8);

			var text = _fileStreamReader.ReadToEnd();
			Self.Tell(new Message.InitialRead(FilePath, text));

			base.PreStart();
		}

		protected override void PostStop()
		{
			_observer.Dispose();
			_observer = null;

			_fileStreamReader.Close();
			_fileStreamReader.Dispose();
			_fileStreamReader = null;

			_fileStream.Close();
			_fileStream.Dispose();
			_fileStream = null;

			base.PostStop();
		}

		protected override void OnReceive(object message)
		{
			if (message is Message.FileWrite)
			{
				var text = _fileStreamReader.ReadToEnd();
				if (!string.IsNullOrEmpty(text))
				{
					_reporterActor.Tell(text);
				}

			}
			else if (message is Message.FileError)
			{
				var fe = message as Message.FileError;
				_reporterActor.Tell(string.Format("Tail error: {0}", fe.Reason));
			}
			else if (message is Message.InitialRead)
			{
				var ir = message as Message.InitialRead;
				_reporterActor.Tell(ir.Text);
			}
		}

		private class FileObserver : IDisposable
		{
			private readonly IActorRef _tailActor;
			private readonly string _absoluteFilePath;
			private FileSystemWatcher _watcher;
			private readonly string _fileDir;
			private readonly string _fileNameOnly;

			public FileObserver(IActorRef tailActor, string absoluteFilePath)
			{
				_tailActor = tailActor;
				_absoluteFilePath = absoluteFilePath;
				_fileDir = Path.GetDirectoryName(absoluteFilePath);
				_fileNameOnly = Path.GetFileName(absoluteFilePath);
			}

			public void Start()
			{
				_watcher = new FileSystemWatcher(_fileDir, _fileNameOnly);

				_watcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite;

				_watcher.Changed += OnFileChanged;
				_watcher.Error += OnFileError;

				_watcher.EnableRaisingEvents = true;
			}

			public void Dispose()
			{
				_watcher.Dispose();
			}

			private void OnFileError(object sender, ErrorEventArgs e)
			{
				_tailActor.Tell(new Message.FileError(_fileNameOnly, e.GetException().Message), ActorRefs.NoSender);
			}

			private void OnFileChanged(object sender, FileSystemEventArgs e)
			{
				if (e.ChangeType == WatcherChangeTypes.Changed)
				{
					_tailActor.Tell(new Message.FileWrite(e.Name), ActorRefs.NoSender);
				}
			}
		}
	}
}