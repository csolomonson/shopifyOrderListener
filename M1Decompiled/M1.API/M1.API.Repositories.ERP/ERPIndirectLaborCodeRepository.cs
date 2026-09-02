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

public class ERPIndirectLaborCodeRepository : APIBaseRepository, IERPIndirectLaborCodeRepository, IAPIBaseRepository, IDisposable
{
	public ERPIndirectLaborCodeRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesIndirectLaborCodeExist(Guid indirectLaborCodeId)
	{
		InitializeParameterLists();
		base.filterList.Add("lmiUniqueID|C", indirectLaborCodeId);
		base.selectList.Add("lmiUniqueID");
		return Task.FromResult(GetAsObject("IndirectLaborCodes", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPIndirectLaborCodeInformationDto>> GetAllIndirectLaborCodes(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPIndirectLaborCodeInformationDto> collection = new List<ERPIndirectLaborCodeInformationDto>();
		InitializeParameterLists();
		string[] array = new string[9] { "lmiCreatedBy", "lmiCreatedDate", "lmiDescription", "lmiUniqueID", "lmiInactiveDate", "lmiIndirectLaborID", "lmiIndirectLaborType", "lmiInactive", "lmiRowVersion" };
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("IndirectLaborCodes");
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
		using (DataTable dataTable = GetAsDataTable("IndirectLaborCodes", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPIndirectLaborCodeInformationDto eRPIndirectLaborCodeInformationDto = new ERPIndirectLaborCodeInformationDto();
				eRPIndirectLaborCodeInformationDto.lmiCreatedBy = dataTable.Rows[i].Field<string>("lmiCreatedBy");
				eRPIndirectLaborCodeInformationDto.lmiCreatedDate = dataTable.Rows[i].Field<DateTime?>("lmiCreatedDate");
				eRPIndirectLaborCodeInformationDto.lmiDescription = dataTable.Rows[i].Field<string>("lmiDescription");
				eRPIndirectLaborCodeInformationDto.lmiUniqueID = dataTable.Rows[i].Field<Guid>("lmiUniqueID");
				eRPIndirectLaborCodeInformationDto.lmiInactiveDate = dataTable.Rows[i].Field<DateTime?>("lmiInactiveDate");
				eRPIndirectLaborCodeInformationDto.lmiIndirectLaborID = dataTable.Rows[i].Field<string>("lmiIndirectLaborID");
				eRPIndirectLaborCodeInformationDto.lmiIndirectLaborType = dataTable.Rows[i].Field<byte>("lmiIndirectLaborType");
				eRPIndirectLaborCodeInformationDto.lmiInactive = dataTable.Rows[i].Field<bool>("lmiInactive");
				eRPIndirectLaborCodeInformationDto.lmiRowVersion = dataTable.Rows[i].Field<byte[]>("lmiRowVersion");
				eRPIndirectLaborCodeInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPIndirectLaborCodeInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPIndirectLaborCodeInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPIndirectLaborCodeInformationDto> GetIndirectLaborCode(Guid indirectLaborCodeId)
	{
		ERPIndirectLaborCodeInformationDto eRPIndirectLaborCodeInformationDto = new ERPIndirectLaborCodeInformationDto();
		InitializeParameterLists();
		string[] collection = new string[9] { "lmiCreatedBy", "lmiCreatedDate", "lmiDescription", "lmiUniqueID", "lmiInactiveDate", "lmiIndirectLaborID", "lmiIndirectLaborType", "lmiInactive", "lmiRowVersion" };
		base.selectList.AddRange(collection);
		base.filterList.Add("lmiUniqueID|C", indirectLaborCodeId);
		AddCustomFieldsToSelectList("IndirectLaborCodes");
		using (DataTable dataTable = GetAsDataTable("IndirectLaborCodes", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPIndirectLaborCodeInformationDto);
			}
			eRPIndirectLaborCodeInformationDto.lmiCreatedBy = dataTable.Rows[0].Field<string>("lmiCreatedBy");
			eRPIndirectLaborCodeInformationDto.lmiCreatedDate = dataTable.Rows[0].Field<DateTime?>("lmiCreatedDate");
			eRPIndirectLaborCodeInformationDto.lmiDescription = dataTable.Rows[0].Field<string>("lmiDescription");
			eRPIndirectLaborCodeInformationDto.lmiUniqueID = dataTable.Rows[0].Field<Guid>("lmiUniqueID");
			eRPIndirectLaborCodeInformationDto.lmiInactiveDate = dataTable.Rows[0].Field<DateTime?>("lmiInactiveDate");
			eRPIndirectLaborCodeInformationDto.lmiIndirectLaborID = dataTable.Rows[0].Field<string>("lmiIndirectLaborID");
			eRPIndirectLaborCodeInformationDto.lmiIndirectLaborType = dataTable.Rows[0].Field<byte>("lmiIndirectLaborType");
			eRPIndirectLaborCodeInformationDto.lmiInactive = dataTable.Rows[0].Field<bool>("lmiInactive");
			eRPIndirectLaborCodeInformationDto.lmiRowVersion = dataTable.Rows[0].Field<byte[]>("lmiRowVersion");
			eRPIndirectLaborCodeInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPIndirectLaborCodeInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPIndirectLaborCodeInformationDto);
	}

	public Task<APIValidationInfoDto> SaveIndirectLaborCode(ERPIndirectLaborCodeDto indirectLaborCode)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM IndirectLaborCodes WHERE lmiUniqueID = " + M1Util.ConvertToLinq(indirectLaborCode.lmiUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["lmiIndirectLaborID"] = indirectLaborCode.lmiIndirectLaborID.ToUpper();
				indirectLaborCode.lmiUniqueID = ((indirectLaborCode.lmiUniqueID == Guid.Empty) ? Guid.NewGuid() : indirectLaborCode.lmiUniqueID);
				dataRow["lmiUniqueID"] = indirectLaborCode.lmiUniqueID;
				dataRow["lmiCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["lmiCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The IndirectLaborCode could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (indirectLaborCode.lmiRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the IndirectLaborCode is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["lmiRowVersion"], indirectLaborCode.lmiRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the IndirectLaborCode has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the IndirectLaborCode again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["lmiDescription"] = indirectLaborCode.lmiDescription;
			DataRow dataRow2 = dataRow;
			DateTime? lmiInactiveDate = indirectLaborCode.lmiInactiveDate;
			dataRow2["lmiInactiveDate"] = (lmiInactiveDate.HasValue ? ((object)lmiInactiveDate.GetValueOrDefault()) : dataRow["lmiInactiveDate"]);
			dataRow["lmiIndirectLaborType"] = indirectLaborCode.lmiIndirectLaborType;
			dataRow["lmiInactive"] = indirectLaborCode.lmiInactive;
			if (indirectLaborCode.CustomFields != null && indirectLaborCode.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in indirectLaborCode.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the IndirectLaborCode [{indirectLaborCode.lmiUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the IndirectLaborCode [{indirectLaborCode.lmiUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
