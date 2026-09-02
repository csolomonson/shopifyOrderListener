using System;

namespace M1.Core;

public class EmailMessageSentEventArgs : EventArgs
{
	public MessageData Message;

	public M1Database Database;

	public EmailMessageSentEventArgs(M1Database database, MessageData message)
	{
		Message = message;
		Database = database;
	}
}
