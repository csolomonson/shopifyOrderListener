using System.Collections.ObjectModel;
using System.Threading;

namespace M1.Core;

public class M1DataDictionaryCollection : KeyedCollection<string, M1DataDictionary>
{
	private AppContext currentContext;

	public M1DataDictionaryCollection(AppContext context)
	{
		currentContext = context;
	}

	protected override string GetKeyForItem(M1DataDictionary item)
	{
		return item.ID.ToUpper();
	}

	public LoginReturnInfo LoginUsingPassedCredentials(string dataDictionaryName)
	{
		dataDictionaryName = dataDictionaryName.ToUpper();
		Mutex mutex = new Mutex(initiallyOwned: false, "M1" + currentContext.Version + "_" + dataDictionaryName);
		mutex.WaitOne();
		try
		{
			LoginReturnInfo loginReturnInfo = new LoginReturnInfo();
			if (Contains(dataDictionaryName))
			{
				loginReturnInfo.DataDictionary = base[dataDictionaryName];
			}
			else
			{
				loginReturnInfo.DataDictionary = new M1DataDictionary(currentContext, currentContext.DDServerManager);
				loginReturnInfo.DataDictionary.LoginDD(dataDictionaryName, languageOnly: false);
				loginReturnInfo.DataDictionaryCreated = true;
				Add(loginReturnInfo.DataDictionary);
			}
			return loginReturnInfo;
		}
		finally
		{
			mutex.ReleaseMutex();
			mutex = null;
		}
	}

	public bool LogoutAndRemove(M1DataDictionary m1DataDictionary)
	{
		M1DataDictionary m1DataDictionary2 = null;
		for (int num = base.Count - 1; num >= 0; num--)
		{
			if (m1DataDictionary == null || base[num] == m1DataDictionary)
			{
				m1DataDictionary2 = base[num];
				string iD = m1DataDictionary2.ID;
				if (!m1DataDictionary2.LogoutDD())
				{
					return false;
				}
				if (base.Dictionary.ContainsKey(iD))
				{
					base.Dictionary.Remove(iD);
				}
				if (Contains(m1DataDictionary2))
				{
					Remove(m1DataDictionary2);
				}
			}
		}
		return true;
	}
}
