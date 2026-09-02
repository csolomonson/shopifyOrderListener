using System;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Text;
using Avalara.AvaTax.Adapter;
using Avalara.AvaTax.Adapter.AddressService;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp.Financials.Avalara;

public class AvalaraAddressFunctions
{
	public class AddressInfo
	{
		public string table = string.Empty;

		public string recordID = string.Empty;

		public string recordUniqueID = string.Empty;

		public string Line1 = string.Empty;

		public string Line2 = string.Empty;

		public string Line3 = string.Empty;

		public string City = string.Empty;

		public string Region = string.Empty;

		public string PostalCode = string.Empty;

		public string Country = string.Empty;

		public string County = string.Empty;

		public bool Updated;

		public SeverityLevel Severity;

		public string MessageSummary = string.Empty;
	}

	public enum AvalaraTransactionType : byte
	{
		Ping = 1,
		ValidateAddress
	}

	public M1Database Database;

	public AddressSvc AddressSvc;

	public M1User User;

	public AvalaraAddressFunctions(M1Database m1Database, M1User m1User)
	{
		Database = m1Database;
		User = m1User;
	}

	public AddressSvc CreateAddressSvcConfig()
	{
		if (AddressSvc == null)
		{
			AddressSvc addressSvc = new AddressSvc();
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
			addressSvc.Profile.Client = "a0o0b00000523Vc";
			addressSvc.Configuration.Url = Database.Props("FN").Field<string>("xafAvalaraURL").Trim();
			addressSvc.Configuration.Security.Account = Database.Props("FN").Field<string>("xafAvalaraAccountID").Trim();
			addressSvc.Configuration.Security.License = Database.Props("FN").Field<string>("xafAvalaraLicenseKey").Trim();
			addressSvc.Configuration.Security.UserName = "";
			addressSvc.Configuration.Security.Password = "";
			addressSvc.Configuration.RequestTimeout = ((Database.Props("FN").Field<short>("xafAvalaraTimeoutSeconds") > 0) ? Convert.ToInt32(Database.Props("FN").Field<short>("xafAvalaraTimeoutSeconds")) : 100);
			AddressSvc = addressSvc;
		}
		return AddressSvc;
	}

	public AddressInfo ValidateAddresses(AddressInfo info)
	{
		bool updated = false;
		AddressSvc addressSvc = CreateAddressSvcConfig();
		Address address = new Address();
		address.Line1 = info.Line1;
		address.Line2 = info.Line2;
		address.Line3 = info.Line3;
		address.City = info.City;
		address.Region = info.Region;
		address.PostalCode = info.PostalCode;
		address.Country = info.Country;
		if (!checkAddressForBlank(address))
		{
			if (checkAddressCountry((address.Country == null) ? string.Empty : address.Country))
			{
				ValidateRequest validateRequest = new ValidateRequest();
				validateRequest.Address = address;
				validateRequest.TextCase = TextCase.Mixed;
				ValidateResult validateResult = addressSvc.Validate(validateRequest);
				AddAvalaraTransaction(info.table, info.recordID, validateResult);
				info.Severity = validateResult.ResultCode;
				if (validateResult.ResultCode == SeverityLevel.Success || validateResult.ResultCode == SeverityLevel.Warning)
				{
					if (!info.Line1.Equals(validateResult.Addresses[0].Line1))
					{
						info.Line1 = validateResult.Addresses[0].Line1;
						updated = true;
					}
					if (!info.Line2.Equals(validateResult.Addresses[0].Line2))
					{
						info.Line2 = validateResult.Addresses[0].Line2;
						updated = true;
					}
					if (!info.Line3.Equals(validateResult.Addresses[0].Line3))
					{
						info.Line3 = validateResult.Addresses[0].Line3;
						updated = true;
					}
					if (!info.City.Equals(validateResult.Addresses[0].City))
					{
						info.City = validateResult.Addresses[0].City;
						updated = true;
					}
					if (!info.Region.Equals(validateResult.Addresses[0].Region))
					{
						info.Region = validateResult.Addresses[0].Region;
						updated = true;
					}
					if (!info.PostalCode.Equals(validateResult.Addresses[0].PostalCode))
					{
						info.PostalCode = validateResult.Addresses[0].PostalCode;
						updated = true;
					}
					if (!info.Country.Equals(validateResult.Addresses[0].Country))
					{
						info.Country = validateResult.Addresses[0].Country;
						updated = true;
					}
					if (!info.County.Equals(validateResult.Addresses[0].County))
					{
						info.County = validateResult.Addresses[0].County;
						updated = true;
					}
				}
				if (validateResult.Messages.Count > 0 && validateResult.Messages[0].Summary.Length > 0)
				{
					info.MessageSummary = validateResult.Messages[0].Summary;
					updated = true;
				}
				info.Updated = updated;
			}
			else
			{
				info.Severity = SeverityLevel.Error;
				info.MessageSummary = "Address Country is not within range.";
				info.Updated = true;
			}
		}
		else
		{
			info.Severity = SeverityLevel.Error;
			info.MessageSummary = "Address cannot be blank.";
			info.Updated = true;
		}
		return info;
	}

	public AddressInfo ValidateAddresses(Address address, string table, string recordID)
	{
		bool updated = false;
		AddressSvc addressSvc = CreateAddressSvcConfig();
		AddressInfo addressInfo = new AddressInfo();
		addressInfo.table = table;
		addressInfo.recordID = recordID;
		if (!checkAddressForBlank(address))
		{
			if (checkAddressCountry(address.Country))
			{
				ValidateRequest validateRequest = new ValidateRequest();
				validateRequest.Address = address;
				validateRequest.TextCase = TextCase.Mixed;
				ValidateResult validateResult = addressSvc.Validate(validateRequest);
				AddAvalaraTransaction(addressInfo.table, addressInfo.recordID, validateResult);
				addressInfo.Severity = validateResult.ResultCode;
				if (validateResult.ResultCode == SeverityLevel.Success || validateResult.ResultCode == SeverityLevel.Warning)
				{
					if (!addressInfo.Line1.Equals(validateResult.Addresses[0].Line1))
					{
						addressInfo.Line1 = validateResult.Addresses[0].Line1;
						updated = true;
					}
					if (!addressInfo.Line2.Equals(validateResult.Addresses[0].Line2))
					{
						addressInfo.Line2 = validateResult.Addresses[0].Line2;
						updated = true;
					}
					if (!addressInfo.Line3.Equals(validateResult.Addresses[0].Line3))
					{
						addressInfo.Line3 = validateResult.Addresses[0].Line3;
						updated = true;
					}
					if (!addressInfo.City.Equals(validateResult.Addresses[0].City))
					{
						addressInfo.City = validateResult.Addresses[0].City;
						updated = true;
					}
					if (!addressInfo.Region.Equals(validateResult.Addresses[0].Region))
					{
						addressInfo.Region = validateResult.Addresses[0].Region;
						updated = true;
					}
					if (!addressInfo.PostalCode.Equals(validateResult.Addresses[0].PostalCode))
					{
						addressInfo.PostalCode = validateResult.Addresses[0].PostalCode;
						updated = true;
					}
					if (!addressInfo.Country.Equals(validateResult.Addresses[0].Country))
					{
						addressInfo.Country = validateResult.Addresses[0].Country;
						updated = true;
					}
					if (!addressInfo.County.Equals(validateResult.Addresses[0].County))
					{
						addressInfo.County = validateResult.Addresses[0].County;
						updated = true;
					}
				}
				if (validateResult.Messages.Count > 0 && validateResult.Messages[0].Summary.Length > 0)
				{
					addressInfo.MessageSummary = validateResult.Messages[0].Summary;
					updated = true;
				}
				addressInfo.Updated = updated;
			}
			else
			{
				addressInfo.Severity = SeverityLevel.Error;
				addressInfo.MessageSummary = "Address Country is not within range.";
				addressInfo.Updated = true;
			}
		}
		else
		{
			addressInfo.Severity = SeverityLevel.Error;
			addressInfo.MessageSummary = "Address cannot be blank.";
			addressInfo.Updated = true;
		}
		return addressInfo;
	}

	private bool checkAddressForBlank(Address info)
	{
		if (string.IsNullOrWhiteSpace(info.Line1) && string.IsNullOrWhiteSpace(info.Line2) && string.IsNullOrWhiteSpace(info.Line3) && string.IsNullOrWhiteSpace(info.City) && string.IsNullOrWhiteSpace(info.Region) && string.IsNullOrWhiteSpace(info.PostalCode) && string.IsNullOrWhiteSpace(info.Country))
		{
			return true;
		}
		return false;
	}

	private bool checkAddressCountry(string country)
	{
		bool result = false;
		decimal value = Database.Props("FN").Field<byte>("xafAvalaraFilterCountry");
		if (!string.IsNullOrWhiteSpace(country))
		{
			country = country.Trim();
			switch (Convert.ToInt16(value))
			{
			case 0:
			case 1:
				if (country.Equals("US", StringComparison.CurrentCultureIgnoreCase) || country.Equals("USA", StringComparison.CurrentCultureIgnoreCase) || country.Equals("CA", StringComparison.CurrentCultureIgnoreCase) || country.Equals("CANADA", StringComparison.CurrentCultureIgnoreCase))
				{
					result = true;
				}
				break;
			case 2:
				if (country.Equals("US", StringComparison.CurrentCultureIgnoreCase) || country.Equals("USA", StringComparison.CurrentCultureIgnoreCase))
				{
					result = true;
				}
				break;
			case 3:
				if (country.Equals("CA", StringComparison.CurrentCultureIgnoreCase) || country.Equals("CANADA", StringComparison.CurrentCultureIgnoreCase))
				{
					result = true;
				}
				break;
			default:
				if (country.Equals("US", StringComparison.CurrentCultureIgnoreCase) || country.Equals("USA", StringComparison.CurrentCultureIgnoreCase) || country.Equals("CA", StringComparison.CurrentCultureIgnoreCase) || country.Equals("CANADA", StringComparison.CurrentCultureIgnoreCase))
				{
					result = true;
				}
				break;
			}
		}
		else
		{
			result = true;
		}
		return result;
	}

	public string PingInterface()
	{
		AddressSvc addressSvc = CreateAddressSvcConfig();
		try
		{
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
			PingResult pingResult = addressSvc.Ping("");
			AddAvalaraTransaction(pingResult);
			if (pingResult.ResultCode >= SeverityLevel.Error)
			{
				return pingResult.Messages[0].Summary;
			}
			return "Result Code: " + pingResult.ResultCode.ToString() + "\r\n# Messages: " + pingResult.Messages.Count + "\r\nService Version: " + pingResult.Version;
		}
		catch (Exception ex)
		{
			return ex.Message;
		}
	}

	public bool AddAvalaraTransaction(PingResult result)
	{
		SqlDataAdapter adapter = new SqlDataAdapter();
		DataTable dataTable = Database.GetDataTable("SELECT * FROM AvalaraTransactions WHERE 0=1", fillSchema: true, out adapter);
		DataRow dataRow = null;
		dataRow = dataTable.NewRow().BlankRow();
		dataRow.BeginEdit();
		dataRow["avtAvalaraTransactionID"] = Database.NextIDs.GetNextIDForTable("AvalaraTransactions");
		dataRow["avtSourceTable"] = string.Empty;
		dataRow["avtSourceTableKeyFields"] = string.Empty;
		dataRow["avtTransactionDate"] = DateTime.Now;
		dataRow["avtTransactionType"] = AvalaraTransactionType.Ping;
		dataRow["avtTransactionID"] = ((result.TransactionId == null) ? string.Empty : result.TransactionId);
		dataRow["avtResultCode"] = result.ResultCode;
		if (result.Messages.Count > 0)
		{
			dataRow["avtMessageSummary"] = result.Messages[0].Summary;
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Message message in result.Messages)
			{
				if (message.Details.Trim().Length > 0)
				{
					stringBuilder.Append(",");
					stringBuilder.Append(message.Details);
				}
			}
			if (stringBuilder.ToString().Length > 0)
			{
				dataRow["avtMessageDetail"] = stringBuilder.ToString().Substring(1);
			}
		}
		if (result.ReferenceCode != null)
		{
			dataRow["avtReferenceCode"] = result.ReferenceCode;
		}
		dataRow["avtCreatedBy"] = User.ID;
		dataRow["avtCreatedDate"] = DateTime.Now;
		dataRow.EndEdit();
		dataTable.Rows.Add(dataRow);
		if (dataTable.Rows.Count > 0)
		{
			Database.UpdateData(dataTable, adapter);
			return true;
		}
		return false;
	}

	public bool AddAvalaraTransaction(string table, string recordID, ValidateResult result)
	{
		SqlDataAdapter adapter = new SqlDataAdapter();
		DataTable dataTable = Database.GetDataTable("SELECT * FROM AvalaraTransactions WHERE 0=1", fillSchema: true, out adapter);
		DataRow dataRow = null;
		dataRow = dataTable.NewRow().BlankRow();
		dataRow.BeginEdit();
		dataRow["avtAvalaraTransactionID"] = Database.NextIDs.GetNextIDForTable("AvalaraTransactions");
		dataRow["avtSourceTable"] = table;
		dataRow["avtSourceTableKeyFields"] = recordID;
		dataRow["avtTransactionDate"] = DateTime.Now;
		dataRow["avtTransactionType"] = AvalaraTransactionType.ValidateAddress;
		dataRow["avtTransactionID"] = result.TransactionId;
		dataRow["avtResultCode"] = result.ResultCode;
		if (result.Messages.Count > 0)
		{
			dataRow["avtMessageSummary"] = result.ResultCode.ToString() + ": " + result.Messages[0].Summary;
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Message message in result.Messages)
			{
				if (message.Details.Trim().Length > 0)
				{
					stringBuilder.Append(",");
					stringBuilder.Append(message.Details);
				}
			}
			if (stringBuilder.ToString().Length > 0)
			{
				dataRow["avtMessageDetail"] = stringBuilder.ToString().Substring(1);
			}
		}
		if (result.ReferenceCode != null)
		{
			dataRow["avtReferenceCode"] = result.ReferenceCode;
		}
		dataRow["avtCreatedBy"] = User.ID;
		dataRow["avtCreatedDate"] = DateTime.Now;
		dataRow.EndEdit();
		dataTable.Rows.Add(dataRow);
		if (dataTable.Rows.Count > 0)
		{
			Database.UpdateData(dataTable, adapter);
			return true;
		}
		return false;
	}
}
