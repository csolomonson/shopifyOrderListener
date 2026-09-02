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

public class ERPDMRClaimLineRepository : APIBaseRepository, IERPDMRClaimLineRepository, IAPIBaseRepository, IDisposable
{
	public ERPDMRClaimLineRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesDMRClaimLineExist(Guid dMRClaimLineId)
	{
		InitializeParameterLists();
		base.filterList.Add("dmlUniqueID|C", dMRClaimLineId);
		base.selectList.Add("dmlUniqueID");
		return Task.FromResult(GetAsObject("DMRClaimLines", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPDMRClaimLineInformationDto>> GetAllDMRClaimLines(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPDMRClaimLineInformationDto> collection = new List<ERPDMRClaimLineInformationDto>();
		InitializeParameterLists();
		string[] array = new string[54]
		{
			"dmlConversionFactor", "dmlCreatedBy", "dmlCreatedDate", "dmlDmrClaimID", "dmlDmrShipmentID", "dmlDmrShipmentLineID", "dmlUniqueID", "dmlExtendedCost", "dmlExtendedCostForeign", "dmlInspectionID",
			"dmlInspectionLineID", "dmlInventoryQuantity", "dmlInventoryQuantityShipped", "dmlInventoryUnitOfMeasure", "dmlInvoicedComplete", "dmlKitPart", "dmlScrap", "dmlShippedComplete", "dmlTransferredToDmrShipment", "dmlTransferredToPurchaseOrder",
			"dmlJobAssemblyID", "dmlJobID", "dmlJobMaterialID", "dmlJobOperationID", "dmlOrgPartID", "dmlOrgPartShortDescription", "dmlPartBinID", "dmlPartID", "dmlPartLongDescriptionRtf", "dmlPartLongDescriptionText",
			"dmlPartRevisionID", "dmlPartShortDescription", "dmlPartWarehouseLocationID", "dmlProjectAreaID", "dmlProjectID", "dmlPurchaseOrderID", "dmlPurchaseOrderLineID", "dmlQuantity", "dmlQuantityShipped", "dmlReceiptID",
			"dmlReceiptLineID", "dmlReceivedDate", "dmlRequiredDate", "dmlReturnedDate", "dmlReturnReasonID", "dmlRowVersion", "dmlDmrClaimLineID", "dmlShippedDate", "dmlShippingMethodID", "dmlSupplierAuthorizationNumber",
			"dmlTrackingNumber", "dmlUnitCost", "dmlUnitCostForeign", "dmlUnitOfMeasure"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("DMRClaimLines");
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
		using (DataTable dataTable = GetAsDataTable("DMRClaimLines", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPDMRClaimLineInformationDto eRPDMRClaimLineInformationDto = new ERPDMRClaimLineInformationDto();
				eRPDMRClaimLineInformationDto.dmlConversionFactor = dataTable.Rows[i].Field<decimal>("dmlConversionFactor");
				eRPDMRClaimLineInformationDto.dmlCreatedBy = dataTable.Rows[i].Field<string>("dmlCreatedBy");
				eRPDMRClaimLineInformationDto.dmlCreatedDate = dataTable.Rows[i].Field<DateTime?>("dmlCreatedDate");
				eRPDMRClaimLineInformationDto.dmlDmrClaimID = dataTable.Rows[i].Field<string>("dmlDmrClaimID");
				eRPDMRClaimLineInformationDto.dmlDmrShipmentID = dataTable.Rows[i].Field<string>("dmlDmrShipmentID");
				eRPDMRClaimLineInformationDto.dmlDmrShipmentLineID = dataTable.Rows[i].Field<short>("dmlDmrShipmentLineID");
				eRPDMRClaimLineInformationDto.dmlUniqueID = dataTable.Rows[i].Field<Guid>("dmlUniqueID");
				eRPDMRClaimLineInformationDto.dmlExtendedCost = dataTable.Rows[i].Field<decimal>("dmlExtendedCost");
				eRPDMRClaimLineInformationDto.dmlExtendedCostForeign = dataTable.Rows[i].Field<decimal>("dmlExtendedCostForeign");
				eRPDMRClaimLineInformationDto.dmlInspectionID = dataTable.Rows[i].Field<string>("dmlInspectionID");
				eRPDMRClaimLineInformationDto.dmlInspectionLineID = dataTable.Rows[i].Field<short>("dmlInspectionLineID");
				eRPDMRClaimLineInformationDto.dmlInventoryQuantity = dataTable.Rows[i].Field<decimal>("dmlInventoryQuantity");
				eRPDMRClaimLineInformationDto.dmlInventoryQuantityShipped = dataTable.Rows[i].Field<decimal>("dmlInventoryQuantityShipped");
				eRPDMRClaimLineInformationDto.dmlInventoryUnitOfMeasure = dataTable.Rows[i].Field<string>("dmlInventoryUnitOfMeasure");
				eRPDMRClaimLineInformationDto.dmlInvoicedComplete = dataTable.Rows[i].Field<bool>("dmlInvoicedComplete");
				eRPDMRClaimLineInformationDto.dmlKitPart = dataTable.Rows[i].Field<bool>("dmlKitPart");
				eRPDMRClaimLineInformationDto.dmlScrap = dataTable.Rows[i].Field<bool>("dmlScrap");
				eRPDMRClaimLineInformationDto.dmlShippedComplete = dataTable.Rows[i].Field<bool>("dmlShippedComplete");
				eRPDMRClaimLineInformationDto.dmlTransferredToDmrShipment = dataTable.Rows[i].Field<bool>("dmlTransferredToDmrShipment");
				eRPDMRClaimLineInformationDto.dmlTransferredToPurchaseOrder = dataTable.Rows[i].Field<bool>("dmlTransferredToPurchaseOrder");
				eRPDMRClaimLineInformationDto.dmlJobAssemblyID = dataTable.Rows[i].Field<int>("dmlJobAssemblyID");
				eRPDMRClaimLineInformationDto.dmlJobID = dataTable.Rows[i].Field<string>("dmlJobID");
				eRPDMRClaimLineInformationDto.dmlJobMaterialID = dataTable.Rows[i].Field<int>("dmlJobMaterialID");
				eRPDMRClaimLineInformationDto.dmlJobOperationID = dataTable.Rows[i].Field<int>("dmlJobOperationID");
				eRPDMRClaimLineInformationDto.dmlOrgPartID = dataTable.Rows[i].Field<string>("dmlOrgPartID");
				eRPDMRClaimLineInformationDto.dmlOrgPartShortDescription = dataTable.Rows[i].Field<string>("dmlOrgPartShortDescription");
				eRPDMRClaimLineInformationDto.dmlPartBinID = dataTable.Rows[i].Field<string>("dmlPartBinID");
				eRPDMRClaimLineInformationDto.dmlPartID = dataTable.Rows[i].Field<string>("dmlPartID");
				eRPDMRClaimLineInformationDto.dmlPartLongDescriptionRtf = dataTable.Rows[i].Field<string>("dmlPartLongDescriptionRtf");
				eRPDMRClaimLineInformationDto.dmlPartLongDescriptionText = dataTable.Rows[i].Field<string>("dmlPartLongDescriptionText");
				eRPDMRClaimLineInformationDto.dmlPartRevisionID = dataTable.Rows[i].Field<string>("dmlPartRevisionID");
				eRPDMRClaimLineInformationDto.dmlPartShortDescription = dataTable.Rows[i].Field<string>("dmlPartShortDescription");
				eRPDMRClaimLineInformationDto.dmlPartWarehouseLocationID = dataTable.Rows[i].Field<string>("dmlPartWarehouseLocationID");
				eRPDMRClaimLineInformationDto.dmlProjectAreaID = dataTable.Rows[i].Field<string>("dmlProjectAreaID");
				eRPDMRClaimLineInformationDto.dmlProjectID = dataTable.Rows[i].Field<string>("dmlProjectID");
				eRPDMRClaimLineInformationDto.dmlPurchaseOrderID = dataTable.Rows[i].Field<string>("dmlPurchaseOrderID");
				eRPDMRClaimLineInformationDto.dmlPurchaseOrderLineID = dataTable.Rows[i].Field<short>("dmlPurchaseOrderLineID");
				eRPDMRClaimLineInformationDto.dmlQuantity = dataTable.Rows[i].Field<decimal>("dmlQuantity");
				eRPDMRClaimLineInformationDto.dmlQuantityShipped = dataTable.Rows[i].Field<decimal>("dmlQuantityShipped");
				eRPDMRClaimLineInformationDto.dmlReceiptID = dataTable.Rows[i].Field<string>("dmlReceiptID");
				eRPDMRClaimLineInformationDto.dmlReceiptLineID = dataTable.Rows[i].Field<short>("dmlReceiptLineID");
				eRPDMRClaimLineInformationDto.dmlReceivedDate = dataTable.Rows[i].Field<DateTime?>("dmlReceivedDate");
				eRPDMRClaimLineInformationDto.dmlRequiredDate = dataTable.Rows[i].Field<DateTime?>("dmlRequiredDate");
				eRPDMRClaimLineInformationDto.dmlReturnedDate = dataTable.Rows[i].Field<DateTime?>("dmlReturnedDate");
				eRPDMRClaimLineInformationDto.dmlReturnReasonID = dataTable.Rows[i].Field<string>("dmlReturnReasonID");
				eRPDMRClaimLineInformationDto.dmlRowVersion = dataTable.Rows[i].Field<byte[]>("dmlRowVersion");
				eRPDMRClaimLineInformationDto.dmlDmrClaimLineID = dataTable.Rows[i].Field<short>("dmlDmrClaimLineID");
				eRPDMRClaimLineInformationDto.dmlShippedDate = dataTable.Rows[i].Field<DateTime?>("dmlShippedDate");
				eRPDMRClaimLineInformationDto.dmlShippingMethodID = dataTable.Rows[i].Field<string>("dmlShippingMethodID");
				eRPDMRClaimLineInformationDto.dmlSupplierAuthorizationNumber = dataTable.Rows[i].Field<string>("dmlSupplierAuthorizationNumber");
				eRPDMRClaimLineInformationDto.dmlTrackingNumber = dataTable.Rows[i].Field<string>("dmlTrackingNumber");
				eRPDMRClaimLineInformationDto.dmlUnitCost = dataTable.Rows[i].Field<decimal>("dmlUnitCost");
				eRPDMRClaimLineInformationDto.dmlUnitCostForeign = dataTable.Rows[i].Field<decimal>("dmlUnitCostForeign");
				eRPDMRClaimLineInformationDto.dmlUnitOfMeasure = dataTable.Rows[i].Field<string>("dmlUnitOfMeasure");
				eRPDMRClaimLineInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPDMRClaimLineInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPDMRClaimLineInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPDMRClaimLineInformationDto> GetDMRClaimLine(Guid dMRClaimLineId)
	{
		ERPDMRClaimLineInformationDto eRPDMRClaimLineInformationDto = new ERPDMRClaimLineInformationDto();
		InitializeParameterLists();
		string[] collection = new string[54]
		{
			"dmlConversionFactor", "dmlCreatedBy", "dmlCreatedDate", "dmlDmrClaimID", "dmlDmrShipmentID", "dmlDmrShipmentLineID", "dmlUniqueID", "dmlExtendedCost", "dmlExtendedCostForeign", "dmlInspectionID",
			"dmlInspectionLineID", "dmlInventoryQuantity", "dmlInventoryQuantityShipped", "dmlInventoryUnitOfMeasure", "dmlInvoicedComplete", "dmlKitPart", "dmlScrap", "dmlShippedComplete", "dmlTransferredToDmrShipment", "dmlTransferredToPurchaseOrder",
			"dmlJobAssemblyID", "dmlJobID", "dmlJobMaterialID", "dmlJobOperationID", "dmlOrgPartID", "dmlOrgPartShortDescription", "dmlPartBinID", "dmlPartID", "dmlPartLongDescriptionRtf", "dmlPartLongDescriptionText",
			"dmlPartRevisionID", "dmlPartShortDescription", "dmlPartWarehouseLocationID", "dmlProjectAreaID", "dmlProjectID", "dmlPurchaseOrderID", "dmlPurchaseOrderLineID", "dmlQuantity", "dmlQuantityShipped", "dmlReceiptID",
			"dmlReceiptLineID", "dmlReceivedDate", "dmlRequiredDate", "dmlReturnedDate", "dmlReturnReasonID", "dmlRowVersion", "dmlDmrClaimLineID", "dmlShippedDate", "dmlShippingMethodID", "dmlSupplierAuthorizationNumber",
			"dmlTrackingNumber", "dmlUnitCost", "dmlUnitCostForeign", "dmlUnitOfMeasure"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("dmlUniqueID|C", dMRClaimLineId);
		AddCustomFieldsToSelectList("DMRClaimLines");
		using (DataTable dataTable = GetAsDataTable("DMRClaimLines", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPDMRClaimLineInformationDto);
			}
			eRPDMRClaimLineInformationDto.dmlConversionFactor = dataTable.Rows[0].Field<decimal>("dmlConversionFactor");
			eRPDMRClaimLineInformationDto.dmlCreatedBy = dataTable.Rows[0].Field<string>("dmlCreatedBy");
			eRPDMRClaimLineInformationDto.dmlCreatedDate = dataTable.Rows[0].Field<DateTime?>("dmlCreatedDate");
			eRPDMRClaimLineInformationDto.dmlDmrClaimID = dataTable.Rows[0].Field<string>("dmlDmrClaimID");
			eRPDMRClaimLineInformationDto.dmlDmrShipmentID = dataTable.Rows[0].Field<string>("dmlDmrShipmentID");
			eRPDMRClaimLineInformationDto.dmlDmrShipmentLineID = dataTable.Rows[0].Field<short>("dmlDmrShipmentLineID");
			eRPDMRClaimLineInformationDto.dmlUniqueID = dataTable.Rows[0].Field<Guid>("dmlUniqueID");
			eRPDMRClaimLineInformationDto.dmlExtendedCost = dataTable.Rows[0].Field<decimal>("dmlExtendedCost");
			eRPDMRClaimLineInformationDto.dmlExtendedCostForeign = dataTable.Rows[0].Field<decimal>("dmlExtendedCostForeign");
			eRPDMRClaimLineInformationDto.dmlInspectionID = dataTable.Rows[0].Field<string>("dmlInspectionID");
			eRPDMRClaimLineInformationDto.dmlInspectionLineID = dataTable.Rows[0].Field<short>("dmlInspectionLineID");
			eRPDMRClaimLineInformationDto.dmlInventoryQuantity = dataTable.Rows[0].Field<decimal>("dmlInventoryQuantity");
			eRPDMRClaimLineInformationDto.dmlInventoryQuantityShipped = dataTable.Rows[0].Field<decimal>("dmlInventoryQuantityShipped");
			eRPDMRClaimLineInformationDto.dmlInventoryUnitOfMeasure = dataTable.Rows[0].Field<string>("dmlInventoryUnitOfMeasure");
			eRPDMRClaimLineInformationDto.dmlInvoicedComplete = dataTable.Rows[0].Field<bool>("dmlInvoicedComplete");
			eRPDMRClaimLineInformationDto.dmlKitPart = dataTable.Rows[0].Field<bool>("dmlKitPart");
			eRPDMRClaimLineInformationDto.dmlScrap = dataTable.Rows[0].Field<bool>("dmlScrap");
			eRPDMRClaimLineInformationDto.dmlShippedComplete = dataTable.Rows[0].Field<bool>("dmlShippedComplete");
			eRPDMRClaimLineInformationDto.dmlTransferredToDmrShipment = dataTable.Rows[0].Field<bool>("dmlTransferredToDmrShipment");
			eRPDMRClaimLineInformationDto.dmlTransferredToPurchaseOrder = dataTable.Rows[0].Field<bool>("dmlTransferredToPurchaseOrder");
			eRPDMRClaimLineInformationDto.dmlJobAssemblyID = dataTable.Rows[0].Field<int>("dmlJobAssemblyID");
			eRPDMRClaimLineInformationDto.dmlJobID = dataTable.Rows[0].Field<string>("dmlJobID");
			eRPDMRClaimLineInformationDto.dmlJobMaterialID = dataTable.Rows[0].Field<int>("dmlJobMaterialID");
			eRPDMRClaimLineInformationDto.dmlJobOperationID = dataTable.Rows[0].Field<int>("dmlJobOperationID");
			eRPDMRClaimLineInformationDto.dmlOrgPartID = dataTable.Rows[0].Field<string>("dmlOrgPartID");
			eRPDMRClaimLineInformationDto.dmlOrgPartShortDescription = dataTable.Rows[0].Field<string>("dmlOrgPartShortDescription");
			eRPDMRClaimLineInformationDto.dmlPartBinID = dataTable.Rows[0].Field<string>("dmlPartBinID");
			eRPDMRClaimLineInformationDto.dmlPartID = dataTable.Rows[0].Field<string>("dmlPartID");
			eRPDMRClaimLineInformationDto.dmlPartLongDescriptionRtf = dataTable.Rows[0].Field<string>("dmlPartLongDescriptionRtf");
			eRPDMRClaimLineInformationDto.dmlPartLongDescriptionText = dataTable.Rows[0].Field<string>("dmlPartLongDescriptionText");
			eRPDMRClaimLineInformationDto.dmlPartRevisionID = dataTable.Rows[0].Field<string>("dmlPartRevisionID");
			eRPDMRClaimLineInformationDto.dmlPartShortDescription = dataTable.Rows[0].Field<string>("dmlPartShortDescription");
			eRPDMRClaimLineInformationDto.dmlPartWarehouseLocationID = dataTable.Rows[0].Field<string>("dmlPartWarehouseLocationID");
			eRPDMRClaimLineInformationDto.dmlProjectAreaID = dataTable.Rows[0].Field<string>("dmlProjectAreaID");
			eRPDMRClaimLineInformationDto.dmlProjectID = dataTable.Rows[0].Field<string>("dmlProjectID");
			eRPDMRClaimLineInformationDto.dmlPurchaseOrderID = dataTable.Rows[0].Field<string>("dmlPurchaseOrderID");
			eRPDMRClaimLineInformationDto.dmlPurchaseOrderLineID = dataTable.Rows[0].Field<short>("dmlPurchaseOrderLineID");
			eRPDMRClaimLineInformationDto.dmlQuantity = dataTable.Rows[0].Field<decimal>("dmlQuantity");
			eRPDMRClaimLineInformationDto.dmlQuantityShipped = dataTable.Rows[0].Field<decimal>("dmlQuantityShipped");
			eRPDMRClaimLineInformationDto.dmlReceiptID = dataTable.Rows[0].Field<string>("dmlReceiptID");
			eRPDMRClaimLineInformationDto.dmlReceiptLineID = dataTable.Rows[0].Field<short>("dmlReceiptLineID");
			eRPDMRClaimLineInformationDto.dmlReceivedDate = dataTable.Rows[0].Field<DateTime?>("dmlReceivedDate");
			eRPDMRClaimLineInformationDto.dmlRequiredDate = dataTable.Rows[0].Field<DateTime?>("dmlRequiredDate");
			eRPDMRClaimLineInformationDto.dmlReturnedDate = dataTable.Rows[0].Field<DateTime?>("dmlReturnedDate");
			eRPDMRClaimLineInformationDto.dmlReturnReasonID = dataTable.Rows[0].Field<string>("dmlReturnReasonID");
			eRPDMRClaimLineInformationDto.dmlRowVersion = dataTable.Rows[0].Field<byte[]>("dmlRowVersion");
			eRPDMRClaimLineInformationDto.dmlDmrClaimLineID = dataTable.Rows[0].Field<short>("dmlDmrClaimLineID");
			eRPDMRClaimLineInformationDto.dmlShippedDate = dataTable.Rows[0].Field<DateTime?>("dmlShippedDate");
			eRPDMRClaimLineInformationDto.dmlShippingMethodID = dataTable.Rows[0].Field<string>("dmlShippingMethodID");
			eRPDMRClaimLineInformationDto.dmlSupplierAuthorizationNumber = dataTable.Rows[0].Field<string>("dmlSupplierAuthorizationNumber");
			eRPDMRClaimLineInformationDto.dmlTrackingNumber = dataTable.Rows[0].Field<string>("dmlTrackingNumber");
			eRPDMRClaimLineInformationDto.dmlUnitCost = dataTable.Rows[0].Field<decimal>("dmlUnitCost");
			eRPDMRClaimLineInformationDto.dmlUnitCostForeign = dataTable.Rows[0].Field<decimal>("dmlUnitCostForeign");
			eRPDMRClaimLineInformationDto.dmlUnitOfMeasure = dataTable.Rows[0].Field<string>("dmlUnitOfMeasure");
			eRPDMRClaimLineInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPDMRClaimLineInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPDMRClaimLineInformationDto);
	}

	public Task<APIValidationInfoDto> SaveDMRClaimLine(ERPDMRClaimLineDto dMRClaimLine)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM DMRClaimLines WHERE dmlUniqueID = " + M1Util.ConvertToLinq(dMRClaimLine.dmlUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["dmlDmrClaimID"] = dMRClaimLine.dmlDmrClaimID.ToUpper();
				dataRow["dmlDmrClaimLineID"] = dMRClaimLine.dmlDmrClaimLineID;
				dMRClaimLine.dmlUniqueID = ((dMRClaimLine.dmlUniqueID == Guid.Empty) ? Guid.NewGuid() : dMRClaimLine.dmlUniqueID);
				dataRow["dmlUniqueID"] = dMRClaimLine.dmlUniqueID;
				dataRow["dmlCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["dmlCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The DMRClaimLine could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (dMRClaimLine.dmlRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the DMRClaimLine is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["dmlRowVersion"], dMRClaimLine.dmlRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the DMRClaimLine has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the DMRClaimLine again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["dmlConversionFactor"] = dMRClaimLine.dmlConversionFactor;
			dataRow["dmlDmrShipmentID"] = dMRClaimLine.dmlDmrShipmentID;
			dataRow["dmlDmrShipmentLineID"] = dMRClaimLine.dmlDmrShipmentLineID;
			dataRow["dmlExtendedCost"] = dMRClaimLine.dmlExtendedCost;
			dataRow["dmlExtendedCostForeign"] = dMRClaimLine.dmlExtendedCostForeign;
			dataRow["dmlInspectionID"] = dMRClaimLine.dmlInspectionID;
			dataRow["dmlInspectionLineID"] = dMRClaimLine.dmlInspectionLineID;
			dataRow["dmlInventoryQuantity"] = dMRClaimLine.dmlInventoryQuantity;
			dataRow["dmlInventoryQuantityShipped"] = dMRClaimLine.dmlInventoryQuantityShipped;
			dataRow["dmlInventoryUnitOfMeasure"] = dMRClaimLine.dmlInventoryUnitOfMeasure;
			dataRow["dmlInvoicedComplete"] = dMRClaimLine.dmlInvoicedComplete;
			dataRow["dmlKitPart"] = dMRClaimLine.dmlKitPart;
			dataRow["dmlScrap"] = dMRClaimLine.dmlScrap;
			dataRow["dmlShippedComplete"] = dMRClaimLine.dmlShippedComplete;
			dataRow["dmlTransferredToDmrShipment"] = dMRClaimLine.dmlTransferredToDmrShipment;
			dataRow["dmlTransferredToPurchaseOrder"] = dMRClaimLine.dmlTransferredToPurchaseOrder;
			dataRow["dmlJobAssemblyID"] = dMRClaimLine.dmlJobAssemblyID;
			dataRow["dmlJobID"] = dMRClaimLine.dmlJobID;
			dataRow["dmlJobMaterialID"] = dMRClaimLine.dmlJobMaterialID;
			dataRow["dmlJobOperationID"] = dMRClaimLine.dmlJobOperationID;
			dataRow["dmlOrgPartID"] = dMRClaimLine.dmlOrgPartID;
			dataRow["dmlOrgPartShortDescription"] = dMRClaimLine.dmlOrgPartShortDescription;
			dataRow["dmlPartBinID"] = dMRClaimLine.dmlPartBinID;
			dataRow["dmlPartID"] = dMRClaimLine.dmlPartID;
			dataRow["dmlPartLongDescriptionRtf"] = dMRClaimLine.dmlPartLongDescriptionRtf ?? dataRow["dmlPartLongDescriptionRtf"];
			dataRow["dmlPartLongDescriptionText"] = dMRClaimLine.dmlPartLongDescriptionText ?? dataRow["dmlPartLongDescriptionText"];
			dataRow["dmlPartRevisionID"] = dMRClaimLine.dmlPartRevisionID;
			dataRow["dmlPartShortDescription"] = dMRClaimLine.dmlPartShortDescription;
			dataRow["dmlPartWarehouseLocationID"] = dMRClaimLine.dmlPartWarehouseLocationID;
			dataRow["dmlProjectAreaID"] = dMRClaimLine.dmlProjectAreaID;
			dataRow["dmlProjectID"] = dMRClaimLine.dmlProjectID;
			dataRow["dmlPurchaseOrderID"] = dMRClaimLine.dmlPurchaseOrderID;
			dataRow["dmlPurchaseOrderLineID"] = dMRClaimLine.dmlPurchaseOrderLineID;
			dataRow["dmlQuantity"] = dMRClaimLine.dmlQuantity;
			dataRow["dmlQuantityShipped"] = dMRClaimLine.dmlQuantityShipped;
			dataRow["dmlReceiptID"] = dMRClaimLine.dmlReceiptID;
			dataRow["dmlReceiptLineID"] = dMRClaimLine.dmlReceiptLineID;
			DataRow dataRow2 = dataRow;
			DateTime? dmlReceivedDate = dMRClaimLine.dmlReceivedDate;
			dataRow2["dmlReceivedDate"] = (dmlReceivedDate.HasValue ? ((object)dmlReceivedDate.GetValueOrDefault()) : dataRow["dmlReceivedDate"]);
			DataRow dataRow3 = dataRow;
			dmlReceivedDate = dMRClaimLine.dmlRequiredDate;
			dataRow3["dmlRequiredDate"] = (dmlReceivedDate.HasValue ? ((object)dmlReceivedDate.GetValueOrDefault()) : dataRow["dmlRequiredDate"]);
			DataRow dataRow4 = dataRow;
			dmlReceivedDate = dMRClaimLine.dmlReturnedDate;
			dataRow4["dmlReturnedDate"] = (dmlReceivedDate.HasValue ? ((object)dmlReceivedDate.GetValueOrDefault()) : dataRow["dmlReturnedDate"]);
			dataRow["dmlReturnReasonID"] = dMRClaimLine.dmlReturnReasonID;
			DataRow dataRow5 = dataRow;
			dmlReceivedDate = dMRClaimLine.dmlShippedDate;
			dataRow5["dmlShippedDate"] = (dmlReceivedDate.HasValue ? ((object)dmlReceivedDate.GetValueOrDefault()) : dataRow["dmlShippedDate"]);
			dataRow["dmlShippingMethodID"] = dMRClaimLine.dmlShippingMethodID;
			dataRow["dmlSupplierAuthorizationNumber"] = dMRClaimLine.dmlSupplierAuthorizationNumber;
			dataRow["dmlTrackingNumber"] = dMRClaimLine.dmlTrackingNumber;
			dataRow["dmlUnitCost"] = dMRClaimLine.dmlUnitCost;
			dataRow["dmlUnitCostForeign"] = dMRClaimLine.dmlUnitCostForeign;
			dataRow["dmlUnitOfMeasure"] = dMRClaimLine.dmlUnitOfMeasure;
			if (dMRClaimLine.CustomFields != null && dMRClaimLine.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in dMRClaimLine.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the DMRClaimLine [{dMRClaimLine.dmlUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the DMRClaimLine [{dMRClaimLine.dmlUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
