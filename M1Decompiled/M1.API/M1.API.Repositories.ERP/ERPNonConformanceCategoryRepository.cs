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

public class ERPNonConformanceCategoryRepository : APIBaseRepository, IERPNonConformanceCategoryRepository, IAPIBaseRepository, IDisposable
{
	public ERPNonConformanceCategoryRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesNonConformanceCategoryExist(Guid nonConformanceCategoryId)
	{
		InitializeParameterLists();
		base.filterList.Add("qagUniqueID|C", nonConformanceCategoryId);
		base.selectList.Add("qagUniqueID");
		return Task.FromResult(GetAsObject("NonConformanceCategories", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPNonConformanceCategoryInformationDto>> GetAllNonConformanceCategories(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPNonConformanceCategoryInformationDto> collection = new List<ERPNonConformanceCategoryInformationDto>();
		InitializeParameterLists();
		string[] array = new string[6] { "qagNonConformanceCategoryID", "qagCreatedBy", "qagCreatedDate", "qagDescription", "qagUniqueID", "qagRowVersion" };
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("NonConformanceCategories");
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
		using (DataTable dataTable = GetAsDataTable("NonConformanceCategories", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPNonConformanceCategoryInformationDto eRPNonConformanceCategoryInformationDto = new ERPNonConformanceCategoryInformationDto();
				eRPNonConformanceCategoryInformationDto.qagNonConformanceCategoryID = dataTable.Rows[i].Field<string>("qagNonConformanceCategoryID");
				eRPNonConformanceCategoryInformationDto.qagCreatedBy = dataTable.Rows[i].Field<string>("qagCreatedBy");
				eRPNonConformanceCategoryInformationDto.qagCreatedDate = dataTable.Rows[i].Field<DateTime?>("qagCreatedDate");
				eRPNonConformanceCategoryInformationDto.qagDescription = dataTable.Rows[i].Field<string>("qagDescription");
				eRPNonConformanceCategoryInformationDto.qagUniqueID = dataTable.Rows[i].Field<Guid>("qagUniqueID");
				eRPNonConformanceCategoryInformationDto.qagRowVersion = dataTable.Rows[i].Field<byte[]>("qagRowVersion");
				eRPNonConformanceCategoryInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPNonConformanceCategoryInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPNonConformanceCategoryInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPNonConformanceCategoryInformationDto> GetNonConformanceCategory(Guid nonConformanceCategoryId)
	{
		ERPNonConformanceCategoryInformationDto eRPNonConformanceCategoryInformationDto = new ERPNonConformanceCategoryInformationDto();
		InitializeParameterLists();
		string[] collection = new string[6] { "qagNonConformanceCategoryID", "qagCreatedBy", "qagCreatedDate", "qagDescription", "qagUniqueID", "qagRowVersion" };
		base.selectList.AddRange(collection);
		base.filterList.Add("qagUniqueID|C", nonConformanceCategoryId);
		AddCustomFieldsToSelectList("NonConformanceCategories");
		using (DataTable dataTable = GetAsDataTable("NonConformanceCategories", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPNonConformanceCategoryInformationDto);
			}
			eRPNonConformanceCategoryInformationDto.qagNonConformanceCategoryID = dataTable.Rows[0].Field<string>("qagNonConformanceCategoryID");
			eRPNonConformanceCategoryInformationDto.qagCreatedBy = dataTable.Rows[0].Field<string>("qagCreatedBy");
			eRPNonConformanceCategoryInformationDto.qagCreatedDate = dataTable.Rows[0].Field<DateTime?>("qagCreatedDate");
			eRPNonConformanceCategoryInformationDto.qagDescription = dataTable.Rows[0].Field<string>("qagDescription");
			eRPNonConformanceCategoryInformationDto.qagUniqueID = dataTable.Rows[0].Field<Guid>("qagUniqueID");
			eRPNonConformanceCategoryInformationDto.qagRowVersion = dataTable.Rows[0].Field<byte[]>("qagRowVersion");
			eRPNonConformanceCategoryInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPNonConformanceCategoryInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPNonConformanceCategoryInformationDto);
	}

	public Task<APIValidationInfoDto> SaveNonConformanceCategory(ERPNonConformanceCategoryDto nonConformanceCategory)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM NonConformanceCategories WHERE qagUniqueID = " + M1Util.ConvertToLinq(nonConformanceCategory.qagUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["qagNonConformanceCategoryID"] = nonConformanceCategory.qagNonConformanceCategoryID.ToUpper();
				nonConformanceCategory.qagUniqueID = ((nonConformanceCategory.qagUniqueID == Guid.Empty) ? Guid.NewGuid() : nonConformanceCategory.qagUniqueID);
				dataRow["qagUniqueID"] = nonConformanceCategory.qagUniqueID;
				dataRow["qagCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["qagCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The NonConformanceCategory could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (nonConformanceCategory.qagRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the NonConformanceCategory is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["qagRowVersion"], nonConformanceCategory.qagRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the NonConformanceCategory has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the NonConformanceCategory again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["qagDescription"] = nonConformanceCategory.qagDescription;
			if (nonConformanceCategory.CustomFields != null && nonConformanceCategory.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in nonConformanceCategory.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the NonConformanceCategory [{nonConformanceCategory.qagUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the NonConformanceCategory [{nonConformanceCategory.qagUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
