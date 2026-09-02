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

public class ERPSalesOrderApprovalRepository : APIBaseRepository, IERPSalesOrderApprovalRepository, IAPIBaseRepository, IDisposable
{
	public ERPSalesOrderApprovalRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesSalesOrderApprovalExist(Guid salesOrderApprovalId)
	{
		InitializeParameterLists();
		base.filterList.Add("omaUniqueID|C", salesOrderApprovalId);
		base.selectList.Add("omaUniqueID");
		return Task.FromResult(GetAsObject("SalesOrderApprovals", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPSalesOrderApprovalInformationDto>> GetAllSalesOrderApprovals(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPSalesOrderApprovalInformationDto> collection = new List<ERPSalesOrderApprovalInformationDto>();
		InitializeParameterLists();
		string[] array = new string[10] { "omaApprovalEmployeeID", "omaCreatedBy", "omaCreatedDate", "omaDescription", "omaUniqueID", "omaRowVersion", "omaSalesOrderID", "omaSalesOrderApprovalID", "omaStatus", "omaStatusDate" };
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("SalesOrderApprovals");
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
		using (DataTable dataTable = GetAsDataTable("SalesOrderApprovals", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPSalesOrderApprovalInformationDto eRPSalesOrderApprovalInformationDto = new ERPSalesOrderApprovalInformationDto();
				eRPSalesOrderApprovalInformationDto.omaApprovalEmployeeID = dataTable.Rows[i].Field<string>("omaApprovalEmployeeID");
				eRPSalesOrderApprovalInformationDto.omaCreatedBy = dataTable.Rows[i].Field<string>("omaCreatedBy");
				eRPSalesOrderApprovalInformationDto.omaCreatedDate = dataTable.Rows[i].Field<DateTime?>("omaCreatedDate");
				eRPSalesOrderApprovalInformationDto.omaDescription = dataTable.Rows[i].Field<string>("omaDescription");
				eRPSalesOrderApprovalInformationDto.omaUniqueID = dataTable.Rows[i].Field<Guid>("omaUniqueID");
				eRPSalesOrderApprovalInformationDto.omaRowVersion = dataTable.Rows[i].Field<byte[]>("omaRowVersion");
				eRPSalesOrderApprovalInformationDto.omaSalesOrderID = dataTable.Rows[i].Field<string>("omaSalesOrderID");
				eRPSalesOrderApprovalInformationDto.omaSalesOrderApprovalID = dataTable.Rows[i].Field<byte>("omaSalesOrderApprovalID");
				eRPSalesOrderApprovalInformationDto.omaStatus = dataTable.Rows[i].Field<byte>("omaStatus");
				eRPSalesOrderApprovalInformationDto.omaStatusDate = dataTable.Rows[i].Field<DateTime?>("omaStatusDate");
				eRPSalesOrderApprovalInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPSalesOrderApprovalInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPSalesOrderApprovalInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPSalesOrderApprovalInformationDto> GetSalesOrderApproval(Guid salesOrderApprovalId)
	{
		ERPSalesOrderApprovalInformationDto eRPSalesOrderApprovalInformationDto = new ERPSalesOrderApprovalInformationDto();
		InitializeParameterLists();
		string[] collection = new string[10] { "omaApprovalEmployeeID", "omaCreatedBy", "omaCreatedDate", "omaDescription", "omaUniqueID", "omaRowVersion", "omaSalesOrderID", "omaSalesOrderApprovalID", "omaStatus", "omaStatusDate" };
		base.selectList.AddRange(collection);
		base.filterList.Add("omaUniqueID|C", salesOrderApprovalId);
		AddCustomFieldsToSelectList("SalesOrderApprovals");
		using (DataTable dataTable = GetAsDataTable("SalesOrderApprovals", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPSalesOrderApprovalInformationDto);
			}
			eRPSalesOrderApprovalInformationDto.omaApprovalEmployeeID = dataTable.Rows[0].Field<string>("omaApprovalEmployeeID");
			eRPSalesOrderApprovalInformationDto.omaCreatedBy = dataTable.Rows[0].Field<string>("omaCreatedBy");
			eRPSalesOrderApprovalInformationDto.omaCreatedDate = dataTable.Rows[0].Field<DateTime?>("omaCreatedDate");
			eRPSalesOrderApprovalInformationDto.omaDescription = dataTable.Rows[0].Field<string>("omaDescription");
			eRPSalesOrderApprovalInformationDto.omaUniqueID = dataTable.Rows[0].Field<Guid>("omaUniqueID");
			eRPSalesOrderApprovalInformationDto.omaRowVersion = dataTable.Rows[0].Field<byte[]>("omaRowVersion");
			eRPSalesOrderApprovalInformationDto.omaSalesOrderID = dataTable.Rows[0].Field<string>("omaSalesOrderID");
			eRPSalesOrderApprovalInformationDto.omaSalesOrderApprovalID = dataTable.Rows[0].Field<byte>("omaSalesOrderApprovalID");
			eRPSalesOrderApprovalInformationDto.omaStatus = dataTable.Rows[0].Field<byte>("omaStatus");
			eRPSalesOrderApprovalInformationDto.omaStatusDate = dataTable.Rows[0].Field<DateTime?>("omaStatusDate");
			eRPSalesOrderApprovalInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPSalesOrderApprovalInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPSalesOrderApprovalInformationDto);
	}

	public Task<APIValidationInfoDto> SaveSalesOrderApproval(ERPSalesOrderApprovalDto salesOrderApproval)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM SalesOrderApprovals WHERE omaUniqueID = " + M1Util.ConvertToLinq(salesOrderApproval.omaUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["omaSalesOrderID"] = salesOrderApproval.omaSalesOrderID.ToUpper();
				dataRow["omaApprovalEmployeeID"] = salesOrderApproval.omaApprovalEmployeeID.ToUpper();
				salesOrderApproval.omaUniqueID = ((salesOrderApproval.omaUniqueID == Guid.Empty) ? Guid.NewGuid() : salesOrderApproval.omaUniqueID);
				dataRow["omaUniqueID"] = salesOrderApproval.omaUniqueID;
				dataRow["omaCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["omaCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The SalesOrderApproval could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (salesOrderApproval.omaRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the SalesOrderApproval is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["omaRowVersion"], salesOrderApproval.omaRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the SalesOrderApproval has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the SalesOrderApproval again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["omaDescription"] = salesOrderApproval.omaDescription;
			dataRow["omaSalesOrderApprovalID"] = salesOrderApproval.omaSalesOrderApprovalID;
			dataRow["omaStatus"] = salesOrderApproval.omaStatus;
			DataRow dataRow2 = dataRow;
			DateTime? omaStatusDate = salesOrderApproval.omaStatusDate;
			dataRow2["omaStatusDate"] = (omaStatusDate.HasValue ? ((object)omaStatusDate.GetValueOrDefault()) : dataRow["omaStatusDate"]);
			if (salesOrderApproval.CustomFields != null && salesOrderApproval.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in salesOrderApproval.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the SalesOrderApproval [{salesOrderApproval.omaUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the SalesOrderApproval [{salesOrderApproval.omaUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
