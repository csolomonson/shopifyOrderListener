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

public class ERPAttachmentMemoRepository : APIBaseRepository, IERPAttachmentMemoRepository, IAPIBaseRepository, IDisposable
{
	public ERPAttachmentMemoRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesAttachmentMemoExist(Guid attachmentMemoId)
	{
		InitializeParameterLists();
		base.filterList.Add("cmqUniqueID|C", attachmentMemoId);
		base.selectList.Add("cmqUniqueID");
		return Task.FromResult(GetAsObject("AttachmentMemos", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPAttachmentMemoInformationDto>> GetAllAttachmentMemos(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPAttachmentMemoInformationDto> collection = new List<ERPAttachmentMemoInformationDto>();
		InitializeParameterLists();
		string[] array = new string[10] { "cmqAttachmentID", "cmqCreatedBy", "cmqCreatedDate", "cmqUniqueID", "cmqLongDescriptionRtf", "cmqLongDescriptionText", "cmqMemoDate", "cmqRowVersion", "cmqAttachmentMemoID", "cmqShortDescription" };
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("AttachmentMemos");
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
		using (DataTable dataTable = GetAsDataTable("AttachmentMemos", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPAttachmentMemoInformationDto eRPAttachmentMemoInformationDto = new ERPAttachmentMemoInformationDto();
				eRPAttachmentMemoInformationDto.cmqAttachmentID = dataTable.Rows[i].Field<string>("cmqAttachmentID");
				eRPAttachmentMemoInformationDto.cmqCreatedBy = dataTable.Rows[i].Field<string>("cmqCreatedBy");
				eRPAttachmentMemoInformationDto.cmqCreatedDate = dataTable.Rows[i].Field<DateTime?>("cmqCreatedDate");
				eRPAttachmentMemoInformationDto.cmqUniqueID = dataTable.Rows[i].Field<Guid>("cmqUniqueID");
				eRPAttachmentMemoInformationDto.cmqLongDescriptionRtf = dataTable.Rows[i].Field<string>("cmqLongDescriptionRtf");
				eRPAttachmentMemoInformationDto.cmqLongDescriptionText = dataTable.Rows[i].Field<string>("cmqLongDescriptionText");
				eRPAttachmentMemoInformationDto.cmqMemoDate = dataTable.Rows[i].Field<DateTime?>("cmqMemoDate");
				eRPAttachmentMemoInformationDto.cmqRowVersion = dataTable.Rows[i].Field<byte[]>("cmqRowVersion");
				eRPAttachmentMemoInformationDto.cmqAttachmentMemoID = dataTable.Rows[i].Field<short>("cmqAttachmentMemoID");
				eRPAttachmentMemoInformationDto.cmqShortDescription = dataTable.Rows[i].Field<string>("cmqShortDescription");
				eRPAttachmentMemoInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPAttachmentMemoInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPAttachmentMemoInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPAttachmentMemoInformationDto> GetAttachmentMemo(Guid attachmentMemoId)
	{
		ERPAttachmentMemoInformationDto eRPAttachmentMemoInformationDto = new ERPAttachmentMemoInformationDto();
		InitializeParameterLists();
		string[] collection = new string[10] { "cmqAttachmentID", "cmqCreatedBy", "cmqCreatedDate", "cmqUniqueID", "cmqLongDescriptionRtf", "cmqLongDescriptionText", "cmqMemoDate", "cmqRowVersion", "cmqAttachmentMemoID", "cmqShortDescription" };
		base.selectList.AddRange(collection);
		base.filterList.Add("cmqUniqueID|C", attachmentMemoId);
		AddCustomFieldsToSelectList("AttachmentMemos");
		using (DataTable dataTable = GetAsDataTable("AttachmentMemos", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPAttachmentMemoInformationDto);
			}
			eRPAttachmentMemoInformationDto.cmqAttachmentID = dataTable.Rows[0].Field<string>("cmqAttachmentID");
			eRPAttachmentMemoInformationDto.cmqCreatedBy = dataTable.Rows[0].Field<string>("cmqCreatedBy");
			eRPAttachmentMemoInformationDto.cmqCreatedDate = dataTable.Rows[0].Field<DateTime?>("cmqCreatedDate");
			eRPAttachmentMemoInformationDto.cmqUniqueID = dataTable.Rows[0].Field<Guid>("cmqUniqueID");
			eRPAttachmentMemoInformationDto.cmqLongDescriptionRtf = dataTable.Rows[0].Field<string>("cmqLongDescriptionRtf");
			eRPAttachmentMemoInformationDto.cmqLongDescriptionText = dataTable.Rows[0].Field<string>("cmqLongDescriptionText");
			eRPAttachmentMemoInformationDto.cmqMemoDate = dataTable.Rows[0].Field<DateTime?>("cmqMemoDate");
			eRPAttachmentMemoInformationDto.cmqRowVersion = dataTable.Rows[0].Field<byte[]>("cmqRowVersion");
			eRPAttachmentMemoInformationDto.cmqAttachmentMemoID = dataTable.Rows[0].Field<short>("cmqAttachmentMemoID");
			eRPAttachmentMemoInformationDto.cmqShortDescription = dataTable.Rows[0].Field<string>("cmqShortDescription");
			eRPAttachmentMemoInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPAttachmentMemoInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPAttachmentMemoInformationDto);
	}

	public Task<APIValidationInfoDto> SaveAttachmentMemo(ERPAttachmentMemoDto attachmentMemo)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM AttachmentMemos WHERE cmqUniqueID = " + M1Util.ConvertToLinq(attachmentMemo.cmqUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["cmqAttachmentID"] = attachmentMemo.cmqAttachmentID.ToUpper();
				dataRow["cmqAttachmentMemoID"] = attachmentMemo.cmqAttachmentMemoID;
				attachmentMemo.cmqUniqueID = ((attachmentMemo.cmqUniqueID == Guid.Empty) ? Guid.NewGuid() : attachmentMemo.cmqUniqueID);
				dataRow["cmqUniqueID"] = attachmentMemo.cmqUniqueID;
				dataRow["cmqCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["cmqCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The AttachmentMemo could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (attachmentMemo.cmqRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the AttachmentMemo is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["cmqRowVersion"], attachmentMemo.cmqRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the AttachmentMemo has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the AttachmentMemo again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["cmqLongDescriptionRtf"] = attachmentMemo.cmqLongDescriptionRtf ?? dataRow["cmqLongDescriptionRtf"];
			dataRow["cmqLongDescriptionText"] = attachmentMemo.cmqLongDescriptionText ?? dataRow["cmqLongDescriptionText"];
			DataRow dataRow2 = dataRow;
			DateTime? cmqMemoDate = attachmentMemo.cmqMemoDate;
			dataRow2["cmqMemoDate"] = (cmqMemoDate.HasValue ? ((object)cmqMemoDate.GetValueOrDefault()) : dataRow["cmqMemoDate"]);
			dataRow["cmqShortDescription"] = attachmentMemo.cmqShortDescription;
			if (attachmentMemo.CustomFields != null && attachmentMemo.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in attachmentMemo.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the AttachmentMemo [{attachmentMemo.cmqUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the AttachmentMemo [{attachmentMemo.cmqUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
