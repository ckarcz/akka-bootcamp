using System;
using Akka.Actor;
using WinTail.Extensions;

namespace WinTail.Messages
{
	internal class WriteToConsoleMessage
	{
		public WriteToConsoleMessage(string message)
		{
			Message = message;
		}

		public string Message { get; private set; }
	}

	internal static class WriteToConsoleMessageExtension
	{
		public static void TellWriteToConsole(this IActorRef actor, string message)
		{
			if (actor == null)
			{
				throw new ArgumentNullException(nameof(actor));
			}

			actor.Tell(new WriteToConsoleMessage(message));
		}
	}
}