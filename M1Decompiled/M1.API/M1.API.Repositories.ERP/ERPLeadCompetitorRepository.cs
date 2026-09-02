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

public class ERPLeadCompetitorRepository : APIBaseRepository, IERPLeadCompetitorRepository, IAPIBaseRepository, IDisposable
{
	public ERPLeadCompetitorRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesLeadCompetitorExist(Guid leadCompetitorId)
	{
		InitializeParameterLists();
		base.filterList.Add("locUniqueID|C", leadCompetitorId);
		base.selectList.Add("locUniqueID");
		return Task.FromResult(GetAsObject("LeadCompetitors", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPLeadCompetitorInformationDto>> GetAllLeadCompetitors(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPLeadCompetitorInformationDto> collection = new List<ERPLeadCompetitorInformationDto>();
		InitializeParameterLists();
		string[] array = new string[10] { "locCreatedBy", "locCreatedDate", "locUniqueID", "locLeadID", "locLeadNotesRTF", "locLeadNotesText", "locOrganizationID", "locProductName", "locRowVersion", "locLeadCompetitorID" };
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("LeadCompetitors");
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
		using (DataTable dataTable = GetAsDataTable("LeadCompetitors", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPLeadCompetitorInformationDto eRPLeadCompetitorInformationDto = new ERPLeadCompetitorInformationDto();
				eRPLeadCompetitorInformationDto.locCreatedBy = dataTable.Rows[i].Field<string>("locCreatedBy");
				eRPLeadCompetitorInformationDto.locCreatedDate = dataTable.Rows[i].Field<DateTime?>("locCreatedDate");
				eRPLeadCompetitorInformationDto.locUniqueID = dataTable.Rows[i].Field<Guid>("locUniqueID");
				eRPLeadCompetitorInformationDto.locLeadID = dataTable.Rows[i].Field<string>("locLeadID");
				eRPLeadCompetitorInformationDto.locLeadNotesRTF = dataTable.Rows[i].Field<string>("locLeadNotesRTF");
				eRPLeadCompetitorInformationDto.locLeadNotesText = dataTable.Rows[i].Field<string>("locLeadNotesText");
				eRPLeadCompetitorInformationDto.locOrganizationID = dataTable.Rows[i].Field<string>("locOrganizationID");
				eRPLeadCompetitorInformationDto.locProductName = dataTable.Rows[i].Field<string>("locProductName");
				eRPLeadCompetitorInformationDto.locRowVersion = dataTable.Rows[i].Field<byte[]>("locRowVersion");
				eRPLeadCompetitorInformationDto.locLeadCompetitorID = dataTable.Rows[i].Field<short>("locLeadCompetitorID");
				eRPLeadCompetitorInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPLeadCompetitorInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPLeadCompetitorInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPLeadCompetitorInformationDto> GetLeadCompetitor(Guid leadCompetitorId)
	{
		ERPLeadCompetitorInformationDto eRPLeadCompetitorInformationDto = new ERPLeadCompetitorInformationDto();
		InitializeParameterLists();
		string[] collection = new string[10] { "locCreatedBy", "locCreatedDate", "locUniqueID", "locLeadID", "locLeadNotesRTF", "locLeadNotesText", "locOrganizationID", "locProductName", "locRowVersion", "locLeadCompetitorID" };
		base.selectList.AddRange(collection);
		base.filterList.Add("locUniqueID|C", leadCompetitorId);
		AddCustomFieldsToSelectList("LeadCompetitors");
		using (DataTable dataTable = GetAsDataTable("LeadCompetitors", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPLeadCompetitorInformationDto);
			}
			eRPLeadCompetitorInformationDto.locCreatedBy = dataTable.Rows[0].Field<string>("locCreatedBy");
			eRPLeadCompetitorInformationDto.locCreatedDate = dataTable.Rows[0].Field<DateTime?>("locCreatedDate");
			eRPLeadCompetitorInformationDto.locUniqueID = dataTable.Rows[0].Field<Guid>("locUniqueID");
			eRPLeadCompetitorInformationDto.locLeadID = dataTable.Rows[0].Field<string>("locLeadID");
			eRPLeadCompetitorInformationDto.locLeadNotesRTF = dataTable.Rows[0].Field<string>("locLeadNotesRTF");
			eRPLeadCompetitorInformationDto.locLeadNotesText = dataTable.Rows[0].Field<string>("locLeadNotesText");
			eRPLeadCompetitorInformationDto.locOrganizationID = dataTable.Rows[0].Field<string>("locOrganizationID");
			eRPLeadCompetitorInformationDto.locProductName = dataTable.Rows[0].Field<string>("locProductName");
			eRPLeadCompetitorInformationDto.locRowVersion = dataTable.Rows[0].Field<byte[]>("locRowVersion");
			eRPLeadCompetitorInformationDto.locLeadCompetitorID = dataTable.Rows[0].Field<short>("locLeadCompetitorID");
			eRPLeadCompetitorInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPLeadCompetitorInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPLeadCompetitorInformationDto);
	}

	public Task<APIValidationInfoDto> SaveLeadCompetitor(ERPLeadCompetitorDto leadCompetitor)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM LeadCompetitors WHERE locUniqueID = " + M1Util.ConvertToLinq(leadCompetitor.locUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["locLeadID"] = leadCompetitor.locLeadID.ToUpper();
				dataRow["locLeadCompetitorID"] = leadCompetitor.locLeadCompetitorID;
				leadCompetitor.locUniqueID = ((leadCompetitor.locUniqueID == Guid.Empty) ? Guid.NewGuid() : leadCompetitor.locUniqueID);
				dataRow["locUniqueID"] = leadCompetitor.locUniqueID;
				dataRow["locCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["locCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The LeadCompetitor could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (leadCompetitor.locRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the LeadCompetitor is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["locRowVersion"], leadCompetitor.locRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the LeadCompetitor has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the LeadCompetitor again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["locLeadNotesRTF"] = leadCompetitor.locLeadNotesRTF ?? dataRow["locLeadNotesRTF"];
			dataRow["locLeadNotesText"] = leadCompetitor.locLeadNotesText ?? dataRow["locLeadNotesText"];
			dataRow["locOrganizationID"] = leadCompetitor.locOrganizationID;
			dataRow["locProductName"] = leadCompetitor.locProductName;
			if (leadCompetitor.CustomFields != null && leadCompetitor.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in leadCompetitor.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the LeadCompetitor [{leadCompetitor.locUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the LeadCompetitor [{leadCompetitor.locUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
