using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using M1.API.DTOs.BOM;
using M1.API.Utilities;

namespace M1.API.Repositories.Core;

public class QuoteQuantityRepository : APIBaseRepository, IQuoteQuantityRepository, IAPIBaseRepository, IDisposable
{
	private readonly string[] quoteQuantityFields = new string[46]
	{
		"qmqQuoteID", "qmqQuoteLineID", "qmqQuoteQuantityID", "qmqQuoteQuantity", "qmqScrapPercent", "qmqTotalRunQuantity", "qmqQuoteMarkupType", "qmqPurchaseToOrder", "qmqSetupHours", "qmqProductionHours",
		"qmqMaterialCost", "qmqMaterialMarkupPercent", "qmqMaterialPrice", "qmqSubcontractPrice", "qmqLaborCost", "qmqLaborMarkupPercent", "qmqLaborPrice", "qmqOverheadCost", "qmqOverheadMarkupPercent", "qmqOverheadPrice",
		"qmqQuotingPrice", "qmqPurchaseUnitCostBase", "qmqPurchaseToOrderCost", "qmqPurToOrderMarkupPercent", "qmqPurchaseToOrderPrice", "qmqAdditionalCostAmount", "qmqAdditionalMarkupPercent", "qmqAdditionalCostPrice", "qmqTotalCost", "qmqTotalPrice",
		"qmqTotalUnitCost", "qmqTotalMarkupPercent", "qmqTotalUnitPrice", "qmqCalculatedUnitPrice", "qmqFullRevisedUnitPriceForeign", "qmqDiscountPercent", "qmqUnitDiscountForeign", "qmqRevisedUnitPriceForeign", "qmqAdditionalChargeForeign", "qmqAdditionalChargeDescription",
		"qmqLeadTime", "qmqClosed", "qmqCreatedBy", "qmqCreatedDate", "qmqUniqueID", "qmqRowVersion"
	};

	public QuoteQuantityRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
	}

	public Task<bool> DoesQuoteQuantityExists(string quoteQuantityId)
	{
		InitializeParameterLists();
		base.filterList.Add("qmqQuoteQuantityID|C", quoteQuantityId);
		base.selectList.Add("qmqQuoteQuantityID");
		return Task.FromResult(GetAsObject("QuoteQuantities", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<BOMQuoteQuantityDto>> GetAllQuoteQuantities(int? pageSize = null, int? pageNumber = null)
	{
		ICollection<BOMQuoteQuantityDto> collection = new List<BOMQuoteQuantityDto>();
		InitializeParameterLists();
		base.selectList.AddRange(quoteQuantityFields);
		List<string> orderbyList = new List<string> { "qmqQuoteQuantityID" };
		using (DataTable dataTable = GetAsDataTable("QuoteQuantities", base.filterList, base.selectList, orderbyList, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				BOMQuoteQuantityDto bOMQuoteQuantityDto = new BOMQuoteQuantityDto();
				bOMQuoteQuantityDto.QuoteID = dataTable.Rows[i].Field<string>("qmqQuoteID");
				bOMQuoteQuantityDto.QuoteLineID = dataTable.Rows[i].Field<short>("qmqQuoteLineID");
				bOMQuoteQuantityDto.QuoteQuantityID = dataTable.Rows[i].Field<byte>("qmqQuoteQuantityID");
				bOMQuoteQuantityDto.QuoteQuantity = dataTable.Rows[i].Field<decimal>("qmqQuoteQuantity");
				bOMQuoteQuantityDto.ScrapPercent = dataTable.Rows[i].Field<decimal>("qmqScrapPercent");
				bOMQuoteQuantityDto.TotalRunQuantity = dataTable.Rows[i].Field<decimal>("qmqTotalRunQuantity");
				bOMQuoteQuantityDto.QuoteMarkupType = dataTable.Rows[i].Field<byte>("qmqQuoteMarkupType");
				bOMQuoteQuantityDto.PurchaseToOrder = dataTable.Rows[i].Field<bool>("qmqPurchaseToOrder");
				bOMQuoteQuantityDto.SetupHours = dataTable.Rows[i].Field<decimal>("qmqSetupHours");
				bOMQuoteQuantityDto.ProductionHours = dataTable.Rows[i].Field<decimal>("qmqProductionHours");
				bOMQuoteQuantityDto.MaterialCost = dataTable.Rows[i].Field<decimal>("qmqMaterialCost");
				bOMQuoteQuantityDto.MaterialMarkupPercent = dataTable.Rows[i].Field<decimal>("qmqMaterialMarkupPercent");
				bOMQuoteQuantityDto.MaterialPrice = dataTable.Rows[i].Field<decimal>("qmqMaterialPrice");
				bOMQuoteQuantityDto.SubcontractPrice = dataTable.Rows[i].Field<decimal>("qmqSubcontractPrice");
				bOMQuoteQuantityDto.LaborCost = dataTable.Rows[i].Field<decimal>("qmqLaborCost");
				bOMQuoteQuantityDto.LaborMarkupPercent = dataTable.Rows[i].Field<decimal>("qmqLaborMarkupPercent");
				bOMQuoteQuantityDto.LaborPrice = dataTable.Rows[i].Field<decimal>("qmqLaborPrice");
				bOMQuoteQuantityDto.OverheadCost = dataTable.Rows[i].Field<decimal>("qmqOverheadCost");
				bOMQuoteQuantityDto.OverheadMarkupPercent = dataTable.Rows[i].Field<decimal>("qmqOverheadMarkupPercent");
				bOMQuoteQuantityDto.OverheadPrice = dataTable.Rows[i].Field<decimal>("qmqOverheadPrice");
				bOMQuoteQuantityDto.QuotingPrice = dataTable.Rows[i].Field<decimal>("qmqQuotingPrice");
				bOMQuoteQuantityDto.PurchaseUnitCostBase = dataTable.Rows[i].Field<decimal>("qmqPurchaseUnitCostBase");
				bOMQuoteQuantityDto.PurchaseToOrderCost = dataTable.Rows[i].Field<decimal>("qmqPurchaseToOrderCost");
				bOMQuoteQuantityDto.PurToOrderMarkupPercent = dataTable.Rows[i].Field<decimal>("qmqPurToOrderMarkupPercent");
				bOMQuoteQuantityDto.PurchaseToOrderPrice = dataTable.Rows[i].Field<decimal>("qmqPurchaseToOrderPrice");
				bOMQuoteQuantityDto.AdditionalCostAmount = dataTable.Rows[i].Field<decimal>("qmqAdditionalCostAmount");
				bOMQuoteQuantityDto.AdditionalMarkupPercent = dataTable.Rows[i].Field<decimal>("qmqAdditionalMarkupPercent");
				bOMQuoteQuantityDto.AdditionalCostPrice = dataTable.Rows[i].Field<decimal>("qmqAdditionalCostPrice");
				bOMQuoteQuantityDto.TotalCost = dataTable.Rows[i].Field<decimal>("qmqTotalCost");
				bOMQuoteQuantityDto.TotalPrice = dataTable.Rows[i].Field<decimal>("qmqTotalPrice");
				bOMQuoteQuantityDto.TotalUnitCost = dataTable.Rows[i].Field<decimal>("qmqTotalUnitCost");
				bOMQuoteQuantityDto.TotalMarkupPercent = dataTable.Rows[i].Field<decimal>("qmqTotalMarkupPercent");
				bOMQuoteQuantityDto.TotalUnitPrice = dataTable.Rows[i].Field<decimal>("qmqTotalUnitPrice");
				bOMQuoteQuantityDto.CalculatedUnitPrice = dataTable.Rows[i].Field<decimal>("qmqCalculatedUnitPrice");
				bOMQuoteQuantityDto.FullRevisedUnitPriceForeign = dataTable.Rows[i].Field<decimal>("qmqFullRevisedUnitPriceForeign");
				bOMQuoteQuantityDto.DiscountPercent = dataTable.Rows[i].Field<decimal>("qmqDiscountPercent");
				bOMQuoteQuantityDto.UnitDiscountForeign = dataTable.Rows[i].Field<decimal>("qmqUnitDiscountForeign");
				bOMQuoteQuantityDto.RevisedUnitPriceForeign = dataTable.Rows[i].Field<decimal>("qmqRevisedUnitPriceForeign");
				bOMQuoteQuantityDto.AdditionalChargeForeign = dataTable.Rows[i].Field<decimal>("qmqAdditionalChargeForeign");
				bOMQuoteQuantityDto.AdditionalChargeDescription = dataTable.Rows[i].Field<string>("qmqAdditionalChargeDescription");
				bOMQuoteQuantityDto.LeadTime = dataTable.Rows[i].Field<string>("qmqLeadTime");
				bOMQuoteQuantityDto.Closed = dataTable.Rows[i].Field<bool>("qmqClosed");
				bOMQuoteQuantityDto.CreatedBy = dataTable.Rows[i].Field<string>("qmqCreatedBy");
				bOMQuoteQuantityDto.CreatedDate = ((!dataTable.Rows[i].Field<DateTime?>("qmqCreatedDate").HasValue) ? new DateTime?(DateTime.Parse("01/01/1900")) : dataTable.Rows[i].Field<DateTime?>("qmqCreatedDate"));
				bOMQuoteQuantityDto.UniqueID = dataTable.Rows[i].Field<Guid>("qmqUniqueID");
				bOMQuoteQuantityDto.RowVersion = dataTable.Rows[0].Field<byte[]>("qmqRowVersion");
				collection.Add(bOMQuoteQuantityDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<IList<BOMQuoteQuantityDto>> GetQuoteQuantitiesInfo(string quoteId, string quoteLineId)
	{
		IList<BOMQuoteQuantityDto> list = new List<BOMQuoteQuantityDto>();
		InitializeParameterLists();
		base.filterList.Add("@QuoteID", quoteId);
		bool flag = !string.IsNullOrEmpty(quoteLineId);
		if (flag)
		{
			base.filterList.Add("@QuoteLineID", quoteLineId);
		}
		using DataTable dataTable = GetAsDataTable(GetSelectQuoteQuantitiesQuery(flag), base.filterList, null);
		if (dataTable == null || dataTable.Rows.Count <= 0)
		{
			return Task.FromResult(list);
		}
		foreach (DataRow row in dataTable.Rows)
		{
			BOMQuoteQuantityDto item = new BOMQuoteQuantityDto
			{
				QuoteID = row.Field<string>("qmqQuoteID"),
				QuoteLineID = row.Field<short>("qmqQuoteLineID"),
				QuoteQuantityID = row.Field<byte>("qmqQuoteQuantityID"),
				QuoteQuantity = row.Field<decimal>("qmqQuoteQuantity"),
				ScrapPercent = row.Field<decimal>("qmqScrapPercent"),
				TotalRunQuantity = row.Field<decimal>("qmqTotalRunQuantity"),
				QuoteMarkupType = row.Field<byte>("qmqQuoteMarkupType"),
				PurchaseToOrder = row.Field<bool>("qmqPurchaseToOrder"),
				SetupHours = row.Field<decimal>("qmqSetupHours"),
				ProductionHours = row.Field<decimal>("qmqProductionHours"),
				MaterialCost = row.Field<decimal>("qmqMaterialCost"),
				MaterialMarkupPercent = row.Field<decimal>("qmqMaterialMarkupPercent"),
				MaterialPrice = row.Field<decimal>("qmqMaterialPrice"),
				SubcontractPrice = row.Field<decimal>("qmqSubcontractPrice"),
				LaborCost = row.Field<decimal>("qmqLaborCost"),
				LaborMarkupPercent = row.Field<decimal>("qmqLaborMarkupPercent"),
				LaborPrice = row.Field<decimal>("qmqLaborPrice"),
				OverheadCost = row.Field<decimal>("qmqOverheadCost"),
				OverheadMarkupPercent = row.Field<decimal>("qmqOverheadMarkupPercent"),
				OverheadPrice = row.Field<decimal>("qmqOverheadPrice"),
				QuotingPrice = row.Field<decimal>("qmqQuotingPrice"),
				PurchaseUnitCostBase = row.Field<decimal>("qmqPurchaseUnitCostBase"),
				PurchaseToOrderCost = row.Field<decimal>("qmqPurchaseToOrderCost"),
				PurToOrderMarkupPercent = row.Field<decimal>("qmqPurToOrderMarkupPercent"),
				PurchaseToOrderPrice = row.Field<decimal>("qmqPurchaseToOrderPrice"),
				AdditionalCostAmount = row.Field<decimal>("qmqAdditionalCostAmount"),
				AdditionalMarkupPercent = row.Field<decimal>("qmqAdditionalMarkupPercent"),
				AdditionalCostPrice = row.Field<decimal>("qmqAdditionalCostPrice"),
				TotalCost = row.Field<decimal>("qmqTotalCost"),
				TotalPrice = row.Field<decimal>("qmqTotalPrice"),
				TotalUnitCost = row.Field<decimal>("qmqTotalUnitCost"),
				TotalMarkupPercent = row.Field<decimal>("qmqTotalMarkupPercent"),
				TotalUnitPrice = row.Field<decimal>("qmqTotalUnitPrice"),
				CalculatedUnitPrice = row.Field<decimal>("qmqCalculatedUnitPrice"),
				FullRevisedUnitPriceForeign = row.Field<decimal>("qmqFullRevisedUnitPriceForeign"),
				DiscountPercent = row.Field<decimal>("qmqDiscountPercent"),
				UnitDiscountForeign = row.Field<decimal>("qmqUnitDiscountForeign"),
				RevisedUnitPriceForeign = row.Field<decimal>("qmqRevisedUnitPriceForeign"),
				AdditionalChargeForeign = row.Field<decimal>("qmqAdditionalChargeForeign"),
				AdditionalChargeDescription = row.Field<string>("qmqAdditionalChargeDescription"),
				LeadTime = row.Field<string>("qmqLeadTime"),
				Closed = row.Field<bool>("qmqClosed"),
				CreatedBy = row.Field<string>("qmqCreatedBy"),
				CreatedDate = ((!row.Field<DateTime?>("qmqCreatedDate").HasValue) ? new DateTime?(DateTime.Parse("01/01/1900")) : row.Field<DateTime?>("qmqCreatedDate")),
				UniqueID = row.Field<Guid>("qmqUniqueID"),
				RowVersion = row.Field<byte[]>("qmqRowVersion")
			};
			list.Add(item);
		}
		return Task.FromResult(list);
	}

	private string GetSelectQuoteQuantitiesQuery(bool includeAdditionalCondition)
	{
		string text = "SELECT qmqQuoteID, qmqQuoteLineID, qmqQuoteQuantityID, qmqQuoteQuantity, \r\n                                qmqScrapPercent, qmqTotalRunQuantity, qmqQuoteMarkupType, qmqPurchaseToOrder, \r\n                                qmqSetupHours, qmqProductionHours, qmqMaterialCost, qmqMaterialMarkupPercent, \r\n                                qmqMaterialPrice, qmqSubcontractPrice, qmqLaborCost, qmqLaborMarkupPercent,\r\n                                qmqLaborPrice, qmqOverheadCost, qmqOverheadMarkupPercent, qmqOverheadPrice, \r\n                                qmqQuotingPrice, qmqPurchaseUnitCostBase, qmqPurchaseToOrderCost, qmqPurToOrderMarkupPercent, \r\n                                qmqPurchaseToOrderPrice, qmqAdditionalCostAmount, qmqAdditionalMarkupPercent, \r\n                                qmqAdditionalCostPrice, qmqTotalCost, qmqTotalPrice, qmqTotalUnitCost, qmqTotalMarkupPercent, \r\n                                qmqTotalUnitPrice, qmqCalculatedUnitPrice, qmqFullRevisedUnitPriceForeign, qmqDiscountPercent, \r\n                                qmqUnitDiscountForeign, qmqRevisedUnitPriceForeign, qmqAdditionalChargeForeign, \r\n                                qmqAdditionalChargeDescription, qmqLeadTime, qmqClosed, qmqCreatedBy, qmqCreatedDate, \r\n                                qmqUniqueID, qmqRowVersion\r\n                         FROM QuoteQuantities\r\n                         WHERE qmqQuoteID = @QuoteID";
		if (includeAdditionalCondition)
		{
			text += " AND qmqQuoteLineID = @QuoteLineID;";
		}
		return text;
	}
}
