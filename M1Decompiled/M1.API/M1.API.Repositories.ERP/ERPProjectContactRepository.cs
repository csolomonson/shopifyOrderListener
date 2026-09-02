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

public class ERPProjectContactRepository : APIBaseRepository, IERPProjectContactRepository, IAPIBaseRepository, IDisposable
{
	public ERPProjectContactRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesProjectContactExist(Guid projectContactId)
	{
		InitializeParameterLists();
		base.filterList.Add("prcUniqueID|C", projectContactId);
		base.selectList.Add("prcUniqueID");
		return Task.FromResult(GetAsObject("ProjectContacts", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPProjectContactInformationDto>> GetAllProjectContacts(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPProjectContactInformationDto> collection = new List<ERPProjectContactInformationDto>();
		InitializeParameterLists();
		string[] array = new string[12]
		{
			"prcContactID", "prcContactTitleID", "prcCreatedBy", "prcCreatedDate", "prcUniqueID", "prcLocationID", "prcNotesRTF", "prcNotesText", "prcOrganizationID", "prcProjectID",
			"prcRowVersion", "prcProjectContactID"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("ProjectContacts");
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
		using (DataTable dataTable = GetAsDataTable("ProjectContacts", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPProjectContactInformationDto eRPProjectContactInformationDto = new ERPProjectContactInformationDto();
				eRPProjectContactInformationDto.prcContactID = dataTable.Rows[i].Field<string>("prcContactID");
				eRPProjectContactInformationDto.prcContactTitleID = dataTable.Rows[i].Field<string>("prcContactTitleID");
				eRPProjectContactInformationDto.prcCreatedBy = dataTable.Rows[i].Field<string>("prcCreatedBy");
				eRPProjectContactInformationDto.prcCreatedDate = dataTable.Rows[i].Field<DateTime?>("prcCreatedDate");
				eRPProjectContactInformationDto.prcUniqueID = dataTable.Rows[i].Field<Guid>("prcUniqueID");
				eRPProjectContactInformationDto.prcLocationID = dataTable.Rows[i].Field<string>("prcLocationID");
				eRPProjectContactInformationDto.prcNotesRTF = dataTable.Rows[i].Field<string>("prcNotesRTF");
				eRPProjectContactInformationDto.prcNotesText = dataTable.Rows[i].Field<string>("prcNotesText");
				eRPProjectContactInformationDto.prcOrganizationID = dataTable.Rows[i].Field<string>("prcOrganizationID");
				eRPProjectContactInformationDto.prcProjectID = dataTable.Rows[i].Field<string>("prcProjectID");
				eRPProjectContactInformationDto.prcRowVersion = dataTable.Rows[i].Field<byte[]>("prcRowVersion");
				eRPProjectContactInformationDto.prcProjectContactID = dataTable.Rows[i].Field<short>("prcProjectContactID");
				eRPProjectContactInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPProjectContactInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPProjectContactInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPProjectContactInformationDto> GetProjectContact(Guid projectContactId)
	{
		ERPProjectContactInformationDto eRPProjectContactInformationDto = new ERPProjectContactInformationDto();
		InitializeParameterLists();
		string[] collection = new string[12]
		{
			"prcContactID", "prcContactTitleID", "prcCreatedBy", "prcCreatedDate", "prcUniqueID", "prcLocationID", "prcNotesRTF", "prcNotesText", "prcOrganizationID", "prcProjectID",
			"prcRowVersion", "prcProjectContactID"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("prcUniqueID|C", projectContactId);
		AddCustomFieldsToSelectList("ProjectContacts");
		using (DataTable dataTable = GetAsDataTable("ProjectContacts", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPProjectContactInformationDto);
			}
			eRPProjectContactInformationDto.prcContactID = dataTable.Rows[0].Field<string>("prcContactID");
			eRPProjectContactInformationDto.prcContactTitleID = dataTable.Rows[0].Field<string>("prcContactTitleID");
			eRPProjectContactInformationDto.prcCreatedBy = dataTable.Rows[0].Field<string>("prcCreatedBy");
			eRPProjectContactInformationDto.prcCreatedDate = dataTable.Rows[0].Field<DateTime?>("prcCreatedDate");
			eRPProjectContactInformationDto.prcUniqueID = dataTable.Rows[0].Field<Guid>("prcUniqueID");
			eRPProjectContactInformationDto.prcLocationID = dataTable.Rows[0].Field<string>("prcLocationID");
			eRPProjectContactInformationDto.prcNotesRTF = dataTable.Rows[0].Field<string>("prcNotesRTF");
			eRPProjectContactInformationDto.prcNotesText = dataTable.Rows[0].Field<string>("prcNotesText");
			eRPProjectContactInformationDto.prcOrganizationID = dataTable.Rows[0].Field<string>("prcOrganizationID");
			eRPProjectContactInformationDto.prcProjectID = dataTable.Rows[0].Field<string>("prcProjectID");
			eRPProjectContactInformationDto.prcRowVersion = dataTable.Rows[0].Field<byte[]>("prcRowVersion");
			eRPProjectContactInformationDto.prcProjectContactID = dataTable.Rows[0].Field<short>("prcProjectContactID");
			eRPProjectContactInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPProjectContactInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPProjectContactInformationDto);
	}

	public Task<APIValidationInfoDto> SaveProjectContact(ERPProjectContactDto projectContact)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM ProjectContacts WHERE prcUniqueID = " + M1Util.ConvertToLinq(projectContact.prcUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["prcProjectID"] = projectContact.prcProjectID.ToUpper();
				dataRow["prcProjectContactID"] = projectContact.prcProjectContactID;
				projectContact.prcUniqueID = ((projectContact.prcUniqueID == Guid.Empty) ? Guid.NewGuid() : projectContact.prcUniqueID);
				dataRow["prcUniqueID"] = projectContact.prcUniqueID;
				dataRow["prcCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["prcCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The ProjectContact could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (projectContact.prcRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the ProjectContact is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["prcRowVersion"], projectContact.prcRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the ProjectContact has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the ProjectContact again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["prcContactID"] = projectContact.prcContactID;
			dataRow["prcContactTitleID"] = projectContact.prcContactTitleID;
			dataRow["prcLocationID"] = projectContact.prcLocationID;
			dataRow["prcNotesRTF"] = projectContact.prcNotesRTF ?? dataRow["prcNotesRTF"];
			dataRow["prcNotesText"] = projectContact.prcNotesText ?? dataRow["prcNotesText"];
			dataRow["prcOrganizationID"] = projectContact.prcOrganizationID;
			if (projectContact.CustomFields != null && projectContact.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in projectContact.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the ProjectContact [{projectContact.prcUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the ProjectContact [{projectContact.prcUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
