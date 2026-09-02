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

public class ERPEmployeeSOApprovalRepository : APIBaseRepository, IERPEmployeeSOApprovalRepository, IAPIBaseRepository, IDisposable
{
	public ERPEmployeeSOApprovalRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesEmployeeSOApprovalExist(Guid employeeSOApprovalId)
	{
		InitializeParameterLists();
		base.filterList.Add("lmoUniqueID|C", employeeSOApprovalId);
		base.selectList.Add("lmoUniqueID");
		return Task.FromResult(GetAsObject("EmployeeSOApprovals", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPEmployeeSOApprovalInformationDto>> GetAllEmployeeSOApprovals(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPEmployeeSOApprovalInformationDto> collection = new List<ERPEmployeeSOApprovalInformationDto>();
		InitializeParameterLists();
		string[] array = new string[7] { "lmoApprovalEmployeeID", "lmoCreatedBy", "lmoCreatedDate", "lmoEmployeeID", "lmoUniqueID", "lmoRowVersion", "lmoEmployeeSOApprovalID" };
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("EmployeeSOApprovals");
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
		using (DataTable dataTable = GetAsDataTable("EmployeeSOApprovals", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPEmployeeSOApprovalInformationDto eRPEmployeeSOApprovalInformationDto = new ERPEmployeeSOApprovalInformationDto();
				eRPEmployeeSOApprovalInformationDto.lmoApprovalEmployeeID = dataTable.Rows[i].Field<string>("lmoApprovalEmployeeID");
				eRPEmployeeSOApprovalInformationDto.lmoCreatedBy = dataTable.Rows[i].Field<string>("lmoCreatedBy");
				eRPEmployeeSOApprovalInformationDto.lmoCreatedDate = dataTable.Rows[i].Field<DateTime?>("lmoCreatedDate");
				eRPEmployeeSOApprovalInformationDto.lmoEmployeeID = dataTable.Rows[i].Field<string>("lmoEmployeeID");
				eRPEmployeeSOApprovalInformationDto.lmoUniqueID = dataTable.Rows[i].Field<Guid>("lmoUniqueID");
				eRPEmployeeSOApprovalInformationDto.lmoRowVersion = dataTable.Rows[i].Field<byte[]>("lmoRowVersion");
				eRPEmployeeSOApprovalInformationDto.lmoEmployeeSOApprovalID = dataTable.Rows[i].Field<byte>("lmoEmployeeSOApprovalID");
				eRPEmployeeSOApprovalInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPEmployeeSOApprovalInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPEmployeeSOApprovalInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPEmployeeSOApprovalInformationDto> GetEmployeeSOApproval(Guid employeeSOApprovalId)
	{
		ERPEmployeeSOApprovalInformationDto eRPEmployeeSOApprovalInformationDto = new ERPEmployeeSOApprovalInformationDto();
		InitializeParameterLists();
		string[] collection = new string[7] { "lmoApprovalEmployeeID", "lmoCreatedBy", "lmoCreatedDate", "lmoEmployeeID", "lmoUniqueID", "lmoRowVersion", "lmoEmployeeSOApprovalID" };
		base.selectList.AddRange(collection);
		base.filterList.Add("lmoUniqueID|C", employeeSOApprovalId);
		AddCustomFieldsToSelectList("EmployeeSOApprovals");
		using (DataTable dataTable = GetAsDataTable("EmployeeSOApprovals", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPEmployeeSOApprovalInformationDto);
			}
			eRPEmployeeSOApprovalInformationDto.lmoApprovalEmployeeID = dataTable.Rows[0].Field<string>("lmoApprovalEmployeeID");
			eRPEmployeeSOApprovalInformationDto.lmoCreatedBy = dataTable.Rows[0].Field<string>("lmoCreatedBy");
			eRPEmployeeSOApprovalInformationDto.lmoCreatedDate = dataTable.Rows[0].Field<DateTime?>("lmoCreatedDate");
			eRPEmployeeSOApprovalInformationDto.lmoEmployeeID = dataTable.Rows[0].Field<string>("lmoEmployeeID");
			eRPEmployeeSOApprovalInformationDto.lmoUniqueID = dataTable.Rows[0].Field<Guid>("lmoUniqueID");
			eRPEmployeeSOApprovalInformationDto.lmoRowVersion = dataTable.Rows[0].Field<byte[]>("lmoRowVersion");
			eRPEmployeeSOApprovalInformationDto.lmoEmployeeSOApprovalID = dataTable.Rows[0].Field<byte>("lmoEmployeeSOApprovalID");
			eRPEmployeeSOApprovalInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPEmployeeSOApprovalInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPEmployeeSOApprovalInformationDto);
	}

	public Task<APIValidationInfoDto> SaveEmployeeSOApproval(ERPEmployeeSOApprovalDto employeeSOApproval)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM EmployeeSOApprovals WHERE lmoUniqueID = " + M1Util.ConvertToLinq(employeeSOApproval.lmoUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["lmoEmployeeID"] = employeeSOApproval.lmoEmployeeID.ToUpper();
				dataRow["lmoApprovalEmployeeID"] = employeeSOApproval.lmoApprovalEmployeeID.ToUpper();
				employeeSOApproval.lmoUniqueID = ((employeeSOApproval.lmoUniqueID == Guid.Empty) ? Guid.NewGuid() : employeeSOApproval.lmoUniqueID);
				dataRow["lmoUniqueID"] = employeeSOApproval.lmoUniqueID;
				dataRow["lmoCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["lmoCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The EmployeeSOApproval could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (employeeSOApproval.lmoRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the EmployeeSOApproval is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["lmoRowVersion"], employeeSOApproval.lmoRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the EmployeeSOApproval has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the EmployeeSOApproval again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["lmoEmployeeSOApprovalID"] = employeeSOApproval.lmoEmployeeSOApprovalID;
			if (employeeSOApproval.CustomFields != null && employeeSOApproval.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in employeeSOApproval.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the EmployeeSOApproval [{employeeSOApproval.lmoUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the EmployeeSOApproval [{employeeSOApproval.lmoUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
