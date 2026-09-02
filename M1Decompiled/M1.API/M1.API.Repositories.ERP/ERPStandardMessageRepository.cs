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

public class ERPStandardMessageRepository : APIBaseRepository, IERPStandardMessageRepository, IAPIBaseRepository, IDisposable
{
	public ERPStandardMessageRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesStandardMessageExist(Guid standardMessageId)
	{
		InitializeParameterLists();
		base.filterList.Add("xamUniqueID|C", standardMessageId);
		base.selectList.Add("xamUniqueID");
		return Task.FromResult(GetAsObject("StandardMessages", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPStandardMessageInformationDto>> GetAllStandardMessages(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPStandardMessageInformationDto> collection = new List<ERPStandardMessageInformationDto>();
		InitializeParameterLists();
		string[] array = new string[11]
		{
			"xamStandardMessageID", "xamCreatedBy", "xamCreatedDate", "xamUniqueID", "xamInactiveDate", "xamInactive", "xamLongDescriptionRtf", "xamLongDescriptionText", "xamMessageType", "xamRowVersion",
			"xamShortDescription"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("StandardMessages");
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
		using (DataTable dataTable = GetAsDataTable("StandardMessages", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPStandardMessageInformationDto eRPStandardMessageInformationDto = new ERPStandardMessageInformationDto();
				eRPStandardMessageInformationDto.xamStandardMessageID = dataTable.Rows[i].Field<string>("xamStandardMessageID");
				eRPStandardMessageInformationDto.xamCreatedBy = dataTable.Rows[i].Field<string>("xamCreatedBy");
				eRPStandardMessageInformationDto.xamCreatedDate = dataTable.Rows[i].Field<DateTime?>("xamCreatedDate");
				eRPStandardMessageInformationDto.xamUniqueID = dataTable.Rows[i].Field<Guid>("xamUniqueID");
				eRPStandardMessageInformationDto.xamInactiveDate = dataTable.Rows[i].Field<DateTime?>("xamInactiveDate");
				eRPStandardMessageInformationDto.xamInactive = dataTable.Rows[i].Field<bool>("xamInactive");
				eRPStandardMessageInformationDto.xamLongDescriptionRtf = dataTable.Rows[i].Field<string>("xamLongDescriptionRtf");
				eRPStandardMessageInformationDto.xamLongDescriptionText = dataTable.Rows[i].Field<string>("xamLongDescriptionText");
				eRPStandardMessageInformationDto.xamMessageType = dataTable.Rows[i].Field<byte>("xamMessageType");
				eRPStandardMessageInformationDto.xamRowVersion = dataTable.Rows[i].Field<byte[]>("xamRowVersion");
				eRPStandardMessageInformationDto.xamShortDescription = dataTable.Rows[i].Field<string>("xamShortDescription");
				eRPStandardMessageInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPStandardMessageInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPStandardMessageInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPStandardMessageInformationDto> GetStandardMessage(Guid standardMessageId)
	{
		ERPStandardMessageInformationDto eRPStandardMessageInformationDto = new ERPStandardMessageInformationDto();
		InitializeParameterLists();
		string[] collection = new string[11]
		{
			"xamStandardMessageID", "xamCreatedBy", "xamCreatedDate", "xamUniqueID", "xamInactiveDate", "xamInactive", "xamLongDescriptionRtf", "xamLongDescriptionText", "xamMessageType", "xamRowVersion",
			"xamShortDescription"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("xamUniqueID|C", standardMessageId);
		AddCustomFieldsToSelectList("StandardMessages");
		using (DataTable dataTable = GetAsDataTable("StandardMessages", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPStandardMessageInformationDto);
			}
			eRPStandardMessageInformationDto.xamStandardMessageID = dataTable.Rows[0].Field<string>("xamStandardMessageID");
			eRPStandardMessageInformationDto.xamCreatedBy = dataTable.Rows[0].Field<string>("xamCreatedBy");
			eRPStandardMessageInformationDto.xamCreatedDate = dataTable.Rows[0].Field<DateTime?>("xamCreatedDate");
			eRPStandardMessageInformationDto.xamUniqueID = dataTable.Rows[0].Field<Guid>("xamUniqueID");
			eRPStandardMessageInformationDto.xamInactiveDate = dataTable.Rows[0].Field<DateTime?>("xamInactiveDate");
			eRPStandardMessageInformationDto.xamInactive = dataTable.Rows[0].Field<bool>("xamInactive");
			eRPStandardMessageInformationDto.xamLongDescriptionRtf = dataTable.Rows[0].Field<string>("xamLongDescriptionRtf");
			eRPStandardMessageInformationDto.xamLongDescriptionText = dataTable.Rows[0].Field<string>("xamLongDescriptionText");
			eRPStandardMessageInformationDto.xamMessageType = dataTable.Rows[0].Field<byte>("xamMessageType");
			eRPStandardMessageInformationDto.xamRowVersion = dataTable.Rows[0].Field<byte[]>("xamRowVersion");
			eRPStandardMessageInformationDto.xamShortDescription = dataTable.Rows[0].Field<string>("xamShortDescription");
			eRPStandardMessageInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPStandardMessageInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPStandardMessageInformationDto);
	}

	public Task<APIValidationInfoDto> SaveStandardMessage(ERPStandardMessageDto standardMessage)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM StandardMessages WHERE xamUniqueID = " + M1Util.ConvertToLinq(standardMessage.xamUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["xamStandardMessageID"] = standardMessage.xamStandardMessageID.ToUpper();
				standardMessage.xamUniqueID = ((standardMessage.xamUniqueID == Guid.Empty) ? Guid.NewGuid() : standardMessage.xamUniqueID);
				dataRow["xamUniqueID"] = standardMessage.xamUniqueID;
				dataRow["xamCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["xamCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The StandardMessage could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (standardMessage.xamRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the StandardMessage is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["xamRowVersion"], standardMessage.xamRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the StandardMessage has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the StandardMessage again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			DataRow dataRow2 = dataRow;
			DateTime? xamInactiveDate = standardMessage.xamInactiveDate;
			dataRow2["xamInactiveDate"] = (xamInactiveDate.HasValue ? ((object)xamInactiveDate.GetValueOrDefault()) : dataRow["xamInactiveDate"]);
			dataRow["xamInactive"] = standardMessage.xamInactive;
			dataRow["xamLongDescriptionRtf"] = standardMessage.xamLongDescriptionRtf ?? dataRow["xamLongDescriptionRtf"];
			dataRow["xamLongDescriptionText"] = standardMessage.xamLongDescriptionText ?? dataRow["xamLongDescriptionText"];
			dataRow["xamMessageType"] = standardMessage.xamMessageType;
			dataRow["xamShortDescription"] = standardMessage.xamShortDescription;
			if (standardMessage.CustomFields != null && standardMessage.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in standardMessage.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the StandardMessage [{standardMessage.xamUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the StandardMessage [{standardMessage.xamUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
