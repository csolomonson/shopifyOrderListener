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

public class ERPReceiptRepository : APIBaseRepository, IERPReceiptRepository, IAPIBaseRepository, IDisposable
{
	public ERPReceiptRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesReceiptExist(Guid receiptId)
	{
		InitializeParameterLists();
		base.filterList.Add("rmpUniqueID|C", receiptId);
		base.selectList.Add("rmpUniqueID");
		return Task.FromResult(GetAsObject("Receipts", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPReceiptInformationDto>> GetAllReceipts(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPReceiptInformationDto> collection = new List<ERPReceiptInformationDto>();
		InitializeParameterLists();
		string[] array = new string[33]
		{
			"rmpApInvoiceContactID", "rmpApInvoiceLocationID", "rmpClosedDate", "rmpReceiptID", "rmpCreatedBy", "rmpCreatedDate", "rmpCurrencyRateID", "rmpDeliveryDocket", "rmpUniqueID", "rmpExchangeRate",
			"rmpFreightCharge", "rmpFreightChargeForeign", "rmpClosed", "rmpCustomRate", "rmpNestlinkProcessed", "rmpPostedToGl", "rmpReversalEntry", "rmpReversed", "rmpLandedCostID", "rmpPlantDepartmentID",
			"rmpPlantID", "rmpPostedDate", "rmpProjectID", "rmpPurchaseContactID", "rmpPurchaseLocationID", "rmpReceiptDate", "rmpReceiptSubtotal", "rmpReceiptSubtotalForeign", "rmpReceiptTotal", "rmpReceiptTotalForeign",
			"rmpRowVersion", "rmpShippingMethodID", "rmpSupplierOrganizationID"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("Receipts");
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
		using (DataTable dataTable = GetAsDataTable("Receipts", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPReceiptInformationDto eRPReceiptInformationDto = new ERPReceiptInformationDto();
				eRPReceiptInformationDto.rmpApInvoiceContactID = dataTable.Rows[i].Field<string>("rmpApInvoiceContactID");
				eRPReceiptInformationDto.rmpApInvoiceLocationID = dataTable.Rows[i].Field<string>("rmpApInvoiceLocationID");
				eRPReceiptInformationDto.rmpClosedDate = dataTable.Rows[i].Field<DateTime?>("rmpClosedDate");
				eRPReceiptInformationDto.rmpReceiptID = dataTable.Rows[i].Field<string>("rmpReceiptID");
				eRPReceiptInformationDto.rmpCreatedBy = dataTable.Rows[i].Field<string>("rmpCreatedBy");
				eRPReceiptInformationDto.rmpCreatedDate = dataTable.Rows[i].Field<DateTime?>("rmpCreatedDate");
				eRPReceiptInformationDto.rmpCurrencyRateID = dataTable.Rows[i].Field<string>("rmpCurrencyRateID");
				eRPReceiptInformationDto.rmpDeliveryDocket = dataTable.Rows[i].Field<string>("rmpDeliveryDocket");
				eRPReceiptInformationDto.rmpUniqueID = dataTable.Rows[i].Field<Guid>("rmpUniqueID");
				eRPReceiptInformationDto.rmpExchangeRate = dataTable.Rows[i].Field<decimal>("rmpExchangeRate");
				eRPReceiptInformationDto.rmpFreightCharge = dataTable.Rows[i].Field<decimal>("rmpFreightCharge");
				eRPReceiptInformationDto.rmpFreightChargeForeign = dataTable.Rows[i].Field<decimal>("rmpFreightChargeForeign");
				eRPReceiptInformationDto.rmpClosed = dataTable.Rows[i].Field<bool>("rmpClosed");
				eRPReceiptInformationDto.rmpCustomRate = dataTable.Rows[i].Field<bool>("rmpCustomRate");
				eRPReceiptInformationDto.rmpNestlinkProcessed = dataTable.Rows[i].Field<bool>("rmpNestlinkProcessed");
				eRPReceiptInformationDto.rmpPostedToGl = dataTable.Rows[i].Field<bool>("rmpPostedToGl");
				eRPReceiptInformationDto.rmpReversalEntry = dataTable.Rows[i].Field<bool>("rmpReversalEntry");
				eRPReceiptInformationDto.rmpReversed = dataTable.Rows[i].Field<bool>("rmpReversed");
				eRPReceiptInformationDto.rmpLandedCostID = dataTable.Rows[i].Field<string>("rmpLandedCostID");
				eRPReceiptInformationDto.rmpPlantDepartmentID = dataTable.Rows[i].Field<string>("rmpPlantDepartmentID");
				eRPReceiptInformationDto.rmpPlantID = dataTable.Rows[i].Field<string>("rmpPlantID");
				eRPReceiptInformationDto.rmpPostedDate = dataTable.Rows[i].Field<DateTime?>("rmpPostedDate");
				eRPReceiptInformationDto.rmpProjectID = dataTable.Rows[i].Field<string>("rmpProjectID");
				eRPReceiptInformationDto.rmpPurchaseContactID = dataTable.Rows[i].Field<string>("rmpPurchaseContactID");
				eRPReceiptInformationDto.rmpPurchaseLocationID = dataTable.Rows[i].Field<string>("rmpPurchaseLocationID");
				eRPReceiptInformationDto.rmpReceiptDate = dataTable.Rows[i].Field<DateTime?>("rmpReceiptDate");
				eRPReceiptInformationDto.rmpReceiptSubtotal = dataTable.Rows[i].Field<decimal>("rmpReceiptSubtotal");
				eRPReceiptInformationDto.rmpReceiptSubtotalForeign = dataTable.Rows[i].Field<decimal>("rmpReceiptSubtotalForeign");
				eRPReceiptInformationDto.rmpReceiptTotal = dataTable.Rows[i].Field<decimal>("rmpReceiptTotal");
				eRPReceiptInformationDto.rmpReceiptTotalForeign = dataTable.Rows[i].Field<decimal>("rmpReceiptTotalForeign");
				eRPReceiptInformationDto.rmpRowVersion = dataTable.Rows[i].Field<byte[]>("rmpRowVersion");
				eRPReceiptInformationDto.rmpShippingMethodID = dataTable.Rows[i].Field<string>("rmpShippingMethodID");
				eRPReceiptInformationDto.rmpSupplierOrganizationID = dataTable.Rows[i].Field<string>("rmpSupplierOrganizationID");
				eRPReceiptInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPReceiptInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPReceiptInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPReceiptInformationDto> GetReceipt(Guid receiptId)
	{
		ERPReceiptInformationDto eRPReceiptInformationDto = new ERPReceiptInformationDto();
		InitializeParameterLists();
		string[] collection = new string[33]
		{
			"rmpApInvoiceContactID", "rmpApInvoiceLocationID", "rmpClosedDate", "rmpReceiptID", "rmpCreatedBy", "rmpCreatedDate", "rmpCurrencyRateID", "rmpDeliveryDocket", "rmpUniqueID", "rmpExchangeRate",
			"rmpFreightCharge", "rmpFreightChargeForeign", "rmpClosed", "rmpCustomRate", "rmpNestlinkProcessed", "rmpPostedToGl", "rmpReversalEntry", "rmpReversed", "rmpLandedCostID", "rmpPlantDepartmentID",
			"rmpPlantID", "rmpPostedDate", "rmpProjectID", "rmpPurchaseContactID", "rmpPurchaseLocationID", "rmpReceiptDate", "rmpReceiptSubtotal", "rmpReceiptSubtotalForeign", "rmpReceiptTotal", "rmpReceiptTotalForeign",
			"rmpRowVersion", "rmpShippingMethodID", "rmpSupplierOrganizationID"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("rmpUniqueID|C", receiptId);
		AddCustomFieldsToSelectList("Receipts");
		using (DataTable dataTable = GetAsDataTable("Receipts", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPReceiptInformationDto);
			}
			eRPReceiptInformationDto.rmpApInvoiceContactID = dataTable.Rows[0].Field<string>("rmpApInvoiceContactID");
			eRPReceiptInformationDto.rmpApInvoiceLocationID = dataTable.Rows[0].Field<string>("rmpApInvoiceLocationID");
			eRPReceiptInformationDto.rmpClosedDate = dataTable.Rows[0].Field<DateTime?>("rmpClosedDate");
			eRPReceiptInformationDto.rmpReceiptID = dataTable.Rows[0].Field<string>("rmpReceiptID");
			eRPReceiptInformationDto.rmpCreatedBy = dataTable.Rows[0].Field<string>("rmpCreatedBy");
			eRPReceiptInformationDto.rmpCreatedDate = dataTable.Rows[0].Field<DateTime?>("rmpCreatedDate");
			eRPReceiptInformationDto.rmpCurrencyRateID = dataTable.Rows[0].Field<string>("rmpCurrencyRateID");
			eRPReceiptInformationDto.rmpDeliveryDocket = dataTable.Rows[0].Field<string>("rmpDeliveryDocket");
			eRPReceiptInformationDto.rmpUniqueID = dataTable.Rows[0].Field<Guid>("rmpUniqueID");
			eRPReceiptInformationDto.rmpExchangeRate = dataTable.Rows[0].Field<decimal>("rmpExchangeRate");
			eRPReceiptInformationDto.rmpFreightCharge = dataTable.Rows[0].Field<decimal>("rmpFreightCharge");
			eRPReceiptInformationDto.rmpFreightChargeForeign = dataTable.Rows[0].Field<decimal>("rmpFreightChargeForeign");
			eRPReceiptInformationDto.rmpClosed = dataTable.Rows[0].Field<bool>("rmpClosed");
			eRPReceiptInformationDto.rmpCustomRate = dataTable.Rows[0].Field<bool>("rmpCustomRate");
			eRPReceiptInformationDto.rmpNestlinkProcessed = dataTable.Rows[0].Field<bool>("rmpNestlinkProcessed");
			eRPReceiptInformationDto.rmpPostedToGl = dataTable.Rows[0].Field<bool>("rmpPostedToGl");
			eRPReceiptInformationDto.rmpReversalEntry = dataTable.Rows[0].Field<bool>("rmpReversalEntry");
			eRPReceiptInformationDto.rmpReversed = dataTable.Rows[0].Field<bool>("rmpReversed");
			eRPReceiptInformationDto.rmpLandedCostID = dataTable.Rows[0].Field<string>("rmpLandedCostID");
			eRPReceiptInformationDto.rmpPlantDepartmentID = dataTable.Rows[0].Field<string>("rmpPlantDepartmentID");
			eRPReceiptInformationDto.rmpPlantID = dataTable.Rows[0].Field<string>("rmpPlantID");
			eRPReceiptInformationDto.rmpPostedDate = dataTable.Rows[0].Field<DateTime?>("rmpPostedDate");
			eRPReceiptInformationDto.rmpProjectID = dataTable.Rows[0].Field<string>("rmpProjectID");
			eRPReceiptInformationDto.rmpPurchaseContactID = dataTable.Rows[0].Field<string>("rmpPurchaseContactID");
			eRPReceiptInformationDto.rmpPurchaseLocationID = dataTable.Rows[0].Field<string>("rmpPurchaseLocationID");
			eRPReceiptInformationDto.rmpReceiptDate = dataTable.Rows[0].Field<DateTime?>("rmpReceiptDate");
			eRPReceiptInformationDto.rmpReceiptSubtotal = dataTable.Rows[0].Field<decimal>("rmpReceiptSubtotal");
			eRPReceiptInformationDto.rmpReceiptSubtotalForeign = dataTable.Rows[0].Field<decimal>("rmpReceiptSubtotalForeign");
			eRPReceiptInformationDto.rmpReceiptTotal = dataTable.Rows[0].Field<decimal>("rmpReceiptTotal");
			eRPReceiptInformationDto.rmpReceiptTotalForeign = dataTable.Rows[0].Field<decimal>("rmpReceiptTotalForeign");
			eRPReceiptInformationDto.rmpRowVersion = dataTable.Rows[0].Field<byte[]>("rmpRowVersion");
			eRPReceiptInformationDto.rmpShippingMethodID = dataTable.Rows[0].Field<string>("rmpShippingMethodID");
			eRPReceiptInformationDto.rmpSupplierOrganizationID = dataTable.Rows[0].Field<string>("rmpSupplierOrganizationID");
			eRPReceiptInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPReceiptInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPReceiptInformationDto);
	}

	public Task<APIValidationInfoDto> SaveReceipt(ERPReceiptDto receipt)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM Receipts WHERE rmpUniqueID = " + M1Util.ConvertToLinq(receipt.rmpUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["rmpReceiptID"] = receipt.rmpReceiptID.ToUpper();
				receipt.rmpUniqueID = ((receipt.rmpUniqueID == Guid.Empty) ? Guid.NewGuid() : receipt.rmpUniqueID);
				dataRow["rmpUniqueID"] = receipt.rmpUniqueID;
				dataRow["rmpCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["rmpCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The Receipt could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (receipt.rmpRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the Receipt is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["rmpRowVersion"], receipt.rmpRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the Receipt has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the Receipt again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["rmpApInvoiceContactID"] = receipt.rmpApInvoiceContactID;
			dataRow["rmpApInvoiceLocationID"] = receipt.rmpApInvoiceLocationID;
			DataRow dataRow2 = dataRow;
			DateTime? rmpClosedDate = receipt.rmpClosedDate;
			dataRow2["rmpClosedDate"] = (rmpClosedDate.HasValue ? ((object)rmpClosedDate.GetValueOrDefault()) : dataRow["rmpClosedDate"]);
			dataRow["rmpCurrencyRateID"] = receipt.rmpCurrencyRateID;
			dataRow["rmpDeliveryDocket"] = receipt.rmpDeliveryDocket;
			dataRow["rmpExchangeRate"] = receipt.rmpExchangeRate;
			dataRow["rmpFreightCharge"] = receipt.rmpFreightCharge;
			dataRow["rmpFreightChargeForeign"] = receipt.rmpFreightChargeForeign;
			dataRow["rmpClosed"] = receipt.rmpClosed;
			dataRow["rmpCustomRate"] = receipt.rmpCustomRate;
			dataRow["rmpNestlinkProcessed"] = receipt.rmpNestlinkProcessed;
			dataRow["rmpPostedToGl"] = receipt.rmpPostedToGl;
			dataRow["rmpReversalEntry"] = receipt.rmpReversalEntry;
			dataRow["rmpReversed"] = receipt.rmpReversed;
			dataRow["rmpLandedCostID"] = receipt.rmpLandedCostID;
			dataRow["rmpPlantDepartmentID"] = receipt.rmpPlantDepartmentID;
			dataRow["rmpPlantID"] = receipt.rmpPlantID;
			DataRow dataRow3 = dataRow;
			rmpClosedDate = receipt.rmpPostedDate;
			dataRow3["rmpPostedDate"] = (rmpClosedDate.HasValue ? ((object)rmpClosedDate.GetValueOrDefault()) : dataRow["rmpPostedDate"]);
			dataRow["rmpProjectID"] = receipt.rmpProjectID;
			dataRow["rmpPurchaseContactID"] = receipt.rmpPurchaseContactID;
			dataRow["rmpPurchaseLocationID"] = receipt.rmpPurchaseLocationID;
			DataRow dataRow4 = dataRow;
			rmpClosedDate = receipt.rmpReceiptDate;
			dataRow4["rmpReceiptDate"] = (rmpClosedDate.HasValue ? ((object)rmpClosedDate.GetValueOrDefault()) : dataRow["rmpReceiptDate"]);
			dataRow["rmpReceiptSubtotal"] = receipt.rmpReceiptSubtotal;
			dataRow["rmpReceiptSubtotalForeign"] = receipt.rmpReceiptSubtotalForeign;
			dataRow["rmpReceiptTotal"] = receipt.rmpReceiptTotal;
			dataRow["rmpReceiptTotalForeign"] = receipt.rmpReceiptTotalForeign;
			dataRow["rmpShippingMethodID"] = receipt.rmpShippingMethodID;
			dataRow["rmpSupplierOrganizationID"] = receipt.rmpSupplierOrganizationID;
			if (receipt.CustomFields != null && receipt.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in receipt.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the Receipt [{receipt.rmpUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the Receipt [{receipt.rmpUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
