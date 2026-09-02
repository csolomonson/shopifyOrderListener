using System;
using System.Data;
using Avalara.AvaTax.Adapter;
using M1.Ax.Erp.Financials.Avalara;
using M1.Core;

namespace M1.Ax.Erp;

public class AvalaraTax
{
	public string ValidateAddress(M1BindingSource bsAddress)
	{
		DataRow currentAsDataRow = bsAddress.CurrentAsDataRow;
		string result = string.Empty;
		AvalaraAddressFunctions.AddressInfo addressInfo = new AvalaraAddressFunctions.AddressInfo();
		addressInfo.table = bsAddress.PrimaryTable.TableName;
		string text = string.Empty;
		string[] keyFieldsArray = bsAddress.PrimaryTable.KeyFieldsArray;
		foreach (string columnName in keyFieldsArray)
		{
			if (!string.IsNullOrWhiteSpace(Convert.ToString(currentAsDataRow[columnName])))
			{
				if (text.Length > 0)
				{
					text += "-";
				}
				text += Convert.ToString(currentAsDataRow[columnName]);
			}
		}
		addressInfo.recordID = text;
		string fieldPrefix = bsAddress.PrimaryTable.FieldPrefix;
		addressInfo.Line1 = currentAsDataRow.Field<string>(fieldPrefix + "AddressLine1");
		addressInfo.Line2 = currentAsDataRow.Field<string>(fieldPrefix + "AddressLine2");
		addressInfo.Line3 = currentAsDataRow.Field<string>(fieldPrefix + "AddressLine3");
		addressInfo.City = currentAsDataRow.Field<string>(fieldPrefix + "City");
		addressInfo.Region = currentAsDataRow.Field<string>(fieldPrefix + "State");
		addressInfo.PostalCode = currentAsDataRow.Field<string>(fieldPrefix + "PostCode");
		addressInfo.Country = currentAsDataRow.Field<string>(fieldPrefix + "Country");
		if (bsAddress.Fields.Contains(fieldPrefix + "County"))
		{
			addressInfo.County = currentAsDataRow.Field<string>(fieldPrefix + "County");
		}
		AvalaraAddressFunctions.AddressInfo addressInfo2 = new AvalaraAddressFunctions(bsAddress.Database, bsAddress.User).ValidateAddresses(addressInfo);
		if (addressInfo2 != null)
		{
			if (addressInfo2.Updated && (addressInfo2.Severity == SeverityLevel.Success || addressInfo2.Severity == SeverityLevel.Warning))
			{
				currentAsDataRow[fieldPrefix + "AddressLine1"] = addressInfo2.Line1;
				currentAsDataRow[fieldPrefix + "AddressLine2"] = addressInfo2.Line2;
				currentAsDataRow[fieldPrefix + "AddressLine3"] = addressInfo2.Line3;
				currentAsDataRow[fieldPrefix + "City"] = addressInfo2.City;
				currentAsDataRow[fieldPrefix + "State"] = addressInfo2.Region;
				currentAsDataRow[fieldPrefix + "PostCode"] = addressInfo2.PostalCode;
				currentAsDataRow[fieldPrefix + "Country"] = addressInfo2.Country;
				if (bsAddress.Fields.Contains(fieldPrefix + "County"))
				{
					currentAsDataRow[fieldPrefix + "County"] = addressInfo2.County;
				}
				currentAsDataRow[fieldPrefix + "AvalaraAddressValidated"] = true;
			}
			switch (addressInfo2.Severity)
			{
			case SeverityLevel.Success:
				result = "Address Validate Successfully.";
				break;
			case SeverityLevel.Warning:
				result = "Warning: " + addressInfo2.MessageSummary;
				break;
			case SeverityLevel.Error:
				result = "Error: " + addressInfo2.MessageSummary;
				break;
			case SeverityLevel.Exception:
				result = "Exception: " + addressInfo2.MessageSummary;
				break;
			}
		}
		else
		{
			result = "Unable to validate address.";
		}
		return result;
	}

	public string GetTax(M1BindingSource bsGetTax, bool postToAvalara)
	{
		string empty = string.Empty;
		_ = string.Empty;
		return new AvalaraTaxFunctions(bsGetTax.Database, bsGetTax.User).GetTax(recordID: bsGetTax.CurrentAsDataRow[bsGetTax.PrimaryTable.KeyFieldsArray[0]].ToString(), table: bsGetTax.PrimaryTable.TableName, postToAvalara: postToAvalara, bs: bsGetTax);
	}

	public string PostTax(M1BindingSource bsPostTax)
	{
		string empty = string.Empty;
		_ = string.Empty;
		return new AvalaraTaxFunctions(bsPostTax.Database, bsPostTax.User).PostTax(recordID: bsPostTax.CurrentAsDataRow[bsPostTax.PrimaryTable.KeyFieldsArray[0]].ToString(), table: bsPostTax.PrimaryTable.TableName);
	}

	public string GetARPaymentTax(M1BindingSource bsGetTax)
	{
		object[] array = new object[1];
		_ = string.Empty;
		AvalaraTaxFunctions avalaraTaxFunctions = new AvalaraTaxFunctions(bsGetTax.Database, bsGetTax.User);
		array[0] = bsGetTax.CurrentAsDataRow[bsGetTax.PrimaryTable.KeyFieldsArray[0]];
		return avalaraTaxFunctions.GetARPaymentTax(bsGetTax.PrimaryTable.TableName, array, bsGetTax);
	}

	public string PostPaymentTax(M1BindingSource bsPostTax)
	{
		int num = 0;
		_ = string.Empty;
		AvalaraTaxFunctions avalaraTaxFunctions = new AvalaraTaxFunctions(bsPostTax.Database, bsPostTax.User);
		num = Convert.ToInt16(bsPostTax.CurrentAsDataRow[bsPostTax.PrimaryTable.KeyFieldsArray[0]]);
		return avalaraTaxFunctions.PostPaymentTax(num);
	}

	public int CheckLastSuccessfulTransaction(M1BindingSource bs)
	{
		object[] array = new object[bs.PrimaryTable.KeyFieldsArray.Length];
		int num = 0;
		DataRow currentAsDataRow = bs.CurrentAsDataRow;
		string[] keyFieldsArray = bs.PrimaryTable.KeyFieldsArray;
		foreach (string columnName in keyFieldsArray)
		{
			array[num] = currentAsDataRow[columnName];
			num++;
		}
		return new AvalaraTaxFunctions(bs.Database, bs.User).CheckLastSuccessfulTransaction(bs.PrimaryTable.TableName, array);
	}

	public string CancelTax(M1BindingSource bs, string table, string recordID)
	{
		return new AvalaraTaxFunctions(bs.Database, bs.User).CancelTax(table, recordID);
	}

	public string CancelPaymentTax(M1BindingSource bs, int sessionID)
	{
		return new AvalaraTaxFunctions(bs.Database, bs.User).CancelPaymentTax(sessionID);
	}
}
