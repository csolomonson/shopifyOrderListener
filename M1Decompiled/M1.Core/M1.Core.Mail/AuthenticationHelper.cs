using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Graph;
using Microsoft.Identity.Client;

namespace M1.Core.Mail;

public class AuthenticationHelper
{
	private static string clientIdForUser = "d3c33262-9dc3-4f2a-8ab0-082f7ef9b60f";

	public static IEnumerable<string> Scopes = new List<string> { "User.Read", "User.ReadWrite", "User.ReadBasic.All", "Calendars.ReadWrite", "Contacts.Read", "Mail.Send", "Mail.ReadWrite", "Files.ReadWrite" };

	public static IPublicClientApplication IdentityClientApp = PublicClientApplicationBuilder.Create(clientIdForUser).Build();

	private static GraphServiceClient _graphClient = null;

	private static string _accessToken = "";

	private static DateTimeOffset _tokenExpirationDate;

	public static GraphServiceClient GetAuthenticatedClientForUser()
	{
		try
		{
			_graphClient = new GraphServiceClient("https://graph.microsoft.com/v1.0", new DelegateAuthenticationProvider(async delegate(HttpRequestMessage requestMessage)
			{
				if (DateTimeOffset.Now > _tokenExpirationDate)
				{
					AuthenticationResult obj = await GetAuthenticatedUserAsync();
					_tokenExpirationDate = obj.ExpiresOn;
					_accessToken = obj.AccessToken;
				}
				requestMessage.Headers.Authorization = new AuthenticationHeaderValue("bearer", _accessToken);
			}));
			return _graphClient;
		}
		catch (Exception)
		{
		}
		return _graphClient;
	}

	public static async Task<AuthenticationResult> GetAuthenticatedUserAsync()
	{
		IAccount account = (await IdentityClientApp.GetAccountsAsync()).FirstOrDefault();
		dynamic authResult;
		try
		{
			authResult = await IdentityClientApp.AcquireTokenSilent(Scopes, account).WithForceRefresh(forceRefresh: true).ExecuteAsync();
		}
		catch (MsalUiRequiredException)
		{
			authResult = await IdentityClientApp.AcquireTokenInteractive(Scopes).ExecuteAsync();
		}
		return authResult;
	}

	public static async void SignOut()
	{
		IAccount[] array = (await IdentityClientApp.GetAccountsAsync()).ToArray();
		foreach (IAccount account in array)
		{
			await IdentityClientApp.RemoveAsync(account);
		}
		_graphClient = null;
	}
}
