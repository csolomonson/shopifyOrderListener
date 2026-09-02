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

public class ERPGLCategoryRepository : APIBaseRepository, IERPGLCategoryRepository, IAPIBaseRepository, IDisposable
{
	public ERPGLCategoryRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesGLCategoryExist(Guid gLCategoryId)
	{
		InitializeParameterLists();
		base.filterList.Add("gltUniqueID|C", gLCategoryId);
		base.selectList.Add("gltUniqueID");
		return Task.FromResult(GetAsObject("GLCategories", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPGLCategoryInformationDto>> GetAllGLCategories(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPGLCategoryInformationDto> collection = new List<ERPGLCategoryInformationDto>();
		InitializeParameterLists();
		string[] array = new string[8] { "gltCategoryType", "gltGlCategoryID", "gltCreatedBy", "gltCreatedDate", "gltDescription", "gltUniqueID", "gltReportSequence", "gltRowVersion" };
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("GLCategories");
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
		using (DataTable dataTable = GetAsDataTable("GLCategories", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPGLCategoryInformationDto eRPGLCategoryInformationDto = new ERPGLCategoryInformationDto();
				eRPGLCategoryInformationDto.gltCategoryType = dataTable.Rows[i].Field<byte>("gltCategoryType");
				eRPGLCategoryInformationDto.gltGlCategoryID = dataTable.Rows[i].Field<string>("gltGlCategoryID");
				eRPGLCategoryInformationDto.gltCreatedBy = dataTable.Rows[i].Field<string>("gltCreatedBy");
				eRPGLCategoryInformationDto.gltCreatedDate = dataTable.Rows[i].Field<DateTime?>("gltCreatedDate");
				eRPGLCategoryInformationDto.gltDescription = dataTable.Rows[i].Field<string>("gltDescription");
				eRPGLCategoryInformationDto.gltUniqueID = dataTable.Rows[i].Field<Guid>("gltUniqueID");
				eRPGLCategoryInformationDto.gltReportSequence = dataTable.Rows[i].Field<byte>("gltReportSequence");
				eRPGLCategoryInformationDto.gltRowVersion = dataTable.Rows[i].Field<byte[]>("gltRowVersion");
				eRPGLCategoryInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPGLCategoryInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPGLCategoryInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPGLCategoryInformationDto> GetGLCategory(Guid gLCategoryId)
	{
		ERPGLCategoryInformationDto eRPGLCategoryInformationDto = new ERPGLCategoryInformationDto();
		InitializeParameterLists();
		string[] collection = new string[8] { "gltCategoryType", "gltGlCategoryID", "gltCreatedBy", "gltCreatedDate", "gltDescription", "gltUniqueID", "gltReportSequence", "gltRowVersion" };
		base.selectList.AddRange(collection);
		base.filterList.Add("gltUniqueID|C", gLCategoryId);
		AddCustomFieldsToSelectList("GLCategories");
		using (DataTable dataTable = GetAsDataTable("GLCategories", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPGLCategoryInformationDto);
			}
			eRPGLCategoryInformationDto.gltCategoryType = dataTable.Rows[0].Field<byte>("gltCategoryType");
			eRPGLCategoryInformationDto.gltGlCategoryID = dataTable.Rows[0].Field<string>("gltGlCategoryID");
			eRPGLCategoryInformationDto.gltCreatedBy = dataTable.Rows[0].Field<string>("gltCreatedBy");
			eRPGLCategoryInformationDto.gltCreatedDate = dataTable.Rows[0].Field<DateTime?>("gltCreatedDate");
			eRPGLCategoryInformationDto.gltDescription = dataTable.Rows[0].Field<string>("gltDescription");
			eRPGLCategoryInformationDto.gltUniqueID = dataTable.Rows[0].Field<Guid>("gltUniqueID");
			eRPGLCategoryInformationDto.gltReportSequence = dataTable.Rows[0].Field<byte>("gltReportSequence");
			eRPGLCategoryInformationDto.gltRowVersion = dataTable.Rows[0].Field<byte[]>("gltRowVersion");
			eRPGLCategoryInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPGLCategoryInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPGLCategoryInformationDto);
	}

	public Task<APIValidationInfoDto> SaveGLCategory(ERPGLCategoryDto gLCategory)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM GLCategories WHERE gltUniqueID = " + M1Util.ConvertToLinq(gLCategory.gltUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["gltGlCategoryID"] = gLCategory.gltGlCategoryID.ToUpper();
				gLCategory.gltUniqueID = ((gLCategory.gltUniqueID == Guid.Empty) ? Guid.NewGuid() : gLCategory.gltUniqueID);
				dataRow["gltUniqueID"] = gLCategory.gltUniqueID;
				dataRow["gltCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["gltCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The GLCategory could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (gLCategory.gltRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the GLCategory is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["gltRowVersion"], gLCategory.gltRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the GLCategory has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the GLCategory again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["gltCategoryType"] = gLCategory.gltCategoryType;
			dataRow["gltDescription"] = gLCategory.gltDescription;
			dataRow["gltReportSequence"] = gLCategory.gltReportSequence;
			if (gLCategory.CustomFields != null && gLCategory.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in gLCategory.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the GLCategory [{gLCategory.gltUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the GLCategory [{gLCategory.gltUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
