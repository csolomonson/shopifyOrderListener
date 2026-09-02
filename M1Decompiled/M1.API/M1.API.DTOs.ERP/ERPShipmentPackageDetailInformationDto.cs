using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPShipmentPackageDetailInformationDto
{
	public string spdCommodityDescription { get; set; }

	public string spdCountryOfManufacture { get; set; }

	public string spdCreatedBy { get; set; }

	public DateTime? spdCreatedDate { get; set; }

	public Guid spdUniqueID { get; set; }

	public string spdPartID { get; set; }

	public string spdPartRevisionID { get; set; }

	public decimal spdQuantity { get; set; }

	public byte[] SPDRowVersion { get; set; }

	public string spdShipmentID { get; set; }

	public string spdShipmentIDNumber { get; set; }

	public short spdShipmentLineID { get; set; }

	public int spdShipmentPackageID { get; set; }

	public int spdShipmentPackageLineID { get; set; }

	public decimal spdTotalPriceBase { get; set; }

	public decimal spdTotalPriceForeign { get; set; }

	public decimal spdWeight { get; set; }

	public string spdWeightUnitOfMeasure { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
