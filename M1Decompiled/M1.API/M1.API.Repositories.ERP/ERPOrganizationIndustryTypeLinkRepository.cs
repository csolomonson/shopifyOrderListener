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

public class ERPOrganizationIndustryTypeLinkRepository : APIBaseRepository, IERPOrganizationIndustryTypeLinkRepository, IAPIBaseRepository, IDisposable
{
	public ERPOrganizationIndustryTypeLinkRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesOrganizationIndustryTypeLinkExist(Guid organizationIndustryTypeLinkId)
	{
		InitializeParameterLists();
		base.filterList.Add("cmdUniqueID|C", organizationIndustryTypeLinkId);
		base.selectList.Add("cmdUniqueID");
		return Task.FromResult(GetAsObject("OrganizationIndustryTypeLinks", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPOrganizationIndustryTypeLinkInformationDto>> GetAllOrganizationIndustryTypeLinks(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPOrganizationIndustryTypeLinkInformationDto> collection = new List<ERPOrganizationIndustryTypeLinkInformationDto>();
		InitializeParameterLists();
		string[] array = new string[7] { "cmdCreatedBy", "cmdCreatedDate", "cmdUniqueID", "cmdIndustryTypeID", "cmdIndustryTypeLinkID", "cmdOrganizationID", "cmdRowVersion" };
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("OrganizationIndustryTypeLinks");
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
		using (DataTable dataTable = GetAsDataTable("OrganizationIndustryTypeLinks", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPOrganizationIndustryTypeLinkInformationDto eRPOrganizationIndustryTypeLinkInformationDto = new ERPOrganizationIndustryTypeLinkInformationDto();
				eRPOrganizationIndustryTypeLinkInformationDto.cmdCreatedBy = dataTable.Rows[i].Field<string>("cmdCreatedBy");
				eRPOrganizationIndustryTypeLinkInformationDto.cmdCreatedDate = dataTable.Rows[i].Field<DateTime?>("cmdCreatedDate");
				eRPOrganizationIndustryTypeLinkInformationDto.cmdUniqueID = dataTable.Rows[i].Field<Guid>("cmdUniqueID");
				eRPOrganizationIndustryTypeLinkInformationDto.cmdIndustryTypeID = dataTable.Rows[i].Field<string>("cmdIndustryTypeID");
				eRPOrganizationIndustryTypeLinkInformationDto.cmdIndustryTypeLinkID = dataTable.Rows[i].Field<short>("cmdIndustryTypeLinkID");
				eRPOrganizationIndustryTypeLinkInformationDto.cmdOrganizationID = dataTable.Rows[i].Field<string>("cmdOrganizationID");
				eRPOrganizationIndustryTypeLinkInformationDto.cmdRowVersion = dataTable.Rows[i].Field<byte[]>("cmdRowVersion");
				eRPOrganizationIndustryTypeLinkInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPOrganizationIndustryTypeLinkInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPOrganizationIndustryTypeLinkInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPOrganizationIndustryTypeLinkInformationDto> GetOrganizationIndustryTypeLink(Guid organizationIndustryTypeLinkId)
	{
		ERPOrganizationIndustryTypeLinkInformationDto eRPOrganizationIndustryTypeLinkInformationDto = new ERPOrganizationIndustryTypeLinkInformationDto();
		InitializeParameterLists();
		string[] collection = new string[7] { "cmdCreatedBy", "cmdCreatedDate", "cmdUniqueID", "cmdIndustryTypeID", "cmdIndustryTypeLinkID", "cmdOrganizationID", "cmdRowVersion" };
		base.selectList.AddRange(collection);
		base.filterList.Add("cmdUniqueID|C", organizationIndustryTypeLinkId);
		AddCustomFieldsToSelectList("OrganizationIndustryTypeLinks");
		using (DataTable dataTable = GetAsDataTable("OrganizationIndustryTypeLinks", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPOrganizationIndustryTypeLinkInformationDto);
			}
			eRPOrganizationIndustryTypeLinkInformationDto.cmdCreatedBy = dataTable.Rows[0].Field<string>("cmdCreatedBy");
			eRPOrganizationIndustryTypeLinkInformationDto.cmdCreatedDate = dataTable.Rows[0].Field<DateTime?>("cmdCreatedDate");
			eRPOrganizationIndustryTypeLinkInformationDto.cmdUniqueID = dataTable.Rows[0].Field<Guid>("cmdUniqueID");
			eRPOrganizationIndustryTypeLinkInformationDto.cmdIndustryTypeID = dataTable.Rows[0].Field<string>("cmdIndustryTypeID");
			eRPOrganizationIndustryTypeLinkInformationDto.cmdIndustryTypeLinkID = dataTable.Rows[0].Field<short>("cmdIndustryTypeLinkID");
			eRPOrganizationIndustryTypeLinkInformationDto.cmdOrganizationID = dataTable.Rows[0].Field<string>("cmdOrganizationID");
			eRPOrganizationIndustryTypeLinkInformationDto.cmdRowVersion = dataTable.Rows[0].Field<byte[]>("cmdRowVersion");
			eRPOrganizationIndustryTypeLinkInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPOrganizationIndustryTypeLinkInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPOrganizationIndustryTypeLinkInformationDto);
	}

	public Task<APIValidationInfoDto> SaveOrganizationIndustryTypeLink(ERPOrganizationIndustryTypeLinkDto organizationIndustryTypeLink)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM OrganizationIndustryTypeLinks WHERE cmdUniqueID = " + M1Util.ConvertToLinq(organizationIndustryTypeLink.cmdUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["cmdOrganizationID"] = organizationIndustryTypeLink.cmdOrganizationID.ToUpper();
				dataRow["cmdIndustryTypeLinkID"] = organizationIndustryTypeLink.cmdIndustryTypeLinkID;
				organizationIndustryTypeLink.cmdUniqueID = ((organizationIndustryTypeLink.cmdUniqueID == Guid.Empty) ? Guid.NewGuid() : organizationIndustryTypeLink.cmdUniqueID);
				dataRow["cmdUniqueID"] = organizationIndustryTypeLink.cmdUniqueID;
				dataRow["cmdCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["cmdCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The OrganizationIndustryTypeLink could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (organizationIndustryTypeLink.cmdRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the OrganizationIndustryTypeLink is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["cmdRowVersion"], organizationIndustryTypeLink.cmdRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the OrganizationIndustryTypeLink has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the OrganizationIndustryTypeLink again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["cmdIndustryTypeID"] = organizationIndustryTypeLink.cmdIndustryTypeID;
			if (organizationIndustryTypeLink.CustomFields != null && organizationIndustryTypeLink.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in organizationIndustryTypeLink.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the OrganizationIndustryTypeLink [{organizationIndustryTypeLink.cmdUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the OrganizationIndustryTypeLink [{organizationIndustryTypeLink.cmdUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
