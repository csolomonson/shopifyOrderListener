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

public class ERPReceiptLineRepository : APIBaseRepository, IERPReceiptLineRepository, IAPIBaseRepository, IDisposable
{
	public ERPReceiptLineRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesReceiptLineExist(Guid receiptLineId)
	{
		InitializeParameterLists();
		base.filterList.Add("rmlUniqueID|C", receiptLineId);
		base.selectList.Add("rmlUniqueID");
		return Task.FromResult(GetAsObject("ReceiptLines", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPReceiptLineInformationDto>> GetAllReceiptLines(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPReceiptLineInformationDto> collection = new List<ERPReceiptLineInformationDto>();
		InitializeParameterLists();
		string[] array = new string[73]
		{
			"rmlConversionFactor", "rmlCreatedBy", "rmlCreatedDate", "rmlDescription", "rmlDmrClaimID", "rmlDmrClaimLineID", "rmlDutyUnitCost", "rmlUniqueID", "rmlExtendedCostBase", "rmlExtendedCostForeign",
			"rmlForm1099Box", "rmlFreightUnitCost", "rmlHeatLot", "rmlInspectionNotesRTF", "rmlInspectionNotesText", "rmlInventoryQuantityReceived", "rmlInventoryUnitCost", "rmlInventoryUnitCostForeign", "rmlInventoryUnitOfMeasure", "rmlClosed",
			"rmlInInspection", "rmlInspectionComplete", "rmlInvoicedComplete", "rmlJobReceivedComplete", "rmlKitPart", "rmlPoReceivedComplete", "rmlPostedToGl", "rmlRequiresInspection", "rmlReversed", "rmlTrackSerialNumbers",
			"rmlJobAssemblyID", "rmlJobEstimatedQuantity", "rmlJobID", "rmlJobMaterialID", "rmlJobMatQuantityReceived", "rmlJobOpenQuantity", "rmlJobOperationID", "rmlJobOprQuantityReceived", "rmlJobType", "rmlMiscUnitCost",
			"rmlOrgPartID", "rmlOrgPartShortDescription", "rmlPartBinID", "rmlPartID", "rmlPartLongDescriptionRtf", "rmlPartLongDescriptionText", "rmlPartRevisionID", "rmlPartWarehouseLocationID", "rmlPoOpenQuantity", "rmlPoPurchaseQuantity",
			"rmlProjectAreaID", "rmlProjectID", "rmlPurchaseOrderID", "rmlPurchaseOrderLineID", "rmlPurchaseQuantityReceived", "rmlPurchaseUnitCost", "rmlPurchaseUnitCostForeign", "rmlPurchaseUnitOfMeasure", "rmlQuantityToInspect", "rmlReceiptID",
			"rmlReference", "rmlReverseReceiptID", "rmlReverseReceiptLineID", "rmlRmaClaimID", "rmlRmaClaimLineID", "rmlRowVersion", "rmlSalesOrderDeliveryID", "rmlSalesOrderID", "rmlSalesOrderLineID", "rmlReceiptLineID",
			"rmlSetupCharge", "rmlSetupChargeForeign", "rmlTotalComponentCosts"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("ReceiptLines");
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
		using (DataTable dataTable = GetAsDataTable("ReceiptLines", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPReceiptLineInformationDto eRPReceiptLineInformationDto = new ERPReceiptLineInformationDto();
				eRPReceiptLineInformationDto.rmlConversionFactor = dataTable.Rows[i].Field<decimal>("rmlConversionFactor");
				eRPReceiptLineInformationDto.rmlCreatedBy = dataTable.Rows[i].Field<string>("rmlCreatedBy");
				eRPReceiptLineInformationDto.rmlCreatedDate = dataTable.Rows[i].Field<DateTime?>("rmlCreatedDate");
				eRPReceiptLineInformationDto.rmlDescription = dataTable.Rows[i].Field<string>("rmlDescription");
				eRPReceiptLineInformationDto.rmlDmrClaimID = dataTable.Rows[i].Field<string>("rmlDmrClaimID");
				eRPReceiptLineInformationDto.rmlDmrClaimLineID = dataTable.Rows[i].Field<short>("rmlDmrClaimLineID");
				eRPReceiptLineInformationDto.rmlDutyUnitCost = dataTable.Rows[i].Field<decimal>("rmlDutyUnitCost");
				eRPReceiptLineInformationDto.rmlUniqueID = dataTable.Rows[i].Field<Guid>("rmlUniqueID");
				eRPReceiptLineInformationDto.rmlExtendedCostBase = dataTable.Rows[i].Field<decimal>("rmlExtendedCostBase");
				eRPReceiptLineInformationDto.rmlExtendedCostForeign = dataTable.Rows[i].Field<decimal>("rmlExtendedCostForeign");
				eRPReceiptLineInformationDto.rmlForm1099Box = dataTable.Rows[i].Field<byte>("rmlForm1099Box");
				eRPReceiptLineInformationDto.rmlFreightUnitCost = dataTable.Rows[i].Field<decimal>("rmlFreightUnitCost");
				eRPReceiptLineInformationDto.rmlHeatLot = dataTable.Rows[i].Field<string>("rmlHeatLot");
				eRPReceiptLineInformationDto.rmlInspectionNotesRTF = dataTable.Rows[i].Field<string>("rmlInspectionNotesRTF");
				eRPReceiptLineInformationDto.rmlInspectionNotesText = dataTable.Rows[i].Field<string>("rmlInspectionNotesText");
				eRPReceiptLineInformationDto.rmlInventoryQuantityReceived = dataTable.Rows[i].Field<decimal>("rmlInventoryQuantityReceived");
				eRPReceiptLineInformationDto.rmlInventoryUnitCost = dataTable.Rows[i].Field<decimal>("rmlInventoryUnitCost");
				eRPReceiptLineInformationDto.rmlInventoryUnitCostForeign = dataTable.Rows[i].Field<decimal>("rmlInventoryUnitCostForeign");
				eRPReceiptLineInformationDto.rmlInventoryUnitOfMeasure = dataTable.Rows[i].Field<string>("rmlInventoryUnitOfMeasure");
				eRPReceiptLineInformationDto.rmlClosed = dataTable.Rows[i].Field<bool>("rmlClosed");
				eRPReceiptLineInformationDto.rmlInInspection = dataTable.Rows[i].Field<bool>("rmlInInspection");
				eRPReceiptLineInformationDto.rmlInspectionComplete = dataTable.Rows[i].Field<bool>("rmlInspectionComplete");
				eRPReceiptLineInformationDto.rmlInvoicedComplete = dataTable.Rows[i].Field<bool>("rmlInvoicedComplete");
				eRPReceiptLineInformationDto.rmlJobReceivedComplete = dataTable.Rows[i].Field<bool>("rmlJobReceivedComplete");
				eRPReceiptLineInformationDto.rmlKitPart = dataTable.Rows[i].Field<bool>("rmlKitPart");
				eRPReceiptLineInformationDto.rmlPoReceivedComplete = dataTable.Rows[i].Field<bool>("rmlPoReceivedComplete");
				eRPReceiptLineInformationDto.rmlPostedToGl = dataTable.Rows[i].Field<bool>("rmlPostedToGl");
				eRPReceiptLineInformationDto.rmlRequiresInspection = dataTable.Rows[i].Field<bool>("rmlRequiresInspection");
				eRPReceiptLineInformationDto.rmlReversed = dataTable.Rows[i].Field<bool>("rmlReversed");
				eRPReceiptLineInformationDto.rmlTrackSerialNumbers = dataTable.Rows[i].Field<bool>("rmlTrackSerialNumbers");
				eRPReceiptLineInformationDto.rmlJobAssemblyID = dataTable.Rows[i].Field<int>("rmlJobAssemblyID");
				eRPReceiptLineInformationDto.rmlJobEstimatedQuantity = dataTable.Rows[i].Field<decimal>("rmlJobEstimatedQuantity");
				eRPReceiptLineInformationDto.rmlJobID = dataTable.Rows[i].Field<string>("rmlJobID");
				eRPReceiptLineInformationDto.rmlJobMaterialID = dataTable.Rows[i].Field<int>("rmlJobMaterialID");
				eRPReceiptLineInformationDto.rmlJobMatQuantityReceived = dataTable.Rows[i].Field<decimal>("rmlJobMatQuantityReceived");
				eRPReceiptLineInformationDto.rmlJobOpenQuantity = dataTable.Rows[i].Field<decimal>("rmlJobOpenQuantity");
				eRPReceiptLineInformationDto.rmlJobOperationID = dataTable.Rows[i].Field<int>("rmlJobOperationID");
				eRPReceiptLineInformationDto.rmlJobOprQuantityReceived = dataTable.Rows[i].Field<decimal>("rmlJobOprQuantityReceived");
				eRPReceiptLineInformationDto.rmlJobType = dataTable.Rows[i].Field<byte>("rmlJobType");
				eRPReceiptLineInformationDto.rmlMiscUnitCost = dataTable.Rows[i].Field<decimal>("rmlMiscUnitCost");
				eRPReceiptLineInformationDto.rmlOrgPartID = dataTable.Rows[i].Field<string>("rmlOrgPartID");
				eRPReceiptLineInformationDto.rmlOrgPartShortDescription = dataTable.Rows[i].Field<string>("rmlOrgPartShortDescription");
				eRPReceiptLineInformationDto.rmlPartBinID = dataTable.Rows[i].Field<string>("rmlPartBinID");
				eRPReceiptLineInformationDto.rmlPartID = dataTable.Rows[i].Field<string>("rmlPartID");
				eRPReceiptLineInformationDto.rmlPartLongDescriptionRtf = dataTable.Rows[i].Field<string>("rmlPartLongDescriptionRtf");
				eRPReceiptLineInformationDto.rmlPartLongDescriptionText = dataTable.Rows[i].Field<string>("rmlPartLongDescriptionText");
				eRPReceiptLineInformationDto.rmlPartRevisionID = dataTable.Rows[i].Field<string>("rmlPartRevisionID");
				eRPReceiptLineInformationDto.rmlPartWarehouseLocationID = dataTable.Rows[i].Field<string>("rmlPartWarehouseLocationID");
				eRPReceiptLineInformationDto.rmlPoOpenQuantity = dataTable.Rows[i].Field<decimal>("rmlPoOpenQuantity");
				eRPReceiptLineInformationDto.rmlPoPurchaseQuantity = dataTable.Rows[i].Field<decimal>("rmlPoPurchaseQuantity");
				eRPReceiptLineInformationDto.rmlProjectAreaID = dataTable.Rows[i].Field<string>("rmlProjectAreaID");
				eRPReceiptLineInformationDto.rmlProjectID = dataTable.Rows[i].Field<string>("rmlProjectID");
				eRPReceiptLineInformationDto.rmlPurchaseOrderID = dataTable.Rows[i].Field<string>("rmlPurchaseOrderID");
				eRPReceiptLineInformationDto.rmlPurchaseOrderLineID = dataTable.Rows[i].Field<short>("rmlPurchaseOrderLineID");
				eRPReceiptLineInformationDto.rmlPurchaseQuantityReceived = dataTable.Rows[i].Field<decimal>("rmlPurchaseQuantityReceived");
				eRPReceiptLineInformationDto.rmlPurchaseUnitCost = dataTable.Rows[i].Field<decimal>("rmlPurchaseUnitCost");
				eRPReceiptLineInformationDto.rmlPurchaseUnitCostForeign = dataTable.Rows[i].Field<decimal>("rmlPurchaseUnitCostForeign");
				eRPReceiptLineInformationDto.rmlPurchaseUnitOfMeasure = dataTable.Rows[i].Field<string>("rmlPurchaseUnitOfMeasure");
				eRPReceiptLineInformationDto.rmlQuantityToInspect = dataTable.Rows[i].Field<decimal>("rmlQuantityToInspect");
				eRPReceiptLineInformationDto.rmlReceiptID = dataTable.Rows[i].Field<string>("rmlReceiptID");
				eRPReceiptLineInformationDto.rmlReference = dataTable.Rows[i].Field<string>("rmlReference");
				eRPReceiptLineInformationDto.rmlReverseReceiptID = dataTable.Rows[i].Field<string>("rmlReverseReceiptID");
				eRPReceiptLineInformationDto.rmlReverseReceiptLineID = dataTable.Rows[i].Field<short>("rmlReverseReceiptLineID");
				eRPReceiptLineInformationDto.rmlRmaClaimID = dataTable.Rows[i].Field<string>("rmlRmaClaimID");
				eRPReceiptLineInformationDto.rmlRmaClaimLineID = dataTable.Rows[i].Field<short>("rmlRmaClaimLineID");
				eRPReceiptLineInformationDto.rmlRowVersion = dataTable.Rows[i].Field<byte[]>("rmlRowVersion");
				eRPReceiptLineInformationDto.rmlSalesOrderDeliveryID = dataTable.Rows[i].Field<short>("rmlSalesOrderDeliveryID");
				eRPReceiptLineInformationDto.rmlSalesOrderID = dataTable.Rows[i].Field<string>("rmlSalesOrderID");
				eRPReceiptLineInformationDto.rmlSalesOrderLineID = dataTable.Rows[i].Field<short>("rmlSalesOrderLineID");
				eRPReceiptLineInformationDto.rmlReceiptLineID = dataTable.Rows[i].Field<short>("rmlReceiptLineID");
				eRPReceiptLineInformationDto.rmlSetupCharge = dataTable.Rows[i].Field<decimal>("rmlSetupCharge");
				eRPReceiptLineInformationDto.rmlSetupChargeForeign = dataTable.Rows[i].Field<decimal>("rmlSetupChargeForeign");
				eRPReceiptLineInformationDto.rmlTotalComponentCosts = dataTable.Rows[i].Field<decimal>("rmlTotalComponentCosts");
				eRPReceiptLineInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPReceiptLineInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPReceiptLineInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPReceiptLineInformationDto> GetReceiptLine(Guid receiptLineId)
	{
		ERPReceiptLineInformationDto eRPReceiptLineInformationDto = new ERPReceiptLineInformationDto();
		InitializeParameterLists();
		string[] collection = new string[73]
		{
			"rmlConversionFactor", "rmlCreatedBy", "rmlCreatedDate", "rmlDescription", "rmlDmrClaimID", "rmlDmrClaimLineID", "rmlDutyUnitCost", "rmlUniqueID", "rmlExtendedCostBase", "rmlExtendedCostForeign",
			"rmlForm1099Box", "rmlFreightUnitCost", "rmlHeatLot", "rmlInspectionNotesRTF", "rmlInspectionNotesText", "rmlInventoryQuantityReceived", "rmlInventoryUnitCost", "rmlInventoryUnitCostForeign", "rmlInventoryUnitOfMeasure", "rmlClosed",
			"rmlInInspection", "rmlInspectionComplete", "rmlInvoicedComplete", "rmlJobReceivedComplete", "rmlKitPart", "rmlPoReceivedComplete", "rmlPostedToGl", "rmlRequiresInspection", "rmlReversed", "rmlTrackSerialNumbers",
			"rmlJobAssemblyID", "rmlJobEstimatedQuantity", "rmlJobID", "rmlJobMaterialID", "rmlJobMatQuantityReceived", "rmlJobOpenQuantity", "rmlJobOperationID", "rmlJobOprQuantityReceived", "rmlJobType", "rmlMiscUnitCost",
			"rmlOrgPartID", "rmlOrgPartShortDescription", "rmlPartBinID", "rmlPartID", "rmlPartLongDescriptionRtf", "rmlPartLongDescriptionText", "rmlPartRevisionID", "rmlPartWarehouseLocationID", "rmlPoOpenQuantity", "rmlPoPurchaseQuantity",
			"rmlProjectAreaID", "rmlProjectID", "rmlPurchaseOrderID", "rmlPurchaseOrderLineID", "rmlPurchaseQuantityReceived", "rmlPurchaseUnitCost", "rmlPurchaseUnitCostForeign", "rmlPurchaseUnitOfMeasure", "rmlQuantityToInspect", "rmlReceiptID",
			"rmlReference", "rmlReverseReceiptID", "rmlReverseReceiptLineID", "rmlRmaClaimID", "rmlRmaClaimLineID", "rmlRowVersion", "rmlSalesOrderDeliveryID", "rmlSalesOrderID", "rmlSalesOrderLineID", "rmlReceiptLineID",
			"rmlSetupCharge", "rmlSetupChargeForeign", "rmlTotalComponentCosts"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("rmlUniqueID|C", receiptLineId);
		AddCustomFieldsToSelectList("ReceiptLines");
		using (DataTable dataTable = GetAsDataTable("ReceiptLines", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPReceiptLineInformationDto);
			}
			eRPReceiptLineInformationDto.rmlConversionFactor = dataTable.Rows[0].Field<decimal>("rmlConversionFactor");
			eRPReceiptLineInformationDto.rmlCreatedBy = dataTable.Rows[0].Field<string>("rmlCreatedBy");
			eRPReceiptLineInformationDto.rmlCreatedDate = dataTable.Rows[0].Field<DateTime?>("rmlCreatedDate");
			eRPReceiptLineInformationDto.rmlDescription = dataTable.Rows[0].Field<string>("rmlDescription");
			eRPReceiptLineInformationDto.rmlDmrClaimID = dataTable.Rows[0].Field<string>("rmlDmrClaimID");
			eRPReceiptLineInformationDto.rmlDmrClaimLineID = dataTable.Rows[0].Field<short>("rmlDmrClaimLineID");
			eRPReceiptLineInformationDto.rmlDutyUnitCost = dataTable.Rows[0].Field<decimal>("rmlDutyUnitCost");
			eRPReceiptLineInformationDto.rmlUniqueID = dataTable.Rows[0].Field<Guid>("rmlUniqueID");
			eRPReceiptLineInformationDto.rmlExtendedCostBase = dataTable.Rows[0].Field<decimal>("rmlExtendedCostBase");
			eRPReceiptLineInformationDto.rmlExtendedCostForeign = dataTable.Rows[0].Field<decimal>("rmlExtendedCostForeign");
			eRPReceiptLineInformationDto.rmlForm1099Box = dataTable.Rows[0].Field<byte>("rmlForm1099Box");
			eRPReceiptLineInformationDto.rmlFreightUnitCost = dataTable.Rows[0].Field<decimal>("rmlFreightUnitCost");
			eRPReceiptLineInformationDto.rmlHeatLot = dataTable.Rows[0].Field<string>("rmlHeatLot");
			eRPReceiptLineInformationDto.rmlInspectionNotesRTF = dataTable.Rows[0].Field<string>("rmlInspectionNotesRTF");
			eRPReceiptLineInformationDto.rmlInspectionNotesText = dataTable.Rows[0].Field<string>("rmlInspectionNotesText");
			eRPReceiptLineInformationDto.rmlInventoryQuantityReceived = dataTable.Rows[0].Field<decimal>("rmlInventoryQuantityReceived");
			eRPReceiptLineInformationDto.rmlInventoryUnitCost = dataTable.Rows[0].Field<decimal>("rmlInventoryUnitCost");
			eRPReceiptLineInformationDto.rmlInventoryUnitCostForeign = dataTable.Rows[0].Field<decimal>("rmlInventoryUnitCostForeign");
			eRPReceiptLineInformationDto.rmlInventoryUnitOfMeasure = dataTable.Rows[0].Field<string>("rmlInventoryUnitOfMeasure");
			eRPReceiptLineInformationDto.rmlClosed = dataTable.Rows[0].Field<bool>("rmlClosed");
			eRPReceiptLineInformationDto.rmlInInspection = dataTable.Rows[0].Field<bool>("rmlInInspection");
			eRPReceiptLineInformationDto.rmlInspectionComplete = dataTable.Rows[0].Field<bool>("rmlInspectionComplete");
			eRPReceiptLineInformationDto.rmlInvoicedComplete = dataTable.Rows[0].Field<bool>("rmlInvoicedComplete");
			eRPReceiptLineInformationDto.rmlJobReceivedComplete = dataTable.Rows[0].Field<bool>("rmlJobReceivedComplete");
			eRPReceiptLineInformationDto.rmlKitPart = dataTable.Rows[0].Field<bool>("rmlKitPart");
			eRPReceiptLineInformationDto.rmlPoReceivedComplete = dataTable.Rows[0].Field<bool>("rmlPoReceivedComplete");
			eRPReceiptLineInformationDto.rmlPostedToGl = dataTable.Rows[0].Field<bool>("rmlPostedToGl");
			eRPReceiptLineInformationDto.rmlRequiresInspection = dataTable.Rows[0].Field<bool>("rmlRequiresInspection");
			eRPReceiptLineInformationDto.rmlReversed = dataTable.Rows[0].Field<bool>("rmlReversed");
			eRPReceiptLineInformationDto.rmlTrackSerialNumbers = dataTable.Rows[0].Field<bool>("rmlTrackSerialNumbers");
			eRPReceiptLineInformationDto.rmlJobAssemblyID = dataTable.Rows[0].Field<int>("rmlJobAssemblyID");
			eRPReceiptLineInformationDto.rmlJobEstimatedQuantity = dataTable.Rows[0].Field<decimal>("rmlJobEstimatedQuantity");
			eRPReceiptLineInformationDto.rmlJobID = dataTable.Rows[0].Field<string>("rmlJobID");
			eRPReceiptLineInformationDto.rmlJobMaterialID = dataTable.Rows[0].Field<int>("rmlJobMaterialID");
			eRPReceiptLineInformationDto.rmlJobMatQuantityReceived = dataTable.Rows[0].Field<decimal>("rmlJobMatQuantityReceived");
			eRPReceiptLineInformationDto.rmlJobOpenQuantity = dataTable.Rows[0].Field<decimal>("rmlJobOpenQuantity");
			eRPReceiptLineInformationDto.rmlJobOperationID = dataTable.Rows[0].Field<int>("rmlJobOperationID");
			eRPReceiptLineInformationDto.rmlJobOprQuantityReceived = dataTable.Rows[0].Field<decimal>("rmlJobOprQuantityReceived");
			eRPReceiptLineInformationDto.rmlJobType = dataTable.Rows[0].Field<byte>("rmlJobType");
			eRPReceiptLineInformationDto.rmlMiscUnitCost = dataTable.Rows[0].Field<decimal>("rmlMiscUnitCost");
			eRPReceiptLineInformationDto.rmlOrgPartID = dataTable.Rows[0].Field<string>("rmlOrgPartID");
			eRPReceiptLineInformationDto.rmlOrgPartShortDescription = dataTable.Rows[0].Field<string>("rmlOrgPartShortDescription");
			eRPReceiptLineInformationDto.rmlPartBinID = dataTable.Rows[0].Field<string>("rmlPartBinID");
			eRPReceiptLineInformationDto.rmlPartID = dataTable.Rows[0].Field<string>("rmlPartID");
			eRPReceiptLineInformationDto.rmlPartLongDescriptionRtf = dataTable.Rows[0].Field<string>("rmlPartLongDescriptionRtf");
			eRPReceiptLineInformationDto.rmlPartLongDescriptionText = dataTable.Rows[0].Field<string>("rmlPartLongDescriptionText");
			eRPReceiptLineInformationDto.rmlPartRevisionID = dataTable.Rows[0].Field<string>("rmlPartRevisionID");
			eRPReceiptLineInformationDto.rmlPartWarehouseLocationID = dataTable.Rows[0].Field<string>("rmlPartWarehouseLocationID");
			eRPReceiptLineInformationDto.rmlPoOpenQuantity = dataTable.Rows[0].Field<decimal>("rmlPoOpenQuantity");
			eRPReceiptLineInformationDto.rmlPoPurchaseQuantity = dataTable.Rows[0].Field<decimal>("rmlPoPurchaseQuantity");
			eRPReceiptLineInformationDto.rmlProjectAreaID = dataTable.Rows[0].Field<string>("rmlProjectAreaID");
			eRPReceiptLineInformationDto.rmlProjectID = dataTable.Rows[0].Field<string>("rmlProjectID");
			eRPReceiptLineInformationDto.rmlPurchaseOrderID = dataTable.Rows[0].Field<string>("rmlPurchaseOrderID");
			eRPReceiptLineInformationDto.rmlPurchaseOrderLineID = dataTable.Rows[0].Field<short>("rmlPurchaseOrderLineID");
			eRPReceiptLineInformationDto.rmlPurchaseQuantityReceived = dataTable.Rows[0].Field<decimal>("rmlPurchaseQuantityReceived");
			eRPReceiptLineInformationDto.rmlPurchaseUnitCost = dataTable.Rows[0].Field<decimal>("rmlPurchaseUnitCost");
			eRPReceiptLineInformationDto.rmlPurchaseUnitCostForeign = dataTable.Rows[0].Field<decimal>("rmlPurchaseUnitCostForeign");
			eRPReceiptLineInformationDto.rmlPurchaseUnitOfMeasure = dataTable.Rows[0].Field<string>("rmlPurchaseUnitOfMeasure");
			eRPReceiptLineInformationDto.rmlQuantityToInspect = dataTable.Rows[0].Field<decimal>("rmlQuantityToInspect");
			eRPReceiptLineInformationDto.rmlReceiptID = dataTable.Rows[0].Field<string>("rmlReceiptID");
			eRPReceiptLineInformationDto.rmlReference = dataTable.Rows[0].Field<string>("rmlReference");
			eRPReceiptLineInformationDto.rmlReverseReceiptID = dataTable.Rows[0].Field<string>("rmlReverseReceiptID");
			eRPReceiptLineInformationDto.rmlReverseReceiptLineID = dataTable.Rows[0].Field<short>("rmlReverseReceiptLineID");
			eRPReceiptLineInformationDto.rmlRmaClaimID = dataTable.Rows[0].Field<string>("rmlRmaClaimID");
			eRPReceiptLineInformationDto.rmlRmaClaimLineID = dataTable.Rows[0].Field<short>("rmlRmaClaimLineID");
			eRPReceiptLineInformationDto.rmlRowVersion = dataTable.Rows[0].Field<byte[]>("rmlRowVersion");
			eRPReceiptLineInformationDto.rmlSalesOrderDeliveryID = dataTable.Rows[0].Field<short>("rmlSalesOrderDeliveryID");
			eRPReceiptLineInformationDto.rmlSalesOrderID = dataTable.Rows[0].Field<string>("rmlSalesOrderID");
			eRPReceiptLineInformationDto.rmlSalesOrderLineID = dataTable.Rows[0].Field<short>("rmlSalesOrderLineID");
			eRPReceiptLineInformationDto.rmlReceiptLineID = dataTable.Rows[0].Field<short>("rmlReceiptLineID");
			eRPReceiptLineInformationDto.rmlSetupCharge = dataTable.Rows[0].Field<decimal>("rmlSetupCharge");
			eRPReceiptLineInformationDto.rmlSetupChargeForeign = dataTable.Rows[0].Field<decimal>("rmlSetupChargeForeign");
			eRPReceiptLineInformationDto.rmlTotalComponentCosts = dataTable.Rows[0].Field<decimal>("rmlTotalComponentCosts");
			eRPReceiptLineInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPReceiptLineInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPReceiptLineInformationDto);
	}

	public Task<APIValidationInfoDto> SaveReceiptLine(ERPReceiptLineDto receiptLine)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM ReceiptLines WHERE rmlUniqueID = " + M1Util.ConvertToLinq(receiptLine.rmlUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["rmlReceiptID"] = receiptLine.rmlReceiptID.ToUpper();
				dataRow["rmlReceiptLineID"] = receiptLine.rmlReceiptLineID;
				receiptLine.rmlUniqueID = ((receiptLine.rmlUniqueID == Guid.Empty) ? Guid.NewGuid() : receiptLine.rmlUniqueID);
				dataRow["rmlUniqueID"] = receiptLine.rmlUniqueID;
				dataRow["rmlCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["rmlCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The ReceiptLine could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (receiptLine.rmlRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the ReceiptLine is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["rmlRowVersion"], receiptLine.rmlRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the ReceiptLine has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the ReceiptLine again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["rmlConversionFactor"] = receiptLine.rmlConversionFactor;
			dataRow["rmlDescription"] = receiptLine.rmlDescription;
			dataRow["rmlDmrClaimID"] = receiptLine.rmlDmrClaimID;
			dataRow["rmlDmrClaimLineID"] = receiptLine.rmlDmrClaimLineID;
			dataRow["rmlDutyUnitCost"] = receiptLine.rmlDutyUnitCost;
			dataRow["rmlExtendedCostBase"] = receiptLine.rmlExtendedCostBase;
			dataRow["rmlExtendedCostForeign"] = receiptLine.rmlExtendedCostForeign;
			dataRow["rmlForm1099Box"] = receiptLine.rmlForm1099Box;
			dataRow["rmlFreightUnitCost"] = receiptLine.rmlFreightUnitCost;
			dataRow["rmlHeatLot"] = receiptLine.rmlHeatLot;
			dataRow["rmlInspectionNotesRTF"] = receiptLine.rmlInspectionNotesRTF ?? dataRow["rmlInspectionNotesRTF"];
			dataRow["rmlInspectionNotesText"] = receiptLine.rmlInspectionNotesText ?? dataRow["rmlInspectionNotesText"];
			dataRow["rmlInventoryQuantityReceived"] = receiptLine.rmlInventoryQuantityReceived;
			dataRow["rmlInventoryUnitCost"] = receiptLine.rmlInventoryUnitCost;
			dataRow["rmlInventoryUnitCostForeign"] = receiptLine.rmlInventoryUnitCostForeign;
			dataRow["rmlInventoryUnitOfMeasure"] = receiptLine.rmlInventoryUnitOfMeasure;
			dataRow["rmlClosed"] = receiptLine.rmlClosed;
			dataRow["rmlInInspection"] = receiptLine.rmlInInspection;
			dataRow["rmlInspectionComplete"] = receiptLine.rmlInspectionComplete;
			dataRow["rmlInvoicedComplete"] = receiptLine.rmlInvoicedComplete;
			dataRow["rmlJobReceivedComplete"] = receiptLine.rmlJobReceivedComplete;
			dataRow["rmlKitPart"] = receiptLine.rmlKitPart;
			dataRow["rmlPoReceivedComplete"] = receiptLine.rmlPoReceivedComplete;
			dataRow["rmlPostedToGl"] = receiptLine.rmlPostedToGl;
			dataRow["rmlRequiresInspection"] = receiptLine.rmlRequiresInspection;
			dataRow["rmlReversed"] = receiptLine.rmlReversed;
			dataRow["rmlTrackSerialNumbers"] = receiptLine.rmlTrackSerialNumbers;
			dataRow["rmlJobAssemblyID"] = receiptLine.rmlJobAssemblyID;
			dataRow["rmlJobEstimatedQuantity"] = receiptLine.rmlJobEstimatedQuantity;
			dataRow["rmlJobID"] = receiptLine.rmlJobID;
			dataRow["rmlJobMaterialID"] = receiptLine.rmlJobMaterialID;
			dataRow["rmlJobMatQuantityReceived"] = receiptLine.rmlJobMatQuantityReceived;
			dataRow["rmlJobOpenQuantity"] = receiptLine.rmlJobOpenQuantity;
			dataRow["rmlJobOperationID"] = receiptLine.rmlJobOperationID;
			dataRow["rmlJobOprQuantityReceived"] = receiptLine.rmlJobOprQuantityReceived;
			dataRow["rmlJobType"] = receiptLine.rmlJobType;
			dataRow["rmlMiscUnitCost"] = receiptLine.rmlMiscUnitCost;
			dataRow["rmlOrgPartID"] = receiptLine.rmlOrgPartID;
			dataRow["rmlOrgPartShortDescription"] = receiptLine.rmlOrgPartShortDescription;
			dataRow["rmlPartBinID"] = receiptLine.rmlPartBinID;
			dataRow["rmlPartID"] = receiptLine.rmlPartID;
			dataRow["rmlPartLongDescriptionRtf"] = receiptLine.rmlPartLongDescriptionRtf ?? dataRow["rmlPartLongDescriptionRtf"];
			dataRow["rmlPartLongDescriptionText"] = receiptLine.rmlPartLongDescriptionText ?? dataRow["rmlPartLongDescriptionText"];
			dataRow["rmlPartRevisionID"] = receiptLine.rmlPartRevisionID;
			dataRow["rmlPartWarehouseLocationID"] = receiptLine.rmlPartWarehouseLocationID;
			dataRow["rmlPoOpenQuantity"] = receiptLine.rmlPoOpenQuantity;
			dataRow["rmlPoPurchaseQuantity"] = receiptLine.rmlPoPurchaseQuantity;
			dataRow["rmlProjectAreaID"] = receiptLine.rmlProjectAreaID;
			dataRow["rmlProjectID"] = receiptLine.rmlProjectID;
			dataRow["rmlPurchaseOrderID"] = receiptLine.rmlPurchaseOrderID;
			dataRow["rmlPurchaseOrderLineID"] = receiptLine.rmlPurchaseOrderLineID;
			dataRow["rmlPurchaseQuantityReceived"] = receiptLine.rmlPurchaseQuantityReceived;
			dataRow["rmlPurchaseUnitCost"] = receiptLine.rmlPurchaseUnitCost;
			dataRow["rmlPurchaseUnitCostForeign"] = receiptLine.rmlPurchaseUnitCostForeign;
			dataRow["rmlPurchaseUnitOfMeasure"] = receiptLine.rmlPurchaseUnitOfMeasure;
			dataRow["rmlQuantityToInspect"] = receiptLine.rmlQuantityToInspect;
			dataRow["rmlReference"] = receiptLine.rmlReference;
			dataRow["rmlReverseReceiptID"] = receiptLine.rmlReverseReceiptID;
			dataRow["rmlReverseReceiptLineID"] = receiptLine.rmlReverseReceiptLineID;
			dataRow["rmlRmaClaimID"] = receiptLine.rmlRmaClaimID;
			dataRow["rmlRmaClaimLineID"] = receiptLine.rmlRmaClaimLineID;
			dataRow["rmlSalesOrderDeliveryID"] = receiptLine.rmlSalesOrderDeliveryID;
			dataRow["rmlSalesOrderID"] = receiptLine.rmlSalesOrderID;
			dataRow["rmlSalesOrderLineID"] = receiptLine.rmlSalesOrderLineID;
			dataRow["rmlSetupCharge"] = receiptLine.rmlSetupCharge;
			dataRow["rmlSetupChargeForeign"] = receiptLine.rmlSetupChargeForeign;
			dataRow["rmlTotalComponentCosts"] = receiptLine.rmlTotalComponentCosts;
			if (receiptLine.CustomFields != null && receiptLine.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in receiptLine.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the ReceiptLine [{receiptLine.rmlUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the ReceiptLine [{receiptLine.rmlUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
