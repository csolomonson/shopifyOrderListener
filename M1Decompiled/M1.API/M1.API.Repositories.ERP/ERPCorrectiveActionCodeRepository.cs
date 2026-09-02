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

public class ERPCorrectiveActionCodeRepository : APIBaseRepository, IERPCorrectiveActionCodeRepository, IAPIBaseRepository, IDisposable
{
	public ERPCorrectiveActionCodeRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesCorrectiveActionCodeExist(Guid correctiveActionCodeId)
	{
		InitializeParameterLists();
		base.filterList.Add("qaoUniqueID|C", correctiveActionCodeId);
		base.selectList.Add("qaoUniqueID");
		return Task.FromResult(GetAsObject("CorrectiveActionCodes", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPCorrectiveActionCodeInformationDto>> GetAllCorrectiveActionCodes(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPCorrectiveActionCodeInformationDto> collection = new List<ERPCorrectiveActionCodeInformationDto>();
		InitializeParameterLists();
		string[] array = new string[8] { "qaoCorrectiveActionCodeID", "qaoCorrectiveActionCategoryID", "qaoCreatedBy", "qaoCreatedDate", "qaoDescription", "qaoUniqueID", "qaoHoursAllowed", "qaoRowVersion" };
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("CorrectiveActionCodes");
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
		using (DataTable dataTable = GetAsDataTable("CorrectiveActionCodes", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPCorrectiveActionCodeInformationDto eRPCorrectiveActionCodeInformationDto = new ERPCorrectiveActionCodeInformationDto();
				eRPCorrectiveActionCodeInformationDto.qaoCorrectiveActionCodeID = dataTable.Rows[i].Field<string>("qaoCorrectiveActionCodeID");
				eRPCorrectiveActionCodeInformationDto.qaoCorrectiveActionCategoryID = dataTable.Rows[i].Field<string>("qaoCorrectiveActionCategoryID");
				eRPCorrectiveActionCodeInformationDto.qaoCreatedBy = dataTable.Rows[i].Field<string>("qaoCreatedBy");
				eRPCorrectiveActionCodeInformationDto.qaoCreatedDate = dataTable.Rows[i].Field<DateTime?>("qaoCreatedDate");
				eRPCorrectiveActionCodeInformationDto.qaoDescription = dataTable.Rows[i].Field<string>("qaoDescription");
				eRPCorrectiveActionCodeInformationDto.qaoUniqueID = dataTable.Rows[i].Field<Guid>("qaoUniqueID");
				eRPCorrectiveActionCodeInformationDto.qaoHoursAllowed = dataTable.Rows[i].Field<decimal>("qaoHoursAllowed");
				eRPCorrectiveActionCodeInformationDto.qaoRowVersion = dataTable.Rows[i].Field<byte[]>("qaoRowVersion");
				eRPCorrectiveActionCodeInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPCorrectiveActionCodeInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPCorrectiveActionCodeInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPCorrectiveActionCodeInformationDto> GetCorrectiveActionCode(Guid correctiveActionCodeId)
	{
		ERPCorrectiveActionCodeInformationDto eRPCorrectiveActionCodeInformationDto = new ERPCorrectiveActionCodeInformationDto();
		InitializeParameterLists();
		string[] collection = new string[8] { "qaoCorrectiveActionCodeID", "qaoCorrectiveActionCategoryID", "qaoCreatedBy", "qaoCreatedDate", "qaoDescription", "qaoUniqueID", "qaoHoursAllowed", "qaoRowVersion" };
		base.selectList.AddRange(collection);
		base.filterList.Add("qaoUniqueID|C", correctiveActionCodeId);
		AddCustomFieldsToSelectList("CorrectiveActionCodes");
		using (DataTable dataTable = GetAsDataTable("CorrectiveActionCodes", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPCorrectiveActionCodeInformationDto);
			}
			eRPCorrectiveActionCodeInformationDto.qaoCorrectiveActionCodeID = dataTable.Rows[0].Field<string>("qaoCorrectiveActionCodeID");
			eRPCorrectiveActionCodeInformationDto.qaoCorrectiveActionCategoryID = dataTable.Rows[0].Field<string>("qaoCorrectiveActionCategoryID");
			eRPCorrectiveActionCodeInformationDto.qaoCreatedBy = dataTable.Rows[0].Field<string>("qaoCreatedBy");
			eRPCorrectiveActionCodeInformationDto.qaoCreatedDate = dataTable.Rows[0].Field<DateTime?>("qaoCreatedDate");
			eRPCorrectiveActionCodeInformationDto.qaoDescription = dataTable.Rows[0].Field<string>("qaoDescription");
			eRPCorrectiveActionCodeInformationDto.qaoUniqueID = dataTable.Rows[0].Field<Guid>("qaoUniqueID");
			eRPCorrectiveActionCodeInformationDto.qaoHoursAllowed = dataTable.Rows[0].Field<decimal>("qaoHoursAllowed");
			eRPCorrectiveActionCodeInformationDto.qaoRowVersion = dataTable.Rows[0].Field<byte[]>("qaoRowVersion");
			eRPCorrectiveActionCodeInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPCorrectiveActionCodeInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPCorrectiveActionCodeInformationDto);
	}

	public Task<APIValidationInfoDto> SaveCorrectiveActionCode(ERPCorrectiveActionCodeDto correctiveActionCode)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM CorrectiveActionCodes WHERE qaoUniqueID = " + M1Util.ConvertToLinq(correctiveActionCode.qaoUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["qaoCorrectiveActionCodeID"] = correctiveActionCode.qaoCorrectiveActionCodeID.ToUpper();
				correctiveActionCode.qaoUniqueID = ((correctiveActionCode.qaoUniqueID == Guid.Empty) ? Guid.NewGuid() : correctiveActionCode.qaoUniqueID);
				dataRow["qaoUniqueID"] = correctiveActionCode.qaoUniqueID;
				dataRow["qaoCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["qaoCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The CorrectiveActionCode could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (correctiveActionCode.qaoRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the CorrectiveActionCode is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["qaoRowVersion"], correctiveActionCode.qaoRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the CorrectiveActionCode has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the CorrectiveActionCode again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["qaoCorrectiveActionCategoryID"] = correctiveActionCode.qaoCorrectiveActionCategoryID;
			dataRow["qaoDescription"] = correctiveActionCode.qaoDescription;
			dataRow["qaoHoursAllowed"] = correctiveActionCode.qaoHoursAllowed;
			if (correctiveActionCode.CustomFields != null && correctiveActionCode.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in correctiveActionCode.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the CorrectiveActionCode [{correctiveActionCode.qaoUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the CorrectiveActionCode [{correctiveActionCode.qaoUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
