using System;
using Akka.Actor;
using WinTail.Messages;

namespace WinTail.Actors
{
	internal class ConsoleWriterActor : UntypedActor
	{
		protected override void OnReceive(object message)
		{
			if (message is ValidationActor.Message.ValidationResultMessage)
			{
				OnValidationResultMessageRecieved(message as ValidationActor.Message.ValidationResultMessage);
			}
			else if (message is TerminateMessage)
			{
				OnTerminateMessageReceived();
			}
			else
			{
				WriteToConsole(message);
			}
		}

		private void OnValidationResultMessageRecieved(ValidationActor.Message.ValidationResultMessage message)
		{
			if (message.Valid)
			{
				Console.ForegroundColor = ConsoleColor.Green;
				Console.WriteLine(message.Reason);
				Console.ResetColor();
			}
			else
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine(message.Reason);
				Console.ResetColor();
			}
		}

		private void WriteToConsole(object message)
		{
			Console.WriteLine(message);
		}

		private void OnTerminateMessageReceived()
		{
			Context.System.Terminate();
		}
	}
}