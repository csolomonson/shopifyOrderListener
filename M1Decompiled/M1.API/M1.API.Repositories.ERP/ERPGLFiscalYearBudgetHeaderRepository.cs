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

public class ERPGLFiscalYearBudgetHeaderRepository : APIBaseRepository, IERPGLFiscalYearBudgetHeaderRepository, IAPIBaseRepository, IDisposable
{
	public ERPGLFiscalYearBudgetHeaderRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesGLFiscalYearBudgetHeaderExist(Guid gLFiscalYearBudgetHeaderId)
	{
		InitializeParameterLists();
		base.filterList.Add("glkUniqueID|C", gLFiscalYearBudgetHeaderId);
		base.selectList.Add("glkUniqueID");
		return Task.FromResult(GetAsObject("GLFiscalYearBudgetHeaders", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPGLFiscalYearBudgetHeaderInformationDto>> GetAllGLFiscalYearBudgetHeaders(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPGLFiscalYearBudgetHeaderInformationDto> collection = new List<ERPGLFiscalYearBudgetHeaderInformationDto>();
		InitializeParameterLists();
		string[] array = new string[8] { "glkAnnualAmount", "glkBudgetHeaderID", "glkCreatedBy", "glkCreatedDate", "glkUniqueID", "glkGlAccountID", "glkGlFiscalYearID", "glkRowVersion" };
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("GLFiscalYearBudgetHeaders");
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
		using (DataTable dataTable = GetAsDataTable("GLFiscalYearBudgetHeaders", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPGLFiscalYearBudgetHeaderInformationDto eRPGLFiscalYearBudgetHeaderInformationDto = new ERPGLFiscalYearBudgetHeaderInformationDto();
				eRPGLFiscalYearBudgetHeaderInformationDto.glkAnnualAmount = dataTable.Rows[i].Field<decimal>("glkAnnualAmount");
				eRPGLFiscalYearBudgetHeaderInformationDto.glkBudgetHeaderID = dataTable.Rows[i].Field<short>("glkBudgetHeaderID");
				eRPGLFiscalYearBudgetHeaderInformationDto.glkCreatedBy = dataTable.Rows[i].Field<string>("glkCreatedBy");
				eRPGLFiscalYearBudgetHeaderInformationDto.glkCreatedDate = dataTable.Rows[i].Field<DateTime?>("glkCreatedDate");
				eRPGLFiscalYearBudgetHeaderInformationDto.glkUniqueID = dataTable.Rows[i].Field<Guid>("glkUniqueID");
				eRPGLFiscalYearBudgetHeaderInformationDto.glkGlAccountID = dataTable.Rows[i].Field<string>("glkGlAccountID");
				eRPGLFiscalYearBudgetHeaderInformationDto.glkGlFiscalYearID = dataTable.Rows[i].Field<short>("glkGlFiscalYearID");
				eRPGLFiscalYearBudgetHeaderInformationDto.glkRowVersion = dataTable.Rows[i].Field<byte[]>("glkRowVersion");
				eRPGLFiscalYearBudgetHeaderInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPGLFiscalYearBudgetHeaderInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPGLFiscalYearBudgetHeaderInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPGLFiscalYearBudgetHeaderInformationDto> GetGLFiscalYearBudgetHeader(Guid gLFiscalYearBudgetHeaderId)
	{
		ERPGLFiscalYearBudgetHeaderInformationDto eRPGLFiscalYearBudgetHeaderInformationDto = new ERPGLFiscalYearBudgetHeaderInformationDto();
		InitializeParameterLists();
		string[] collection = new string[8] { "glkAnnualAmount", "glkBudgetHeaderID", "glkCreatedBy", "glkCreatedDate", "glkUniqueID", "glkGlAccountID", "glkGlFiscalYearID", "glkRowVersion" };
		base.selectList.AddRange(collection);
		base.filterList.Add("glkUniqueID|C", gLFiscalYearBudgetHeaderId);
		AddCustomFieldsToSelectList("GLFiscalYearBudgetHeaders");
		using (DataTable dataTable = GetAsDataTable("GLFiscalYearBudgetHeaders", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPGLFiscalYearBudgetHeaderInformationDto);
			}
			eRPGLFiscalYearBudgetHeaderInformationDto.glkAnnualAmount = dataTable.Rows[0].Field<decimal>("glkAnnualAmount");
			eRPGLFiscalYearBudgetHeaderInformationDto.glkBudgetHeaderID = dataTable.Rows[0].Field<short>("glkBudgetHeaderID");
			eRPGLFiscalYearBudgetHeaderInformationDto.glkCreatedBy = dataTable.Rows[0].Field<string>("glkCreatedBy");
			eRPGLFiscalYearBudgetHeaderInformationDto.glkCreatedDate = dataTable.Rows[0].Field<DateTime?>("glkCreatedDate");
			eRPGLFiscalYearBudgetHeaderInformationDto.glkUniqueID = dataTable.Rows[0].Field<Guid>("glkUniqueID");
			eRPGLFiscalYearBudgetHeaderInformationDto.glkGlAccountID = dataTable.Rows[0].Field<string>("glkGlAccountID");
			eRPGLFiscalYearBudgetHeaderInformationDto.glkGlFiscalYearID = dataTable.Rows[0].Field<short>("glkGlFiscalYearID");
			eRPGLFiscalYearBudgetHeaderInformationDto.glkRowVersion = dataTable.Rows[0].Field<byte[]>("glkRowVersion");
			eRPGLFiscalYearBudgetHeaderInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPGLFiscalYearBudgetHeaderInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPGLFiscalYearBudgetHeaderInformationDto);
	}

	public Task<APIValidationInfoDto> SaveGLFiscalYearBudgetHeader(ERPGLFiscalYearBudgetHeaderDto gLFiscalYearBudgetHeader)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM GLFiscalYearBudgetHeaders WHERE glkUniqueID = " + M1Util.ConvertToLinq(gLFiscalYearBudgetHeader.glkUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["glkGlFiscalYearID"] = gLFiscalYearBudgetHeader.glkGlFiscalYearID;
				dataRow["glkBudgetHeaderID"] = gLFiscalYearBudgetHeader.glkBudgetHeaderID;
				gLFiscalYearBudgetHeader.glkUniqueID = ((gLFiscalYearBudgetHeader.glkUniqueID == Guid.Empty) ? Guid.NewGuid() : gLFiscalYearBudgetHeader.glkUniqueID);
				dataRow["glkUniqueID"] = gLFiscalYearBudgetHeader.glkUniqueID;
				dataRow["glkCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["glkCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The GLFiscalYearBudgetHeader could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (gLFiscalYearBudgetHeader.glkRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the GLFiscalYearBudgetHeader is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["glkRowVersion"], gLFiscalYearBudgetHeader.glkRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the GLFiscalYearBudgetHeader has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the GLFiscalYearBudgetHeader again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["glkAnnualAmount"] = gLFiscalYearBudgetHeader.glkAnnualAmount;
			dataRow["glkGlAccountID"] = gLFiscalYearBudgetHeader.glkGlAccountID;
			if (gLFiscalYearBudgetHeader.CustomFields != null && gLFiscalYearBudgetHeader.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in gLFiscalYearBudgetHeader.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the GLFiscalYearBudgetHeader [{gLFiscalYearBudgetHeader.glkUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the GLFiscalYearBudgetHeader [{gLFiscalYearBudgetHeader.glkUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
