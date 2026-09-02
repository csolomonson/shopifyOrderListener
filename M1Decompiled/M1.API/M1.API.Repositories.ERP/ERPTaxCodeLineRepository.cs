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

public class ERPTaxCodeLineRepository : APIBaseRepository, IERPTaxCodeLineRepository, IAPIBaseRepository, IDisposable
{
	public ERPTaxCodeLineRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesTaxCodeLineExist(Guid taxCodeLineId)
	{
		InitializeParameterLists();
		base.filterList.Add("xabUniqueID|C", taxCodeLineId);
		base.selectList.Add("xabUniqueID");
		return Task.FromResult(GetAsObject("TaxCodeLines", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPTaxCodeLineInformationDto>> GetAllTaxCodeLines(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPTaxCodeLineInformationDto> collection = new List<ERPTaxCodeLineInformationDto>();
		InitializeParameterLists();
		string[] array = new string[10] { "xabCreatedBy", "xabCreatedDate", "xabEffectiveDate", "xabUniqueID", "xabRowVersion", "xabTaxCodeLineID", "xabTaxCodeID", "xabTaxRate", "xabTaxRateNotesRTF", "xabTaxRateNotesText" };
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("TaxCodeLines");
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
		using (DataTable dataTable = GetAsDataTable("TaxCodeLines", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPTaxCodeLineInformationDto eRPTaxCodeLineInformationDto = new ERPTaxCodeLineInformationDto();
				eRPTaxCodeLineInformationDto.xabCreatedBy = dataTable.Rows[i].Field<string>("xabCreatedBy");
				eRPTaxCodeLineInformationDto.xabCreatedDate = dataTable.Rows[i].Field<DateTime?>("xabCreatedDate");
				eRPTaxCodeLineInformationDto.xabEffectiveDate = dataTable.Rows[i].Field<DateTime?>("xabEffectiveDate");
				eRPTaxCodeLineInformationDto.xabUniqueID = dataTable.Rows[i].Field<Guid>("xabUniqueID");
				eRPTaxCodeLineInformationDto.xabRowVersion = dataTable.Rows[i].Field<byte[]>("xabRowVersion");
				eRPTaxCodeLineInformationDto.xabTaxCodeLineID = dataTable.Rows[i].Field<int>("xabTaxCodeLineID");
				eRPTaxCodeLineInformationDto.xabTaxCodeID = dataTable.Rows[i].Field<string>("xabTaxCodeID");
				eRPTaxCodeLineInformationDto.xabTaxRate = dataTable.Rows[i].Field<decimal>("xabTaxRate");
				eRPTaxCodeLineInformationDto.xabTaxRateNotesRTF = dataTable.Rows[i].Field<string>("xabTaxRateNotesRTF");
				eRPTaxCodeLineInformationDto.xabTaxRateNotesText = dataTable.Rows[i].Field<string>("xabTaxRateNotesText");
				eRPTaxCodeLineInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPTaxCodeLineInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPTaxCodeLineInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPTaxCodeLineInformationDto> GetTaxCodeLine(Guid taxCodeLineId)
	{
		ERPTaxCodeLineInformationDto eRPTaxCodeLineInformationDto = new ERPTaxCodeLineInformationDto();
		InitializeParameterLists();
		string[] collection = new string[10] { "xabCreatedBy", "xabCreatedDate", "xabEffectiveDate", "xabUniqueID", "xabRowVersion", "xabTaxCodeLineID", "xabTaxCodeID", "xabTaxRate", "xabTaxRateNotesRTF", "xabTaxRateNotesText" };
		base.selectList.AddRange(collection);
		base.filterList.Add("xabUniqueID|C", taxCodeLineId);
		AddCustomFieldsToSelectList("TaxCodeLines");
		using (DataTable dataTable = GetAsDataTable("TaxCodeLines", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPTaxCodeLineInformationDto);
			}
			eRPTaxCodeLineInformationDto.xabCreatedBy = dataTable.Rows[0].Field<string>("xabCreatedBy");
			eRPTaxCodeLineInformationDto.xabCreatedDate = dataTable.Rows[0].Field<DateTime?>("xabCreatedDate");
			eRPTaxCodeLineInformationDto.xabEffectiveDate = dataTable.Rows[0].Field<DateTime?>("xabEffectiveDate");
			eRPTaxCodeLineInformationDto.xabUniqueID = dataTable.Rows[0].Field<Guid>("xabUniqueID");
			eRPTaxCodeLineInformationDto.xabRowVersion = dataTable.Rows[0].Field<byte[]>("xabRowVersion");
			eRPTaxCodeLineInformationDto.xabTaxCodeLineID = dataTable.Rows[0].Field<int>("xabTaxCodeLineID");
			eRPTaxCodeLineInformationDto.xabTaxCodeID = dataTable.Rows[0].Field<string>("xabTaxCodeID");
			eRPTaxCodeLineInformationDto.xabTaxRate = dataTable.Rows[0].Field<decimal>("xabTaxRate");
			eRPTaxCodeLineInformationDto.xabTaxRateNotesRTF = dataTable.Rows[0].Field<string>("xabTaxRateNotesRTF");
			eRPTaxCodeLineInformationDto.xabTaxRateNotesText = dataTable.Rows[0].Field<string>("xabTaxRateNotesText");
			eRPTaxCodeLineInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPTaxCodeLineInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPTaxCodeLineInformationDto);
	}

	public Task<APIValidationInfoDto> SaveTaxCodeLine(ERPTaxCodeLineDto taxCodeLine)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM TaxCodeLines WHERE xabUniqueID = " + M1Util.ConvertToLinq(taxCodeLine.xabUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["xabTaxCodeID"] = taxCodeLine.xabTaxCodeID.ToUpper();
				dataRow["xabTaxCodeLineID"] = taxCodeLine.xabTaxCodeLineID;
				taxCodeLine.xabUniqueID = ((taxCodeLine.xabUniqueID == Guid.Empty) ? Guid.NewGuid() : taxCodeLine.xabUniqueID);
				dataRow["xabUniqueID"] = taxCodeLine.xabUniqueID;
				dataRow["xabCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["xabCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The TaxCodeLine could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (taxCodeLine.xabRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the TaxCodeLine is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["xabRowVersion"], taxCodeLine.xabRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the TaxCodeLine has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the TaxCodeLine again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			DataRow dataRow2 = dataRow;
			DateTime? xabEffectiveDate = taxCodeLine.xabEffectiveDate;
			dataRow2["xabEffectiveDate"] = (xabEffectiveDate.HasValue ? ((object)xabEffectiveDate.GetValueOrDefault()) : dataRow["xabEffectiveDate"]);
			dataRow["xabTaxRate"] = taxCodeLine.xabTaxRate;
			dataRow["xabTaxRateNotesRTF"] = taxCodeLine.xabTaxRateNotesRTF ?? dataRow["xabTaxRateNotesRTF"];
			dataRow["xabTaxRateNotesText"] = taxCodeLine.xabTaxRateNotesText ?? dataRow["xabTaxRateNotesText"];
			if (taxCodeLine.CustomFields != null && taxCodeLine.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in taxCodeLine.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the TaxCodeLine [{taxCodeLine.xabUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the TaxCodeLine [{taxCodeLine.xabUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
