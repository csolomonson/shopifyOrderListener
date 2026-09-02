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

public class ERPDMRClaimRepository : APIBaseRepository, IERPDMRClaimRepository, IAPIBaseRepository, IDisposable
{
	public ERPDMRClaimRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesDMRClaimExist(Guid dMRClaimId)
	{
		InitializeParameterLists();
		base.filterList.Add("dmpUniqueID|C", dMRClaimId);
		base.selectList.Add("dmpUniqueID");
		return Task.FromResult(GetAsObject("DMRClaims", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPDMRClaimInformationDto>> GetAllDMRClaims(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPDMRClaimInformationDto> collection = new List<ERPDMRClaimInformationDto>();
		InitializeParameterLists();
		string[] array = new string[28]
		{
			"dmpApInvoiceContactID", "dmpApInvoiceLocationID", "dmpAuthorizationDate", "dmpAuthorizationNumber", "dmpAuthorizedByEmployeeID", "dmpClaimDate", "dmpClaimTotal", "dmpClaimTotalForeign", "dmpClosedDate", "dmpClosedReasonID",
			"dmpDmrClaimID", "dmpCreatedBy", "dmpCreatedDate", "dmpCurrencyRateID", "dmpUniqueID", "dmpExchangeRate", "dmpCustomRate", "dmpPlantDepartmentID", "dmpPlantID", "dmpProcessedByEmployeeID",
			"dmpProjectID", "dmpPurchaseContactID", "dmpPurchaseLocationID", "dmpReference", "dmpRequestedDate", "dmpRowVersion", "dmpStatus", "dmpSupplierOrganizationID"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("DMRClaims");
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
		using (DataTable dataTable = GetAsDataTable("DMRClaims", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPDMRClaimInformationDto eRPDMRClaimInformationDto = new ERPDMRClaimInformationDto();
				eRPDMRClaimInformationDto.dmpApInvoiceContactID = dataTable.Rows[i].Field<string>("dmpApInvoiceContactID");
				eRPDMRClaimInformationDto.dmpApInvoiceLocationID = dataTable.Rows[i].Field<string>("dmpApInvoiceLocationID");
				eRPDMRClaimInformationDto.dmpAuthorizationDate = dataTable.Rows[i].Field<DateTime?>("dmpAuthorizationDate");
				eRPDMRClaimInformationDto.dmpAuthorizationNumber = dataTable.Rows[i].Field<string>("dmpAuthorizationNumber");
				eRPDMRClaimInformationDto.dmpAuthorizedByEmployeeID = dataTable.Rows[i].Field<string>("dmpAuthorizedByEmployeeID");
				eRPDMRClaimInformationDto.dmpClaimDate = dataTable.Rows[i].Field<DateTime?>("dmpClaimDate");
				eRPDMRClaimInformationDto.dmpClaimTotal = dataTable.Rows[i].Field<decimal>("dmpClaimTotal");
				eRPDMRClaimInformationDto.dmpClaimTotalForeign = dataTable.Rows[i].Field<decimal>("dmpClaimTotalForeign");
				eRPDMRClaimInformationDto.dmpClosedDate = dataTable.Rows[i].Field<DateTime?>("dmpClosedDate");
				eRPDMRClaimInformationDto.dmpClosedReasonID = dataTable.Rows[i].Field<string>("dmpClosedReasonID");
				eRPDMRClaimInformationDto.dmpDmrClaimID = dataTable.Rows[i].Field<string>("dmpDmrClaimID");
				eRPDMRClaimInformationDto.dmpCreatedBy = dataTable.Rows[i].Field<string>("dmpCreatedBy");
				eRPDMRClaimInformationDto.dmpCreatedDate = dataTable.Rows[i].Field<DateTime?>("dmpCreatedDate");
				eRPDMRClaimInformationDto.dmpCurrencyRateID = dataTable.Rows[i].Field<string>("dmpCurrencyRateID");
				eRPDMRClaimInformationDto.dmpUniqueID = dataTable.Rows[i].Field<Guid>("dmpUniqueID");
				eRPDMRClaimInformationDto.dmpExchangeRate = dataTable.Rows[i].Field<decimal>("dmpExchangeRate");
				eRPDMRClaimInformationDto.dmpCustomRate = dataTable.Rows[i].Field<bool>("dmpCustomRate");
				eRPDMRClaimInformationDto.dmpPlantDepartmentID = dataTable.Rows[i].Field<string>("dmpPlantDepartmentID");
				eRPDMRClaimInformationDto.dmpPlantID = dataTable.Rows[i].Field<string>("dmpPlantID");
				eRPDMRClaimInformationDto.dmpProcessedByEmployeeID = dataTable.Rows[i].Field<string>("dmpProcessedByEmployeeID");
				eRPDMRClaimInformationDto.dmpProjectID = dataTable.Rows[i].Field<string>("dmpProjectID");
				eRPDMRClaimInformationDto.dmpPurchaseContactID = dataTable.Rows[i].Field<string>("dmpPurchaseContactID");
				eRPDMRClaimInformationDto.dmpPurchaseLocationID = dataTable.Rows[i].Field<string>("dmpPurchaseLocationID");
				eRPDMRClaimInformationDto.dmpReference = dataTable.Rows[i].Field<string>("dmpReference");
				eRPDMRClaimInformationDto.dmpRequestedDate = dataTable.Rows[i].Field<DateTime?>("dmpRequestedDate");
				eRPDMRClaimInformationDto.dmpRowVersion = dataTable.Rows[i].Field<byte[]>("dmpRowVersion");
				eRPDMRClaimInformationDto.dmpStatus = dataTable.Rows[i].Field<string>("dmpStatus");
				eRPDMRClaimInformationDto.dmpSupplierOrganizationID = dataTable.Rows[i].Field<string>("dmpSupplierOrganizationID");
				eRPDMRClaimInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPDMRClaimInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPDMRClaimInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPDMRClaimInformationDto> GetDMRClaim(Guid dMRClaimId)
	{
		ERPDMRClaimInformationDto eRPDMRClaimInformationDto = new ERPDMRClaimInformationDto();
		InitializeParameterLists();
		string[] collection = new string[28]
		{
			"dmpApInvoiceContactID", "dmpApInvoiceLocationID", "dmpAuthorizationDate", "dmpAuthorizationNumber", "dmpAuthorizedByEmployeeID", "dmpClaimDate", "dmpClaimTotal", "dmpClaimTotalForeign", "dmpClosedDate", "dmpClosedReasonID",
			"dmpDmrClaimID", "dmpCreatedBy", "dmpCreatedDate", "dmpCurrencyRateID", "dmpUniqueID", "dmpExchangeRate", "dmpCustomRate", "dmpPlantDepartmentID", "dmpPlantID", "dmpProcessedByEmployeeID",
			"dmpProjectID", "dmpPurchaseContactID", "dmpPurchaseLocationID", "dmpReference", "dmpRequestedDate", "dmpRowVersion", "dmpStatus", "dmpSupplierOrganizationID"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("dmpUniqueID|C", dMRClaimId);
		AddCustomFieldsToSelectList("DMRClaims");
		using (DataTable dataTable = GetAsDataTable("DMRClaims", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPDMRClaimInformationDto);
			}
			eRPDMRClaimInformationDto.dmpApInvoiceContactID = dataTable.Rows[0].Field<string>("dmpApInvoiceContactID");
			eRPDMRClaimInformationDto.dmpApInvoiceLocationID = dataTable.Rows[0].Field<string>("dmpApInvoiceLocationID");
			eRPDMRClaimInformationDto.dmpAuthorizationDate = dataTable.Rows[0].Field<DateTime?>("dmpAuthorizationDate");
			eRPDMRClaimInformationDto.dmpAuthorizationNumber = dataTable.Rows[0].Field<string>("dmpAuthorizationNumber");
			eRPDMRClaimInformationDto.dmpAuthorizedByEmployeeID = dataTable.Rows[0].Field<string>("dmpAuthorizedByEmployeeID");
			eRPDMRClaimInformationDto.dmpClaimDate = dataTable.Rows[0].Field<DateTime?>("dmpClaimDate");
			eRPDMRClaimInformationDto.dmpClaimTotal = dataTable.Rows[0].Field<decimal>("dmpClaimTotal");
			eRPDMRClaimInformationDto.dmpClaimTotalForeign = dataTable.Rows[0].Field<decimal>("dmpClaimTotalForeign");
			eRPDMRClaimInformationDto.dmpClosedDate = dataTable.Rows[0].Field<DateTime?>("dmpClosedDate");
			eRPDMRClaimInformationDto.dmpClosedReasonID = dataTable.Rows[0].Field<string>("dmpClosedReasonID");
			eRPDMRClaimInformationDto.dmpDmrClaimID = dataTable.Rows[0].Field<string>("dmpDmrClaimID");
			eRPDMRClaimInformationDto.dmpCreatedBy = dataTable.Rows[0].Field<string>("dmpCreatedBy");
			eRPDMRClaimInformationDto.dmpCreatedDate = dataTable.Rows[0].Field<DateTime?>("dmpCreatedDate");
			eRPDMRClaimInformationDto.dmpCurrencyRateID = dataTable.Rows[0].Field<string>("dmpCurrencyRateID");
			eRPDMRClaimInformationDto.dmpUniqueID = dataTable.Rows[0].Field<Guid>("dmpUniqueID");
			eRPDMRClaimInformationDto.dmpExchangeRate = dataTable.Rows[0].Field<decimal>("dmpExchangeRate");
			eRPDMRClaimInformationDto.dmpCustomRate = dataTable.Rows[0].Field<bool>("dmpCustomRate");
			eRPDMRClaimInformationDto.dmpPlantDepartmentID = dataTable.Rows[0].Field<string>("dmpPlantDepartmentID");
			eRPDMRClaimInformationDto.dmpPlantID = dataTable.Rows[0].Field<string>("dmpPlantID");
			eRPDMRClaimInformationDto.dmpProcessedByEmployeeID = dataTable.Rows[0].Field<string>("dmpProcessedByEmployeeID");
			eRPDMRClaimInformationDto.dmpProjectID = dataTable.Rows[0].Field<string>("dmpProjectID");
			eRPDMRClaimInformationDto.dmpPurchaseContactID = dataTable.Rows[0].Field<string>("dmpPurchaseContactID");
			eRPDMRClaimInformationDto.dmpPurchaseLocationID = dataTable.Rows[0].Field<string>("dmpPurchaseLocationID");
			eRPDMRClaimInformationDto.dmpReference = dataTable.Rows[0].Field<string>("dmpReference");
			eRPDMRClaimInformationDto.dmpRequestedDate = dataTable.Rows[0].Field<DateTime?>("dmpRequestedDate");
			eRPDMRClaimInformationDto.dmpRowVersion = dataTable.Rows[0].Field<byte[]>("dmpRowVersion");
			eRPDMRClaimInformationDto.dmpStatus = dataTable.Rows[0].Field<string>("dmpStatus");
			eRPDMRClaimInformationDto.dmpSupplierOrganizationID = dataTable.Rows[0].Field<string>("dmpSupplierOrganizationID");
			eRPDMRClaimInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPDMRClaimInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPDMRClaimInformationDto);
	}

	public Task<APIValidationInfoDto> SaveDMRClaim(ERPDMRClaimDto dMRClaim)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM DMRClaims WHERE dmpUniqueID = " + M1Util.ConvertToLinq(dMRClaim.dmpUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["dmpDmrClaimID"] = dMRClaim.dmpDmrClaimID.ToUpper();
				dMRClaim.dmpUniqueID = ((dMRClaim.dmpUniqueID == Guid.Empty) ? Guid.NewGuid() : dMRClaim.dmpUniqueID);
				dataRow["dmpUniqueID"] = dMRClaim.dmpUniqueID;
				dataRow["dmpCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["dmpCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The DMRClaim could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (dMRClaim.dmpRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the DMRClaim is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["dmpRowVersion"], dMRClaim.dmpRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the DMRClaim has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the DMRClaim again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["dmpApInvoiceContactID"] = dMRClaim.dmpApInvoiceContactID;
			dataRow["dmpApInvoiceLocationID"] = dMRClaim.dmpApInvoiceLocationID;
			DataRow dataRow2 = dataRow;
			DateTime? dmpAuthorizationDate = dMRClaim.dmpAuthorizationDate;
			dataRow2["dmpAuthorizationDate"] = (dmpAuthorizationDate.HasValue ? ((object)dmpAuthorizationDate.GetValueOrDefault()) : dataRow["dmpAuthorizationDate"]);
			dataRow["dmpAuthorizationNumber"] = dMRClaim.dmpAuthorizationNumber;
			dataRow["dmpAuthorizedByEmployeeID"] = dMRClaim.dmpAuthorizedByEmployeeID;
			DataRow dataRow3 = dataRow;
			dmpAuthorizationDate = dMRClaim.dmpClaimDate;
			dataRow3["dmpClaimDate"] = (dmpAuthorizationDate.HasValue ? ((object)dmpAuthorizationDate.GetValueOrDefault()) : dataRow["dmpClaimDate"]);
			dataRow["dmpClaimTotal"] = dMRClaim.dmpClaimTotal;
			dataRow["dmpClaimTotalForeign"] = dMRClaim.dmpClaimTotalForeign;
			DataRow dataRow4 = dataRow;
			dmpAuthorizationDate = dMRClaim.dmpClosedDate;
			dataRow4["dmpClosedDate"] = (dmpAuthorizationDate.HasValue ? ((object)dmpAuthorizationDate.GetValueOrDefault()) : dataRow["dmpClosedDate"]);
			dataRow["dmpClosedReasonID"] = dMRClaim.dmpClosedReasonID;
			dataRow["dmpCurrencyRateID"] = dMRClaim.dmpCurrencyRateID;
			dataRow["dmpExchangeRate"] = dMRClaim.dmpExchangeRate;
			dataRow["dmpCustomRate"] = dMRClaim.dmpCustomRate;
			dataRow["dmpPlantDepartmentID"] = dMRClaim.dmpPlantDepartmentID;
			dataRow["dmpPlantID"] = dMRClaim.dmpPlantID;
			dataRow["dmpProcessedByEmployeeID"] = dMRClaim.dmpProcessedByEmployeeID;
			dataRow["dmpProjectID"] = dMRClaim.dmpProjectID;
			dataRow["dmpPurchaseContactID"] = dMRClaim.dmpPurchaseContactID;
			dataRow["dmpPurchaseLocationID"] = dMRClaim.dmpPurchaseLocationID;
			dataRow["dmpReference"] = dMRClaim.dmpReference;
			DataRow dataRow5 = dataRow;
			dmpAuthorizationDate = dMRClaim.dmpRequestedDate;
			dataRow5["dmpRequestedDate"] = (dmpAuthorizationDate.HasValue ? ((object)dmpAuthorizationDate.GetValueOrDefault()) : dataRow["dmpRequestedDate"]);
			dataRow["dmpStatus"] = dMRClaim.dmpStatus;
			dataRow["dmpSupplierOrganizationID"] = dMRClaim.dmpSupplierOrganizationID;
			if (dMRClaim.CustomFields != null && dMRClaim.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in dMRClaim.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the DMRClaim [{dMRClaim.dmpUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the DMRClaim [{dMRClaim.dmpUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
