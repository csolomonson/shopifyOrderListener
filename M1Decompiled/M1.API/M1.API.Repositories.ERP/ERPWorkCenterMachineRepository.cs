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

public class ERPWorkCenterMachineRepository : APIBaseRepository, IERPWorkCenterMachineRepository, IAPIBaseRepository, IDisposable
{
	public ERPWorkCenterMachineRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesWorkCenterMachineExist(Guid workCenterMachineId)
	{
		InitializeParameterLists();
		base.filterList.Add("xaqUniqueID|C", workCenterMachineId);
		base.selectList.Add("xaqUniqueID");
		return Task.FromResult(GetAsObject("WorkCenterMachines", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPWorkCenterMachineInformationDto>> GetAllWorkCenterMachines(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPWorkCenterMachineInformationDto> collection = new List<ERPWorkCenterMachineInformationDto>();
		InitializeParameterLists();
		string[] array = new string[5] { "xaqDescription", "xaqUniqueID", "xaqRowVersion", "xaqWorkCenterMachineID", "xaqWorkCenterID" };
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("WorkCenterMachines");
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
		using (DataTable dataTable = GetAsDataTable("WorkCenterMachines", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPWorkCenterMachineInformationDto eRPWorkCenterMachineInformationDto = new ERPWorkCenterMachineInformationDto();
				eRPWorkCenterMachineInformationDto.xaqDescription = dataTable.Rows[i].Field<string>("xaqDescription");
				eRPWorkCenterMachineInformationDto.xaqUniqueID = dataTable.Rows[i].Field<Guid>("xaqUniqueID");
				eRPWorkCenterMachineInformationDto.xaqRowVersion = dataTable.Rows[i].Field<byte[]>("xaqRowVersion");
				eRPWorkCenterMachineInformationDto.xaqWorkCenterMachineID = dataTable.Rows[i].Field<short>("xaqWorkCenterMachineID");
				eRPWorkCenterMachineInformationDto.xaqWorkCenterID = dataTable.Rows[i].Field<string>("xaqWorkCenterID");
				eRPWorkCenterMachineInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPWorkCenterMachineInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPWorkCenterMachineInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPWorkCenterMachineInformationDto> GetWorkCenterMachine(Guid workCenterMachineId)
	{
		ERPWorkCenterMachineInformationDto eRPWorkCenterMachineInformationDto = new ERPWorkCenterMachineInformationDto();
		InitializeParameterLists();
		string[] collection = new string[5] { "xaqDescription", "xaqUniqueID", "xaqRowVersion", "xaqWorkCenterMachineID", "xaqWorkCenterID" };
		base.selectList.AddRange(collection);
		base.filterList.Add("xaqUniqueID|C", workCenterMachineId);
		AddCustomFieldsToSelectList("WorkCenterMachines");
		using (DataTable dataTable = GetAsDataTable("WorkCenterMachines", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPWorkCenterMachineInformationDto);
			}
			eRPWorkCenterMachineInformationDto.xaqDescription = dataTable.Rows[0].Field<string>("xaqDescription");
			eRPWorkCenterMachineInformationDto.xaqUniqueID = dataTable.Rows[0].Field<Guid>("xaqUniqueID");
			eRPWorkCenterMachineInformationDto.xaqRowVersion = dataTable.Rows[0].Field<byte[]>("xaqRowVersion");
			eRPWorkCenterMachineInformationDto.xaqWorkCenterMachineID = dataTable.Rows[0].Field<short>("xaqWorkCenterMachineID");
			eRPWorkCenterMachineInformationDto.xaqWorkCenterID = dataTable.Rows[0].Field<string>("xaqWorkCenterID");
			eRPWorkCenterMachineInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPWorkCenterMachineInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPWorkCenterMachineInformationDto);
	}

	public Task<APIValidationInfoDto> SaveWorkCenterMachine(ERPWorkCenterMachineDto workCenterMachine)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM WorkCenterMachines WHERE xaqUniqueID = " + M1Util.ConvertToLinq(workCenterMachine.xaqUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["xaqWorkCenterID"] = workCenterMachine.xaqWorkCenterID.ToUpper();
				dataRow["xaqWorkCenterMachineID"] = workCenterMachine.xaqWorkCenterMachineID;
				workCenterMachine.xaqUniqueID = ((workCenterMachine.xaqUniqueID == Guid.Empty) ? Guid.NewGuid() : workCenterMachine.xaqUniqueID);
				dataRow["xaqUniqueID"] = workCenterMachine.xaqUniqueID;
				dataRow["xaqCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["xaqCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The WorkCenterMachine could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (workCenterMachine.xaqRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the WorkCenterMachine is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["xaqRowVersion"], workCenterMachine.xaqRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the WorkCenterMachine has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the WorkCenterMachine again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["xaqDescription"] = workCenterMachine.xaqDescription;
			if (workCenterMachine.CustomFields != null && workCenterMachine.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in workCenterMachine.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the WorkCenterMachine [{workCenterMachine.xaqUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the WorkCenterMachine [{workCenterMachine.xaqUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
