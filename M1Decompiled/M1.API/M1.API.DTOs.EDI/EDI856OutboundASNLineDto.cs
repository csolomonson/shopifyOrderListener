using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace M1.API.DTOs.EDI;

[Serializable]
[DataContract(Namespace = "", Name = "edI856ShipmentLine")]
public class EDI856OutboundASNLineDto
{
	[DataMember(Name = "shipmentLineNo", Order = 1)]
	public int ShipmentLineNo { get; set; }

	[DataMember(Name = "customerPO", Order = 2)]
	public string CustomerPO { get; set; }

	[DataMember(Name = "orderDate", Order = 3)]
	public string OrderDate { get; set; }

	[DataMember(Name = "salesOrderID", Order = 4)]
	public string SalesOrderID { get; set; }

	[DataMember(Name = "salesOrderLineID", Order = 5)]
	public int SalesOrderLineID { get; set; }

	[DataMember(Name = "salesOrderDeliveryID", Order = 6)]
	public int SalesOrderDeliveryID { get; set; }

	[DataMember(Name = "releaseNumber", Order = 7)]
	public string ReleaseNumber { get; set; }

	[DataMember(Name = "vendorItemNo", Order = 8)]
	public string VendorItemNo { get; set; }

	[DataMember(Name = "partID", Order = 9)]
	public string PartID { get; set; }

	[DataMember(Name = "engineeringLevel", Order = 10)]
	public string EngineeringLevel { get; set; }

	[DataMember(Name = "partShortDescription", Order = 11)]
	public string PartShortDescription { get; set; }

	[DataMember(Name = "shipmentLineWeight", Order = 12)]
	public decimal Weight { get; set; }

	[DataMember(Name = "partWeightUOM", Order = 13)]
	public string PartWeightUOM { get; set; }

	[DataMember(Name = "partCountryOfManufacture", Order = 14)]
	public string PartCountryOfManufacture { get; set; }

	[DataMember(Name = "shipmentLineQuantity", Order = 15)]
	public decimal ShipmentQuantity { get; set; }

	[DataMember(Name = "quantityUOM", Order = 16)]
	public string QuantityUOM { get; set; }

	[DataMember(Name = "itemPrice", Order = 17)]
	public decimal ItemPrice { get; set; }

	[DataMember(Name = "countryofOrigin", Order = 18)]
	public string CountryofOrigin { get; set; }

	[DataMember(Name = "edI856ShipmentPackages", Order = 19)]
	public List<EDI856ASNOutboundPackageDto> EDI856ASNShipmentPackages { get; set; }
}
