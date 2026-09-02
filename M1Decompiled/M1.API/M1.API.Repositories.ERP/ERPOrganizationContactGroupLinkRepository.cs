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

public class ERPOrganizationContactGroupLinkRepository : APIBaseRepository, IERPOrganizationContactGroupLinkRepository, IAPIBaseRepository, IDisposable
{
	public ERPOrganizationContactGroupLinkRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesOrganizationContactGroupLinkExist(Guid organizationContactGroupLinkId)
	{
		InitializeParameterLists();
		base.filterList.Add("cmrUniqueID|C", organizationContactGroupLinkId);
		base.selectList.Add("cmrUniqueID");
		return Task.FromResult(GetAsObject("OrganizationContactGroupLinks", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPOrganizationContactGroupLinkInformationDto>> GetAllOrganizationContactGroupLinks(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPOrganizationContactGroupLinkInformationDto> collection = new List<ERPOrganizationContactGroupLinkInformationDto>();
		InitializeParameterLists();
		string[] array = new string[9] { "cmrContactGroupID", "cmrContactGroupLinkID", "cmrContactID", "cmrCreatedBy", "cmrCreatedDate", "cmrUniqueID", "cmrLocationID", "cmrOrganizationID", "cmrRowVersion" };
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("OrganizationContactGroupLinks");
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
		using (DataTable dataTable = GetAsDataTable("OrganizationContactGroupLinks", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPOrganizationContactGroupLinkInformationDto eRPOrganizationContactGroupLinkInformationDto = new ERPOrganizationContactGroupLinkInformationDto();
				eRPOrganizationContactGroupLinkInformationDto.cmrContactGroupID = dataTable.Rows[i].Field<string>("cmrContactGroupID");
				eRPOrganizationContactGroupLinkInformationDto.cmrContactGroupLinkID = dataTable.Rows[i].Field<short>("cmrContactGroupLinkID");
				eRPOrganizationContactGroupLinkInformationDto.cmrContactID = dataTable.Rows[i].Field<string>("cmrContactID");
				eRPOrganizationContactGroupLinkInformationDto.cmrCreatedBy = dataTable.Rows[i].Field<string>("cmrCreatedBy");
				eRPOrganizationContactGroupLinkInformationDto.cmrCreatedDate = dataTable.Rows[i].Field<DateTime?>("cmrCreatedDate");
				eRPOrganizationContactGroupLinkInformationDto.cmrUniqueID = dataTable.Rows[i].Field<Guid>("cmrUniqueID");
				eRPOrganizationContactGroupLinkInformationDto.cmrLocationID = dataTable.Rows[i].Field<string>("cmrLocationID");
				eRPOrganizationContactGroupLinkInformationDto.cmrOrganizationID = dataTable.Rows[i].Field<string>("cmrOrganizationID");
				eRPOrganizationContactGroupLinkInformationDto.cmrRowVersion = dataTable.Rows[i].Field<byte[]>("cmrRowVersion");
				eRPOrganizationContactGroupLinkInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPOrganizationContactGroupLinkInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPOrganizationContactGroupLinkInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPOrganizationContactGroupLinkInformationDto> GetOrganizationContactGroupLink(Guid organizationContactGroupLinkId)
	{
		ERPOrganizationContactGroupLinkInformationDto eRPOrganizationContactGroupLinkInformationDto = new ERPOrganizationContactGroupLinkInformationDto();
		InitializeParameterLists();
		string[] collection = new string[9] { "cmrContactGroupID", "cmrContactGroupLinkID", "cmrContactID", "cmrCreatedBy", "cmrCreatedDate", "cmrUniqueID", "cmrLocationID", "cmrOrganizationID", "cmrRowVersion" };
		base.selectList.AddRange(collection);
		base.filterList.Add("cmrUniqueID|C", organizationContactGroupLinkId);
		AddCustomFieldsToSelectList("OrganizationContactGroupLinks");
		using (DataTable dataTable = GetAsDataTable("OrganizationContactGroupLinks", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPOrganizationContactGroupLinkInformationDto);
			}
			eRPOrganizationContactGroupLinkInformationDto.cmrContactGroupID = dataTable.Rows[0].Field<string>("cmrContactGroupID");
			eRPOrganizationContactGroupLinkInformationDto.cmrContactGroupLinkID = dataTable.Rows[0].Field<short>("cmrContactGroupLinkID");
			eRPOrganizationContactGroupLinkInformationDto.cmrContactID = dataTable.Rows[0].Field<string>("cmrContactID");
			eRPOrganizationContactGroupLinkInformationDto.cmrCreatedBy = dataTable.Rows[0].Field<string>("cmrCreatedBy");
			eRPOrganizationContactGroupLinkInformationDto.cmrCreatedDate = dataTable.Rows[0].Field<DateTime?>("cmrCreatedDate");
			eRPOrganizationContactGroupLinkInformationDto.cmrUniqueID = dataTable.Rows[0].Field<Guid>("cmrUniqueID");
			eRPOrganizationContactGroupLinkInformationDto.cmrLocationID = dataTable.Rows[0].Field<string>("cmrLocationID");
			eRPOrganizationContactGroupLinkInformationDto.cmrOrganizationID = dataTable.Rows[0].Field<string>("cmrOrganizationID");
			eRPOrganizationContactGroupLinkInformationDto.cmrRowVersion = dataTable.Rows[0].Field<byte[]>("cmrRowVersion");
			eRPOrganizationContactGroupLinkInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPOrganizationContactGroupLinkInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPOrganizationContactGroupLinkInformationDto);
	}

	public Task<APIValidationInfoDto> SaveOrganizationContactGroupLink(ERPOrganizationContactGroupLinkDto organizationContactGroupLink)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM OrganizationContactGroupLinks WHERE cmrUniqueID = " + M1Util.ConvertToLinq(organizationContactGroupLink.cmrUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["cmrOrganizationID"] = organizationContactGroupLink.cmrOrganizationID.ToUpper();
				dataRow["cmrLocationID"] = organizationContactGroupLink.cmrLocationID.ToUpper();
				dataRow["cmrContactID"] = organizationContactGroupLink.cmrContactID.ToUpper();
				dataRow["cmrContactGroupLinkID"] = organizationContactGroupLink.cmrContactGroupLinkID;
				organizationContactGroupLink.cmrUniqueID = ((organizationContactGroupLink.cmrUniqueID == Guid.Empty) ? Guid.NewGuid() : organizationContactGroupLink.cmrUniqueID);
				dataRow["cmrUniqueID"] = organizationContactGroupLink.cmrUniqueID;
				dataRow["cmrCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["cmrCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The OrganizationContactGroupLink could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (organizationContactGroupLink.cmrRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the OrganizationContactGroupLink is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["cmrRowVersion"], organizationContactGroupLink.cmrRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the OrganizationContactGroupLink has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the OrganizationContactGroupLink again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["cmrContactGroupID"] = organizationContactGroupLink.cmrContactGroupID;
			if (organizationContactGroupLink.CustomFields != null && organizationContactGroupLink.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in organizationContactGroupLink.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the OrganizationContactGroupLink [{organizationContactGroupLink.cmrUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the OrganizationContactGroupLink [{organizationContactGroupLink.cmrUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
