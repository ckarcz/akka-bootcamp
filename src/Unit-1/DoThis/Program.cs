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
			var consoleWriterActorProps = Props.Create(() => new ConsoleWriterActor());
			var consoleWriterActor = MyActorSystem.ActorOf(consoleWriterActorProps);
			var consoleReaderActorProps = Props.Create(() => new ConsoleReaderActor(consoleWriterActor));
			var consoleReaderActor = MyActorSystem.ActorOf(consoleReaderActorProps);

			// tell console reader to begin
			consoleReaderActor.TellStart();

			// blocks the main thread from exiting until the actor system is shut down
			MyActorSystem.WhenTerminated.Wait();
		}
	}
}
