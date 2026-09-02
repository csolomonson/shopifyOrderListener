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

public class ERPKnowledgeBasePageRepository : APIBaseRepository, IERPKnowledgeBasePageRepository, IAPIBaseRepository, IDisposable
{
	public ERPKnowledgeBasePageRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesKnowledgeBasePageExist(Guid knowledgeBasePageId)
	{
		InitializeParameterLists();
		base.filterList.Add("kbbUniqueID|C", knowledgeBasePageId);
		base.selectList.Add("kbbUniqueID");
		return Task.FromResult(GetAsObject("KnowledgeBasePages", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPKnowledgeBasePageInformationDto>> GetAllKnowledgeBasePages(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPKnowledgeBasePageInformationDto> collection = new List<ERPKnowledgeBasePageInformationDto>();
		InitializeParameterLists();
		string[] array = new string[22]
		{
			"kbbAccessedCount", "kbbClosedByEmployeeID", "kbbClosedDate", "kbbKnowledgeBasePageID", "kbbCreatedBy", "kbbCreatedDate", "kbbDescription", "kbbUniqueID", "kbbOpenedByEmployeeID", "kbbOpenedDate",
			"kbbPartID", "kbbPartRevisionID", "kbbProblemDescriptionRtf", "kbbProblemDescriptionText", "kbbResolutionDescriptionRtf", "kbbResolutionDescriptionText", "kbbResolvedPartID", "kbbResolvedPartRevisionID", "kbbRowVersion", "kbbStatus",
			"kbbWorkAroundDescriptionRtf", "kbbWorkAroundDescriptionText"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("KnowledgeBasePages");
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
		using (DataTable dataTable = GetAsDataTable("KnowledgeBasePages", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPKnowledgeBasePageInformationDto eRPKnowledgeBasePageInformationDto = new ERPKnowledgeBasePageInformationDto();
				eRPKnowledgeBasePageInformationDto.kbbAccessedCount = dataTable.Rows[i].Field<decimal>("kbbAccessedCount");
				eRPKnowledgeBasePageInformationDto.kbbClosedByEmployeeID = dataTable.Rows[i].Field<string>("kbbClosedByEmployeeID");
				eRPKnowledgeBasePageInformationDto.kbbClosedDate = dataTable.Rows[i].Field<DateTime?>("kbbClosedDate");
				eRPKnowledgeBasePageInformationDto.kbbKnowledgeBasePageID = dataTable.Rows[i].Field<string>("kbbKnowledgeBasePageID");
				eRPKnowledgeBasePageInformationDto.kbbCreatedBy = dataTable.Rows[i].Field<string>("kbbCreatedBy");
				eRPKnowledgeBasePageInformationDto.kbbCreatedDate = dataTable.Rows[i].Field<DateTime?>("kbbCreatedDate");
				eRPKnowledgeBasePageInformationDto.kbbDescription = dataTable.Rows[i].Field<string>("kbbDescription");
				eRPKnowledgeBasePageInformationDto.kbbUniqueID = dataTable.Rows[i].Field<Guid>("kbbUniqueID");
				eRPKnowledgeBasePageInformationDto.kbbOpenedByEmployeeID = dataTable.Rows[i].Field<string>("kbbOpenedByEmployeeID");
				eRPKnowledgeBasePageInformationDto.kbbOpenedDate = dataTable.Rows[i].Field<DateTime?>("kbbOpenedDate");
				eRPKnowledgeBasePageInformationDto.kbbPartID = dataTable.Rows[i].Field<string>("kbbPartID");
				eRPKnowledgeBasePageInformationDto.kbbPartRevisionID = dataTable.Rows[i].Field<string>("kbbPartRevisionID");
				eRPKnowledgeBasePageInformationDto.kbbProblemDescriptionRtf = dataTable.Rows[i].Field<string>("kbbProblemDescriptionRtf");
				eRPKnowledgeBasePageInformationDto.kbbProblemDescriptionText = dataTable.Rows[i].Field<string>("kbbProblemDescriptionText");
				eRPKnowledgeBasePageInformationDto.kbbResolutionDescriptionRtf = dataTable.Rows[i].Field<string>("kbbResolutionDescriptionRtf");
				eRPKnowledgeBasePageInformationDto.kbbResolutionDescriptionText = dataTable.Rows[i].Field<string>("kbbResolutionDescriptionText");
				eRPKnowledgeBasePageInformationDto.kbbResolvedPartID = dataTable.Rows[i].Field<string>("kbbResolvedPartID");
				eRPKnowledgeBasePageInformationDto.kbbResolvedPartRevisionID = dataTable.Rows[i].Field<string>("kbbResolvedPartRevisionID");
				eRPKnowledgeBasePageInformationDto.kbbRowVersion = dataTable.Rows[i].Field<byte[]>("kbbRowVersion");
				eRPKnowledgeBasePageInformationDto.kbbStatus = dataTable.Rows[i].Field<string>("kbbStatus");
				eRPKnowledgeBasePageInformationDto.kbbWorkAroundDescriptionRtf = dataTable.Rows[i].Field<string>("kbbWorkAroundDescriptionRtf");
				eRPKnowledgeBasePageInformationDto.kbbWorkAroundDescriptionText = dataTable.Rows[i].Field<string>("kbbWorkAroundDescriptionText");
				eRPKnowledgeBasePageInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPKnowledgeBasePageInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPKnowledgeBasePageInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPKnowledgeBasePageInformationDto> GetKnowledgeBasePage(Guid knowledgeBasePageId)
	{
		ERPKnowledgeBasePageInformationDto eRPKnowledgeBasePageInformationDto = new ERPKnowledgeBasePageInformationDto();
		InitializeParameterLists();
		string[] collection = new string[22]
		{
			"kbbAccessedCount", "kbbClosedByEmployeeID", "kbbClosedDate", "kbbKnowledgeBasePageID", "kbbCreatedBy", "kbbCreatedDate", "kbbDescription", "kbbUniqueID", "kbbOpenedByEmployeeID", "kbbOpenedDate",
			"kbbPartID", "kbbPartRevisionID", "kbbProblemDescriptionRtf", "kbbProblemDescriptionText", "kbbResolutionDescriptionRtf", "kbbResolutionDescriptionText", "kbbResolvedPartID", "kbbResolvedPartRevisionID", "kbbRowVersion", "kbbStatus",
			"kbbWorkAroundDescriptionRtf", "kbbWorkAroundDescriptionText"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("kbbUniqueID|C", knowledgeBasePageId);
		AddCustomFieldsToSelectList("KnowledgeBasePages");
		using (DataTable dataTable = GetAsDataTable("KnowledgeBasePages", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPKnowledgeBasePageInformationDto);
			}
			eRPKnowledgeBasePageInformationDto.kbbAccessedCount = dataTable.Rows[0].Field<decimal>("kbbAccessedCount");
			eRPKnowledgeBasePageInformationDto.kbbClosedByEmployeeID = dataTable.Rows[0].Field<string>("kbbClosedByEmployeeID");
			eRPKnowledgeBasePageInformationDto.kbbClosedDate = dataTable.Rows[0].Field<DateTime?>("kbbClosedDate");
			eRPKnowledgeBasePageInformationDto.kbbKnowledgeBasePageID = dataTable.Rows[0].Field<string>("kbbKnowledgeBasePageID");
			eRPKnowledgeBasePageInformationDto.kbbCreatedBy = dataTable.Rows[0].Field<string>("kbbCreatedBy");
			eRPKnowledgeBasePageInformationDto.kbbCreatedDate = dataTable.Rows[0].Field<DateTime?>("kbbCreatedDate");
			eRPKnowledgeBasePageInformationDto.kbbDescription = dataTable.Rows[0].Field<string>("kbbDescription");
			eRPKnowledgeBasePageInformationDto.kbbUniqueID = dataTable.Rows[0].Field<Guid>("kbbUniqueID");
			eRPKnowledgeBasePageInformationDto.kbbOpenedByEmployeeID = dataTable.Rows[0].Field<string>("kbbOpenedByEmployeeID");
			eRPKnowledgeBasePageInformationDto.kbbOpenedDate = dataTable.Rows[0].Field<DateTime?>("kbbOpenedDate");
			eRPKnowledgeBasePageInformationDto.kbbPartID = dataTable.Rows[0].Field<string>("kbbPartID");
			eRPKnowledgeBasePageInformationDto.kbbPartRevisionID = dataTable.Rows[0].Field<string>("kbbPartRevisionID");
			eRPKnowledgeBasePageInformationDto.kbbProblemDescriptionRtf = dataTable.Rows[0].Field<string>("kbbProblemDescriptionRtf");
			eRPKnowledgeBasePageInformationDto.kbbProblemDescriptionText = dataTable.Rows[0].Field<string>("kbbProblemDescriptionText");
			eRPKnowledgeBasePageInformationDto.kbbResolutionDescriptionRtf = dataTable.Rows[0].Field<string>("kbbResolutionDescriptionRtf");
			eRPKnowledgeBasePageInformationDto.kbbResolutionDescriptionText = dataTable.Rows[0].Field<string>("kbbResolutionDescriptionText");
			eRPKnowledgeBasePageInformationDto.kbbResolvedPartID = dataTable.Rows[0].Field<string>("kbbResolvedPartID");
			eRPKnowledgeBasePageInformationDto.kbbResolvedPartRevisionID = dataTable.Rows[0].Field<string>("kbbResolvedPartRevisionID");
			eRPKnowledgeBasePageInformationDto.kbbRowVersion = dataTable.Rows[0].Field<byte[]>("kbbRowVersion");
			eRPKnowledgeBasePageInformationDto.kbbStatus = dataTable.Rows[0].Field<string>("kbbStatus");
			eRPKnowledgeBasePageInformationDto.kbbWorkAroundDescriptionRtf = dataTable.Rows[0].Field<string>("kbbWorkAroundDescriptionRtf");
			eRPKnowledgeBasePageInformationDto.kbbWorkAroundDescriptionText = dataTable.Rows[0].Field<string>("kbbWorkAroundDescriptionText");
			eRPKnowledgeBasePageInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPKnowledgeBasePageInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPKnowledgeBasePageInformationDto);
	}

	public Task<APIValidationInfoDto> SaveKnowledgeBasePage(ERPKnowledgeBasePageDto knowledgeBasePage)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM KnowledgeBasePages WHERE kbbUniqueID = " + M1Util.ConvertToLinq(knowledgeBasePage.kbbUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["kbbKnowledgeBasePageID"] = knowledgeBasePage.kbbKnowledgeBasePageID.ToUpper();
				knowledgeBasePage.kbbUniqueID = ((knowledgeBasePage.kbbUniqueID == Guid.Empty) ? Guid.NewGuid() : knowledgeBasePage.kbbUniqueID);
				dataRow["kbbUniqueID"] = knowledgeBasePage.kbbUniqueID;
				dataRow["kbbCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["kbbCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The KnowledgeBasePage could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (knowledgeBasePage.kbbRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the KnowledgeBasePage is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["kbbRowVersion"], knowledgeBasePage.kbbRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the KnowledgeBasePage has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the KnowledgeBasePage again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["kbbAccessedCount"] = knowledgeBasePage.kbbAccessedCount;
			dataRow["kbbClosedByEmployeeID"] = knowledgeBasePage.kbbClosedByEmployeeID;
			DataRow dataRow2 = dataRow;
			DateTime? kbbClosedDate = knowledgeBasePage.kbbClosedDate;
			dataRow2["kbbClosedDate"] = (kbbClosedDate.HasValue ? ((object)kbbClosedDate.GetValueOrDefault()) : dataRow["kbbClosedDate"]);
			dataRow["kbbDescription"] = knowledgeBasePage.kbbDescription;
			dataRow["kbbOpenedByEmployeeID"] = knowledgeBasePage.kbbOpenedByEmployeeID;
			DataRow dataRow3 = dataRow;
			kbbClosedDate = knowledgeBasePage.kbbOpenedDate;
			dataRow3["kbbOpenedDate"] = (kbbClosedDate.HasValue ? ((object)kbbClosedDate.GetValueOrDefault()) : dataRow["kbbOpenedDate"]);
			dataRow["kbbPartID"] = knowledgeBasePage.kbbPartID;
			dataRow["kbbPartRevisionID"] = knowledgeBasePage.kbbPartRevisionID;
			dataRow["kbbProblemDescriptionRtf"] = knowledgeBasePage.kbbProblemDescriptionRtf ?? dataRow["kbbProblemDescriptionRtf"];
			dataRow["kbbProblemDescriptionText"] = knowledgeBasePage.kbbProblemDescriptionText ?? dataRow["kbbProblemDescriptionText"];
			dataRow["kbbResolutionDescriptionRtf"] = knowledgeBasePage.kbbResolutionDescriptionRtf ?? dataRow["kbbResolutionDescriptionRtf"];
			dataRow["kbbResolutionDescriptionText"] = knowledgeBasePage.kbbResolutionDescriptionText ?? dataRow["kbbResolutionDescriptionText"];
			dataRow["kbbResolvedPartID"] = knowledgeBasePage.kbbResolvedPartID;
			dataRow["kbbResolvedPartRevisionID"] = knowledgeBasePage.kbbResolvedPartRevisionID;
			dataRow["kbbStatus"] = knowledgeBasePage.kbbStatus;
			dataRow["kbbWorkAroundDescriptionRtf"] = knowledgeBasePage.kbbWorkAroundDescriptionRtf ?? dataRow["kbbWorkAroundDescriptionRtf"];
			dataRow["kbbWorkAroundDescriptionText"] = knowledgeBasePage.kbbWorkAroundDescriptionText ?? dataRow["kbbWorkAroundDescriptionText"];
			if (knowledgeBasePage.CustomFields != null && knowledgeBasePage.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in knowledgeBasePage.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the KnowledgeBasePage [{knowledgeBasePage.kbbUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the KnowledgeBasePage [{knowledgeBasePage.kbbUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
