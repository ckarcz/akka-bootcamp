using System;
using Akka.Actor;

namespace WinTail.Messages
{
	internal class ValidationErrorMessage : InputErrorMessage
	{
		public ValidationErrorMessage(string reason) : base(reason) { }
	}

	internal static class ValidationErrorMessageExtension
	{
		public static void TellValidationError(this IActorRef actor, string reason)
		{
			if (actor == null)
			{
				throw new ArgumentNullException(nameof(actor));
			}

			actor.Tell(new ValidationErrorMessage(reason));
		}
	}
}