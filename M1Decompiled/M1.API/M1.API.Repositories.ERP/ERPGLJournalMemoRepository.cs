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

public class ERPGLJournalMemoRepository : APIBaseRepository, IERPGLJournalMemoRepository, IAPIBaseRepository, IDisposable
{
	public ERPGLJournalMemoRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesGLJournalMemoExist(Guid gLJournalMemoId)
	{
		InitializeParameterLists();
		base.filterList.Add("glmUniqueID|C", gLJournalMemoId);
		base.selectList.Add("glmUniqueID");
		return Task.FromResult(GetAsObject("GLJournalMemos", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPGLJournalMemoInformationDto>> GetAllGLJournalMemos(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPGLJournalMemoInformationDto> collection = new List<ERPGLJournalMemoInformationDto>();
		InitializeParameterLists();
		string[] array = new string[11]
		{
			"glmCreatedBy", "glmCreatedDate", "glmUniqueID", "glmGlJournalID", "glmClosed", "glmLongDescriptionRtf", "glmLongDescriptionText", "glmMemoDate", "glmRowVersion", "glmGlJournalMemoID",
			"glmShortDescription"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("GLJournalMemos");
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
		using (DataTable dataTable = GetAsDataTable("GLJournalMemos", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPGLJournalMemoInformationDto eRPGLJournalMemoInformationDto = new ERPGLJournalMemoInformationDto();
				eRPGLJournalMemoInformationDto.glmCreatedBy = dataTable.Rows[i].Field<string>("glmCreatedBy");
				eRPGLJournalMemoInformationDto.glmCreatedDate = dataTable.Rows[i].Field<DateTime?>("glmCreatedDate");
				eRPGLJournalMemoInformationDto.glmUniqueID = dataTable.Rows[i].Field<Guid>("glmUniqueID");
				eRPGLJournalMemoInformationDto.glmGlJournalID = dataTable.Rows[i].Field<int>("glmGlJournalID");
				eRPGLJournalMemoInformationDto.glmClosed = dataTable.Rows[i].Field<bool>("glmClosed");
				eRPGLJournalMemoInformationDto.glmLongDescriptionRtf = dataTable.Rows[i].Field<string>("glmLongDescriptionRtf");
				eRPGLJournalMemoInformationDto.glmLongDescriptionText = dataTable.Rows[i].Field<string>("glmLongDescriptionText");
				eRPGLJournalMemoInformationDto.glmMemoDate = dataTable.Rows[i].Field<DateTime?>("glmMemoDate");
				eRPGLJournalMemoInformationDto.glmRowVersion = dataTable.Rows[i].Field<byte[]>("glmRowVersion");
				eRPGLJournalMemoInformationDto.glmGlJournalMemoID = dataTable.Rows[i].Field<short>("glmGlJournalMemoID");
				eRPGLJournalMemoInformationDto.glmShortDescription = dataTable.Rows[i].Field<string>("glmShortDescription");
				eRPGLJournalMemoInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPGLJournalMemoInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPGLJournalMemoInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPGLJournalMemoInformationDto> GetGLJournalMemo(Guid gLJournalMemoId)
	{
		ERPGLJournalMemoInformationDto eRPGLJournalMemoInformationDto = new ERPGLJournalMemoInformationDto();
		InitializeParameterLists();
		string[] collection = new string[11]
		{
			"glmCreatedBy", "glmCreatedDate", "glmUniqueID", "glmGlJournalID", "glmClosed", "glmLongDescriptionRtf", "glmLongDescriptionText", "glmMemoDate", "glmRowVersion", "glmGlJournalMemoID",
			"glmShortDescription"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("glmUniqueID|C", gLJournalMemoId);
		AddCustomFieldsToSelectList("GLJournalMemos");
		using (DataTable dataTable = GetAsDataTable("GLJournalMemos", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPGLJournalMemoInformationDto);
			}
			eRPGLJournalMemoInformationDto.glmCreatedBy = dataTable.Rows[0].Field<string>("glmCreatedBy");
			eRPGLJournalMemoInformationDto.glmCreatedDate = dataTable.Rows[0].Field<DateTime?>("glmCreatedDate");
			eRPGLJournalMemoInformationDto.glmUniqueID = dataTable.Rows[0].Field<Guid>("glmUniqueID");
			eRPGLJournalMemoInformationDto.glmGlJournalID = dataTable.Rows[0].Field<int>("glmGlJournalID");
			eRPGLJournalMemoInformationDto.glmClosed = dataTable.Rows[0].Field<bool>("glmClosed");
			eRPGLJournalMemoInformationDto.glmLongDescriptionRtf = dataTable.Rows[0].Field<string>("glmLongDescriptionRtf");
			eRPGLJournalMemoInformationDto.glmLongDescriptionText = dataTable.Rows[0].Field<string>("glmLongDescriptionText");
			eRPGLJournalMemoInformationDto.glmMemoDate = dataTable.Rows[0].Field<DateTime?>("glmMemoDate");
			eRPGLJournalMemoInformationDto.glmRowVersion = dataTable.Rows[0].Field<byte[]>("glmRowVersion");
			eRPGLJournalMemoInformationDto.glmGlJournalMemoID = dataTable.Rows[0].Field<short>("glmGlJournalMemoID");
			eRPGLJournalMemoInformationDto.glmShortDescription = dataTable.Rows[0].Field<string>("glmShortDescription");
			eRPGLJournalMemoInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPGLJournalMemoInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPGLJournalMemoInformationDto);
	}

	public Task<APIValidationInfoDto> SaveGLJournalMemo(ERPGLJournalMemoDto gLJournalMemo)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM GLJournalMemos WHERE glmUniqueID = " + M1Util.ConvertToLinq(gLJournalMemo.glmUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["glmGlJournalID"] = gLJournalMemo.glmGlJournalID;
				dataRow["glmGlJournalMemoID"] = gLJournalMemo.glmGlJournalMemoID;
				gLJournalMemo.glmUniqueID = ((gLJournalMemo.glmUniqueID == Guid.Empty) ? Guid.NewGuid() : gLJournalMemo.glmUniqueID);
				dataRow["glmUniqueID"] = gLJournalMemo.glmUniqueID;
				dataRow["glmCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["glmCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The GLJournalMemo could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (gLJournalMemo.glmRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the GLJournalMemo is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["glmRowVersion"], gLJournalMemo.glmRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the GLJournalMemo has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the GLJournalMemo again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["glmClosed"] = gLJournalMemo.glmClosed;
			dataRow["glmLongDescriptionRtf"] = gLJournalMemo.glmLongDescriptionRtf ?? dataRow["glmLongDescriptionRtf"];
			dataRow["glmLongDescriptionText"] = gLJournalMemo.glmLongDescriptionText ?? dataRow["glmLongDescriptionText"];
			DataRow dataRow2 = dataRow;
			DateTime? glmMemoDate = gLJournalMemo.glmMemoDate;
			dataRow2["glmMemoDate"] = (glmMemoDate.HasValue ? ((object)glmMemoDate.GetValueOrDefault()) : dataRow["glmMemoDate"]);
			dataRow["glmShortDescription"] = gLJournalMemo.glmShortDescription;
			if (gLJournalMemo.CustomFields != null && gLJournalMemo.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in gLJournalMemo.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the GLJournalMemo [{gLJournalMemo.glmUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the GLJournalMemo [{gLJournalMemo.glmUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
