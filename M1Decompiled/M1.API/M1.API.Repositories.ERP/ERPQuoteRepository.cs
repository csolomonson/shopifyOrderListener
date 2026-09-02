using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Utilities;
using M1.Extensions;

namespace M1.API.Repositories.ERP;

public class ERPQuoteRepository : APIBaseRepository, IERPQuoteRepository, IAPIBaseRepository, IDisposable
{
	public ERPQuoteRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesQuoteExist(Guid quoteId)
	{
		InitializeParameterLists();
		base.filterList.Add("qmpUniqueID|C", quoteId);
		base.selectList.Add("qmpUniqueID");
		return Task.FromResult(GetAsObject("Quotes", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPQuoteInformationDto>> GetAllQuotes(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPQuoteInformationDto> collection = new List<ERPQuoteInformationDto>();
		InitializeParameterLists();
		string[] array = new string[38]
		{
			"qmpArInvoiceContactID", "qmpArInvoiceLocationID", "qmpClosedDate", "qmpQuoteID", "qmpCreatedBy", "qmpCreatedDate", "qmpCurrencyRateID", "qmpCustomerOrganizationID", "qmpDueDate", "qmpUniqueID",
			"qmpExchangeRate", "qmpExpirationDate", "qmpFreeOnBoardDescription", "qmpAvalaraTaxCalculated", "qmpClosed", "qmpCreatedFromMobile", "qmpCustomRate", "qmpPaymentTermID", "qmpPlantDepartmentID", "qmpPlantID",
			"qmpProjectID", "qmpQuoteContactID", "qmpQuoteDate", "qmpQuoteFooterMessageRTF", "qmpQuoteFooterMessageText", "qmpQuoteHeaderMessageRTF", "qmpQuoteHeaderMessageText", "qmpQuoteLocationID", "qmpQuoterEmployeeID", "qmpRowVersion",
			"qmpShipContactID", "qmpShipLocationID", "qmpShipOrganizationID", "qmpShippingMethodID", "qmpShippingPaymentTypeID", "qmpSplitPercentTotal", "qmpStandardMessageID", "qmpTaxDate"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("Quotes");
		List<string> list = new List<string>();
		string[] fields = ((base.selectList.Count != array.Count()) ? base.selectList.ToArray() : array);
		if (orderBy != null && orderBy.Length > 0)
		{
			ParseAndAddOrderByFields(orderBy, list, fields);
		}
		if (list.Count == 0)
		{
			list = new List<string> { "1" };
		}
		if (filter != null && filter.Length != 0)
		{
			ParseAndAddFilter(filter, base.filterList, fields);
		}
		using (DataTable dataTable = GetAsDataTable("Quotes", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPQuoteInformationDto eRPQuoteInformationDto = new ERPQuoteInformationDto();
				eRPQuoteInformationDto.qmpArInvoiceContactID = dataTable.Rows[i].Field<string>("qmpArInvoiceContactID");
				eRPQuoteInformationDto.qmpArInvoiceLocationID = dataTable.Rows[i].Field<string>("qmpArInvoiceLocationID");
				eRPQuoteInformationDto.qmpClosedDate = dataTable.Rows[i].Field<DateTime?>("qmpClosedDate");
				eRPQuoteInformationDto.qmpQuoteID = dataTable.Rows[i].Field<string>("qmpQuoteID");
				eRPQuoteInformationDto.qmpCreatedBy = dataTable.Rows[i].Field<string>("qmpCreatedBy");
				eRPQuoteInformationDto.qmpCreatedDate = dataTable.Rows[i].Field<DateTime?>("qmpCreatedDate");
				eRPQuoteInformationDto.qmpCurrencyRateID = dataTable.Rows[i].Field<string>("qmpCurrencyRateID");
				eRPQuoteInformationDto.qmpCustomerOrganizationID = dataTable.Rows[i].Field<string>("qmpCustomerOrganizationID");
				eRPQuoteInformationDto.qmpDueDate = dataTable.Rows[i].Field<DateTime?>("qmpDueDate");
				eRPQuoteInformationDto.qmpUniqueID = dataTable.Rows[i].Field<Guid>("qmpUniqueID");
				eRPQuoteInformationDto.qmpExchangeRate = dataTable.Rows[i].Field<decimal>("qmpExchangeRate");
				eRPQuoteInformationDto.qmpExpirationDate = dataTable.Rows[i].Field<DateTime?>("qmpExpirationDate");
				eRPQuoteInformationDto.qmpFreeOnBoardDescription = dataTable.Rows[i].Field<string>("qmpFreeOnBoardDescription");
				eRPQuoteInformationDto.qmpAvalaraTaxCalculated = dataTable.Rows[i].Field<bool>("qmpAvalaraTaxCalculated");
				eRPQuoteInformationDto.qmpClosed = dataTable.Rows[i].Field<bool>("qmpClosed");
				eRPQuoteInformationDto.qmpCreatedFromMobile = dataTable.Rows[i].Field<bool>("qmpCreatedFromMobile");
				eRPQuoteInformationDto.qmpCustomRate = dataTable.Rows[i].Field<bool>("qmpCustomRate");
				eRPQuoteInformationDto.qmpPaymentTermID = dataTable.Rows[i].Field<string>("qmpPaymentTermID");
				eRPQuoteInformationDto.qmpPlantDepartmentID = dataTable.Rows[i].Field<string>("qmpPlantDepartmentID");
				eRPQuoteInformationDto.qmpPlantID = dataTable.Rows[i].Field<string>("qmpPlantID");
				eRPQuoteInformationDto.qmpProjectID = dataTable.Rows[i].Field<string>("qmpProjectID");
				eRPQuoteInformationDto.qmpQuoteContactID = dataTable.Rows[i].Field<string>("qmpQuoteContactID");
				eRPQuoteInformationDto.qmpQuoteDate = dataTable.Rows[i].Field<DateTime?>("qmpQuoteDate");
				eRPQuoteInformationDto.qmpQuoteFooterMessageRTF = dataTable.Rows[i].Field<string>("qmpQuoteFooterMessageRTF");
				eRPQuoteInformationDto.qmpQuoteFooterMessageText = dataTable.Rows[i].Field<string>("qmpQuoteFooterMessageText");
				eRPQuoteInformationDto.qmpQuoteHeaderMessageRTF = dataTable.Rows[i].Field<string>("qmpQuoteHeaderMessageRTF");
				eRPQuoteInformationDto.qmpQuoteHeaderMessageText = dataTable.Rows[i].Field<string>("qmpQuoteHeaderMessageText");
				eRPQuoteInformationDto.qmpQuoteLocationID = dataTable.Rows[i].Field<string>("qmpQuoteLocationID");
				eRPQuoteInformationDto.qmpQuoterEmployeeID = dataTable.Rows[i].Field<string>("qmpQuoterEmployeeID");
				eRPQuoteInformationDto.qmpRowVersion = dataTable.Rows[i].Field<byte[]>("qmpRowVersion");
				eRPQuoteInformationDto.qmpShipContactID = dataTable.Rows[i].Field<string>("qmpShipContactID");
				eRPQuoteInformationDto.qmpShipLocationID = dataTable.Rows[i].Field<string>("qmpShipLocationID");
				eRPQuoteInformationDto.qmpShipOrganizationID = dataTable.Rows[i].Field<string>("qmpShipOrganizationID");
				eRPQuoteInformationDto.qmpShippingMethodID = dataTable.Rows[i].Field<string>("qmpShippingMethodID");
				eRPQuoteInformationDto.qmpShippingPaymentTypeID = dataTable.Rows[i].Field<string>("qmpShippingPaymentTypeID");
				eRPQuoteInformationDto.qmpSplitPercentTotal = dataTable.Rows[i].Field<decimal>("qmpSplitPercentTotal");
				eRPQuoteInformationDto.qmpStandardMessageID = dataTable.Rows[i].Field<string>("qmpStandardMessageID");
				eRPQuoteInformationDto.qmpTaxDate = dataTable.Rows[i].Field<DateTime?>("qmpTaxDate");
				eRPQuoteInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPQuoteInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPQuoteInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPQuoteInformationDto> GetQuote(Guid quoteId)
	{
		ERPQuoteInformationDto eRPQuoteInformationDto = new ERPQuoteInformationDto();
		InitializeParameterLists();
		string[] collection = new string[38]
		{
			"qmpArInvoiceContactID", "qmpArInvoiceLocationID", "qmpClosedDate", "qmpQuoteID", "qmpCreatedBy", "qmpCreatedDate", "qmpCurrencyRateID", "qmpCustomerOrganizationID", "qmpDueDate", "qmpUniqueID",
			"qmpExchangeRate", "qmpExpirationDate", "qmpFreeOnBoardDescription", "qmpAvalaraTaxCalculated", "qmpClosed", "qmpCreatedFromMobile", "qmpCustomRate", "qmpPaymentTermID", "qmpPlantDepartmentID", "qmpPlantID",
			"qmpProjectID", "qmpQuoteContactID", "qmpQuoteDate", "qmpQuoteFooterMessageRTF", "qmpQuoteFooterMessageText", "qmpQuoteHeaderMessageRTF", "qmpQuoteHeaderMessageText", "qmpQuoteLocationID", "qmpQuoterEmployeeID", "qmpRowVersion",
			"qmpShipContactID", "qmpShipLocationID", "qmpShipOrganizationID", "qmpShippingMethodID", "qmpShippingPaymentTypeID", "qmpSplitPercentTotal", "qmpStandardMessageID", "qmpTaxDate"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("qmpUniqueID|C", quoteId);
		AddCustomFieldsToSelectList("Quotes");
		using (DataTable dataTable = GetAsDataTable("Quotes", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPQuoteInformationDto);
			}
			eRPQuoteInformationDto.qmpArInvoiceContactID = dataTable.Rows[0].Field<string>("qmpArInvoiceContactID");
			eRPQuoteInformationDto.qmpArInvoiceLocationID = dataTable.Rows[0].Field<string>("qmpArInvoiceLocationID");
			eRPQuoteInformationDto.qmpClosedDate = dataTable.Rows[0].Field<DateTime?>("qmpClosedDate");
			eRPQuoteInformationDto.qmpQuoteID = dataTable.Rows[0].Field<string>("qmpQuoteID");
			eRPQuoteInformationDto.qmpCreatedBy = dataTable.Rows[0].Field<string>("qmpCreatedBy");
			eRPQuoteInformationDto.qmpCreatedDate = dataTable.Rows[0].Field<DateTime?>("qmpCreatedDate");
			eRPQuoteInformationDto.qmpCurrencyRateID = dataTable.Rows[0].Field<string>("qmpCurrencyRateID");
			eRPQuoteInformationDto.qmpCustomerOrganizationID = dataTable.Rows[0].Field<string>("qmpCustomerOrganizationID");
			eRPQuoteInformationDto.qmpDueDate = dataTable.Rows[0].Field<DateTime?>("qmpDueDate");
			eRPQuoteInformationDto.qmpUniqueID = dataTable.Rows[0].Field<Guid>("qmpUniqueID");
			eRPQuoteInformationDto.qmpExchangeRate = dataTable.Rows[0].Field<decimal>("qmpExchangeRate");
			eRPQuoteInformationDto.qmpExpirationDate = dataTable.Rows[0].Field<DateTime?>("qmpExpirationDate");
			eRPQuoteInformationDto.qmpFreeOnBoardDescription = dataTable.Rows[0].Field<string>("qmpFreeOnBoardDescription");
			eRPQuoteInformationDto.qmpAvalaraTaxCalculated = dataTable.Rows[0].Field<bool>("qmpAvalaraTaxCalculated");
			eRPQuoteInformationDto.qmpClosed = dataTable.Rows[0].Field<bool>("qmpClosed");
			eRPQuoteInformationDto.qmpCreatedFromMobile = dataTable.Rows[0].Field<bool>("qmpCreatedFromMobile");
			eRPQuoteInformationDto.qmpCustomRate = dataTable.Rows[0].Field<bool>("qmpCustomRate");
			eRPQuoteInformationDto.qmpPaymentTermID = dataTable.Rows[0].Field<string>("qmpPaymentTermID");
			eRPQuoteInformationDto.qmpPlantDepartmentID = dataTable.Rows[0].Field<string>("qmpPlantDepartmentID");
			eRPQuoteInformationDto.qmpPlantID = dataTable.Rows[0].Field<string>("qmpPlantID");
			eRPQuoteInformationDto.qmpProjectID = dataTable.Rows[0].Field<string>("qmpProjectID");
			eRPQuoteInformationDto.qmpQuoteContactID = dataTable.Rows[0].Field<string>("qmpQuoteContactID");
			eRPQuoteInformationDto.qmpQuoteDate = dataTable.Rows[0].Field<DateTime?>("qmpQuoteDate");
			eRPQuoteInformationDto.qmpQuoteFooterMessageRTF = dataTable.Rows[0].Field<string>("qmpQuoteFooterMessageRTF");
			eRPQuoteInformationDto.qmpQuoteFooterMessageText = dataTable.Rows[0].Field<string>("qmpQuoteFooterMessageText");
			eRPQuoteInformationDto.qmpQuoteHeaderMessageRTF = dataTable.Rows[0].Field<string>("qmpQuoteHeaderMessageRTF");
			eRPQuoteInformationDto.qmpQuoteHeaderMessageText = dataTable.Rows[0].Field<string>("qmpQuoteHeaderMessageText");
			eRPQuoteInformationDto.qmpQuoteLocationID = dataTable.Rows[0].Field<string>("qmpQuoteLocationID");
			eRPQuoteInformationDto.qmpQuoterEmployeeID = dataTable.Rows[0].Field<string>("qmpQuoterEmployeeID");
			eRPQuoteInformationDto.qmpRowVersion = dataTable.Rows[0].Field<byte[]>("qmpRowVersion");
			eRPQuoteInformationDto.qmpShipContactID = dataTable.Rows[0].Field<string>("qmpShipContactID");
			eRPQuoteInformationDto.qmpShipLocationID = dataTable.Rows[0].Field<string>("qmpShipLocationID");
			eRPQuoteInformationDto.qmpShipOrganizationID = dataTable.Rows[0].Field<string>("qmpShipOrganizationID");
			eRPQuoteInformationDto.qmpShippingMethodID = dataTable.Rows[0].Field<string>("qmpShippingMethodID");
			eRPQuoteInformationDto.qmpShippingPaymentTypeID = dataTable.Rows[0].Field<string>("qmpShippingPaymentTypeID");
			eRPQuoteInformationDto.qmpSplitPercentTotal = dataTable.Rows[0].Field<decimal>("qmpSplitPercentTotal");
			eRPQuoteInformationDto.qmpStandardMessageID = dataTable.Rows[0].Field<string>("qmpStandardMessageID");
			eRPQuoteInformationDto.qmpTaxDate = dataTable.Rows[0].Field<DateTime?>("qmpTaxDate");
			eRPQuoteInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPQuoteInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPQuoteInformationDto);
	}

	public Task<APIValidationInfoDto> SaveQuote(ERPQuoteDto quote)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM Quotes WHERE qmpUniqueID = " + M1Util.ConvertToLinq(quote.qmpUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["qmpQuoteID"] = quote.qmpQuoteID.ToUpper();
				quote.qmpUniqueID = ((quote.qmpUniqueID == Guid.Empty) ? Guid.NewGuid() : quote.qmpUniqueID);
				dataRow["qmpUniqueID"] = quote.qmpUniqueID;
				dataRow["qmpCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["qmpCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The Quote could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (quote.qmpRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the Quote is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["qmpRowVersion"], quote.qmpRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the Quote has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the Quote again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["qmpArInvoiceContactID"] = quote.qmpArInvoiceContactID;
			dataRow["qmpArInvoiceLocationID"] = quote.qmpArInvoiceLocationID;
			DataRow dataRow2 = dataRow;
			DateTime? qmpClosedDate = quote.qmpClosedDate;
			dataRow2["qmpClosedDate"] = (qmpClosedDate.HasValue ? ((object)qmpClosedDate.GetValueOrDefault()) : dataRow["qmpClosedDate"]);
			dataRow["qmpCurrencyRateID"] = quote.qmpCurrencyRateID;
			dataRow["qmpCustomerOrganizationID"] = quote.qmpCustomerOrganizationID;
			DataRow dataRow3 = dataRow;
			qmpClosedDate = quote.qmpDueDate;
			dataRow3["qmpDueDate"] = (qmpClosedDate.HasValue ? ((object)qmpClosedDate.GetValueOrDefault()) : dataRow["qmpDueDate"]);
			dataRow["qmpExchangeRate"] = quote.qmpExchangeRate;
			DataRow dataRow4 = dataRow;
			qmpClosedDate = quote.qmpExpirationDate;
			dataRow4["qmpExpirationDate"] = (qmpClosedDate.HasValue ? ((object)qmpClosedDate.GetValueOrDefault()) : dataRow["qmpExpirationDate"]);
			dataRow["qmpFreeOnBoardDescription"] = quote.qmpFreeOnBoardDescription;
			dataRow["qmpAvalaraTaxCalculated"] = quote.qmpAvalaraTaxCalculated;
			dataRow["qmpClosed"] = quote.qmpClosed;
			dataRow["qmpCreatedFromMobile"] = quote.qmpCreatedFromMobile;
			dataRow["qmpCustomRate"] = quote.qmpCustomRate;
			dataRow["qmpPaymentTermID"] = quote.qmpPaymentTermID;
			dataRow["qmpPlantDepartmentID"] = quote.qmpPlantDepartmentID;
			dataRow["qmpPlantID"] = quote.qmpPlantID;
			dataRow["qmpProjectID"] = quote.qmpProjectID;
			dataRow["qmpQuoteContactID"] = quote.qmpQuoteContactID;
			DataRow dataRow5 = dataRow;
			qmpClosedDate = quote.qmpQuoteDate;
			dataRow5["qmpQuoteDate"] = (qmpClosedDate.HasValue ? ((object)qmpClosedDate.GetValueOrDefault()) : dataRow["qmpQuoteDate"]);
			dataRow["qmpQuoteFooterMessageRTF"] = quote.qmpQuoteFooterMessageRTF ?? dataRow["qmpQuoteFooterMessageRTF"];
			dataRow["qmpQuoteFooterMessageText"] = quote.qmpQuoteFooterMessageText ?? dataRow["qmpQuoteFooterMessageText"];
			dataRow["qmpQuoteHeaderMessageRTF"] = quote.qmpQuoteHeaderMessageRTF ?? dataRow["qmpQuoteHeaderMessageRTF"];
			dataRow["qmpQuoteHeaderMessageText"] = quote.qmpQuoteHeaderMessageText ?? dataRow["qmpQuoteHeaderMessageText"];
			dataRow["qmpQuoteLocationID"] = quote.qmpQuoteLocationID;
			dataRow["qmpQuoterEmployeeID"] = quote.qmpQuoterEmployeeID;
			dataRow["qmpShipContactID"] = quote.qmpShipContactID;
			dataRow["qmpShipLocationID"] = quote.qmpShipLocationID;
			dataRow["qmpShipOrganizationID"] = quote.qmpShipOrganizationID;
			dataRow["qmpShippingMethodID"] = quote.qmpShippingMethodID;
			dataRow["qmpShippingPaymentTypeID"] = quote.qmpShippingPaymentTypeID;
			dataRow["qmpSplitPercentTotal"] = quote.qmpSplitPercentTotal;
			dataRow["qmpStandardMessageID"] = quote.qmpStandardMessageID;
			DataRow dataRow6 = dataRow;
			qmpClosedDate = quote.qmpTaxDate;
			dataRow6["qmpTaxDate"] = (qmpClosedDate.HasValue ? ((object)qmpClosedDate.GetValueOrDefault()) : dataRow["qmpTaxDate"]);
			if (quote.CustomFields != null && quote.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in quote.CustomFields)
				{
					if (dataTable.Columns.Contains(customField.Key))
					{
						dataRow[customField.Key] = customField.Value;
					}
				}
			}
			dataRow.EndEdit();
			if (flag)
			{
				dataTable.Rows.Add(dataRow);
			}
			if (base.M1database.UpdateData(dataTable, adapter))
			{
				if (flag)
				{
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Created;
				}
				else
				{
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.OK;
				}
			}
			else
			{
				aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.InternalServerError;
			}
		}
		catch (SqlException ex)
		{
			SqlErrorResult httpStatusCodeForSqlException = SqlExceptionMapper.GetHttpStatusCodeForSqlException(ex);
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the Quote [{quote.qmpUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the Quote [{quote.qmpUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
