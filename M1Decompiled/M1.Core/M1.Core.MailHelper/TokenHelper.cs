using System.Text;
using Microsoft.Identity.Client;

namespace M1.Core.MailHelper;

public class TokenHelper
{
	private readonly M1UserSettings _userSettings;

	private static readonly object FileLock = new object();

	public TokenHelper(M1UserSettings userSettings)
	{
		_userSettings = userSettings;
	}

	public void BeforeAccessNotification(TokenCacheNotificationArgs args)
	{
		lock (FileLock)
		{
			string cacheToken = _userSettings.CacheToken;
			byte[] msalV3State = (string.IsNullOrEmpty(cacheToken) ? null : Encoding.ASCII.GetBytes(cacheToken));
			args.TokenCache.DeserializeMsalV3(msalV3State);
		}
	}

	public void AfterAccessNotification(TokenCacheNotificationArgs args)
	{
		if (args.HasStateChanged)
		{
			lock (FileLock)
			{
				byte[] bytes = args.TokenCache.SerializeMsalV3();
				string cacheToken = Encoding.ASCII.GetString(bytes);
				_userSettings.CacheToken = cacheToken;
			}
		}
	}

	internal void EnableSerialization(ITokenCache tokenCache)
	{
		tokenCache.SetBeforeAccess(BeforeAccessNotification);
		tokenCache.SetAfterAccess(AfterAccessNotification);
	}
}
