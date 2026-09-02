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

public class ERPContactGroupRepository : APIBaseRepository, IERPContactGroupRepository, IAPIBaseRepository, IDisposable
{
	public ERPContactGroupRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesContactGroupExist(Guid contactGroupId)
	{
		InitializeParameterLists();
		base.filterList.Add("cmgUniqueID|C", contactGroupId);
		base.selectList.Add("cmgUniqueID");
		return Task.FromResult(GetAsObject("ContactGroups", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPContactGroupInformationDto>> GetAllContactGroups(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPContactGroupInformationDto> collection = new List<ERPContactGroupInformationDto>();
		InitializeParameterLists();
		string[] array = new string[6] { "cmgContactGroupID", "cmgCreatedBy", "cmgCreatedDate", "cmgDescription", "cmgUniqueID", "cmgRowVersion" };
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("ContactGroups");
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
		using (DataTable dataTable = GetAsDataTable("ContactGroups", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPContactGroupInformationDto eRPContactGroupInformationDto = new ERPContactGroupInformationDto();
				eRPContactGroupInformationDto.cmgContactGroupID = dataTable.Rows[i].Field<string>("cmgContactGroupID");
				eRPContactGroupInformationDto.cmgCreatedBy = dataTable.Rows[i].Field<string>("cmgCreatedBy");
				eRPContactGroupInformationDto.cmgCreatedDate = dataTable.Rows[i].Field<DateTime?>("cmgCreatedDate");
				eRPContactGroupInformationDto.cmgDescription = dataTable.Rows[i].Field<string>("cmgDescription");
				eRPContactGroupInformationDto.cmgUniqueID = dataTable.Rows[i].Field<Guid>("cmgUniqueID");
				eRPContactGroupInformationDto.cmgRowVersion = dataTable.Rows[i].Field<byte[]>("cmgRowVersion");
				eRPContactGroupInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPContactGroupInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPContactGroupInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPContactGroupInformationDto> GetContactGroup(Guid contactGroupId)
	{
		ERPContactGroupInformationDto eRPContactGroupInformationDto = new ERPContactGroupInformationDto();
		InitializeParameterLists();
		string[] collection = new string[6] { "cmgContactGroupID", "cmgCreatedBy", "cmgCreatedDate", "cmgDescription", "cmgUniqueID", "cmgRowVersion" };
		base.selectList.AddRange(collection);
		base.filterList.Add("cmgUniqueID|C", contactGroupId);
		AddCustomFieldsToSelectList("ContactGroups");
		using (DataTable dataTable = GetAsDataTable("ContactGroups", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPContactGroupInformationDto);
			}
			eRPContactGroupInformationDto.cmgContactGroupID = dataTable.Rows[0].Field<string>("cmgContactGroupID");
			eRPContactGroupInformationDto.cmgCreatedBy = dataTable.Rows[0].Field<string>("cmgCreatedBy");
			eRPContactGroupInformationDto.cmgCreatedDate = dataTable.Rows[0].Field<DateTime?>("cmgCreatedDate");
			eRPContactGroupInformationDto.cmgDescription = dataTable.Rows[0].Field<string>("cmgDescription");
			eRPContactGroupInformationDto.cmgUniqueID = dataTable.Rows[0].Field<Guid>("cmgUniqueID");
			eRPContactGroupInformationDto.cmgRowVersion = dataTable.Rows[0].Field<byte[]>("cmgRowVersion");
			eRPContactGroupInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPContactGroupInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPContactGroupInformationDto);
	}

	public Task<APIValidationInfoDto> SaveContactGroup(ERPContactGroupDto contactGroup)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM ContactGroups WHERE cmgUniqueID = " + M1Util.ConvertToLinq(contactGroup.cmgUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["cmgContactGroupID"] = contactGroup.cmgContactGroupID.ToUpper();
				contactGroup.cmgUniqueID = ((contactGroup.cmgUniqueID == Guid.Empty) ? Guid.NewGuid() : contactGroup.cmgUniqueID);
				dataRow["cmgUniqueID"] = contactGroup.cmgUniqueID;
				dataRow["cmgCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["cmgCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The ContactGroup could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (contactGroup.cmgRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the ContactGroup is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["cmgRowVersion"], contactGroup.cmgRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the ContactGroup has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the ContactGroup again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["cmgDescription"] = contactGroup.cmgDescription;
			if (contactGroup.CustomFields != null && contactGroup.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in contactGroup.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the ContactGroup [{contactGroup.cmgUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the ContactGroup [{contactGroup.cmgUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
