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

public class ERPSalesOrderLineRepository : APIBaseRepository, IERPSalesOrderLineRepository, IAPIBaseRepository, IDisposable
{
	public ERPSalesOrderLineRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesSalesOrderLineExist(Guid salesOrderLineId)
	{
		InitializeParameterLists();
		base.filterList.Add("omlUniqueID|C", salesOrderLineId);
		base.selectList.Add("omlUniqueID");
		return Task.FromResult(GetAsObject("SalesOrderLines", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPSalesOrderLineInformationDto>> GetAllSalesOrderLines(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPSalesOrderLineInformationDto> collection = new List<ERPSalesOrderLineInformationDto>();
		InitializeParameterLists();
		string[] array = new string[65]
		{
			"omlCreatedBy", "omlCreatedDate", "omlDeliveryQuantityTotal", "omlDepositAmountBase", "omlDepositAmountForeign", "omlDepositPercent", "omlDiscountPercent", "omlDocuments", "omlUniqueID", "omlExtendedDiscountBase",
			"omlExtendedDiscountForeign", "omlExtendedPriceBase", "omlExtendedPriceForeign", "omlExtendedWeight", "omlFreightAmountBase", "omlFreightAmountForeign", "omlFullExtendedPriceBase", "omlFullExtendedPriceForeign", "omlFullUnitPriceBase", "omlFullUnitPriceForeign",
			"omlAvalaraIgnoreLine", "omlClosed", "omlConfigured", "omlDeposit", "omlDepositCreated", "omlDepositCredited", "omlPayCommission", "omlPriceOverride", "omlTimeAndMaterial", "omlLeadID",
			"omlLeadLineID", "omlNonTaxReasonID", "omlOrderQuantity", "omlOrgPartID", "omlOrgPartShortDescription", "omlPartGroupID", "omlPartID", "omlPartLongDescriptionRtf", "omlPartLongDescriptionText", "omlPartRevisionID",
			"omlPartShortDescription", "omlProjectAreaID", "omlProjectID", "omlQuantityShipped", "omlQuoteID", "omlQuoteLineID", "omlQuoteQuantityID", "omlReleaseNumber", "omlRmaClaimID", "omlRmaClaimLineID",
			"omlRowVersion", "omlSalesOrderID", "omlSecondTaxAmountBase", "omlSecondTaxAmountForeign", "omlSecondTaxCodeID", "omlSalesOrderLineID", "omlTaxAmountBase", "omlTaxAmountForeign", "omlTaxCodeID", "omlUnitDiscountBase",
			"omlUnitDiscountForeign", "omlUnitOfMeasure", "omlUnitPriceBase", "omlUnitPriceForeign", "omlWeight"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("SalesOrderLines");
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
		using (DataTable dataTable = GetAsDataTable("SalesOrderLines", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPSalesOrderLineInformationDto eRPSalesOrderLineInformationDto = new ERPSalesOrderLineInformationDto();
				eRPSalesOrderLineInformationDto.omlCreatedBy = dataTable.Rows[i].Field<string>("omlCreatedBy");
				eRPSalesOrderLineInformationDto.omlCreatedDate = dataTable.Rows[i].Field<DateTime?>("omlCreatedDate");
				eRPSalesOrderLineInformationDto.omlDeliveryQuantityTotal = dataTable.Rows[i].Field<decimal>("omlDeliveryQuantityTotal");
				eRPSalesOrderLineInformationDto.omlDepositAmountBase = dataTable.Rows[i].Field<decimal>("omlDepositAmountBase");
				eRPSalesOrderLineInformationDto.omlDepositAmountForeign = dataTable.Rows[i].Field<decimal>("omlDepositAmountForeign");
				eRPSalesOrderLineInformationDto.omlDepositPercent = dataTable.Rows[i].Field<decimal>("omlDepositPercent");
				eRPSalesOrderLineInformationDto.omlDiscountPercent = dataTable.Rows[i].Field<decimal>("omlDiscountPercent");
				eRPSalesOrderLineInformationDto.omlDocuments = dataTable.Rows[i].Field<string>("omlDocuments");
				eRPSalesOrderLineInformationDto.omlUniqueID = dataTable.Rows[i].Field<Guid>("omlUniqueID");
				eRPSalesOrderLineInformationDto.omlExtendedDiscountBase = dataTable.Rows[i].Field<decimal>("omlExtendedDiscountBase");
				eRPSalesOrderLineInformationDto.omlExtendedDiscountForeign = dataTable.Rows[i].Field<decimal>("omlExtendedDiscountForeign");
				eRPSalesOrderLineInformationDto.omlExtendedPriceBase = dataTable.Rows[i].Field<decimal>("omlExtendedPriceBase");
				eRPSalesOrderLineInformationDto.omlExtendedPriceForeign = dataTable.Rows[i].Field<decimal>("omlExtendedPriceForeign");
				eRPSalesOrderLineInformationDto.omlExtendedWeight = dataTable.Rows[i].Field<decimal>("omlExtendedWeight");
				eRPSalesOrderLineInformationDto.omlFreightAmountBase = dataTable.Rows[i].Field<decimal>("omlFreightAmountBase");
				eRPSalesOrderLineInformationDto.omlFreightAmountForeign = dataTable.Rows[i].Field<decimal>("omlFreightAmountForeign");
				eRPSalesOrderLineInformationDto.omlFullExtendedPriceBase = dataTable.Rows[i].Field<decimal>("omlFullExtendedPriceBase");
				eRPSalesOrderLineInformationDto.omlFullExtendedPriceForeign = dataTable.Rows[i].Field<decimal>("omlFullExtendedPriceForeign");
				eRPSalesOrderLineInformationDto.omlFullUnitPriceBase = dataTable.Rows[i].Field<decimal>("omlFullUnitPriceBase");
				eRPSalesOrderLineInformationDto.omlFullUnitPriceForeign = dataTable.Rows[i].Field<decimal>("omlFullUnitPriceForeign");
				eRPSalesOrderLineInformationDto.omlAvalaraIgnoreLine = dataTable.Rows[i].Field<bool>("omlAvalaraIgnoreLine");
				eRPSalesOrderLineInformationDto.omlClosed = dataTable.Rows[i].Field<bool>("omlClosed");
				eRPSalesOrderLineInformationDto.omlConfigured = dataTable.Rows[i].Field<bool>("omlConfigured");
				eRPSalesOrderLineInformationDto.omlDeposit = dataTable.Rows[i].Field<bool>("omlDeposit");
				eRPSalesOrderLineInformationDto.omlDepositCreated = dataTable.Rows[i].Field<bool>("omlDepositCreated");
				eRPSalesOrderLineInformationDto.omlDepositCredited = dataTable.Rows[i].Field<bool>("omlDepositCredited");
				eRPSalesOrderLineInformationDto.omlPayCommission = dataTable.Rows[i].Field<bool>("omlPayCommission");
				eRPSalesOrderLineInformationDto.omlPriceOverride = dataTable.Rows[i].Field<bool>("omlPriceOverride");
				eRPSalesOrderLineInformationDto.omlTimeAndMaterial = dataTable.Rows[i].Field<bool>("omlTimeAndMaterial");
				eRPSalesOrderLineInformationDto.omlLeadID = dataTable.Rows[i].Field<string>("omlLeadID");
				eRPSalesOrderLineInformationDto.omlLeadLineID = dataTable.Rows[i].Field<short>("omlLeadLineID");
				eRPSalesOrderLineInformationDto.omlNonTaxReasonID = dataTable.Rows[i].Field<string>("omlNonTaxReasonID");
				eRPSalesOrderLineInformationDto.omlOrderQuantity = dataTable.Rows[i].Field<decimal>("omlOrderQuantity");
				eRPSalesOrderLineInformationDto.omlOrgPartID = dataTable.Rows[i].Field<string>("omlOrgPartID");
				eRPSalesOrderLineInformationDto.omlOrgPartShortDescription = dataTable.Rows[i].Field<string>("omlOrgPartShortDescription");
				eRPSalesOrderLineInformationDto.omlPartGroupID = dataTable.Rows[i].Field<string>("omlPartGroupID");
				eRPSalesOrderLineInformationDto.omlPartID = dataTable.Rows[i].Field<string>("omlPartID");
				eRPSalesOrderLineInformationDto.omlPartLongDescriptionRtf = dataTable.Rows[i].Field<string>("omlPartLongDescriptionRtf");
				eRPSalesOrderLineInformationDto.omlPartLongDescriptionText = dataTable.Rows[i].Field<string>("omlPartLongDescriptionText");
				eRPSalesOrderLineInformationDto.omlPartRevisionID = dataTable.Rows[i].Field<string>("omlPartRevisionID");
				eRPSalesOrderLineInformationDto.omlPartShortDescription = dataTable.Rows[i].Field<string>("omlPartShortDescription");
				eRPSalesOrderLineInformationDto.omlProjectAreaID = dataTable.Rows[i].Field<string>("omlProjectAreaID");
				eRPSalesOrderLineInformationDto.omlProjectID = dataTable.Rows[i].Field<string>("omlProjectID");
				eRPSalesOrderLineInformationDto.omlQuantityShipped = dataTable.Rows[i].Field<decimal>("omlQuantityShipped");
				eRPSalesOrderLineInformationDto.omlQuoteID = dataTable.Rows[i].Field<string>("omlQuoteID");
				eRPSalesOrderLineInformationDto.omlQuoteLineID = dataTable.Rows[i].Field<short>("omlQuoteLineID");
				eRPSalesOrderLineInformationDto.omlQuoteQuantityID = dataTable.Rows[i].Field<byte>("omlQuoteQuantityID");
				eRPSalesOrderLineInformationDto.omlReleaseNumber = dataTable.Rows[i].Field<string>("omlReleaseNumber");
				eRPSalesOrderLineInformationDto.omlRmaClaimID = dataTable.Rows[i].Field<string>("omlRmaClaimID");
				eRPSalesOrderLineInformationDto.omlRmaClaimLineID = dataTable.Rows[i].Field<short>("omlRmaClaimLineID");
				eRPSalesOrderLineInformationDto.omlRowVersion = dataTable.Rows[i].Field<byte[]>("omlRowVersion");
				eRPSalesOrderLineInformationDto.omlSalesOrderID = dataTable.Rows[i].Field<string>("omlSalesOrderID");
				eRPSalesOrderLineInformationDto.omlSecondTaxAmountBase = dataTable.Rows[i].Field<decimal>("omlSecondTaxAmountBase");
				eRPSalesOrderLineInformationDto.omlSecondTaxAmountForeign = dataTable.Rows[i].Field<decimal>("omlSecondTaxAmountForeign");
				eRPSalesOrderLineInformationDto.omlSecondTaxCodeID = dataTable.Rows[i].Field<string>("omlSecondTaxCodeID");
				eRPSalesOrderLineInformationDto.omlSalesOrderLineID = dataTable.Rows[i].Field<short>("omlSalesOrderLineID");
				eRPSalesOrderLineInformationDto.omlTaxAmountBase = dataTable.Rows[i].Field<decimal>("omlTaxAmountBase");
				eRPSalesOrderLineInformationDto.omlTaxAmountForeign = dataTable.Rows[i].Field<decimal>("omlTaxAmountForeign");
				eRPSalesOrderLineInformationDto.omlTaxCodeID = dataTable.Rows[i].Field<string>("omlTaxCodeID");
				eRPSalesOrderLineInformationDto.omlUnitDiscountBase = dataTable.Rows[i].Field<decimal>("omlUnitDiscountBase");
				eRPSalesOrderLineInformationDto.omlUnitDiscountForeign = dataTable.Rows[i].Field<decimal>("omlUnitDiscountForeign");
				eRPSalesOrderLineInformationDto.omlUnitOfMeasure = dataTable.Rows[i].Field<string>("omlUnitOfMeasure");
				eRPSalesOrderLineInformationDto.omlUnitPriceBase = dataTable.Rows[i].Field<decimal>("omlUnitPriceBase");
				eRPSalesOrderLineInformationDto.omlUnitPriceForeign = dataTable.Rows[i].Field<decimal>("omlUnitPriceForeign");
				eRPSalesOrderLineInformationDto.omlWeight = dataTable.Rows[i].Field<decimal>("omlWeight");
				eRPSalesOrderLineInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPSalesOrderLineInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPSalesOrderLineInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPSalesOrderLineInformationDto> GetSalesOrderLine(Guid salesOrderLineId)
	{
		ERPSalesOrderLineInformationDto eRPSalesOrderLineInformationDto = new ERPSalesOrderLineInformationDto();
		InitializeParameterLists();
		string[] collection = new string[65]
		{
			"omlCreatedBy", "omlCreatedDate", "omlDeliveryQuantityTotal", "omlDepositAmountBase", "omlDepositAmountForeign", "omlDepositPercent", "omlDiscountPercent", "omlDocuments", "omlUniqueID", "omlExtendedDiscountBase",
			"omlExtendedDiscountForeign", "omlExtendedPriceBase", "omlExtendedPriceForeign", "omlExtendedWeight", "omlFreightAmountBase", "omlFreightAmountForeign", "omlFullExtendedPriceBase", "omlFullExtendedPriceForeign", "omlFullUnitPriceBase", "omlFullUnitPriceForeign",
			"omlAvalaraIgnoreLine", "omlClosed", "omlConfigured", "omlDeposit", "omlDepositCreated", "omlDepositCredited", "omlPayCommission", "omlPriceOverride", "omlTimeAndMaterial", "omlLeadID",
			"omlLeadLineID", "omlNonTaxReasonID", "omlOrderQuantity", "omlOrgPartID", "omlOrgPartShortDescription", "omlPartGroupID", "omlPartID", "omlPartLongDescriptionRtf", "omlPartLongDescriptionText", "omlPartRevisionID",
			"omlPartShortDescription", "omlProjectAreaID", "omlProjectID", "omlQuantityShipped", "omlQuoteID", "omlQuoteLineID", "omlQuoteQuantityID", "omlReleaseNumber", "omlRmaClaimID", "omlRmaClaimLineID",
			"omlRowVersion", "omlSalesOrderID", "omlSecondTaxAmountBase", "omlSecondTaxAmountForeign", "omlSecondTaxCodeID", "omlSalesOrderLineID", "omlTaxAmountBase", "omlTaxAmountForeign", "omlTaxCodeID", "omlUnitDiscountBase",
			"omlUnitDiscountForeign", "omlUnitOfMeasure", "omlUnitPriceBase", "omlUnitPriceForeign", "omlWeight"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("omlUniqueID|C", salesOrderLineId);
		AddCustomFieldsToSelectList("SalesOrderLines");
		using (DataTable dataTable = GetAsDataTable("SalesOrderLines", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPSalesOrderLineInformationDto);
			}
			eRPSalesOrderLineInformationDto.omlCreatedBy = dataTable.Rows[0].Field<string>("omlCreatedBy");
			eRPSalesOrderLineInformationDto.omlCreatedDate = dataTable.Rows[0].Field<DateTime?>("omlCreatedDate");
			eRPSalesOrderLineInformationDto.omlDeliveryQuantityTotal = dataTable.Rows[0].Field<decimal>("omlDeliveryQuantityTotal");
			eRPSalesOrderLineInformationDto.omlDepositAmountBase = dataTable.Rows[0].Field<decimal>("omlDepositAmountBase");
			eRPSalesOrderLineInformationDto.omlDepositAmountForeign = dataTable.Rows[0].Field<decimal>("omlDepositAmountForeign");
			eRPSalesOrderLineInformationDto.omlDepositPercent = dataTable.Rows[0].Field<decimal>("omlDepositPercent");
			eRPSalesOrderLineInformationDto.omlDiscountPercent = dataTable.Rows[0].Field<decimal>("omlDiscountPercent");
			eRPSalesOrderLineInformationDto.omlDocuments = dataTable.Rows[0].Field<string>("omlDocuments");
			eRPSalesOrderLineInformationDto.omlUniqueID = dataTable.Rows[0].Field<Guid>("omlUniqueID");
			eRPSalesOrderLineInformationDto.omlExtendedDiscountBase = dataTable.Rows[0].Field<decimal>("omlExtendedDiscountBase");
			eRPSalesOrderLineInformationDto.omlExtendedDiscountForeign = dataTable.Rows[0].Field<decimal>("omlExtendedDiscountForeign");
			eRPSalesOrderLineInformationDto.omlExtendedPriceBase = dataTable.Rows[0].Field<decimal>("omlExtendedPriceBase");
			eRPSalesOrderLineInformationDto.omlExtendedPriceForeign = dataTable.Rows[0].Field<decimal>("omlExtendedPriceForeign");
			eRPSalesOrderLineInformationDto.omlExtendedWeight = dataTable.Rows[0].Field<decimal>("omlExtendedWeight");
			eRPSalesOrderLineInformationDto.omlFreightAmountBase = dataTable.Rows[0].Field<decimal>("omlFreightAmountBase");
			eRPSalesOrderLineInformationDto.omlFreightAmountForeign = dataTable.Rows[0].Field<decimal>("omlFreightAmountForeign");
			eRPSalesOrderLineInformationDto.omlFullExtendedPriceBase = dataTable.Rows[0].Field<decimal>("omlFullExtendedPriceBase");
			eRPSalesOrderLineInformationDto.omlFullExtendedPriceForeign = dataTable.Rows[0].Field<decimal>("omlFullExtendedPriceForeign");
			eRPSalesOrderLineInformationDto.omlFullUnitPriceBase = dataTable.Rows[0].Field<decimal>("omlFullUnitPriceBase");
			eRPSalesOrderLineInformationDto.omlFullUnitPriceForeign = dataTable.Rows[0].Field<decimal>("omlFullUnitPriceForeign");
			eRPSalesOrderLineInformationDto.omlAvalaraIgnoreLine = dataTable.Rows[0].Field<bool>("omlAvalaraIgnoreLine");
			eRPSalesOrderLineInformationDto.omlClosed = dataTable.Rows[0].Field<bool>("omlClosed");
			eRPSalesOrderLineInformationDto.omlConfigured = dataTable.Rows[0].Field<bool>("omlConfigured");
			eRPSalesOrderLineInformationDto.omlDeposit = dataTable.Rows[0].Field<bool>("omlDeposit");
			eRPSalesOrderLineInformationDto.omlDepositCreated = dataTable.Rows[0].Field<bool>("omlDepositCreated");
			eRPSalesOrderLineInformationDto.omlDepositCredited = dataTable.Rows[0].Field<bool>("omlDepositCredited");
			eRPSalesOrderLineInformationDto.omlPayCommission = dataTable.Rows[0].Field<bool>("omlPayCommission");
			eRPSalesOrderLineInformationDto.omlPriceOverride = dataTable.Rows[0].Field<bool>("omlPriceOverride");
			eRPSalesOrderLineInformationDto.omlTimeAndMaterial = dataTable.Rows[0].Field<bool>("omlTimeAndMaterial");
			eRPSalesOrderLineInformationDto.omlLeadID = dataTable.Rows[0].Field<string>("omlLeadID");
			eRPSalesOrderLineInformationDto.omlLeadLineID = dataTable.Rows[0].Field<short>("omlLeadLineID");
			eRPSalesOrderLineInformationDto.omlNonTaxReasonID = dataTable.Rows[0].Field<string>("omlNonTaxReasonID");
			eRPSalesOrderLineInformationDto.omlOrderQuantity = dataTable.Rows[0].Field<decimal>("omlOrderQuantity");
			eRPSalesOrderLineInformationDto.omlOrgPartID = dataTable.Rows[0].Field<string>("omlOrgPartID");
			eRPSalesOrderLineInformationDto.omlOrgPartShortDescription = dataTable.Rows[0].Field<string>("omlOrgPartShortDescription");
			eRPSalesOrderLineInformationDto.omlPartGroupID = dataTable.Rows[0].Field<string>("omlPartGroupID");
			eRPSalesOrderLineInformationDto.omlPartID = dataTable.Rows[0].Field<string>("omlPartID");
			eRPSalesOrderLineInformationDto.omlPartLongDescriptionRtf = dataTable.Rows[0].Field<string>("omlPartLongDescriptionRtf");
			eRPSalesOrderLineInformationDto.omlPartLongDescriptionText = dataTable.Rows[0].Field<string>("omlPartLongDescriptionText");
			eRPSalesOrderLineInformationDto.omlPartRevisionID = dataTable.Rows[0].Field<string>("omlPartRevisionID");
			eRPSalesOrderLineInformationDto.omlPartShortDescription = dataTable.Rows[0].Field<string>("omlPartShortDescription");
			eRPSalesOrderLineInformationDto.omlProjectAreaID = dataTable.Rows[0].Field<string>("omlProjectAreaID");
			eRPSalesOrderLineInformationDto.omlProjectID = dataTable.Rows[0].Field<string>("omlProjectID");
			eRPSalesOrderLineInformationDto.omlQuantityShipped = dataTable.Rows[0].Field<decimal>("omlQuantityShipped");
			eRPSalesOrderLineInformationDto.omlQuoteID = dataTable.Rows[0].Field<string>("omlQuoteID");
			eRPSalesOrderLineInformationDto.omlQuoteLineID = dataTable.Rows[0].Field<short>("omlQuoteLineID");
			eRPSalesOrderLineInformationDto.omlQuoteQuantityID = dataTable.Rows[0].Field<byte>("omlQuoteQuantityID");
			eRPSalesOrderLineInformationDto.omlReleaseNumber = dataTable.Rows[0].Field<string>("omlReleaseNumber");
			eRPSalesOrderLineInformationDto.omlRmaClaimID = dataTable.Rows[0].Field<string>("omlRmaClaimID");
			eRPSalesOrderLineInformationDto.omlRmaClaimLineID = dataTable.Rows[0].Field<short>("omlRmaClaimLineID");
			eRPSalesOrderLineInformationDto.omlRowVersion = dataTable.Rows[0].Field<byte[]>("omlRowVersion");
			eRPSalesOrderLineInformationDto.omlSalesOrderID = dataTable.Rows[0].Field<string>("omlSalesOrderID");
			eRPSalesOrderLineInformationDto.omlSecondTaxAmountBase = dataTable.Rows[0].Field<decimal>("omlSecondTaxAmountBase");
			eRPSalesOrderLineInformationDto.omlSecondTaxAmountForeign = dataTable.Rows[0].Field<decimal>("omlSecondTaxAmountForeign");
			eRPSalesOrderLineInformationDto.omlSecondTaxCodeID = dataTable.Rows[0].Field<string>("omlSecondTaxCodeID");
			eRPSalesOrderLineInformationDto.omlSalesOrderLineID = dataTable.Rows[0].Field<short>("omlSalesOrderLineID");
			eRPSalesOrderLineInformationDto.omlTaxAmountBase = dataTable.Rows[0].Field<decimal>("omlTaxAmountBase");
			eRPSalesOrderLineInformationDto.omlTaxAmountForeign = dataTable.Rows[0].Field<decimal>("omlTaxAmountForeign");
			eRPSalesOrderLineInformationDto.omlTaxCodeID = dataTable.Rows[0].Field<string>("omlTaxCodeID");
			eRPSalesOrderLineInformationDto.omlUnitDiscountBase = dataTable.Rows[0].Field<decimal>("omlUnitDiscountBase");
			eRPSalesOrderLineInformationDto.omlUnitDiscountForeign = dataTable.Rows[0].Field<decimal>("omlUnitDiscountForeign");
			eRPSalesOrderLineInformationDto.omlUnitOfMeasure = dataTable.Rows[0].Field<string>("omlUnitOfMeasure");
			eRPSalesOrderLineInformationDto.omlUnitPriceBase = dataTable.Rows[0].Field<decimal>("omlUnitPriceBase");
			eRPSalesOrderLineInformationDto.omlUnitPriceForeign = dataTable.Rows[0].Field<decimal>("omlUnitPriceForeign");
			eRPSalesOrderLineInformationDto.omlWeight = dataTable.Rows[0].Field<decimal>("omlWeight");
			eRPSalesOrderLineInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPSalesOrderLineInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPSalesOrderLineInformationDto);
	}

	public Task<APIValidationInfoDto> SaveSalesOrderLine(ERPSalesOrderLineDto salesOrderLine)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM SalesOrderLines WHERE omlUniqueID = " + M1Util.ConvertToLinq(salesOrderLine.omlUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["omlSalesOrderID"] = salesOrderLine.omlSalesOrderID.ToUpper();
				dataRow["omlSalesOrderLineID"] = salesOrderLine.omlSalesOrderLineID;
				salesOrderLine.omlUniqueID = ((salesOrderLine.omlUniqueID == Guid.Empty) ? Guid.NewGuid() : salesOrderLine.omlUniqueID);
				dataRow["omlUniqueID"] = salesOrderLine.omlUniqueID;
				dataRow["omlCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["omlCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The SalesOrderLine could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (salesOrderLine.omlRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the SalesOrderLine is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["omlRowVersion"], salesOrderLine.omlRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the SalesOrderLine has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the SalesOrderLine again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["omlDeliveryQuantityTotal"] = salesOrderLine.omlDeliveryQuantityTotal;
			dataRow["omlDepositAmountBase"] = salesOrderLine.omlDepositAmountBase;
			dataRow["omlDepositAmountForeign"] = salesOrderLine.omlDepositAmountForeign;
			dataRow["omlDepositPercent"] = salesOrderLine.omlDepositPercent;
			dataRow["omlDiscountPercent"] = salesOrderLine.omlDiscountPercent;
			dataRow["omlDocuments"] = salesOrderLine.omlDocuments ?? dataRow["omlDocuments"];
			dataRow["omlExtendedDiscountBase"] = salesOrderLine.omlExtendedDiscountBase;
			dataRow["omlExtendedDiscountForeign"] = salesOrderLine.omlExtendedDiscountForeign;
			dataRow["omlExtendedPriceBase"] = salesOrderLine.omlExtendedPriceBase;
			dataRow["omlExtendedPriceForeign"] = salesOrderLine.omlExtendedPriceForeign;
			dataRow["omlExtendedWeight"] = salesOrderLine.omlExtendedWeight;
			dataRow["omlFreightAmountBase"] = salesOrderLine.omlFreightAmountBase;
			dataRow["omlFreightAmountForeign"] = salesOrderLine.omlFreightAmountForeign;
			dataRow["omlFullExtendedPriceBase"] = salesOrderLine.omlFullExtendedPriceBase;
			dataRow["omlFullExtendedPriceForeign"] = salesOrderLine.omlFullExtendedPriceForeign;
			dataRow["omlFullUnitPriceBase"] = salesOrderLine.omlFullUnitPriceBase;
			dataRow["omlFullUnitPriceForeign"] = salesOrderLine.omlFullUnitPriceForeign;
			dataRow["omlAvalaraIgnoreLine"] = salesOrderLine.omlAvalaraIgnoreLine;
			dataRow["omlClosed"] = salesOrderLine.omlClosed;
			dataRow["omlConfigured"] = salesOrderLine.omlConfigured;
			dataRow["omlDeposit"] = salesOrderLine.omlDeposit;
			dataRow["omlDepositCreated"] = salesOrderLine.omlDepositCreated;
			dataRow["omlDepositCredited"] = salesOrderLine.omlDepositCredited;
			dataRow["omlPayCommission"] = salesOrderLine.omlPayCommission;
			dataRow["omlPriceOverride"] = salesOrderLine.omlPriceOverride;
			dataRow["omlTimeAndMaterial"] = salesOrderLine.omlTimeAndMaterial;
			dataRow["omlLeadID"] = salesOrderLine.omlLeadID;
			dataRow["omlLeadLineID"] = salesOrderLine.omlLeadLineID;
			dataRow["omlNonTaxReasonID"] = salesOrderLine.omlNonTaxReasonID;
			dataRow["omlOrderQuantity"] = salesOrderLine.omlOrderQuantity;
			dataRow["omlOrgPartID"] = salesOrderLine.omlOrgPartID;
			dataRow["omlOrgPartShortDescription"] = salesOrderLine.omlOrgPartShortDescription;
			dataRow["omlPartGroupID"] = salesOrderLine.omlPartGroupID;
			dataRow["omlPartID"] = salesOrderLine.omlPartID;
			dataRow["omlPartLongDescriptionRtf"] = salesOrderLine.omlPartLongDescriptionRtf ?? dataRow["omlPartLongDescriptionRtf"];
			dataRow["omlPartLongDescriptionText"] = salesOrderLine.omlPartLongDescriptionText ?? dataRow["omlPartLongDescriptionText"];
			dataRow["omlPartRevisionID"] = salesOrderLine.omlPartRevisionID;
			dataRow["omlPartShortDescription"] = salesOrderLine.omlPartShortDescription;
			dataRow["omlProjectAreaID"] = salesOrderLine.omlProjectAreaID;
			dataRow["omlProjectID"] = salesOrderLine.omlProjectID;
			dataRow["omlQuantityShipped"] = salesOrderLine.omlQuantityShipped;
			dataRow["omlQuoteID"] = salesOrderLine.omlQuoteID;
			dataRow["omlQuoteLineID"] = salesOrderLine.omlQuoteLineID;
			dataRow["omlQuoteQuantityID"] = salesOrderLine.omlQuoteQuantityID;
			dataRow["omlReleaseNumber"] = salesOrderLine.omlReleaseNumber;
			dataRow["omlRmaClaimID"] = salesOrderLine.omlRmaClaimID;
			dataRow["omlRmaClaimLineID"] = salesOrderLine.omlRmaClaimLineID;
			dataRow["omlSecondTaxAmountBase"] = salesOrderLine.omlSecondTaxAmountBase;
			dataRow["omlSecondTaxAmountForeign"] = salesOrderLine.omlSecondTaxAmountForeign;
			dataRow["omlSecondTaxCodeID"] = salesOrderLine.omlSecondTaxCodeID;
			dataRow["omlTaxAmountBase"] = salesOrderLine.omlTaxAmountBase;
			dataRow["omlTaxAmountForeign"] = salesOrderLine.omlTaxAmountForeign;
			dataRow["omlTaxCodeID"] = salesOrderLine.omlTaxCodeID;
			dataRow["omlUnitDiscountBase"] = salesOrderLine.omlUnitDiscountBase;
			dataRow["omlUnitDiscountForeign"] = salesOrderLine.omlUnitDiscountForeign;
			dataRow["omlUnitOfMeasure"] = salesOrderLine.omlUnitOfMeasure;
			dataRow["omlUnitPriceBase"] = salesOrderLine.omlUnitPriceBase;
			dataRow["omlUnitPriceForeign"] = salesOrderLine.omlUnitPriceForeign;
			dataRow["omlWeight"] = salesOrderLine.omlWeight;
			if (salesOrderLine.CustomFields != null && salesOrderLine.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in salesOrderLine.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the SalesOrderLine [{salesOrderLine.omlUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the SalesOrderLine [{salesOrderLine.omlUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
