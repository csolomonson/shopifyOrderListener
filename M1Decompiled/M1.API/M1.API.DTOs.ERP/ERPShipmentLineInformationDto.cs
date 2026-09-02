using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPShipmentLineInformationDto
{
	public string smlCreatedBy { get; set; }

	public DateTime? smlCreatedDate { get; set; }

	public string smlDescription { get; set; }

	public Guid smlUniqueID { get; set; }

	public decimal smlExtendedPriceBase { get; set; }

	public decimal smlExtendedPriceForeign { get; set; }

	public decimal smlExtendedWeight { get; set; }

	public decimal smlFreightAmount { get; set; }

	public decimal smlFreightAmountForeign { get; set; }

	public string smlHeatLot { get; set; }

	public bool smlClosed { get; set; }

	public bool smlInvoicedComplete { get; set; }

	public bool smlKitPart { get; set; }

	public bool smlOverridePrice { get; set; }

	public bool smlPostedToGl { get; set; }

	public bool smlRequiresInspection { get; set; }

	public bool smlReversed { get; set; }

	public bool smlShippedComplete { get; set; }

	public string smlJobID { get; set; }

	public decimal smlJobQuantityShipped { get; set; }

	public string smlOrgPartID { get; set; }

	public string smlOrgPartShortDescription { get; set; }

	public string smlPartBinID { get; set; }

	public string smlPartGroupID { get; set; }

	public string smlPartID { get; set; }

	public string smlPartLongDescriptionRtf { get; set; }

	public string smlPartLongDescriptionText { get; set; }

	public string smlPartRevisionID { get; set; }

	public string smlPartWarehouseLocationID { get; set; }

	public string smlProjectAreaID { get; set; }

	public string smlProjectID { get; set; }

	public decimal smlQuantityShipped { get; set; }

	public string smlReverseShipmentID { get; set; }

	public short smlReverseShipmentLineID { get; set; }

	public byte[] smlRowVersion { get; set; }

	public short smlSalesOrderDeliveryID { get; set; }

	public string smlSalesOrderID { get; set; }

	public short smlSalesOrderLineID { get; set; }

	public short smlShipmentLineID { get; set; }

	public string smlShipmentID { get; set; }

	public string smlShipmentIDNumber { get; set; }

	public decimal smlSODeliveryQuantity { get; set; }

	public decimal smlSOOpenQuantity { get; set; }

	public string smlSourceTableName { get; set; }

	public Guid smlSourceTableUniqueID { get; set; }

	public string smlUnitOfMeasure { get; set; }

	public decimal smlUnitPrice { get; set; }

	public decimal smlUnitPriceForeign { get; set; }

	public decimal smlWeight { get; set; }

	public string smlWeightUnitOfMeasure { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
