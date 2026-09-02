using System;
using M1.Core;

namespace M1.API.Utilities;

public sealed class APIClientContext : IDisposable
{
	public Guid ID = Guid.Empty;

	public string UserID = string.Empty;

	public string UserPassword = string.Empty;

	public string HashedUserPassword = string.Empty;

	public string DatabaseID = string.Empty;

	public string DataDictionaryID = string.Empty;

	public string Module = string.Empty;

	public M1Database Database;

	public M1DataDictionary DataDictionary;

	public M1User User;

	public bool Active;

	public bool LoginAuthenticated;

	public M1.Core.AppContext DbContext { get; set; }

	public string LoginErrorOutputString { get; set; }

	public string WebSessionID { get; set; }

	public string APIID { get; set; }

	public string M1ModuleCode { get; set; }

	public bool IsReadOnly { get; set; }

	public string KeyStoreKey => M1ModuleCode + ":" + APIID;

	public void Dispose()
	{
	}
}
