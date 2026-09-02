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

public class ERPJobMemoRepository : APIBaseRepository, IERPJobMemoRepository, IAPIBaseRepository, IDisposable
{
	public ERPJobMemoRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesJobMemoExist(Guid jobMemoId)
	{
		InitializeParameterLists();
		base.filterList.Add("jmkUniqueID|C", jobMemoId);
		base.selectList.Add("jmkUniqueID");
		return Task.FromResult(GetAsObject("JobMemos", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPJobMemoInformationDto>> GetAllJobMemos(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPJobMemoInformationDto> collection = new List<ERPJobMemoInformationDto>();
		InitializeParameterLists();
		string[] array = new string[12]
		{
			"jmkCreatedBy", "jmkCreatedDate", "jmkUniqueID", "jmkClosed", "jmkJobID", "jmkLongDescriptionRtf", "jmkLongDescriptionText", "jmkMemoDate", "jmkRowVersion", "jmkJobMemoID",
			"jmkShortDescription", "jmkShowInJobs"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("JobMemos");
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
		using (DataTable dataTable = GetAsDataTable("JobMemos", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPJobMemoInformationDto eRPJobMemoInformationDto = new ERPJobMemoInformationDto();
				eRPJobMemoInformationDto.jmkCreatedBy = dataTable.Rows[i].Field<string>("jmkCreatedBy");
				eRPJobMemoInformationDto.jmkCreatedDate = dataTable.Rows[i].Field<DateTime?>("jmkCreatedDate");
				eRPJobMemoInformationDto.jmkUniqueID = dataTable.Rows[i].Field<Guid>("jmkUniqueID");
				eRPJobMemoInformationDto.jmkClosed = dataTable.Rows[i].Field<bool>("jmkClosed");
				eRPJobMemoInformationDto.jmkJobID = dataTable.Rows[i].Field<string>("jmkJobID");
				eRPJobMemoInformationDto.jmkLongDescriptionRtf = dataTable.Rows[i].Field<string>("jmkLongDescriptionRtf");
				eRPJobMemoInformationDto.jmkLongDescriptionText = dataTable.Rows[i].Field<string>("jmkLongDescriptionText");
				eRPJobMemoInformationDto.jmkMemoDate = dataTable.Rows[i].Field<DateTime?>("jmkMemoDate");
				eRPJobMemoInformationDto.jmkRowVersion = dataTable.Rows[i].Field<byte[]>("jmkRowVersion");
				eRPJobMemoInformationDto.jmkJobMemoID = dataTable.Rows[i].Field<short>("jmkJobMemoID");
				eRPJobMemoInformationDto.jmkShortDescription = dataTable.Rows[i].Field<string>("jmkShortDescription");
				eRPJobMemoInformationDto.jmkShowInJobs = dataTable.Rows[i].Field<bool>("jmkShowInJobs");
				eRPJobMemoInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPJobMemoInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPJobMemoInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPJobMemoInformationDto> GetJobMemo(Guid jobMemoId)
	{
		ERPJobMemoInformationDto eRPJobMemoInformationDto = new ERPJobMemoInformationDto();
		InitializeParameterLists();
		string[] collection = new string[12]
		{
			"jmkCreatedBy", "jmkCreatedDate", "jmkUniqueID", "jmkClosed", "jmkJobID", "jmkLongDescriptionRtf", "jmkLongDescriptionText", "jmkMemoDate", "jmkRowVersion", "jmkJobMemoID",
			"jmkShortDescription", "jmkShowInJobs"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("jmkUniqueID|C", jobMemoId);
		AddCustomFieldsToSelectList("JobMemos");
		using (DataTable dataTable = GetAsDataTable("JobMemos", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPJobMemoInformationDto);
			}
			eRPJobMemoInformationDto.jmkCreatedBy = dataTable.Rows[0].Field<string>("jmkCreatedBy");
			eRPJobMemoInformationDto.jmkCreatedDate = dataTable.Rows[0].Field<DateTime?>("jmkCreatedDate");
			eRPJobMemoInformationDto.jmkUniqueID = dataTable.Rows[0].Field<Guid>("jmkUniqueID");
			eRPJobMemoInformationDto.jmkClosed = dataTable.Rows[0].Field<bool>("jmkClosed");
			eRPJobMemoInformationDto.jmkJobID = dataTable.Rows[0].Field<string>("jmkJobID");
			eRPJobMemoInformationDto.jmkLongDescriptionRtf = dataTable.Rows[0].Field<string>("jmkLongDescriptionRtf");
			eRPJobMemoInformationDto.jmkLongDescriptionText = dataTable.Rows[0].Field<string>("jmkLongDescriptionText");
			eRPJobMemoInformationDto.jmkMemoDate = dataTable.Rows[0].Field<DateTime?>("jmkMemoDate");
			eRPJobMemoInformationDto.jmkRowVersion = dataTable.Rows[0].Field<byte[]>("jmkRowVersion");
			eRPJobMemoInformationDto.jmkJobMemoID = dataTable.Rows[0].Field<short>("jmkJobMemoID");
			eRPJobMemoInformationDto.jmkShortDescription = dataTable.Rows[0].Field<string>("jmkShortDescription");
			eRPJobMemoInformationDto.jmkShowInJobs = dataTable.Rows[0].Field<bool>("jmkShowInJobs");
			eRPJobMemoInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPJobMemoInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPJobMemoInformationDto);
	}

	public Task<APIValidationInfoDto> SaveJobMemo(ERPJobMemoDto jobMemo)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM JobMemos WHERE jmkUniqueID = " + M1Util.ConvertToLinq(jobMemo.jmkUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["jmkJobID"] = jobMemo.jmkJobID.ToUpper();
				dataRow["jmkJobMemoID"] = jobMemo.jmkJobMemoID;
				jobMemo.jmkUniqueID = ((jobMemo.jmkUniqueID == Guid.Empty) ? Guid.NewGuid() : jobMemo.jmkUniqueID);
				dataRow["jmkUniqueID"] = jobMemo.jmkUniqueID;
				dataRow["jmkCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["jmkCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The JobMemo could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (jobMemo.jmkRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the JobMemo is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["jmkRowVersion"], jobMemo.jmkRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the JobMemo has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the JobMemo again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["jmkClosed"] = jobMemo.jmkClosed;
			dataRow["jmkLongDescriptionRtf"] = jobMemo.jmkLongDescriptionRtf ?? dataRow["jmkLongDescriptionRtf"];
			dataRow["jmkLongDescriptionText"] = jobMemo.jmkLongDescriptionText ?? dataRow["jmkLongDescriptionText"];
			DataRow dataRow2 = dataRow;
			DateTime? jmkMemoDate = jobMemo.jmkMemoDate;
			dataRow2["jmkMemoDate"] = (jmkMemoDate.HasValue ? ((object)jmkMemoDate.GetValueOrDefault()) : dataRow["jmkMemoDate"]);
			dataRow["jmkShortDescription"] = jobMemo.jmkShortDescription;
			dataRow["jmkShowInJobs"] = jobMemo.jmkShowInJobs;
			if (jobMemo.CustomFields != null && jobMemo.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in jobMemo.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the JobMemo [{jobMemo.jmkUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the JobMemo [{jobMemo.jmkUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
