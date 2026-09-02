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

public class ERPEmployeeQAApprovalRepository : APIBaseRepository, IERPEmployeeQAApprovalRepository, IAPIBaseRepository, IDisposable
{
	public ERPEmployeeQAApprovalRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesEmployeeQAApprovalExist(Guid employeeQAApprovalId)
	{
		InitializeParameterLists();
		base.filterList.Add("lmbUniqueID|C", employeeQAApprovalId);
		base.selectList.Add("lmbUniqueID");
		return Task.FromResult(GetAsObject("EmployeeQAApprovals", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPEmployeeQAApprovalInformationDto>> GetAllEmployeeQAApprovals(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPEmployeeQAApprovalInformationDto> collection = new List<ERPEmployeeQAApprovalInformationDto>();
		InitializeParameterLists();
		string[] array = new string[7] { "lmbApprovalEmployeeID", "lmbCreatedBy", "lmbCreatedDate", "lmbEmployeeID", "lmbUniqueID", "lmbRowVersion", "lmbEmployeeQAApprovalID" };
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("EmployeeQAApprovals");
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
		using (DataTable dataTable = GetAsDataTable("EmployeeQAApprovals", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPEmployeeQAApprovalInformationDto eRPEmployeeQAApprovalInformationDto = new ERPEmployeeQAApprovalInformationDto();
				eRPEmployeeQAApprovalInformationDto.lmbApprovalEmployeeID = dataTable.Rows[i].Field<string>("lmbApprovalEmployeeID");
				eRPEmployeeQAApprovalInformationDto.lmbCreatedBy = dataTable.Rows[i].Field<string>("lmbCreatedBy");
				eRPEmployeeQAApprovalInformationDto.lmbCreatedDate = dataTable.Rows[i].Field<DateTime?>("lmbCreatedDate");
				eRPEmployeeQAApprovalInformationDto.lmbEmployeeID = dataTable.Rows[i].Field<string>("lmbEmployeeID");
				eRPEmployeeQAApprovalInformationDto.lmbUniqueID = dataTable.Rows[i].Field<Guid>("lmbUniqueID");
				eRPEmployeeQAApprovalInformationDto.lmbRowVersion = dataTable.Rows[i].Field<byte[]>("lmbRowVersion");
				eRPEmployeeQAApprovalInformationDto.lmbEmployeeQAApprovalID = dataTable.Rows[i].Field<byte>("lmbEmployeeQAApprovalID");
				eRPEmployeeQAApprovalInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPEmployeeQAApprovalInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPEmployeeQAApprovalInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPEmployeeQAApprovalInformationDto> GetEmployeeQAApproval(Guid employeeQAApprovalId)
	{
		ERPEmployeeQAApprovalInformationDto eRPEmployeeQAApprovalInformationDto = new ERPEmployeeQAApprovalInformationDto();
		InitializeParameterLists();
		string[] collection = new string[7] { "lmbApprovalEmployeeID", "lmbCreatedBy", "lmbCreatedDate", "lmbEmployeeID", "lmbUniqueID", "lmbRowVersion", "lmbEmployeeQAApprovalID" };
		base.selectList.AddRange(collection);
		base.filterList.Add("lmbUniqueID|C", employeeQAApprovalId);
		AddCustomFieldsToSelectList("EmployeeQAApprovals");
		using (DataTable dataTable = GetAsDataTable("EmployeeQAApprovals", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPEmployeeQAApprovalInformationDto);
			}
			eRPEmployeeQAApprovalInformationDto.lmbApprovalEmployeeID = dataTable.Rows[0].Field<string>("lmbApprovalEmployeeID");
			eRPEmployeeQAApprovalInformationDto.lmbCreatedBy = dataTable.Rows[0].Field<string>("lmbCreatedBy");
			eRPEmployeeQAApprovalInformationDto.lmbCreatedDate = dataTable.Rows[0].Field<DateTime?>("lmbCreatedDate");
			eRPEmployeeQAApprovalInformationDto.lmbEmployeeID = dataTable.Rows[0].Field<string>("lmbEmployeeID");
			eRPEmployeeQAApprovalInformationDto.lmbUniqueID = dataTable.Rows[0].Field<Guid>("lmbUniqueID");
			eRPEmployeeQAApprovalInformationDto.lmbRowVersion = dataTable.Rows[0].Field<byte[]>("lmbRowVersion");
			eRPEmployeeQAApprovalInformationDto.lmbEmployeeQAApprovalID = dataTable.Rows[0].Field<byte>("lmbEmployeeQAApprovalID");
			eRPEmployeeQAApprovalInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPEmployeeQAApprovalInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPEmployeeQAApprovalInformationDto);
	}

	public Task<APIValidationInfoDto> SaveEmployeeQAApproval(ERPEmployeeQAApprovalDto employeeQAApproval)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM EmployeeQAApprovals WHERE lmbUniqueID = " + M1Util.ConvertToLinq(employeeQAApproval.lmbUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["lmbEmployeeID"] = employeeQAApproval.lmbEmployeeID.ToUpper();
				dataRow["lmbApprovalEmployeeID"] = employeeQAApproval.lmbApprovalEmployeeID.ToUpper();
				employeeQAApproval.lmbUniqueID = ((employeeQAApproval.lmbUniqueID == Guid.Empty) ? Guid.NewGuid() : employeeQAApproval.lmbUniqueID);
				dataRow["lmbUniqueID"] = employeeQAApproval.lmbUniqueID;
				dataRow["lmbCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["lmbCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The EmployeeQAApproval could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (employeeQAApproval.lmbRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the EmployeeQAApproval is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["lmbRowVersion"], employeeQAApproval.lmbRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the EmployeeQAApproval has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the EmployeeQAApproval again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["lmbEmployeeQAApprovalID"] = employeeQAApproval.lmbEmployeeQAApprovalID;
			if (employeeQAApproval.CustomFields != null && employeeQAApproval.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in employeeQAApproval.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the EmployeeQAApproval [{employeeQAApproval.lmbUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the EmployeeQAApproval [{employeeQAApproval.lmbUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
