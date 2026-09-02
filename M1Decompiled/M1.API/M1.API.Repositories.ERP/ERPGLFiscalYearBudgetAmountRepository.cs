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

public class ERPGLFiscalYearBudgetAmountRepository : APIBaseRepository, IERPGLFiscalYearBudgetAmountRepository, IAPIBaseRepository, IDisposable
{
	public ERPGLFiscalYearBudgetAmountRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesGLFiscalYearBudgetAmountExist(Guid gLFiscalYearBudgetAmountId)
	{
		InitializeParameterLists();
		base.filterList.Add("glbUniqueID|C", gLFiscalYearBudgetAmountId);
		base.selectList.Add("glbUniqueID");
		return Task.FromResult(GetAsObject("GLFiscalYearBudgetAmounts", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPGLFiscalYearBudgetAmountInformationDto>> GetAllGLFiscalYearBudgetAmounts(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPGLFiscalYearBudgetAmountInformationDto> collection = new List<ERPGLFiscalYearBudgetAmountInformationDto>();
		InitializeParameterLists();
		string[] array = new string[9] { "glbBudgetAmount", "glbBudgetHeaderID", "glbBudgetLineID", "glbCreatedBy", "glbCreatedDate", "glbUniqueID", "glbGlFiscalYearID", "glbGlFiscalYearPeriodID", "glbRowVersion" };
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("GLFiscalYearBudgetAmounts");
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
		using (DataTable dataTable = GetAsDataTable("GLFiscalYearBudgetAmounts", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPGLFiscalYearBudgetAmountInformationDto eRPGLFiscalYearBudgetAmountInformationDto = new ERPGLFiscalYearBudgetAmountInformationDto();
				eRPGLFiscalYearBudgetAmountInformationDto.glbBudgetAmount = dataTable.Rows[i].Field<decimal>("glbBudgetAmount");
				eRPGLFiscalYearBudgetAmountInformationDto.glbBudgetHeaderID = dataTable.Rows[i].Field<short>("glbBudgetHeaderID");
				eRPGLFiscalYearBudgetAmountInformationDto.glbBudgetLineID = dataTable.Rows[i].Field<short>("glbBudgetLineID");
				eRPGLFiscalYearBudgetAmountInformationDto.glbCreatedBy = dataTable.Rows[i].Field<string>("glbCreatedBy");
				eRPGLFiscalYearBudgetAmountInformationDto.glbCreatedDate = dataTable.Rows[i].Field<DateTime?>("glbCreatedDate");
				eRPGLFiscalYearBudgetAmountInformationDto.glbUniqueID = dataTable.Rows[i].Field<Guid>("glbUniqueID");
				eRPGLFiscalYearBudgetAmountInformationDto.glbGlFiscalYearID = dataTable.Rows[i].Field<short>("glbGlFiscalYearID");
				eRPGLFiscalYearBudgetAmountInformationDto.glbGlFiscalYearPeriodID = dataTable.Rows[i].Field<byte>("glbGlFiscalYearPeriodID");
				eRPGLFiscalYearBudgetAmountInformationDto.glbRowVersion = dataTable.Rows[i].Field<byte[]>("glbRowVersion");
				eRPGLFiscalYearBudgetAmountInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPGLFiscalYearBudgetAmountInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPGLFiscalYearBudgetAmountInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPGLFiscalYearBudgetAmountInformationDto> GetGLFiscalYearBudgetAmount(Guid gLFiscalYearBudgetAmountId)
	{
		ERPGLFiscalYearBudgetAmountInformationDto eRPGLFiscalYearBudgetAmountInformationDto = new ERPGLFiscalYearBudgetAmountInformationDto();
		InitializeParameterLists();
		string[] collection = new string[9] { "glbBudgetAmount", "glbBudgetHeaderID", "glbBudgetLineID", "glbCreatedBy", "glbCreatedDate", "glbUniqueID", "glbGlFiscalYearID", "glbGlFiscalYearPeriodID", "glbRowVersion" };
		base.selectList.AddRange(collection);
		base.filterList.Add("glbUniqueID|C", gLFiscalYearBudgetAmountId);
		AddCustomFieldsToSelectList("GLFiscalYearBudgetAmounts");
		using (DataTable dataTable = GetAsDataTable("GLFiscalYearBudgetAmounts", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPGLFiscalYearBudgetAmountInformationDto);
			}
			eRPGLFiscalYearBudgetAmountInformationDto.glbBudgetAmount = dataTable.Rows[0].Field<decimal>("glbBudgetAmount");
			eRPGLFiscalYearBudgetAmountInformationDto.glbBudgetHeaderID = dataTable.Rows[0].Field<short>("glbBudgetHeaderID");
			eRPGLFiscalYearBudgetAmountInformationDto.glbBudgetLineID = dataTable.Rows[0].Field<short>("glbBudgetLineID");
			eRPGLFiscalYearBudgetAmountInformationDto.glbCreatedBy = dataTable.Rows[0].Field<string>("glbCreatedBy");
			eRPGLFiscalYearBudgetAmountInformationDto.glbCreatedDate = dataTable.Rows[0].Field<DateTime?>("glbCreatedDate");
			eRPGLFiscalYearBudgetAmountInformationDto.glbUniqueID = dataTable.Rows[0].Field<Guid>("glbUniqueID");
			eRPGLFiscalYearBudgetAmountInformationDto.glbGlFiscalYearID = dataTable.Rows[0].Field<short>("glbGlFiscalYearID");
			eRPGLFiscalYearBudgetAmountInformationDto.glbGlFiscalYearPeriodID = dataTable.Rows[0].Field<byte>("glbGlFiscalYearPeriodID");
			eRPGLFiscalYearBudgetAmountInformationDto.glbRowVersion = dataTable.Rows[0].Field<byte[]>("glbRowVersion");
			eRPGLFiscalYearBudgetAmountInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPGLFiscalYearBudgetAmountInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPGLFiscalYearBudgetAmountInformationDto);
	}

	public Task<APIValidationInfoDto> SaveGLFiscalYearBudgetAmount(ERPGLFiscalYearBudgetAmountDto gLFiscalYearBudgetAmount)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM GLFiscalYearBudgetAmounts WHERE glbUniqueID = " + M1Util.ConvertToLinq(gLFiscalYearBudgetAmount.glbUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["glbGlFiscalYearID"] = gLFiscalYearBudgetAmount.glbGlFiscalYearID;
				dataRow["glbBudgetHeaderID"] = gLFiscalYearBudgetAmount.glbBudgetHeaderID;
				dataRow["glbBudgetLineID"] = gLFiscalYearBudgetAmount.glbBudgetLineID;
				dataRow["glbGlFiscalYearPeriodID"] = gLFiscalYearBudgetAmount.glbGlFiscalYearPeriodID;
				gLFiscalYearBudgetAmount.glbUniqueID = ((gLFiscalYearBudgetAmount.glbUniqueID == Guid.Empty) ? Guid.NewGuid() : gLFiscalYearBudgetAmount.glbUniqueID);
				dataRow["glbUniqueID"] = gLFiscalYearBudgetAmount.glbUniqueID;
				dataRow["glbCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["glbCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The GLFiscalYearBudgetAmount could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (gLFiscalYearBudgetAmount.glbRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the GLFiscalYearBudgetAmount is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["glbRowVersion"], gLFiscalYearBudgetAmount.glbRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the GLFiscalYearBudgetAmount has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the GLFiscalYearBudgetAmount again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["glbBudgetAmount"] = gLFiscalYearBudgetAmount.glbBudgetAmount;
			if (gLFiscalYearBudgetAmount.CustomFields != null && gLFiscalYearBudgetAmount.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in gLFiscalYearBudgetAmount.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the GLFiscalYearBudgetAmount [{gLFiscalYearBudgetAmount.glbUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the GLFiscalYearBudgetAmount [{gLFiscalYearBudgetAmount.glbUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
