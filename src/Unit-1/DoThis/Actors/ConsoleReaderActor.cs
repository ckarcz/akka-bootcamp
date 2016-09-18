using System;
using System.Text;
using Akka.Actor;
using WinTail.Messages;

namespace WinTail.Actors
{
	internal class ConsoleReaderActor : UntypedActor
	{
		private static readonly string ExitCommand = "exit";

		private IActorRef _consoleWriterActor;
		private IActorRef _validationActor;

		public ConsoleReaderActor(IActorRef consoleWriterActor, IActorRef validationActor)
		{
			if (consoleWriterActor == null)
			{
				throw new ArgumentNullException(nameof(consoleWriterActor));
			}
			if (validationActor == null)
			{
				throw new ArgumentNullException(nameof(validationActor));
			}

			_consoleWriterActor = consoleWriterActor;
			_validationActor = validationActor;
		}

		protected override void OnReceive(object message)
		{
			if (message is StartMessage)
			{
				OnStartMessageReceived();
			}
			else if (message is TerminateMessage)
			{
				OnTerminateMessageReceived();
			}
			else if (message is ContinueMessage)
			{
				OnContinueMessageReceived();
			}
		}

		private void OnStartMessageReceived()
		{
			var stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Write whatever you want into the console!");
			stringBuilder.AppendLine("Some entries will pass validation, and some won't...\n\n");
			stringBuilder.AppendLine("Type 'exit' to quit this application at any time.\n");
			_consoleWriterActor.TellWriteToConsole(stringBuilder.ToString());

			Self.TellContinue();
		}

		private void OnTerminateMessageReceived()
		{
			_validationActor.TellWriteToConsole("Terminating...");
			Context.System.Terminate();
		}

		private void OnContinueMessageReceived()
		{
			var userInput = Console.ReadLine();
			if (string.Equals(userInput, ExitCommand, StringComparison.OrdinalIgnoreCase))
			{
				Self.TellTerminate();
				return;
			}
			else
			{
				_validationActor.TellValidate(userInput);
			}
		}
	}
}