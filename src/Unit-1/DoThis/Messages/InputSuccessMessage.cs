using System;
using Akka.Actor;

namespace WinTail.Messages
{
	internal class InputSuccessMessage
	{
		public InputSuccessMessage(string reason)
		{
			Reason = reason;
		}

		public string Reason { get; private set; }
	}

	internal static class InputSuccessMessageExtension
	{
		public static void TellInputSucess(this IActorRef actor, string reason)
		{
			if (actor == null)
			{
				throw new ArgumentNullException(nameof(actor));
			}

			actor.Tell(new InputSuccessMessage(reason));
		}
	}
}