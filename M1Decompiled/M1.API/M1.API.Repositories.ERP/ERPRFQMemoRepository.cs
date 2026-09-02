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

public class ERPRFQMemoRepository : APIBaseRepository, IERPRFQMemoRepository, IAPIBaseRepository, IDisposable
{
	public ERPRFQMemoRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesRFQMemoExist(Guid rFQMemoId)
	{
		InitializeParameterLists();
		base.filterList.Add("rqkUniqueID|C", rFQMemoId);
		base.selectList.Add("rqkUniqueID");
		return Task.FromResult(GetAsObject("RFQMemos", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPRFQMemoInformationDto>> GetAllRFQMemos(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPRFQMemoInformationDto> collection = new List<ERPRFQMemoInformationDto>();
		InitializeParameterLists();
		string[] array = new string[11]
		{
			"rqkCreatedBy", "rqkCreatedDate", "rqkUniqueID", "rqkClosed", "rqkLongDescriptionRtf", "rqkLongDescriptionText", "rqkMemoDate", "rqkRfqID", "rqkRowVersion", "rqkRfqMemoID",
			"rqkShortDescription"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("RFQMemos");
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
		using (DataTable dataTable = GetAsDataTable("RFQMemos", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPRFQMemoInformationDto eRPRFQMemoInformationDto = new ERPRFQMemoInformationDto();
				eRPRFQMemoInformationDto.rqkCreatedBy = dataTable.Rows[i].Field<string>("rqkCreatedBy");
				eRPRFQMemoInformationDto.rqkCreatedDate = dataTable.Rows[i].Field<DateTime?>("rqkCreatedDate");
				eRPRFQMemoInformationDto.rqkUniqueID = dataTable.Rows[i].Field<Guid>("rqkUniqueID");
				eRPRFQMemoInformationDto.rqkClosed = dataTable.Rows[i].Field<bool>("rqkClosed");
				eRPRFQMemoInformationDto.rqkLongDescriptionRtf = dataTable.Rows[i].Field<string>("rqkLongDescriptionRtf");
				eRPRFQMemoInformationDto.rqkLongDescriptionText = dataTable.Rows[i].Field<string>("rqkLongDescriptionText");
				eRPRFQMemoInformationDto.rqkMemoDate = dataTable.Rows[i].Field<DateTime?>("rqkMemoDate");
				eRPRFQMemoInformationDto.rqkRfqID = dataTable.Rows[i].Field<string>("rqkRfqID");
				eRPRFQMemoInformationDto.rqkRowVersion = dataTable.Rows[i].Field<byte[]>("rqkRowVersion");
				eRPRFQMemoInformationDto.rqkRfqMemoID = dataTable.Rows[i].Field<short>("rqkRfqMemoID");
				eRPRFQMemoInformationDto.rqkShortDescription = dataTable.Rows[i].Field<string>("rqkShortDescription");
				eRPRFQMemoInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPRFQMemoInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPRFQMemoInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPRFQMemoInformationDto> GetRFQMemo(Guid rFQMemoId)
	{
		ERPRFQMemoInformationDto eRPRFQMemoInformationDto = new ERPRFQMemoInformationDto();
		InitializeParameterLists();
		string[] collection = new string[11]
		{
			"rqkCreatedBy", "rqkCreatedDate", "rqkUniqueID", "rqkClosed", "rqkLongDescriptionRtf", "rqkLongDescriptionText", "rqkMemoDate", "rqkRfqID", "rqkRowVersion", "rqkRfqMemoID",
			"rqkShortDescription"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("rqkUniqueID|C", rFQMemoId);
		AddCustomFieldsToSelectList("RFQMemos");
		using (DataTable dataTable = GetAsDataTable("RFQMemos", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPRFQMemoInformationDto);
			}
			eRPRFQMemoInformationDto.rqkCreatedBy = dataTable.Rows[0].Field<string>("rqkCreatedBy");
			eRPRFQMemoInformationDto.rqkCreatedDate = dataTable.Rows[0].Field<DateTime?>("rqkCreatedDate");
			eRPRFQMemoInformationDto.rqkUniqueID = dataTable.Rows[0].Field<Guid>("rqkUniqueID");
			eRPRFQMemoInformationDto.rqkClosed = dataTable.Rows[0].Field<bool>("rqkClosed");
			eRPRFQMemoInformationDto.rqkLongDescriptionRtf = dataTable.Rows[0].Field<string>("rqkLongDescriptionRtf");
			eRPRFQMemoInformationDto.rqkLongDescriptionText = dataTable.Rows[0].Field<string>("rqkLongDescriptionText");
			eRPRFQMemoInformationDto.rqkMemoDate = dataTable.Rows[0].Field<DateTime?>("rqkMemoDate");
			eRPRFQMemoInformationDto.rqkRfqID = dataTable.Rows[0].Field<string>("rqkRfqID");
			eRPRFQMemoInformationDto.rqkRowVersion = dataTable.Rows[0].Field<byte[]>("rqkRowVersion");
			eRPRFQMemoInformationDto.rqkRfqMemoID = dataTable.Rows[0].Field<short>("rqkRfqMemoID");
			eRPRFQMemoInformationDto.rqkShortDescription = dataTable.Rows[0].Field<string>("rqkShortDescription");
			eRPRFQMemoInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPRFQMemoInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPRFQMemoInformationDto);
	}

	public Task<APIValidationInfoDto> SaveRFQMemo(ERPRFQMemoDto rFQMemo)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM RFQMemos WHERE rqkUniqueID = " + M1Util.ConvertToLinq(rFQMemo.rqkUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["rqkRfqID"] = rFQMemo.rqkRfqID.ToUpper();
				dataRow["rqkRfqMemoID"] = rFQMemo.rqkRfqMemoID;
				rFQMemo.rqkUniqueID = ((rFQMemo.rqkUniqueID == Guid.Empty) ? Guid.NewGuid() : rFQMemo.rqkUniqueID);
				dataRow["rqkUniqueID"] = rFQMemo.rqkUniqueID;
				dataRow["rqkCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["rqkCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The RFQMemo could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (rFQMemo.rqkRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the RFQMemo is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["rqkRowVersion"], rFQMemo.rqkRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the RFQMemo has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the RFQMemo again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["rqkClosed"] = rFQMemo.rqkClosed;
			dataRow["rqkLongDescriptionRtf"] = rFQMemo.rqkLongDescriptionRtf ?? dataRow["rqkLongDescriptionRtf"];
			dataRow["rqkLongDescriptionText"] = rFQMemo.rqkLongDescriptionText ?? dataRow["rqkLongDescriptionText"];
			DataRow dataRow2 = dataRow;
			DateTime? rqkMemoDate = rFQMemo.rqkMemoDate;
			dataRow2["rqkMemoDate"] = (rqkMemoDate.HasValue ? ((object)rqkMemoDate.GetValueOrDefault()) : dataRow["rqkMemoDate"]);
			dataRow["rqkShortDescription"] = rFQMemo.rqkShortDescription;
			if (rFQMemo.CustomFields != null && rFQMemo.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in rFQMemo.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the RFQMemo [{rFQMemo.rqkUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the RFQMemo [{rFQMemo.rqkUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
