using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPDMRClaimLineInformationDto
{
	public decimal dmlConversionFactor { get; set; }

	public string dmlCreatedBy { get; set; }

	public DateTime? dmlCreatedDate { get; set; }

	public string dmlDmrClaimID { get; set; }

	public string dmlDmrShipmentID { get; set; }

	public short dmlDmrShipmentLineID { get; set; }

	public Guid dmlUniqueID { get; set; }

	public decimal dmlExtendedCost { get; set; }

	public decimal dmlExtendedCostForeign { get; set; }

	public string dmlInspectionID { get; set; }

	public short dmlInspectionLineID { get; set; }

	public decimal dmlInventoryQuantity { get; set; }

	public decimal dmlInventoryQuantityShipped { get; set; }

	public string dmlInventoryUnitOfMeasure { get; set; }

	public bool dmlInvoicedComplete { get; set; }

	public bool dmlKitPart { get; set; }

	public bool dmlScrap { get; set; }

	public bool dmlShippedComplete { get; set; }

	public bool dmlTransferredToDmrShipment { get; set; }

	public bool dmlTransferredToPurchaseOrder { get; set; }

	public int dmlJobAssemblyID { get; set; }

	public string dmlJobID { get; set; }

	public int dmlJobMaterialID { get; set; }

	public int dmlJobOperationID { get; set; }

	public string dmlOrgPartID { get; set; }

	public string dmlOrgPartShortDescription { get; set; }

	public string dmlPartBinID { get; set; }

	public string dmlPartID { get; set; }

	public string dmlPartLongDescriptionRtf { get; set; }

	public string dmlPartLongDescriptionText { get; set; }

	public string dmlPartRevisionID { get; set; }

	public string dmlPartShortDescription { get; set; }

	public string dmlPartWarehouseLocationID { get; set; }

	public string dmlProjectAreaID { get; set; }

	public string dmlProjectID { get; set; }

	public string dmlPurchaseOrderID { get; set; }

	public short dmlPurchaseOrderLineID { get; set; }

	public decimal dmlQuantity { get; set; }

	public decimal dmlQuantityShipped { get; set; }

	public string dmlReceiptID { get; set; }

	public short dmlReceiptLineID { get; set; }

	public DateTime? dmlReceivedDate { get; set; }

	public DateTime? dmlRequiredDate { get; set; }

	public DateTime? dmlReturnedDate { get; set; }

	public string dmlReturnReasonID { get; set; }

	public byte[] dmlRowVersion { get; set; }

	public short dmlDmrClaimLineID { get; set; }

	public DateTime? dmlShippedDate { get; set; }

	public string dmlShippingMethodID { get; set; }

	public string dmlSupplierAuthorizationNumber { get; set; }

	public string dmlTrackingNumber { get; set; }

	public decimal dmlUnitCost { get; set; }

	public decimal dmlUnitCostForeign { get; set; }

	public string dmlUnitOfMeasure { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
