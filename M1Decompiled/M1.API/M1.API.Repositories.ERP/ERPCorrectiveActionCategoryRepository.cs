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

public class ERPCorrectiveActionCategoryRepository : APIBaseRepository, IERPCorrectiveActionCategoryRepository, IAPIBaseRepository, IDisposable
{
	public ERPCorrectiveActionCategoryRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesCorrectiveActionCategoryExist(Guid correctiveActionCategoryId)
	{
		InitializeParameterLists();
		base.filterList.Add("qatUniqueID|C", correctiveActionCategoryId);
		base.selectList.Add("qatUniqueID");
		return Task.FromResult(GetAsObject("CorrectiveActionCategories", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPCorrectiveActionCategoryInformationDto>> GetAllCorrectiveActionCategories(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPCorrectiveActionCategoryInformationDto> collection = new List<ERPCorrectiveActionCategoryInformationDto>();
		InitializeParameterLists();
		string[] array = new string[6] { "qatCorrectiveActionCategoryID", "qatCreatedBy", "qatCreatedDate", "qatDescription", "qatUniqueID", "qatRowVersion" };
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("CorrectiveActionCategories");
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
		using (DataTable dataTable = GetAsDataTable("CorrectiveActionCategories", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPCorrectiveActionCategoryInformationDto eRPCorrectiveActionCategoryInformationDto = new ERPCorrectiveActionCategoryInformationDto();
				eRPCorrectiveActionCategoryInformationDto.qatCorrectiveActionCategoryID = dataTable.Rows[i].Field<string>("qatCorrectiveActionCategoryID");
				eRPCorrectiveActionCategoryInformationDto.qatCreatedBy = dataTable.Rows[i].Field<string>("qatCreatedBy");
				eRPCorrectiveActionCategoryInformationDto.qatCreatedDate = dataTable.Rows[i].Field<DateTime?>("qatCreatedDate");
				eRPCorrectiveActionCategoryInformationDto.qatDescription = dataTable.Rows[i].Field<string>("qatDescription");
				eRPCorrectiveActionCategoryInformationDto.qatUniqueID = dataTable.Rows[i].Field<Guid>("qatUniqueID");
				eRPCorrectiveActionCategoryInformationDto.qatRowVersion = dataTable.Rows[i].Field<byte[]>("qatRowVersion");
				eRPCorrectiveActionCategoryInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPCorrectiveActionCategoryInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPCorrectiveActionCategoryInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPCorrectiveActionCategoryInformationDto> GetCorrectiveActionCategory(Guid correctiveActionCategoryId)
	{
		ERPCorrectiveActionCategoryInformationDto eRPCorrectiveActionCategoryInformationDto = new ERPCorrectiveActionCategoryInformationDto();
		InitializeParameterLists();
		string[] collection = new string[6] { "qatCorrectiveActionCategoryID", "qatCreatedBy", "qatCreatedDate", "qatDescription", "qatUniqueID", "qatRowVersion" };
		base.selectList.AddRange(collection);
		base.filterList.Add("qatUniqueID|C", correctiveActionCategoryId);
		AddCustomFieldsToSelectList("CorrectiveActionCategories");
		using (DataTable dataTable = GetAsDataTable("CorrectiveActionCategories", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPCorrectiveActionCategoryInformationDto);
			}
			eRPCorrectiveActionCategoryInformationDto.qatCorrectiveActionCategoryID = dataTable.Rows[0].Field<string>("qatCorrectiveActionCategoryID");
			eRPCorrectiveActionCategoryInformationDto.qatCreatedBy = dataTable.Rows[0].Field<string>("qatCreatedBy");
			eRPCorrectiveActionCategoryInformationDto.qatCreatedDate = dataTable.Rows[0].Field<DateTime?>("qatCreatedDate");
			eRPCorrectiveActionCategoryInformationDto.qatDescription = dataTable.Rows[0].Field<string>("qatDescription");
			eRPCorrectiveActionCategoryInformationDto.qatUniqueID = dataTable.Rows[0].Field<Guid>("qatUniqueID");
			eRPCorrectiveActionCategoryInformationDto.qatRowVersion = dataTable.Rows[0].Field<byte[]>("qatRowVersion");
			eRPCorrectiveActionCategoryInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPCorrectiveActionCategoryInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPCorrectiveActionCategoryInformationDto);
	}

	public Task<APIValidationInfoDto> SaveCorrectiveActionCategory(ERPCorrectiveActionCategoryDto correctiveActionCategory)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM CorrectiveActionCategories WHERE qatUniqueID = " + M1Util.ConvertToLinq(correctiveActionCategory.qatUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["qatCorrectiveActionCategoryID"] = correctiveActionCategory.qatCorrectiveActionCategoryID.ToUpper();
				correctiveActionCategory.qatUniqueID = ((correctiveActionCategory.qatUniqueID == Guid.Empty) ? Guid.NewGuid() : correctiveActionCategory.qatUniqueID);
				dataRow["qatUniqueID"] = correctiveActionCategory.qatUniqueID;
				dataRow["qatCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["qatCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The CorrectiveActionCategory could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (correctiveActionCategory.qatRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the CorrectiveActionCategory is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["qatRowVersion"], correctiveActionCategory.qatRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the CorrectiveActionCategory has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the CorrectiveActionCategory again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["qatDescription"] = correctiveActionCategory.qatDescription;
			if (correctiveActionCategory.CustomFields != null && correctiveActionCategory.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in correctiveActionCategory.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the CorrectiveActionCategory [{correctiveActionCategory.qatUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the CorrectiveActionCategory [{correctiveActionCategory.qatUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
