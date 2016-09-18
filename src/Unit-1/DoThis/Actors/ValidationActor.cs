using System;
using Akka.Actor;
using WinTail.Messages;

namespace WinTail.Actors
{
	public class ValidationActor : UntypedActor
	{
		private readonly IActorRef _consoleWriterActor;

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
			if (message is ValidateMessage)
			{
				OnValidateMessageReceived(message as ValidateMessage);
			}
		}

		private void OnValidateMessageReceived(ValidateMessage message)
		{
			if (string.IsNullOrEmpty(message.Input))
			{
				_consoleWriterActor.TellNullInputError("Input is null.");
			}
			else if (IsValid(message.Input))
			{
				_consoleWriterActor.TellInputSucess("Input is valid.");
			}
			else
			{
				_consoleWriterActor.TellValidationError("Input is invalid.");
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