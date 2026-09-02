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

public class ERPChangeRequestTypeRepository : APIBaseRepository, IERPChangeRequestTypeRepository, IAPIBaseRepository, IDisposable
{
	public ERPChangeRequestTypeRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesChangeRequestTypeExist(Guid changeRequestTypeId)
	{
		InitializeParameterLists();
		base.filterList.Add("chtUniqueID|C", changeRequestTypeId);
		base.selectList.Add("chtUniqueID");
		return Task.FromResult(GetAsObject("ChangeRequestTypes", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPChangeRequestTypeInformationDto>> GetAllChangeRequestTypes(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPChangeRequestTypeInformationDto> collection = new List<ERPChangeRequestTypeInformationDto>();
		InitializeParameterLists();
		string[] array = new string[8] { "chtChangeRequestTypeID", "chtCreatedBy", "chtCreatedDate", "chtDescription", "chtUniqueID", "chtInactiveDate", "chtInactive", "chtRowVersion" };
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("ChangeRequestTypes");
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
		using (DataTable dataTable = GetAsDataTable("ChangeRequestTypes", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPChangeRequestTypeInformationDto eRPChangeRequestTypeInformationDto = new ERPChangeRequestTypeInformationDto();
				eRPChangeRequestTypeInformationDto.chtChangeRequestTypeID = dataTable.Rows[i].Field<string>("chtChangeRequestTypeID");
				eRPChangeRequestTypeInformationDto.chtCreatedBy = dataTable.Rows[i].Field<string>("chtCreatedBy");
				eRPChangeRequestTypeInformationDto.chtCreatedDate = dataTable.Rows[i].Field<DateTime?>("chtCreatedDate");
				eRPChangeRequestTypeInformationDto.chtDescription = dataTable.Rows[i].Field<string>("chtDescription");
				eRPChangeRequestTypeInformationDto.chtUniqueID = dataTable.Rows[i].Field<Guid>("chtUniqueID");
				eRPChangeRequestTypeInformationDto.chtInactiveDate = dataTable.Rows[i].Field<DateTime?>("chtInactiveDate");
				eRPChangeRequestTypeInformationDto.chtInactive = dataTable.Rows[i].Field<bool>("chtInactive");
				eRPChangeRequestTypeInformationDto.chtRowVersion = dataTable.Rows[i].Field<byte[]>("chtRowVersion");
				eRPChangeRequestTypeInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPChangeRequestTypeInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPChangeRequestTypeInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPChangeRequestTypeInformationDto> GetChangeRequestType(Guid changeRequestTypeId)
	{
		ERPChangeRequestTypeInformationDto eRPChangeRequestTypeInformationDto = new ERPChangeRequestTypeInformationDto();
		InitializeParameterLists();
		string[] collection = new string[8] { "chtChangeRequestTypeID", "chtCreatedBy", "chtCreatedDate", "chtDescription", "chtUniqueID", "chtInactiveDate", "chtInactive", "chtRowVersion" };
		base.selectList.AddRange(collection);
		base.filterList.Add("chtUniqueID|C", changeRequestTypeId);
		AddCustomFieldsToSelectList("ChangeRequestTypes");
		using (DataTable dataTable = GetAsDataTable("ChangeRequestTypes", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPChangeRequestTypeInformationDto);
			}
			eRPChangeRequestTypeInformationDto.chtChangeRequestTypeID = dataTable.Rows[0].Field<string>("chtChangeRequestTypeID");
			eRPChangeRequestTypeInformationDto.chtCreatedBy = dataTable.Rows[0].Field<string>("chtCreatedBy");
			eRPChangeRequestTypeInformationDto.chtCreatedDate = dataTable.Rows[0].Field<DateTime?>("chtCreatedDate");
			eRPChangeRequestTypeInformationDto.chtDescription = dataTable.Rows[0].Field<string>("chtDescription");
			eRPChangeRequestTypeInformationDto.chtUniqueID = dataTable.Rows[0].Field<Guid>("chtUniqueID");
			eRPChangeRequestTypeInformationDto.chtInactiveDate = dataTable.Rows[0].Field<DateTime?>("chtInactiveDate");
			eRPChangeRequestTypeInformationDto.chtInactive = dataTable.Rows[0].Field<bool>("chtInactive");
			eRPChangeRequestTypeInformationDto.chtRowVersion = dataTable.Rows[0].Field<byte[]>("chtRowVersion");
			eRPChangeRequestTypeInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPChangeRequestTypeInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPChangeRequestTypeInformationDto);
	}

	public Task<APIValidationInfoDto> SaveChangeRequestType(ERPChangeRequestTypeDto changeRequestType)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM ChangeRequestTypes WHERE chtUniqueID = " + M1Util.ConvertToLinq(changeRequestType.chtUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["chtChangeRequestTypeID"] = changeRequestType.chtChangeRequestTypeID.ToUpper();
				changeRequestType.chtUniqueID = ((changeRequestType.chtUniqueID == Guid.Empty) ? Guid.NewGuid() : changeRequestType.chtUniqueID);
				dataRow["chtUniqueID"] = changeRequestType.chtUniqueID;
				dataRow["chtCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["chtCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The ChangeRequestType could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (changeRequestType.chtRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the ChangeRequestType is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["chtRowVersion"], changeRequestType.chtRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the ChangeRequestType has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the ChangeRequestType again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["chtDescription"] = changeRequestType.chtDescription;
			DataRow dataRow2 = dataRow;
			DateTime? chtInactiveDate = changeRequestType.chtInactiveDate;
			dataRow2["chtInactiveDate"] = (chtInactiveDate.HasValue ? ((object)chtInactiveDate.GetValueOrDefault()) : dataRow["chtInactiveDate"]);
			dataRow["chtInactive"] = changeRequestType.chtInactive;
			if (changeRequestType.CustomFields != null && changeRequestType.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in changeRequestType.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the ChangeRequestType [{changeRequestType.chtUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the ChangeRequestType [{changeRequestType.chtUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
