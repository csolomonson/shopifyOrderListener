using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace M1.API.DTOs.Core;

[Serializable]
[DataContract(Namespace = "")]
public class SalesOrderDeliveryDto
{
	[DataMember(Name = "SalesOrderID", Order = 1)]
	public string SalesOrderID { get; set; }

	[DataMember(Name = "SalesOrderLineID", Order = 2)]
	public short SalesOrderLineID { get; set; }

	[DataMember(Name = "SalesOrderDeliveryID", Order = 3)]
	public short SalesOrderDeliveryID { get; set; }

	[XmlIgnore]
	public string CustomerOrganizationID { get; set; }

	[DataMember(Name = "PartID", Order = 4)]
	public string PartID { get; set; }

	[DataMember(Name = "PartRevisionID", Order = 5)]
	public string PartRevisionID { get; set; }

	[DataMember(Name = "PartWarehouseLocationID", Order = 6)]
	public string PartWarehouseLocationID { get; set; }

	[DataMember(Name = "PartBinID", Order = 7)]
	public string PartBinID { get; set; }

	[DataMember(Name = "DeliveryQuantity", Order = 8)]
	public decimal DeliveryQuantity { get; set; }

	[DataMember(Name = "DeliveryDate", Order = 9)]
	public DateTime DeliveryDate { get; set; }

	[DataMember(Name = "DeliveryType", Order = 10)]
	public byte DeliveryType { get; set; }

	[DataMember(Name = "Firm", Order = 11)]
	public bool Firm { get; set; }

	[XmlIgnore]
	public string CreatedBy { get; set; }

	[XmlIgnore]
	public DateTime? CreatedDate { get; set; }
}
