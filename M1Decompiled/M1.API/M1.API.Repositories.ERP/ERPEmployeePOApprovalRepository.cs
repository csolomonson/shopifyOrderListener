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

public class ERPEmployeePOApprovalRepository : APIBaseRepository, IERPEmployeePOApprovalRepository, IAPIBaseRepository, IDisposable
{
	public ERPEmployeePOApprovalRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesEmployeePOApprovalExist(Guid employeePOApprovalId)
	{
		InitializeParameterLists();
		base.filterList.Add("lmhUniqueID|C", employeePOApprovalId);
		base.selectList.Add("lmhUniqueID");
		return Task.FromResult(GetAsObject("EmployeePOApprovals", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPEmployeePOApprovalInformationDto>> GetAllEmployeePOApprovals(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPEmployeePOApprovalInformationDto> collection = new List<ERPEmployeePOApprovalInformationDto>();
		InitializeParameterLists();
		string[] array = new string[7] { "lmhApprovalEmployeeID", "lmhCreatedBy", "lmhCreatedDate", "lmhEmployeeID", "lmhUniqueID", "lmhRowVersion", "lmhEmployeePoApprovalID" };
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("EmployeePOApprovals");
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
		using (DataTable dataTable = GetAsDataTable("EmployeePOApprovals", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPEmployeePOApprovalInformationDto eRPEmployeePOApprovalInformationDto = new ERPEmployeePOApprovalInformationDto();
				eRPEmployeePOApprovalInformationDto.lmhApprovalEmployeeID = dataTable.Rows[i].Field<string>("lmhApprovalEmployeeID");
				eRPEmployeePOApprovalInformationDto.lmhCreatedBy = dataTable.Rows[i].Field<string>("lmhCreatedBy");
				eRPEmployeePOApprovalInformationDto.lmhCreatedDate = dataTable.Rows[i].Field<DateTime?>("lmhCreatedDate");
				eRPEmployeePOApprovalInformationDto.lmhEmployeeID = dataTable.Rows[i].Field<string>("lmhEmployeeID");
				eRPEmployeePOApprovalInformationDto.lmhUniqueID = dataTable.Rows[i].Field<Guid>("lmhUniqueID");
				eRPEmployeePOApprovalInformationDto.lmhRowVersion = dataTable.Rows[i].Field<byte[]>("lmhRowVersion");
				eRPEmployeePOApprovalInformationDto.lmhEmployeePoApprovalID = dataTable.Rows[i].Field<byte>("lmhEmployeePoApprovalID");
				eRPEmployeePOApprovalInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPEmployeePOApprovalInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPEmployeePOApprovalInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPEmployeePOApprovalInformationDto> GetEmployeePOApproval(Guid employeePOApprovalId)
	{
		ERPEmployeePOApprovalInformationDto eRPEmployeePOApprovalInformationDto = new ERPEmployeePOApprovalInformationDto();
		InitializeParameterLists();
		string[] collection = new string[7] { "lmhApprovalEmployeeID", "lmhCreatedBy", "lmhCreatedDate", "lmhEmployeeID", "lmhUniqueID", "lmhRowVersion", "lmhEmployeePoApprovalID" };
		base.selectList.AddRange(collection);
		base.filterList.Add("lmhUniqueID|C", employeePOApprovalId);
		AddCustomFieldsToSelectList("EmployeePOApprovals");
		using (DataTable dataTable = GetAsDataTable("EmployeePOApprovals", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPEmployeePOApprovalInformationDto);
			}
			eRPEmployeePOApprovalInformationDto.lmhApprovalEmployeeID = dataTable.Rows[0].Field<string>("lmhApprovalEmployeeID");
			eRPEmployeePOApprovalInformationDto.lmhCreatedBy = dataTable.Rows[0].Field<string>("lmhCreatedBy");
			eRPEmployeePOApprovalInformationDto.lmhCreatedDate = dataTable.Rows[0].Field<DateTime?>("lmhCreatedDate");
			eRPEmployeePOApprovalInformationDto.lmhEmployeeID = dataTable.Rows[0].Field<string>("lmhEmployeeID");
			eRPEmployeePOApprovalInformationDto.lmhUniqueID = dataTable.Rows[0].Field<Guid>("lmhUniqueID");
			eRPEmployeePOApprovalInformationDto.lmhRowVersion = dataTable.Rows[0].Field<byte[]>("lmhRowVersion");
			eRPEmployeePOApprovalInformationDto.lmhEmployeePoApprovalID = dataTable.Rows[0].Field<byte>("lmhEmployeePoApprovalID");
			eRPEmployeePOApprovalInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPEmployeePOApprovalInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPEmployeePOApprovalInformationDto);
	}

	public Task<APIValidationInfoDto> SaveEmployeePOApproval(ERPEmployeePOApprovalDto employeePOApproval)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM EmployeePOApprovals WHERE lmhUniqueID = " + M1Util.ConvertToLinq(employeePOApproval.lmhUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["lmhEmployeeID"] = employeePOApproval.lmhEmployeeID.ToUpper();
				dataRow["lmhApprovalEmployeeID"] = employeePOApproval.lmhApprovalEmployeeID.ToUpper();
				employeePOApproval.lmhUniqueID = ((employeePOApproval.lmhUniqueID == Guid.Empty) ? Guid.NewGuid() : employeePOApproval.lmhUniqueID);
				dataRow["lmhUniqueID"] = employeePOApproval.lmhUniqueID;
				dataRow["lmhCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["lmhCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The EmployeePOApproval could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (employeePOApproval.lmhRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the EmployeePOApproval is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["lmhRowVersion"], employeePOApproval.lmhRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the EmployeePOApproval has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the EmployeePOApproval again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["lmhEmployeePoApprovalID"] = employeePOApproval.lmhEmployeePoApprovalID;
			if (employeePOApproval.CustomFields != null && employeePOApproval.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in employeePOApproval.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the EmployeePOApproval [{employeePOApproval.lmhUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the EmployeePOApproval [{employeePOApproval.lmhUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
