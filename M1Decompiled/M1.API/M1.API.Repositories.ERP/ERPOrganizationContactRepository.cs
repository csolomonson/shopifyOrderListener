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

public class ERPOrganizationContactRepository : APIBaseRepository, IERPOrganizationContactRepository, IAPIBaseRepository, IDisposable
{
	public ERPOrganizationContactRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesOrganizationContactExist(Guid organizationContactId)
	{
		InitializeParameterLists();
		base.filterList.Add("cmcUniqueID|C", organizationContactId);
		base.selectList.Add("cmcUniqueID");
		return Task.FromResult(GetAsObject("OrganizationContacts", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPOrganizationContactInformationDto>> GetAllOrganizationContacts(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPOrganizationContactInformationDto> collection = new List<ERPOrganizationContactInformationDto>();
		InitializeParameterLists();
		string[] array = new string[21]
		{
			"cmcAlternatePhoneNumber", "cmcContactID", "cmcContactTitleID", "cmcCorrespondenceMethod", "cmcCreatedBy", "cmcCreatedDate", "cmcEmailAddress", "cmcUniqueID", "cmcFaxNumber", "cmcInactiveDate",
			"cmcInactive", "cmcCreatedFromMobile", "cmcNoMailings", "cmcLocationID", "cmcMobileNumber", "cmcName", "cmcNoteRtf", "cmcNoteText", "cmcOrganizationID", "cmcPhoneNumber",
			"cmcRowVersion"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("OrganizationContacts");
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
		using (DataTable dataTable = GetAsDataTable("OrganizationContacts", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPOrganizationContactInformationDto eRPOrganizationContactInformationDto = new ERPOrganizationContactInformationDto();
				eRPOrganizationContactInformationDto.cmcAlternatePhoneNumber = dataTable.Rows[i].Field<string>("cmcAlternatePhoneNumber");
				eRPOrganizationContactInformationDto.cmcContactID = dataTable.Rows[i].Field<string>("cmcContactID");
				eRPOrganizationContactInformationDto.cmcContactTitleID = dataTable.Rows[i].Field<string>("cmcContactTitleID");
				eRPOrganizationContactInformationDto.cmcCorrespondenceMethod = dataTable.Rows[i].Field<string>("cmcCorrespondenceMethod");
				eRPOrganizationContactInformationDto.cmcCreatedBy = dataTable.Rows[i].Field<string>("cmcCreatedBy");
				eRPOrganizationContactInformationDto.cmcCreatedDate = dataTable.Rows[i].Field<DateTime?>("cmcCreatedDate");
				eRPOrganizationContactInformationDto.cmcEmailAddress = dataTable.Rows[i].Field<string>("cmcEmailAddress");
				eRPOrganizationContactInformationDto.cmcUniqueID = dataTable.Rows[i].Field<Guid>("cmcUniqueID");
				eRPOrganizationContactInformationDto.cmcFaxNumber = dataTable.Rows[i].Field<string>("cmcFaxNumber");
				eRPOrganizationContactInformationDto.cmcInactiveDate = dataTable.Rows[i].Field<DateTime?>("cmcInactiveDate");
				eRPOrganizationContactInformationDto.cmcInactive = dataTable.Rows[i].Field<bool>("cmcInactive");
				eRPOrganizationContactInformationDto.cmcCreatedFromMobile = dataTable.Rows[i].Field<bool>("cmcCreatedFromMobile");
				eRPOrganizationContactInformationDto.cmcNoMailings = dataTable.Rows[i].Field<bool>("cmcNoMailings");
				eRPOrganizationContactInformationDto.cmcLocationID = dataTable.Rows[i].Field<string>("cmcLocationID");
				eRPOrganizationContactInformationDto.cmcMobileNumber = dataTable.Rows[i].Field<string>("cmcMobileNumber");
				eRPOrganizationContactInformationDto.cmcName = dataTable.Rows[i].Field<string>("cmcName");
				eRPOrganizationContactInformationDto.cmcNoteRtf = dataTable.Rows[i].Field<string>("cmcNoteRtf");
				eRPOrganizationContactInformationDto.cmcNoteText = dataTable.Rows[i].Field<string>("cmcNoteText");
				eRPOrganizationContactInformationDto.cmcOrganizationID = dataTable.Rows[i].Field<string>("cmcOrganizationID");
				eRPOrganizationContactInformationDto.cmcPhoneNumber = dataTable.Rows[i].Field<string>("cmcPhoneNumber");
				eRPOrganizationContactInformationDto.cmcRowVersion = dataTable.Rows[i].Field<byte[]>("cmcRowVersion");
				eRPOrganizationContactInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPOrganizationContactInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPOrganizationContactInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPOrganizationContactInformationDto> GetOrganizationContact(Guid organizationContactId)
	{
		ERPOrganizationContactInformationDto eRPOrganizationContactInformationDto = new ERPOrganizationContactInformationDto();
		InitializeParameterLists();
		string[] collection = new string[21]
		{
			"cmcAlternatePhoneNumber", "cmcContactID", "cmcContactTitleID", "cmcCorrespondenceMethod", "cmcCreatedBy", "cmcCreatedDate", "cmcEmailAddress", "cmcUniqueID", "cmcFaxNumber", "cmcInactiveDate",
			"cmcInactive", "cmcCreatedFromMobile", "cmcNoMailings", "cmcLocationID", "cmcMobileNumber", "cmcName", "cmcNoteRtf", "cmcNoteText", "cmcOrganizationID", "cmcPhoneNumber",
			"cmcRowVersion"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("cmcUniqueID|C", organizationContactId);
		AddCustomFieldsToSelectList("OrganizationContacts");
		using (DataTable dataTable = GetAsDataTable("OrganizationContacts", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPOrganizationContactInformationDto);
			}
			eRPOrganizationContactInformationDto.cmcAlternatePhoneNumber = dataTable.Rows[0].Field<string>("cmcAlternatePhoneNumber");
			eRPOrganizationContactInformationDto.cmcContactID = dataTable.Rows[0].Field<string>("cmcContactID");
			eRPOrganizationContactInformationDto.cmcContactTitleID = dataTable.Rows[0].Field<string>("cmcContactTitleID");
			eRPOrganizationContactInformationDto.cmcCorrespondenceMethod = dataTable.Rows[0].Field<string>("cmcCorrespondenceMethod");
			eRPOrganizationContactInformationDto.cmcCreatedBy = dataTable.Rows[0].Field<string>("cmcCreatedBy");
			eRPOrganizationContactInformationDto.cmcCreatedDate = dataTable.Rows[0].Field<DateTime?>("cmcCreatedDate");
			eRPOrganizationContactInformationDto.cmcEmailAddress = dataTable.Rows[0].Field<string>("cmcEmailAddress");
			eRPOrganizationContactInformationDto.cmcUniqueID = dataTable.Rows[0].Field<Guid>("cmcUniqueID");
			eRPOrganizationContactInformationDto.cmcFaxNumber = dataTable.Rows[0].Field<string>("cmcFaxNumber");
			eRPOrganizationContactInformationDto.cmcInactiveDate = dataTable.Rows[0].Field<DateTime?>("cmcInactiveDate");
			eRPOrganizationContactInformationDto.cmcInactive = dataTable.Rows[0].Field<bool>("cmcInactive");
			eRPOrganizationContactInformationDto.cmcCreatedFromMobile = dataTable.Rows[0].Field<bool>("cmcCreatedFromMobile");
			eRPOrganizationContactInformationDto.cmcNoMailings = dataTable.Rows[0].Field<bool>("cmcNoMailings");
			eRPOrganizationContactInformationDto.cmcLocationID = dataTable.Rows[0].Field<string>("cmcLocationID");
			eRPOrganizationContactInformationDto.cmcMobileNumber = dataTable.Rows[0].Field<string>("cmcMobileNumber");
			eRPOrganizationContactInformationDto.cmcName = dataTable.Rows[0].Field<string>("cmcName");
			eRPOrganizationContactInformationDto.cmcNoteRtf = dataTable.Rows[0].Field<string>("cmcNoteRtf");
			eRPOrganizationContactInformationDto.cmcNoteText = dataTable.Rows[0].Field<string>("cmcNoteText");
			eRPOrganizationContactInformationDto.cmcOrganizationID = dataTable.Rows[0].Field<string>("cmcOrganizationID");
			eRPOrganizationContactInformationDto.cmcPhoneNumber = dataTable.Rows[0].Field<string>("cmcPhoneNumber");
			eRPOrganizationContactInformationDto.cmcRowVersion = dataTable.Rows[0].Field<byte[]>("cmcRowVersion");
			eRPOrganizationContactInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPOrganizationContactInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPOrganizationContactInformationDto);
	}

	public Task<APIValidationInfoDto> SaveOrganizationContact(ERPOrganizationContactDto organizationContact)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM OrganizationContacts WHERE cmcUniqueID = " + M1Util.ConvertToLinq(organizationContact.cmcUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["cmcOrganizationID"] = organizationContact.cmcOrganizationID.ToUpper();
				dataRow["cmcLocationID"] = organizationContact.cmcLocationID.ToUpper();
				dataRow["cmcContactID"] = organizationContact.cmcContactID.ToUpper();
				organizationContact.cmcUniqueID = ((organizationContact.cmcUniqueID == Guid.Empty) ? Guid.NewGuid() : organizationContact.cmcUniqueID);
				dataRow["cmcUniqueID"] = organizationContact.cmcUniqueID;
				dataRow["cmcCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["cmcCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The OrganizationContact could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (organizationContact.cmcRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the OrganizationContact is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["cmcRowVersion"], organizationContact.cmcRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the OrganizationContact has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the OrganizationContact again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["cmcAlternatePhoneNumber"] = organizationContact.cmcAlternatePhoneNumber;
			dataRow["cmcContactTitleID"] = organizationContact.cmcContactTitleID;
			dataRow["cmcCorrespondenceMethod"] = organizationContact.cmcCorrespondenceMethod;
			dataRow["cmcEmailAddress"] = organizationContact.cmcEmailAddress ?? dataRow["cmcEmailAddress"];
			dataRow["cmcFaxNumber"] = organizationContact.cmcFaxNumber;
			DataRow dataRow2 = dataRow;
			DateTime? cmcInactiveDate = organizationContact.cmcInactiveDate;
			dataRow2["cmcInactiveDate"] = (cmcInactiveDate.HasValue ? ((object)cmcInactiveDate.GetValueOrDefault()) : dataRow["cmcInactiveDate"]);
			dataRow["cmcInactive"] = organizationContact.cmcInactive;
			dataRow["cmcCreatedFromMobile"] = organizationContact.cmcCreatedFromMobile;
			dataRow["cmcNoMailings"] = organizationContact.cmcNoMailings;
			dataRow["cmcMobileNumber"] = organizationContact.cmcMobileNumber;
			dataRow["cmcName"] = organizationContact.cmcName;
			dataRow["cmcNoteRtf"] = organizationContact.cmcNoteRtf ?? dataRow["cmcNoteRtf"];
			dataRow["cmcNoteText"] = organizationContact.cmcNoteText ?? dataRow["cmcNoteText"];
			dataRow["cmcPhoneNumber"] = organizationContact.cmcPhoneNumber;
			if (organizationContact.CustomFields != null && organizationContact.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in organizationContact.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the OrganizationContact [{organizationContact.cmcUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the OrganizationContact [{organizationContact.cmcUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
