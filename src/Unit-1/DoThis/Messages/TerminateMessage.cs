using System;
using Akka.Actor;
using WinTail.Extensions;

namespace WinTail.Messages
{
	internal class TerminateMessage { }

	internal static class TerminateMessageExtensions
	{
		public static void TellTerminate(this IActorRef actor)
		{
			if (actor == null)
			{
				throw new ArgumentNullException(nameof(actor));
			}

			actor.Tell<TerminateMessage>();
		}
	}
}