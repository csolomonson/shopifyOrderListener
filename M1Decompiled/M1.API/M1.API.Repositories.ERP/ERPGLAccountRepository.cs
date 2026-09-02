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

public class ERPGLAccountRepository : APIBaseRepository, IERPGLAccountRepository, IAPIBaseRepository, IDisposable
{
	public ERPGLAccountRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesGLAccountExist(Guid gLAccountId)
	{
		InitializeParameterLists();
		base.filterList.Add("glaUniqueID|C", gLAccountId);
		base.selectList.Add("glaUniqueID");
		return Task.FromResult(GetAsObject("GLAccounts", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPGLAccountInformationDto>> GetAllGLAccounts(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPGLAccountInformationDto> collection = new List<ERPGLAccountInformationDto>();
		InitializeParameterLists();
		string[] array = new string[11]
		{
			"glaGlAccountID", "glaCreatedBy", "glaCreatedDate", "glaUniqueID", "glaExternalGlCode", "glaGlChartID", "glaGlDepartmentID", "glaGlDivisionID", "glaInactiveDate", "glaInactive",
			"glaRowVersion"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("GLAccounts");
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
		using (DataTable dataTable = GetAsDataTable("GLAccounts", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPGLAccountInformationDto eRPGLAccountInformationDto = new ERPGLAccountInformationDto();
				eRPGLAccountInformationDto.glaGlAccountID = dataTable.Rows[i].Field<string>("glaGlAccountID");
				eRPGLAccountInformationDto.glaCreatedBy = dataTable.Rows[i].Field<string>("glaCreatedBy");
				eRPGLAccountInformationDto.glaCreatedDate = dataTable.Rows[i].Field<DateTime?>("glaCreatedDate");
				eRPGLAccountInformationDto.glaUniqueID = dataTable.Rows[i].Field<Guid>("glaUniqueID");
				eRPGLAccountInformationDto.glaExternalGlCode = dataTable.Rows[i].Field<string>("glaExternalGlCode");
				eRPGLAccountInformationDto.glaGlChartID = dataTable.Rows[i].Field<string>("glaGlChartID");
				eRPGLAccountInformationDto.glaGlDepartmentID = dataTable.Rows[i].Field<string>("glaGlDepartmentID");
				eRPGLAccountInformationDto.glaGlDivisionID = dataTable.Rows[i].Field<string>("glaGlDivisionID");
				eRPGLAccountInformationDto.glaInactiveDate = dataTable.Rows[i].Field<DateTime?>("glaInactiveDate");
				eRPGLAccountInformationDto.glaInactive = dataTable.Rows[i].Field<bool>("glaInactive");
				eRPGLAccountInformationDto.glaRowVersion = dataTable.Rows[i].Field<byte[]>("glaRowVersion");
				eRPGLAccountInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPGLAccountInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPGLAccountInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPGLAccountInformationDto> GetGLAccount(Guid gLAccountId)
	{
		ERPGLAccountInformationDto eRPGLAccountInformationDto = new ERPGLAccountInformationDto();
		InitializeParameterLists();
		string[] collection = new string[11]
		{
			"glaGlAccountID", "glaCreatedBy", "glaCreatedDate", "glaUniqueID", "glaExternalGlCode", "glaGlChartID", "glaGlDepartmentID", "glaGlDivisionID", "glaInactiveDate", "glaInactive",
			"glaRowVersion"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("glaUniqueID|C", gLAccountId);
		AddCustomFieldsToSelectList("GLAccounts");
		using (DataTable dataTable = GetAsDataTable("GLAccounts", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPGLAccountInformationDto);
			}
			eRPGLAccountInformationDto.glaGlAccountID = dataTable.Rows[0].Field<string>("glaGlAccountID");
			eRPGLAccountInformationDto.glaCreatedBy = dataTable.Rows[0].Field<string>("glaCreatedBy");
			eRPGLAccountInformationDto.glaCreatedDate = dataTable.Rows[0].Field<DateTime?>("glaCreatedDate");
			eRPGLAccountInformationDto.glaUniqueID = dataTable.Rows[0].Field<Guid>("glaUniqueID");
			eRPGLAccountInformationDto.glaExternalGlCode = dataTable.Rows[0].Field<string>("glaExternalGlCode");
			eRPGLAccountInformationDto.glaGlChartID = dataTable.Rows[0].Field<string>("glaGlChartID");
			eRPGLAccountInformationDto.glaGlDepartmentID = dataTable.Rows[0].Field<string>("glaGlDepartmentID");
			eRPGLAccountInformationDto.glaGlDivisionID = dataTable.Rows[0].Field<string>("glaGlDivisionID");
			eRPGLAccountInformationDto.glaInactiveDate = dataTable.Rows[0].Field<DateTime?>("glaInactiveDate");
			eRPGLAccountInformationDto.glaInactive = dataTable.Rows[0].Field<bool>("glaInactive");
			eRPGLAccountInformationDto.glaRowVersion = dataTable.Rows[0].Field<byte[]>("glaRowVersion");
			eRPGLAccountInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPGLAccountInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPGLAccountInformationDto);
	}

	public Task<APIValidationInfoDto> SaveGLAccount(ERPGLAccountDto gLAccount)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM GLAccounts WHERE glaUniqueID = " + M1Util.ConvertToLinq(gLAccount.glaUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["glaGlAccountID"] = gLAccount.glaGlAccountID.ToUpper();
				gLAccount.glaUniqueID = ((gLAccount.glaUniqueID == Guid.Empty) ? Guid.NewGuid() : gLAccount.glaUniqueID);
				dataRow["glaUniqueID"] = gLAccount.glaUniqueID;
				dataRow["glaCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["glaCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The GLAccount could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (gLAccount.glaRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the GLAccount is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["glaRowVersion"], gLAccount.glaRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the GLAccount has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the GLAccount again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["glaExternalGlCode"] = gLAccount.glaExternalGlCode;
			dataRow["glaGlChartID"] = gLAccount.glaGlChartID;
			dataRow["glaGlDepartmentID"] = gLAccount.glaGlDepartmentID;
			dataRow["glaGlDivisionID"] = gLAccount.glaGlDivisionID;
			DataRow dataRow2 = dataRow;
			DateTime? glaInactiveDate = gLAccount.glaInactiveDate;
			dataRow2["glaInactiveDate"] = (glaInactiveDate.HasValue ? ((object)glaInactiveDate.GetValueOrDefault()) : dataRow["glaInactiveDate"]);
			dataRow["glaInactive"] = gLAccount.glaInactive;
			if (gLAccount.CustomFields != null && gLAccount.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in gLAccount.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the GLAccount [{gLAccount.glaUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the GLAccount [{gLAccount.glaUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
