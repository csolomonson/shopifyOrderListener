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

public class ERPRMAReceiptRepository : APIBaseRepository, IERPRMAReceiptRepository, IAPIBaseRepository, IDisposable
{
	public ERPRMAReceiptRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesRMAReceiptExist(Guid rMAReceiptId)
	{
		InitializeParameterLists();
		base.filterList.Add("rrpUniqueID|C", rMAReceiptId);
		base.selectList.Add("rrpUniqueID");
		return Task.FromResult(GetAsObject("RMAReceipts", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPRMAReceiptInformationDto>> GetAllRMAReceipts(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPRMAReceiptInformationDto> collection = new List<ERPRMAReceiptInformationDto>();
		InitializeParameterLists();
		string[] array = new string[28]
		{
			"rrpArInvoiceContactID", "rrpArInvoiceLocationID", "rrpClosedDate", "rrpRmaReceiptID", "rrpCreatedBy", "rrpCreatedDate", "rrpCurrencyRateID", "rrpCustomerOrganizationID", "rrpDeliveryDocket", "rrpUniqueID",
			"rrpExchangeRate", "rrpFreightCharge", "rrpFreightChargeForeign", "rrpClosed", "rrpCustomRate", "rrpPosted", "rrpReversalEntry", "rrpReversed", "rrpPlantDepartmentID", "rrpPlantID",
			"rrpPostedDate", "rrpProjectID", "rrpReceiptDate", "rrpRowVersion", "rrpShipContactID", "rrpShipLocationID", "rrpShipOrganizationID", "rrpShippingMethodID"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("RMAReceipts");
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
		using (DataTable dataTable = GetAsDataTable("RMAReceipts", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPRMAReceiptInformationDto eRPRMAReceiptInformationDto = new ERPRMAReceiptInformationDto();
				eRPRMAReceiptInformationDto.rrpArInvoiceContactID = dataTable.Rows[i].Field<string>("rrpArInvoiceContactID");
				eRPRMAReceiptInformationDto.rrpArInvoiceLocationID = dataTable.Rows[i].Field<string>("rrpArInvoiceLocationID");
				eRPRMAReceiptInformationDto.rrpClosedDate = dataTable.Rows[i].Field<DateTime?>("rrpClosedDate");
				eRPRMAReceiptInformationDto.rrpRmaReceiptID = dataTable.Rows[i].Field<string>("rrpRmaReceiptID");
				eRPRMAReceiptInformationDto.rrpCreatedBy = dataTable.Rows[i].Field<string>("rrpCreatedBy");
				eRPRMAReceiptInformationDto.rrpCreatedDate = dataTable.Rows[i].Field<DateTime?>("rrpCreatedDate");
				eRPRMAReceiptInformationDto.rrpCurrencyRateID = dataTable.Rows[i].Field<string>("rrpCurrencyRateID");
				eRPRMAReceiptInformationDto.rrpCustomerOrganizationID = dataTable.Rows[i].Field<string>("rrpCustomerOrganizationID");
				eRPRMAReceiptInformationDto.rrpDeliveryDocket = dataTable.Rows[i].Field<string>("rrpDeliveryDocket");
				eRPRMAReceiptInformationDto.rrpUniqueID = dataTable.Rows[i].Field<Guid>("rrpUniqueID");
				eRPRMAReceiptInformationDto.rrpExchangeRate = dataTable.Rows[i].Field<decimal>("rrpExchangeRate");
				eRPRMAReceiptInformationDto.rrpFreightCharge = dataTable.Rows[i].Field<decimal>("rrpFreightCharge");
				eRPRMAReceiptInformationDto.rrpFreightChargeForeign = dataTable.Rows[i].Field<decimal>("rrpFreightChargeForeign");
				eRPRMAReceiptInformationDto.rrpClosed = dataTable.Rows[i].Field<bool>("rrpClosed");
				eRPRMAReceiptInformationDto.rrpCustomRate = dataTable.Rows[i].Field<bool>("rrpCustomRate");
				eRPRMAReceiptInformationDto.rrpPosted = dataTable.Rows[i].Field<bool>("rrpPosted");
				eRPRMAReceiptInformationDto.rrpReversalEntry = dataTable.Rows[i].Field<bool>("rrpReversalEntry");
				eRPRMAReceiptInformationDto.rrpReversed = dataTable.Rows[i].Field<bool>("rrpReversed");
				eRPRMAReceiptInformationDto.rrpPlantDepartmentID = dataTable.Rows[i].Field<string>("rrpPlantDepartmentID");
				eRPRMAReceiptInformationDto.rrpPlantID = dataTable.Rows[i].Field<string>("rrpPlantID");
				eRPRMAReceiptInformationDto.rrpPostedDate = dataTable.Rows[i].Field<DateTime?>("rrpPostedDate");
				eRPRMAReceiptInformationDto.rrpProjectID = dataTable.Rows[i].Field<string>("rrpProjectID");
				eRPRMAReceiptInformationDto.rrpReceiptDate = dataTable.Rows[i].Field<DateTime?>("rrpReceiptDate");
				eRPRMAReceiptInformationDto.rrpRowVersion = dataTable.Rows[i].Field<byte[]>("rrpRowVersion");
				eRPRMAReceiptInformationDto.rrpShipContactID = dataTable.Rows[i].Field<string>("rrpShipContactID");
				eRPRMAReceiptInformationDto.rrpShipLocationID = dataTable.Rows[i].Field<string>("rrpShipLocationID");
				eRPRMAReceiptInformationDto.rrpShipOrganizationID = dataTable.Rows[i].Field<string>("rrpShipOrganizationID");
				eRPRMAReceiptInformationDto.rrpShippingMethodID = dataTable.Rows[i].Field<string>("rrpShippingMethodID");
				eRPRMAReceiptInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPRMAReceiptInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPRMAReceiptInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPRMAReceiptInformationDto> GetRMAReceipt(Guid rMAReceiptId)
	{
		ERPRMAReceiptInformationDto eRPRMAReceiptInformationDto = new ERPRMAReceiptInformationDto();
		InitializeParameterLists();
		string[] collection = new string[28]
		{
			"rrpArInvoiceContactID", "rrpArInvoiceLocationID", "rrpClosedDate", "rrpRmaReceiptID", "rrpCreatedBy", "rrpCreatedDate", "rrpCurrencyRateID", "rrpCustomerOrganizationID", "rrpDeliveryDocket", "rrpUniqueID",
			"rrpExchangeRate", "rrpFreightCharge", "rrpFreightChargeForeign", "rrpClosed", "rrpCustomRate", "rrpPosted", "rrpReversalEntry", "rrpReversed", "rrpPlantDepartmentID", "rrpPlantID",
			"rrpPostedDate", "rrpProjectID", "rrpReceiptDate", "rrpRowVersion", "rrpShipContactID", "rrpShipLocationID", "rrpShipOrganizationID", "rrpShippingMethodID"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("rrpUniqueID|C", rMAReceiptId);
		AddCustomFieldsToSelectList("RMAReceipts");
		using (DataTable dataTable = GetAsDataTable("RMAReceipts", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPRMAReceiptInformationDto);
			}
			eRPRMAReceiptInformationDto.rrpArInvoiceContactID = dataTable.Rows[0].Field<string>("rrpArInvoiceContactID");
			eRPRMAReceiptInformationDto.rrpArInvoiceLocationID = dataTable.Rows[0].Field<string>("rrpArInvoiceLocationID");
			eRPRMAReceiptInformationDto.rrpClosedDate = dataTable.Rows[0].Field<DateTime?>("rrpClosedDate");
			eRPRMAReceiptInformationDto.rrpRmaReceiptID = dataTable.Rows[0].Field<string>("rrpRmaReceiptID");
			eRPRMAReceiptInformationDto.rrpCreatedBy = dataTable.Rows[0].Field<string>("rrpCreatedBy");
			eRPRMAReceiptInformationDto.rrpCreatedDate = dataTable.Rows[0].Field<DateTime?>("rrpCreatedDate");
			eRPRMAReceiptInformationDto.rrpCurrencyRateID = dataTable.Rows[0].Field<string>("rrpCurrencyRateID");
			eRPRMAReceiptInformationDto.rrpCustomerOrganizationID = dataTable.Rows[0].Field<string>("rrpCustomerOrganizationID");
			eRPRMAReceiptInformationDto.rrpDeliveryDocket = dataTable.Rows[0].Field<string>("rrpDeliveryDocket");
			eRPRMAReceiptInformationDto.rrpUniqueID = dataTable.Rows[0].Field<Guid>("rrpUniqueID");
			eRPRMAReceiptInformationDto.rrpExchangeRate = dataTable.Rows[0].Field<decimal>("rrpExchangeRate");
			eRPRMAReceiptInformationDto.rrpFreightCharge = dataTable.Rows[0].Field<decimal>("rrpFreightCharge");
			eRPRMAReceiptInformationDto.rrpFreightChargeForeign = dataTable.Rows[0].Field<decimal>("rrpFreightChargeForeign");
			eRPRMAReceiptInformationDto.rrpClosed = dataTable.Rows[0].Field<bool>("rrpClosed");
			eRPRMAReceiptInformationDto.rrpCustomRate = dataTable.Rows[0].Field<bool>("rrpCustomRate");
			eRPRMAReceiptInformationDto.rrpPosted = dataTable.Rows[0].Field<bool>("rrpPosted");
			eRPRMAReceiptInformationDto.rrpReversalEntry = dataTable.Rows[0].Field<bool>("rrpReversalEntry");
			eRPRMAReceiptInformationDto.rrpReversed = dataTable.Rows[0].Field<bool>("rrpReversed");
			eRPRMAReceiptInformationDto.rrpPlantDepartmentID = dataTable.Rows[0].Field<string>("rrpPlantDepartmentID");
			eRPRMAReceiptInformationDto.rrpPlantID = dataTable.Rows[0].Field<string>("rrpPlantID");
			eRPRMAReceiptInformationDto.rrpPostedDate = dataTable.Rows[0].Field<DateTime?>("rrpPostedDate");
			eRPRMAReceiptInformationDto.rrpProjectID = dataTable.Rows[0].Field<string>("rrpProjectID");
			eRPRMAReceiptInformationDto.rrpReceiptDate = dataTable.Rows[0].Field<DateTime?>("rrpReceiptDate");
			eRPRMAReceiptInformationDto.rrpRowVersion = dataTable.Rows[0].Field<byte[]>("rrpRowVersion");
			eRPRMAReceiptInformationDto.rrpShipContactID = dataTable.Rows[0].Field<string>("rrpShipContactID");
			eRPRMAReceiptInformationDto.rrpShipLocationID = dataTable.Rows[0].Field<string>("rrpShipLocationID");
			eRPRMAReceiptInformationDto.rrpShipOrganizationID = dataTable.Rows[0].Field<string>("rrpShipOrganizationID");
			eRPRMAReceiptInformationDto.rrpShippingMethodID = dataTable.Rows[0].Field<string>("rrpShippingMethodID");
			eRPRMAReceiptInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPRMAReceiptInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPRMAReceiptInformationDto);
	}

	public Task<APIValidationInfoDto> SaveRMAReceipt(ERPRMAReceiptDto rMAReceipt)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM RMAReceipts WHERE rrpUniqueID = " + M1Util.ConvertToLinq(rMAReceipt.rrpUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["rrpRmaReceiptID"] = rMAReceipt.rrpRmaReceiptID.ToUpper();
				rMAReceipt.rrpUniqueID = ((rMAReceipt.rrpUniqueID == Guid.Empty) ? Guid.NewGuid() : rMAReceipt.rrpUniqueID);
				dataRow["rrpUniqueID"] = rMAReceipt.rrpUniqueID;
				dataRow["rrpCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["rrpCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The RMAReceipt could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (rMAReceipt.rrpRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the RMAReceipt is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["rrpRowVersion"], rMAReceipt.rrpRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the RMAReceipt has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the RMAReceipt again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["rrpArInvoiceContactID"] = rMAReceipt.rrpArInvoiceContactID;
			dataRow["rrpArInvoiceLocationID"] = rMAReceipt.rrpArInvoiceLocationID;
			DataRow dataRow2 = dataRow;
			DateTime? rrpClosedDate = rMAReceipt.rrpClosedDate;
			dataRow2["rrpClosedDate"] = (rrpClosedDate.HasValue ? ((object)rrpClosedDate.GetValueOrDefault()) : dataRow["rrpClosedDate"]);
			dataRow["rrpCurrencyRateID"] = rMAReceipt.rrpCurrencyRateID;
			dataRow["rrpCustomerOrganizationID"] = rMAReceipt.rrpCustomerOrganizationID;
			dataRow["rrpDeliveryDocket"] = rMAReceipt.rrpDeliveryDocket;
			dataRow["rrpExchangeRate"] = rMAReceipt.rrpExchangeRate;
			dataRow["rrpFreightCharge"] = rMAReceipt.rrpFreightCharge;
			dataRow["rrpFreightChargeForeign"] = rMAReceipt.rrpFreightChargeForeign;
			dataRow["rrpClosed"] = rMAReceipt.rrpClosed;
			dataRow["rrpCustomRate"] = rMAReceipt.rrpCustomRate;
			dataRow["rrpPosted"] = rMAReceipt.rrpPosted;
			dataRow["rrpReversalEntry"] = rMAReceipt.rrpReversalEntry;
			dataRow["rrpReversed"] = rMAReceipt.rrpReversed;
			dataRow["rrpPlantDepartmentID"] = rMAReceipt.rrpPlantDepartmentID;
			dataRow["rrpPlantID"] = rMAReceipt.rrpPlantID;
			DataRow dataRow3 = dataRow;
			rrpClosedDate = rMAReceipt.rrpPostedDate;
			dataRow3["rrpPostedDate"] = (rrpClosedDate.HasValue ? ((object)rrpClosedDate.GetValueOrDefault()) : dataRow["rrpPostedDate"]);
			dataRow["rrpProjectID"] = rMAReceipt.rrpProjectID;
			DataRow dataRow4 = dataRow;
			rrpClosedDate = rMAReceipt.rrpReceiptDate;
			dataRow4["rrpReceiptDate"] = (rrpClosedDate.HasValue ? ((object)rrpClosedDate.GetValueOrDefault()) : dataRow["rrpReceiptDate"]);
			dataRow["rrpShipContactID"] = rMAReceipt.rrpShipContactID;
			dataRow["rrpShipLocationID"] = rMAReceipt.rrpShipLocationID;
			dataRow["rrpShipOrganizationID"] = rMAReceipt.rrpShipOrganizationID;
			dataRow["rrpShippingMethodID"] = rMAReceipt.rrpShippingMethodID;
			if (rMAReceipt.CustomFields != null && rMAReceipt.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in rMAReceipt.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the RMAReceipt [{rMAReceipt.rrpUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the RMAReceipt [{rMAReceipt.rrpUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
