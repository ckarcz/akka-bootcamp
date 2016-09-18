using System;
using Akka.Actor;
using WinTail.Extensions;

namespace WinTail.Messages
{
	internal class ContinueMessage { }

	internal static class ContinueMessageExtension
	{
		public static void TellContinue(this IActorRef actor)
		{
			if (actor == null)
			{
				throw new ArgumentNullException(nameof(actor));
			}

			actor.Tell<ContinueMessage>();
		}
	}
}