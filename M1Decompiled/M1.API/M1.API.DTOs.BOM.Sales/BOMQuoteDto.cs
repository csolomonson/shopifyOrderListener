using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace M1.API.DTOs.BOM.Sales;

[Serializable]
[DataContract(Namespace = "", Name = "quote")]
[XmlRoot(ElementName = "quote")]
[XmlType(AnonymousType = true)]
public class BOMQuoteDto
{
	[XmlElement(ElementName = "quoteID")]
	[DataMember(Name = "quoteID", Order = 1)]
	[Required(ErrorMessage = "QuoteID is invalid or empty.")]
	public string QuoteID { get; set; }

	[XmlElement(ElementName = "customerOrganizationID")]
	[DataMember(Name = "customerOrganizationID", Order = 2)]
	[Required(ErrorMessage = "CustomerOrganizationID is invalid or empty.")]
	public string CustomerOrganizationID { get; set; }

	[XmlElement(ElementName = "plantID")]
	[DataMember(Name = "plantID", Order = 3)]
	public string PlantID { get; set; }

	[XmlElement(ElementName = "quoterEmployeeID")]
	[DataMember(Name = "quoterEmployeeID", Order = 4)]
	[Required(ErrorMessage = "QuoterEmployeeID is invalid or empty.")]
	public string QuoterEmployeeID { get; set; }

	[XmlElement(ElementName = "quoteDate")]
	[DataMember(Name = "quoteDate", Order = 5)]
	public DateTime? QuoteDate { get; set; }

	[XmlElement(ElementName = "dueDate")]
	[DataMember(Name = "dueDate", Order = 6)]
	[Required(ErrorMessage = "DueDate is invalid or empty.")]
	public DateTime? DueDate { get; set; }

	[XmlElement(ElementName = "expirationDate")]
	[DataMember(Name = "expirationDate", Order = 7)]
	public DateTime? ExpirationDate { get; set; }

	[XmlElement(ElementName = "projectID")]
	[DataMember(Name = "projectID", Order = 8)]
	public string ProjectID { get; set; }

	[XmlElement(ElementName = "closed")]
	[DataMember(Name = "closed", Order = 9)]
	public bool Closed { get; set; }

	[XmlElement(ElementName = "closedDate")]
	[DataMember(Name = "closedDate", Order = 10)]
	public DateTime? ClosedDate { get; set; }

	[XmlElement(ElementName = "paymentTermID")]
	[DataMember(Name = "paymentTermID", Order = 11)]
	public string PaymentTermID { get; set; }

	[XmlElement(ElementName = "shippingMethodID")]
	[DataMember(Name = "shippingMethodID", Order = 12)]
	public string ShippingMethodID { get; set; }

	[XmlElement(ElementName = "createdBy")]
	[DataMember(Name = "createdBy", Order = 13)]
	public string CreatedBy { get; set; }

	[XmlElement(ElementName = "createdDate")]
	[DataMember(Name = "createdDate", Order = 14)]
	public DateTime? CreatedDate { get; set; }

	[XmlElement(ElementName = "uniqueID")]
	[DataMember(Name = "uniqueID", Order = 15)]
	public Guid UniqueID { get; set; }

	[XmlElement(ElementName = "rowVersion")]
	[DataMember(Name = "rowVersion", Order = 16)]
	public byte[] RowVersion { get; set; }
}
