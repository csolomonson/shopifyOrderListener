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

public class ERPARInvoiceLineRepository : APIBaseRepository, IERPARInvoiceLineRepository, IAPIBaseRepository, IDisposable
{
	public ERPARInvoiceLineRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesARInvoiceLineExist(Guid aRInvoiceLineId)
	{
		InitializeParameterLists();
		base.filterList.Add("arlUniqueID|C", aRInvoiceLineId);
		base.selectList.Add("arlUniqueID");
		return Task.FromResult(GetAsObject("ARInvoiceLines", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPARInvoiceLineInformationDto>> GetAllARInvoiceLines(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPARInvoiceLineInformationDto> collection = new List<ERPARInvoiceLineInformationDto>();
		InitializeParameterLists();
		string[] array = new string[108]
		{
			"arlActualTotalCostOfGoodsSold", "arlActualTotalLaborCost", "arlActualTotalMaterialCost", "arlActualTotalOverheadCost", "arlActualTotalSubcontractCost", "arlActualUnitCostOfGoodsSold", "arlActualUnitLaborCost", "arlActualUnitMaterialCost", "arlActualUnitOverheadCost", "arlActualUnitSubcontractCost",
			"arlAmtForResellerCommission", "arlAmtForSalesCommission", "arlArInvoiceID", "arlArRecurringInvoiceID", "arlArRecurringInvoiceLineID", "arlAssetAdjustmentID", "arlAssetID", "arlCallID", "arlCogsCalculatedDate", "arlCommissionAmount",
			"arlCommissionRate", "arlCreatedBy", "arlCreatedDate", "arlCustomerPo", "arlDepositAmountBase", "arlDepositAmountForeign", "arlDepositBalanceBase", "arlDepositBalanceForeign", "arlDepositInvoiceID", "arlDepositInvoiceLineID",
			"arlDepositTransferredBase", "arlDepositTransferredForeign", "arlDiscountPercent", "arlUniqueID", "arlEstTotalCostOfGoodsSold", "arlEstTotalLaborCost", "arlEstTotalMaterialCost", "arlEstTotalOverheadCost", "arlEstTotalSubcontractCost", "arlEstUnitCostOfGoodsSold",
			"arlEstUnitLaborCost", "arlEstUnitMaterialCost", "arlEstUnitOverheadCost", "arlEstUnitSubcontractCost", "arlExtendedDiscountBase", "arlExtendedDiscountForeign", "arlExtendedPriceBase", "arlExtendedPriceForeign", "arlFinanceSourceInvoiceID", "arlFreightAmountBase",
			"arlFreightAmountForeign", "arlFullExtendedPriceBase", "arlFullExtendedPriceForeign", "arlFullUnitPriceBase", "arlFullUnitPriceForeign", "arlInvoiceQuantity", "arlAvalaraIgnoreLine", "arlCogsPostedToGl", "arlDeliveryInvoicedComplete", "arlDepositLine",
			"arlIncludeTaxInRetention", "arlIntraCompanyPosted", "arlPayCommission", "arlPostedToGl", "arlRetention", "arlJobAssemblyID", "arlJobID", "arlJobMaterialID", "arlLineType", "arlNonTaxReasonID",
			"arlOrderQuantity", "arlOrgPartID", "arlOrgPartShortDescription", "arlPartGroupID", "arlPartID", "arlPartLongDescriptionRtf", "arlPartLongDescriptionText", "arlPartRevisionID", "arlPartShortDescription", "arlProjectAreaID",
			"arlProjectID", "arlRetentionAmountBase", "arlRetentionAmountForeign", "arlRetentionDueDate", "arlRetentionPercent", "arlRmaClaimID", "arlRmaClaimLineID", "arlRmaReceiptID", "arlRmaReceiptLineID", "arlRowVersion",
			"arlSalesOrderDeliveryID", "arlSalesOrderID", "arlSalesOrderLineID", "arlSecondTaxAmountBase", "arlSecondTaxAmountForeign", "arlSecondTaxCodeID", "arlArInvoiceLineID", "arlShipmentID", "arlShipmentLineID", "arlTaxAmountBase",
			"arlTaxAmountForeign", "arlTaxCodeID", "arlTaxDate", "arlUnitDiscountBase", "arlUnitDiscountForeign", "arlUnitOfMeasure", "arlUnitPriceBase", "arlUnitPriceForeign"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("ARInvoiceLines");
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
		using (DataTable dataTable = GetAsDataTable("ARInvoiceLines", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPARInvoiceLineInformationDto eRPARInvoiceLineInformationDto = new ERPARInvoiceLineInformationDto();
				eRPARInvoiceLineInformationDto.arlActualTotalCostOfGoodsSold = dataTable.Rows[i].Field<decimal>("arlActualTotalCostOfGoodsSold");
				eRPARInvoiceLineInformationDto.arlActualTotalLaborCost = dataTable.Rows[i].Field<decimal>("arlActualTotalLaborCost");
				eRPARInvoiceLineInformationDto.arlActualTotalMaterialCost = dataTable.Rows[i].Field<decimal>("arlActualTotalMaterialCost");
				eRPARInvoiceLineInformationDto.arlActualTotalOverheadCost = dataTable.Rows[i].Field<decimal>("arlActualTotalOverheadCost");
				eRPARInvoiceLineInformationDto.arlActualTotalSubcontractCost = dataTable.Rows[i].Field<decimal>("arlActualTotalSubcontractCost");
				eRPARInvoiceLineInformationDto.arlActualUnitCostOfGoodsSold = dataTable.Rows[i].Field<decimal>("arlActualUnitCostOfGoodsSold");
				eRPARInvoiceLineInformationDto.arlActualUnitLaborCost = dataTable.Rows[i].Field<decimal>("arlActualUnitLaborCost");
				eRPARInvoiceLineInformationDto.arlActualUnitMaterialCost = dataTable.Rows[i].Field<decimal>("arlActualUnitMaterialCost");
				eRPARInvoiceLineInformationDto.arlActualUnitOverheadCost = dataTable.Rows[i].Field<decimal>("arlActualUnitOverheadCost");
				eRPARInvoiceLineInformationDto.arlActualUnitSubcontractCost = dataTable.Rows[i].Field<decimal>("arlActualUnitSubcontractCost");
				eRPARInvoiceLineInformationDto.arlAmtForResellerCommission = dataTable.Rows[i].Field<decimal>("arlAmtForResellerCommission");
				eRPARInvoiceLineInformationDto.arlAmtForSalesCommission = dataTable.Rows[i].Field<decimal>("arlAmtForSalesCommission");
				eRPARInvoiceLineInformationDto.arlArInvoiceID = dataTable.Rows[i].Field<string>("arlArInvoiceID");
				eRPARInvoiceLineInformationDto.arlArRecurringInvoiceID = dataTable.Rows[i].Field<int>("arlArRecurringInvoiceID");
				eRPARInvoiceLineInformationDto.arlArRecurringInvoiceLineID = dataTable.Rows[i].Field<short>("arlArRecurringInvoiceLineID");
				eRPARInvoiceLineInformationDto.arlAssetAdjustmentID = dataTable.Rows[i].Field<int>("arlAssetAdjustmentID");
				eRPARInvoiceLineInformationDto.arlAssetID = dataTable.Rows[i].Field<string>("arlAssetID");
				eRPARInvoiceLineInformationDto.arlCallID = dataTable.Rows[i].Field<string>("arlCallID");
				eRPARInvoiceLineInformationDto.arlCogsCalculatedDate = dataTable.Rows[i].Field<DateTime?>("arlCogsCalculatedDate");
				eRPARInvoiceLineInformationDto.arlCommissionAmount = dataTable.Rows[i].Field<decimal>("arlCommissionAmount");
				eRPARInvoiceLineInformationDto.arlCommissionRate = dataTable.Rows[i].Field<decimal>("arlCommissionRate");
				eRPARInvoiceLineInformationDto.arlCreatedBy = dataTable.Rows[i].Field<string>("arlCreatedBy");
				eRPARInvoiceLineInformationDto.arlCreatedDate = dataTable.Rows[i].Field<DateTime?>("arlCreatedDate");
				eRPARInvoiceLineInformationDto.arlCustomerPo = dataTable.Rows[i].Field<string>("arlCustomerPo");
				eRPARInvoiceLineInformationDto.arlDepositAmountBase = dataTable.Rows[i].Field<decimal>("arlDepositAmountBase");
				eRPARInvoiceLineInformationDto.arlDepositAmountForeign = dataTable.Rows[i].Field<decimal>("arlDepositAmountForeign");
				eRPARInvoiceLineInformationDto.arlDepositBalanceBase = dataTable.Rows[i].Field<decimal>("arlDepositBalanceBase");
				eRPARInvoiceLineInformationDto.arlDepositBalanceForeign = dataTable.Rows[i].Field<decimal>("arlDepositBalanceForeign");
				eRPARInvoiceLineInformationDto.arlDepositInvoiceID = dataTable.Rows[i].Field<string>("arlDepositInvoiceID");
				eRPARInvoiceLineInformationDto.arlDepositInvoiceLineID = dataTable.Rows[i].Field<short>("arlDepositInvoiceLineID");
				eRPARInvoiceLineInformationDto.arlDepositTransferredBase = dataTable.Rows[i].Field<decimal>("arlDepositTransferredBase");
				eRPARInvoiceLineInformationDto.arlDepositTransferredForeign = dataTable.Rows[i].Field<decimal>("arlDepositTransferredForeign");
				eRPARInvoiceLineInformationDto.arlDiscountPercent = dataTable.Rows[i].Field<decimal>("arlDiscountPercent");
				eRPARInvoiceLineInformationDto.arlUniqueID = dataTable.Rows[i].Field<Guid>("arlUniqueID");
				eRPARInvoiceLineInformationDto.arlEstTotalCostOfGoodsSold = dataTable.Rows[i].Field<decimal>("arlEstTotalCostOfGoodsSold");
				eRPARInvoiceLineInformationDto.arlEstTotalLaborCost = dataTable.Rows[i].Field<decimal>("arlEstTotalLaborCost");
				eRPARInvoiceLineInformationDto.arlEstTotalMaterialCost = dataTable.Rows[i].Field<decimal>("arlEstTotalMaterialCost");
				eRPARInvoiceLineInformationDto.arlEstTotalOverheadCost = dataTable.Rows[i].Field<decimal>("arlEstTotalOverheadCost");
				eRPARInvoiceLineInformationDto.arlEstTotalSubcontractCost = dataTable.Rows[i].Field<decimal>("arlEstTotalSubcontractCost");
				eRPARInvoiceLineInformationDto.arlEstUnitCostOfGoodsSold = dataTable.Rows[i].Field<decimal>("arlEstUnitCostOfGoodsSold");
				eRPARInvoiceLineInformationDto.arlEstUnitLaborCost = dataTable.Rows[i].Field<decimal>("arlEstUnitLaborCost");
				eRPARInvoiceLineInformationDto.arlEstUnitMaterialCost = dataTable.Rows[i].Field<decimal>("arlEstUnitMaterialCost");
				eRPARInvoiceLineInformationDto.arlEstUnitOverheadCost = dataTable.Rows[i].Field<decimal>("arlEstUnitOverheadCost");
				eRPARInvoiceLineInformationDto.arlEstUnitSubcontractCost = dataTable.Rows[i].Field<decimal>("arlEstUnitSubcontractCost");
				eRPARInvoiceLineInformationDto.arlExtendedDiscountBase = dataTable.Rows[i].Field<decimal>("arlExtendedDiscountBase");
				eRPARInvoiceLineInformationDto.arlExtendedDiscountForeign = dataTable.Rows[i].Field<decimal>("arlExtendedDiscountForeign");
				eRPARInvoiceLineInformationDto.arlExtendedPriceBase = dataTable.Rows[i].Field<decimal>("arlExtendedPriceBase");
				eRPARInvoiceLineInformationDto.arlExtendedPriceForeign = dataTable.Rows[i].Field<decimal>("arlExtendedPriceForeign");
				eRPARInvoiceLineInformationDto.arlFinanceSourceInvoiceID = dataTable.Rows[i].Field<string>("arlFinanceSourceInvoiceID");
				eRPARInvoiceLineInformationDto.arlFreightAmountBase = dataTable.Rows[i].Field<decimal>("arlFreightAmountBase");
				eRPARInvoiceLineInformationDto.arlFreightAmountForeign = dataTable.Rows[i].Field<decimal>("arlFreightAmountForeign");
				eRPARInvoiceLineInformationDto.arlFullExtendedPriceBase = dataTable.Rows[i].Field<decimal>("arlFullExtendedPriceBase");
				eRPARInvoiceLineInformationDto.arlFullExtendedPriceForeign = dataTable.Rows[i].Field<decimal>("arlFullExtendedPriceForeign");
				eRPARInvoiceLineInformationDto.arlFullUnitPriceBase = dataTable.Rows[i].Field<decimal>("arlFullUnitPriceBase");
				eRPARInvoiceLineInformationDto.arlFullUnitPriceForeign = dataTable.Rows[i].Field<decimal>("arlFullUnitPriceForeign");
				eRPARInvoiceLineInformationDto.arlInvoiceQuantity = dataTable.Rows[i].Field<decimal>("arlInvoiceQuantity");
				eRPARInvoiceLineInformationDto.arlAvalaraIgnoreLine = dataTable.Rows[i].Field<bool>("arlAvalaraIgnoreLine");
				eRPARInvoiceLineInformationDto.arlCogsPostedToGl = dataTable.Rows[i].Field<bool>("arlCogsPostedToGl");
				eRPARInvoiceLineInformationDto.arlDeliveryInvoicedComplete = dataTable.Rows[i].Field<bool>("arlDeliveryInvoicedComplete");
				eRPARInvoiceLineInformationDto.arlDepositLine = dataTable.Rows[i].Field<bool>("arlDepositLine");
				eRPARInvoiceLineInformationDto.arlIncludeTaxInRetention = dataTable.Rows[i].Field<bool>("arlIncludeTaxInRetention");
				eRPARInvoiceLineInformationDto.arlIntraCompanyPosted = dataTable.Rows[i].Field<bool>("arlIntraCompanyPosted");
				eRPARInvoiceLineInformationDto.arlPayCommission = dataTable.Rows[i].Field<bool>("arlPayCommission");
				eRPARInvoiceLineInformationDto.arlPostedToGl = dataTable.Rows[i].Field<bool>("arlPostedToGl");
				eRPARInvoiceLineInformationDto.arlRetention = dataTable.Rows[i].Field<bool>("arlRetention");
				eRPARInvoiceLineInformationDto.arlJobAssemblyID = dataTable.Rows[i].Field<int>("arlJobAssemblyID");
				eRPARInvoiceLineInformationDto.arlJobID = dataTable.Rows[i].Field<string>("arlJobID");
				eRPARInvoiceLineInformationDto.arlJobMaterialID = dataTable.Rows[i].Field<int>("arlJobMaterialID");
				eRPARInvoiceLineInformationDto.arlLineType = dataTable.Rows[i].Field<byte>("arlLineType");
				eRPARInvoiceLineInformationDto.arlNonTaxReasonID = dataTable.Rows[i].Field<string>("arlNonTaxReasonID");
				eRPARInvoiceLineInformationDto.arlOrderQuantity = dataTable.Rows[i].Field<decimal>("arlOrderQuantity");
				eRPARInvoiceLineInformationDto.arlOrgPartID = dataTable.Rows[i].Field<string>("arlOrgPartID");
				eRPARInvoiceLineInformationDto.arlOrgPartShortDescription = dataTable.Rows[i].Field<string>("arlOrgPartShortDescription");
				eRPARInvoiceLineInformationDto.arlPartGroupID = dataTable.Rows[i].Field<string>("arlPartGroupID");
				eRPARInvoiceLineInformationDto.arlPartID = dataTable.Rows[i].Field<string>("arlPartID");
				eRPARInvoiceLineInformationDto.arlPartLongDescriptionRtf = dataTable.Rows[i].Field<string>("arlPartLongDescriptionRtf");
				eRPARInvoiceLineInformationDto.arlPartLongDescriptionText = dataTable.Rows[i].Field<string>("arlPartLongDescriptionText");
				eRPARInvoiceLineInformationDto.arlPartRevisionID = dataTable.Rows[i].Field<string>("arlPartRevisionID");
				eRPARInvoiceLineInformationDto.arlPartShortDescription = dataTable.Rows[i].Field<string>("arlPartShortDescription");
				eRPARInvoiceLineInformationDto.arlProjectAreaID = dataTable.Rows[i].Field<string>("arlProjectAreaID");
				eRPARInvoiceLineInformationDto.arlProjectID = dataTable.Rows[i].Field<string>("arlProjectID");
				eRPARInvoiceLineInformationDto.arlRetentionAmountBase = dataTable.Rows[i].Field<decimal>("arlRetentionAmountBase");
				eRPARInvoiceLineInformationDto.arlRetentionAmountForeign = dataTable.Rows[i].Field<decimal>("arlRetentionAmountForeign");
				eRPARInvoiceLineInformationDto.arlRetentionDueDate = dataTable.Rows[i].Field<DateTime?>("arlRetentionDueDate");
				eRPARInvoiceLineInformationDto.arlRetentionPercent = dataTable.Rows[i].Field<decimal>("arlRetentionPercent");
				eRPARInvoiceLineInformationDto.arlRmaClaimID = dataTable.Rows[i].Field<string>("arlRmaClaimID");
				eRPARInvoiceLineInformationDto.arlRmaClaimLineID = dataTable.Rows[i].Field<short>("arlRmaClaimLineID");
				eRPARInvoiceLineInformationDto.arlRmaReceiptID = dataTable.Rows[i].Field<string>("arlRmaReceiptID");
				eRPARInvoiceLineInformationDto.arlRmaReceiptLineID = dataTable.Rows[i].Field<short>("arlRmaReceiptLineID");
				eRPARInvoiceLineInformationDto.arlRowVersion = dataTable.Rows[i].Field<byte[]>("arlRowVersion");
				eRPARInvoiceLineInformationDto.arlSalesOrderDeliveryID = dataTable.Rows[i].Field<short>("arlSalesOrderDeliveryID");
				eRPARInvoiceLineInformationDto.arlSalesOrderID = dataTable.Rows[i].Field<string>("arlSalesOrderID");
				eRPARInvoiceLineInformationDto.arlSalesOrderLineID = dataTable.Rows[i].Field<short>("arlSalesOrderLineID");
				eRPARInvoiceLineInformationDto.arlSecondTaxAmountBase = dataTable.Rows[i].Field<decimal>("arlSecondTaxAmountBase");
				eRPARInvoiceLineInformationDto.arlSecondTaxAmountForeign = dataTable.Rows[i].Field<decimal>("arlSecondTaxAmountForeign");
				eRPARInvoiceLineInformationDto.arlSecondTaxCodeID = dataTable.Rows[i].Field<string>("arlSecondTaxCodeID");
				eRPARInvoiceLineInformationDto.arlArInvoiceLineID = dataTable.Rows[i].Field<short>("arlArInvoiceLineID");
				eRPARInvoiceLineInformationDto.arlShipmentID = dataTable.Rows[i].Field<string>("arlShipmentID");
				eRPARInvoiceLineInformationDto.arlShipmentLineID = dataTable.Rows[i].Field<short>("arlShipmentLineID");
				eRPARInvoiceLineInformationDto.arlTaxAmountBase = dataTable.Rows[i].Field<decimal>("arlTaxAmountBase");
				eRPARInvoiceLineInformationDto.arlTaxAmountForeign = dataTable.Rows[i].Field<decimal>("arlTaxAmountForeign");
				eRPARInvoiceLineInformationDto.arlTaxCodeID = dataTable.Rows[i].Field<string>("arlTaxCodeID");
				eRPARInvoiceLineInformationDto.arlTaxDate = dataTable.Rows[i].Field<DateTime?>("arlTaxDate");
				eRPARInvoiceLineInformationDto.arlUnitDiscountBase = dataTable.Rows[i].Field<decimal>("arlUnitDiscountBase");
				eRPARInvoiceLineInformationDto.arlUnitDiscountForeign = dataTable.Rows[i].Field<decimal>("arlUnitDiscountForeign");
				eRPARInvoiceLineInformationDto.arlUnitOfMeasure = dataTable.Rows[i].Field<string>("arlUnitOfMeasure");
				eRPARInvoiceLineInformationDto.arlUnitPriceBase = dataTable.Rows[i].Field<decimal>("arlUnitPriceBase");
				eRPARInvoiceLineInformationDto.arlUnitPriceForeign = dataTable.Rows[i].Field<decimal>("arlUnitPriceForeign");
				eRPARInvoiceLineInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPARInvoiceLineInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPARInvoiceLineInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPARInvoiceLineInformationDto> GetARInvoiceLine(Guid aRInvoiceLineId)
	{
		ERPARInvoiceLineInformationDto eRPARInvoiceLineInformationDto = new ERPARInvoiceLineInformationDto();
		InitializeParameterLists();
		string[] collection = new string[108]
		{
			"arlActualTotalCostOfGoodsSold", "arlActualTotalLaborCost", "arlActualTotalMaterialCost", "arlActualTotalOverheadCost", "arlActualTotalSubcontractCost", "arlActualUnitCostOfGoodsSold", "arlActualUnitLaborCost", "arlActualUnitMaterialCost", "arlActualUnitOverheadCost", "arlActualUnitSubcontractCost",
			"arlAmtForResellerCommission", "arlAmtForSalesCommission", "arlArInvoiceID", "arlArRecurringInvoiceID", "arlArRecurringInvoiceLineID", "arlAssetAdjustmentID", "arlAssetID", "arlCallID", "arlCogsCalculatedDate", "arlCommissionAmount",
			"arlCommissionRate", "arlCreatedBy", "arlCreatedDate", "arlCustomerPo", "arlDepositAmountBase", "arlDepositAmountForeign", "arlDepositBalanceBase", "arlDepositBalanceForeign", "arlDepositInvoiceID", "arlDepositInvoiceLineID",
			"arlDepositTransferredBase", "arlDepositTransferredForeign", "arlDiscountPercent", "arlUniqueID", "arlEstTotalCostOfGoodsSold", "arlEstTotalLaborCost", "arlEstTotalMaterialCost", "arlEstTotalOverheadCost", "arlEstTotalSubcontractCost", "arlEstUnitCostOfGoodsSold",
			"arlEstUnitLaborCost", "arlEstUnitMaterialCost", "arlEstUnitOverheadCost", "arlEstUnitSubcontractCost", "arlExtendedDiscountBase", "arlExtendedDiscountForeign", "arlExtendedPriceBase", "arlExtendedPriceForeign", "arlFinanceSourceInvoiceID", "arlFreightAmountBase",
			"arlFreightAmountForeign", "arlFullExtendedPriceBase", "arlFullExtendedPriceForeign", "arlFullUnitPriceBase", "arlFullUnitPriceForeign", "arlInvoiceQuantity", "arlAvalaraIgnoreLine", "arlCogsPostedToGl", "arlDeliveryInvoicedComplete", "arlDepositLine",
			"arlIncludeTaxInRetention", "arlIntraCompanyPosted", "arlPayCommission", "arlPostedToGl", "arlRetention", "arlJobAssemblyID", "arlJobID", "arlJobMaterialID", "arlLineType", "arlNonTaxReasonID",
			"arlOrderQuantity", "arlOrgPartID", "arlOrgPartShortDescription", "arlPartGroupID", "arlPartID", "arlPartLongDescriptionRtf", "arlPartLongDescriptionText", "arlPartRevisionID", "arlPartShortDescription", "arlProjectAreaID",
			"arlProjectID", "arlRetentionAmountBase", "arlRetentionAmountForeign", "arlRetentionDueDate", "arlRetentionPercent", "arlRmaClaimID", "arlRmaClaimLineID", "arlRmaReceiptID", "arlRmaReceiptLineID", "arlRowVersion",
			"arlSalesOrderDeliveryID", "arlSalesOrderID", "arlSalesOrderLineID", "arlSecondTaxAmountBase", "arlSecondTaxAmountForeign", "arlSecondTaxCodeID", "arlArInvoiceLineID", "arlShipmentID", "arlShipmentLineID", "arlTaxAmountBase",
			"arlTaxAmountForeign", "arlTaxCodeID", "arlTaxDate", "arlUnitDiscountBase", "arlUnitDiscountForeign", "arlUnitOfMeasure", "arlUnitPriceBase", "arlUnitPriceForeign"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("arlUniqueID|C", aRInvoiceLineId);
		AddCustomFieldsToSelectList("ARInvoiceLines");
		using (DataTable dataTable = GetAsDataTable("ARInvoiceLines", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPARInvoiceLineInformationDto);
			}
			eRPARInvoiceLineInformationDto.arlActualTotalCostOfGoodsSold = dataTable.Rows[0].Field<decimal>("arlActualTotalCostOfGoodsSold");
			eRPARInvoiceLineInformationDto.arlActualTotalLaborCost = dataTable.Rows[0].Field<decimal>("arlActualTotalLaborCost");
			eRPARInvoiceLineInformationDto.arlActualTotalMaterialCost = dataTable.Rows[0].Field<decimal>("arlActualTotalMaterialCost");
			eRPARInvoiceLineInformationDto.arlActualTotalOverheadCost = dataTable.Rows[0].Field<decimal>("arlActualTotalOverheadCost");
			eRPARInvoiceLineInformationDto.arlActualTotalSubcontractCost = dataTable.Rows[0].Field<decimal>("arlActualTotalSubcontractCost");
			eRPARInvoiceLineInformationDto.arlActualUnitCostOfGoodsSold = dataTable.Rows[0].Field<decimal>("arlActualUnitCostOfGoodsSold");
			eRPARInvoiceLineInformationDto.arlActualUnitLaborCost = dataTable.Rows[0].Field<decimal>("arlActualUnitLaborCost");
			eRPARInvoiceLineInformationDto.arlActualUnitMaterialCost = dataTable.Rows[0].Field<decimal>("arlActualUnitMaterialCost");
			eRPARInvoiceLineInformationDto.arlActualUnitOverheadCost = dataTable.Rows[0].Field<decimal>("arlActualUnitOverheadCost");
			eRPARInvoiceLineInformationDto.arlActualUnitSubcontractCost = dataTable.Rows[0].Field<decimal>("arlActualUnitSubcontractCost");
			eRPARInvoiceLineInformationDto.arlAmtForResellerCommission = dataTable.Rows[0].Field<decimal>("arlAmtForResellerCommission");
			eRPARInvoiceLineInformationDto.arlAmtForSalesCommission = dataTable.Rows[0].Field<decimal>("arlAmtForSalesCommission");
			eRPARInvoiceLineInformationDto.arlArInvoiceID = dataTable.Rows[0].Field<string>("arlArInvoiceID");
			eRPARInvoiceLineInformationDto.arlArRecurringInvoiceID = dataTable.Rows[0].Field<int>("arlArRecurringInvoiceID");
			eRPARInvoiceLineInformationDto.arlArRecurringInvoiceLineID = dataTable.Rows[0].Field<short>("arlArRecurringInvoiceLineID");
			eRPARInvoiceLineInformationDto.arlAssetAdjustmentID = dataTable.Rows[0].Field<int>("arlAssetAdjustmentID");
			eRPARInvoiceLineInformationDto.arlAssetID = dataTable.Rows[0].Field<string>("arlAssetID");
			eRPARInvoiceLineInformationDto.arlCallID = dataTable.Rows[0].Field<string>("arlCallID");
			eRPARInvoiceLineInformationDto.arlCogsCalculatedDate = dataTable.Rows[0].Field<DateTime?>("arlCogsCalculatedDate");
			eRPARInvoiceLineInformationDto.arlCommissionAmount = dataTable.Rows[0].Field<decimal>("arlCommissionAmount");
			eRPARInvoiceLineInformationDto.arlCommissionRate = dataTable.Rows[0].Field<decimal>("arlCommissionRate");
			eRPARInvoiceLineInformationDto.arlCreatedBy = dataTable.Rows[0].Field<string>("arlCreatedBy");
			eRPARInvoiceLineInformationDto.arlCreatedDate = dataTable.Rows[0].Field<DateTime?>("arlCreatedDate");
			eRPARInvoiceLineInformationDto.arlCustomerPo = dataTable.Rows[0].Field<string>("arlCustomerPo");
			eRPARInvoiceLineInformationDto.arlDepositAmountBase = dataTable.Rows[0].Field<decimal>("arlDepositAmountBase");
			eRPARInvoiceLineInformationDto.arlDepositAmountForeign = dataTable.Rows[0].Field<decimal>("arlDepositAmountForeign");
			eRPARInvoiceLineInformationDto.arlDepositBalanceBase = dataTable.Rows[0].Field<decimal>("arlDepositBalanceBase");
			eRPARInvoiceLineInformationDto.arlDepositBalanceForeign = dataTable.Rows[0].Field<decimal>("arlDepositBalanceForeign");
			eRPARInvoiceLineInformationDto.arlDepositInvoiceID = dataTable.Rows[0].Field<string>("arlDepositInvoiceID");
			eRPARInvoiceLineInformationDto.arlDepositInvoiceLineID = dataTable.Rows[0].Field<short>("arlDepositInvoiceLineID");
			eRPARInvoiceLineInformationDto.arlDepositTransferredBase = dataTable.Rows[0].Field<decimal>("arlDepositTransferredBase");
			eRPARInvoiceLineInformationDto.arlDepositTransferredForeign = dataTable.Rows[0].Field<decimal>("arlDepositTransferredForeign");
			eRPARInvoiceLineInformationDto.arlDiscountPercent = dataTable.Rows[0].Field<decimal>("arlDiscountPercent");
			eRPARInvoiceLineInformationDto.arlUniqueID = dataTable.Rows[0].Field<Guid>("arlUniqueID");
			eRPARInvoiceLineInformationDto.arlEstTotalCostOfGoodsSold = dataTable.Rows[0].Field<decimal>("arlEstTotalCostOfGoodsSold");
			eRPARInvoiceLineInformationDto.arlEstTotalLaborCost = dataTable.Rows[0].Field<decimal>("arlEstTotalLaborCost");
			eRPARInvoiceLineInformationDto.arlEstTotalMaterialCost = dataTable.Rows[0].Field<decimal>("arlEstTotalMaterialCost");
			eRPARInvoiceLineInformationDto.arlEstTotalOverheadCost = dataTable.Rows[0].Field<decimal>("arlEstTotalOverheadCost");
			eRPARInvoiceLineInformationDto.arlEstTotalSubcontractCost = dataTable.Rows[0].Field<decimal>("arlEstTotalSubcontractCost");
			eRPARInvoiceLineInformationDto.arlEstUnitCostOfGoodsSold = dataTable.Rows[0].Field<decimal>("arlEstUnitCostOfGoodsSold");
			eRPARInvoiceLineInformationDto.arlEstUnitLaborCost = dataTable.Rows[0].Field<decimal>("arlEstUnitLaborCost");
			eRPARInvoiceLineInformationDto.arlEstUnitMaterialCost = dataTable.Rows[0].Field<decimal>("arlEstUnitMaterialCost");
			eRPARInvoiceLineInformationDto.arlEstUnitOverheadCost = dataTable.Rows[0].Field<decimal>("arlEstUnitOverheadCost");
			eRPARInvoiceLineInformationDto.arlEstUnitSubcontractCost = dataTable.Rows[0].Field<decimal>("arlEstUnitSubcontractCost");
			eRPARInvoiceLineInformationDto.arlExtendedDiscountBase = dataTable.Rows[0].Field<decimal>("arlExtendedDiscountBase");
			eRPARInvoiceLineInformationDto.arlExtendedDiscountForeign = dataTable.Rows[0].Field<decimal>("arlExtendedDiscountForeign");
			eRPARInvoiceLineInformationDto.arlExtendedPriceBase = dataTable.Rows[0].Field<decimal>("arlExtendedPriceBase");
			eRPARInvoiceLineInformationDto.arlExtendedPriceForeign = dataTable.Rows[0].Field<decimal>("arlExtendedPriceForeign");
			eRPARInvoiceLineInformationDto.arlFinanceSourceInvoiceID = dataTable.Rows[0].Field<string>("arlFinanceSourceInvoiceID");
			eRPARInvoiceLineInformationDto.arlFreightAmountBase = dataTable.Rows[0].Field<decimal>("arlFreightAmountBase");
			eRPARInvoiceLineInformationDto.arlFreightAmountForeign = dataTable.Rows[0].Field<decimal>("arlFreightAmountForeign");
			eRPARInvoiceLineInformationDto.arlFullExtendedPriceBase = dataTable.Rows[0].Field<decimal>("arlFullExtendedPriceBase");
			eRPARInvoiceLineInformationDto.arlFullExtendedPriceForeign = dataTable.Rows[0].Field<decimal>("arlFullExtendedPriceForeign");
			eRPARInvoiceLineInformationDto.arlFullUnitPriceBase = dataTable.Rows[0].Field<decimal>("arlFullUnitPriceBase");
			eRPARInvoiceLineInformationDto.arlFullUnitPriceForeign = dataTable.Rows[0].Field<decimal>("arlFullUnitPriceForeign");
			eRPARInvoiceLineInformationDto.arlInvoiceQuantity = dataTable.Rows[0].Field<decimal>("arlInvoiceQuantity");
			eRPARInvoiceLineInformationDto.arlAvalaraIgnoreLine = dataTable.Rows[0].Field<bool>("arlAvalaraIgnoreLine");
			eRPARInvoiceLineInformationDto.arlCogsPostedToGl = dataTable.Rows[0].Field<bool>("arlCogsPostedToGl");
			eRPARInvoiceLineInformationDto.arlDeliveryInvoicedComplete = dataTable.Rows[0].Field<bool>("arlDeliveryInvoicedComplete");
			eRPARInvoiceLineInformationDto.arlDepositLine = dataTable.Rows[0].Field<bool>("arlDepositLine");
			eRPARInvoiceLineInformationDto.arlIncludeTaxInRetention = dataTable.Rows[0].Field<bool>("arlIncludeTaxInRetention");
			eRPARInvoiceLineInformationDto.arlIntraCompanyPosted = dataTable.Rows[0].Field<bool>("arlIntraCompanyPosted");
			eRPARInvoiceLineInformationDto.arlPayCommission = dataTable.Rows[0].Field<bool>("arlPayCommission");
			eRPARInvoiceLineInformationDto.arlPostedToGl = dataTable.Rows[0].Field<bool>("arlPostedToGl");
			eRPARInvoiceLineInformationDto.arlRetention = dataTable.Rows[0].Field<bool>("arlRetention");
			eRPARInvoiceLineInformationDto.arlJobAssemblyID = dataTable.Rows[0].Field<int>("arlJobAssemblyID");
			eRPARInvoiceLineInformationDto.arlJobID = dataTable.Rows[0].Field<string>("arlJobID");
			eRPARInvoiceLineInformationDto.arlJobMaterialID = dataTable.Rows[0].Field<int>("arlJobMaterialID");
			eRPARInvoiceLineInformationDto.arlLineType = dataTable.Rows[0].Field<byte>("arlLineType");
			eRPARInvoiceLineInformationDto.arlNonTaxReasonID = dataTable.Rows[0].Field<string>("arlNonTaxReasonID");
			eRPARInvoiceLineInformationDto.arlOrderQuantity = dataTable.Rows[0].Field<decimal>("arlOrderQuantity");
			eRPARInvoiceLineInformationDto.arlOrgPartID = dataTable.Rows[0].Field<string>("arlOrgPartID");
			eRPARInvoiceLineInformationDto.arlOrgPartShortDescription = dataTable.Rows[0].Field<string>("arlOrgPartShortDescription");
			eRPARInvoiceLineInformationDto.arlPartGroupID = dataTable.Rows[0].Field<string>("arlPartGroupID");
			eRPARInvoiceLineInformationDto.arlPartID = dataTable.Rows[0].Field<string>("arlPartID");
			eRPARInvoiceLineInformationDto.arlPartLongDescriptionRtf = dataTable.Rows[0].Field<string>("arlPartLongDescriptionRtf");
			eRPARInvoiceLineInformationDto.arlPartLongDescriptionText = dataTable.Rows[0].Field<string>("arlPartLongDescriptionText");
			eRPARInvoiceLineInformationDto.arlPartRevisionID = dataTable.Rows[0].Field<string>("arlPartRevisionID");
			eRPARInvoiceLineInformationDto.arlPartShortDescription = dataTable.Rows[0].Field<string>("arlPartShortDescription");
			eRPARInvoiceLineInformationDto.arlProjectAreaID = dataTable.Rows[0].Field<string>("arlProjectAreaID");
			eRPARInvoiceLineInformationDto.arlProjectID = dataTable.Rows[0].Field<string>("arlProjectID");
			eRPARInvoiceLineInformationDto.arlRetentionAmountBase = dataTable.Rows[0].Field<decimal>("arlRetentionAmountBase");
			eRPARInvoiceLineInformationDto.arlRetentionAmountForeign = dataTable.Rows[0].Field<decimal>("arlRetentionAmountForeign");
			eRPARInvoiceLineInformationDto.arlRetentionDueDate = dataTable.Rows[0].Field<DateTime?>("arlRetentionDueDate");
			eRPARInvoiceLineInformationDto.arlRetentionPercent = dataTable.Rows[0].Field<decimal>("arlRetentionPercent");
			eRPARInvoiceLineInformationDto.arlRmaClaimID = dataTable.Rows[0].Field<string>("arlRmaClaimID");
			eRPARInvoiceLineInformationDto.arlRmaClaimLineID = dataTable.Rows[0].Field<short>("arlRmaClaimLineID");
			eRPARInvoiceLineInformationDto.arlRmaReceiptID = dataTable.Rows[0].Field<string>("arlRmaReceiptID");
			eRPARInvoiceLineInformationDto.arlRmaReceiptLineID = dataTable.Rows[0].Field<short>("arlRmaReceiptLineID");
			eRPARInvoiceLineInformationDto.arlRowVersion = dataTable.Rows[0].Field<byte[]>("arlRowVersion");
			eRPARInvoiceLineInformationDto.arlSalesOrderDeliveryID = dataTable.Rows[0].Field<short>("arlSalesOrderDeliveryID");
			eRPARInvoiceLineInformationDto.arlSalesOrderID = dataTable.Rows[0].Field<string>("arlSalesOrderID");
			eRPARInvoiceLineInformationDto.arlSalesOrderLineID = dataTable.Rows[0].Field<short>("arlSalesOrderLineID");
			eRPARInvoiceLineInformationDto.arlSecondTaxAmountBase = dataTable.Rows[0].Field<decimal>("arlSecondTaxAmountBase");
			eRPARInvoiceLineInformationDto.arlSecondTaxAmountForeign = dataTable.Rows[0].Field<decimal>("arlSecondTaxAmountForeign");
			eRPARInvoiceLineInformationDto.arlSecondTaxCodeID = dataTable.Rows[0].Field<string>("arlSecondTaxCodeID");
			eRPARInvoiceLineInformationDto.arlArInvoiceLineID = dataTable.Rows[0].Field<short>("arlArInvoiceLineID");
			eRPARInvoiceLineInformationDto.arlShipmentID = dataTable.Rows[0].Field<string>("arlShipmentID");
			eRPARInvoiceLineInformationDto.arlShipmentLineID = dataTable.Rows[0].Field<short>("arlShipmentLineID");
			eRPARInvoiceLineInformationDto.arlTaxAmountBase = dataTable.Rows[0].Field<decimal>("arlTaxAmountBase");
			eRPARInvoiceLineInformationDto.arlTaxAmountForeign = dataTable.Rows[0].Field<decimal>("arlTaxAmountForeign");
			eRPARInvoiceLineInformationDto.arlTaxCodeID = dataTable.Rows[0].Field<string>("arlTaxCodeID");
			eRPARInvoiceLineInformationDto.arlTaxDate = dataTable.Rows[0].Field<DateTime?>("arlTaxDate");
			eRPARInvoiceLineInformationDto.arlUnitDiscountBase = dataTable.Rows[0].Field<decimal>("arlUnitDiscountBase");
			eRPARInvoiceLineInformationDto.arlUnitDiscountForeign = dataTable.Rows[0].Field<decimal>("arlUnitDiscountForeign");
			eRPARInvoiceLineInformationDto.arlUnitOfMeasure = dataTable.Rows[0].Field<string>("arlUnitOfMeasure");
			eRPARInvoiceLineInformationDto.arlUnitPriceBase = dataTable.Rows[0].Field<decimal>("arlUnitPriceBase");
			eRPARInvoiceLineInformationDto.arlUnitPriceForeign = dataTable.Rows[0].Field<decimal>("arlUnitPriceForeign");
			eRPARInvoiceLineInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPARInvoiceLineInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPARInvoiceLineInformationDto);
	}

	public Task<APIValidationInfoDto> SaveARInvoiceLine(ERPARInvoiceLineDto aRInvoiceLine)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM ARInvoiceLines WHERE arlUniqueID = " + M1Util.ConvertToLinq(aRInvoiceLine.arlUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["arlArInvoiceID"] = aRInvoiceLine.arlArInvoiceID.ToUpper();
				dataRow["arlArInvoiceLineID"] = aRInvoiceLine.arlArInvoiceLineID;
				aRInvoiceLine.arlUniqueID = ((aRInvoiceLine.arlUniqueID == Guid.Empty) ? Guid.NewGuid() : aRInvoiceLine.arlUniqueID);
				dataRow["arlUniqueID"] = aRInvoiceLine.arlUniqueID;
				dataRow["arlCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["arlCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The ARInvoiceLine could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (aRInvoiceLine.arlRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the ARInvoiceLine is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["arlRowVersion"], aRInvoiceLine.arlRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the ARInvoiceLine has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the ARInvoiceLine again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["arlActualTotalCostOfGoodsSold"] = aRInvoiceLine.arlActualTotalCostOfGoodsSold;
			dataRow["arlActualTotalLaborCost"] = aRInvoiceLine.arlActualTotalLaborCost;
			dataRow["arlActualTotalMaterialCost"] = aRInvoiceLine.arlActualTotalMaterialCost;
			dataRow["arlActualTotalOverheadCost"] = aRInvoiceLine.arlActualTotalOverheadCost;
			dataRow["arlActualTotalSubcontractCost"] = aRInvoiceLine.arlActualTotalSubcontractCost;
			dataRow["arlActualUnitCostOfGoodsSold"] = aRInvoiceLine.arlActualUnitCostOfGoodsSold;
			dataRow["arlActualUnitLaborCost"] = aRInvoiceLine.arlActualUnitLaborCost;
			dataRow["arlActualUnitMaterialCost"] = aRInvoiceLine.arlActualUnitMaterialCost;
			dataRow["arlActualUnitOverheadCost"] = aRInvoiceLine.arlActualUnitOverheadCost;
			dataRow["arlActualUnitSubcontractCost"] = aRInvoiceLine.arlActualUnitSubcontractCost;
			dataRow["arlAmtForResellerCommission"] = aRInvoiceLine.arlAmtForResellerCommission;
			dataRow["arlAmtForSalesCommission"] = aRInvoiceLine.arlAmtForSalesCommission;
			dataRow["arlArRecurringInvoiceID"] = aRInvoiceLine.arlArRecurringInvoiceID;
			dataRow["arlArRecurringInvoiceLineID"] = aRInvoiceLine.arlArRecurringInvoiceLineID;
			dataRow["arlAssetAdjustmentID"] = aRInvoiceLine.arlAssetAdjustmentID;
			dataRow["arlAssetID"] = aRInvoiceLine.arlAssetID;
			dataRow["arlCallID"] = aRInvoiceLine.arlCallID;
			DataRow dataRow2 = dataRow;
			DateTime? arlCogsCalculatedDate = aRInvoiceLine.arlCogsCalculatedDate;
			dataRow2["arlCogsCalculatedDate"] = (arlCogsCalculatedDate.HasValue ? ((object)arlCogsCalculatedDate.GetValueOrDefault()) : dataRow["arlCogsCalculatedDate"]);
			dataRow["arlCommissionAmount"] = aRInvoiceLine.arlCommissionAmount;
			dataRow["arlCommissionRate"] = aRInvoiceLine.arlCommissionRate;
			dataRow["arlCustomerPo"] = aRInvoiceLine.arlCustomerPo;
			dataRow["arlDepositAmountBase"] = aRInvoiceLine.arlDepositAmountBase;
			dataRow["arlDepositAmountForeign"] = aRInvoiceLine.arlDepositAmountForeign;
			dataRow["arlDepositBalanceBase"] = aRInvoiceLine.arlDepositBalanceBase;
			dataRow["arlDepositBalanceForeign"] = aRInvoiceLine.arlDepositBalanceForeign;
			dataRow["arlDepositInvoiceID"] = aRInvoiceLine.arlDepositInvoiceID;
			dataRow["arlDepositInvoiceLineID"] = aRInvoiceLine.arlDepositInvoiceLineID;
			dataRow["arlDepositTransferredBase"] = aRInvoiceLine.arlDepositTransferredBase;
			dataRow["arlDepositTransferredForeign"] = aRInvoiceLine.arlDepositTransferredForeign;
			dataRow["arlDiscountPercent"] = aRInvoiceLine.arlDiscountPercent;
			dataRow["arlEstTotalCostOfGoodsSold"] = aRInvoiceLine.arlEstTotalCostOfGoodsSold;
			dataRow["arlEstTotalLaborCost"] = aRInvoiceLine.arlEstTotalLaborCost;
			dataRow["arlEstTotalMaterialCost"] = aRInvoiceLine.arlEstTotalMaterialCost;
			dataRow["arlEstTotalOverheadCost"] = aRInvoiceLine.arlEstTotalOverheadCost;
			dataRow["arlEstTotalSubcontractCost"] = aRInvoiceLine.arlEstTotalSubcontractCost;
			dataRow["arlEstUnitCostOfGoodsSold"] = aRInvoiceLine.arlEstUnitCostOfGoodsSold;
			dataRow["arlEstUnitLaborCost"] = aRInvoiceLine.arlEstUnitLaborCost;
			dataRow["arlEstUnitMaterialCost"] = aRInvoiceLine.arlEstUnitMaterialCost;
			dataRow["arlEstUnitOverheadCost"] = aRInvoiceLine.arlEstUnitOverheadCost;
			dataRow["arlEstUnitSubcontractCost"] = aRInvoiceLine.arlEstUnitSubcontractCost;
			dataRow["arlExtendedDiscountBase"] = aRInvoiceLine.arlExtendedDiscountBase;
			dataRow["arlExtendedDiscountForeign"] = aRInvoiceLine.arlExtendedDiscountForeign;
			dataRow["arlExtendedPriceBase"] = aRInvoiceLine.arlExtendedPriceBase;
			dataRow["arlExtendedPriceForeign"] = aRInvoiceLine.arlExtendedPriceForeign;
			dataRow["arlFinanceSourceInvoiceID"] = aRInvoiceLine.arlFinanceSourceInvoiceID;
			dataRow["arlFreightAmountBase"] = aRInvoiceLine.arlFreightAmountBase;
			dataRow["arlFreightAmountForeign"] = aRInvoiceLine.arlFreightAmountForeign;
			dataRow["arlFullExtendedPriceBase"] = aRInvoiceLine.arlFullExtendedPriceBase;
			dataRow["arlFullExtendedPriceForeign"] = aRInvoiceLine.arlFullExtendedPriceForeign;
			dataRow["arlFullUnitPriceBase"] = aRInvoiceLine.arlFullUnitPriceBase;
			dataRow["arlFullUnitPriceForeign"] = aRInvoiceLine.arlFullUnitPriceForeign;
			dataRow["arlInvoiceQuantity"] = aRInvoiceLine.arlInvoiceQuantity;
			dataRow["arlAvalaraIgnoreLine"] = aRInvoiceLine.arlAvalaraIgnoreLine;
			dataRow["arlCogsPostedToGl"] = aRInvoiceLine.arlCogsPostedToGl;
			dataRow["arlDeliveryInvoicedComplete"] = aRInvoiceLine.arlDeliveryInvoicedComplete;
			dataRow["arlDepositLine"] = aRInvoiceLine.arlDepositLine;
			dataRow["arlIncludeTaxInRetention"] = aRInvoiceLine.arlIncludeTaxInRetention;
			dataRow["arlIntraCompanyPosted"] = aRInvoiceLine.arlIntraCompanyPosted;
			dataRow["arlPayCommission"] = aRInvoiceLine.arlPayCommission;
			dataRow["arlPostedToGl"] = aRInvoiceLine.arlPostedToGl;
			dataRow["arlRetention"] = aRInvoiceLine.arlRetention;
			dataRow["arlJobAssemblyID"] = aRInvoiceLine.arlJobAssemblyID;
			dataRow["arlJobID"] = aRInvoiceLine.arlJobID;
			dataRow["arlJobMaterialID"] = aRInvoiceLine.arlJobMaterialID;
			dataRow["arlLineType"] = aRInvoiceLine.arlLineType;
			dataRow["arlNonTaxReasonID"] = aRInvoiceLine.arlNonTaxReasonID;
			dataRow["arlOrderQuantity"] = aRInvoiceLine.arlOrderQuantity;
			dataRow["arlOrgPartID"] = aRInvoiceLine.arlOrgPartID;
			dataRow["arlOrgPartShortDescription"] = aRInvoiceLine.arlOrgPartShortDescription;
			dataRow["arlPartGroupID"] = aRInvoiceLine.arlPartGroupID;
			dataRow["arlPartID"] = aRInvoiceLine.arlPartID;
			dataRow["arlPartLongDescriptionRtf"] = aRInvoiceLine.arlPartLongDescriptionRtf ?? dataRow["arlPartLongDescriptionRtf"];
			dataRow["arlPartLongDescriptionText"] = aRInvoiceLine.arlPartLongDescriptionText ?? dataRow["arlPartLongDescriptionText"];
			dataRow["arlPartRevisionID"] = aRInvoiceLine.arlPartRevisionID;
			dataRow["arlPartShortDescription"] = aRInvoiceLine.arlPartShortDescription;
			dataRow["arlProjectAreaID"] = aRInvoiceLine.arlProjectAreaID;
			dataRow["arlProjectID"] = aRInvoiceLine.arlProjectID;
			dataRow["arlRetentionAmountBase"] = aRInvoiceLine.arlRetentionAmountBase;
			dataRow["arlRetentionAmountForeign"] = aRInvoiceLine.arlRetentionAmountForeign;
			DataRow dataRow3 = dataRow;
			arlCogsCalculatedDate = aRInvoiceLine.arlRetentionDueDate;
			dataRow3["arlRetentionDueDate"] = (arlCogsCalculatedDate.HasValue ? ((object)arlCogsCalculatedDate.GetValueOrDefault()) : dataRow["arlRetentionDueDate"]);
			dataRow["arlRetentionPercent"] = aRInvoiceLine.arlRetentionPercent;
			dataRow["arlRmaClaimID"] = aRInvoiceLine.arlRmaClaimID;
			dataRow["arlRmaClaimLineID"] = aRInvoiceLine.arlRmaClaimLineID;
			dataRow["arlRmaReceiptID"] = aRInvoiceLine.arlRmaReceiptID;
			dataRow["arlRmaReceiptLineID"] = aRInvoiceLine.arlRmaReceiptLineID;
			dataRow["arlSalesOrderDeliveryID"] = aRInvoiceLine.arlSalesOrderDeliveryID;
			dataRow["arlSalesOrderID"] = aRInvoiceLine.arlSalesOrderID;
			dataRow["arlSalesOrderLineID"] = aRInvoiceLine.arlSalesOrderLineID;
			dataRow["arlSecondTaxAmountBase"] = aRInvoiceLine.arlSecondTaxAmountBase;
			dataRow["arlSecondTaxAmountForeign"] = aRInvoiceLine.arlSecondTaxAmountForeign;
			dataRow["arlSecondTaxCodeID"] = aRInvoiceLine.arlSecondTaxCodeID;
			dataRow["arlShipmentID"] = aRInvoiceLine.arlShipmentID;
			dataRow["arlShipmentLineID"] = aRInvoiceLine.arlShipmentLineID;
			dataRow["arlTaxAmountBase"] = aRInvoiceLine.arlTaxAmountBase;
			dataRow["arlTaxAmountForeign"] = aRInvoiceLine.arlTaxAmountForeign;
			dataRow["arlTaxCodeID"] = aRInvoiceLine.arlTaxCodeID;
			DataRow dataRow4 = dataRow;
			arlCogsCalculatedDate = aRInvoiceLine.arlTaxDate;
			dataRow4["arlTaxDate"] = (arlCogsCalculatedDate.HasValue ? ((object)arlCogsCalculatedDate.GetValueOrDefault()) : dataRow["arlTaxDate"]);
			dataRow["arlUnitDiscountBase"] = aRInvoiceLine.arlUnitDiscountBase;
			dataRow["arlUnitDiscountForeign"] = aRInvoiceLine.arlUnitDiscountForeign;
			dataRow["arlUnitOfMeasure"] = aRInvoiceLine.arlUnitOfMeasure;
			dataRow["arlUnitPriceBase"] = aRInvoiceLine.arlUnitPriceBase;
			dataRow["arlUnitPriceForeign"] = aRInvoiceLine.arlUnitPriceForeign;
			if (aRInvoiceLine.CustomFields != null && aRInvoiceLine.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in aRInvoiceLine.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the ARInvoiceLine [{aRInvoiceLine.arlUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the ARInvoiceLine [{aRInvoiceLine.arlUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
