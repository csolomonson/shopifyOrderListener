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

public class ERPIndustryTypeRepository : APIBaseRepository, IERPIndustryTypeRepository, IAPIBaseRepository, IDisposable
{
	public ERPIndustryTypeRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesIndustryTypeExist(Guid industryTypeId)
	{
		InitializeParameterLists();
		base.filterList.Add("cmiUniqueID|C", industryTypeId);
		base.selectList.Add("cmiUniqueID");
		return Task.FromResult(GetAsObject("IndustryTypes", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPIndustryTypeInformationDto>> GetAllIndustryTypes(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPIndustryTypeInformationDto> collection = new List<ERPIndustryTypeInformationDto>();
		InitializeParameterLists();
		string[] array = new string[8] { "cmiIndustryTypeID", "cmiCreatedBy", "cmiCreatedDate", "cmiUniqueID", "cmiLongDescriptionRtf", "cmiLongDescriptionText", "cmiRowVersion", "cmiShortDescription" };
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("IndustryTypes");
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
		using (DataTable dataTable = GetAsDataTable("IndustryTypes", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPIndustryTypeInformationDto eRPIndustryTypeInformationDto = new ERPIndustryTypeInformationDto();
				eRPIndustryTypeInformationDto.cmiIndustryTypeID = dataTable.Rows[i].Field<string>("cmiIndustryTypeID");
				eRPIndustryTypeInformationDto.cmiCreatedBy = dataTable.Rows[i].Field<string>("cmiCreatedBy");
				eRPIndustryTypeInformationDto.cmiCreatedDate = dataTable.Rows[i].Field<DateTime?>("cmiCreatedDate");
				eRPIndustryTypeInformationDto.cmiUniqueID = dataTable.Rows[i].Field<Guid>("cmiUniqueID");
				eRPIndustryTypeInformationDto.cmiLongDescriptionRtf = dataTable.Rows[i].Field<string>("cmiLongDescriptionRtf");
				eRPIndustryTypeInformationDto.cmiLongDescriptionText = dataTable.Rows[i].Field<string>("cmiLongDescriptionText");
				eRPIndustryTypeInformationDto.cmiRowVersion = dataTable.Rows[i].Field<byte[]>("cmiRowVersion");
				eRPIndustryTypeInformationDto.cmiShortDescription = dataTable.Rows[i].Field<string>("cmiShortDescription");
				eRPIndustryTypeInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPIndustryTypeInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPIndustryTypeInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPIndustryTypeInformationDto> GetIndustryType(Guid industryTypeId)
	{
		ERPIndustryTypeInformationDto eRPIndustryTypeInformationDto = new ERPIndustryTypeInformationDto();
		InitializeParameterLists();
		string[] collection = new string[8] { "cmiIndustryTypeID", "cmiCreatedBy", "cmiCreatedDate", "cmiUniqueID", "cmiLongDescriptionRtf", "cmiLongDescriptionText", "cmiRowVersion", "cmiShortDescription" };
		base.selectList.AddRange(collection);
		base.filterList.Add("cmiUniqueID|C", industryTypeId);
		AddCustomFieldsToSelectList("IndustryTypes");
		using (DataTable dataTable = GetAsDataTable("IndustryTypes", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPIndustryTypeInformationDto);
			}
			eRPIndustryTypeInformationDto.cmiIndustryTypeID = dataTable.Rows[0].Field<string>("cmiIndustryTypeID");
			eRPIndustryTypeInformationDto.cmiCreatedBy = dataTable.Rows[0].Field<string>("cmiCreatedBy");
			eRPIndustryTypeInformationDto.cmiCreatedDate = dataTable.Rows[0].Field<DateTime?>("cmiCreatedDate");
			eRPIndustryTypeInformationDto.cmiUniqueID = dataTable.Rows[0].Field<Guid>("cmiUniqueID");
			eRPIndustryTypeInformationDto.cmiLongDescriptionRtf = dataTable.Rows[0].Field<string>("cmiLongDescriptionRtf");
			eRPIndustryTypeInformationDto.cmiLongDescriptionText = dataTable.Rows[0].Field<string>("cmiLongDescriptionText");
			eRPIndustryTypeInformationDto.cmiRowVersion = dataTable.Rows[0].Field<byte[]>("cmiRowVersion");
			eRPIndustryTypeInformationDto.cmiShortDescription = dataTable.Rows[0].Field<string>("cmiShortDescription");
			eRPIndustryTypeInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPIndustryTypeInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPIndustryTypeInformationDto);
	}

	public Task<APIValidationInfoDto> SaveIndustryType(ERPIndustryTypeDto industryType)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM IndustryTypes WHERE cmiUniqueID = " + M1Util.ConvertToLinq(industryType.cmiUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["cmiIndustryTypeID"] = industryType.cmiIndustryTypeID.ToUpper();
				industryType.cmiUniqueID = ((industryType.cmiUniqueID == Guid.Empty) ? Guid.NewGuid() : industryType.cmiUniqueID);
				dataRow["cmiUniqueID"] = industryType.cmiUniqueID;
				dataRow["cmiCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["cmiCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The IndustryType could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (industryType.cmiRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the IndustryType is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["cmiRowVersion"], industryType.cmiRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the IndustryType has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the IndustryType again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["cmiLongDescriptionRtf"] = industryType.cmiLongDescriptionRtf ?? dataRow["cmiLongDescriptionRtf"];
			dataRow["cmiLongDescriptionText"] = industryType.cmiLongDescriptionText ?? dataRow["cmiLongDescriptionText"];
			dataRow["cmiShortDescription"] = industryType.cmiShortDescription;
			if (industryType.CustomFields != null && industryType.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in industryType.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the IndustryType [{industryType.cmiUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the IndustryType [{industryType.cmiUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
