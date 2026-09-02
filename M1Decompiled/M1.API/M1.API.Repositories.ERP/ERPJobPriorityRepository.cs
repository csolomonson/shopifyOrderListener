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

public class ERPJobPriorityRepository : APIBaseRepository, IERPJobPriorityRepository, IAPIBaseRepository, IDisposable
{
	public ERPJobPriorityRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesJobPriorityExist(Guid jobPriorityId)
	{
		InitializeParameterLists();
		base.filterList.Add("jmjUniqueID|C", jobPriorityId);
		base.selectList.Add("jmjUniqueID");
		return Task.FromResult(GetAsObject("JobPriorities", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPJobPriorityInformationDto>> GetAllJobPriorities(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPJobPriorityInformationDto> collection = new List<ERPJobPriorityInformationDto>();
		InitializeParameterLists();
		string[] array = new string[4] { "jmjDescription", "jmjUniqueID", "jmjRowVersion", "jmjJobPriorityID" };
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("JobPriorities");
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
		using (DataTable dataTable = GetAsDataTable("JobPriorities", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPJobPriorityInformationDto eRPJobPriorityInformationDto = new ERPJobPriorityInformationDto();
				eRPJobPriorityInformationDto.jmjDescription = dataTable.Rows[i].Field<string>("jmjDescription");
				eRPJobPriorityInformationDto.jmjUniqueID = dataTable.Rows[i].Field<Guid>("jmjUniqueID");
				eRPJobPriorityInformationDto.jmjRowVersion = dataTable.Rows[i].Field<byte[]>("jmjRowVersion");
				eRPJobPriorityInformationDto.jmjJobPriorityID = dataTable.Rows[i].Field<short>("jmjJobPriorityID");
				eRPJobPriorityInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPJobPriorityInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPJobPriorityInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPJobPriorityInformationDto> GetJobPriority(Guid jobPriorityId)
	{
		ERPJobPriorityInformationDto eRPJobPriorityInformationDto = new ERPJobPriorityInformationDto();
		InitializeParameterLists();
		string[] collection = new string[4] { "jmjDescription", "jmjUniqueID", "jmjRowVersion", "jmjJobPriorityID" };
		base.selectList.AddRange(collection);
		base.filterList.Add("jmjUniqueID|C", jobPriorityId);
		AddCustomFieldsToSelectList("JobPriorities");
		using (DataTable dataTable = GetAsDataTable("JobPriorities", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPJobPriorityInformationDto);
			}
			eRPJobPriorityInformationDto.jmjDescription = dataTable.Rows[0].Field<string>("jmjDescription");
			eRPJobPriorityInformationDto.jmjUniqueID = dataTable.Rows[0].Field<Guid>("jmjUniqueID");
			eRPJobPriorityInformationDto.jmjRowVersion = dataTable.Rows[0].Field<byte[]>("jmjRowVersion");
			eRPJobPriorityInformationDto.jmjJobPriorityID = dataTable.Rows[0].Field<short>("jmjJobPriorityID");
			eRPJobPriorityInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPJobPriorityInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPJobPriorityInformationDto);
	}

	public Task<APIValidationInfoDto> SaveJobPriority(ERPJobPriorityDto jobPriority)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM JobPriorities WHERE jmjUniqueID = " + M1Util.ConvertToLinq(jobPriority.jmjUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["jmjJobPriorityID"] = jobPriority.jmjJobPriorityID;
				jobPriority.jmjUniqueID = ((jobPriority.jmjUniqueID == Guid.Empty) ? Guid.NewGuid() : jobPriority.jmjUniqueID);
				dataRow["jmjUniqueID"] = jobPriority.jmjUniqueID;
				dataRow["jmjCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["jmjCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The JobPriority could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (jobPriority.jmjRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the JobPriority is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["jmjRowVersion"], jobPriority.jmjRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the JobPriority has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the JobPriority again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["jmjDescription"] = jobPriority.jmjDescription;
			if (jobPriority.CustomFields != null && jobPriority.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in jobPriority.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the JobPriority [{jobPriority.jmjUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the JobPriority [{jobPriority.jmjUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
