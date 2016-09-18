using System;
using Akka.Actor;

namespace WinTail.Extensions
{
	internal static class AkkaTellExtensions
	{
		public static void Tell<T>(this IActorRef actor) where T : new()
		{
			if (actor == null)
			{
				throw new ArgumentNullException(nameof(actor));
			}

			actor.Tell(new T());
		}
	}
}