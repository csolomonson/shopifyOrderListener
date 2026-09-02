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

public class ERPCountyCodeRepository : APIBaseRepository, IERPCountyCodeRepository, IAPIBaseRepository, IDisposable
{
	public ERPCountyCodeRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesCountyCodeExist(Guid countyCodeId)
	{
		InitializeParameterLists();
		base.filterList.Add("xccUniqueID|C", countyCodeId);
		base.selectList.Add("xccUniqueID");
		return Task.FromResult(GetAsObject("CountyCodes", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPCountyCodeInformationDto>> GetAllCountyCodes(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPCountyCodeInformationDto> collection = new List<ERPCountyCodeInformationDto>();
		InitializeParameterLists();
		string[] array = new string[8] { "xccCountyCodeID", "xccCounty", "xccCountyCode", "xccCreatedBy", "xccCreatedDate", "xccUniqueID", "XCCRowVersion", "xccStateCode" };
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("CountyCodes");
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
		using (DataTable dataTable = GetAsDataTable("CountyCodes", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPCountyCodeInformationDto eRPCountyCodeInformationDto = new ERPCountyCodeInformationDto();
				eRPCountyCodeInformationDto.xccCountyCodeID = dataTable.Rows[i].Field<string>("xccCountyCodeID");
				eRPCountyCodeInformationDto.xccCounty = dataTable.Rows[i].Field<string>("xccCounty");
				eRPCountyCodeInformationDto.xccCountyCode = dataTable.Rows[i].Field<string>("xccCountyCode");
				eRPCountyCodeInformationDto.xccCreatedBy = dataTable.Rows[i].Field<string>("xccCreatedBy");
				eRPCountyCodeInformationDto.xccCreatedDate = dataTable.Rows[i].Field<DateTime?>("xccCreatedDate");
				eRPCountyCodeInformationDto.xccUniqueID = dataTable.Rows[i].Field<Guid>("xccUniqueID");
				eRPCountyCodeInformationDto.XCCRowVersion = dataTable.Rows[i].Field<byte[]>("XCCRowVersion");
				eRPCountyCodeInformationDto.xccStateCode = dataTable.Rows[i].Field<string>("xccStateCode");
				eRPCountyCodeInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPCountyCodeInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPCountyCodeInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPCountyCodeInformationDto> GetCountyCode(Guid countyCodeId)
	{
		ERPCountyCodeInformationDto eRPCountyCodeInformationDto = new ERPCountyCodeInformationDto();
		InitializeParameterLists();
		string[] collection = new string[8] { "xccCountyCodeID", "xccCounty", "xccCountyCode", "xccCreatedBy", "xccCreatedDate", "xccUniqueID", "XCCRowVersion", "xccStateCode" };
		base.selectList.AddRange(collection);
		base.filterList.Add("xccUniqueID|C", countyCodeId);
		AddCustomFieldsToSelectList("CountyCodes");
		using (DataTable dataTable = GetAsDataTable("CountyCodes", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPCountyCodeInformationDto);
			}
			eRPCountyCodeInformationDto.xccCountyCodeID = dataTable.Rows[0].Field<string>("xccCountyCodeID");
			eRPCountyCodeInformationDto.xccCounty = dataTable.Rows[0].Field<string>("xccCounty");
			eRPCountyCodeInformationDto.xccCountyCode = dataTable.Rows[0].Field<string>("xccCountyCode");
			eRPCountyCodeInformationDto.xccCreatedBy = dataTable.Rows[0].Field<string>("xccCreatedBy");
			eRPCountyCodeInformationDto.xccCreatedDate = dataTable.Rows[0].Field<DateTime?>("xccCreatedDate");
			eRPCountyCodeInformationDto.xccUniqueID = dataTable.Rows[0].Field<Guid>("xccUniqueID");
			eRPCountyCodeInformationDto.XCCRowVersion = dataTable.Rows[0].Field<byte[]>("XCCRowVersion");
			eRPCountyCodeInformationDto.xccStateCode = dataTable.Rows[0].Field<string>("xccStateCode");
			eRPCountyCodeInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPCountyCodeInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPCountyCodeInformationDto);
	}

	public Task<APIValidationInfoDto> SaveCountyCode(ERPCountyCodeDto countyCode)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM CountyCodes WHERE xccUniqueID = " + M1Util.ConvertToLinq(countyCode.xccUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["xccCountyCodeID"] = countyCode.xccCountyCodeID.ToUpper();
				countyCode.xccUniqueID = ((countyCode.xccUniqueID == Guid.Empty) ? Guid.NewGuid() : countyCode.xccUniqueID);
				dataRow["xccUniqueID"] = countyCode.xccUniqueID;
				dataRow["xccCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["xccCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The CountyCode could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (countyCode.xccRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the CountyCode is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["xccRowVersion"], countyCode.xccRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the CountyCode has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the CountyCode again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["xccCounty"] = countyCode.xccCounty;
			dataRow["xccCountyCode"] = countyCode.xccCountyCode;
			dataRow["xccStateCode"] = countyCode.xccStateCode;
			if (countyCode.CustomFields != null && countyCode.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in countyCode.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the CountyCode [{countyCode.xccUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the CountyCode [{countyCode.xccUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
