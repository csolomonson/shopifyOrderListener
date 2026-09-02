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

public class ERPCycleCodeRepository : APIBaseRepository, IERPCycleCodeRepository, IAPIBaseRepository, IDisposable
{
	public ERPCycleCodeRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesCycleCodeExist(Guid cycleCodeId)
	{
		InitializeParameterLists();
		base.filterList.Add("imdUniqueID|C", cycleCodeId);
		base.selectList.Add("imdUniqueID");
		return Task.FromResult(GetAsObject("CycleCodes", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPCycleCodeInformationDto>> GetAllCycleCodes(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPCycleCodeInformationDto> collection = new List<ERPCycleCodeInformationDto>();
		InitializeParameterLists();
		string[] array = new string[6] { "imdCycleCodeID", "imdCreatedBy", "imdCreatedDate", "imdDescription", "imdUniqueID", "imdRowVersion" };
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("CycleCodes");
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
		using (DataTable dataTable = GetAsDataTable("CycleCodes", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPCycleCodeInformationDto eRPCycleCodeInformationDto = new ERPCycleCodeInformationDto();
				eRPCycleCodeInformationDto.imdCycleCodeID = dataTable.Rows[i].Field<string>("imdCycleCodeID");
				eRPCycleCodeInformationDto.imdCreatedBy = dataTable.Rows[i].Field<string>("imdCreatedBy");
				eRPCycleCodeInformationDto.imdCreatedDate = dataTable.Rows[i].Field<DateTime?>("imdCreatedDate");
				eRPCycleCodeInformationDto.imdDescription = dataTable.Rows[i].Field<string>("imdDescription");
				eRPCycleCodeInformationDto.imdUniqueID = dataTable.Rows[i].Field<Guid>("imdUniqueID");
				eRPCycleCodeInformationDto.imdRowVersion = dataTable.Rows[i].Field<byte[]>("imdRowVersion");
				eRPCycleCodeInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPCycleCodeInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPCycleCodeInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPCycleCodeInformationDto> GetCycleCode(Guid cycleCodeId)
	{
		ERPCycleCodeInformationDto eRPCycleCodeInformationDto = new ERPCycleCodeInformationDto();
		InitializeParameterLists();
		string[] collection = new string[6] { "imdCycleCodeID", "imdCreatedBy", "imdCreatedDate", "imdDescription", "imdUniqueID", "imdRowVersion" };
		base.selectList.AddRange(collection);
		base.filterList.Add("imdUniqueID|C", cycleCodeId);
		AddCustomFieldsToSelectList("CycleCodes");
		using (DataTable dataTable = GetAsDataTable("CycleCodes", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPCycleCodeInformationDto);
			}
			eRPCycleCodeInformationDto.imdCycleCodeID = dataTable.Rows[0].Field<string>("imdCycleCodeID");
			eRPCycleCodeInformationDto.imdCreatedBy = dataTable.Rows[0].Field<string>("imdCreatedBy");
			eRPCycleCodeInformationDto.imdCreatedDate = dataTable.Rows[0].Field<DateTime?>("imdCreatedDate");
			eRPCycleCodeInformationDto.imdDescription = dataTable.Rows[0].Field<string>("imdDescription");
			eRPCycleCodeInformationDto.imdUniqueID = dataTable.Rows[0].Field<Guid>("imdUniqueID");
			eRPCycleCodeInformationDto.imdRowVersion = dataTable.Rows[0].Field<byte[]>("imdRowVersion");
			eRPCycleCodeInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPCycleCodeInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPCycleCodeInformationDto);
	}

	public Task<APIValidationInfoDto> SaveCycleCode(ERPCycleCodeDto cycleCode)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM CycleCodes WHERE imdUniqueID = " + M1Util.ConvertToLinq(cycleCode.imdUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["imdCycleCodeID"] = cycleCode.imdCycleCodeID.ToUpper();
				cycleCode.imdUniqueID = ((cycleCode.imdUniqueID == Guid.Empty) ? Guid.NewGuid() : cycleCode.imdUniqueID);
				dataRow["imdUniqueID"] = cycleCode.imdUniqueID;
				dataRow["imdCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["imdCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The CycleCode could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (cycleCode.imdRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the CycleCode is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["imdRowVersion"], cycleCode.imdRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the CycleCode has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the CycleCode again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["imdDescription"] = cycleCode.imdDescription;
			if (cycleCode.CustomFields != null && cycleCode.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in cycleCode.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the CycleCode [{cycleCode.imdUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the CycleCode [{cycleCode.imdUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
