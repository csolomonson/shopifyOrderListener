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

public class ERPQuoteQuantityRepository : APIBaseRepository, IERPQuoteQuantityRepository, IAPIBaseRepository, IDisposable
{
	public ERPQuoteQuantityRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesQuoteQuantityExist(Guid quoteQuantityId)
	{
		InitializeParameterLists();
		base.filterList.Add("qmqUniqueID|C", quoteQuantityId);
		base.selectList.Add("qmqUniqueID");
		return Task.FromResult(GetAsObject("QuoteQuantities", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPQuoteQuantityInformationDto>> GetAllQuoteQuantities(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPQuoteQuantityInformationDto> collection = new List<ERPQuoteQuantityInformationDto>();
		InitializeParameterLists();
		string[] array = new string[70]
		{
			"qmqAdditionalChargeBase", "qmqAdditionalChargeDescription", "qmqAdditionalChargeForeign", "qmqAdditionalCostAmount", "qmqAdditionalCostDescription", "qmqAdditionalCostPrice", "qmqAdditionalMarkupPercent", "qmqAddSecondTaxAmountBase", "qmqAddSecondTaxAmountForeign", "qmqAddTaxAmountBase",
			"qmqAddTaxAmountForeign", "qmqCalculatedUnitPrice", "qmqCommissionPercent", "qmqCreatedBy", "qmqCreatedDate", "qmqDiscountPercent", "qmqDueDate", "qmqUniqueID", "qmqFullRevisedUnitPriceBase", "qmqFullRevisedUnitPriceForeign",
			"qmqClosed", "qmqCreatedFromMobile", "qmqPurchaseToOrder", "qmqLaborCost", "qmqLaborMarkupPercent", "qmqLaborPrice", "qmqLeadTime", "qmqMaterialCost", "qmqMaterialMarkupPercent", "qmqMaterialPrice",
			"qmqOverheadCost", "qmqOverheadMarkupPercent", "qmqOverheadPrice", "qmqProductionHours", "qmqPurchaseToOrderCost", "qmqPurchaseToOrderPrice", "qmqPurchaseUnitCostBase", "qmqPurToOrderMarkupPercent", "qmqQuoteID", "qmqQuoteLineID",
			"qmqQuoteMarkupType", "qmqQuoteQuantity", "qmqQuotingCost", "qmqQuotingMarkupPercent", "qmqQuotingPrice", "qmqRevisedUnitPriceBase", "qmqRevisedUnitPriceForeign", "qmqRowVersion", "qmqScrapPercent", "qmqSecondTaxCodeID",
			"qmqQuoteQuantityID", "qmqSetupHours", "qmqStartDate", "qmqSubcontractCost", "qmqSubcontractMarkupPercent", "qmqSubcontractPrice", "qmqTaxCodeID", "qmqTaxDate", "qmqTotalCost", "qmqTotalMarkupPercent",
			"qmqTotalPrice", "qmqTotalRunQuantity", "qmqTotalUnitCost", "qmqTotalUnitPrice", "qmqUnitDiscountBase", "qmqUnitDiscountForeign", "qmqUnitSecondTaxAmountBase", "qmqUnitSecondTaxAmountForeign", "qmqUnitTaxAmountBase", "qmqUnitTaxAmountForeign"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("QuoteQuantities");
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
		using (DataTable dataTable = GetAsDataTable("QuoteQuantities", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPQuoteQuantityInformationDto eRPQuoteQuantityInformationDto = new ERPQuoteQuantityInformationDto();
				eRPQuoteQuantityInformationDto.qmqAdditionalChargeBase = dataTable.Rows[i].Field<decimal>("qmqAdditionalChargeBase");
				eRPQuoteQuantityInformationDto.qmqAdditionalChargeDescription = dataTable.Rows[i].Field<string>("qmqAdditionalChargeDescription");
				eRPQuoteQuantityInformationDto.qmqAdditionalChargeForeign = dataTable.Rows[i].Field<decimal>("qmqAdditionalChargeForeign");
				eRPQuoteQuantityInformationDto.qmqAdditionalCostAmount = dataTable.Rows[i].Field<decimal>("qmqAdditionalCostAmount");
				eRPQuoteQuantityInformationDto.qmqAdditionalCostDescription = dataTable.Rows[i].Field<string>("qmqAdditionalCostDescription");
				eRPQuoteQuantityInformationDto.qmqAdditionalCostPrice = dataTable.Rows[i].Field<decimal>("qmqAdditionalCostPrice");
				eRPQuoteQuantityInformationDto.qmqAdditionalMarkupPercent = dataTable.Rows[i].Field<decimal>("qmqAdditionalMarkupPercent");
				eRPQuoteQuantityInformationDto.qmqAddSecondTaxAmountBase = dataTable.Rows[i].Field<decimal>("qmqAddSecondTaxAmountBase");
				eRPQuoteQuantityInformationDto.qmqAddSecondTaxAmountForeign = dataTable.Rows[i].Field<decimal>("qmqAddSecondTaxAmountForeign");
				eRPQuoteQuantityInformationDto.qmqAddTaxAmountBase = dataTable.Rows[i].Field<decimal>("qmqAddTaxAmountBase");
				eRPQuoteQuantityInformationDto.qmqAddTaxAmountForeign = dataTable.Rows[i].Field<decimal>("qmqAddTaxAmountForeign");
				eRPQuoteQuantityInformationDto.qmqCalculatedUnitPrice = dataTable.Rows[i].Field<decimal>("qmqCalculatedUnitPrice");
				eRPQuoteQuantityInformationDto.qmqCommissionPercent = dataTable.Rows[i].Field<decimal>("qmqCommissionPercent");
				eRPQuoteQuantityInformationDto.qmqCreatedBy = dataTable.Rows[i].Field<string>("qmqCreatedBy");
				eRPQuoteQuantityInformationDto.qmqCreatedDate = dataTable.Rows[i].Field<DateTime?>("qmqCreatedDate");
				eRPQuoteQuantityInformationDto.qmqDiscountPercent = dataTable.Rows[i].Field<decimal>("qmqDiscountPercent");
				eRPQuoteQuantityInformationDto.qmqDueDate = dataTable.Rows[i].Field<DateTime?>("qmqDueDate");
				eRPQuoteQuantityInformationDto.qmqUniqueID = dataTable.Rows[i].Field<Guid>("qmqUniqueID");
				eRPQuoteQuantityInformationDto.qmqFullRevisedUnitPriceBase = dataTable.Rows[i].Field<decimal>("qmqFullRevisedUnitPriceBase");
				eRPQuoteQuantityInformationDto.qmqFullRevisedUnitPriceForeign = dataTable.Rows[i].Field<decimal>("qmqFullRevisedUnitPriceForeign");
				eRPQuoteQuantityInformationDto.qmqClosed = dataTable.Rows[i].Field<bool>("qmqClosed");
				eRPQuoteQuantityInformationDto.qmqCreatedFromMobile = dataTable.Rows[i].Field<bool>("qmqCreatedFromMobile");
				eRPQuoteQuantityInformationDto.qmqPurchaseToOrder = dataTable.Rows[i].Field<bool>("qmqPurchaseToOrder");
				eRPQuoteQuantityInformationDto.qmqLaborCost = dataTable.Rows[i].Field<decimal>("qmqLaborCost");
				eRPQuoteQuantityInformationDto.qmqLaborMarkupPercent = dataTable.Rows[i].Field<decimal>("qmqLaborMarkupPercent");
				eRPQuoteQuantityInformationDto.qmqLaborPrice = dataTable.Rows[i].Field<decimal>("qmqLaborPrice");
				eRPQuoteQuantityInformationDto.qmqLeadTime = dataTable.Rows[i].Field<string>("qmqLeadTime");
				eRPQuoteQuantityInformationDto.qmqMaterialCost = dataTable.Rows[i].Field<decimal>("qmqMaterialCost");
				eRPQuoteQuantityInformationDto.qmqMaterialMarkupPercent = dataTable.Rows[i].Field<decimal>("qmqMaterialMarkupPercent");
				eRPQuoteQuantityInformationDto.qmqMaterialPrice = dataTable.Rows[i].Field<decimal>("qmqMaterialPrice");
				eRPQuoteQuantityInformationDto.qmqOverheadCost = dataTable.Rows[i].Field<decimal>("qmqOverheadCost");
				eRPQuoteQuantityInformationDto.qmqOverheadMarkupPercent = dataTable.Rows[i].Field<decimal>("qmqOverheadMarkupPercent");
				eRPQuoteQuantityInformationDto.qmqOverheadPrice = dataTable.Rows[i].Field<decimal>("qmqOverheadPrice");
				eRPQuoteQuantityInformationDto.qmqProductionHours = dataTable.Rows[i].Field<decimal>("qmqProductionHours");
				eRPQuoteQuantityInformationDto.qmqPurchaseToOrderCost = dataTable.Rows[i].Field<decimal>("qmqPurchaseToOrderCost");
				eRPQuoteQuantityInformationDto.qmqPurchaseToOrderPrice = dataTable.Rows[i].Field<decimal>("qmqPurchaseToOrderPrice");
				eRPQuoteQuantityInformationDto.qmqPurchaseUnitCostBase = dataTable.Rows[i].Field<decimal>("qmqPurchaseUnitCostBase");
				eRPQuoteQuantityInformationDto.qmqPurToOrderMarkupPercent = dataTable.Rows[i].Field<decimal>("qmqPurToOrderMarkupPercent");
				eRPQuoteQuantityInformationDto.qmqQuoteID = dataTable.Rows[i].Field<string>("qmqQuoteID");
				eRPQuoteQuantityInformationDto.qmqQuoteLineID = dataTable.Rows[i].Field<short>("qmqQuoteLineID");
				eRPQuoteQuantityInformationDto.qmqQuoteMarkupType = dataTable.Rows[i].Field<byte>("qmqQuoteMarkupType");
				eRPQuoteQuantityInformationDto.qmqQuoteQuantity = dataTable.Rows[i].Field<decimal>("qmqQuoteQuantity");
				eRPQuoteQuantityInformationDto.qmqQuotingCost = dataTable.Rows[i].Field<decimal>("qmqQuotingCost");
				eRPQuoteQuantityInformationDto.qmqQuotingMarkupPercent = dataTable.Rows[i].Field<decimal>("qmqQuotingMarkupPercent");
				eRPQuoteQuantityInformationDto.qmqQuotingPrice = dataTable.Rows[i].Field<decimal>("qmqQuotingPrice");
				eRPQuoteQuantityInformationDto.qmqRevisedUnitPriceBase = dataTable.Rows[i].Field<decimal>("qmqRevisedUnitPriceBase");
				eRPQuoteQuantityInformationDto.qmqRevisedUnitPriceForeign = dataTable.Rows[i].Field<decimal>("qmqRevisedUnitPriceForeign");
				eRPQuoteQuantityInformationDto.qmqRowVersion = dataTable.Rows[i].Field<byte[]>("qmqRowVersion");
				eRPQuoteQuantityInformationDto.qmqScrapPercent = dataTable.Rows[i].Field<decimal>("qmqScrapPercent");
				eRPQuoteQuantityInformationDto.qmqSecondTaxCodeID = dataTable.Rows[i].Field<string>("qmqSecondTaxCodeID");
				eRPQuoteQuantityInformationDto.qmqQuoteQuantityID = dataTable.Rows[i].Field<byte>("qmqQuoteQuantityID");
				eRPQuoteQuantityInformationDto.qmqSetupHours = dataTable.Rows[i].Field<decimal>("qmqSetupHours");
				eRPQuoteQuantityInformationDto.qmqStartDate = dataTable.Rows[i].Field<DateTime?>("qmqStartDate");
				eRPQuoteQuantityInformationDto.qmqSubcontractCost = dataTable.Rows[i].Field<decimal>("qmqSubcontractCost");
				eRPQuoteQuantityInformationDto.qmqSubcontractMarkupPercent = dataTable.Rows[i].Field<decimal>("qmqSubcontractMarkupPercent");
				eRPQuoteQuantityInformationDto.qmqSubcontractPrice = dataTable.Rows[i].Field<decimal>("qmqSubcontractPrice");
				eRPQuoteQuantityInformationDto.qmqTaxCodeID = dataTable.Rows[i].Field<string>("qmqTaxCodeID");
				eRPQuoteQuantityInformationDto.qmqTaxDate = dataTable.Rows[i].Field<DateTime?>("qmqTaxDate");
				eRPQuoteQuantityInformationDto.qmqTotalCost = dataTable.Rows[i].Field<decimal>("qmqTotalCost");
				eRPQuoteQuantityInformationDto.qmqTotalMarkupPercent = dataTable.Rows[i].Field<decimal>("qmqTotalMarkupPercent");
				eRPQuoteQuantityInformationDto.qmqTotalPrice = dataTable.Rows[i].Field<decimal>("qmqTotalPrice");
				eRPQuoteQuantityInformationDto.qmqTotalRunQuantity = dataTable.Rows[i].Field<decimal>("qmqTotalRunQuantity");
				eRPQuoteQuantityInformationDto.qmqTotalUnitCost = dataTable.Rows[i].Field<decimal>("qmqTotalUnitCost");
				eRPQuoteQuantityInformationDto.qmqTotalUnitPrice = dataTable.Rows[i].Field<decimal>("qmqTotalUnitPrice");
				eRPQuoteQuantityInformationDto.qmqUnitDiscountBase = dataTable.Rows[i].Field<decimal>("qmqUnitDiscountBase");
				eRPQuoteQuantityInformationDto.qmqUnitDiscountForeign = dataTable.Rows[i].Field<decimal>("qmqUnitDiscountForeign");
				eRPQuoteQuantityInformationDto.qmqUnitSecondTaxAmountBase = dataTable.Rows[i].Field<decimal>("qmqUnitSecondTaxAmountBase");
				eRPQuoteQuantityInformationDto.qmqUnitSecondTaxAmountForeign = dataTable.Rows[i].Field<decimal>("qmqUnitSecondTaxAmountForeign");
				eRPQuoteQuantityInformationDto.qmqUnitTaxAmountBase = dataTable.Rows[i].Field<decimal>("qmqUnitTaxAmountBase");
				eRPQuoteQuantityInformationDto.qmqUnitTaxAmountForeign = dataTable.Rows[i].Field<decimal>("qmqUnitTaxAmountForeign");
				eRPQuoteQuantityInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPQuoteQuantityInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPQuoteQuantityInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPQuoteQuantityInformationDto> GetQuoteQuantity(Guid quoteQuantityId)
	{
		ERPQuoteQuantityInformationDto eRPQuoteQuantityInformationDto = new ERPQuoteQuantityInformationDto();
		InitializeParameterLists();
		string[] collection = new string[70]
		{
			"qmqAdditionalChargeBase", "qmqAdditionalChargeDescription", "qmqAdditionalChargeForeign", "qmqAdditionalCostAmount", "qmqAdditionalCostDescription", "qmqAdditionalCostPrice", "qmqAdditionalMarkupPercent", "qmqAddSecondTaxAmountBase", "qmqAddSecondTaxAmountForeign", "qmqAddTaxAmountBase",
			"qmqAddTaxAmountForeign", "qmqCalculatedUnitPrice", "qmqCommissionPercent", "qmqCreatedBy", "qmqCreatedDate", "qmqDiscountPercent", "qmqDueDate", "qmqUniqueID", "qmqFullRevisedUnitPriceBase", "qmqFullRevisedUnitPriceForeign",
			"qmqClosed", "qmqCreatedFromMobile", "qmqPurchaseToOrder", "qmqLaborCost", "qmqLaborMarkupPercent", "qmqLaborPrice", "qmqLeadTime", "qmqMaterialCost", "qmqMaterialMarkupPercent", "qmqMaterialPrice",
			"qmqOverheadCost", "qmqOverheadMarkupPercent", "qmqOverheadPrice", "qmqProductionHours", "qmqPurchaseToOrderCost", "qmqPurchaseToOrderPrice", "qmqPurchaseUnitCostBase", "qmqPurToOrderMarkupPercent", "qmqQuoteID", "qmqQuoteLineID",
			"qmqQuoteMarkupType", "qmqQuoteQuantity", "qmqQuotingCost", "qmqQuotingMarkupPercent", "qmqQuotingPrice", "qmqRevisedUnitPriceBase", "qmqRevisedUnitPriceForeign", "qmqRowVersion", "qmqScrapPercent", "qmqSecondTaxCodeID",
			"qmqQuoteQuantityID", "qmqSetupHours", "qmqStartDate", "qmqSubcontractCost", "qmqSubcontractMarkupPercent", "qmqSubcontractPrice", "qmqTaxCodeID", "qmqTaxDate", "qmqTotalCost", "qmqTotalMarkupPercent",
			"qmqTotalPrice", "qmqTotalRunQuantity", "qmqTotalUnitCost", "qmqTotalUnitPrice", "qmqUnitDiscountBase", "qmqUnitDiscountForeign", "qmqUnitSecondTaxAmountBase", "qmqUnitSecondTaxAmountForeign", "qmqUnitTaxAmountBase", "qmqUnitTaxAmountForeign"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("qmqUniqueID|C", quoteQuantityId);
		AddCustomFieldsToSelectList("QuoteQuantities");
		using (DataTable dataTable = GetAsDataTable("QuoteQuantities", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPQuoteQuantityInformationDto);
			}
			eRPQuoteQuantityInformationDto.qmqAdditionalChargeBase = dataTable.Rows[0].Field<decimal>("qmqAdditionalChargeBase");
			eRPQuoteQuantityInformationDto.qmqAdditionalChargeDescription = dataTable.Rows[0].Field<string>("qmqAdditionalChargeDescription");
			eRPQuoteQuantityInformationDto.qmqAdditionalChargeForeign = dataTable.Rows[0].Field<decimal>("qmqAdditionalChargeForeign");
			eRPQuoteQuantityInformationDto.qmqAdditionalCostAmount = dataTable.Rows[0].Field<decimal>("qmqAdditionalCostAmount");
			eRPQuoteQuantityInformationDto.qmqAdditionalCostDescription = dataTable.Rows[0].Field<string>("qmqAdditionalCostDescription");
			eRPQuoteQuantityInformationDto.qmqAdditionalCostPrice = dataTable.Rows[0].Field<decimal>("qmqAdditionalCostPrice");
			eRPQuoteQuantityInformationDto.qmqAdditionalMarkupPercent = dataTable.Rows[0].Field<decimal>("qmqAdditionalMarkupPercent");
			eRPQuoteQuantityInformationDto.qmqAddSecondTaxAmountBase = dataTable.Rows[0].Field<decimal>("qmqAddSecondTaxAmountBase");
			eRPQuoteQuantityInformationDto.qmqAddSecondTaxAmountForeign = dataTable.Rows[0].Field<decimal>("qmqAddSecondTaxAmountForeign");
			eRPQuoteQuantityInformationDto.qmqAddTaxAmountBase = dataTable.Rows[0].Field<decimal>("qmqAddTaxAmountBase");
			eRPQuoteQuantityInformationDto.qmqAddTaxAmountForeign = dataTable.Rows[0].Field<decimal>("qmqAddTaxAmountForeign");
			eRPQuoteQuantityInformationDto.qmqCalculatedUnitPrice = dataTable.Rows[0].Field<decimal>("qmqCalculatedUnitPrice");
			eRPQuoteQuantityInformationDto.qmqCommissionPercent = dataTable.Rows[0].Field<decimal>("qmqCommissionPercent");
			eRPQuoteQuantityInformationDto.qmqCreatedBy = dataTable.Rows[0].Field<string>("qmqCreatedBy");
			eRPQuoteQuantityInformationDto.qmqCreatedDate = dataTable.Rows[0].Field<DateTime?>("qmqCreatedDate");
			eRPQuoteQuantityInformationDto.qmqDiscountPercent = dataTable.Rows[0].Field<decimal>("qmqDiscountPercent");
			eRPQuoteQuantityInformationDto.qmqDueDate = dataTable.Rows[0].Field<DateTime?>("qmqDueDate");
			eRPQuoteQuantityInformationDto.qmqUniqueID = dataTable.Rows[0].Field<Guid>("qmqUniqueID");
			eRPQuoteQuantityInformationDto.qmqFullRevisedUnitPriceBase = dataTable.Rows[0].Field<decimal>("qmqFullRevisedUnitPriceBase");
			eRPQuoteQuantityInformationDto.qmqFullRevisedUnitPriceForeign = dataTable.Rows[0].Field<decimal>("qmqFullRevisedUnitPriceForeign");
			eRPQuoteQuantityInformationDto.qmqClosed = dataTable.Rows[0].Field<bool>("qmqClosed");
			eRPQuoteQuantityInformationDto.qmqCreatedFromMobile = dataTable.Rows[0].Field<bool>("qmqCreatedFromMobile");
			eRPQuoteQuantityInformationDto.qmqPurchaseToOrder = dataTable.Rows[0].Field<bool>("qmqPurchaseToOrder");
			eRPQuoteQuantityInformationDto.qmqLaborCost = dataTable.Rows[0].Field<decimal>("qmqLaborCost");
			eRPQuoteQuantityInformationDto.qmqLaborMarkupPercent = dataTable.Rows[0].Field<decimal>("qmqLaborMarkupPercent");
			eRPQuoteQuantityInformationDto.qmqLaborPrice = dataTable.Rows[0].Field<decimal>("qmqLaborPrice");
			eRPQuoteQuantityInformationDto.qmqLeadTime = dataTable.Rows[0].Field<string>("qmqLeadTime");
			eRPQuoteQuantityInformationDto.qmqMaterialCost = dataTable.Rows[0].Field<decimal>("qmqMaterialCost");
			eRPQuoteQuantityInformationDto.qmqMaterialMarkupPercent = dataTable.Rows[0].Field<decimal>("qmqMaterialMarkupPercent");
			eRPQuoteQuantityInformationDto.qmqMaterialPrice = dataTable.Rows[0].Field<decimal>("qmqMaterialPrice");
			eRPQuoteQuantityInformationDto.qmqOverheadCost = dataTable.Rows[0].Field<decimal>("qmqOverheadCost");
			eRPQuoteQuantityInformationDto.qmqOverheadMarkupPercent = dataTable.Rows[0].Field<decimal>("qmqOverheadMarkupPercent");
			eRPQuoteQuantityInformationDto.qmqOverheadPrice = dataTable.Rows[0].Field<decimal>("qmqOverheadPrice");
			eRPQuoteQuantityInformationDto.qmqProductionHours = dataTable.Rows[0].Field<decimal>("qmqProductionHours");
			eRPQuoteQuantityInformationDto.qmqPurchaseToOrderCost = dataTable.Rows[0].Field<decimal>("qmqPurchaseToOrderCost");
			eRPQuoteQuantityInformationDto.qmqPurchaseToOrderPrice = dataTable.Rows[0].Field<decimal>("qmqPurchaseToOrderPrice");
			eRPQuoteQuantityInformationDto.qmqPurchaseUnitCostBase = dataTable.Rows[0].Field<decimal>("qmqPurchaseUnitCostBase");
			eRPQuoteQuantityInformationDto.qmqPurToOrderMarkupPercent = dataTable.Rows[0].Field<decimal>("qmqPurToOrderMarkupPercent");
			eRPQuoteQuantityInformationDto.qmqQuoteID = dataTable.Rows[0].Field<string>("qmqQuoteID");
			eRPQuoteQuantityInformationDto.qmqQuoteLineID = dataTable.Rows[0].Field<short>("qmqQuoteLineID");
			eRPQuoteQuantityInformationDto.qmqQuoteMarkupType = dataTable.Rows[0].Field<byte>("qmqQuoteMarkupType");
			eRPQuoteQuantityInformationDto.qmqQuoteQuantity = dataTable.Rows[0].Field<decimal>("qmqQuoteQuantity");
			eRPQuoteQuantityInformationDto.qmqQuotingCost = dataTable.Rows[0].Field<decimal>("qmqQuotingCost");
			eRPQuoteQuantityInformationDto.qmqQuotingMarkupPercent = dataTable.Rows[0].Field<decimal>("qmqQuotingMarkupPercent");
			eRPQuoteQuantityInformationDto.qmqQuotingPrice = dataTable.Rows[0].Field<decimal>("qmqQuotingPrice");
			eRPQuoteQuantityInformationDto.qmqRevisedUnitPriceBase = dataTable.Rows[0].Field<decimal>("qmqRevisedUnitPriceBase");
			eRPQuoteQuantityInformationDto.qmqRevisedUnitPriceForeign = dataTable.Rows[0].Field<decimal>("qmqRevisedUnitPriceForeign");
			eRPQuoteQuantityInformationDto.qmqRowVersion = dataTable.Rows[0].Field<byte[]>("qmqRowVersion");
			eRPQuoteQuantityInformationDto.qmqScrapPercent = dataTable.Rows[0].Field<decimal>("qmqScrapPercent");
			eRPQuoteQuantityInformationDto.qmqSecondTaxCodeID = dataTable.Rows[0].Field<string>("qmqSecondTaxCodeID");
			eRPQuoteQuantityInformationDto.qmqQuoteQuantityID = dataTable.Rows[0].Field<byte>("qmqQuoteQuantityID");
			eRPQuoteQuantityInformationDto.qmqSetupHours = dataTable.Rows[0].Field<decimal>("qmqSetupHours");
			eRPQuoteQuantityInformationDto.qmqStartDate = dataTable.Rows[0].Field<DateTime?>("qmqStartDate");
			eRPQuoteQuantityInformationDto.qmqSubcontractCost = dataTable.Rows[0].Field<decimal>("qmqSubcontractCost");
			eRPQuoteQuantityInformationDto.qmqSubcontractMarkupPercent = dataTable.Rows[0].Field<decimal>("qmqSubcontractMarkupPercent");
			eRPQuoteQuantityInformationDto.qmqSubcontractPrice = dataTable.Rows[0].Field<decimal>("qmqSubcontractPrice");
			eRPQuoteQuantityInformationDto.qmqTaxCodeID = dataTable.Rows[0].Field<string>("qmqTaxCodeID");
			eRPQuoteQuantityInformationDto.qmqTaxDate = dataTable.Rows[0].Field<DateTime?>("qmqTaxDate");
			eRPQuoteQuantityInformationDto.qmqTotalCost = dataTable.Rows[0].Field<decimal>("qmqTotalCost");
			eRPQuoteQuantityInformationDto.qmqTotalMarkupPercent = dataTable.Rows[0].Field<decimal>("qmqTotalMarkupPercent");
			eRPQuoteQuantityInformationDto.qmqTotalPrice = dataTable.Rows[0].Field<decimal>("qmqTotalPrice");
			eRPQuoteQuantityInformationDto.qmqTotalRunQuantity = dataTable.Rows[0].Field<decimal>("qmqTotalRunQuantity");
			eRPQuoteQuantityInformationDto.qmqTotalUnitCost = dataTable.Rows[0].Field<decimal>("qmqTotalUnitCost");
			eRPQuoteQuantityInformationDto.qmqTotalUnitPrice = dataTable.Rows[0].Field<decimal>("qmqTotalUnitPrice");
			eRPQuoteQuantityInformationDto.qmqUnitDiscountBase = dataTable.Rows[0].Field<decimal>("qmqUnitDiscountBase");
			eRPQuoteQuantityInformationDto.qmqUnitDiscountForeign = dataTable.Rows[0].Field<decimal>("qmqUnitDiscountForeign");
			eRPQuoteQuantityInformationDto.qmqUnitSecondTaxAmountBase = dataTable.Rows[0].Field<decimal>("qmqUnitSecondTaxAmountBase");
			eRPQuoteQuantityInformationDto.qmqUnitSecondTaxAmountForeign = dataTable.Rows[0].Field<decimal>("qmqUnitSecondTaxAmountForeign");
			eRPQuoteQuantityInformationDto.qmqUnitTaxAmountBase = dataTable.Rows[0].Field<decimal>("qmqUnitTaxAmountBase");
			eRPQuoteQuantityInformationDto.qmqUnitTaxAmountForeign = dataTable.Rows[0].Field<decimal>("qmqUnitTaxAmountForeign");
			eRPQuoteQuantityInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPQuoteQuantityInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPQuoteQuantityInformationDto);
	}

	public Task<APIValidationInfoDto> SaveQuoteQuantity(ERPQuoteQuantityDto quoteQuantity)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM QuoteQuantities WHERE qmqUniqueID = " + M1Util.ConvertToLinq(quoteQuantity.qmqUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["qmqQuoteID"] = quoteQuantity.qmqQuoteID.ToUpper();
				dataRow["qmqQuoteLineID"] = quoteQuantity.qmqQuoteLineID;
				dataRow["qmqQuoteQuantityID"] = quoteQuantity.qmqQuoteQuantityID;
				quoteQuantity.qmqUniqueID = ((quoteQuantity.qmqUniqueID == Guid.Empty) ? Guid.NewGuid() : quoteQuantity.qmqUniqueID);
				dataRow["qmqUniqueID"] = quoteQuantity.qmqUniqueID;
				dataRow["qmqCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["qmqCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The QuoteQuantity could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (quoteQuantity.qmqRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the QuoteQuantity is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["qmqRowVersion"], quoteQuantity.qmqRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the QuoteQuantity has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the QuoteQuantity again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["qmqAdditionalChargeBase"] = quoteQuantity.qmqAdditionalChargeBase;
			dataRow["qmqAdditionalChargeDescription"] = quoteQuantity.qmqAdditionalChargeDescription;
			dataRow["qmqAdditionalChargeForeign"] = quoteQuantity.qmqAdditionalChargeForeign;
			dataRow["qmqAdditionalCostAmount"] = quoteQuantity.qmqAdditionalCostAmount;
			dataRow["qmqAdditionalCostDescription"] = quoteQuantity.qmqAdditionalCostDescription;
			dataRow["qmqAdditionalCostPrice"] = quoteQuantity.qmqAdditionalCostPrice;
			dataRow["qmqAdditionalMarkupPercent"] = quoteQuantity.qmqAdditionalMarkupPercent;
			dataRow["qmqAddSecondTaxAmountBase"] = quoteQuantity.qmqAddSecondTaxAmountBase;
			dataRow["qmqAddSecondTaxAmountForeign"] = quoteQuantity.qmqAddSecondTaxAmountForeign;
			dataRow["qmqAddTaxAmountBase"] = quoteQuantity.qmqAddTaxAmountBase;
			dataRow["qmqAddTaxAmountForeign"] = quoteQuantity.qmqAddTaxAmountForeign;
			dataRow["qmqCalculatedUnitPrice"] = quoteQuantity.qmqCalculatedUnitPrice;
			dataRow["qmqCommissionPercent"] = quoteQuantity.qmqCommissionPercent;
			dataRow["qmqDiscountPercent"] = quoteQuantity.qmqDiscountPercent;
			DataRow dataRow2 = dataRow;
			DateTime? qmqDueDate = quoteQuantity.qmqDueDate;
			dataRow2["qmqDueDate"] = (qmqDueDate.HasValue ? ((object)qmqDueDate.GetValueOrDefault()) : dataRow["qmqDueDate"]);
			dataRow["qmqFullRevisedUnitPriceBase"] = quoteQuantity.qmqFullRevisedUnitPriceBase;
			dataRow["qmqFullRevisedUnitPriceForeign"] = quoteQuantity.qmqFullRevisedUnitPriceForeign;
			dataRow["qmqClosed"] = quoteQuantity.qmqClosed;
			dataRow["qmqCreatedFromMobile"] = quoteQuantity.qmqCreatedFromMobile;
			dataRow["qmqPurchaseToOrder"] = quoteQuantity.qmqPurchaseToOrder;
			dataRow["qmqLaborCost"] = quoteQuantity.qmqLaborCost;
			dataRow["qmqLaborMarkupPercent"] = quoteQuantity.qmqLaborMarkupPercent;
			dataRow["qmqLaborPrice"] = quoteQuantity.qmqLaborPrice;
			dataRow["qmqLeadTime"] = quoteQuantity.qmqLeadTime;
			dataRow["qmqMaterialCost"] = quoteQuantity.qmqMaterialCost;
			dataRow["qmqMaterialMarkupPercent"] = quoteQuantity.qmqMaterialMarkupPercent;
			dataRow["qmqMaterialPrice"] = quoteQuantity.qmqMaterialPrice;
			dataRow["qmqOverheadCost"] = quoteQuantity.qmqOverheadCost;
			dataRow["qmqOverheadMarkupPercent"] = quoteQuantity.qmqOverheadMarkupPercent;
			dataRow["qmqOverheadPrice"] = quoteQuantity.qmqOverheadPrice;
			dataRow["qmqProductionHours"] = quoteQuantity.qmqProductionHours;
			dataRow["qmqPurchaseToOrderCost"] = quoteQuantity.qmqPurchaseToOrderCost;
			dataRow["qmqPurchaseToOrderPrice"] = quoteQuantity.qmqPurchaseToOrderPrice;
			dataRow["qmqPurchaseUnitCostBase"] = quoteQuantity.qmqPurchaseUnitCostBase;
			dataRow["qmqPurToOrderMarkupPercent"] = quoteQuantity.qmqPurToOrderMarkupPercent;
			dataRow["qmqQuoteMarkupType"] = quoteQuantity.qmqQuoteMarkupType;
			dataRow["qmqQuoteQuantity"] = quoteQuantity.qmqQuoteQuantity;
			dataRow["qmqQuotingCost"] = quoteQuantity.qmqQuotingCost;
			dataRow["qmqQuotingMarkupPercent"] = quoteQuantity.qmqQuotingMarkupPercent;
			dataRow["qmqQuotingPrice"] = quoteQuantity.qmqQuotingPrice;
			dataRow["qmqRevisedUnitPriceBase"] = quoteQuantity.qmqRevisedUnitPriceBase;
			dataRow["qmqRevisedUnitPriceForeign"] = quoteQuantity.qmqRevisedUnitPriceForeign;
			dataRow["qmqScrapPercent"] = quoteQuantity.qmqScrapPercent;
			dataRow["qmqSecondTaxCodeID"] = quoteQuantity.qmqSecondTaxCodeID;
			dataRow["qmqSetupHours"] = quoteQuantity.qmqSetupHours;
			DataRow dataRow3 = dataRow;
			qmqDueDate = quoteQuantity.qmqStartDate;
			dataRow3["qmqStartDate"] = (qmqDueDate.HasValue ? ((object)qmqDueDate.GetValueOrDefault()) : dataRow["qmqStartDate"]);
			dataRow["qmqSubcontractCost"] = quoteQuantity.qmqSubcontractCost;
			dataRow["qmqSubcontractMarkupPercent"] = quoteQuantity.qmqSubcontractMarkupPercent;
			dataRow["qmqSubcontractPrice"] = quoteQuantity.qmqSubcontractPrice;
			dataRow["qmqTaxCodeID"] = quoteQuantity.qmqTaxCodeID;
			DataRow dataRow4 = dataRow;
			qmqDueDate = quoteQuantity.qmqTaxDate;
			dataRow4["qmqTaxDate"] = (qmqDueDate.HasValue ? ((object)qmqDueDate.GetValueOrDefault()) : dataRow["qmqTaxDate"]);
			dataRow["qmqTotalCost"] = quoteQuantity.qmqTotalCost;
			dataRow["qmqTotalMarkupPercent"] = quoteQuantity.qmqTotalMarkupPercent;
			dataRow["qmqTotalPrice"] = quoteQuantity.qmqTotalPrice;
			dataRow["qmqTotalRunQuantity"] = quoteQuantity.qmqTotalRunQuantity;
			dataRow["qmqTotalUnitCost"] = quoteQuantity.qmqTotalUnitCost;
			dataRow["qmqTotalUnitPrice"] = quoteQuantity.qmqTotalUnitPrice;
			dataRow["qmqUnitDiscountBase"] = quoteQuantity.qmqUnitDiscountBase;
			dataRow["qmqUnitDiscountForeign"] = quoteQuantity.qmqUnitDiscountForeign;
			dataRow["qmqUnitSecondTaxAmountBase"] = quoteQuantity.qmqUnitSecondTaxAmountBase;
			dataRow["qmqUnitSecondTaxAmountForeign"] = quoteQuantity.qmqUnitSecondTaxAmountForeign;
			dataRow["qmqUnitTaxAmountBase"] = quoteQuantity.qmqUnitTaxAmountBase;
			dataRow["qmqUnitTaxAmountForeign"] = quoteQuantity.qmqUnitTaxAmountForeign;
			if (quoteQuantity.CustomFields != null && quoteQuantity.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in quoteQuantity.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the QuoteQuantity [{quoteQuantity.qmqUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the QuoteQuantity [{quoteQuantity.qmqUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
