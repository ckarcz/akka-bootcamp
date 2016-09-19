using System;
using Akka.Actor;
using WinTail.Actors;
using WinTail.Messages;

namespace WinTail
{
	internal class Program
	{
		public static ActorSystem MyActorSystem;

		static void Main(string[] args)
		{
			// create actor system
			MyActorSystem = ActorSystem.Create("MyActorSystem");

			// create actors
			var consoleWriterActorProps = Props.Create<ConsoleWriterActor>();
			var consoleWriterActor = MyActorSystem.ActorOf(consoleWriterActorProps, "ConsoleWriterActor");
			var validationActorProps = Props.Create<ValidationActor>(consoleWriterActor);
			var validationActor = MyActorSystem.ActorOf(validationActorProps, "ValidationActor");
			//var consoleReaderActorProps = Props.Create<ConsoleReaderActor>(consoleWriterActor, validationActor);
			//var consoleReaderActor = MyActorSystem.ActorOf(consoleReaderActorProps, "ConsoleReaderActor");
			var fileTailCoordinatorActorProps = Props.Create(() => new FileTailCoordinatorActor());
			var fileTailCoordinatorActor = MyActorSystem.ActorOf(fileTailCoordinatorActorProps, "FileTailCoordinatorActor");

			// start certain actors
			//consoleReaderActor.TellStart();
			fileTailCoordinatorActor.Tell(new FileTailCoordinatorActor.Message.StartTail("sample_log_file.txt", consoleWriterActor));

			// blocks the main thread from exiting until the actor system is shut down
			MyActorSystem.WhenTerminated.Wait();
		}
	}
}