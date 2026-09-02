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

public class ERPPurchaseOrderLineRepository : APIBaseRepository, IERPPurchaseOrderLineRepository, IAPIBaseRepository, IDisposable
{
	public ERPPurchaseOrderLineRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesPurchaseOrderLineExist(Guid purchaseOrderLineId)
	{
		InitializeParameterLists();
		base.filterList.Add("pmlUniqueID|C", purchaseOrderLineId);
		base.selectList.Add("pmlUniqueID");
		return Task.FromResult(GetAsObject("PurchaseOrderLines", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPPurchaseOrderLineInformationDto>> GetAllPurchaseOrderLines(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPPurchaseOrderLineInformationDto> collection = new List<ERPPurchaseOrderLineInformationDto>();
		InitializeParameterLists();
		string[] array = new string[86]
		{
			"pmlAssetID", "pmlAssetTypeID", "pmlConversionFactor", "pmlCreatedBy", "pmlCreatedDate", "pmlDmrClaimID", "pmlDmrClaimLineID", "pmlDocuments", "pmlDueDate", "pmlUniqueID",
			"pmlExpenseSplitPercentTotal", "pmlExtendedCostBase", "pmlExtendedCostForeign", "pmlForm1099Box", "pmlInventoryQuantity", "pmlInventoryQuantityReceived", "pmlInventoryUnitOfMeasure", "pmlClosed", "pmlCreateJobSeq", "pmlIntraCompanyPosted",
			"pmlInTransit", "pmlInTransitJournalsCreated", "pmlInvoicedComplete", "pmlKitPart", "pmlPlanned", "pmlPriceOverride", "pmlReceivedComplete", "pmlRequiresInspection", "pmlSupplierRequirement", "pmlTaxable",
			"pmlItemType", "pmlJobAssemblyID", "pmlJobID", "pmlJobMaterialID", "pmlJobOpenQuantity", "pmlJobOperationID", "pmlJobType", "pmlLandedCostID", "pmlLeadTime", "pmlNonTaxReasonID",
			"pmlOrgPartID", "pmlOrgPartShortDescription", "pmlPartBinID", "pmlPartID", "pmlPartLongDescriptionRtf", "pmlPartLongDescriptionText", "pmlPartRevisionID", "pmlPartShortDescription", "pmlPartWarehouseLocationID", "pmlProcessID",
			"pmlProjectAreaID", "pmlProjectID", "pmlPurchaseOrderID", "pmlPurchaseQuantity", "pmlPurchaseQuantityReceived", "pmlPurchaseType", "pmlPurchaseUnitCostBase", "pmlPurchaseUnitCostForeign", "pmlPurchaseUnitOfMeasure", "pmlQuantityOnOrder",
			"pmlRfqID", "pmlRfqLineID", "pmlRmaClaimID", "pmlRmaClaimLineID", "pmlRowVersion", "pmlSalesOrderDeliveryID", "pmlSalesOrderID", "pmlSalesOrderLineID", "pmlSecondTaxAmountBase", "pmlSecondTaxAmountForeign",
			"pmlSecondTaxCodeID", "pmlPurchaseOrderLineID", "pmlSetupChargeBase", "pmlSetupChargeForeign", "pmlSourcePurchaseOrderID", "pmlSourcePurchaseOrderLineID", "pmlSourceTableName", "pmlSourceTableUniqueID", "pmlTaxAmountBase", "pmlTaxAmountForeign",
			"pmlTaxCodeID", "pmlTotalComponentCosts", "pmlTotalExtendedCostBase", "pmlTotalExtendedCostForeign", "pmlTrackingNumber", "pmlWorkCenterID"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("PurchaseOrderLines");
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
		using (DataTable dataTable = GetAsDataTable("PurchaseOrderLines", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPPurchaseOrderLineInformationDto eRPPurchaseOrderLineInformationDto = new ERPPurchaseOrderLineInformationDto();
				eRPPurchaseOrderLineInformationDto.pmlAssetID = dataTable.Rows[i].Field<string>("pmlAssetID");
				eRPPurchaseOrderLineInformationDto.pmlAssetTypeID = dataTable.Rows[i].Field<string>("pmlAssetTypeID");
				eRPPurchaseOrderLineInformationDto.pmlConversionFactor = dataTable.Rows[i].Field<decimal>("pmlConversionFactor");
				eRPPurchaseOrderLineInformationDto.pmlCreatedBy = dataTable.Rows[i].Field<string>("pmlCreatedBy");
				eRPPurchaseOrderLineInformationDto.pmlCreatedDate = dataTable.Rows[i].Field<DateTime?>("pmlCreatedDate");
				eRPPurchaseOrderLineInformationDto.pmlDmrClaimID = dataTable.Rows[i].Field<string>("pmlDmrClaimID");
				eRPPurchaseOrderLineInformationDto.pmlDmrClaimLineID = dataTable.Rows[i].Field<short>("pmlDmrClaimLineID");
				eRPPurchaseOrderLineInformationDto.pmlDocuments = dataTable.Rows[i].Field<string>("pmlDocuments");
				eRPPurchaseOrderLineInformationDto.pmlDueDate = dataTable.Rows[i].Field<DateTime?>("pmlDueDate");
				eRPPurchaseOrderLineInformationDto.pmlUniqueID = dataTable.Rows[i].Field<Guid>("pmlUniqueID");
				eRPPurchaseOrderLineInformationDto.pmlExpenseSplitPercentTotal = dataTable.Rows[i].Field<decimal>("pmlExpenseSplitPercentTotal");
				eRPPurchaseOrderLineInformationDto.pmlExtendedCostBase = dataTable.Rows[i].Field<decimal>("pmlExtendedCostBase");
				eRPPurchaseOrderLineInformationDto.pmlExtendedCostForeign = dataTable.Rows[i].Field<decimal>("pmlExtendedCostForeign");
				eRPPurchaseOrderLineInformationDto.pmlForm1099Box = dataTable.Rows[i].Field<byte>("pmlForm1099Box");
				eRPPurchaseOrderLineInformationDto.pmlInventoryQuantity = dataTable.Rows[i].Field<decimal>("pmlInventoryQuantity");
				eRPPurchaseOrderLineInformationDto.pmlInventoryQuantityReceived = dataTable.Rows[i].Field<decimal>("pmlInventoryQuantityReceived");
				eRPPurchaseOrderLineInformationDto.pmlInventoryUnitOfMeasure = dataTable.Rows[i].Field<string>("pmlInventoryUnitOfMeasure");
				eRPPurchaseOrderLineInformationDto.pmlClosed = dataTable.Rows[i].Field<bool>("pmlClosed");
				eRPPurchaseOrderLineInformationDto.pmlCreateJobSeq = dataTable.Rows[i].Field<bool>("pmlCreateJobSeq");
				eRPPurchaseOrderLineInformationDto.pmlIntraCompanyPosted = dataTable.Rows[i].Field<bool>("pmlIntraCompanyPosted");
				eRPPurchaseOrderLineInformationDto.pmlInTransit = dataTable.Rows[i].Field<bool>("pmlInTransit");
				eRPPurchaseOrderLineInformationDto.pmlInTransitJournalsCreated = dataTable.Rows[i].Field<bool>("pmlInTransitJournalsCreated");
				eRPPurchaseOrderLineInformationDto.pmlInvoicedComplete = dataTable.Rows[i].Field<bool>("pmlInvoicedComplete");
				eRPPurchaseOrderLineInformationDto.pmlKitPart = dataTable.Rows[i].Field<bool>("pmlKitPart");
				eRPPurchaseOrderLineInformationDto.pmlPlanned = dataTable.Rows[i].Field<bool>("pmlPlanned");
				eRPPurchaseOrderLineInformationDto.pmlPriceOverride = dataTable.Rows[i].Field<bool>("pmlPriceOverride");
				eRPPurchaseOrderLineInformationDto.pmlReceivedComplete = dataTable.Rows[i].Field<bool>("pmlReceivedComplete");
				eRPPurchaseOrderLineInformationDto.pmlRequiresInspection = dataTable.Rows[i].Field<bool>("pmlRequiresInspection");
				eRPPurchaseOrderLineInformationDto.pmlSupplierRequirement = dataTable.Rows[i].Field<bool>("pmlSupplierRequirement");
				eRPPurchaseOrderLineInformationDto.pmlTaxable = dataTable.Rows[i].Field<bool>("pmlTaxable");
				eRPPurchaseOrderLineInformationDto.pmlItemType = dataTable.Rows[i].Field<string>("pmlItemType");
				eRPPurchaseOrderLineInformationDto.pmlJobAssemblyID = dataTable.Rows[i].Field<int>("pmlJobAssemblyID");
				eRPPurchaseOrderLineInformationDto.pmlJobID = dataTable.Rows[i].Field<string>("pmlJobID");
				eRPPurchaseOrderLineInformationDto.pmlJobMaterialID = dataTable.Rows[i].Field<int>("pmlJobMaterialID");
				eRPPurchaseOrderLineInformationDto.pmlJobOpenQuantity = dataTable.Rows[i].Field<decimal>("pmlJobOpenQuantity");
				eRPPurchaseOrderLineInformationDto.pmlJobOperationID = dataTable.Rows[i].Field<int>("pmlJobOperationID");
				eRPPurchaseOrderLineInformationDto.pmlJobType = dataTable.Rows[i].Field<byte>("pmlJobType");
				eRPPurchaseOrderLineInformationDto.pmlLandedCostID = dataTable.Rows[i].Field<string>("pmlLandedCostID");
				eRPPurchaseOrderLineInformationDto.pmlLeadTime = dataTable.Rows[i].Field<short>("pmlLeadTime");
				eRPPurchaseOrderLineInformationDto.pmlNonTaxReasonID = dataTable.Rows[i].Field<string>("pmlNonTaxReasonID");
				eRPPurchaseOrderLineInformationDto.pmlOrgPartID = dataTable.Rows[i].Field<string>("pmlOrgPartID");
				eRPPurchaseOrderLineInformationDto.pmlOrgPartShortDescription = dataTable.Rows[i].Field<string>("pmlOrgPartShortDescription");
				eRPPurchaseOrderLineInformationDto.pmlPartBinID = dataTable.Rows[i].Field<string>("pmlPartBinID");
				eRPPurchaseOrderLineInformationDto.pmlPartID = dataTable.Rows[i].Field<string>("pmlPartID");
				eRPPurchaseOrderLineInformationDto.pmlPartLongDescriptionRtf = dataTable.Rows[i].Field<string>("pmlPartLongDescriptionRtf");
				eRPPurchaseOrderLineInformationDto.pmlPartLongDescriptionText = dataTable.Rows[i].Field<string>("pmlPartLongDescriptionText");
				eRPPurchaseOrderLineInformationDto.pmlPartRevisionID = dataTable.Rows[i].Field<string>("pmlPartRevisionID");
				eRPPurchaseOrderLineInformationDto.pmlPartShortDescription = dataTable.Rows[i].Field<string>("pmlPartShortDescription");
				eRPPurchaseOrderLineInformationDto.pmlPartWarehouseLocationID = dataTable.Rows[i].Field<string>("pmlPartWarehouseLocationID");
				eRPPurchaseOrderLineInformationDto.pmlProcessID = dataTable.Rows[i].Field<string>("pmlProcessID");
				eRPPurchaseOrderLineInformationDto.pmlProjectAreaID = dataTable.Rows[i].Field<string>("pmlProjectAreaID");
				eRPPurchaseOrderLineInformationDto.pmlProjectID = dataTable.Rows[i].Field<string>("pmlProjectID");
				eRPPurchaseOrderLineInformationDto.pmlPurchaseOrderID = dataTable.Rows[i].Field<string>("pmlPurchaseOrderID");
				eRPPurchaseOrderLineInformationDto.pmlPurchaseQuantity = dataTable.Rows[i].Field<decimal>("pmlPurchaseQuantity");
				eRPPurchaseOrderLineInformationDto.pmlPurchaseQuantityReceived = dataTable.Rows[i].Field<decimal>("pmlPurchaseQuantityReceived");
				eRPPurchaseOrderLineInformationDto.pmlPurchaseType = dataTable.Rows[i].Field<byte>("pmlPurchaseType");
				eRPPurchaseOrderLineInformationDto.pmlPurchaseUnitCostBase = dataTable.Rows[i].Field<decimal>("pmlPurchaseUnitCostBase");
				eRPPurchaseOrderLineInformationDto.pmlPurchaseUnitCostForeign = dataTable.Rows[i].Field<decimal>("pmlPurchaseUnitCostForeign");
				eRPPurchaseOrderLineInformationDto.pmlPurchaseUnitOfMeasure = dataTable.Rows[i].Field<string>("pmlPurchaseUnitOfMeasure");
				eRPPurchaseOrderLineInformationDto.pmlQuantityOnOrder = dataTable.Rows[i].Field<decimal>("pmlQuantityOnOrder");
				eRPPurchaseOrderLineInformationDto.pmlRfqID = dataTable.Rows[i].Field<string>("pmlRfqID");
				eRPPurchaseOrderLineInformationDto.pmlRfqLineID = dataTable.Rows[i].Field<short>("pmlRfqLineID");
				eRPPurchaseOrderLineInformationDto.pmlRmaClaimID = dataTable.Rows[i].Field<string>("pmlRmaClaimID");
				eRPPurchaseOrderLineInformationDto.pmlRmaClaimLineID = dataTable.Rows[i].Field<short>("pmlRmaClaimLineID");
				eRPPurchaseOrderLineInformationDto.pmlRowVersion = dataTable.Rows[i].Field<byte[]>("pmlRowVersion");
				eRPPurchaseOrderLineInformationDto.pmlSalesOrderDeliveryID = dataTable.Rows[i].Field<short>("pmlSalesOrderDeliveryID");
				eRPPurchaseOrderLineInformationDto.pmlSalesOrderID = dataTable.Rows[i].Field<string>("pmlSalesOrderID");
				eRPPurchaseOrderLineInformationDto.pmlSalesOrderLineID = dataTable.Rows[i].Field<short>("pmlSalesOrderLineID");
				eRPPurchaseOrderLineInformationDto.pmlSecondTaxAmountBase = dataTable.Rows[i].Field<decimal>("pmlSecondTaxAmountBase");
				eRPPurchaseOrderLineInformationDto.pmlSecondTaxAmountForeign = dataTable.Rows[i].Field<decimal>("pmlSecondTaxAmountForeign");
				eRPPurchaseOrderLineInformationDto.pmlSecondTaxCodeID = dataTable.Rows[i].Field<string>("pmlSecondTaxCodeID");
				eRPPurchaseOrderLineInformationDto.pmlPurchaseOrderLineID = dataTable.Rows[i].Field<short>("pmlPurchaseOrderLineID");
				eRPPurchaseOrderLineInformationDto.pmlSetupChargeBase = dataTable.Rows[i].Field<decimal>("pmlSetupChargeBase");
				eRPPurchaseOrderLineInformationDto.pmlSetupChargeForeign = dataTable.Rows[i].Field<decimal>("pmlSetupChargeForeign");
				eRPPurchaseOrderLineInformationDto.pmlSourcePurchaseOrderID = dataTable.Rows[i].Field<string>("pmlSourcePurchaseOrderID");
				eRPPurchaseOrderLineInformationDto.pmlSourcePurchaseOrderLineID = dataTable.Rows[i].Field<short>("pmlSourcePurchaseOrderLineID");
				eRPPurchaseOrderLineInformationDto.pmlSourceTableName = dataTable.Rows[i].Field<string>("pmlSourceTableName");
				eRPPurchaseOrderLineInformationDto.pmlSourceTableUniqueID = dataTable.Rows[i].Field<Guid>("pmlSourceTableUniqueID");
				eRPPurchaseOrderLineInformationDto.pmlTaxAmountBase = dataTable.Rows[i].Field<decimal>("pmlTaxAmountBase");
				eRPPurchaseOrderLineInformationDto.pmlTaxAmountForeign = dataTable.Rows[i].Field<decimal>("pmlTaxAmountForeign");
				eRPPurchaseOrderLineInformationDto.pmlTaxCodeID = dataTable.Rows[i].Field<string>("pmlTaxCodeID");
				eRPPurchaseOrderLineInformationDto.pmlTotalComponentCosts = dataTable.Rows[i].Field<decimal>("pmlTotalComponentCosts");
				eRPPurchaseOrderLineInformationDto.pmlTotalExtendedCostBase = dataTable.Rows[i].Field<decimal>("pmlTotalExtendedCostBase");
				eRPPurchaseOrderLineInformationDto.pmlTotalExtendedCostForeign = dataTable.Rows[i].Field<decimal>("pmlTotalExtendedCostForeign");
				eRPPurchaseOrderLineInformationDto.pmlTrackingNumber = dataTable.Rows[i].Field<string>("pmlTrackingNumber");
				eRPPurchaseOrderLineInformationDto.pmlWorkCenterID = dataTable.Rows[i].Field<string>("pmlWorkCenterID");
				eRPPurchaseOrderLineInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPPurchaseOrderLineInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPPurchaseOrderLineInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPPurchaseOrderLineInformationDto> GetPurchaseOrderLine(Guid purchaseOrderLineId)
	{
		ERPPurchaseOrderLineInformationDto eRPPurchaseOrderLineInformationDto = new ERPPurchaseOrderLineInformationDto();
		InitializeParameterLists();
		string[] collection = new string[86]
		{
			"pmlAssetID", "pmlAssetTypeID", "pmlConversionFactor", "pmlCreatedBy", "pmlCreatedDate", "pmlDmrClaimID", "pmlDmrClaimLineID", "pmlDocuments", "pmlDueDate", "pmlUniqueID",
			"pmlExpenseSplitPercentTotal", "pmlExtendedCostBase", "pmlExtendedCostForeign", "pmlForm1099Box", "pmlInventoryQuantity", "pmlInventoryQuantityReceived", "pmlInventoryUnitOfMeasure", "pmlClosed", "pmlCreateJobSeq", "pmlIntraCompanyPosted",
			"pmlInTransit", "pmlInTransitJournalsCreated", "pmlInvoicedComplete", "pmlKitPart", "pmlPlanned", "pmlPriceOverride", "pmlReceivedComplete", "pmlRequiresInspection", "pmlSupplierRequirement", "pmlTaxable",
			"pmlItemType", "pmlJobAssemblyID", "pmlJobID", "pmlJobMaterialID", "pmlJobOpenQuantity", "pmlJobOperationID", "pmlJobType", "pmlLandedCostID", "pmlLeadTime", "pmlNonTaxReasonID",
			"pmlOrgPartID", "pmlOrgPartShortDescription", "pmlPartBinID", "pmlPartID", "pmlPartLongDescriptionRtf", "pmlPartLongDescriptionText", "pmlPartRevisionID", "pmlPartShortDescription", "pmlPartWarehouseLocationID", "pmlProcessID",
			"pmlProjectAreaID", "pmlProjectID", "pmlPurchaseOrderID", "pmlPurchaseQuantity", "pmlPurchaseQuantityReceived", "pmlPurchaseType", "pmlPurchaseUnitCostBase", "pmlPurchaseUnitCostForeign", "pmlPurchaseUnitOfMeasure", "pmlQuantityOnOrder",
			"pmlRfqID", "pmlRfqLineID", "pmlRmaClaimID", "pmlRmaClaimLineID", "pmlRowVersion", "pmlSalesOrderDeliveryID", "pmlSalesOrderID", "pmlSalesOrderLineID", "pmlSecondTaxAmountBase", "pmlSecondTaxAmountForeign",
			"pmlSecondTaxCodeID", "pmlPurchaseOrderLineID", "pmlSetupChargeBase", "pmlSetupChargeForeign", "pmlSourcePurchaseOrderID", "pmlSourcePurchaseOrderLineID", "pmlSourceTableName", "pmlSourceTableUniqueID", "pmlTaxAmountBase", "pmlTaxAmountForeign",
			"pmlTaxCodeID", "pmlTotalComponentCosts", "pmlTotalExtendedCostBase", "pmlTotalExtendedCostForeign", "pmlTrackingNumber", "pmlWorkCenterID"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("pmlUniqueID|C", purchaseOrderLineId);
		AddCustomFieldsToSelectList("PurchaseOrderLines");
		using (DataTable dataTable = GetAsDataTable("PurchaseOrderLines", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPPurchaseOrderLineInformationDto);
			}
			eRPPurchaseOrderLineInformationDto.pmlAssetID = dataTable.Rows[0].Field<string>("pmlAssetID");
			eRPPurchaseOrderLineInformationDto.pmlAssetTypeID = dataTable.Rows[0].Field<string>("pmlAssetTypeID");
			eRPPurchaseOrderLineInformationDto.pmlConversionFactor = dataTable.Rows[0].Field<decimal>("pmlConversionFactor");
			eRPPurchaseOrderLineInformationDto.pmlCreatedBy = dataTable.Rows[0].Field<string>("pmlCreatedBy");
			eRPPurchaseOrderLineInformationDto.pmlCreatedDate = dataTable.Rows[0].Field<DateTime?>("pmlCreatedDate");
			eRPPurchaseOrderLineInformationDto.pmlDmrClaimID = dataTable.Rows[0].Field<string>("pmlDmrClaimID");
			eRPPurchaseOrderLineInformationDto.pmlDmrClaimLineID = dataTable.Rows[0].Field<short>("pmlDmrClaimLineID");
			eRPPurchaseOrderLineInformationDto.pmlDocuments = dataTable.Rows[0].Field<string>("pmlDocuments");
			eRPPurchaseOrderLineInformationDto.pmlDueDate = dataTable.Rows[0].Field<DateTime?>("pmlDueDate");
			eRPPurchaseOrderLineInformationDto.pmlUniqueID = dataTable.Rows[0].Field<Guid>("pmlUniqueID");
			eRPPurchaseOrderLineInformationDto.pmlExpenseSplitPercentTotal = dataTable.Rows[0].Field<decimal>("pmlExpenseSplitPercentTotal");
			eRPPurchaseOrderLineInformationDto.pmlExtendedCostBase = dataTable.Rows[0].Field<decimal>("pmlExtendedCostBase");
			eRPPurchaseOrderLineInformationDto.pmlExtendedCostForeign = dataTable.Rows[0].Field<decimal>("pmlExtendedCostForeign");
			eRPPurchaseOrderLineInformationDto.pmlForm1099Box = dataTable.Rows[0].Field<byte>("pmlForm1099Box");
			eRPPurchaseOrderLineInformationDto.pmlInventoryQuantity = dataTable.Rows[0].Field<decimal>("pmlInventoryQuantity");
			eRPPurchaseOrderLineInformationDto.pmlInventoryQuantityReceived = dataTable.Rows[0].Field<decimal>("pmlInventoryQuantityReceived");
			eRPPurchaseOrderLineInformationDto.pmlInventoryUnitOfMeasure = dataTable.Rows[0].Field<string>("pmlInventoryUnitOfMeasure");
			eRPPurchaseOrderLineInformationDto.pmlClosed = dataTable.Rows[0].Field<bool>("pmlClosed");
			eRPPurchaseOrderLineInformationDto.pmlCreateJobSeq = dataTable.Rows[0].Field<bool>("pmlCreateJobSeq");
			eRPPurchaseOrderLineInformationDto.pmlIntraCompanyPosted = dataTable.Rows[0].Field<bool>("pmlIntraCompanyPosted");
			eRPPurchaseOrderLineInformationDto.pmlInTransit = dataTable.Rows[0].Field<bool>("pmlInTransit");
			eRPPurchaseOrderLineInformationDto.pmlInTransitJournalsCreated = dataTable.Rows[0].Field<bool>("pmlInTransitJournalsCreated");
			eRPPurchaseOrderLineInformationDto.pmlInvoicedComplete = dataTable.Rows[0].Field<bool>("pmlInvoicedComplete");
			eRPPurchaseOrderLineInformationDto.pmlKitPart = dataTable.Rows[0].Field<bool>("pmlKitPart");
			eRPPurchaseOrderLineInformationDto.pmlPlanned = dataTable.Rows[0].Field<bool>("pmlPlanned");
			eRPPurchaseOrderLineInformationDto.pmlPriceOverride = dataTable.Rows[0].Field<bool>("pmlPriceOverride");
			eRPPurchaseOrderLineInformationDto.pmlReceivedComplete = dataTable.Rows[0].Field<bool>("pmlReceivedComplete");
			eRPPurchaseOrderLineInformationDto.pmlRequiresInspection = dataTable.Rows[0].Field<bool>("pmlRequiresInspection");
			eRPPurchaseOrderLineInformationDto.pmlSupplierRequirement = dataTable.Rows[0].Field<bool>("pmlSupplierRequirement");
			eRPPurchaseOrderLineInformationDto.pmlTaxable = dataTable.Rows[0].Field<bool>("pmlTaxable");
			eRPPurchaseOrderLineInformationDto.pmlItemType = dataTable.Rows[0].Field<string>("pmlItemType");
			eRPPurchaseOrderLineInformationDto.pmlJobAssemblyID = dataTable.Rows[0].Field<int>("pmlJobAssemblyID");
			eRPPurchaseOrderLineInformationDto.pmlJobID = dataTable.Rows[0].Field<string>("pmlJobID");
			eRPPurchaseOrderLineInformationDto.pmlJobMaterialID = dataTable.Rows[0].Field<int>("pmlJobMaterialID");
			eRPPurchaseOrderLineInformationDto.pmlJobOpenQuantity = dataTable.Rows[0].Field<decimal>("pmlJobOpenQuantity");
			eRPPurchaseOrderLineInformationDto.pmlJobOperationID = dataTable.Rows[0].Field<int>("pmlJobOperationID");
			eRPPurchaseOrderLineInformationDto.pmlJobType = dataTable.Rows[0].Field<byte>("pmlJobType");
			eRPPurchaseOrderLineInformationDto.pmlLandedCostID = dataTable.Rows[0].Field<string>("pmlLandedCostID");
			eRPPurchaseOrderLineInformationDto.pmlLeadTime = dataTable.Rows[0].Field<short>("pmlLeadTime");
			eRPPurchaseOrderLineInformationDto.pmlNonTaxReasonID = dataTable.Rows[0].Field<string>("pmlNonTaxReasonID");
			eRPPurchaseOrderLineInformationDto.pmlOrgPartID = dataTable.Rows[0].Field<string>("pmlOrgPartID");
			eRPPurchaseOrderLineInformationDto.pmlOrgPartShortDescription = dataTable.Rows[0].Field<string>("pmlOrgPartShortDescription");
			eRPPurchaseOrderLineInformationDto.pmlPartBinID = dataTable.Rows[0].Field<string>("pmlPartBinID");
			eRPPurchaseOrderLineInformationDto.pmlPartID = dataTable.Rows[0].Field<string>("pmlPartID");
			eRPPurchaseOrderLineInformationDto.pmlPartLongDescriptionRtf = dataTable.Rows[0].Field<string>("pmlPartLongDescriptionRtf");
			eRPPurchaseOrderLineInformationDto.pmlPartLongDescriptionText = dataTable.Rows[0].Field<string>("pmlPartLongDescriptionText");
			eRPPurchaseOrderLineInformationDto.pmlPartRevisionID = dataTable.Rows[0].Field<string>("pmlPartRevisionID");
			eRPPurchaseOrderLineInformationDto.pmlPartShortDescription = dataTable.Rows[0].Field<string>("pmlPartShortDescription");
			eRPPurchaseOrderLineInformationDto.pmlPartWarehouseLocationID = dataTable.Rows[0].Field<string>("pmlPartWarehouseLocationID");
			eRPPurchaseOrderLineInformationDto.pmlProcessID = dataTable.Rows[0].Field<string>("pmlProcessID");
			eRPPurchaseOrderLineInformationDto.pmlProjectAreaID = dataTable.Rows[0].Field<string>("pmlProjectAreaID");
			eRPPurchaseOrderLineInformationDto.pmlProjectID = dataTable.Rows[0].Field<string>("pmlProjectID");
			eRPPurchaseOrderLineInformationDto.pmlPurchaseOrderID = dataTable.Rows[0].Field<string>("pmlPurchaseOrderID");
			eRPPurchaseOrderLineInformationDto.pmlPurchaseQuantity = dataTable.Rows[0].Field<decimal>("pmlPurchaseQuantity");
			eRPPurchaseOrderLineInformationDto.pmlPurchaseQuantityReceived = dataTable.Rows[0].Field<decimal>("pmlPurchaseQuantityReceived");
			eRPPurchaseOrderLineInformationDto.pmlPurchaseType = dataTable.Rows[0].Field<byte>("pmlPurchaseType");
			eRPPurchaseOrderLineInformationDto.pmlPurchaseUnitCostBase = dataTable.Rows[0].Field<decimal>("pmlPurchaseUnitCostBase");
			eRPPurchaseOrderLineInformationDto.pmlPurchaseUnitCostForeign = dataTable.Rows[0].Field<decimal>("pmlPurchaseUnitCostForeign");
			eRPPurchaseOrderLineInformationDto.pmlPurchaseUnitOfMeasure = dataTable.Rows[0].Field<string>("pmlPurchaseUnitOfMeasure");
			eRPPurchaseOrderLineInformationDto.pmlQuantityOnOrder = dataTable.Rows[0].Field<decimal>("pmlQuantityOnOrder");
			eRPPurchaseOrderLineInformationDto.pmlRfqID = dataTable.Rows[0].Field<string>("pmlRfqID");
			eRPPurchaseOrderLineInformationDto.pmlRfqLineID = dataTable.Rows[0].Field<short>("pmlRfqLineID");
			eRPPurchaseOrderLineInformationDto.pmlRmaClaimID = dataTable.Rows[0].Field<string>("pmlRmaClaimID");
			eRPPurchaseOrderLineInformationDto.pmlRmaClaimLineID = dataTable.Rows[0].Field<short>("pmlRmaClaimLineID");
			eRPPurchaseOrderLineInformationDto.pmlRowVersion = dataTable.Rows[0].Field<byte[]>("pmlRowVersion");
			eRPPurchaseOrderLineInformationDto.pmlSalesOrderDeliveryID = dataTable.Rows[0].Field<short>("pmlSalesOrderDeliveryID");
			eRPPurchaseOrderLineInformationDto.pmlSalesOrderID = dataTable.Rows[0].Field<string>("pmlSalesOrderID");
			eRPPurchaseOrderLineInformationDto.pmlSalesOrderLineID = dataTable.Rows[0].Field<short>("pmlSalesOrderLineID");
			eRPPurchaseOrderLineInformationDto.pmlSecondTaxAmountBase = dataTable.Rows[0].Field<decimal>("pmlSecondTaxAmountBase");
			eRPPurchaseOrderLineInformationDto.pmlSecondTaxAmountForeign = dataTable.Rows[0].Field<decimal>("pmlSecondTaxAmountForeign");
			eRPPurchaseOrderLineInformationDto.pmlSecondTaxCodeID = dataTable.Rows[0].Field<string>("pmlSecondTaxCodeID");
			eRPPurchaseOrderLineInformationDto.pmlPurchaseOrderLineID = dataTable.Rows[0].Field<short>("pmlPurchaseOrderLineID");
			eRPPurchaseOrderLineInformationDto.pmlSetupChargeBase = dataTable.Rows[0].Field<decimal>("pmlSetupChargeBase");
			eRPPurchaseOrderLineInformationDto.pmlSetupChargeForeign = dataTable.Rows[0].Field<decimal>("pmlSetupChargeForeign");
			eRPPurchaseOrderLineInformationDto.pmlSourcePurchaseOrderID = dataTable.Rows[0].Field<string>("pmlSourcePurchaseOrderID");
			eRPPurchaseOrderLineInformationDto.pmlSourcePurchaseOrderLineID = dataTable.Rows[0].Field<short>("pmlSourcePurchaseOrderLineID");
			eRPPurchaseOrderLineInformationDto.pmlSourceTableName = dataTable.Rows[0].Field<string>("pmlSourceTableName");
			eRPPurchaseOrderLineInformationDto.pmlSourceTableUniqueID = dataTable.Rows[0].Field<Guid>("pmlSourceTableUniqueID");
			eRPPurchaseOrderLineInformationDto.pmlTaxAmountBase = dataTable.Rows[0].Field<decimal>("pmlTaxAmountBase");
			eRPPurchaseOrderLineInformationDto.pmlTaxAmountForeign = dataTable.Rows[0].Field<decimal>("pmlTaxAmountForeign");
			eRPPurchaseOrderLineInformationDto.pmlTaxCodeID = dataTable.Rows[0].Field<string>("pmlTaxCodeID");
			eRPPurchaseOrderLineInformationDto.pmlTotalComponentCosts = dataTable.Rows[0].Field<decimal>("pmlTotalComponentCosts");
			eRPPurchaseOrderLineInformationDto.pmlTotalExtendedCostBase = dataTable.Rows[0].Field<decimal>("pmlTotalExtendedCostBase");
			eRPPurchaseOrderLineInformationDto.pmlTotalExtendedCostForeign = dataTable.Rows[0].Field<decimal>("pmlTotalExtendedCostForeign");
			eRPPurchaseOrderLineInformationDto.pmlTrackingNumber = dataTable.Rows[0].Field<string>("pmlTrackingNumber");
			eRPPurchaseOrderLineInformationDto.pmlWorkCenterID = dataTable.Rows[0].Field<string>("pmlWorkCenterID");
			eRPPurchaseOrderLineInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPPurchaseOrderLineInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPPurchaseOrderLineInformationDto);
	}

	public Task<APIValidationInfoDto> SavePurchaseOrderLine(ERPPurchaseOrderLineDto purchaseOrderLine)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM PurchaseOrderLines WHERE pmlUniqueID = " + M1Util.ConvertToLinq(purchaseOrderLine.pmlUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["pmlPurchaseOrderID"] = purchaseOrderLine.pmlPurchaseOrderID.ToUpper();
				dataRow["pmlPurchaseOrderLineID"] = purchaseOrderLine.pmlPurchaseOrderLineID;
				purchaseOrderLine.pmlUniqueID = ((purchaseOrderLine.pmlUniqueID == Guid.Empty) ? Guid.NewGuid() : purchaseOrderLine.pmlUniqueID);
				dataRow["pmlUniqueID"] = purchaseOrderLine.pmlUniqueID;
				dataRow["pmlCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["pmlCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The PurchaseOrderLine could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (purchaseOrderLine.pmlRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the PurchaseOrderLine is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["pmlRowVersion"], purchaseOrderLine.pmlRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the PurchaseOrderLine has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the PurchaseOrderLine again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["pmlAssetID"] = purchaseOrderLine.pmlAssetID;
			dataRow["pmlAssetTypeID"] = purchaseOrderLine.pmlAssetTypeID;
			dataRow["pmlConversionFactor"] = purchaseOrderLine.pmlConversionFactor;
			dataRow["pmlDmrClaimID"] = purchaseOrderLine.pmlDmrClaimID;
			dataRow["pmlDmrClaimLineID"] = purchaseOrderLine.pmlDmrClaimLineID;
			dataRow["pmlDocuments"] = purchaseOrderLine.pmlDocuments ?? dataRow["pmlDocuments"];
			DataRow dataRow2 = dataRow;
			DateTime? pmlDueDate = purchaseOrderLine.pmlDueDate;
			dataRow2["pmlDueDate"] = (pmlDueDate.HasValue ? ((object)pmlDueDate.GetValueOrDefault()) : dataRow["pmlDueDate"]);
			dataRow["pmlExpenseSplitPercentTotal"] = purchaseOrderLine.pmlExpenseSplitPercentTotal;
			dataRow["pmlExtendedCostBase"] = purchaseOrderLine.pmlExtendedCostBase;
			dataRow["pmlExtendedCostForeign"] = purchaseOrderLine.pmlExtendedCostForeign;
			dataRow["pmlForm1099Box"] = purchaseOrderLine.pmlForm1099Box;
			dataRow["pmlInventoryQuantity"] = purchaseOrderLine.pmlInventoryQuantity;
			dataRow["pmlInventoryQuantityReceived"] = purchaseOrderLine.pmlInventoryQuantityReceived;
			dataRow["pmlInventoryUnitOfMeasure"] = purchaseOrderLine.pmlInventoryUnitOfMeasure;
			dataRow["pmlClosed"] = purchaseOrderLine.pmlClosed;
			dataRow["pmlCreateJobSeq"] = purchaseOrderLine.pmlCreateJobSeq;
			dataRow["pmlIntraCompanyPosted"] = purchaseOrderLine.pmlIntraCompanyPosted;
			dataRow["pmlInTransit"] = purchaseOrderLine.pmlInTransit;
			dataRow["pmlInTransitJournalsCreated"] = purchaseOrderLine.pmlInTransitJournalsCreated;
			dataRow["pmlInvoicedComplete"] = purchaseOrderLine.pmlInvoicedComplete;
			dataRow["pmlKitPart"] = purchaseOrderLine.pmlKitPart;
			dataRow["pmlPlanned"] = purchaseOrderLine.pmlPlanned;
			dataRow["pmlPriceOverride"] = purchaseOrderLine.pmlPriceOverride;
			dataRow["pmlReceivedComplete"] = purchaseOrderLine.pmlReceivedComplete;
			dataRow["pmlRequiresInspection"] = purchaseOrderLine.pmlRequiresInspection;
			dataRow["pmlSupplierRequirement"] = purchaseOrderLine.pmlSupplierRequirement;
			dataRow["pmlTaxable"] = purchaseOrderLine.pmlTaxable;
			dataRow["pmlItemType"] = purchaseOrderLine.pmlItemType;
			dataRow["pmlJobAssemblyID"] = purchaseOrderLine.pmlJobAssemblyID;
			dataRow["pmlJobID"] = purchaseOrderLine.pmlJobID;
			dataRow["pmlJobMaterialID"] = purchaseOrderLine.pmlJobMaterialID;
			dataRow["pmlJobOpenQuantity"] = purchaseOrderLine.pmlJobOpenQuantity;
			dataRow["pmlJobOperationID"] = purchaseOrderLine.pmlJobOperationID;
			dataRow["pmlJobType"] = purchaseOrderLine.pmlJobType;
			dataRow["pmlLandedCostID"] = purchaseOrderLine.pmlLandedCostID;
			dataRow["pmlLeadTime"] = purchaseOrderLine.pmlLeadTime;
			dataRow["pmlNonTaxReasonID"] = purchaseOrderLine.pmlNonTaxReasonID;
			dataRow["pmlOrgPartID"] = purchaseOrderLine.pmlOrgPartID;
			dataRow["pmlOrgPartShortDescription"] = purchaseOrderLine.pmlOrgPartShortDescription;
			dataRow["pmlPartBinID"] = purchaseOrderLine.pmlPartBinID;
			dataRow["pmlPartID"] = purchaseOrderLine.pmlPartID;
			dataRow["pmlPartLongDescriptionRtf"] = purchaseOrderLine.pmlPartLongDescriptionRtf ?? dataRow["pmlPartLongDescriptionRtf"];
			dataRow["pmlPartLongDescriptionText"] = purchaseOrderLine.pmlPartLongDescriptionText ?? dataRow["pmlPartLongDescriptionText"];
			dataRow["pmlPartRevisionID"] = purchaseOrderLine.pmlPartRevisionID;
			dataRow["pmlPartShortDescription"] = purchaseOrderLine.pmlPartShortDescription;
			dataRow["pmlPartWarehouseLocationID"] = purchaseOrderLine.pmlPartWarehouseLocationID;
			dataRow["pmlProcessID"] = purchaseOrderLine.pmlProcessID;
			dataRow["pmlProjectAreaID"] = purchaseOrderLine.pmlProjectAreaID;
			dataRow["pmlProjectID"] = purchaseOrderLine.pmlProjectID;
			dataRow["pmlPurchaseQuantity"] = purchaseOrderLine.pmlPurchaseQuantity;
			dataRow["pmlPurchaseQuantityReceived"] = purchaseOrderLine.pmlPurchaseQuantityReceived;
			dataRow["pmlPurchaseType"] = purchaseOrderLine.pmlPurchaseType;
			dataRow["pmlPurchaseUnitCostBase"] = purchaseOrderLine.pmlPurchaseUnitCostBase;
			dataRow["pmlPurchaseUnitCostForeign"] = purchaseOrderLine.pmlPurchaseUnitCostForeign;
			dataRow["pmlPurchaseUnitOfMeasure"] = purchaseOrderLine.pmlPurchaseUnitOfMeasure;
			dataRow["pmlQuantityOnOrder"] = purchaseOrderLine.pmlQuantityOnOrder;
			dataRow["pmlRfqID"] = purchaseOrderLine.pmlRfqID;
			dataRow["pmlRfqLineID"] = purchaseOrderLine.pmlRfqLineID;
			dataRow["pmlRmaClaimID"] = purchaseOrderLine.pmlRmaClaimID;
			dataRow["pmlRmaClaimLineID"] = purchaseOrderLine.pmlRmaClaimLineID;
			dataRow["pmlSalesOrderDeliveryID"] = purchaseOrderLine.pmlSalesOrderDeliveryID;
			dataRow["pmlSalesOrderID"] = purchaseOrderLine.pmlSalesOrderID;
			dataRow["pmlSalesOrderLineID"] = purchaseOrderLine.pmlSalesOrderLineID;
			dataRow["pmlSecondTaxAmountBase"] = purchaseOrderLine.pmlSecondTaxAmountBase;
			dataRow["pmlSecondTaxAmountForeign"] = purchaseOrderLine.pmlSecondTaxAmountForeign;
			dataRow["pmlSecondTaxCodeID"] = purchaseOrderLine.pmlSecondTaxCodeID;
			dataRow["pmlSetupChargeBase"] = purchaseOrderLine.pmlSetupChargeBase;
			dataRow["pmlSetupChargeForeign"] = purchaseOrderLine.pmlSetupChargeForeign;
			dataRow["pmlSourcePurchaseOrderID"] = purchaseOrderLine.pmlSourcePurchaseOrderID;
			dataRow["pmlSourcePurchaseOrderLineID"] = purchaseOrderLine.pmlSourcePurchaseOrderLineID;
			dataRow["pmlSourceTableName"] = purchaseOrderLine.pmlSourceTableName;
			dataRow["pmlSourceTableUniqueID"] = purchaseOrderLine.pmlSourceTableUniqueID;
			dataRow["pmlTaxAmountBase"] = purchaseOrderLine.pmlTaxAmountBase;
			dataRow["pmlTaxAmountForeign"] = purchaseOrderLine.pmlTaxAmountForeign;
			dataRow["pmlTaxCodeID"] = purchaseOrderLine.pmlTaxCodeID;
			dataRow["pmlTotalComponentCosts"] = purchaseOrderLine.pmlTotalComponentCosts;
			dataRow["pmlTotalExtendedCostBase"] = purchaseOrderLine.pmlTotalExtendedCostBase;
			dataRow["pmlTotalExtendedCostForeign"] = purchaseOrderLine.pmlTotalExtendedCostForeign;
			dataRow["pmlTrackingNumber"] = purchaseOrderLine.pmlTrackingNumber;
			dataRow["pmlWorkCenterID"] = purchaseOrderLine.pmlWorkCenterID;
			if (purchaseOrderLine.CustomFields != null && purchaseOrderLine.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in purchaseOrderLine.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the PurchaseOrderLine [{purchaseOrderLine.pmlUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the PurchaseOrderLine [{purchaseOrderLine.pmlUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
