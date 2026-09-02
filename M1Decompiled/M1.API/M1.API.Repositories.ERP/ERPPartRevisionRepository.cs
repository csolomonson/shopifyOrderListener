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

public class ERPPartRevisionRepository : APIBaseRepository, IERPPartRevisionRepository, IAPIBaseRepository, IDisposable
{
	public ERPPartRevisionRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesPartRevisionExist(Guid partRevisionId)
	{
		InitializeParameterLists();
		base.filterList.Add("imrUniqueID|C", partRevisionId);
		base.selectList.Add("imrUniqueID");
		return Task.FromResult(GetAsObject("PartRevisions", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPPartRevisionInformationDto>> GetAllPartRevisions(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPPartRevisionInformationDto> collection = new List<ERPPartRevisionInformationDto>();
		InitializeParameterLists();
		string[] array = new string[98]
		{
			"imrAverageDutyCost", "imrAverageFreightCost", "imrAverageLaborCost", "imrAverageMaterialCost", "imrAverageMiscCost", "imrAverageOverheadCost", "imrAverageSubcontractCost", "imrBarLength", "imrBlanketPeriodBegin", "imrBlanketPeriodEnd",
			"imrPartRevisionID", "imrCommodityCode", "imrCommodityDescription", "imrConversionFactor", "imrCountryOfManufacture", "imrCreatedBy", "imrCreatedDate", "imrDocuments", "imrEffectiveEndDate", "imrEffectiveStartDate",
			"imrUniqueID", "imrExpenseSplitPercentTotal", "imrFdxHandlingCost", "imrFdxPackageHeight", "imrFdxPackageLength", "imrFdxPackageWidth", "imrFdxPackaging", "imrFdxPackagingCost", "imrFdxShipCostMarkupPct", "imrFormID",
			"imrInspectionNotesRTF", "imrInspectionNotesText", "imrInventoryUnitOfMeasure", "imrInactive", "imrConfigured", "imrFdxNonstandardContainer", "imrFdxOneItemPerShipment", "imrPreferredRefExists", "imrPurchasableItem", "imrSuppressShortDescription",
			"imrUseQuotePrice", "imrLastDutyCost", "imrLastFreightCost", "imrLastLaborCost", "imrLastMaterialCost", "imrLastMiscCost", "imrLastOverheadCost", "imrLastReceiptDate", "imrLastRunDatePurchasePlanner", "imrLastSubcontractCost",
			"imrLastTransactionDate", "imrLeadTime", "imrLongDescriptionHtml", "imrLongDescriptionRtf", "imrLongDescriptionText", "imrManufacturingLotSize", "imrMaximumQuantity", "imrMinimumQuantity", "imrNetCostBeginDate", "imrNetCostCode",
			"imrNetCostEndDate", "imrPartID", "imrPartImageFileName", "imrPreferenceCriteria", "imrProducerDetermination", "imrProductCategoryID", "imrProductCategoryLineID", "imrProductionNotesRTF", "imrProductionNotesText", "imrPurchaseLocationID",
			"imrPurchaseUnitOfMeasure", "imrQuantityAllocated", "imrQuantityOnHand", "imrQuantityOnOrderPurchases", "imrQuantityOnOrderSales", "imrQuantityToInspect", "imrQuantityToReturn", "imrQuantityToReturnJob", "imrRequiresInspection", "imrRowVersion",
			"imrSheetSizeX", "imrSheetSizeY", "imrShortDescription", "imrSourceMethodID", "imrSourceRevisionID", "imrStandardDutyCost", "imrStandardFreightCost", "imrStandardLaborCost", "imrStandardMaterialCost", "imrStandardMiscCost",
			"imrStandardOverheadCost", "imrStandardSubcontractCost", "imrSupplierOrganizationID", "imrThickness", "imrUniversalProductCode", "imrVolume", "imrWeight", "imrWeightUnitOfMeasure"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("PartRevisions");
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
		using (DataTable dataTable = GetAsDataTable("PartRevisions", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPPartRevisionInformationDto eRPPartRevisionInformationDto = new ERPPartRevisionInformationDto();
				eRPPartRevisionInformationDto.imrAverageDutyCost = dataTable.Rows[i].Field<decimal>("imrAverageDutyCost");
				eRPPartRevisionInformationDto.imrAverageFreightCost = dataTable.Rows[i].Field<decimal>("imrAverageFreightCost");
				eRPPartRevisionInformationDto.imrAverageLaborCost = dataTable.Rows[i].Field<decimal>("imrAverageLaborCost");
				eRPPartRevisionInformationDto.imrAverageMaterialCost = dataTable.Rows[i].Field<decimal>("imrAverageMaterialCost");
				eRPPartRevisionInformationDto.imrAverageMiscCost = dataTable.Rows[i].Field<decimal>("imrAverageMiscCost");
				eRPPartRevisionInformationDto.imrAverageOverheadCost = dataTable.Rows[i].Field<decimal>("imrAverageOverheadCost");
				eRPPartRevisionInformationDto.imrAverageSubcontractCost = dataTable.Rows[i].Field<decimal>("imrAverageSubcontractCost");
				eRPPartRevisionInformationDto.imrBarLength = dataTable.Rows[i].Field<decimal>("imrBarLength");
				eRPPartRevisionInformationDto.imrBlanketPeriodBegin = dataTable.Rows[i].Field<DateTime?>("imrBlanketPeriodBegin");
				eRPPartRevisionInformationDto.imrBlanketPeriodEnd = dataTable.Rows[i].Field<DateTime?>("imrBlanketPeriodEnd");
				eRPPartRevisionInformationDto.imrPartRevisionID = dataTable.Rows[i].Field<string>("imrPartRevisionID");
				eRPPartRevisionInformationDto.imrCommodityCode = dataTable.Rows[i].Field<string>("imrCommodityCode");
				eRPPartRevisionInformationDto.imrCommodityDescription = dataTable.Rows[i].Field<string>("imrCommodityDescription");
				eRPPartRevisionInformationDto.imrConversionFactor = dataTable.Rows[i].Field<decimal>("imrConversionFactor");
				eRPPartRevisionInformationDto.imrCountryOfManufacture = dataTable.Rows[i].Field<string>("imrCountryOfManufacture");
				eRPPartRevisionInformationDto.imrCreatedBy = dataTable.Rows[i].Field<string>("imrCreatedBy");
				eRPPartRevisionInformationDto.imrCreatedDate = dataTable.Rows[i].Field<DateTime?>("imrCreatedDate");
				eRPPartRevisionInformationDto.imrDocuments = dataTable.Rows[i].Field<string>("imrDocuments");
				eRPPartRevisionInformationDto.imrEffectiveEndDate = dataTable.Rows[i].Field<DateTime?>("imrEffectiveEndDate");
				eRPPartRevisionInformationDto.imrEffectiveStartDate = dataTable.Rows[i].Field<DateTime?>("imrEffectiveStartDate");
				eRPPartRevisionInformationDto.imrUniqueID = dataTable.Rows[i].Field<Guid>("imrUniqueID");
				eRPPartRevisionInformationDto.imrExpenseSplitPercentTotal = dataTable.Rows[i].Field<decimal>("imrExpenseSplitPercentTotal");
				eRPPartRevisionInformationDto.imrFdxHandlingCost = dataTable.Rows[i].Field<decimal>("imrFdxHandlingCost");
				eRPPartRevisionInformationDto.imrFdxPackageHeight = dataTable.Rows[i].Field<int>("imrFdxPackageHeight");
				eRPPartRevisionInformationDto.imrFdxPackageLength = dataTable.Rows[i].Field<int>("imrFdxPackageLength");
				eRPPartRevisionInformationDto.imrFdxPackageWidth = dataTable.Rows[i].Field<int>("imrFdxPackageWidth");
				eRPPartRevisionInformationDto.imrFdxPackaging = dataTable.Rows[i].Field<string>("imrFdxPackaging");
				eRPPartRevisionInformationDto.imrFdxPackagingCost = dataTable.Rows[i].Field<decimal>("imrFdxPackagingCost");
				eRPPartRevisionInformationDto.imrFdxShipCostMarkupPct = dataTable.Rows[i].Field<decimal>("imrFdxShipCostMarkupPct");
				eRPPartRevisionInformationDto.imrFormID = dataTable.Rows[i].Field<string>("imrFormID");
				eRPPartRevisionInformationDto.imrInspectionNotesRTF = dataTable.Rows[i].Field<string>("imrInspectionNotesRTF");
				eRPPartRevisionInformationDto.imrInspectionNotesText = dataTable.Rows[i].Field<string>("imrInspectionNotesText");
				eRPPartRevisionInformationDto.imrInventoryUnitOfMeasure = dataTable.Rows[i].Field<string>("imrInventoryUnitOfMeasure");
				eRPPartRevisionInformationDto.imrInactive = dataTable.Rows[i].Field<bool>("imrInactive");
				eRPPartRevisionInformationDto.imrConfigured = dataTable.Rows[i].Field<bool>("imrConfigured");
				eRPPartRevisionInformationDto.imrFdxNonstandardContainer = dataTable.Rows[i].Field<bool>("imrFdxNonstandardContainer");
				eRPPartRevisionInformationDto.imrFdxOneItemPerShipment = dataTable.Rows[i].Field<bool>("imrFdxOneItemPerShipment");
				eRPPartRevisionInformationDto.imrPreferredRefExists = dataTable.Rows[i].Field<bool>("imrPreferredRefExists");
				eRPPartRevisionInformationDto.imrPurchasableItem = dataTable.Rows[i].Field<bool>("imrPurchasableItem");
				eRPPartRevisionInformationDto.imrSuppressShortDescription = dataTable.Rows[i].Field<bool>("imrSuppressShortDescription");
				eRPPartRevisionInformationDto.imrUseQuotePrice = dataTable.Rows[i].Field<bool>("imrUseQuotePrice");
				eRPPartRevisionInformationDto.imrLastDutyCost = dataTable.Rows[i].Field<decimal>("imrLastDutyCost");
				eRPPartRevisionInformationDto.imrLastFreightCost = dataTable.Rows[i].Field<decimal>("imrLastFreightCost");
				eRPPartRevisionInformationDto.imrLastLaborCost = dataTable.Rows[i].Field<decimal>("imrLastLaborCost");
				eRPPartRevisionInformationDto.imrLastMaterialCost = dataTable.Rows[i].Field<decimal>("imrLastMaterialCost");
				eRPPartRevisionInformationDto.imrLastMiscCost = dataTable.Rows[i].Field<decimal>("imrLastMiscCost");
				eRPPartRevisionInformationDto.imrLastOverheadCost = dataTable.Rows[i].Field<decimal>("imrLastOverheadCost");
				eRPPartRevisionInformationDto.imrLastReceiptDate = dataTable.Rows[i].Field<DateTime?>("imrLastReceiptDate");
				eRPPartRevisionInformationDto.imrLastRunDatePurchasePlanner = dataTable.Rows[i].Field<DateTime?>("imrLastRunDatePurchasePlanner");
				eRPPartRevisionInformationDto.imrLastSubcontractCost = dataTable.Rows[i].Field<decimal>("imrLastSubcontractCost");
				eRPPartRevisionInformationDto.imrLastTransactionDate = dataTable.Rows[i].Field<DateTime?>("imrLastTransactionDate");
				eRPPartRevisionInformationDto.imrLeadTime = dataTable.Rows[i].Field<short>("imrLeadTime");
				eRPPartRevisionInformationDto.imrLongDescriptionHtml = dataTable.Rows[i].Field<string>("imrLongDescriptionHtml");
				eRPPartRevisionInformationDto.imrLongDescriptionRtf = dataTable.Rows[i].Field<string>("imrLongDescriptionRtf");
				eRPPartRevisionInformationDto.imrLongDescriptionText = dataTable.Rows[i].Field<string>("imrLongDescriptionText");
				eRPPartRevisionInformationDto.imrManufacturingLotSize = dataTable.Rows[i].Field<decimal>("imrManufacturingLotSize");
				eRPPartRevisionInformationDto.imrMaximumQuantity = dataTable.Rows[i].Field<decimal>("imrMaximumQuantity");
				eRPPartRevisionInformationDto.imrMinimumQuantity = dataTable.Rows[i].Field<decimal>("imrMinimumQuantity");
				eRPPartRevisionInformationDto.imrNetCostBeginDate = dataTable.Rows[i].Field<DateTime?>("imrNetCostBeginDate");
				eRPPartRevisionInformationDto.imrNetCostCode = dataTable.Rows[i].Field<string>("imrNetCostCode");
				eRPPartRevisionInformationDto.imrNetCostEndDate = dataTable.Rows[i].Field<DateTime?>("imrNetCostEndDate");
				eRPPartRevisionInformationDto.imrPartID = dataTable.Rows[i].Field<string>("imrPartID");
				eRPPartRevisionInformationDto.imrPartImageFileName = dataTable.Rows[i].Field<string>("imrPartImageFileName");
				eRPPartRevisionInformationDto.imrPreferenceCriteria = dataTable.Rows[i].Field<string>("imrPreferenceCriteria");
				eRPPartRevisionInformationDto.imrProducerDetermination = dataTable.Rows[i].Field<string>("imrProducerDetermination");
				eRPPartRevisionInformationDto.imrProductCategoryID = dataTable.Rows[i].Field<string>("imrProductCategoryID");
				eRPPartRevisionInformationDto.imrProductCategoryLineID = dataTable.Rows[i].Field<short>("imrProductCategoryLineID");
				eRPPartRevisionInformationDto.imrProductionNotesRTF = dataTable.Rows[i].Field<string>("imrProductionNotesRTF");
				eRPPartRevisionInformationDto.imrProductionNotesText = dataTable.Rows[i].Field<string>("imrProductionNotesText");
				eRPPartRevisionInformationDto.imrPurchaseLocationID = dataTable.Rows[i].Field<string>("imrPurchaseLocationID");
				eRPPartRevisionInformationDto.imrPurchaseUnitOfMeasure = dataTable.Rows[i].Field<string>("imrPurchaseUnitOfMeasure");
				eRPPartRevisionInformationDto.imrQuantityAllocated = dataTable.Rows[i].Field<decimal>("imrQuantityAllocated");
				eRPPartRevisionInformationDto.imrQuantityOnHand = dataTable.Rows[i].Field<decimal>("imrQuantityOnHand");
				eRPPartRevisionInformationDto.imrQuantityOnOrderPurchases = dataTable.Rows[i].Field<decimal>("imrQuantityOnOrderPurchases");
				eRPPartRevisionInformationDto.imrQuantityOnOrderSales = dataTable.Rows[i].Field<decimal>("imrQuantityOnOrderSales");
				eRPPartRevisionInformationDto.imrQuantityToInspect = dataTable.Rows[i].Field<decimal>("imrQuantityToInspect");
				eRPPartRevisionInformationDto.imrQuantityToReturn = dataTable.Rows[i].Field<decimal>("imrQuantityToReturn");
				eRPPartRevisionInformationDto.imrQuantityToReturnJob = dataTable.Rows[i].Field<decimal>("imrQuantityToReturnJob");
				eRPPartRevisionInformationDto.imrRequiresInspection = dataTable.Rows[i].Field<byte>("imrRequiresInspection");
				eRPPartRevisionInformationDto.imrRowVersion = dataTable.Rows[i].Field<byte[]>("imrRowVersion");
				eRPPartRevisionInformationDto.imrSheetSizeX = dataTable.Rows[i].Field<decimal>("imrSheetSizeX");
				eRPPartRevisionInformationDto.imrSheetSizeY = dataTable.Rows[i].Field<decimal>("imrSheetSizeY");
				eRPPartRevisionInformationDto.imrShortDescription = dataTable.Rows[i].Field<string>("imrShortDescription");
				eRPPartRevisionInformationDto.imrSourceMethodID = dataTable.Rows[i].Field<string>("imrSourceMethodID");
				eRPPartRevisionInformationDto.imrSourceRevisionID = dataTable.Rows[i].Field<string>("imrSourceRevisionID");
				eRPPartRevisionInformationDto.imrStandardDutyCost = dataTable.Rows[i].Field<decimal>("imrStandardDutyCost");
				eRPPartRevisionInformationDto.imrStandardFreightCost = dataTable.Rows[i].Field<decimal>("imrStandardFreightCost");
				eRPPartRevisionInformationDto.imrStandardLaborCost = dataTable.Rows[i].Field<decimal>("imrStandardLaborCost");
				eRPPartRevisionInformationDto.imrStandardMaterialCost = dataTable.Rows[i].Field<decimal>("imrStandardMaterialCost");
				eRPPartRevisionInformationDto.imrStandardMiscCost = dataTable.Rows[i].Field<decimal>("imrStandardMiscCost");
				eRPPartRevisionInformationDto.imrStandardOverheadCost = dataTable.Rows[i].Field<decimal>("imrStandardOverheadCost");
				eRPPartRevisionInformationDto.imrStandardSubcontractCost = dataTable.Rows[i].Field<decimal>("imrStandardSubcontractCost");
				eRPPartRevisionInformationDto.imrSupplierOrganizationID = dataTable.Rows[i].Field<string>("imrSupplierOrganizationID");
				eRPPartRevisionInformationDto.imrThickness = dataTable.Rows[i].Field<decimal>("imrThickness");
				eRPPartRevisionInformationDto.imrUniversalProductCode = dataTable.Rows[i].Field<string>("imrUniversalProductCode");
				eRPPartRevisionInformationDto.imrVolume = dataTable.Rows[i].Field<decimal>("imrVolume");
				eRPPartRevisionInformationDto.imrWeight = dataTable.Rows[i].Field<decimal>("imrWeight");
				eRPPartRevisionInformationDto.imrWeightUnitOfMeasure = dataTable.Rows[i].Field<string>("imrWeightUnitOfMeasure");
				eRPPartRevisionInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPPartRevisionInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPPartRevisionInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPPartRevisionInformationDto> GetPartRevision(Guid partRevisionId)
	{
		ERPPartRevisionInformationDto eRPPartRevisionInformationDto = new ERPPartRevisionInformationDto();
		InitializeParameterLists();
		string[] collection = new string[98]
		{
			"imrAverageDutyCost", "imrAverageFreightCost", "imrAverageLaborCost", "imrAverageMaterialCost", "imrAverageMiscCost", "imrAverageOverheadCost", "imrAverageSubcontractCost", "imrBarLength", "imrBlanketPeriodBegin", "imrBlanketPeriodEnd",
			"imrPartRevisionID", "imrCommodityCode", "imrCommodityDescription", "imrConversionFactor", "imrCountryOfManufacture", "imrCreatedBy", "imrCreatedDate", "imrDocuments", "imrEffectiveEndDate", "imrEffectiveStartDate",
			"imrUniqueID", "imrExpenseSplitPercentTotal", "imrFdxHandlingCost", "imrFdxPackageHeight", "imrFdxPackageLength", "imrFdxPackageWidth", "imrFdxPackaging", "imrFdxPackagingCost", "imrFdxShipCostMarkupPct", "imrFormID",
			"imrInspectionNotesRTF", "imrInspectionNotesText", "imrInventoryUnitOfMeasure", "imrInactive", "imrConfigured", "imrFdxNonstandardContainer", "imrFdxOneItemPerShipment", "imrPreferredRefExists", "imrPurchasableItem", "imrSuppressShortDescription",
			"imrUseQuotePrice", "imrLastDutyCost", "imrLastFreightCost", "imrLastLaborCost", "imrLastMaterialCost", "imrLastMiscCost", "imrLastOverheadCost", "imrLastReceiptDate", "imrLastRunDatePurchasePlanner", "imrLastSubcontractCost",
			"imrLastTransactionDate", "imrLeadTime", "imrLongDescriptionHtml", "imrLongDescriptionRtf", "imrLongDescriptionText", "imrManufacturingLotSize", "imrMaximumQuantity", "imrMinimumQuantity", "imrNetCostBeginDate", "imrNetCostCode",
			"imrNetCostEndDate", "imrPartID", "imrPartImageFileName", "imrPreferenceCriteria", "imrProducerDetermination", "imrProductCategoryID", "imrProductCategoryLineID", "imrProductionNotesRTF", "imrProductionNotesText", "imrPurchaseLocationID",
			"imrPurchaseUnitOfMeasure", "imrQuantityAllocated", "imrQuantityOnHand", "imrQuantityOnOrderPurchases", "imrQuantityOnOrderSales", "imrQuantityToInspect", "imrQuantityToReturn", "imrQuantityToReturnJob", "imrRequiresInspection", "imrRowVersion",
			"imrSheetSizeX", "imrSheetSizeY", "imrShortDescription", "imrSourceMethodID", "imrSourceRevisionID", "imrStandardDutyCost", "imrStandardFreightCost", "imrStandardLaborCost", "imrStandardMaterialCost", "imrStandardMiscCost",
			"imrStandardOverheadCost", "imrStandardSubcontractCost", "imrSupplierOrganizationID", "imrThickness", "imrUniversalProductCode", "imrVolume", "imrWeight", "imrWeightUnitOfMeasure"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("imrUniqueID|C", partRevisionId);
		AddCustomFieldsToSelectList("PartRevisions");
		using (DataTable dataTable = GetAsDataTable("PartRevisions", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPPartRevisionInformationDto);
			}
			eRPPartRevisionInformationDto.imrAverageDutyCost = dataTable.Rows[0].Field<decimal>("imrAverageDutyCost");
			eRPPartRevisionInformationDto.imrAverageFreightCost = dataTable.Rows[0].Field<decimal>("imrAverageFreightCost");
			eRPPartRevisionInformationDto.imrAverageLaborCost = dataTable.Rows[0].Field<decimal>("imrAverageLaborCost");
			eRPPartRevisionInformationDto.imrAverageMaterialCost = dataTable.Rows[0].Field<decimal>("imrAverageMaterialCost");
			eRPPartRevisionInformationDto.imrAverageMiscCost = dataTable.Rows[0].Field<decimal>("imrAverageMiscCost");
			eRPPartRevisionInformationDto.imrAverageOverheadCost = dataTable.Rows[0].Field<decimal>("imrAverageOverheadCost");
			eRPPartRevisionInformationDto.imrAverageSubcontractCost = dataTable.Rows[0].Field<decimal>("imrAverageSubcontractCost");
			eRPPartRevisionInformationDto.imrBarLength = dataTable.Rows[0].Field<decimal>("imrBarLength");
			eRPPartRevisionInformationDto.imrBlanketPeriodBegin = dataTable.Rows[0].Field<DateTime?>("imrBlanketPeriodBegin");
			eRPPartRevisionInformationDto.imrBlanketPeriodEnd = dataTable.Rows[0].Field<DateTime?>("imrBlanketPeriodEnd");
			eRPPartRevisionInformationDto.imrPartRevisionID = dataTable.Rows[0].Field<string>("imrPartRevisionID");
			eRPPartRevisionInformationDto.imrCommodityCode = dataTable.Rows[0].Field<string>("imrCommodityCode");
			eRPPartRevisionInformationDto.imrCommodityDescription = dataTable.Rows[0].Field<string>("imrCommodityDescription");
			eRPPartRevisionInformationDto.imrConversionFactor = dataTable.Rows[0].Field<decimal>("imrConversionFactor");
			eRPPartRevisionInformationDto.imrCountryOfManufacture = dataTable.Rows[0].Field<string>("imrCountryOfManufacture");
			eRPPartRevisionInformationDto.imrCreatedBy = dataTable.Rows[0].Field<string>("imrCreatedBy");
			eRPPartRevisionInformationDto.imrCreatedDate = dataTable.Rows[0].Field<DateTime?>("imrCreatedDate");
			eRPPartRevisionInformationDto.imrDocuments = dataTable.Rows[0].Field<string>("imrDocuments");
			eRPPartRevisionInformationDto.imrEffectiveEndDate = dataTable.Rows[0].Field<DateTime?>("imrEffectiveEndDate");
			eRPPartRevisionInformationDto.imrEffectiveStartDate = dataTable.Rows[0].Field<DateTime?>("imrEffectiveStartDate");
			eRPPartRevisionInformationDto.imrUniqueID = dataTable.Rows[0].Field<Guid>("imrUniqueID");
			eRPPartRevisionInformationDto.imrExpenseSplitPercentTotal = dataTable.Rows[0].Field<decimal>("imrExpenseSplitPercentTotal");
			eRPPartRevisionInformationDto.imrFdxHandlingCost = dataTable.Rows[0].Field<decimal>("imrFdxHandlingCost");
			eRPPartRevisionInformationDto.imrFdxPackageHeight = dataTable.Rows[0].Field<int>("imrFdxPackageHeight");
			eRPPartRevisionInformationDto.imrFdxPackageLength = dataTable.Rows[0].Field<int>("imrFdxPackageLength");
			eRPPartRevisionInformationDto.imrFdxPackageWidth = dataTable.Rows[0].Field<int>("imrFdxPackageWidth");
			eRPPartRevisionInformationDto.imrFdxPackaging = dataTable.Rows[0].Field<string>("imrFdxPackaging");
			eRPPartRevisionInformationDto.imrFdxPackagingCost = dataTable.Rows[0].Field<decimal>("imrFdxPackagingCost");
			eRPPartRevisionInformationDto.imrFdxShipCostMarkupPct = dataTable.Rows[0].Field<decimal>("imrFdxShipCostMarkupPct");
			eRPPartRevisionInformationDto.imrFormID = dataTable.Rows[0].Field<string>("imrFormID");
			eRPPartRevisionInformationDto.imrInspectionNotesRTF = dataTable.Rows[0].Field<string>("imrInspectionNotesRTF");
			eRPPartRevisionInformationDto.imrInspectionNotesText = dataTable.Rows[0].Field<string>("imrInspectionNotesText");
			eRPPartRevisionInformationDto.imrInventoryUnitOfMeasure = dataTable.Rows[0].Field<string>("imrInventoryUnitOfMeasure");
			eRPPartRevisionInformationDto.imrInactive = dataTable.Rows[0].Field<bool>("imrInactive");
			eRPPartRevisionInformationDto.imrConfigured = dataTable.Rows[0].Field<bool>("imrConfigured");
			eRPPartRevisionInformationDto.imrFdxNonstandardContainer = dataTable.Rows[0].Field<bool>("imrFdxNonstandardContainer");
			eRPPartRevisionInformationDto.imrFdxOneItemPerShipment = dataTable.Rows[0].Field<bool>("imrFdxOneItemPerShipment");
			eRPPartRevisionInformationDto.imrPreferredRefExists = dataTable.Rows[0].Field<bool>("imrPreferredRefExists");
			eRPPartRevisionInformationDto.imrPurchasableItem = dataTable.Rows[0].Field<bool>("imrPurchasableItem");
			eRPPartRevisionInformationDto.imrSuppressShortDescription = dataTable.Rows[0].Field<bool>("imrSuppressShortDescription");
			eRPPartRevisionInformationDto.imrUseQuotePrice = dataTable.Rows[0].Field<bool>("imrUseQuotePrice");
			eRPPartRevisionInformationDto.imrLastDutyCost = dataTable.Rows[0].Field<decimal>("imrLastDutyCost");
			eRPPartRevisionInformationDto.imrLastFreightCost = dataTable.Rows[0].Field<decimal>("imrLastFreightCost");
			eRPPartRevisionInformationDto.imrLastLaborCost = dataTable.Rows[0].Field<decimal>("imrLastLaborCost");
			eRPPartRevisionInformationDto.imrLastMaterialCost = dataTable.Rows[0].Field<decimal>("imrLastMaterialCost");
			eRPPartRevisionInformationDto.imrLastMiscCost = dataTable.Rows[0].Field<decimal>("imrLastMiscCost");
			eRPPartRevisionInformationDto.imrLastOverheadCost = dataTable.Rows[0].Field<decimal>("imrLastOverheadCost");
			eRPPartRevisionInformationDto.imrLastReceiptDate = dataTable.Rows[0].Field<DateTime?>("imrLastReceiptDate");
			eRPPartRevisionInformationDto.imrLastRunDatePurchasePlanner = dataTable.Rows[0].Field<DateTime?>("imrLastRunDatePurchasePlanner");
			eRPPartRevisionInformationDto.imrLastSubcontractCost = dataTable.Rows[0].Field<decimal>("imrLastSubcontractCost");
			eRPPartRevisionInformationDto.imrLastTransactionDate = dataTable.Rows[0].Field<DateTime?>("imrLastTransactionDate");
			eRPPartRevisionInformationDto.imrLeadTime = dataTable.Rows[0].Field<short>("imrLeadTime");
			eRPPartRevisionInformationDto.imrLongDescriptionHtml = dataTable.Rows[0].Field<string>("imrLongDescriptionHtml");
			eRPPartRevisionInformationDto.imrLongDescriptionRtf = dataTable.Rows[0].Field<string>("imrLongDescriptionRtf");
			eRPPartRevisionInformationDto.imrLongDescriptionText = dataTable.Rows[0].Field<string>("imrLongDescriptionText");
			eRPPartRevisionInformationDto.imrManufacturingLotSize = dataTable.Rows[0].Field<decimal>("imrManufacturingLotSize");
			eRPPartRevisionInformationDto.imrMaximumQuantity = dataTable.Rows[0].Field<decimal>("imrMaximumQuantity");
			eRPPartRevisionInformationDto.imrMinimumQuantity = dataTable.Rows[0].Field<decimal>("imrMinimumQuantity");
			eRPPartRevisionInformationDto.imrNetCostBeginDate = dataTable.Rows[0].Field<DateTime?>("imrNetCostBeginDate");
			eRPPartRevisionInformationDto.imrNetCostCode = dataTable.Rows[0].Field<string>("imrNetCostCode");
			eRPPartRevisionInformationDto.imrNetCostEndDate = dataTable.Rows[0].Field<DateTime?>("imrNetCostEndDate");
			eRPPartRevisionInformationDto.imrPartID = dataTable.Rows[0].Field<string>("imrPartID");
			eRPPartRevisionInformationDto.imrPartImageFileName = dataTable.Rows[0].Field<string>("imrPartImageFileName");
			eRPPartRevisionInformationDto.imrPreferenceCriteria = dataTable.Rows[0].Field<string>("imrPreferenceCriteria");
			eRPPartRevisionInformationDto.imrProducerDetermination = dataTable.Rows[0].Field<string>("imrProducerDetermination");
			eRPPartRevisionInformationDto.imrProductCategoryID = dataTable.Rows[0].Field<string>("imrProductCategoryID");
			eRPPartRevisionInformationDto.imrProductCategoryLineID = dataTable.Rows[0].Field<short>("imrProductCategoryLineID");
			eRPPartRevisionInformationDto.imrProductionNotesRTF = dataTable.Rows[0].Field<string>("imrProductionNotesRTF");
			eRPPartRevisionInformationDto.imrProductionNotesText = dataTable.Rows[0].Field<string>("imrProductionNotesText");
			eRPPartRevisionInformationDto.imrPurchaseLocationID = dataTable.Rows[0].Field<string>("imrPurchaseLocationID");
			eRPPartRevisionInformationDto.imrPurchaseUnitOfMeasure = dataTable.Rows[0].Field<string>("imrPurchaseUnitOfMeasure");
			eRPPartRevisionInformationDto.imrQuantityAllocated = dataTable.Rows[0].Field<decimal>("imrQuantityAllocated");
			eRPPartRevisionInformationDto.imrQuantityOnHand = dataTable.Rows[0].Field<decimal>("imrQuantityOnHand");
			eRPPartRevisionInformationDto.imrQuantityOnOrderPurchases = dataTable.Rows[0].Field<decimal>("imrQuantityOnOrderPurchases");
			eRPPartRevisionInformationDto.imrQuantityOnOrderSales = dataTable.Rows[0].Field<decimal>("imrQuantityOnOrderSales");
			eRPPartRevisionInformationDto.imrQuantityToInspect = dataTable.Rows[0].Field<decimal>("imrQuantityToInspect");
			eRPPartRevisionInformationDto.imrQuantityToReturn = dataTable.Rows[0].Field<decimal>("imrQuantityToReturn");
			eRPPartRevisionInformationDto.imrQuantityToReturnJob = dataTable.Rows[0].Field<decimal>("imrQuantityToReturnJob");
			eRPPartRevisionInformationDto.imrRequiresInspection = dataTable.Rows[0].Field<byte>("imrRequiresInspection");
			eRPPartRevisionInformationDto.imrRowVersion = dataTable.Rows[0].Field<byte[]>("imrRowVersion");
			eRPPartRevisionInformationDto.imrSheetSizeX = dataTable.Rows[0].Field<decimal>("imrSheetSizeX");
			eRPPartRevisionInformationDto.imrSheetSizeY = dataTable.Rows[0].Field<decimal>("imrSheetSizeY");
			eRPPartRevisionInformationDto.imrShortDescription = dataTable.Rows[0].Field<string>("imrShortDescription");
			eRPPartRevisionInformationDto.imrSourceMethodID = dataTable.Rows[0].Field<string>("imrSourceMethodID");
			eRPPartRevisionInformationDto.imrSourceRevisionID = dataTable.Rows[0].Field<string>("imrSourceRevisionID");
			eRPPartRevisionInformationDto.imrStandardDutyCost = dataTable.Rows[0].Field<decimal>("imrStandardDutyCost");
			eRPPartRevisionInformationDto.imrStandardFreightCost = dataTable.Rows[0].Field<decimal>("imrStandardFreightCost");
			eRPPartRevisionInformationDto.imrStandardLaborCost = dataTable.Rows[0].Field<decimal>("imrStandardLaborCost");
			eRPPartRevisionInformationDto.imrStandardMaterialCost = dataTable.Rows[0].Field<decimal>("imrStandardMaterialCost");
			eRPPartRevisionInformationDto.imrStandardMiscCost = dataTable.Rows[0].Field<decimal>("imrStandardMiscCost");
			eRPPartRevisionInformationDto.imrStandardOverheadCost = dataTable.Rows[0].Field<decimal>("imrStandardOverheadCost");
			eRPPartRevisionInformationDto.imrStandardSubcontractCost = dataTable.Rows[0].Field<decimal>("imrStandardSubcontractCost");
			eRPPartRevisionInformationDto.imrSupplierOrganizationID = dataTable.Rows[0].Field<string>("imrSupplierOrganizationID");
			eRPPartRevisionInformationDto.imrThickness = dataTable.Rows[0].Field<decimal>("imrThickness");
			eRPPartRevisionInformationDto.imrUniversalProductCode = dataTable.Rows[0].Field<string>("imrUniversalProductCode");
			eRPPartRevisionInformationDto.imrVolume = dataTable.Rows[0].Field<decimal>("imrVolume");
			eRPPartRevisionInformationDto.imrWeight = dataTable.Rows[0].Field<decimal>("imrWeight");
			eRPPartRevisionInformationDto.imrWeightUnitOfMeasure = dataTable.Rows[0].Field<string>("imrWeightUnitOfMeasure");
			eRPPartRevisionInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPPartRevisionInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPPartRevisionInformationDto);
	}

	public Task<APIValidationInfoDto> SavePartRevision(ERPPartRevisionDto partRevision)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM PartRevisions WHERE imrUniqueID = " + M1Util.ConvertToLinq(partRevision.imrUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["imrPartID"] = partRevision.imrPartID.ToUpper();
				dataRow["imrPartRevisionID"] = partRevision.imrPartRevisionID.ToUpper();
				partRevision.imrUniqueID = ((partRevision.imrUniqueID == Guid.Empty) ? Guid.NewGuid() : partRevision.imrUniqueID);
				dataRow["imrUniqueID"] = partRevision.imrUniqueID;
				dataRow["imrCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["imrCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The PartRevision could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (partRevision.imrRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the PartRevision is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["imrRowVersion"], partRevision.imrRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the PartRevision has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the PartRevision again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["imrAverageDutyCost"] = partRevision.imrAverageDutyCost;
			dataRow["imrAverageFreightCost"] = partRevision.imrAverageFreightCost;
			dataRow["imrAverageLaborCost"] = partRevision.imrAverageLaborCost;
			dataRow["imrAverageMaterialCost"] = partRevision.imrAverageMaterialCost;
			dataRow["imrAverageMiscCost"] = partRevision.imrAverageMiscCost;
			dataRow["imrAverageOverheadCost"] = partRevision.imrAverageOverheadCost;
			dataRow["imrAverageSubcontractCost"] = partRevision.imrAverageSubcontractCost;
			dataRow["imrBarLength"] = partRevision.imrBarLength;
			DataRow dataRow2 = dataRow;
			DateTime? imrBlanketPeriodBegin = partRevision.imrBlanketPeriodBegin;
			dataRow2["imrBlanketPeriodBegin"] = (imrBlanketPeriodBegin.HasValue ? ((object)imrBlanketPeriodBegin.GetValueOrDefault()) : dataRow["imrBlanketPeriodBegin"]);
			DataRow dataRow3 = dataRow;
			imrBlanketPeriodBegin = partRevision.imrBlanketPeriodEnd;
			dataRow3["imrBlanketPeriodEnd"] = (imrBlanketPeriodBegin.HasValue ? ((object)imrBlanketPeriodBegin.GetValueOrDefault()) : dataRow["imrBlanketPeriodEnd"]);
			dataRow["imrCommodityCode"] = partRevision.imrCommodityCode;
			dataRow["imrCommodityDescription"] = partRevision.imrCommodityDescription;
			dataRow["imrConversionFactor"] = partRevision.imrConversionFactor;
			dataRow["imrCountryOfManufacture"] = partRevision.imrCountryOfManufacture;
			dataRow["imrDocuments"] = partRevision.imrDocuments ?? dataRow["imrDocuments"];
			DataRow dataRow4 = dataRow;
			imrBlanketPeriodBegin = partRevision.imrEffectiveEndDate;
			dataRow4["imrEffectiveEndDate"] = (imrBlanketPeriodBegin.HasValue ? ((object)imrBlanketPeriodBegin.GetValueOrDefault()) : dataRow["imrEffectiveEndDate"]);
			DataRow dataRow5 = dataRow;
			imrBlanketPeriodBegin = partRevision.imrEffectiveStartDate;
			dataRow5["imrEffectiveStartDate"] = (imrBlanketPeriodBegin.HasValue ? ((object)imrBlanketPeriodBegin.GetValueOrDefault()) : dataRow["imrEffectiveStartDate"]);
			dataRow["imrExpenseSplitPercentTotal"] = partRevision.imrExpenseSplitPercentTotal;
			dataRow["imrFdxHandlingCost"] = partRevision.imrFdxHandlingCost;
			dataRow["imrFdxPackageHeight"] = partRevision.imrFdxPackageHeight;
			dataRow["imrFdxPackageLength"] = partRevision.imrFdxPackageLength;
			dataRow["imrFdxPackageWidth"] = partRevision.imrFdxPackageWidth;
			dataRow["imrFdxPackaging"] = partRevision.imrFdxPackaging;
			dataRow["imrFdxPackagingCost"] = partRevision.imrFdxPackagingCost;
			dataRow["imrFdxShipCostMarkupPct"] = partRevision.imrFdxShipCostMarkupPct;
			dataRow["imrFormID"] = partRevision.imrFormID;
			dataRow["imrInspectionNotesRTF"] = partRevision.imrInspectionNotesRTF ?? dataRow["imrInspectionNotesRTF"];
			dataRow["imrInspectionNotesText"] = partRevision.imrInspectionNotesText ?? dataRow["imrInspectionNotesText"];
			dataRow["imrInventoryUnitOfMeasure"] = partRevision.imrInventoryUnitOfMeasure;
			dataRow["imrInactive"] = partRevision.imrInactive;
			dataRow["imrConfigured"] = partRevision.imrConfigured;
			dataRow["imrFdxNonstandardContainer"] = partRevision.imrFdxNonstandardContainer;
			dataRow["imrFdxOneItemPerShipment"] = partRevision.imrFdxOneItemPerShipment;
			dataRow["imrPreferredRefExists"] = partRevision.imrPreferredRefExists;
			dataRow["imrPurchasableItem"] = partRevision.imrPurchasableItem;
			dataRow["imrSuppressShortDescription"] = partRevision.imrSuppressShortDescription;
			dataRow["imrUseQuotePrice"] = partRevision.imrUseQuotePrice;
			dataRow["imrLastDutyCost"] = partRevision.imrLastDutyCost;
			dataRow["imrLastFreightCost"] = partRevision.imrLastFreightCost;
			dataRow["imrLastLaborCost"] = partRevision.imrLastLaborCost;
			dataRow["imrLastMaterialCost"] = partRevision.imrLastMaterialCost;
			dataRow["imrLastMiscCost"] = partRevision.imrLastMiscCost;
			dataRow["imrLastOverheadCost"] = partRevision.imrLastOverheadCost;
			DataRow dataRow6 = dataRow;
			imrBlanketPeriodBegin = partRevision.imrLastReceiptDate;
			dataRow6["imrLastReceiptDate"] = (imrBlanketPeriodBegin.HasValue ? ((object)imrBlanketPeriodBegin.GetValueOrDefault()) : dataRow["imrLastReceiptDate"]);
			DataRow dataRow7 = dataRow;
			imrBlanketPeriodBegin = partRevision.imrLastRunDatePurchasePlanner;
			dataRow7["imrLastRunDatePurchasePlanner"] = (imrBlanketPeriodBegin.HasValue ? ((object)imrBlanketPeriodBegin.GetValueOrDefault()) : dataRow["imrLastRunDatePurchasePlanner"]);
			dataRow["imrLastSubcontractCost"] = partRevision.imrLastSubcontractCost;
			DataRow dataRow8 = dataRow;
			imrBlanketPeriodBegin = partRevision.imrLastTransactionDate;
			dataRow8["imrLastTransactionDate"] = (imrBlanketPeriodBegin.HasValue ? ((object)imrBlanketPeriodBegin.GetValueOrDefault()) : dataRow["imrLastTransactionDate"]);
			dataRow["imrLeadTime"] = partRevision.imrLeadTime;
			dataRow["imrLongDescriptionHtml"] = partRevision.imrLongDescriptionHtml ?? dataRow["imrLongDescriptionHtml"];
			dataRow["imrLongDescriptionRtf"] = partRevision.imrLongDescriptionRtf ?? dataRow["imrLongDescriptionRtf"];
			dataRow["imrLongDescriptionText"] = partRevision.imrLongDescriptionText ?? dataRow["imrLongDescriptionText"];
			dataRow["imrManufacturingLotSize"] = partRevision.imrManufacturingLotSize;
			dataRow["imrMaximumQuantity"] = partRevision.imrMaximumQuantity;
			dataRow["imrMinimumQuantity"] = partRevision.imrMinimumQuantity;
			DataRow dataRow9 = dataRow;
			imrBlanketPeriodBegin = partRevision.imrNetCostBeginDate;
			dataRow9["imrNetCostBeginDate"] = (imrBlanketPeriodBegin.HasValue ? ((object)imrBlanketPeriodBegin.GetValueOrDefault()) : dataRow["imrNetCostBeginDate"]);
			dataRow["imrNetCostCode"] = partRevision.imrNetCostCode;
			DataRow dataRow10 = dataRow;
			imrBlanketPeriodBegin = partRevision.imrNetCostEndDate;
			dataRow10["imrNetCostEndDate"] = (imrBlanketPeriodBegin.HasValue ? ((object)imrBlanketPeriodBegin.GetValueOrDefault()) : dataRow["imrNetCostEndDate"]);
			dataRow["imrPartImageFileName"] = partRevision.imrPartImageFileName;
			dataRow["imrPreferenceCriteria"] = partRevision.imrPreferenceCriteria;
			dataRow["imrProducerDetermination"] = partRevision.imrProducerDetermination;
			dataRow["imrProductCategoryID"] = partRevision.imrProductCategoryID;
			dataRow["imrProductCategoryLineID"] = partRevision.imrProductCategoryLineID;
			dataRow["imrProductionNotesRTF"] = partRevision.imrProductionNotesRTF ?? dataRow["imrProductionNotesRTF"];
			dataRow["imrProductionNotesText"] = partRevision.imrProductionNotesText ?? dataRow["imrProductionNotesText"];
			dataRow["imrPurchaseLocationID"] = partRevision.imrPurchaseLocationID;
			dataRow["imrPurchaseUnitOfMeasure"] = partRevision.imrPurchaseUnitOfMeasure;
			dataRow["imrQuantityAllocated"] = partRevision.imrQuantityAllocated;
			dataRow["imrQuantityOnHand"] = partRevision.imrQuantityOnHand;
			dataRow["imrQuantityOnOrderPurchases"] = partRevision.imrQuantityOnOrderPurchases;
			dataRow["imrQuantityOnOrderSales"] = partRevision.imrQuantityOnOrderSales;
			dataRow["imrQuantityToInspect"] = partRevision.imrQuantityToInspect;
			dataRow["imrQuantityToReturn"] = partRevision.imrQuantityToReturn;
			dataRow["imrQuantityToReturnJob"] = partRevision.imrQuantityToReturnJob;
			dataRow["imrRequiresInspection"] = partRevision.imrRequiresInspection;
			dataRow["imrSheetSizeX"] = partRevision.imrSheetSizeX;
			dataRow["imrSheetSizeY"] = partRevision.imrSheetSizeY;
			dataRow["imrShortDescription"] = partRevision.imrShortDescription;
			dataRow["imrSourceMethodID"] = partRevision.imrSourceMethodID;
			dataRow["imrSourceRevisionID"] = partRevision.imrSourceRevisionID;
			dataRow["imrStandardDutyCost"] = partRevision.imrStandardDutyCost;
			dataRow["imrStandardFreightCost"] = partRevision.imrStandardFreightCost;
			dataRow["imrStandardLaborCost"] = partRevision.imrStandardLaborCost;
			dataRow["imrStandardMaterialCost"] = partRevision.imrStandardMaterialCost;
			dataRow["imrStandardMiscCost"] = partRevision.imrStandardMiscCost;
			dataRow["imrStandardOverheadCost"] = partRevision.imrStandardOverheadCost;
			dataRow["imrStandardSubcontractCost"] = partRevision.imrStandardSubcontractCost;
			dataRow["imrSupplierOrganizationID"] = partRevision.imrSupplierOrganizationID;
			dataRow["imrThickness"] = partRevision.imrThickness;
			dataRow["imrUniversalProductCode"] = partRevision.imrUniversalProductCode;
			dataRow["imrVolume"] = partRevision.imrVolume;
			dataRow["imrWeight"] = partRevision.imrWeight;
			dataRow["imrWeightUnitOfMeasure"] = partRevision.imrWeightUnitOfMeasure;
			if (partRevision.CustomFields != null && partRevision.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in partRevision.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the PartRevision [{partRevision.imrUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the PartRevision [{partRevision.imrUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
