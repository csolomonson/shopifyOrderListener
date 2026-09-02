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

public class ERPMfgReceiptRepository : APIBaseRepository, IERPMfgReceiptRepository, IAPIBaseRepository, IDisposable
{
	public ERPMfgReceiptRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesMfgReceiptExist(Guid mfgReceiptId)
	{
		InitializeParameterLists();
		base.filterList.Add("rmmUniqueID|C", mfgReceiptId);
		base.selectList.Add("rmmUniqueID");
		return Task.FromResult(GetAsObject("MfgReceipts", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPMfgReceiptInformationDto>> GetAllMfgReceipts(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPMfgReceiptInformationDto> collection = new List<ERPMfgReceiptInformationDto>();
		InitializeParameterLists();
		string[] array = new string[73]
		{
			"rmmMfgReceiptID", "rmmCreatedBy", "rmmCreatedDate", "rmmUniqueID", "rmmEstimatedQuantity", "rmmExtendedCostBase", "rmmHeatLot", "rmmImCostingMethod", "rmmInventoryQuantity", "rmmInventoryQuantityReceived",
			"rmmInventoryUnitOfMeasure", "rmmCreateJobSeq", "rmmInInspection", "rmmInspectionComplete", "rmmKitPart", "rmmNotUpdateJobQtyComplete", "rmmPoLineReceivedComplete", "rmmPosted", "rmmProductionComplete", "rmmReceivedComplete",
			"rmmRequiresInspection", "rmmReversalEntry", "rmmReversed", "rmmJobAsmQuantityReceived", "rmmJobAssemblyID", "rmmJobID", "rmmJobMaterialID", "rmmJobMatQuantityReceived", "rmmJobOpenQuantity", "rmmJobOperationID",
			"rmmJobOprQuantityReceived", "rmmJobScrapQuantity", "rmmJobType", "rmmLongDescriptionRtf", "rmmLongDescriptionText", "rmmMfgCostType", "rmmMiscInvQuantityReceived", "rmmPartBinID", "rmmPartID", "rmmPartRevisionID",
			"rmmPartWarehouseLocationID", "rmmPlantDepartmentID", "rmmPlantID", "rmmPoOpenQuantity", "rmmPostedDate", "rmmProductionQuantity", "rmmProjectAreaID", "rmmProjectID", "rmmPurchaseLocationID", "rmmPurchaseOrderID",
			"rmmPurchaseOrderLineID", "rmmPurchaseQuantity", "rmmPurchaseQuantityReceived", "rmmPurchaseUnitCost", "rmmPurchaseUnitOfMeasure", "rmmQuantityCompleted", "rmmQuantityOnHand", "rmmQuantityReceivedToInventory", "rmmQuantityToInspect", "rmmReceiptDate",
			"rmmReceiptType", "rmmReference", "rmmReverseMfgReceiptID", "rmmRowVersion", "rmmScrapQuantity", "rmmSetupCharge", "rmmSupplierOrganizationID", "rmmTotalComponentCosts", "rmmTotalUnitCost", "rmmUnitLaborCost",
			"rmmUnitMaterialCost", "rmmUnitOverheadCost", "rmmUnitSubcontractCost"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("MfgReceipts");
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
		using (DataTable dataTable = GetAsDataTable("MfgReceipts", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPMfgReceiptInformationDto eRPMfgReceiptInformationDto = new ERPMfgReceiptInformationDto();
				eRPMfgReceiptInformationDto.rmmMfgReceiptID = dataTable.Rows[i].Field<string>("rmmMfgReceiptID");
				eRPMfgReceiptInformationDto.rmmCreatedBy = dataTable.Rows[i].Field<string>("rmmCreatedBy");
				eRPMfgReceiptInformationDto.rmmCreatedDate = dataTable.Rows[i].Field<DateTime?>("rmmCreatedDate");
				eRPMfgReceiptInformationDto.rmmUniqueID = dataTable.Rows[i].Field<Guid>("rmmUniqueID");
				eRPMfgReceiptInformationDto.rmmEstimatedQuantity = dataTable.Rows[i].Field<decimal>("rmmEstimatedQuantity");
				eRPMfgReceiptInformationDto.rmmExtendedCostBase = dataTable.Rows[i].Field<decimal>("rmmExtendedCostBase");
				eRPMfgReceiptInformationDto.rmmHeatLot = dataTable.Rows[i].Field<string>("rmmHeatLot");
				eRPMfgReceiptInformationDto.rmmImCostingMethod = dataTable.Rows[i].Field<byte>("rmmImCostingMethod");
				eRPMfgReceiptInformationDto.rmmInventoryQuantity = dataTable.Rows[i].Field<decimal>("rmmInventoryQuantity");
				eRPMfgReceiptInformationDto.rmmInventoryQuantityReceived = dataTable.Rows[i].Field<decimal>("rmmInventoryQuantityReceived");
				eRPMfgReceiptInformationDto.rmmInventoryUnitOfMeasure = dataTable.Rows[i].Field<string>("rmmInventoryUnitOfMeasure");
				eRPMfgReceiptInformationDto.rmmCreateJobSeq = dataTable.Rows[i].Field<bool>("rmmCreateJobSeq");
				eRPMfgReceiptInformationDto.rmmInInspection = dataTable.Rows[i].Field<bool>("rmmInInspection");
				eRPMfgReceiptInformationDto.rmmInspectionComplete = dataTable.Rows[i].Field<bool>("rmmInspectionComplete");
				eRPMfgReceiptInformationDto.rmmKitPart = dataTable.Rows[i].Field<bool>("rmmKitPart");
				eRPMfgReceiptInformationDto.rmmNotUpdateJobQtyComplete = dataTable.Rows[i].Field<bool>("rmmNotUpdateJobQtyComplete");
				eRPMfgReceiptInformationDto.rmmPoLineReceivedComplete = dataTable.Rows[i].Field<bool>("rmmPoLineReceivedComplete");
				eRPMfgReceiptInformationDto.rmmPosted = dataTable.Rows[i].Field<bool>("rmmPosted");
				eRPMfgReceiptInformationDto.rmmProductionComplete = dataTable.Rows[i].Field<bool>("rmmProductionComplete");
				eRPMfgReceiptInformationDto.rmmReceivedComplete = dataTable.Rows[i].Field<bool>("rmmReceivedComplete");
				eRPMfgReceiptInformationDto.rmmRequiresInspection = dataTable.Rows[i].Field<bool>("rmmRequiresInspection");
				eRPMfgReceiptInformationDto.rmmReversalEntry = dataTable.Rows[i].Field<bool>("rmmReversalEntry");
				eRPMfgReceiptInformationDto.rmmReversed = dataTable.Rows[i].Field<bool>("rmmReversed");
				eRPMfgReceiptInformationDto.rmmJobAsmQuantityReceived = dataTable.Rows[i].Field<decimal>("rmmJobAsmQuantityReceived");
				eRPMfgReceiptInformationDto.rmmJobAssemblyID = dataTable.Rows[i].Field<int>("rmmJobAssemblyID");
				eRPMfgReceiptInformationDto.rmmJobID = dataTable.Rows[i].Field<string>("rmmJobID");
				eRPMfgReceiptInformationDto.rmmJobMaterialID = dataTable.Rows[i].Field<int>("rmmJobMaterialID");
				eRPMfgReceiptInformationDto.rmmJobMatQuantityReceived = dataTable.Rows[i].Field<decimal>("rmmJobMatQuantityReceived");
				eRPMfgReceiptInformationDto.rmmJobOpenQuantity = dataTable.Rows[i].Field<decimal>("rmmJobOpenQuantity");
				eRPMfgReceiptInformationDto.rmmJobOperationID = dataTable.Rows[i].Field<int>("rmmJobOperationID");
				eRPMfgReceiptInformationDto.rmmJobOprQuantityReceived = dataTable.Rows[i].Field<decimal>("rmmJobOprQuantityReceived");
				eRPMfgReceiptInformationDto.rmmJobScrapQuantity = dataTable.Rows[i].Field<decimal>("rmmJobScrapQuantity");
				eRPMfgReceiptInformationDto.rmmJobType = dataTable.Rows[i].Field<byte>("rmmJobType");
				eRPMfgReceiptInformationDto.rmmLongDescriptionRtf = dataTable.Rows[i].Field<string>("rmmLongDescriptionRtf");
				eRPMfgReceiptInformationDto.rmmLongDescriptionText = dataTable.Rows[i].Field<string>("rmmLongDescriptionText");
				eRPMfgReceiptInformationDto.rmmMfgCostType = dataTable.Rows[i].Field<byte>("rmmMfgCostType");
				eRPMfgReceiptInformationDto.rmmMiscInvQuantityReceived = dataTable.Rows[i].Field<decimal>("rmmMiscInvQuantityReceived");
				eRPMfgReceiptInformationDto.rmmPartBinID = dataTable.Rows[i].Field<string>("rmmPartBinID");
				eRPMfgReceiptInformationDto.rmmPartID = dataTable.Rows[i].Field<string>("rmmPartID");
				eRPMfgReceiptInformationDto.rmmPartRevisionID = dataTable.Rows[i].Field<string>("rmmPartRevisionID");
				eRPMfgReceiptInformationDto.rmmPartWarehouseLocationID = dataTable.Rows[i].Field<string>("rmmPartWarehouseLocationID");
				eRPMfgReceiptInformationDto.rmmPlantDepartmentID = dataTable.Rows[i].Field<string>("rmmPlantDepartmentID");
				eRPMfgReceiptInformationDto.rmmPlantID = dataTable.Rows[i].Field<string>("rmmPlantID");
				eRPMfgReceiptInformationDto.rmmPoOpenQuantity = dataTable.Rows[i].Field<decimal>("rmmPoOpenQuantity");
				eRPMfgReceiptInformationDto.rmmPostedDate = dataTable.Rows[i].Field<DateTime?>("rmmPostedDate");
				eRPMfgReceiptInformationDto.rmmProductionQuantity = dataTable.Rows[i].Field<decimal>("rmmProductionQuantity");
				eRPMfgReceiptInformationDto.rmmProjectAreaID = dataTable.Rows[i].Field<string>("rmmProjectAreaID");
				eRPMfgReceiptInformationDto.rmmProjectID = dataTable.Rows[i].Field<string>("rmmProjectID");
				eRPMfgReceiptInformationDto.rmmPurchaseLocationID = dataTable.Rows[i].Field<string>("rmmPurchaseLocationID");
				eRPMfgReceiptInformationDto.rmmPurchaseOrderID = dataTable.Rows[i].Field<string>("rmmPurchaseOrderID");
				eRPMfgReceiptInformationDto.rmmPurchaseOrderLineID = dataTable.Rows[i].Field<short>("rmmPurchaseOrderLineID");
				eRPMfgReceiptInformationDto.rmmPurchaseQuantity = dataTable.Rows[i].Field<decimal>("rmmPurchaseQuantity");
				eRPMfgReceiptInformationDto.rmmPurchaseQuantityReceived = dataTable.Rows[i].Field<decimal>("rmmPurchaseQuantityReceived");
				eRPMfgReceiptInformationDto.rmmPurchaseUnitCost = dataTable.Rows[i].Field<decimal>("rmmPurchaseUnitCost");
				eRPMfgReceiptInformationDto.rmmPurchaseUnitOfMeasure = dataTable.Rows[i].Field<string>("rmmPurchaseUnitOfMeasure");
				eRPMfgReceiptInformationDto.rmmQuantityCompleted = dataTable.Rows[i].Field<decimal>("rmmQuantityCompleted");
				eRPMfgReceiptInformationDto.rmmQuantityOnHand = dataTable.Rows[i].Field<decimal>("rmmQuantityOnHand");
				eRPMfgReceiptInformationDto.rmmQuantityReceivedToInventory = dataTable.Rows[i].Field<decimal>("rmmQuantityReceivedToInventory");
				eRPMfgReceiptInformationDto.rmmQuantityToInspect = dataTable.Rows[i].Field<decimal>("rmmQuantityToInspect");
				eRPMfgReceiptInformationDto.rmmReceiptDate = dataTable.Rows[i].Field<DateTime?>("rmmReceiptDate");
				eRPMfgReceiptInformationDto.rmmReceiptType = dataTable.Rows[i].Field<byte>("rmmReceiptType");
				eRPMfgReceiptInformationDto.rmmReference = dataTable.Rows[i].Field<string>("rmmReference");
				eRPMfgReceiptInformationDto.rmmReverseMfgReceiptID = dataTable.Rows[i].Field<string>("rmmReverseMfgReceiptID");
				eRPMfgReceiptInformationDto.rmmRowVersion = dataTable.Rows[i].Field<byte[]>("rmmRowVersion");
				eRPMfgReceiptInformationDto.rmmScrapQuantity = dataTable.Rows[i].Field<decimal>("rmmScrapQuantity");
				eRPMfgReceiptInformationDto.rmmSetupCharge = dataTable.Rows[i].Field<decimal>("rmmSetupCharge");
				eRPMfgReceiptInformationDto.rmmSupplierOrganizationID = dataTable.Rows[i].Field<string>("rmmSupplierOrganizationID");
				eRPMfgReceiptInformationDto.rmmTotalComponentCosts = dataTable.Rows[i].Field<decimal>("rmmTotalComponentCosts");
				eRPMfgReceiptInformationDto.rmmTotalUnitCost = dataTable.Rows[i].Field<decimal>("rmmTotalUnitCost");
				eRPMfgReceiptInformationDto.rmmUnitLaborCost = dataTable.Rows[i].Field<decimal>("rmmUnitLaborCost");
				eRPMfgReceiptInformationDto.rmmUnitMaterialCost = dataTable.Rows[i].Field<decimal>("rmmUnitMaterialCost");
				eRPMfgReceiptInformationDto.rmmUnitOverheadCost = dataTable.Rows[i].Field<decimal>("rmmUnitOverheadCost");
				eRPMfgReceiptInformationDto.rmmUnitSubcontractCost = dataTable.Rows[i].Field<decimal>("rmmUnitSubcontractCost");
				eRPMfgReceiptInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPMfgReceiptInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPMfgReceiptInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPMfgReceiptInformationDto> GetMfgReceipt(Guid mfgReceiptId)
	{
		ERPMfgReceiptInformationDto eRPMfgReceiptInformationDto = new ERPMfgReceiptInformationDto();
		InitializeParameterLists();
		string[] collection = new string[73]
		{
			"rmmMfgReceiptID", "rmmCreatedBy", "rmmCreatedDate", "rmmUniqueID", "rmmEstimatedQuantity", "rmmExtendedCostBase", "rmmHeatLot", "rmmImCostingMethod", "rmmInventoryQuantity", "rmmInventoryQuantityReceived",
			"rmmInventoryUnitOfMeasure", "rmmCreateJobSeq", "rmmInInspection", "rmmInspectionComplete", "rmmKitPart", "rmmNotUpdateJobQtyComplete", "rmmPoLineReceivedComplete", "rmmPosted", "rmmProductionComplete", "rmmReceivedComplete",
			"rmmRequiresInspection", "rmmReversalEntry", "rmmReversed", "rmmJobAsmQuantityReceived", "rmmJobAssemblyID", "rmmJobID", "rmmJobMaterialID", "rmmJobMatQuantityReceived", "rmmJobOpenQuantity", "rmmJobOperationID",
			"rmmJobOprQuantityReceived", "rmmJobScrapQuantity", "rmmJobType", "rmmLongDescriptionRtf", "rmmLongDescriptionText", "rmmMfgCostType", "rmmMiscInvQuantityReceived", "rmmPartBinID", "rmmPartID", "rmmPartRevisionID",
			"rmmPartWarehouseLocationID", "rmmPlantDepartmentID", "rmmPlantID", "rmmPoOpenQuantity", "rmmPostedDate", "rmmProductionQuantity", "rmmProjectAreaID", "rmmProjectID", "rmmPurchaseLocationID", "rmmPurchaseOrderID",
			"rmmPurchaseOrderLineID", "rmmPurchaseQuantity", "rmmPurchaseQuantityReceived", "rmmPurchaseUnitCost", "rmmPurchaseUnitOfMeasure", "rmmQuantityCompleted", "rmmQuantityOnHand", "rmmQuantityReceivedToInventory", "rmmQuantityToInspect", "rmmReceiptDate",
			"rmmReceiptType", "rmmReference", "rmmReverseMfgReceiptID", "rmmRowVersion", "rmmScrapQuantity", "rmmSetupCharge", "rmmSupplierOrganizationID", "rmmTotalComponentCosts", "rmmTotalUnitCost", "rmmUnitLaborCost",
			"rmmUnitMaterialCost", "rmmUnitOverheadCost", "rmmUnitSubcontractCost"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("rmmUniqueID|C", mfgReceiptId);
		AddCustomFieldsToSelectList("MfgReceipts");
		using (DataTable dataTable = GetAsDataTable("MfgReceipts", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPMfgReceiptInformationDto);
			}
			eRPMfgReceiptInformationDto.rmmMfgReceiptID = dataTable.Rows[0].Field<string>("rmmMfgReceiptID");
			eRPMfgReceiptInformationDto.rmmCreatedBy = dataTable.Rows[0].Field<string>("rmmCreatedBy");
			eRPMfgReceiptInformationDto.rmmCreatedDate = dataTable.Rows[0].Field<DateTime?>("rmmCreatedDate");
			eRPMfgReceiptInformationDto.rmmUniqueID = dataTable.Rows[0].Field<Guid>("rmmUniqueID");
			eRPMfgReceiptInformationDto.rmmEstimatedQuantity = dataTable.Rows[0].Field<decimal>("rmmEstimatedQuantity");
			eRPMfgReceiptInformationDto.rmmExtendedCostBase = dataTable.Rows[0].Field<decimal>("rmmExtendedCostBase");
			eRPMfgReceiptInformationDto.rmmHeatLot = dataTable.Rows[0].Field<string>("rmmHeatLot");
			eRPMfgReceiptInformationDto.rmmImCostingMethod = dataTable.Rows[0].Field<byte>("rmmImCostingMethod");
			eRPMfgReceiptInformationDto.rmmInventoryQuantity = dataTable.Rows[0].Field<decimal>("rmmInventoryQuantity");
			eRPMfgReceiptInformationDto.rmmInventoryQuantityReceived = dataTable.Rows[0].Field<decimal>("rmmInventoryQuantityReceived");
			eRPMfgReceiptInformationDto.rmmInventoryUnitOfMeasure = dataTable.Rows[0].Field<string>("rmmInventoryUnitOfMeasure");
			eRPMfgReceiptInformationDto.rmmCreateJobSeq = dataTable.Rows[0].Field<bool>("rmmCreateJobSeq");
			eRPMfgReceiptInformationDto.rmmInInspection = dataTable.Rows[0].Field<bool>("rmmInInspection");
			eRPMfgReceiptInformationDto.rmmInspectionComplete = dataTable.Rows[0].Field<bool>("rmmInspectionComplete");
			eRPMfgReceiptInformationDto.rmmKitPart = dataTable.Rows[0].Field<bool>("rmmKitPart");
			eRPMfgReceiptInformationDto.rmmNotUpdateJobQtyComplete = dataTable.Rows[0].Field<bool>("rmmNotUpdateJobQtyComplete");
			eRPMfgReceiptInformationDto.rmmPoLineReceivedComplete = dataTable.Rows[0].Field<bool>("rmmPoLineReceivedComplete");
			eRPMfgReceiptInformationDto.rmmPosted = dataTable.Rows[0].Field<bool>("rmmPosted");
			eRPMfgReceiptInformationDto.rmmProductionComplete = dataTable.Rows[0].Field<bool>("rmmProductionComplete");
			eRPMfgReceiptInformationDto.rmmReceivedComplete = dataTable.Rows[0].Field<bool>("rmmReceivedComplete");
			eRPMfgReceiptInformationDto.rmmRequiresInspection = dataTable.Rows[0].Field<bool>("rmmRequiresInspection");
			eRPMfgReceiptInformationDto.rmmReversalEntry = dataTable.Rows[0].Field<bool>("rmmReversalEntry");
			eRPMfgReceiptInformationDto.rmmReversed = dataTable.Rows[0].Field<bool>("rmmReversed");
			eRPMfgReceiptInformationDto.rmmJobAsmQuantityReceived = dataTable.Rows[0].Field<decimal>("rmmJobAsmQuantityReceived");
			eRPMfgReceiptInformationDto.rmmJobAssemblyID = dataTable.Rows[0].Field<int>("rmmJobAssemblyID");
			eRPMfgReceiptInformationDto.rmmJobID = dataTable.Rows[0].Field<string>("rmmJobID");
			eRPMfgReceiptInformationDto.rmmJobMaterialID = dataTable.Rows[0].Field<int>("rmmJobMaterialID");
			eRPMfgReceiptInformationDto.rmmJobMatQuantityReceived = dataTable.Rows[0].Field<decimal>("rmmJobMatQuantityReceived");
			eRPMfgReceiptInformationDto.rmmJobOpenQuantity = dataTable.Rows[0].Field<decimal>("rmmJobOpenQuantity");
			eRPMfgReceiptInformationDto.rmmJobOperationID = dataTable.Rows[0].Field<int>("rmmJobOperationID");
			eRPMfgReceiptInformationDto.rmmJobOprQuantityReceived = dataTable.Rows[0].Field<decimal>("rmmJobOprQuantityReceived");
			eRPMfgReceiptInformationDto.rmmJobScrapQuantity = dataTable.Rows[0].Field<decimal>("rmmJobScrapQuantity");
			eRPMfgReceiptInformationDto.rmmJobType = dataTable.Rows[0].Field<byte>("rmmJobType");
			eRPMfgReceiptInformationDto.rmmLongDescriptionRtf = dataTable.Rows[0].Field<string>("rmmLongDescriptionRtf");
			eRPMfgReceiptInformationDto.rmmLongDescriptionText = dataTable.Rows[0].Field<string>("rmmLongDescriptionText");
			eRPMfgReceiptInformationDto.rmmMfgCostType = dataTable.Rows[0].Field<byte>("rmmMfgCostType");
			eRPMfgReceiptInformationDto.rmmMiscInvQuantityReceived = dataTable.Rows[0].Field<decimal>("rmmMiscInvQuantityReceived");
			eRPMfgReceiptInformationDto.rmmPartBinID = dataTable.Rows[0].Field<string>("rmmPartBinID");
			eRPMfgReceiptInformationDto.rmmPartID = dataTable.Rows[0].Field<string>("rmmPartID");
			eRPMfgReceiptInformationDto.rmmPartRevisionID = dataTable.Rows[0].Field<string>("rmmPartRevisionID");
			eRPMfgReceiptInformationDto.rmmPartWarehouseLocationID = dataTable.Rows[0].Field<string>("rmmPartWarehouseLocationID");
			eRPMfgReceiptInformationDto.rmmPlantDepartmentID = dataTable.Rows[0].Field<string>("rmmPlantDepartmentID");
			eRPMfgReceiptInformationDto.rmmPlantID = dataTable.Rows[0].Field<string>("rmmPlantID");
			eRPMfgReceiptInformationDto.rmmPoOpenQuantity = dataTable.Rows[0].Field<decimal>("rmmPoOpenQuantity");
			eRPMfgReceiptInformationDto.rmmPostedDate = dataTable.Rows[0].Field<DateTime?>("rmmPostedDate");
			eRPMfgReceiptInformationDto.rmmProductionQuantity = dataTable.Rows[0].Field<decimal>("rmmProductionQuantity");
			eRPMfgReceiptInformationDto.rmmProjectAreaID = dataTable.Rows[0].Field<string>("rmmProjectAreaID");
			eRPMfgReceiptInformationDto.rmmProjectID = dataTable.Rows[0].Field<string>("rmmProjectID");
			eRPMfgReceiptInformationDto.rmmPurchaseLocationID = dataTable.Rows[0].Field<string>("rmmPurchaseLocationID");
			eRPMfgReceiptInformationDto.rmmPurchaseOrderID = dataTable.Rows[0].Field<string>("rmmPurchaseOrderID");
			eRPMfgReceiptInformationDto.rmmPurchaseOrderLineID = dataTable.Rows[0].Field<short>("rmmPurchaseOrderLineID");
			eRPMfgReceiptInformationDto.rmmPurchaseQuantity = dataTable.Rows[0].Field<decimal>("rmmPurchaseQuantity");
			eRPMfgReceiptInformationDto.rmmPurchaseQuantityReceived = dataTable.Rows[0].Field<decimal>("rmmPurchaseQuantityReceived");
			eRPMfgReceiptInformationDto.rmmPurchaseUnitCost = dataTable.Rows[0].Field<decimal>("rmmPurchaseUnitCost");
			eRPMfgReceiptInformationDto.rmmPurchaseUnitOfMeasure = dataTable.Rows[0].Field<string>("rmmPurchaseUnitOfMeasure");
			eRPMfgReceiptInformationDto.rmmQuantityCompleted = dataTable.Rows[0].Field<decimal>("rmmQuantityCompleted");
			eRPMfgReceiptInformationDto.rmmQuantityOnHand = dataTable.Rows[0].Field<decimal>("rmmQuantityOnHand");
			eRPMfgReceiptInformationDto.rmmQuantityReceivedToInventory = dataTable.Rows[0].Field<decimal>("rmmQuantityReceivedToInventory");
			eRPMfgReceiptInformationDto.rmmQuantityToInspect = dataTable.Rows[0].Field<decimal>("rmmQuantityToInspect");
			eRPMfgReceiptInformationDto.rmmReceiptDate = dataTable.Rows[0].Field<DateTime?>("rmmReceiptDate");
			eRPMfgReceiptInformationDto.rmmReceiptType = dataTable.Rows[0].Field<byte>("rmmReceiptType");
			eRPMfgReceiptInformationDto.rmmReference = dataTable.Rows[0].Field<string>("rmmReference");
			eRPMfgReceiptInformationDto.rmmReverseMfgReceiptID = dataTable.Rows[0].Field<string>("rmmReverseMfgReceiptID");
			eRPMfgReceiptInformationDto.rmmRowVersion = dataTable.Rows[0].Field<byte[]>("rmmRowVersion");
			eRPMfgReceiptInformationDto.rmmScrapQuantity = dataTable.Rows[0].Field<decimal>("rmmScrapQuantity");
			eRPMfgReceiptInformationDto.rmmSetupCharge = dataTable.Rows[0].Field<decimal>("rmmSetupCharge");
			eRPMfgReceiptInformationDto.rmmSupplierOrganizationID = dataTable.Rows[0].Field<string>("rmmSupplierOrganizationID");
			eRPMfgReceiptInformationDto.rmmTotalComponentCosts = dataTable.Rows[0].Field<decimal>("rmmTotalComponentCosts");
			eRPMfgReceiptInformationDto.rmmTotalUnitCost = dataTable.Rows[0].Field<decimal>("rmmTotalUnitCost");
			eRPMfgReceiptInformationDto.rmmUnitLaborCost = dataTable.Rows[0].Field<decimal>("rmmUnitLaborCost");
			eRPMfgReceiptInformationDto.rmmUnitMaterialCost = dataTable.Rows[0].Field<decimal>("rmmUnitMaterialCost");
			eRPMfgReceiptInformationDto.rmmUnitOverheadCost = dataTable.Rows[0].Field<decimal>("rmmUnitOverheadCost");
			eRPMfgReceiptInformationDto.rmmUnitSubcontractCost = dataTable.Rows[0].Field<decimal>("rmmUnitSubcontractCost");
			eRPMfgReceiptInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPMfgReceiptInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPMfgReceiptInformationDto);
	}

	public Task<APIValidationInfoDto> SaveMfgReceipt(ERPMfgReceiptDto mfgReceipt)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM MfgReceipts WHERE rmmUniqueID = " + M1Util.ConvertToLinq(mfgReceipt.rmmUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["rmmMfgReceiptID"] = mfgReceipt.rmmMfgReceiptID.ToUpper();
				mfgReceipt.rmmUniqueID = ((mfgReceipt.rmmUniqueID == Guid.Empty) ? Guid.NewGuid() : mfgReceipt.rmmUniqueID);
				dataRow["rmmUniqueID"] = mfgReceipt.rmmUniqueID;
				dataRow["rmmCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["rmmCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The MfgReceipt could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (mfgReceipt.rmmRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the MfgReceipt is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["rmmRowVersion"], mfgReceipt.rmmRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the MfgReceipt has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the MfgReceipt again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["rmmEstimatedQuantity"] = mfgReceipt.rmmEstimatedQuantity;
			dataRow["rmmExtendedCostBase"] = mfgReceipt.rmmExtendedCostBase;
			dataRow["rmmHeatLot"] = mfgReceipt.rmmHeatLot;
			dataRow["rmmImCostingMethod"] = mfgReceipt.rmmImCostingMethod;
			dataRow["rmmInventoryQuantity"] = mfgReceipt.rmmInventoryQuantity;
			dataRow["rmmInventoryQuantityReceived"] = mfgReceipt.rmmInventoryQuantityReceived;
			dataRow["rmmInventoryUnitOfMeasure"] = mfgReceipt.rmmInventoryUnitOfMeasure;
			dataRow["rmmCreateJobSeq"] = mfgReceipt.rmmCreateJobSeq;
			dataRow["rmmInInspection"] = mfgReceipt.rmmInInspection;
			dataRow["rmmInspectionComplete"] = mfgReceipt.rmmInspectionComplete;
			dataRow["rmmKitPart"] = mfgReceipt.rmmKitPart;
			dataRow["rmmNotUpdateJobQtyComplete"] = mfgReceipt.rmmNotUpdateJobQtyComplete;
			dataRow["rmmPoLineReceivedComplete"] = mfgReceipt.rmmPoLineReceivedComplete;
			dataRow["rmmPosted"] = mfgReceipt.rmmPosted;
			dataRow["rmmProductionComplete"] = mfgReceipt.rmmProductionComplete;
			dataRow["rmmReceivedComplete"] = mfgReceipt.rmmReceivedComplete;
			dataRow["rmmRequiresInspection"] = mfgReceipt.rmmRequiresInspection;
			dataRow["rmmReversalEntry"] = mfgReceipt.rmmReversalEntry;
			dataRow["rmmReversed"] = mfgReceipt.rmmReversed;
			dataRow["rmmJobAsmQuantityReceived"] = mfgReceipt.rmmJobAsmQuantityReceived;
			dataRow["rmmJobAssemblyID"] = mfgReceipt.rmmJobAssemblyID;
			dataRow["rmmJobID"] = mfgReceipt.rmmJobID;
			dataRow["rmmJobMaterialID"] = mfgReceipt.rmmJobMaterialID;
			dataRow["rmmJobMatQuantityReceived"] = mfgReceipt.rmmJobMatQuantityReceived;
			dataRow["rmmJobOpenQuantity"] = mfgReceipt.rmmJobOpenQuantity;
			dataRow["rmmJobOperationID"] = mfgReceipt.rmmJobOperationID;
			dataRow["rmmJobOprQuantityReceived"] = mfgReceipt.rmmJobOprQuantityReceived;
			dataRow["rmmJobScrapQuantity"] = mfgReceipt.rmmJobScrapQuantity;
			dataRow["rmmJobType"] = mfgReceipt.rmmJobType;
			dataRow["rmmLongDescriptionRtf"] = mfgReceipt.rmmLongDescriptionRtf ?? dataRow["rmmLongDescriptionRtf"];
			dataRow["rmmLongDescriptionText"] = mfgReceipt.rmmLongDescriptionText ?? dataRow["rmmLongDescriptionText"];
			dataRow["rmmMfgCostType"] = mfgReceipt.rmmMfgCostType;
			dataRow["rmmMiscInvQuantityReceived"] = mfgReceipt.rmmMiscInvQuantityReceived;
			dataRow["rmmPartBinID"] = mfgReceipt.rmmPartBinID;
			dataRow["rmmPartID"] = mfgReceipt.rmmPartID;
			dataRow["rmmPartRevisionID"] = mfgReceipt.rmmPartRevisionID;
			dataRow["rmmPartWarehouseLocationID"] = mfgReceipt.rmmPartWarehouseLocationID;
			dataRow["rmmPlantDepartmentID"] = mfgReceipt.rmmPlantDepartmentID;
			dataRow["rmmPlantID"] = mfgReceipt.rmmPlantID;
			dataRow["rmmPoOpenQuantity"] = mfgReceipt.rmmPoOpenQuantity;
			DataRow dataRow2 = dataRow;
			DateTime? rmmPostedDate = mfgReceipt.rmmPostedDate;
			dataRow2["rmmPostedDate"] = (rmmPostedDate.HasValue ? ((object)rmmPostedDate.GetValueOrDefault()) : dataRow["rmmPostedDate"]);
			dataRow["rmmProductionQuantity"] = mfgReceipt.rmmProductionQuantity;
			dataRow["rmmProjectAreaID"] = mfgReceipt.rmmProjectAreaID;
			dataRow["rmmProjectID"] = mfgReceipt.rmmProjectID;
			dataRow["rmmPurchaseLocationID"] = mfgReceipt.rmmPurchaseLocationID;
			dataRow["rmmPurchaseOrderID"] = mfgReceipt.rmmPurchaseOrderID;
			dataRow["rmmPurchaseOrderLineID"] = mfgReceipt.rmmPurchaseOrderLineID;
			dataRow["rmmPurchaseQuantity"] = mfgReceipt.rmmPurchaseQuantity;
			dataRow["rmmPurchaseQuantityReceived"] = mfgReceipt.rmmPurchaseQuantityReceived;
			dataRow["rmmPurchaseUnitCost"] = mfgReceipt.rmmPurchaseUnitCost;
			dataRow["rmmPurchaseUnitOfMeasure"] = mfgReceipt.rmmPurchaseUnitOfMeasure;
			dataRow["rmmQuantityCompleted"] = mfgReceipt.rmmQuantityCompleted;
			dataRow["rmmQuantityOnHand"] = mfgReceipt.rmmQuantityOnHand;
			dataRow["rmmQuantityReceivedToInventory"] = mfgReceipt.rmmQuantityReceivedToInventory;
			dataRow["rmmQuantityToInspect"] = mfgReceipt.rmmQuantityToInspect;
			DataRow dataRow3 = dataRow;
			rmmPostedDate = mfgReceipt.rmmReceiptDate;
			dataRow3["rmmReceiptDate"] = (rmmPostedDate.HasValue ? ((object)rmmPostedDate.GetValueOrDefault()) : dataRow["rmmReceiptDate"]);
			dataRow["rmmReceiptType"] = mfgReceipt.rmmReceiptType;
			dataRow["rmmReference"] = mfgReceipt.rmmReference;
			dataRow["rmmReverseMfgReceiptID"] = mfgReceipt.rmmReverseMfgReceiptID;
			dataRow["rmmScrapQuantity"] = mfgReceipt.rmmScrapQuantity;
			dataRow["rmmSetupCharge"] = mfgReceipt.rmmSetupCharge;
			dataRow["rmmSupplierOrganizationID"] = mfgReceipt.rmmSupplierOrganizationID;
			dataRow["rmmTotalComponentCosts"] = mfgReceipt.rmmTotalComponentCosts;
			dataRow["rmmTotalUnitCost"] = mfgReceipt.rmmTotalUnitCost;
			dataRow["rmmUnitLaborCost"] = mfgReceipt.rmmUnitLaborCost;
			dataRow["rmmUnitMaterialCost"] = mfgReceipt.rmmUnitMaterialCost;
			dataRow["rmmUnitOverheadCost"] = mfgReceipt.rmmUnitOverheadCost;
			dataRow["rmmUnitSubcontractCost"] = mfgReceipt.rmmUnitSubcontractCost;
			if (mfgReceipt.CustomFields != null && mfgReceipt.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in mfgReceipt.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the MfgReceipt [{mfgReceipt.rmmUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the MfgReceipt [{mfgReceipt.rmmUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
