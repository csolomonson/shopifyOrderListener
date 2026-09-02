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

public class ERPGLFiscalYearRepository : APIBaseRepository, IERPGLFiscalYearRepository, IAPIBaseRepository, IDisposable
{
	public ERPGLFiscalYearRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesGLFiscalYearExist(Guid gLFiscalYearId)
	{
		InitializeParameterLists();
		base.filterList.Add("glzUniqueID|C", gLFiscalYearId);
		base.selectList.Add("glzUniqueID");
		return Task.FromResult(GetAsObject("GLFiscalYears", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPGLFiscalYearInformationDto>> GetAllGLFiscalYears(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPGLFiscalYearInformationDto> collection = new List<ERPGLFiscalYearInformationDto>();
		InitializeParameterLists();
		string[] array = new string[7] { "glzCreatedBy", "glzCreatedDate", "glzEndDate", "glzUniqueID", "glzRowVersion", "glzGlFiscalYearID", "glzStartDate" };
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("GLFiscalYears");
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
		using (DataTable dataTable = GetAsDataTable("GLFiscalYears", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPGLFiscalYearInformationDto eRPGLFiscalYearInformationDto = new ERPGLFiscalYearInformationDto();
				eRPGLFiscalYearInformationDto.glzCreatedBy = dataTable.Rows[i].Field<string>("glzCreatedBy");
				eRPGLFiscalYearInformationDto.glzCreatedDate = dataTable.Rows[i].Field<DateTime?>("glzCreatedDate");
				eRPGLFiscalYearInformationDto.glzEndDate = dataTable.Rows[i].Field<DateTime?>("glzEndDate");
				eRPGLFiscalYearInformationDto.glzUniqueID = dataTable.Rows[i].Field<Guid>("glzUniqueID");
				eRPGLFiscalYearInformationDto.glzRowVersion = dataTable.Rows[i].Field<byte[]>("glzRowVersion");
				eRPGLFiscalYearInformationDto.glzGlFiscalYearID = dataTable.Rows[i].Field<short>("glzGlFiscalYearID");
				eRPGLFiscalYearInformationDto.glzStartDate = dataTable.Rows[i].Field<DateTime?>("glzStartDate");
				eRPGLFiscalYearInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPGLFiscalYearInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPGLFiscalYearInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPGLFiscalYearInformationDto> GetGLFiscalYear(Guid gLFiscalYearId)
	{
		ERPGLFiscalYearInformationDto eRPGLFiscalYearInformationDto = new ERPGLFiscalYearInformationDto();
		InitializeParameterLists();
		string[] collection = new string[7] { "glzCreatedBy", "glzCreatedDate", "glzEndDate", "glzUniqueID", "glzRowVersion", "glzGlFiscalYearID", "glzStartDate" };
		base.selectList.AddRange(collection);
		base.filterList.Add("glzUniqueID|C", gLFiscalYearId);
		AddCustomFieldsToSelectList("GLFiscalYears");
		using (DataTable dataTable = GetAsDataTable("GLFiscalYears", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPGLFiscalYearInformationDto);
			}
			eRPGLFiscalYearInformationDto.glzCreatedBy = dataTable.Rows[0].Field<string>("glzCreatedBy");
			eRPGLFiscalYearInformationDto.glzCreatedDate = dataTable.Rows[0].Field<DateTime?>("glzCreatedDate");
			eRPGLFiscalYearInformationDto.glzEndDate = dataTable.Rows[0].Field<DateTime?>("glzEndDate");
			eRPGLFiscalYearInformationDto.glzUniqueID = dataTable.Rows[0].Field<Guid>("glzUniqueID");
			eRPGLFiscalYearInformationDto.glzRowVersion = dataTable.Rows[0].Field<byte[]>("glzRowVersion");
			eRPGLFiscalYearInformationDto.glzGlFiscalYearID = dataTable.Rows[0].Field<short>("glzGlFiscalYearID");
			eRPGLFiscalYearInformationDto.glzStartDate = dataTable.Rows[0].Field<DateTime?>("glzStartDate");
			eRPGLFiscalYearInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPGLFiscalYearInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPGLFiscalYearInformationDto);
	}

	public Task<APIValidationInfoDto> SaveGLFiscalYear(ERPGLFiscalYearDto gLFiscalYear)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM GLFiscalYears WHERE glzUniqueID = " + M1Util.ConvertToLinq(gLFiscalYear.glzUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["glzGlFiscalYearID"] = gLFiscalYear.glzGlFiscalYearID;
				gLFiscalYear.glzUniqueID = ((gLFiscalYear.glzUniqueID == Guid.Empty) ? Guid.NewGuid() : gLFiscalYear.glzUniqueID);
				dataRow["glzUniqueID"] = gLFiscalYear.glzUniqueID;
				dataRow["glzCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["glzCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The GLFiscalYear could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (gLFiscalYear.glzRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the GLFiscalYear is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["glzRowVersion"], gLFiscalYear.glzRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the GLFiscalYear has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the GLFiscalYear again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			DataRow dataRow2 = dataRow;
			DateTime? glzEndDate = gLFiscalYear.glzEndDate;
			dataRow2["glzEndDate"] = (glzEndDate.HasValue ? ((object)glzEndDate.GetValueOrDefault()) : dataRow["glzEndDate"]);
			DataRow dataRow3 = dataRow;
			glzEndDate = gLFiscalYear.glzStartDate;
			dataRow3["glzStartDate"] = (glzEndDate.HasValue ? ((object)glzEndDate.GetValueOrDefault()) : dataRow["glzStartDate"]);
			if (gLFiscalYear.CustomFields != null && gLFiscalYear.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in gLFiscalYear.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the GLFiscalYear [{gLFiscalYear.glzUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the GLFiscalYear [{gLFiscalYear.glzUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
