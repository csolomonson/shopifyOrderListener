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

public class ERPEmployeeMemoRepository : APIBaseRepository, IERPEmployeeMemoRepository, IAPIBaseRepository, IDisposable
{
	public ERPEmployeeMemoRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesEmployeeMemoExist(Guid employeeMemoId)
	{
		InitializeParameterLists();
		base.filterList.Add("lmkUniqueID|C", employeeMemoId);
		base.selectList.Add("lmkUniqueID");
		return Task.FromResult(GetAsObject("EmployeeMemos", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPEmployeeMemoInformationDto>> GetAllEmployeeMemos(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPEmployeeMemoInformationDto> collection = new List<ERPEmployeeMemoInformationDto>();
		InitializeParameterLists();
		string[] array = new string[11]
		{
			"lmkCreatedBy", "lmkCreatedDate", "lmkEmployeeID", "lmkUniqueID", "lmkLongDescriptionRtf", "lmkLongDescriptionText", "lmkMemoDate", "lmkRowVersion", "lmkEmployeeMemoID", "lmkShortDescription",
			"lmkShowInEmployees"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("EmployeeMemos");
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
		using (DataTable dataTable = GetAsDataTable("EmployeeMemos", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPEmployeeMemoInformationDto eRPEmployeeMemoInformationDto = new ERPEmployeeMemoInformationDto();
				eRPEmployeeMemoInformationDto.lmkCreatedBy = dataTable.Rows[i].Field<string>("lmkCreatedBy");
				eRPEmployeeMemoInformationDto.lmkCreatedDate = dataTable.Rows[i].Field<DateTime?>("lmkCreatedDate");
				eRPEmployeeMemoInformationDto.lmkEmployeeID = dataTable.Rows[i].Field<string>("lmkEmployeeID");
				eRPEmployeeMemoInformationDto.lmkUniqueID = dataTable.Rows[i].Field<Guid>("lmkUniqueID");
				eRPEmployeeMemoInformationDto.lmkLongDescriptionRtf = dataTable.Rows[i].Field<string>("lmkLongDescriptionRtf");
				eRPEmployeeMemoInformationDto.lmkLongDescriptionText = dataTable.Rows[i].Field<string>("lmkLongDescriptionText");
				eRPEmployeeMemoInformationDto.lmkMemoDate = dataTable.Rows[i].Field<DateTime?>("lmkMemoDate");
				eRPEmployeeMemoInformationDto.lmkRowVersion = dataTable.Rows[i].Field<byte[]>("lmkRowVersion");
				eRPEmployeeMemoInformationDto.lmkEmployeeMemoID = dataTable.Rows[i].Field<short>("lmkEmployeeMemoID");
				eRPEmployeeMemoInformationDto.lmkShortDescription = dataTable.Rows[i].Field<string>("lmkShortDescription");
				eRPEmployeeMemoInformationDto.lmkShowInEmployees = dataTable.Rows[i].Field<bool>("lmkShowInEmployees");
				eRPEmployeeMemoInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPEmployeeMemoInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPEmployeeMemoInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPEmployeeMemoInformationDto> GetEmployeeMemo(Guid employeeMemoId)
	{
		ERPEmployeeMemoInformationDto eRPEmployeeMemoInformationDto = new ERPEmployeeMemoInformationDto();
		InitializeParameterLists();
		string[] collection = new string[11]
		{
			"lmkCreatedBy", "lmkCreatedDate", "lmkEmployeeID", "lmkUniqueID", "lmkLongDescriptionRtf", "lmkLongDescriptionText", "lmkMemoDate", "lmkRowVersion", "lmkEmployeeMemoID", "lmkShortDescription",
			"lmkShowInEmployees"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("lmkUniqueID|C", employeeMemoId);
		AddCustomFieldsToSelectList("EmployeeMemos");
		using (DataTable dataTable = GetAsDataTable("EmployeeMemos", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPEmployeeMemoInformationDto);
			}
			eRPEmployeeMemoInformationDto.lmkCreatedBy = dataTable.Rows[0].Field<string>("lmkCreatedBy");
			eRPEmployeeMemoInformationDto.lmkCreatedDate = dataTable.Rows[0].Field<DateTime?>("lmkCreatedDate");
			eRPEmployeeMemoInformationDto.lmkEmployeeID = dataTable.Rows[0].Field<string>("lmkEmployeeID");
			eRPEmployeeMemoInformationDto.lmkUniqueID = dataTable.Rows[0].Field<Guid>("lmkUniqueID");
			eRPEmployeeMemoInformationDto.lmkLongDescriptionRtf = dataTable.Rows[0].Field<string>("lmkLongDescriptionRtf");
			eRPEmployeeMemoInformationDto.lmkLongDescriptionText = dataTable.Rows[0].Field<string>("lmkLongDescriptionText");
			eRPEmployeeMemoInformationDto.lmkMemoDate = dataTable.Rows[0].Field<DateTime?>("lmkMemoDate");
			eRPEmployeeMemoInformationDto.lmkRowVersion = dataTable.Rows[0].Field<byte[]>("lmkRowVersion");
			eRPEmployeeMemoInformationDto.lmkEmployeeMemoID = dataTable.Rows[0].Field<short>("lmkEmployeeMemoID");
			eRPEmployeeMemoInformationDto.lmkShortDescription = dataTable.Rows[0].Field<string>("lmkShortDescription");
			eRPEmployeeMemoInformationDto.lmkShowInEmployees = dataTable.Rows[0].Field<bool>("lmkShowInEmployees");
			eRPEmployeeMemoInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPEmployeeMemoInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPEmployeeMemoInformationDto);
	}

	public Task<APIValidationInfoDto> SaveEmployeeMemo(ERPEmployeeMemoDto employeeMemo)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM EmployeeMemos WHERE lmkUniqueID = " + M1Util.ConvertToLinq(employeeMemo.lmkUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["lmkEmployeeID"] = employeeMemo.lmkEmployeeID.ToUpper();
				dataRow["lmkEmployeeMemoID"] = employeeMemo.lmkEmployeeMemoID;
				employeeMemo.lmkUniqueID = ((employeeMemo.lmkUniqueID == Guid.Empty) ? Guid.NewGuid() : employeeMemo.lmkUniqueID);
				dataRow["lmkUniqueID"] = employeeMemo.lmkUniqueID;
				dataRow["lmkCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["lmkCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The EmployeeMemo could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (employeeMemo.lmkRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the EmployeeMemo is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["lmkRowVersion"], employeeMemo.lmkRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the EmployeeMemo has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the EmployeeMemo again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["lmkLongDescriptionRtf"] = employeeMemo.lmkLongDescriptionRtf ?? dataRow["lmkLongDescriptionRtf"];
			dataRow["lmkLongDescriptionText"] = employeeMemo.lmkLongDescriptionText ?? dataRow["lmkLongDescriptionText"];
			DataRow dataRow2 = dataRow;
			DateTime? lmkMemoDate = employeeMemo.lmkMemoDate;
			dataRow2["lmkMemoDate"] = (lmkMemoDate.HasValue ? ((object)lmkMemoDate.GetValueOrDefault()) : dataRow["lmkMemoDate"]);
			dataRow["lmkShortDescription"] = employeeMemo.lmkShortDescription;
			dataRow["lmkShowInEmployees"] = employeeMemo.lmkShowInEmployees;
			if (employeeMemo.CustomFields != null && employeeMemo.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in employeeMemo.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the EmployeeMemo [{employeeMemo.lmkUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the EmployeeMemo [{employeeMemo.lmkUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
