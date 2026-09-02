using System.Collections.Generic;

namespace M1.API.DTOs.Core;

public class APIMetadataDto
{
	public string M1ModuleID { get; set; }

	public string APIID { get; set; }

	public string APIKey { get; set; }

	public string DataDictionaryID { get; set; }

	public string AdminUserID { get; set; }

	public string AdminPassword { get; set; }

	public string Server { get; set; }

	public string NetworkLibrary { get; set; }

	public bool TrustedConnection { get; set; }

	public bool IsReadOnly { get; set; }

	public string SqlUserID { get; set; }

	public string SqlPassword { get; set; }

	public IDictionary<string, string> ExtraSettings { get; set; }

	public string DatabaseId { get; set; }

	public APIMetadataDto(string m1ModuleID, string apiID, string aPIKey, string adminUserID, string adminPassword, string dataDictionaryID, string server, string networkLibrary, bool trustedConnection, bool isReadOnly)
	{
		M1ModuleID = m1ModuleID;
		APIID = apiID;
		APIKey = aPIKey;
		AdminUserID = adminUserID;
		AdminPassword = adminPassword;
		DataDictionaryID = dataDictionaryID;
		Server = server;
		NetworkLibrary = networkLibrary;
		TrustedConnection = trustedConnection;
		IsReadOnly = isReadOnly;
	}

	public APIMetadataDto(string aPIKey, string dataDictionaryID)
	{
		APIKey = aPIKey;
		DataDictionaryID = dataDictionaryID;
	}
}
