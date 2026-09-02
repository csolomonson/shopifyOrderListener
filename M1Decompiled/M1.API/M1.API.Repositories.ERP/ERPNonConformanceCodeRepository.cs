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

public class ERPNonConformanceCodeRepository : APIBaseRepository, IERPNonConformanceCodeRepository, IAPIBaseRepository, IDisposable
{
	public ERPNonConformanceCodeRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesNonConformanceCodeExist(Guid nonConformanceCodeId)
	{
		InitializeParameterLists();
		base.filterList.Add("qacUniqueID|C", nonConformanceCodeId);
		base.selectList.Add("qacUniqueID");
		return Task.FromResult(GetAsObject("NonConformanceCodes", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPNonConformanceCodeInformationDto>> GetAllNonConformanceCodes(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPNonConformanceCodeInformationDto> collection = new List<ERPNonConformanceCodeInformationDto>();
		InitializeParameterLists();
		string[] array = new string[7] { "qacNonConformanceCodeID", "qacCreatedBy", "qacCreatedDate", "qacDescription", "qacUniqueID", "qacNonConformanceCategoryID", "qacRowVersion" };
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("NonConformanceCodes");
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
		using (DataTable dataTable = GetAsDataTable("NonConformanceCodes", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPNonConformanceCodeInformationDto eRPNonConformanceCodeInformationDto = new ERPNonConformanceCodeInformationDto();
				eRPNonConformanceCodeInformationDto.qacNonConformanceCodeID = dataTable.Rows[i].Field<string>("qacNonConformanceCodeID");
				eRPNonConformanceCodeInformationDto.qacCreatedBy = dataTable.Rows[i].Field<string>("qacCreatedBy");
				eRPNonConformanceCodeInformationDto.qacCreatedDate = dataTable.Rows[i].Field<DateTime?>("qacCreatedDate");
				eRPNonConformanceCodeInformationDto.qacDescription = dataTable.Rows[i].Field<string>("qacDescription");
				eRPNonConformanceCodeInformationDto.qacUniqueID = dataTable.Rows[i].Field<Guid>("qacUniqueID");
				eRPNonConformanceCodeInformationDto.qacNonConformanceCategoryID = dataTable.Rows[i].Field<string>("qacNonConformanceCategoryID");
				eRPNonConformanceCodeInformationDto.qacRowVersion = dataTable.Rows[i].Field<byte[]>("qacRowVersion");
				eRPNonConformanceCodeInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPNonConformanceCodeInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPNonConformanceCodeInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPNonConformanceCodeInformationDto> GetNonConformanceCode(Guid nonConformanceCodeId)
	{
		ERPNonConformanceCodeInformationDto eRPNonConformanceCodeInformationDto = new ERPNonConformanceCodeInformationDto();
		InitializeParameterLists();
		string[] collection = new string[7] { "qacNonConformanceCodeID", "qacCreatedBy", "qacCreatedDate", "qacDescription", "qacUniqueID", "qacNonConformanceCategoryID", "qacRowVersion" };
		base.selectList.AddRange(collection);
		base.filterList.Add("qacUniqueID|C", nonConformanceCodeId);
		AddCustomFieldsToSelectList("NonConformanceCodes");
		using (DataTable dataTable = GetAsDataTable("NonConformanceCodes", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPNonConformanceCodeInformationDto);
			}
			eRPNonConformanceCodeInformationDto.qacNonConformanceCodeID = dataTable.Rows[0].Field<string>("qacNonConformanceCodeID");
			eRPNonConformanceCodeInformationDto.qacCreatedBy = dataTable.Rows[0].Field<string>("qacCreatedBy");
			eRPNonConformanceCodeInformationDto.qacCreatedDate = dataTable.Rows[0].Field<DateTime?>("qacCreatedDate");
			eRPNonConformanceCodeInformationDto.qacDescription = dataTable.Rows[0].Field<string>("qacDescription");
			eRPNonConformanceCodeInformationDto.qacUniqueID = dataTable.Rows[0].Field<Guid>("qacUniqueID");
			eRPNonConformanceCodeInformationDto.qacNonConformanceCategoryID = dataTable.Rows[0].Field<string>("qacNonConformanceCategoryID");
			eRPNonConformanceCodeInformationDto.qacRowVersion = dataTable.Rows[0].Field<byte[]>("qacRowVersion");
			eRPNonConformanceCodeInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPNonConformanceCodeInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPNonConformanceCodeInformationDto);
	}

	public Task<APIValidationInfoDto> SaveNonConformanceCode(ERPNonConformanceCodeDto nonConformanceCode)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM NonConformanceCodes WHERE qacUniqueID = " + M1Util.ConvertToLinq(nonConformanceCode.qacUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["qacNonConformanceCodeID"] = nonConformanceCode.qacNonConformanceCodeID.ToUpper();
				nonConformanceCode.qacUniqueID = ((nonConformanceCode.qacUniqueID == Guid.Empty) ? Guid.NewGuid() : nonConformanceCode.qacUniqueID);
				dataRow["qacUniqueID"] = nonConformanceCode.qacUniqueID;
				dataRow["qacCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["qacCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The NonConformanceCode could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (nonConformanceCode.qacRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the NonConformanceCode is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["qacRowVersion"], nonConformanceCode.qacRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the NonConformanceCode has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the NonConformanceCode again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["qacDescription"] = nonConformanceCode.qacDescription;
			dataRow["qacNonConformanceCategoryID"] = nonConformanceCode.qacNonConformanceCategoryID;
			if (nonConformanceCode.CustomFields != null && nonConformanceCode.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in nonConformanceCode.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the NonConformanceCode [{nonConformanceCode.qacUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the NonConformanceCode [{nonConformanceCode.qacUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
