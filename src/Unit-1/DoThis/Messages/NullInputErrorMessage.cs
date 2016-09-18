using System;
using Akka.Actor;

namespace WinTail.Messages
{
	internal class NullInputErrorMessage : InputErrorMessage
	{
		public NullInputErrorMessage(string reason)
			: base(reason) { }
	}

	internal static class NullInputErrorMessageExtension
	{
		public static void TellNullInputError(this IActorRef actor, string reason)
		{
			if (actor == null)
			{
				throw new ArgumentNullException(nameof(actor));
			}

			actor.Tell(new NullInputErrorMessage(reason));
		}
	}
}