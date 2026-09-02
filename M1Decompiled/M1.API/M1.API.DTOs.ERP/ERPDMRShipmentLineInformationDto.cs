using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPDMRShipmentLineInformationDto
{
	public decimal dslConversionFactor { get; set; }

	public string dslCreatedBy { get; set; }

	public DateTime? dslCreatedDate { get; set; }

	public string dslDescription { get; set; }

	public string dslDmrClaimID { get; set; }

	public short dslDmrClaimLineID { get; set; }

	public decimal dslDmrClaimQuantity { get; set; }

	public decimal dslDmrOpenQuantity { get; set; }

	public string dslDmrShipmentID { get; set; }

	public Guid dslUniqueID { get; set; }

	public string dslInspectionID { get; set; }

	public short dslInspectionLineID { get; set; }

	public decimal dslInventoryQuantityShipped { get; set; }

	public string dslInventoryUnitOfMeasure { get; set; }

	public bool dslClosed { get; set; }

	public bool dslInvoicedComplete { get; set; }

	public bool dslKitPart { get; set; }

	public bool dslPosted { get; set; }

	public bool dslReversed { get; set; }

	public bool dslShippedComplete { get; set; }

	public int dslJobAssemblyID { get; set; }

	public string dslJobID { get; set; }

	public int dslJobMaterialID { get; set; }

	public decimal dslJobMatQuantityShipped { get; set; }

	public int dslJobOperationID { get; set; }

	public decimal dslJobOprQuantityShipped { get; set; }

	public string dslPartBinID { get; set; }

	public string dslPartID { get; set; }

	public string dslPartLongDescriptionRtf { get; set; }

	public string dslPartLongDescriptionText { get; set; }

	public string dslPartRevisionID { get; set; }

	public string dslPartWarehouseLocationID { get; set; }

	public string dslProjectAreaID { get; set; }

	public string dslProjectID { get; set; }

	public decimal dslQuantityShipped { get; set; }

	public decimal dslReturnQuantityShipped { get; set; }

	public string dslReverseDmrShipmentID { get; set; }

	public short dslReverseDmrShipmentLineID { get; set; }

	public byte[] dslRowVersion { get; set; }

	public short dslDmrShipmentLineID { get; set; }

	public string dslUnitOfMeasure { get; set; }

	public decimal dslUnitPrice { get; set; }

	public decimal dslUnitPriceForeign { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
