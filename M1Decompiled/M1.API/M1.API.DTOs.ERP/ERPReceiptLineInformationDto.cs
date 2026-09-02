using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPReceiptLineInformationDto
{
	public decimal rmlConversionFactor { get; set; }

	public string rmlCreatedBy { get; set; }

	public DateTime? rmlCreatedDate { get; set; }

	public string rmlDescription { get; set; }

	public string rmlDmrClaimID { get; set; }

	public short rmlDmrClaimLineID { get; set; }

	public decimal rmlDutyUnitCost { get; set; }

	public Guid rmlUniqueID { get; set; }

	public decimal rmlExtendedCostBase { get; set; }

	public decimal rmlExtendedCostForeign { get; set; }

	public byte rmlForm1099Box { get; set; }

	public decimal rmlFreightUnitCost { get; set; }

	public string rmlHeatLot { get; set; }

	public string rmlInspectionNotesRTF { get; set; }

	public string rmlInspectionNotesText { get; set; }

	public decimal rmlInventoryQuantityReceived { get; set; }

	public decimal rmlInventoryUnitCost { get; set; }

	public decimal rmlInventoryUnitCostForeign { get; set; }

	public string rmlInventoryUnitOfMeasure { get; set; }

	public bool rmlClosed { get; set; }

	public bool rmlInInspection { get; set; }

	public bool rmlInspectionComplete { get; set; }

	public bool rmlInvoicedComplete { get; set; }

	public bool rmlJobReceivedComplete { get; set; }

	public bool rmlKitPart { get; set; }

	public bool rmlPoReceivedComplete { get; set; }

	public bool rmlPostedToGl { get; set; }

	public bool rmlRequiresInspection { get; set; }

	public bool rmlReversed { get; set; }

	public bool rmlTrackSerialNumbers { get; set; }

	public int rmlJobAssemblyID { get; set; }

	public decimal rmlJobEstimatedQuantity { get; set; }

	public string rmlJobID { get; set; }

	public int rmlJobMaterialID { get; set; }

	public decimal rmlJobMatQuantityReceived { get; set; }

	public decimal rmlJobOpenQuantity { get; set; }

	public int rmlJobOperationID { get; set; }

	public decimal rmlJobOprQuantityReceived { get; set; }

	public byte rmlJobType { get; set; }

	public decimal rmlMiscUnitCost { get; set; }

	public string rmlOrgPartID { get; set; }

	public string rmlOrgPartShortDescription { get; set; }

	public string rmlPartBinID { get; set; }

	public string rmlPartID { get; set; }

	public string rmlPartLongDescriptionRtf { get; set; }

	public string rmlPartLongDescriptionText { get; set; }

	public string rmlPartRevisionID { get; set; }

	public string rmlPartWarehouseLocationID { get; set; }

	public decimal rmlPoOpenQuantity { get; set; }

	public decimal rmlPoPurchaseQuantity { get; set; }

	public string rmlProjectAreaID { get; set; }

	public string rmlProjectID { get; set; }

	public string rmlPurchaseOrderID { get; set; }

	public short rmlPurchaseOrderLineID { get; set; }

	public decimal rmlPurchaseQuantityReceived { get; set; }

	public decimal rmlPurchaseUnitCost { get; set; }

	public decimal rmlPurchaseUnitCostForeign { get; set; }

	public string rmlPurchaseUnitOfMeasure { get; set; }

	public decimal rmlQuantityToInspect { get; set; }

	public string rmlReceiptID { get; set; }

	public string rmlReference { get; set; }

	public string rmlReverseReceiptID { get; set; }

	public short rmlReverseReceiptLineID { get; set; }

	public string rmlRmaClaimID { get; set; }

	public short rmlRmaClaimLineID { get; set; }

	public byte[] rmlRowVersion { get; set; }

	public short rmlSalesOrderDeliveryID { get; set; }

	public string rmlSalesOrderID { get; set; }

	public short rmlSalesOrderLineID { get; set; }

	public short rmlReceiptLineID { get; set; }

	public decimal rmlSetupCharge { get; set; }

	public decimal rmlSetupChargeForeign { get; set; }

	public decimal rmlTotalComponentCosts { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
