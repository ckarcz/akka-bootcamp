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

		public ConsoleReaderActor(IActorRef consoleWriterActor)
		{
			if (consoleWriterActor == null)
			{
				throw new ArgumentNullException(nameof(consoleWriterActor));
			}

			_consoleWriterActor = consoleWriterActor;
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
			_consoleWriterActor.TellWriteToConsole("Terminating...");
			Context.System.Terminate();
		}

		private void OnContinueMessageReceived()
		{
			var userInput = Console.ReadLine();
			if (String.Equals(userInput, ExitCommand, StringComparison.OrdinalIgnoreCase))
			{
				Self.TellTerminate();
				return;
			}
			else if (String.IsNullOrEmpty(userInput))
			{
				_consoleWriterActor.TellNullInputError("Input is null.");
			}
			else if (IsValid(userInput))
			{
				_consoleWriterActor.TellInputSucess("Input is valid.");
			}
			else
			{
				_consoleWriterActor.TellValidationError("Input is invalid.");
			}

			Self.TellContinue();
		}

		private static bool IsValid(string message)
		{
			var valid = message.Length % 2 == 0;
			return valid;
		}
	}
}