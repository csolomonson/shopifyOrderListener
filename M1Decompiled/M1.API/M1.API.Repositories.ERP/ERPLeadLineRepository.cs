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

public class ERPLeadLineRepository : APIBaseRepository, IERPLeadLineRepository, IAPIBaseRepository, IDisposable
{
	public ERPLeadLineRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesLeadLineExist(Guid leadLineId)
	{
		InitializeParameterLists();
		base.filterList.Add("lolUniqueID|C", leadLineId);
		base.selectList.Add("lolUniqueID");
		return Task.FromResult(GetAsObject("LeadLines", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPLeadLineInformationDto>> GetAllLeadLines(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPLeadLineInformationDto> collection = new List<ERPLeadLineInformationDto>();
		InitializeParameterLists();
		string[] array = new string[32]
		{
			"lolCreatedBy", "lolCreatedDate", "lolCurrencyRateID", "lolDescription", "lolDiscountAmount", "lolDiscountAmountForeign", "lolDiscountPercent", "lolUniqueID", "lolExchangeRate", "lolForecastDate",
			"lolGrossAmount", "lolGrossAmountForeign", "lolCreatedFromMobile", "lolCustomRate", "lolTransferredToQuote", "lolLeadDate", "lolLeadID", "lolOrgPartID", "lolOrgPartShortDescription", "lolPartGroupID",
			"lolPartID", "lolPartPriceID", "lolPartRevisionID", "lolQuantity", "lolResolutionReasonID", "lolRevenueForecast", "lolRevenueForecastForeign", "lolRowVersion", "lolLeadLineID", "lolUnitOfMeasure",
			"lolUnitSalePriceBase", "lolUnitSalePriceForeign"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("LeadLines");
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
		using (DataTable dataTable = GetAsDataTable("LeadLines", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPLeadLineInformationDto eRPLeadLineInformationDto = new ERPLeadLineInformationDto();
				eRPLeadLineInformationDto.lolCreatedBy = dataTable.Rows[i].Field<string>("lolCreatedBy");
				eRPLeadLineInformationDto.lolCreatedDate = dataTable.Rows[i].Field<DateTime?>("lolCreatedDate");
				eRPLeadLineInformationDto.lolCurrencyRateID = dataTable.Rows[i].Field<string>("lolCurrencyRateID");
				eRPLeadLineInformationDto.lolDescription = dataTable.Rows[i].Field<string>("lolDescription");
				eRPLeadLineInformationDto.lolDiscountAmount = dataTable.Rows[i].Field<decimal>("lolDiscountAmount");
				eRPLeadLineInformationDto.lolDiscountAmountForeign = dataTable.Rows[i].Field<decimal>("lolDiscountAmountForeign");
				eRPLeadLineInformationDto.lolDiscountPercent = dataTable.Rows[i].Field<decimal>("lolDiscountPercent");
				eRPLeadLineInformationDto.lolUniqueID = dataTable.Rows[i].Field<Guid>("lolUniqueID");
				eRPLeadLineInformationDto.lolExchangeRate = dataTable.Rows[i].Field<decimal>("lolExchangeRate");
				eRPLeadLineInformationDto.lolForecastDate = dataTable.Rows[i].Field<DateTime?>("lolForecastDate");
				eRPLeadLineInformationDto.lolGrossAmount = dataTable.Rows[i].Field<decimal>("lolGrossAmount");
				eRPLeadLineInformationDto.lolGrossAmountForeign = dataTable.Rows[i].Field<decimal>("lolGrossAmountForeign");
				eRPLeadLineInformationDto.lolCreatedFromMobile = dataTable.Rows[i].Field<bool>("lolCreatedFromMobile");
				eRPLeadLineInformationDto.lolCustomRate = dataTable.Rows[i].Field<bool>("lolCustomRate");
				eRPLeadLineInformationDto.lolTransferredToQuote = dataTable.Rows[i].Field<bool>("lolTransferredToQuote");
				eRPLeadLineInformationDto.lolLeadDate = dataTable.Rows[i].Field<DateTime?>("lolLeadDate");
				eRPLeadLineInformationDto.lolLeadID = dataTable.Rows[i].Field<string>("lolLeadID");
				eRPLeadLineInformationDto.lolOrgPartID = dataTable.Rows[i].Field<string>("lolOrgPartID");
				eRPLeadLineInformationDto.lolOrgPartShortDescription = dataTable.Rows[i].Field<string>("lolOrgPartShortDescription");
				eRPLeadLineInformationDto.lolPartGroupID = dataTable.Rows[i].Field<string>("lolPartGroupID");
				eRPLeadLineInformationDto.lolPartID = dataTable.Rows[i].Field<string>("lolPartID");
				eRPLeadLineInformationDto.lolPartPriceID = dataTable.Rows[i].Field<int>("lolPartPriceID");
				eRPLeadLineInformationDto.lolPartRevisionID = dataTable.Rows[i].Field<string>("lolPartRevisionID");
				eRPLeadLineInformationDto.lolQuantity = dataTable.Rows[i].Field<decimal>("lolQuantity");
				eRPLeadLineInformationDto.lolResolutionReasonID = dataTable.Rows[i].Field<string>("lolResolutionReasonID");
				eRPLeadLineInformationDto.lolRevenueForecast = dataTable.Rows[i].Field<decimal>("lolRevenueForecast");
				eRPLeadLineInformationDto.lolRevenueForecastForeign = dataTable.Rows[i].Field<decimal>("lolRevenueForecastForeign");
				eRPLeadLineInformationDto.lolRowVersion = dataTable.Rows[i].Field<byte[]>("lolRowVersion");
				eRPLeadLineInformationDto.lolLeadLineID = dataTable.Rows[i].Field<short>("lolLeadLineID");
				eRPLeadLineInformationDto.lolUnitOfMeasure = dataTable.Rows[i].Field<string>("lolUnitOfMeasure");
				eRPLeadLineInformationDto.lolUnitSalePriceBase = dataTable.Rows[i].Field<decimal>("lolUnitSalePriceBase");
				eRPLeadLineInformationDto.lolUnitSalePriceForeign = dataTable.Rows[i].Field<decimal>("lolUnitSalePriceForeign");
				eRPLeadLineInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPLeadLineInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPLeadLineInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPLeadLineInformationDto> GetLeadLine(Guid leadLineId)
	{
		ERPLeadLineInformationDto eRPLeadLineInformationDto = new ERPLeadLineInformationDto();
		InitializeParameterLists();
		string[] collection = new string[32]
		{
			"lolCreatedBy", "lolCreatedDate", "lolCurrencyRateID", "lolDescription", "lolDiscountAmount", "lolDiscountAmountForeign", "lolDiscountPercent", "lolUniqueID", "lolExchangeRate", "lolForecastDate",
			"lolGrossAmount", "lolGrossAmountForeign", "lolCreatedFromMobile", "lolCustomRate", "lolTransferredToQuote", "lolLeadDate", "lolLeadID", "lolOrgPartID", "lolOrgPartShortDescription", "lolPartGroupID",
			"lolPartID", "lolPartPriceID", "lolPartRevisionID", "lolQuantity", "lolResolutionReasonID", "lolRevenueForecast", "lolRevenueForecastForeign", "lolRowVersion", "lolLeadLineID", "lolUnitOfMeasure",
			"lolUnitSalePriceBase", "lolUnitSalePriceForeign"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("lolUniqueID|C", leadLineId);
		AddCustomFieldsToSelectList("LeadLines");
		using (DataTable dataTable = GetAsDataTable("LeadLines", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPLeadLineInformationDto);
			}
			eRPLeadLineInformationDto.lolCreatedBy = dataTable.Rows[0].Field<string>("lolCreatedBy");
			eRPLeadLineInformationDto.lolCreatedDate = dataTable.Rows[0].Field<DateTime?>("lolCreatedDate");
			eRPLeadLineInformationDto.lolCurrencyRateID = dataTable.Rows[0].Field<string>("lolCurrencyRateID");
			eRPLeadLineInformationDto.lolDescription = dataTable.Rows[0].Field<string>("lolDescription");
			eRPLeadLineInformationDto.lolDiscountAmount = dataTable.Rows[0].Field<decimal>("lolDiscountAmount");
			eRPLeadLineInformationDto.lolDiscountAmountForeign = dataTable.Rows[0].Field<decimal>("lolDiscountAmountForeign");
			eRPLeadLineInformationDto.lolDiscountPercent = dataTable.Rows[0].Field<decimal>("lolDiscountPercent");
			eRPLeadLineInformationDto.lolUniqueID = dataTable.Rows[0].Field<Guid>("lolUniqueID");
			eRPLeadLineInformationDto.lolExchangeRate = dataTable.Rows[0].Field<decimal>("lolExchangeRate");
			eRPLeadLineInformationDto.lolForecastDate = dataTable.Rows[0].Field<DateTime?>("lolForecastDate");
			eRPLeadLineInformationDto.lolGrossAmount = dataTable.Rows[0].Field<decimal>("lolGrossAmount");
			eRPLeadLineInformationDto.lolGrossAmountForeign = dataTable.Rows[0].Field<decimal>("lolGrossAmountForeign");
			eRPLeadLineInformationDto.lolCreatedFromMobile = dataTable.Rows[0].Field<bool>("lolCreatedFromMobile");
			eRPLeadLineInformationDto.lolCustomRate = dataTable.Rows[0].Field<bool>("lolCustomRate");
			eRPLeadLineInformationDto.lolTransferredToQuote = dataTable.Rows[0].Field<bool>("lolTransferredToQuote");
			eRPLeadLineInformationDto.lolLeadDate = dataTable.Rows[0].Field<DateTime?>("lolLeadDate");
			eRPLeadLineInformationDto.lolLeadID = dataTable.Rows[0].Field<string>("lolLeadID");
			eRPLeadLineInformationDto.lolOrgPartID = dataTable.Rows[0].Field<string>("lolOrgPartID");
			eRPLeadLineInformationDto.lolOrgPartShortDescription = dataTable.Rows[0].Field<string>("lolOrgPartShortDescription");
			eRPLeadLineInformationDto.lolPartGroupID = dataTable.Rows[0].Field<string>("lolPartGroupID");
			eRPLeadLineInformationDto.lolPartID = dataTable.Rows[0].Field<string>("lolPartID");
			eRPLeadLineInformationDto.lolPartPriceID = dataTable.Rows[0].Field<int>("lolPartPriceID");
			eRPLeadLineInformationDto.lolPartRevisionID = dataTable.Rows[0].Field<string>("lolPartRevisionID");
			eRPLeadLineInformationDto.lolQuantity = dataTable.Rows[0].Field<decimal>("lolQuantity");
			eRPLeadLineInformationDto.lolResolutionReasonID = dataTable.Rows[0].Field<string>("lolResolutionReasonID");
			eRPLeadLineInformationDto.lolRevenueForecast = dataTable.Rows[0].Field<decimal>("lolRevenueForecast");
			eRPLeadLineInformationDto.lolRevenueForecastForeign = dataTable.Rows[0].Field<decimal>("lolRevenueForecastForeign");
			eRPLeadLineInformationDto.lolRowVersion = dataTable.Rows[0].Field<byte[]>("lolRowVersion");
			eRPLeadLineInformationDto.lolLeadLineID = dataTable.Rows[0].Field<short>("lolLeadLineID");
			eRPLeadLineInformationDto.lolUnitOfMeasure = dataTable.Rows[0].Field<string>("lolUnitOfMeasure");
			eRPLeadLineInformationDto.lolUnitSalePriceBase = dataTable.Rows[0].Field<decimal>("lolUnitSalePriceBase");
			eRPLeadLineInformationDto.lolUnitSalePriceForeign = dataTable.Rows[0].Field<decimal>("lolUnitSalePriceForeign");
			eRPLeadLineInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPLeadLineInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPLeadLineInformationDto);
	}

	public Task<APIValidationInfoDto> SaveLeadLine(ERPLeadLineDto leadLine)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM LeadLines WHERE lolUniqueID = " + M1Util.ConvertToLinq(leadLine.lolUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["lolLeadID"] = leadLine.lolLeadID.ToUpper();
				dataRow["lolLeadLineID"] = leadLine.lolLeadLineID;
				leadLine.lolUniqueID = ((leadLine.lolUniqueID == Guid.Empty) ? Guid.NewGuid() : leadLine.lolUniqueID);
				dataRow["lolUniqueID"] = leadLine.lolUniqueID;
				dataRow["lolCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["lolCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The LeadLine could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (leadLine.lolRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the LeadLine is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["lolRowVersion"], leadLine.lolRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the LeadLine has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the LeadLine again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["lolCurrencyRateID"] = leadLine.lolCurrencyRateID;
			dataRow["lolDescription"] = leadLine.lolDescription;
			dataRow["lolDiscountAmount"] = leadLine.lolDiscountAmount;
			dataRow["lolDiscountAmountForeign"] = leadLine.lolDiscountAmountForeign;
			dataRow["lolDiscountPercent"] = leadLine.lolDiscountPercent;
			dataRow["lolExchangeRate"] = leadLine.lolExchangeRate;
			DataRow dataRow2 = dataRow;
			DateTime? lolForecastDate = leadLine.lolForecastDate;
			dataRow2["lolForecastDate"] = (lolForecastDate.HasValue ? ((object)lolForecastDate.GetValueOrDefault()) : dataRow["lolForecastDate"]);
			dataRow["lolGrossAmount"] = leadLine.lolGrossAmount;
			dataRow["lolGrossAmountForeign"] = leadLine.lolGrossAmountForeign;
			dataRow["lolCreatedFromMobile"] = leadLine.lolCreatedFromMobile;
			dataRow["lolCustomRate"] = leadLine.lolCustomRate;
			dataRow["lolTransferredToQuote"] = leadLine.lolTransferredToQuote;
			DataRow dataRow3 = dataRow;
			lolForecastDate = leadLine.lolLeadDate;
			dataRow3["lolLeadDate"] = (lolForecastDate.HasValue ? ((object)lolForecastDate.GetValueOrDefault()) : dataRow["lolLeadDate"]);
			dataRow["lolOrgPartID"] = leadLine.lolOrgPartID;
			dataRow["lolOrgPartShortDescription"] = leadLine.lolOrgPartShortDescription;
			dataRow["lolPartGroupID"] = leadLine.lolPartGroupID;
			dataRow["lolPartID"] = leadLine.lolPartID;
			dataRow["lolPartPriceID"] = leadLine.lolPartPriceID;
			dataRow["lolPartRevisionID"] = leadLine.lolPartRevisionID;
			dataRow["lolQuantity"] = leadLine.lolQuantity;
			dataRow["lolResolutionReasonID"] = leadLine.lolResolutionReasonID;
			dataRow["lolRevenueForecast"] = leadLine.lolRevenueForecast;
			dataRow["lolRevenueForecastForeign"] = leadLine.lolRevenueForecastForeign;
			dataRow["lolUnitOfMeasure"] = leadLine.lolUnitOfMeasure;
			dataRow["lolUnitSalePriceBase"] = leadLine.lolUnitSalePriceBase;
			dataRow["lolUnitSalePriceForeign"] = leadLine.lolUnitSalePriceForeign;
			if (leadLine.CustomFields != null && leadLine.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in leadLine.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the LeadLine [{leadLine.lolUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the LeadLine [{leadLine.lolUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
