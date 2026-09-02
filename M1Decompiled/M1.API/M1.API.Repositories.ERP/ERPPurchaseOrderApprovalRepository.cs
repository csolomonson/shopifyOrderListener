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

public class ERPPurchaseOrderApprovalRepository : APIBaseRepository, IERPPurchaseOrderApprovalRepository, IAPIBaseRepository, IDisposable
{
	public ERPPurchaseOrderApprovalRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesPurchaseOrderApprovalExist(Guid purchaseOrderApprovalId)
	{
		InitializeParameterLists();
		base.filterList.Add("pmaUniqueID|C", purchaseOrderApprovalId);
		base.selectList.Add("pmaUniqueID");
		return Task.FromResult(GetAsObject("PurchaseOrderApprovals", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPPurchaseOrderApprovalInformationDto>> GetAllPurchaseOrderApprovals(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPPurchaseOrderApprovalInformationDto> collection = new List<ERPPurchaseOrderApprovalInformationDto>();
		InitializeParameterLists();
		string[] array = new string[10] { "pmaApprovalEmployeeID", "pmaCreatedBy", "pmaCreatedDate", "pmaDescription", "pmaUniqueID", "pmaPurchaseOrderID", "pmaRowVersion", "pmaPurchaseOrderApprovalID", "pmaStatus", "pmaStatusDate" };
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("PurchaseOrderApprovals");
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
		using (DataTable dataTable = GetAsDataTable("PurchaseOrderApprovals", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPPurchaseOrderApprovalInformationDto eRPPurchaseOrderApprovalInformationDto = new ERPPurchaseOrderApprovalInformationDto();
				eRPPurchaseOrderApprovalInformationDto.pmaApprovalEmployeeID = dataTable.Rows[i].Field<string>("pmaApprovalEmployeeID");
				eRPPurchaseOrderApprovalInformationDto.pmaCreatedBy = dataTable.Rows[i].Field<string>("pmaCreatedBy");
				eRPPurchaseOrderApprovalInformationDto.pmaCreatedDate = dataTable.Rows[i].Field<DateTime?>("pmaCreatedDate");
				eRPPurchaseOrderApprovalInformationDto.pmaDescription = dataTable.Rows[i].Field<string>("pmaDescription");
				eRPPurchaseOrderApprovalInformationDto.pmaUniqueID = dataTable.Rows[i].Field<Guid>("pmaUniqueID");
				eRPPurchaseOrderApprovalInformationDto.pmaPurchaseOrderID = dataTable.Rows[i].Field<string>("pmaPurchaseOrderID");
				eRPPurchaseOrderApprovalInformationDto.pmaRowVersion = dataTable.Rows[i].Field<byte[]>("pmaRowVersion");
				eRPPurchaseOrderApprovalInformationDto.pmaPurchaseOrderApprovalID = dataTable.Rows[i].Field<byte>("pmaPurchaseOrderApprovalID");
				eRPPurchaseOrderApprovalInformationDto.pmaStatus = dataTable.Rows[i].Field<byte>("pmaStatus");
				eRPPurchaseOrderApprovalInformationDto.pmaStatusDate = dataTable.Rows[i].Field<DateTime?>("pmaStatusDate");
				eRPPurchaseOrderApprovalInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPPurchaseOrderApprovalInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPPurchaseOrderApprovalInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPPurchaseOrderApprovalInformationDto> GetPurchaseOrderApproval(Guid purchaseOrderApprovalId)
	{
		ERPPurchaseOrderApprovalInformationDto eRPPurchaseOrderApprovalInformationDto = new ERPPurchaseOrderApprovalInformationDto();
		InitializeParameterLists();
		string[] collection = new string[10] { "pmaApprovalEmployeeID", "pmaCreatedBy", "pmaCreatedDate", "pmaDescription", "pmaUniqueID", "pmaPurchaseOrderID", "pmaRowVersion", "pmaPurchaseOrderApprovalID", "pmaStatus", "pmaStatusDate" };
		base.selectList.AddRange(collection);
		base.filterList.Add("pmaUniqueID|C", purchaseOrderApprovalId);
		AddCustomFieldsToSelectList("PurchaseOrderApprovals");
		using (DataTable dataTable = GetAsDataTable("PurchaseOrderApprovals", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPPurchaseOrderApprovalInformationDto);
			}
			eRPPurchaseOrderApprovalInformationDto.pmaApprovalEmployeeID = dataTable.Rows[0].Field<string>("pmaApprovalEmployeeID");
			eRPPurchaseOrderApprovalInformationDto.pmaCreatedBy = dataTable.Rows[0].Field<string>("pmaCreatedBy");
			eRPPurchaseOrderApprovalInformationDto.pmaCreatedDate = dataTable.Rows[0].Field<DateTime?>("pmaCreatedDate");
			eRPPurchaseOrderApprovalInformationDto.pmaDescription = dataTable.Rows[0].Field<string>("pmaDescription");
			eRPPurchaseOrderApprovalInformationDto.pmaUniqueID = dataTable.Rows[0].Field<Guid>("pmaUniqueID");
			eRPPurchaseOrderApprovalInformationDto.pmaPurchaseOrderID = dataTable.Rows[0].Field<string>("pmaPurchaseOrderID");
			eRPPurchaseOrderApprovalInformationDto.pmaRowVersion = dataTable.Rows[0].Field<byte[]>("pmaRowVersion");
			eRPPurchaseOrderApprovalInformationDto.pmaPurchaseOrderApprovalID = dataTable.Rows[0].Field<byte>("pmaPurchaseOrderApprovalID");
			eRPPurchaseOrderApprovalInformationDto.pmaStatus = dataTable.Rows[0].Field<byte>("pmaStatus");
			eRPPurchaseOrderApprovalInformationDto.pmaStatusDate = dataTable.Rows[0].Field<DateTime?>("pmaStatusDate");
			eRPPurchaseOrderApprovalInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPPurchaseOrderApprovalInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPPurchaseOrderApprovalInformationDto);
	}

	public Task<APIValidationInfoDto> SavePurchaseOrderApproval(ERPPurchaseOrderApprovalDto purchaseOrderApproval)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM PurchaseOrderApprovals WHERE pmaUniqueID = " + M1Util.ConvertToLinq(purchaseOrderApproval.pmaUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["pmaPurchaseOrderID"] = purchaseOrderApproval.pmaPurchaseOrderID.ToUpper();
				dataRow["pmaApprovalEmployeeID"] = purchaseOrderApproval.pmaApprovalEmployeeID.ToUpper();
				purchaseOrderApproval.pmaUniqueID = ((purchaseOrderApproval.pmaUniqueID == Guid.Empty) ? Guid.NewGuid() : purchaseOrderApproval.pmaUniqueID);
				dataRow["pmaUniqueID"] = purchaseOrderApproval.pmaUniqueID;
				dataRow["pmaCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["pmaCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The PurchaseOrderApproval could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (purchaseOrderApproval.pmaRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the PurchaseOrderApproval is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["pmaRowVersion"], purchaseOrderApproval.pmaRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the PurchaseOrderApproval has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the PurchaseOrderApproval again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["pmaDescription"] = purchaseOrderApproval.pmaDescription;
			dataRow["pmaPurchaseOrderApprovalID"] = purchaseOrderApproval.pmaPurchaseOrderApprovalID;
			dataRow["pmaStatus"] = purchaseOrderApproval.pmaStatus;
			DataRow dataRow2 = dataRow;
			DateTime? pmaStatusDate = purchaseOrderApproval.pmaStatusDate;
			dataRow2["pmaStatusDate"] = (pmaStatusDate.HasValue ? ((object)pmaStatusDate.GetValueOrDefault()) : dataRow["pmaStatusDate"]);
			if (purchaseOrderApproval.CustomFields != null && purchaseOrderApproval.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in purchaseOrderApproval.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the PurchaseOrderApproval [{purchaseOrderApproval.pmaUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the PurchaseOrderApproval [{purchaseOrderApproval.pmaUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
