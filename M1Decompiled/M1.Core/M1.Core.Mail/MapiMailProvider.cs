using System;

namespace M1.Core.Mail;

public class MapiMailProvider : IMailProvider, IDisposable
{
	public void Login(string mailServer, string password, M1UserSettings userSettings)
	{
	}

	public bool Send(MessageData message)
	{
		new MapiMail().SendMAPI(message, showDialog: false);
		return true;
	}

	public string TestConnection(string mailServer, string user, string password, M1UserSettings userSettings)
	{
		return string.Empty;
	}

	public void Dispose()
	{
	}

	public void AttachLogo<T>(T builder, string message, int numberImages)
	{
	}
}
