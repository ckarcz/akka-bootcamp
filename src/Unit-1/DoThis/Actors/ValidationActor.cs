using System;
using Akka.Actor;
using WinTail.Messages;

namespace WinTail.Actors
{
	internal class ValidationActor : UntypedActor
	{
		private readonly IActorRef _consoleWriterActor;

		public static class Message
		{
			public class ValidateMessage
			{
				public ValidateMessage(string input)
				{
					Input = input;
				}

				public string Input { get; private set; }
			}

			public class ValidationResultMessage
			{
				public ValidationResultMessage(bool valid, string reason)
				{
					Reason = reason;
					Valid = valid;
				}

				public bool Valid { get; set; }
				public string Reason { get; set; }
			}
		}

		public ValidationActor(IActorRef consoleWriterActor)
		{
			if (consoleWriterActor == null)
			{
				throw new ArgumentNullException(nameof(consoleWriterActor));
			}

			_consoleWriterActor = consoleWriterActor;
		}

		protected override void OnReceive(object message)
		{
			if (message is Message.ValidateMessage)
			{
				OnValidateMessageReceived(message as Message.ValidateMessage);
			}
		}

		private void OnValidateMessageReceived(Message.ValidateMessage message)
		{
			if (string.IsNullOrEmpty(message.Input))
			{
				_consoleWriterActor.Tell(new Message.ValidationResultMessage(false, "Input is null."));
			}
			else if (IsValid(message.Input))
			{
				_consoleWriterActor.Tell(new Message.ValidationResultMessage(true, "Input is valid."));
			}
			else
			{
				_consoleWriterActor.Tell(new Message.ValidationResultMessage(false, "Input is invalid."));
			}

			Sender.TellContinue();
		}

		private static bool IsValid(string input)
		{
			var valid = input.Length % 2 == 0;
			return valid;
		}
	}
}