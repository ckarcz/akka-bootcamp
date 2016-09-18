using System;
using Akka.Actor;

namespace WinTail.Messages
{
	internal class InputErrorMessage
	{
		public InputErrorMessage(string reason)
		{
			Reason = reason;
		}

		public string Reason { get; private set; }
	}

	internal static class InputErrorMessageExtension
	{
		public static void TellInputError(this IActorRef actor, string reason)
		{
			if (actor == null)
			{
				throw new ArgumentNullException(nameof(actor));
			}

			actor.Tell(new InputErrorMessage(reason));
		}
	}
}