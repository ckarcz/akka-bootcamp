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
			var consoleWriterActor = MyActorSystem.ActorOf(Props.Create(() => new ConsoleWriterActor()), "ConsoleWriterActor");
			var validationAction = MyActorSystem.ActorOf(Props.Create(() => new ValidationActor(consoleWriterActor)), "ValidationActor");
			var consoleReaderActor = MyActorSystem.ActorOf(Props.Create(() => new ConsoleReaderActor(consoleWriterActor, validationAction)), "ConsoleReaderActor");

			// tell console reader to begin
			consoleReaderActor.TellStart();

			// blocks the main thread from exiting until the actor system is shut down
			MyActorSystem.WhenTerminated.Wait();
		}
	}
}