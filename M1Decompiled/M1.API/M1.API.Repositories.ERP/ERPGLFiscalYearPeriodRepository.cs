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

public class ERPGLFiscalYearPeriodRepository : APIBaseRepository, IERPGLFiscalYearPeriodRepository, IAPIBaseRepository, IDisposable
{
	public ERPGLFiscalYearPeriodRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesGLFiscalYearPeriodExist(Guid gLFiscalYearPeriodId)
	{
		InitializeParameterLists();
		base.filterList.Add("glfUniqueID|C", gLFiscalYearPeriodId);
		base.selectList.Add("glfUniqueID");
		return Task.FromResult(GetAsObject("GLFiscalYearPeriods", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPGLFiscalYearPeriodInformationDto>> GetAllGLFiscalYearPeriods(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPGLFiscalYearPeriodInformationDto> collection = new List<ERPGLFiscalYearPeriodInformationDto>();
		InitializeParameterLists();
		string[] array = new string[11]
		{
			"glfCreatedBy", "glfCreatedDate", "glfEndDate", "glfUniqueID", "glfGlFiscalYearID", "glfApClosed", "glfArClosed", "glfGlClosed", "glfRowVersion", "glfGlFiscalYearPeriodID",
			"glfStartDate"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("GLFiscalYearPeriods");
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
		using (DataTable dataTable = GetAsDataTable("GLFiscalYearPeriods", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPGLFiscalYearPeriodInformationDto eRPGLFiscalYearPeriodInformationDto = new ERPGLFiscalYearPeriodInformationDto();
				eRPGLFiscalYearPeriodInformationDto.glfCreatedBy = dataTable.Rows[i].Field<string>("glfCreatedBy");
				eRPGLFiscalYearPeriodInformationDto.glfCreatedDate = dataTable.Rows[i].Field<DateTime?>("glfCreatedDate");
				eRPGLFiscalYearPeriodInformationDto.glfEndDate = dataTable.Rows[i].Field<DateTime?>("glfEndDate");
				eRPGLFiscalYearPeriodInformationDto.glfUniqueID = dataTable.Rows[i].Field<Guid>("glfUniqueID");
				eRPGLFiscalYearPeriodInformationDto.glfGlFiscalYearID = dataTable.Rows[i].Field<short>("glfGlFiscalYearID");
				eRPGLFiscalYearPeriodInformationDto.glfApClosed = dataTable.Rows[i].Field<bool>("glfApClosed");
				eRPGLFiscalYearPeriodInformationDto.glfArClosed = dataTable.Rows[i].Field<bool>("glfArClosed");
				eRPGLFiscalYearPeriodInformationDto.glfGlClosed = dataTable.Rows[i].Field<bool>("glfGlClosed");
				eRPGLFiscalYearPeriodInformationDto.glfRowVersion = dataTable.Rows[i].Field<byte[]>("glfRowVersion");
				eRPGLFiscalYearPeriodInformationDto.glfGlFiscalYearPeriodID = dataTable.Rows[i].Field<byte>("glfGlFiscalYearPeriodID");
				eRPGLFiscalYearPeriodInformationDto.glfStartDate = dataTable.Rows[i].Field<DateTime?>("glfStartDate");
				eRPGLFiscalYearPeriodInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPGLFiscalYearPeriodInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPGLFiscalYearPeriodInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPGLFiscalYearPeriodInformationDto> GetGLFiscalYearPeriod(Guid gLFiscalYearPeriodId)
	{
		ERPGLFiscalYearPeriodInformationDto eRPGLFiscalYearPeriodInformationDto = new ERPGLFiscalYearPeriodInformationDto();
		InitializeParameterLists();
		string[] collection = new string[11]
		{
			"glfCreatedBy", "glfCreatedDate", "glfEndDate", "glfUniqueID", "glfGlFiscalYearID", "glfApClosed", "glfArClosed", "glfGlClosed", "glfRowVersion", "glfGlFiscalYearPeriodID",
			"glfStartDate"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("glfUniqueID|C", gLFiscalYearPeriodId);
		AddCustomFieldsToSelectList("GLFiscalYearPeriods");
		using (DataTable dataTable = GetAsDataTable("GLFiscalYearPeriods", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPGLFiscalYearPeriodInformationDto);
			}
			eRPGLFiscalYearPeriodInformationDto.glfCreatedBy = dataTable.Rows[0].Field<string>("glfCreatedBy");
			eRPGLFiscalYearPeriodInformationDto.glfCreatedDate = dataTable.Rows[0].Field<DateTime?>("glfCreatedDate");
			eRPGLFiscalYearPeriodInformationDto.glfEndDate = dataTable.Rows[0].Field<DateTime?>("glfEndDate");
			eRPGLFiscalYearPeriodInformationDto.glfUniqueID = dataTable.Rows[0].Field<Guid>("glfUniqueID");
			eRPGLFiscalYearPeriodInformationDto.glfGlFiscalYearID = dataTable.Rows[0].Field<short>("glfGlFiscalYearID");
			eRPGLFiscalYearPeriodInformationDto.glfApClosed = dataTable.Rows[0].Field<bool>("glfApClosed");
			eRPGLFiscalYearPeriodInformationDto.glfArClosed = dataTable.Rows[0].Field<bool>("glfArClosed");
			eRPGLFiscalYearPeriodInformationDto.glfGlClosed = dataTable.Rows[0].Field<bool>("glfGlClosed");
			eRPGLFiscalYearPeriodInformationDto.glfRowVersion = dataTable.Rows[0].Field<byte[]>("glfRowVersion");
			eRPGLFiscalYearPeriodInformationDto.glfGlFiscalYearPeriodID = dataTable.Rows[0].Field<byte>("glfGlFiscalYearPeriodID");
			eRPGLFiscalYearPeriodInformationDto.glfStartDate = dataTable.Rows[0].Field<DateTime?>("glfStartDate");
			eRPGLFiscalYearPeriodInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPGLFiscalYearPeriodInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPGLFiscalYearPeriodInformationDto);
	}

	public Task<APIValidationInfoDto> SaveGLFiscalYearPeriod(ERPGLFiscalYearPeriodDto gLFiscalYearPeriod)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM GLFiscalYearPeriods WHERE glfUniqueID = " + M1Util.ConvertToLinq(gLFiscalYearPeriod.glfUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["glfGlFiscalYearID"] = gLFiscalYearPeriod.glfGlFiscalYearID;
				dataRow["glfGlFiscalYearPeriodID"] = gLFiscalYearPeriod.glfGlFiscalYearPeriodID;
				gLFiscalYearPeriod.glfUniqueID = ((gLFiscalYearPeriod.glfUniqueID == Guid.Empty) ? Guid.NewGuid() : gLFiscalYearPeriod.glfUniqueID);
				dataRow["glfUniqueID"] = gLFiscalYearPeriod.glfUniqueID;
				dataRow["glfCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["glfCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The GLFiscalYearPeriod could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (gLFiscalYearPeriod.glfRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the GLFiscalYearPeriod is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["glfRowVersion"], gLFiscalYearPeriod.glfRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the GLFiscalYearPeriod has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the GLFiscalYearPeriod again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			DataRow dataRow2 = dataRow;
			DateTime? glfEndDate = gLFiscalYearPeriod.glfEndDate;
			dataRow2["glfEndDate"] = (glfEndDate.HasValue ? ((object)glfEndDate.GetValueOrDefault()) : dataRow["glfEndDate"]);
			dataRow["glfApClosed"] = gLFiscalYearPeriod.glfApClosed;
			dataRow["glfArClosed"] = gLFiscalYearPeriod.glfArClosed;
			dataRow["glfGlClosed"] = gLFiscalYearPeriod.glfGlClosed;
			DataRow dataRow3 = dataRow;
			glfEndDate = gLFiscalYearPeriod.glfStartDate;
			dataRow3["glfStartDate"] = (glfEndDate.HasValue ? ((object)glfEndDate.GetValueOrDefault()) : dataRow["glfStartDate"]);
			if (gLFiscalYearPeriod.CustomFields != null && gLFiscalYearPeriod.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in gLFiscalYearPeriod.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the GLFiscalYearPeriod [{gLFiscalYearPeriod.glfUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the GLFiscalYearPeriod [{gLFiscalYearPeriod.glfUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
