using System;
using Akka.Actor;
using WinTail.Messages;

namespace WinTail.Actors
{
	internal class ConsoleWriterActor : UntypedActor
	{
		protected override void OnReceive(object message)
		{
			if (message is InputSuccessMessage)
			{
				OnInputSuccessMessageReceieved(message as InputSuccessMessage);
			}
			else if (message is InputErrorMessage)
			{
				OnInputErrorMessageReceived(message as InputErrorMessage);
			}
			else if (message is WriteToConsoleMessage)
			{
				OnWriteToConsoleMessageReceived(message as WriteToConsoleMessage);
			}
			else if (message is TerminateMessage)
			{
				OnTerminateMessageReceived();
			}
		}

		private void OnInputSuccessMessageReceieved(InputSuccessMessage message)
		{
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine(message.Reason);
			Console.ResetColor();
		}

		private void OnInputErrorMessageReceived(InputErrorMessage message)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine(message.Reason);
			Console.ResetColor();
		}

		private void OnWriteToConsoleMessageReceived(WriteToConsoleMessage message)
		{
			Console.WriteLine(message.Message);
		}

		private void OnTerminateMessageReceived()
		{
			Context.System.Terminate();
		}
	}
}