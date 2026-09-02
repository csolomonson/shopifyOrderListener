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

public class ERPGLFiscalYearBudgetLineRepository : APIBaseRepository, IERPGLFiscalYearBudgetLineRepository, IAPIBaseRepository, IDisposable
{
	public ERPGLFiscalYearBudgetLineRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesGLFiscalYearBudgetLineExist(Guid gLFiscalYearBudgetLineId)
	{
		InitializeParameterLists();
		base.filterList.Add("glgUniqueID|C", gLFiscalYearBudgetLineId);
		base.selectList.Add("glgUniqueID");
		return Task.FromResult(GetAsObject("GLFiscalYearBudgetLines", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPGLFiscalYearBudgetLineInformationDto>> GetAllGLFiscalYearBudgetLines(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPGLFiscalYearBudgetLineInformationDto> collection = new List<ERPGLFiscalYearBudgetLineInformationDto>();
		InitializeParameterLists();
		string[] array = new string[8] { "glgAnnualAmount", "glgBudgetHeaderID", "glgBudgetLineID", "glgCreatedBy", "glgCreatedDate", "glgUniqueID", "glgGlFiscalYearID", "glgRowVersion" };
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("GLFiscalYearBudgetLines");
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
		using (DataTable dataTable = GetAsDataTable("GLFiscalYearBudgetLines", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPGLFiscalYearBudgetLineInformationDto eRPGLFiscalYearBudgetLineInformationDto = new ERPGLFiscalYearBudgetLineInformationDto();
				eRPGLFiscalYearBudgetLineInformationDto.glgAnnualAmount = dataTable.Rows[i].Field<decimal>("glgAnnualAmount");
				eRPGLFiscalYearBudgetLineInformationDto.glgBudgetHeaderID = dataTable.Rows[i].Field<short>("glgBudgetHeaderID");
				eRPGLFiscalYearBudgetLineInformationDto.glgBudgetLineID = dataTable.Rows[i].Field<short>("glgBudgetLineID");
				eRPGLFiscalYearBudgetLineInformationDto.glgCreatedBy = dataTable.Rows[i].Field<string>("glgCreatedBy");
				eRPGLFiscalYearBudgetLineInformationDto.glgCreatedDate = dataTable.Rows[i].Field<DateTime?>("glgCreatedDate");
				eRPGLFiscalYearBudgetLineInformationDto.glgUniqueID = dataTable.Rows[i].Field<Guid>("glgUniqueID");
				eRPGLFiscalYearBudgetLineInformationDto.glgGlFiscalYearID = dataTable.Rows[i].Field<short>("glgGlFiscalYearID");
				eRPGLFiscalYearBudgetLineInformationDto.glgRowVersion = dataTable.Rows[i].Field<byte[]>("glgRowVersion");
				eRPGLFiscalYearBudgetLineInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPGLFiscalYearBudgetLineInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPGLFiscalYearBudgetLineInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPGLFiscalYearBudgetLineInformationDto> GetGLFiscalYearBudgetLine(Guid gLFiscalYearBudgetLineId)
	{
		ERPGLFiscalYearBudgetLineInformationDto eRPGLFiscalYearBudgetLineInformationDto = new ERPGLFiscalYearBudgetLineInformationDto();
		InitializeParameterLists();
		string[] collection = new string[8] { "glgAnnualAmount", "glgBudgetHeaderID", "glgBudgetLineID", "glgCreatedBy", "glgCreatedDate", "glgUniqueID", "glgGlFiscalYearID", "glgRowVersion" };
		base.selectList.AddRange(collection);
		base.filterList.Add("glgUniqueID|C", gLFiscalYearBudgetLineId);
		AddCustomFieldsToSelectList("GLFiscalYearBudgetLines");
		using (DataTable dataTable = GetAsDataTable("GLFiscalYearBudgetLines", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPGLFiscalYearBudgetLineInformationDto);
			}
			eRPGLFiscalYearBudgetLineInformationDto.glgAnnualAmount = dataTable.Rows[0].Field<decimal>("glgAnnualAmount");
			eRPGLFiscalYearBudgetLineInformationDto.glgBudgetHeaderID = dataTable.Rows[0].Field<short>("glgBudgetHeaderID");
			eRPGLFiscalYearBudgetLineInformationDto.glgBudgetLineID = dataTable.Rows[0].Field<short>("glgBudgetLineID");
			eRPGLFiscalYearBudgetLineInformationDto.glgCreatedBy = dataTable.Rows[0].Field<string>("glgCreatedBy");
			eRPGLFiscalYearBudgetLineInformationDto.glgCreatedDate = dataTable.Rows[0].Field<DateTime?>("glgCreatedDate");
			eRPGLFiscalYearBudgetLineInformationDto.glgUniqueID = dataTable.Rows[0].Field<Guid>("glgUniqueID");
			eRPGLFiscalYearBudgetLineInformationDto.glgGlFiscalYearID = dataTable.Rows[0].Field<short>("glgGlFiscalYearID");
			eRPGLFiscalYearBudgetLineInformationDto.glgRowVersion = dataTable.Rows[0].Field<byte[]>("glgRowVersion");
			eRPGLFiscalYearBudgetLineInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPGLFiscalYearBudgetLineInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPGLFiscalYearBudgetLineInformationDto);
	}

	public Task<APIValidationInfoDto> SaveGLFiscalYearBudgetLine(ERPGLFiscalYearBudgetLineDto gLFiscalYearBudgetLine)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM GLFiscalYearBudgetLines WHERE glgUniqueID = " + M1Util.ConvertToLinq(gLFiscalYearBudgetLine.glgUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["glgGlFiscalYearID"] = gLFiscalYearBudgetLine.glgGlFiscalYearID;
				dataRow["glgBudgetHeaderID"] = gLFiscalYearBudgetLine.glgBudgetHeaderID;
				dataRow["glgBudgetLineID"] = gLFiscalYearBudgetLine.glgBudgetLineID;
				gLFiscalYearBudgetLine.glgUniqueID = ((gLFiscalYearBudgetLine.glgUniqueID == Guid.Empty) ? Guid.NewGuid() : gLFiscalYearBudgetLine.glgUniqueID);
				dataRow["glgUniqueID"] = gLFiscalYearBudgetLine.glgUniqueID;
				dataRow["glgCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["glgCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The GLFiscalYearBudgetLine could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (gLFiscalYearBudgetLine.glgRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the GLFiscalYearBudgetLine is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["glgRowVersion"], gLFiscalYearBudgetLine.glgRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the GLFiscalYearBudgetLine has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the GLFiscalYearBudgetLine again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["glgAnnualAmount"] = gLFiscalYearBudgetLine.glgAnnualAmount;
			if (gLFiscalYearBudgetLine.CustomFields != null && gLFiscalYearBudgetLine.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in gLFiscalYearBudgetLine.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the GLFiscalYearBudgetLine [{gLFiscalYearBudgetLine.glgUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the GLFiscalYearBudgetLine [{gLFiscalYearBudgetLine.glgUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
