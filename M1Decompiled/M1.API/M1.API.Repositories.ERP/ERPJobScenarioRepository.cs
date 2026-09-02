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

public class ERPJobScenarioRepository : APIBaseRepository, IERPJobScenarioRepository, IAPIBaseRepository, IDisposable
{
	public ERPJobScenarioRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesJobScenarioExist(Guid jobScenarioId)
	{
		InitializeParameterLists();
		base.filterList.Add("jmnUniqueID|C", jobScenarioId);
		base.selectList.Add("jmnUniqueID");
		return Task.FromResult(GetAsObject("JobScenarios", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPJobScenarioInformationDto>> GetAllJobScenarios(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPJobScenarioInformationDto> collection = new List<ERPJobScenarioInformationDto>();
		InitializeParameterLists();
		string[] array = new string[6] { "jmnJobScenarioID", "jmnCreatedBy", "jmnCreatedDate", "jmnDescription", "jmnUniqueID", "jmnRowVersion" };
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("JobScenarios");
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
		using (DataTable dataTable = GetAsDataTable("JobScenarios", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPJobScenarioInformationDto eRPJobScenarioInformationDto = new ERPJobScenarioInformationDto();
				eRPJobScenarioInformationDto.jmnJobScenarioID = dataTable.Rows[i].Field<string>("jmnJobScenarioID");
				eRPJobScenarioInformationDto.jmnCreatedBy = dataTable.Rows[i].Field<string>("jmnCreatedBy");
				eRPJobScenarioInformationDto.jmnCreatedDate = dataTable.Rows[i].Field<DateTime?>("jmnCreatedDate");
				eRPJobScenarioInformationDto.jmnDescription = dataTable.Rows[i].Field<string>("jmnDescription");
				eRPJobScenarioInformationDto.jmnUniqueID = dataTable.Rows[i].Field<Guid>("jmnUniqueID");
				eRPJobScenarioInformationDto.jmnRowVersion = dataTable.Rows[i].Field<byte[]>("jmnRowVersion");
				eRPJobScenarioInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPJobScenarioInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPJobScenarioInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPJobScenarioInformationDto> GetJobScenario(Guid jobScenarioId)
	{
		ERPJobScenarioInformationDto eRPJobScenarioInformationDto = new ERPJobScenarioInformationDto();
		InitializeParameterLists();
		string[] collection = new string[6] { "jmnJobScenarioID", "jmnCreatedBy", "jmnCreatedDate", "jmnDescription", "jmnUniqueID", "jmnRowVersion" };
		base.selectList.AddRange(collection);
		base.filterList.Add("jmnUniqueID|C", jobScenarioId);
		AddCustomFieldsToSelectList("JobScenarios");
		using (DataTable dataTable = GetAsDataTable("JobScenarios", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPJobScenarioInformationDto);
			}
			eRPJobScenarioInformationDto.jmnJobScenarioID = dataTable.Rows[0].Field<string>("jmnJobScenarioID");
			eRPJobScenarioInformationDto.jmnCreatedBy = dataTable.Rows[0].Field<string>("jmnCreatedBy");
			eRPJobScenarioInformationDto.jmnCreatedDate = dataTable.Rows[0].Field<DateTime?>("jmnCreatedDate");
			eRPJobScenarioInformationDto.jmnDescription = dataTable.Rows[0].Field<string>("jmnDescription");
			eRPJobScenarioInformationDto.jmnUniqueID = dataTable.Rows[0].Field<Guid>("jmnUniqueID");
			eRPJobScenarioInformationDto.jmnRowVersion = dataTable.Rows[0].Field<byte[]>("jmnRowVersion");
			eRPJobScenarioInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPJobScenarioInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPJobScenarioInformationDto);
	}

	public Task<APIValidationInfoDto> SaveJobScenario(ERPJobScenarioDto jobScenario)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM JobScenarios WHERE jmnUniqueID = " + M1Util.ConvertToLinq(jobScenario.jmnUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["jmnJobScenarioID"] = jobScenario.jmnJobScenarioID.ToUpper();
				jobScenario.jmnUniqueID = ((jobScenario.jmnUniqueID == Guid.Empty) ? Guid.NewGuid() : jobScenario.jmnUniqueID);
				dataRow["jmnUniqueID"] = jobScenario.jmnUniqueID;
				dataRow["jmnCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["jmnCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The JobScenario could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (jobScenario.jmnRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the JobScenario is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["jmnRowVersion"], jobScenario.jmnRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the JobScenario has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the JobScenario again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["jmnDescription"] = jobScenario.jmnDescription;
			if (jobScenario.CustomFields != null && jobScenario.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in jobScenario.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the JobScenario [{jobScenario.jmnUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the JobScenario [{jobScenario.jmnUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
