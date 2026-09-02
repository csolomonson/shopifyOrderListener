using System.Collections.Generic;

namespace M1.API.DTOs.Core;

public class APISessionDto
{
	public string APIID { get; set; }

	public string M1ModuleCode { get; set; }

	public string APIUserID { get; set; }

	public string APIUserPassword { get; set; }

	public string DatabaseID { get; set; }

	public string DatadictionaryID { get; set; }

	public bool Authenticated { get; set; }

	public IDictionary<string, string> ExtraSettings { get; } = new Dictionary<string, string>();

	public string NetworkLibrary { get; set; }

	public string Server { get; set; }

	public string SQLUserID { get; set; }

	public string SQLUserPassword { get; set; }

	public string KeyStoreKey => M1ModuleCode + ":" + APIID;

	public bool TrustedConnection { get; set; }

	public bool IsReadOnly { get; set; }

	public APISessionDto()
	{
		APIID = string.Empty;
		APIUserID = "Admin";
		APIUserPassword = string.Empty;
		DatabaseID = "M1_MT";
		DatadictionaryID = "M1DD92";
	}
}
