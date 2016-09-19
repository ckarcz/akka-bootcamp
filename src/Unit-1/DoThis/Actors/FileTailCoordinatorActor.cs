using System.Collections.Generic;
using Akka.Actor;

namespace WinTail.Actors
{
	internal class FileTailCoordinatorActor : UntypedActor
	{
		private readonly IDictionary<string, IActorRef> fileTailActors = new Dictionary<string, IActorRef>();

		public static class Message
		{
			public class StartTail
			{
				public StartTail(string filePath, IActorRef reporterActor)
				{
					FilePath = filePath;
					ReporterActor = reporterActor;
				}

				public string FilePath { get; private set; }

				public IActorRef ReporterActor { get; private set; }
			}

			public class StopTail
			{
				public StopTail(string filePath)
				{
					FilePath = filePath;
				}

				public string FilePath { get; private set; }
			}
		}

		protected override void OnReceive(object message)
		{
			if (message is Message.StartTail)
			{
				OnStartFileTailMessageReceived(message as Message.StartTail);
			}
			else if (message is Message.StopTail)
			{
				OnStopFileTailMessageReceived(message as Message.StopTail);
			}
		}

		private void OnStartFileTailMessageReceived(Message.StartTail message)
		{
			if (!fileTailActors.ContainsKey(message.FilePath))
			{
				var fileTailActorProps = Props.Create(() => new FileTailActor(message.ReporterActor, message.FilePath));
				var fileTailActor = Context.ActorOf(fileTailActorProps);
				fileTailActors.Add(message.FilePath, fileTailActor);
			}
		}

		private void OnStopFileTailMessageReceived(Message.StopTail message)
		{
			if (fileTailActors.ContainsKey(message.FilePath))
			{
				var fileTailActor = fileTailActors[message.FilePath];
				Context.Stop(fileTailActor);
				fileTailActors.Remove(message.FilePath);
			}
		}
	}
}