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

public class ERPLeadMemoRepository : APIBaseRepository, IERPLeadMemoRepository, IAPIBaseRepository, IDisposable
{
	public ERPLeadMemoRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesLeadMemoExist(Guid leadMemoId)
	{
		InitializeParameterLists();
		base.filterList.Add("lokUniqueID|C", leadMemoId);
		base.selectList.Add("lokUniqueID");
		return Task.FromResult(GetAsObject("LeadMemos", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPLeadMemoInformationDto>> GetAllLeadMemos(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPLeadMemoInformationDto> collection = new List<ERPLeadMemoInformationDto>();
		InitializeParameterLists();
		string[] array = new string[10] { "lokCreatedBy", "lokCreatedDate", "lokUniqueID", "lokLeadID", "lokLongDescriptionRtf", "lokLongDescriptionText", "lokMemoDate", "lokRowVersion", "lokLeadMemoID", "lokShortDescription" };
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("LeadMemos");
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
		using (DataTable dataTable = GetAsDataTable("LeadMemos", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPLeadMemoInformationDto eRPLeadMemoInformationDto = new ERPLeadMemoInformationDto();
				eRPLeadMemoInformationDto.lokCreatedBy = dataTable.Rows[i].Field<string>("lokCreatedBy");
				eRPLeadMemoInformationDto.lokCreatedDate = dataTable.Rows[i].Field<DateTime?>("lokCreatedDate");
				eRPLeadMemoInformationDto.lokUniqueID = dataTable.Rows[i].Field<Guid>("lokUniqueID");
				eRPLeadMemoInformationDto.lokLeadID = dataTable.Rows[i].Field<string>("lokLeadID");
				eRPLeadMemoInformationDto.lokLongDescriptionRtf = dataTable.Rows[i].Field<string>("lokLongDescriptionRtf");
				eRPLeadMemoInformationDto.lokLongDescriptionText = dataTable.Rows[i].Field<string>("lokLongDescriptionText");
				eRPLeadMemoInformationDto.lokMemoDate = dataTable.Rows[i].Field<DateTime?>("lokMemoDate");
				eRPLeadMemoInformationDto.lokRowVersion = dataTable.Rows[i].Field<byte[]>("lokRowVersion");
				eRPLeadMemoInformationDto.lokLeadMemoID = dataTable.Rows[i].Field<short>("lokLeadMemoID");
				eRPLeadMemoInformationDto.lokShortDescription = dataTable.Rows[i].Field<string>("lokShortDescription");
				eRPLeadMemoInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPLeadMemoInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPLeadMemoInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPLeadMemoInformationDto> GetLeadMemo(Guid leadMemoId)
	{
		ERPLeadMemoInformationDto eRPLeadMemoInformationDto = new ERPLeadMemoInformationDto();
		InitializeParameterLists();
		string[] collection = new string[10] { "lokCreatedBy", "lokCreatedDate", "lokUniqueID", "lokLeadID", "lokLongDescriptionRtf", "lokLongDescriptionText", "lokMemoDate", "lokRowVersion", "lokLeadMemoID", "lokShortDescription" };
		base.selectList.AddRange(collection);
		base.filterList.Add("lokUniqueID|C", leadMemoId);
		AddCustomFieldsToSelectList("LeadMemos");
		using (DataTable dataTable = GetAsDataTable("LeadMemos", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPLeadMemoInformationDto);
			}
			eRPLeadMemoInformationDto.lokCreatedBy = dataTable.Rows[0].Field<string>("lokCreatedBy");
			eRPLeadMemoInformationDto.lokCreatedDate = dataTable.Rows[0].Field<DateTime?>("lokCreatedDate");
			eRPLeadMemoInformationDto.lokUniqueID = dataTable.Rows[0].Field<Guid>("lokUniqueID");
			eRPLeadMemoInformationDto.lokLeadID = dataTable.Rows[0].Field<string>("lokLeadID");
			eRPLeadMemoInformationDto.lokLongDescriptionRtf = dataTable.Rows[0].Field<string>("lokLongDescriptionRtf");
			eRPLeadMemoInformationDto.lokLongDescriptionText = dataTable.Rows[0].Field<string>("lokLongDescriptionText");
			eRPLeadMemoInformationDto.lokMemoDate = dataTable.Rows[0].Field<DateTime?>("lokMemoDate");
			eRPLeadMemoInformationDto.lokRowVersion = dataTable.Rows[0].Field<byte[]>("lokRowVersion");
			eRPLeadMemoInformationDto.lokLeadMemoID = dataTable.Rows[0].Field<short>("lokLeadMemoID");
			eRPLeadMemoInformationDto.lokShortDescription = dataTable.Rows[0].Field<string>("lokShortDescription");
			eRPLeadMemoInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPLeadMemoInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPLeadMemoInformationDto);
	}

	public Task<APIValidationInfoDto> SaveLeadMemo(ERPLeadMemoDto leadMemo)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM LeadMemos WHERE lokUniqueID = " + M1Util.ConvertToLinq(leadMemo.lokUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["lokLeadID"] = leadMemo.lokLeadID.ToUpper();
				dataRow["lokLeadMemoID"] = leadMemo.lokLeadMemoID;
				leadMemo.lokUniqueID = ((leadMemo.lokUniqueID == Guid.Empty) ? Guid.NewGuid() : leadMemo.lokUniqueID);
				dataRow["lokUniqueID"] = leadMemo.lokUniqueID;
				dataRow["lokCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["lokCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The LeadMemo could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (leadMemo.lokRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the LeadMemo is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["lokRowVersion"], leadMemo.lokRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the LeadMemo has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the LeadMemo again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["lokLongDescriptionRtf"] = leadMemo.lokLongDescriptionRtf ?? dataRow["lokLongDescriptionRtf"];
			dataRow["lokLongDescriptionText"] = leadMemo.lokLongDescriptionText ?? dataRow["lokLongDescriptionText"];
			DataRow dataRow2 = dataRow;
			DateTime? lokMemoDate = leadMemo.lokMemoDate;
			dataRow2["lokMemoDate"] = (lokMemoDate.HasValue ? ((object)lokMemoDate.GetValueOrDefault()) : dataRow["lokMemoDate"]);
			dataRow["lokShortDescription"] = leadMemo.lokShortDescription;
			if (leadMemo.CustomFields != null && leadMemo.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in leadMemo.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the LeadMemo [{leadMemo.lokUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the LeadMemo [{leadMemo.lokUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
