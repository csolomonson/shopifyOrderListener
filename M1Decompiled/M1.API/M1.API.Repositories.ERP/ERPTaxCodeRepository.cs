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

public class ERPTaxCodeRepository : APIBaseRepository, IERPTaxCodeRepository, IAPIBaseRepository, IDisposable
{
	public ERPTaxCodeRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesTaxCodeExist(Guid taxCodeId)
	{
		InitializeParameterLists();
		base.filterList.Add("xaxUniqueID|C", taxCodeId);
		base.selectList.Add("xaxUniqueID");
		return Task.FromResult(GetAsObject("TaxCodes", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPTaxCodeInformationDto>> GetAllTaxCodes(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPTaxCodeInformationDto> collection = new List<ERPTaxCodeInformationDto>();
		InitializeParameterLists();
		string[] array = new string[12]
		{
			"xaxAccrualGlAccountID", "xaxTaxCodeID", "xaxCreatedBy", "xaxCreatedDate", "xaxDescription", "xaxUniqueID", "xaxInactiveDate", "xaxInactive", "xaxIncludePrimaryTax", "xaxRowVersion",
			"xaxTaxOption", "xaxTaxType"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("TaxCodes");
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
		using (DataTable dataTable = GetAsDataTable("TaxCodes", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPTaxCodeInformationDto eRPTaxCodeInformationDto = new ERPTaxCodeInformationDto();
				eRPTaxCodeInformationDto.xaxAccrualGlAccountID = dataTable.Rows[i].Field<string>("xaxAccrualGlAccountID");
				eRPTaxCodeInformationDto.xaxTaxCodeID = dataTable.Rows[i].Field<string>("xaxTaxCodeID");
				eRPTaxCodeInformationDto.xaxCreatedBy = dataTable.Rows[i].Field<string>("xaxCreatedBy");
				eRPTaxCodeInformationDto.xaxCreatedDate = dataTable.Rows[i].Field<DateTime?>("xaxCreatedDate");
				eRPTaxCodeInformationDto.xaxDescription = dataTable.Rows[i].Field<string>("xaxDescription");
				eRPTaxCodeInformationDto.xaxUniqueID = dataTable.Rows[i].Field<Guid>("xaxUniqueID");
				eRPTaxCodeInformationDto.xaxInactiveDate = dataTable.Rows[i].Field<DateTime?>("xaxInactiveDate");
				eRPTaxCodeInformationDto.xaxInactive = dataTable.Rows[i].Field<bool>("xaxInactive");
				eRPTaxCodeInformationDto.xaxIncludePrimaryTax = dataTable.Rows[i].Field<bool>("xaxIncludePrimaryTax");
				eRPTaxCodeInformationDto.xaxRowVersion = dataTable.Rows[i].Field<byte[]>("xaxRowVersion");
				eRPTaxCodeInformationDto.xaxTaxOption = dataTable.Rows[i].Field<string>("xaxTaxOption");
				eRPTaxCodeInformationDto.xaxTaxType = dataTable.Rows[i].Field<byte>("xaxTaxType");
				eRPTaxCodeInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPTaxCodeInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPTaxCodeInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPTaxCodeInformationDto> GetTaxCode(Guid taxCodeId)
	{
		ERPTaxCodeInformationDto eRPTaxCodeInformationDto = new ERPTaxCodeInformationDto();
		InitializeParameterLists();
		string[] collection = new string[12]
		{
			"xaxAccrualGlAccountID", "xaxTaxCodeID", "xaxCreatedBy", "xaxCreatedDate", "xaxDescription", "xaxUniqueID", "xaxInactiveDate", "xaxInactive", "xaxIncludePrimaryTax", "xaxRowVersion",
			"xaxTaxOption", "xaxTaxType"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("xaxUniqueID|C", taxCodeId);
		AddCustomFieldsToSelectList("TaxCodes");
		using (DataTable dataTable = GetAsDataTable("TaxCodes", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPTaxCodeInformationDto);
			}
			eRPTaxCodeInformationDto.xaxAccrualGlAccountID = dataTable.Rows[0].Field<string>("xaxAccrualGlAccountID");
			eRPTaxCodeInformationDto.xaxTaxCodeID = dataTable.Rows[0].Field<string>("xaxTaxCodeID");
			eRPTaxCodeInformationDto.xaxCreatedBy = dataTable.Rows[0].Field<string>("xaxCreatedBy");
			eRPTaxCodeInformationDto.xaxCreatedDate = dataTable.Rows[0].Field<DateTime?>("xaxCreatedDate");
			eRPTaxCodeInformationDto.xaxDescription = dataTable.Rows[0].Field<string>("xaxDescription");
			eRPTaxCodeInformationDto.xaxUniqueID = dataTable.Rows[0].Field<Guid>("xaxUniqueID");
			eRPTaxCodeInformationDto.xaxInactiveDate = dataTable.Rows[0].Field<DateTime?>("xaxInactiveDate");
			eRPTaxCodeInformationDto.xaxInactive = dataTable.Rows[0].Field<bool>("xaxInactive");
			eRPTaxCodeInformationDto.xaxIncludePrimaryTax = dataTable.Rows[0].Field<bool>("xaxIncludePrimaryTax");
			eRPTaxCodeInformationDto.xaxRowVersion = dataTable.Rows[0].Field<byte[]>("xaxRowVersion");
			eRPTaxCodeInformationDto.xaxTaxOption = dataTable.Rows[0].Field<string>("xaxTaxOption");
			eRPTaxCodeInformationDto.xaxTaxType = dataTable.Rows[0].Field<byte>("xaxTaxType");
			eRPTaxCodeInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPTaxCodeInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPTaxCodeInformationDto);
	}

	public Task<APIValidationInfoDto> SaveTaxCode(ERPTaxCodeDto taxCode)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM TaxCodes WHERE xaxUniqueID = " + M1Util.ConvertToLinq(taxCode.xaxUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["xaxTaxCodeID"] = taxCode.xaxTaxCodeID.ToUpper();
				taxCode.xaxUniqueID = ((taxCode.xaxUniqueID == Guid.Empty) ? Guid.NewGuid() : taxCode.xaxUniqueID);
				dataRow["xaxUniqueID"] = taxCode.xaxUniqueID;
				dataRow["xaxCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["xaxCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The TaxCode could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (taxCode.xaxRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the TaxCode is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["xaxRowVersion"], taxCode.xaxRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the TaxCode has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the TaxCode again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["xaxAccrualGlAccountID"] = taxCode.xaxAccrualGlAccountID;
			dataRow["xaxDescription"] = taxCode.xaxDescription;
			DataRow dataRow2 = dataRow;
			DateTime? xaxInactiveDate = taxCode.xaxInactiveDate;
			dataRow2["xaxInactiveDate"] = (xaxInactiveDate.HasValue ? ((object)xaxInactiveDate.GetValueOrDefault()) : dataRow["xaxInactiveDate"]);
			dataRow["xaxInactive"] = taxCode.xaxInactive;
			dataRow["xaxIncludePrimaryTax"] = taxCode.xaxIncludePrimaryTax;
			dataRow["xaxTaxOption"] = taxCode.xaxTaxOption;
			dataRow["xaxTaxType"] = taxCode.xaxTaxType;
			if (taxCode.CustomFields != null && taxCode.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in taxCode.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the TaxCode [{taxCode.xaxUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the TaxCode [{taxCode.xaxUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
