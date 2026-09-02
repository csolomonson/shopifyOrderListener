using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Windows.Forms;
using M1.Core.MailHelper;
using Microsoft.Exchange.WebServices.Data;
using Microsoft.Graph;
using Microsoft.Identity.Client;

namespace M1.Core.Mail;

public class ExchangeOnlineMailProvider : IMailProvider, IDisposable, IMailGetMatchingNames
{
	public delegate void AttachLogoToEmail<T>(T builder, string message, int numberImages);

	private ExchangeService _service;

	private IPublicClientApplication _publicClientAplApplication;

	private readonly string[] _ewsScopes = new string[2] { "https://outlook.office.com/EWS.AccessAsUser.All", "Mail.Send" };

	private const string ClientId = "30c5e4e1-06ff-44bb-9f50-a4c4255edd25";

	private const string Tenant = "common";

	private IGraphServiceClient _graphClient;

	public ExchangeOnlineMailProvider()
	{
		ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
	}

	public async System.Threading.Tasks.Task AuthenticateOnEws(M1UserSettings userSettings)
	{
		try
		{
			GeneratePublicClientApplication(userSettings);
			SaveAuthenticationResultOnUserSettings(await _publicClientAplApplication.AcquireTokenInteractive(_ewsScopes).ExecuteAsync(), userSettings);
		}
		catch (MsalException ex)
		{
			if (ex.ErrorCode != "authentication_canceled" && ex.ErrorCode != "access_denied")
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}
	}

	public void Dispose()
	{
		_service = null;
	}

	public void Login(string mailServer, string password, M1UserSettings userSettings)
	{
		SilentAuthenticationOnExchangeOnlineServer(userSettings);
	}

	private void SilentAuthenticationOnExchangeOnlineServer(M1UserSettings userSettings)
	{
		try
		{
			GeneratePublicClientApplication(userSettings);
			string accountIdentifier = userSettings.AccountIdentifier;
			Task<IAccount> accountTask = System.Threading.Tasks.Task.Run(() => _publicClientAplApplication.GetAccountAsync(accountIdentifier));
			accountTask.Wait();
			Task<AuthenticationResult> task = System.Threading.Tasks.Task.Run(() => _publicClientAplApplication.AcquireTokenSilent(_ewsScopes, accountTask.Result).ExecuteAsync());
			task.Wait();
			SaveAuthenticationResultOnUserSettings(task.Result, userSettings);
			GenerateExchangeService(userSettings.PrivateToken);
		}
		catch (AggregateException ex)
		{
			if ((ex.InnerException as MsalException)?.ErrorCode == "invalid_grant" || (ex.InnerException as MsalException)?.ErrorCode == "user_null")
			{
				throw new M1OnlineAuthenticationException("Authentication error, please sign in with your account in user options before trying again.");
			}
		}
	}

	public ExchangeService GetExchangeOnlineService(M1UserSettings userSettings)
	{
		SilentAuthenticationOnExchangeOnlineServer(userSettings);
		return _service;
	}

	public IGraphServiceClient GetAuthenticatedClientForApp(M1UserSettings userSettings)
	{
		try
		{
			return new GraphServiceClient("https://graph.microsoft.com/v1.0", new DelegateAuthenticationProvider(async delegate(HttpRequestMessage requestMessage)
			{
				string privateToken = userSettings.PrivateToken;
				requestMessage.Headers.Authorization = new AuthenticationHeaderValue("bearer", privateToken);
			}));
		}
		catch (Exception)
		{
		}
		return null;
	}

	public bool Logout(M1UserSettings userSettings)
	{
		GeneratePublicClientApplication(userSettings);
		string accountIdentifier = userSettings.AccountIdentifier;
		Task<IAccount> task = System.Threading.Tasks.Task.Run(() => _publicClientAplApplication.GetAccountAsync(accountIdentifier));
		task.Wait();
		IAccount account = task.Result;
		if (account != null)
		{
			try
			{
				System.Threading.Tasks.Task.Run(() => _publicClientAplApplication.RemoveAsync(account)).Wait();
				CleanAuthenticationUserSettings(userSettings);
				MessageBox.Show("User has been signed out successfully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				return true;
			}
			catch (MsalException ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return false;
			}
		}
		MessageBox.Show("No accounts signed in", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		return false;
	}

	public string TestConnection(string mailServer, string user, string password, M1UserSettings userSettings)
	{
		SilentAuthenticationOnExchangeOnlineServer(userSettings);
		try
		{
			Microsoft.Exchange.WebServices.Data.Folder.Bind(_service, WellKnownFolderName.Inbox, new PropertySet());
		}
		catch (Exception ex)
		{
			return ex.Message;
		}
		return string.Empty;
	}

	public bool Send(MessageData message)
	{
		if (!string.IsNullOrWhiteSpace(message.From))
		{
			BuildMailMessage(message, _service).SendAndSaveCopy();
			return true;
		}
		throw new M1MissingOrInvalidDataException("Originating email address was not set.\nCheck Email settings under User Options.");
	}

	public void AttachLogo<T>(T builder, string message, int numberImages)
	{
		byte[] buffer = Convert.FromBase64String(HtmlFormat.GetImageBase64Format(message));
		(builder as EmailMessage).Attachments.AddFileAttachment("image.jpeg", new MemoryStream(buffer));
		(builder as EmailMessage).Attachments[numberImages].ContentId = "ImageLogo" + numberImages;
	}

	private EmailMessage BuildMailMessage(MessageData message, ExchangeService service)
	{
		EmailMessage emailMessage = new EmailMessage(service)
		{
			Subject = message.Subject,
			From = new Microsoft.Exchange.WebServices.Data.EmailAddress(message.From)
		};
		switch (message.Importance)
		{
		case M1MessageImportance.High:
			emailMessage.Importance = Microsoft.Exchange.WebServices.Data.Importance.High;
			break;
		case M1MessageImportance.Low:
			emailMessage.Importance = Microsoft.Exchange.WebServices.Data.Importance.Low;
			break;
		default:
			emailMessage.Importance = Microsoft.Exchange.WebServices.Data.Importance.Normal;
			break;
		}
		foreach (string item in message.CleanRecipients(message.Recipients))
		{
			emailMessage.ToRecipients.Add(MailManager.GetValidEmailAddress(item));
		}
		foreach (string item2 in message.CleanRecipients(message.CC))
		{
			emailMessage.CcRecipients.Add(MailManager.GetValidEmailAddress(item2));
		}
		foreach (string item3 in message.CleanRecipients(message.BCC))
		{
			emailMessage.BccRecipients.Add(MailManager.GetValidEmailAddress(item3));
		}
		if (message.Body.IsHtml)
		{
			string text = BuildHtmlWithMessage(emailMessage, message, AttachLogo);
			emailMessage.Body = new MessageBody(Microsoft.Exchange.WebServices.Data.BodyType.HTML, text);
		}
		else
		{
			emailMessage.Body = new MessageBody(Microsoft.Exchange.WebServices.Data.BodyType.Text, message.Body.Text);
		}
		foreach (MessageAttachment attachment in message.Attachments)
		{
			emailMessage.Attachments.AddFileAttachment(attachment.Description, attachment.GetData());
		}
		return emailMessage;
	}

	public void GetMatchingNames(string name, Dictionary<string, string> searchResults)
	{
		try
		{
			new ExchangeUtilities();
			foreach (NameResolution item in _service.ResolveName(name, ResolveNameSearchLocation.DirectoryThenContacts, returnContactDetails: true))
			{
				Microsoft.Exchange.WebServices.Data.Contact contact = item.Contact;
				string key;
				string value;
				if (contact != null)
				{
					key = ((!string.IsNullOrWhiteSpace(item.Mailbox?.Address)) ? item.Mailbox.Address : GetAddress(contact.EmailAddresses));
					value = contact.DisplayName;
				}
				else
				{
					key = item.Mailbox.Address;
					value = item.Mailbox.Name;
				}
				if (!searchResults.ContainsKey(key))
				{
					searchResults.Add(key, value);
				}
			}
		}
		catch
		{
		}
	}

	private string GetAddress(EmailAddressDictionary addresses)
	{
		EmailAddressKey[] array = new EmailAddressKey[3]
		{
			EmailAddressKey.EmailAddress1,
			EmailAddressKey.EmailAddress2,
			EmailAddressKey.EmailAddress3
		};
		foreach (EmailAddressKey key in array)
		{
			string text = addresses[key].Address.Split(':')[1];
			if (!text.StartsWith("spo", StringComparison.CurrentCultureIgnoreCase) && IsValidEmail(text))
			{
				return text;
			}
		}
		return "";
	}

	private bool IsValidEmail(string email)
	{
		try
		{
			return new MailAddress(email).Address == email;
		}
		catch
		{
			return false;
		}
	}

	private static string BuildHtmlWithMessage(EmailMessage builder, MessageData message, AttachLogoToEmail<EmailMessage> attachLogo)
	{
		string text = message.Body.Html;
		if (!text.Contains("ImageLogo"))
		{
			return message.Body.Html;
		}
		string imgHtmlWithCID = HtmlFormat.GetImgHtmlWithCID("ImageLogo");
		List<string> imageOrPath = HtmlFormat.GetImageOrPath(text);
		int num = 0;
		foreach (string item in imageOrPath)
		{
			attachLogo(builder, item, num);
			text = text.Replace(item, imgHtmlWithCID + num);
			num++;
		}
		return text;
	}

	private void GeneratePublicClientApplication(M1UserSettings userSettings)
	{
		TokenHelper tokenHelper = new TokenHelper(userSettings);
		if (_publicClientAplApplication == null)
		{
			PublicClientApplicationOptions options = new PublicClientApplicationOptions
			{
				ClientId = "30c5e4e1-06ff-44bb-9f50-a4c4255edd25",
				TenantId = "common"
			};
			_publicClientAplApplication = PublicClientApplicationBuilder.CreateWithApplicationOptions(options).WithDefaultRedirectUri().Build();
			tokenHelper.EnableSerialization(_publicClientAplApplication.UserTokenCache);
		}
	}

	private static void CleanAuthenticationUserSettings(M1UserSettings userSettings)
	{
		userSettings.PrivateToken = string.Empty;
		userSettings.ProviderEmailAddress = string.Empty;
		userSettings.AccountIdentifier = string.Empty;
	}

	private static void SaveAuthenticationResultOnUserSettings(AuthenticationResult authentication, M1UserSettings userSettings)
	{
		userSettings.PrivateToken = authentication.AccessToken;
		userSettings.ProviderEmailAddress = authentication.Account.Username;
		userSettings.AccountIdentifier = authentication.Account.HomeAccountId.Identifier;
	}

	private void GenerateExchangeService(string accessToken)
	{
		_service = new ExchangeService
		{
			Url = new Uri("https://outlook.office365.com/EWS/Exchange.asmx"),
			Credentials = new OAuthCredentials(accessToken)
		};
	}
}
