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

public class ERPWorkCenterMemoRepository : APIBaseRepository, IERPWorkCenterMemoRepository, IAPIBaseRepository, IDisposable
{
	public ERPWorkCenterMemoRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesWorkCenterMemoExist(Guid workCenterMemoId)
	{
		InitializeParameterLists();
		base.filterList.Add("xakUniqueID|C", workCenterMemoId);
		base.selectList.Add("xakUniqueID");
		return Task.FromResult(GetAsObject("WorkCenterMemos", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPWorkCenterMemoInformationDto>> GetAllWorkCenterMemos(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPWorkCenterMemoInformationDto> collection = new List<ERPWorkCenterMemoInformationDto>();
		InitializeParameterLists();
		string[] array = new string[14]
		{
			"xakCreatedBy", "xakCreatedDate", "xakUniqueID", "xakLongDescriptionRtf", "xakLongDescriptionText", "xakMemoDate", "xakRowVersion", "xakWorkCenterMemoID", "xakShortDescription", "xakShowInJobs",
			"xakShowInParts", "xakShowInQuotes", "xakShowInWorkCenters", "xakWorkCenterID"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("WorkCenterMemos");
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
		using (DataTable dataTable = GetAsDataTable("WorkCenterMemos", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPWorkCenterMemoInformationDto eRPWorkCenterMemoInformationDto = new ERPWorkCenterMemoInformationDto();
				eRPWorkCenterMemoInformationDto.xakCreatedBy = dataTable.Rows[i].Field<string>("xakCreatedBy");
				eRPWorkCenterMemoInformationDto.xakCreatedDate = dataTable.Rows[i].Field<DateTime?>("xakCreatedDate");
				eRPWorkCenterMemoInformationDto.xakUniqueID = dataTable.Rows[i].Field<Guid>("xakUniqueID");
				eRPWorkCenterMemoInformationDto.xakLongDescriptionRtf = dataTable.Rows[i].Field<string>("xakLongDescriptionRtf");
				eRPWorkCenterMemoInformationDto.xakLongDescriptionText = dataTable.Rows[i].Field<string>("xakLongDescriptionText");
				eRPWorkCenterMemoInformationDto.xakMemoDate = dataTable.Rows[i].Field<DateTime?>("xakMemoDate");
				eRPWorkCenterMemoInformationDto.xakRowVersion = dataTable.Rows[i].Field<byte[]>("xakRowVersion");
				eRPWorkCenterMemoInformationDto.xakWorkCenterMemoID = dataTable.Rows[i].Field<short>("xakWorkCenterMemoID");
				eRPWorkCenterMemoInformationDto.xakShortDescription = dataTable.Rows[i].Field<string>("xakShortDescription");
				eRPWorkCenterMemoInformationDto.xakShowInJobs = dataTable.Rows[i].Field<bool>("xakShowInJobs");
				eRPWorkCenterMemoInformationDto.xakShowInParts = dataTable.Rows[i].Field<bool>("xakShowInParts");
				eRPWorkCenterMemoInformationDto.xakShowInQuotes = dataTable.Rows[i].Field<bool>("xakShowInQuotes");
				eRPWorkCenterMemoInformationDto.xakShowInWorkCenters = dataTable.Rows[i].Field<bool>("xakShowInWorkCenters");
				eRPWorkCenterMemoInformationDto.xakWorkCenterID = dataTable.Rows[i].Field<string>("xakWorkCenterID");
				eRPWorkCenterMemoInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPWorkCenterMemoInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPWorkCenterMemoInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPWorkCenterMemoInformationDto> GetWorkCenterMemo(Guid workCenterMemoId)
	{
		ERPWorkCenterMemoInformationDto eRPWorkCenterMemoInformationDto = new ERPWorkCenterMemoInformationDto();
		InitializeParameterLists();
		string[] collection = new string[14]
		{
			"xakCreatedBy", "xakCreatedDate", "xakUniqueID", "xakLongDescriptionRtf", "xakLongDescriptionText", "xakMemoDate", "xakRowVersion", "xakWorkCenterMemoID", "xakShortDescription", "xakShowInJobs",
			"xakShowInParts", "xakShowInQuotes", "xakShowInWorkCenters", "xakWorkCenterID"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("xakUniqueID|C", workCenterMemoId);
		AddCustomFieldsToSelectList("WorkCenterMemos");
		using (DataTable dataTable = GetAsDataTable("WorkCenterMemos", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPWorkCenterMemoInformationDto);
			}
			eRPWorkCenterMemoInformationDto.xakCreatedBy = dataTable.Rows[0].Field<string>("xakCreatedBy");
			eRPWorkCenterMemoInformationDto.xakCreatedDate = dataTable.Rows[0].Field<DateTime?>("xakCreatedDate");
			eRPWorkCenterMemoInformationDto.xakUniqueID = dataTable.Rows[0].Field<Guid>("xakUniqueID");
			eRPWorkCenterMemoInformationDto.xakLongDescriptionRtf = dataTable.Rows[0].Field<string>("xakLongDescriptionRtf");
			eRPWorkCenterMemoInformationDto.xakLongDescriptionText = dataTable.Rows[0].Field<string>("xakLongDescriptionText");
			eRPWorkCenterMemoInformationDto.xakMemoDate = dataTable.Rows[0].Field<DateTime?>("xakMemoDate");
			eRPWorkCenterMemoInformationDto.xakRowVersion = dataTable.Rows[0].Field<byte[]>("xakRowVersion");
			eRPWorkCenterMemoInformationDto.xakWorkCenterMemoID = dataTable.Rows[0].Field<short>("xakWorkCenterMemoID");
			eRPWorkCenterMemoInformationDto.xakShortDescription = dataTable.Rows[0].Field<string>("xakShortDescription");
			eRPWorkCenterMemoInformationDto.xakShowInJobs = dataTable.Rows[0].Field<bool>("xakShowInJobs");
			eRPWorkCenterMemoInformationDto.xakShowInParts = dataTable.Rows[0].Field<bool>("xakShowInParts");
			eRPWorkCenterMemoInformationDto.xakShowInQuotes = dataTable.Rows[0].Field<bool>("xakShowInQuotes");
			eRPWorkCenterMemoInformationDto.xakShowInWorkCenters = dataTable.Rows[0].Field<bool>("xakShowInWorkCenters");
			eRPWorkCenterMemoInformationDto.xakWorkCenterID = dataTable.Rows[0].Field<string>("xakWorkCenterID");
			eRPWorkCenterMemoInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPWorkCenterMemoInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPWorkCenterMemoInformationDto);
	}

	public Task<APIValidationInfoDto> SaveWorkCenterMemo(ERPWorkCenterMemoDto workCenterMemo)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM WorkCenterMemos WHERE xakUniqueID = " + M1Util.ConvertToLinq(workCenterMemo.xakUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["xakWorkCenterID"] = workCenterMemo.xakWorkCenterID.ToUpper();
				dataRow["xakWorkCenterMemoID"] = workCenterMemo.xakWorkCenterMemoID;
				workCenterMemo.xakUniqueID = ((workCenterMemo.xakUniqueID == Guid.Empty) ? Guid.NewGuid() : workCenterMemo.xakUniqueID);
				dataRow["xakUniqueID"] = workCenterMemo.xakUniqueID;
				dataRow["xakCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["xakCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The WorkCenterMemo could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (workCenterMemo.xakRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the WorkCenterMemo is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["xakRowVersion"], workCenterMemo.xakRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the WorkCenterMemo has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the WorkCenterMemo again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["xakLongDescriptionRtf"] = workCenterMemo.xakLongDescriptionRtf ?? dataRow["xakLongDescriptionRtf"];
			dataRow["xakLongDescriptionText"] = workCenterMemo.xakLongDescriptionText ?? dataRow["xakLongDescriptionText"];
			DataRow dataRow2 = dataRow;
			DateTime? xakMemoDate = workCenterMemo.xakMemoDate;
			dataRow2["xakMemoDate"] = (xakMemoDate.HasValue ? ((object)xakMemoDate.GetValueOrDefault()) : dataRow["xakMemoDate"]);
			dataRow["xakShortDescription"] = workCenterMemo.xakShortDescription;
			dataRow["xakShowInJobs"] = workCenterMemo.xakShowInJobs;
			dataRow["xakShowInParts"] = workCenterMemo.xakShowInParts;
			dataRow["xakShowInQuotes"] = workCenterMemo.xakShowInQuotes;
			dataRow["xakShowInWorkCenters"] = workCenterMemo.xakShowInWorkCenters;
			if (workCenterMemo.CustomFields != null && workCenterMemo.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in workCenterMemo.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the WorkCenterMemo [{workCenterMemo.xakUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the WorkCenterMemo [{workCenterMemo.xakUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
