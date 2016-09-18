using System;
using Akka.Actor;

namespace WinTail.Messages
{
	internal class ValidateMessage
	{
		public ValidateMessage(string input)
		{
			Input = input;
		}

		public string Input { get; private set; }
	}

	internal static class ValidateMessageExtension
	{
		public static void TellValidate(this IActorRef actor, string input)
		{
			if (actor == null)
			{
				throw new ArgumentNullException(nameof(actor));
			}

			actor.Tell(new ValidateMessage(input));
		}
	}
}