namespace M1.Core.Mail;

public class GmailMailProvider : SmtpMailProvider
{
	public override void Login(string mailServer, string password, M1UserSettings userSettings)
	{
		base.Login("smtp.gmail.com", password, userSettings);
	}

	public override string TestConnection(string mailServer, string user, string password, M1UserSettings userSettings)
	{
		return base.TestConnection("smtp.gmail.com", user, password, userSettings);
	}
}
