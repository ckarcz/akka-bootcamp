using System;
using Akka.Actor;
using WinTail.Extensions;

namespace WinTail.Messages
{
	internal class StartMessage { }

	internal static class StartMessageExtension
	{
		public static void TellStart(this IActorRef actor)
		{
			if (actor == null)
			{
				throw new ArgumentNullException(nameof(actor));
			}

			actor.Tell<StartMessage>();
		}
	}
}