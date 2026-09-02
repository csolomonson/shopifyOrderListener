using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace M1.API.DTOs.EDI;

[Serializable]
[DataContract(Namespace = "", Name = "edI856Shipment")]
public class EDI856OutboundASN
{
	public string ShipmentName { get; set; }

	[DataMember(Name = "shipmentNumber", Order = 1)]
	public string ShipmentNumber { get; set; }

	[DataMember(Name = "shipmentDate", Order = 2)]
	public string ShipmentDate { get; set; }

	[DataMember(Name = "customerOrganizationID", Order = 3)]
	public string CustomerOrganizationID { get; set; }

	[DataMember(Name = "shipFromLocation", Order = 4)]
	public OrganizationLocationAddressDto ShipFromLocation { get; set; }

	[DataMember(Name = "shipToLocation", Order = 5)]
	public OrganizationLocationAddressDto ShipToLocation { get; set; }

	[DataMember(Name = "billLocation", Order = 6)]
	public OrganizationLocationAddressDto BillLocation { get; set; }

	[DataMember(Name = "shipmentWeight", Order = 7)]
	public decimal ShipmentWeight { get; set; }

	[DataMember(Name = "carrierCode", Order = 8)]
	public string CarrierCode { get; set; }

	[DataMember(Name = "carrierReferenceNumber", Order = 9)]
	public string CarrierReferenceNumber { get; set; }

	[DataMember(Name = "shippingMethod", Order = 10)]
	public string ShippingMethod { get; set; }

	[DataMember(Name = "shippingCommentsText", Order = 11)]
	public string ShippingCommentsText { get; set; }

	[DataMember(Name = "edI856ShipmentLines", Order = 12)]
	public List<EDI856OutboundASNLineDto> EDI856ASNShipmentLines { get; set; }

	[DataMember(Name = "numberOfLineItems", Order = 13)]
	public int NumberOfLineItems { get; set; }

	[XmlIgnore]
	[JsonIgnore]
	[DataMember(Name = "edITransferred", Order = 14)]
	public bool EDITransferred { get; set; }
}
