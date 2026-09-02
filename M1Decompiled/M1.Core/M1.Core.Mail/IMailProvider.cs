using System;

namespace M1.Core.Mail;

public interface IMailProvider : IDisposable
{
	void Login(string mailServer, string password, M1UserSettings userSettings);

	string TestConnection(string mailServer, string user, string password, M1UserSettings userSettings);

	bool Send(MessageData message);

	void AttachLogo<T>(T builder, string message, int numberImages);
}
