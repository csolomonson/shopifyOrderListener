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

public class ERPContactTitleRepository : APIBaseRepository, IERPContactTitleRepository, IAPIBaseRepository, IDisposable
{
	public ERPContactTitleRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesContactTitleExist(Guid contactTitleId)
	{
		InitializeParameterLists();
		base.filterList.Add("cmeUniqueID|C", contactTitleId);
		base.selectList.Add("cmeUniqueID");
		return Task.FromResult(GetAsObject("ContactTitles", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPContactTitleInformationDto>> GetAllContactTitles(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPContactTitleInformationDto> collection = new List<ERPContactTitleInformationDto>();
		InitializeParameterLists();
		string[] array = new string[6] { "cmeContactTitleID", "cmeCreatedBy", "cmeCreatedDate", "cmeDescription", "cmeUniqueID", "cmeRowVersion" };
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("ContactTitles");
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
		using (DataTable dataTable = GetAsDataTable("ContactTitles", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPContactTitleInformationDto eRPContactTitleInformationDto = new ERPContactTitleInformationDto();
				eRPContactTitleInformationDto.cmeContactTitleID = dataTable.Rows[i].Field<string>("cmeContactTitleID");
				eRPContactTitleInformationDto.cmeCreatedBy = dataTable.Rows[i].Field<string>("cmeCreatedBy");
				eRPContactTitleInformationDto.cmeCreatedDate = dataTable.Rows[i].Field<DateTime?>("cmeCreatedDate");
				eRPContactTitleInformationDto.cmeDescription = dataTable.Rows[i].Field<string>("cmeDescription");
				eRPContactTitleInformationDto.cmeUniqueID = dataTable.Rows[i].Field<Guid>("cmeUniqueID");
				eRPContactTitleInformationDto.cmeRowVersion = dataTable.Rows[i].Field<byte[]>("cmeRowVersion");
				eRPContactTitleInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPContactTitleInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPContactTitleInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPContactTitleInformationDto> GetContactTitle(Guid contactTitleId)
	{
		ERPContactTitleInformationDto eRPContactTitleInformationDto = new ERPContactTitleInformationDto();
		InitializeParameterLists();
		string[] collection = new string[6] { "cmeContactTitleID", "cmeCreatedBy", "cmeCreatedDate", "cmeDescription", "cmeUniqueID", "cmeRowVersion" };
		base.selectList.AddRange(collection);
		base.filterList.Add("cmeUniqueID|C", contactTitleId);
		AddCustomFieldsToSelectList("ContactTitles");
		using (DataTable dataTable = GetAsDataTable("ContactTitles", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPContactTitleInformationDto);
			}
			eRPContactTitleInformationDto.cmeContactTitleID = dataTable.Rows[0].Field<string>("cmeContactTitleID");
			eRPContactTitleInformationDto.cmeCreatedBy = dataTable.Rows[0].Field<string>("cmeCreatedBy");
			eRPContactTitleInformationDto.cmeCreatedDate = dataTable.Rows[0].Field<DateTime?>("cmeCreatedDate");
			eRPContactTitleInformationDto.cmeDescription = dataTable.Rows[0].Field<string>("cmeDescription");
			eRPContactTitleInformationDto.cmeUniqueID = dataTable.Rows[0].Field<Guid>("cmeUniqueID");
			eRPContactTitleInformationDto.cmeRowVersion = dataTable.Rows[0].Field<byte[]>("cmeRowVersion");
			eRPContactTitleInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPContactTitleInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPContactTitleInformationDto);
	}

	public Task<APIValidationInfoDto> SaveContactTitle(ERPContactTitleDto contactTitle)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM ContactTitles WHERE cmeUniqueID = " + M1Util.ConvertToLinq(contactTitle.cmeUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["cmeContactTitleID"] = contactTitle.cmeContactTitleID.ToUpper();
				contactTitle.cmeUniqueID = ((contactTitle.cmeUniqueID == Guid.Empty) ? Guid.NewGuid() : contactTitle.cmeUniqueID);
				dataRow["cmeUniqueID"] = contactTitle.cmeUniqueID;
				dataRow["cmeCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["cmeCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The ContactTitle could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (contactTitle.cmeRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the ContactTitle is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["cmeRowVersion"], contactTitle.cmeRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the ContactTitle has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the ContactTitle again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["cmeDescription"] = contactTitle.cmeDescription;
			if (contactTitle.CustomFields != null && contactTitle.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in contactTitle.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the ContactTitle [{contactTitle.cmeUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the ContactTitle [{contactTitle.cmeUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
