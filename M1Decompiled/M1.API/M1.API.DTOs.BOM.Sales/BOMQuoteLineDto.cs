using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace M1.API.DTOs.BOM.Sales;

[Serializable]
[DataContract(Namespace = "", Name = "quoteline")]
[XmlRoot(ElementName = "quoteline")]
[XmlType(AnonymousType = true)]
public class BOMQuoteLineDto
{
	[XmlElement(ElementName = "quoteID")]
	[DataMember(Name = "quoteID", Order = 1)]
	[Required(ErrorMessage = "QuoteID is invalid or empty.")]
	public string QuoteID { get; set; }

	[XmlElement(ElementName = "quoteLineID")]
	[DataMember(Name = "quoteLineID", Order = 2)]
	[Required(ErrorMessage = "QuoteLineID is invalid or empty.")]
	public short QuoteLineID { get; set; }

	[XmlElement(ElementName = "partID")]
	[DataMember(Name = "partID", Order = 3)]
	[Required(ErrorMessage = "PartID is invalid or empty.")]
	public string PartID { get; set; }

	[XmlElement(ElementName = "partRevisionID")]
	[DataMember(Name = "partRevisionID", Order = 4)]
	[Required(ErrorMessage = "PartRevisionID is invalid or empty.")]
	public string PartRevisionID { get; set; }

	[XmlElement(ElementName = "unitOfMeasure")]
	[DataMember(Name = "unitOfMeasure", Order = 5)]
	public string UnitOfMeasure { get; set; }

	[XmlElement(ElementName = "partGroupID")]
	[DataMember(Name = "partGroupID", Order = 6)]
	public string PartGroupID { get; set; }

	[XmlElement(ElementName = "partShortDescription")]
	[DataMember(Name = "partShortDescription", Order = 7)]
	[Required(ErrorMessage = "PartShortDescription is invalid or empty.")]
	public string PartShortDescription { get; set; }

	[XmlElement(ElementName = "orgPartShortDescription")]
	[DataMember(Name = "orgPartShortDescription", Order = 8)]
	public string OrgPartShortDescription { get; set; }

	[XmlElement(ElementName = "resolutionReasonID")]
	[DataMember(Name = "resolutionReasonID", Order = 9)]
	public string ResolutionReasonID { get; set; }

	[XmlElement(ElementName = "quoteMarkupType")]
	[DataMember(Name = "quoteMarkupType", Order = 10)]
	[Required(ErrorMessage = "QuoteMarkupType is invalid or empty.")]
	public byte QuoteMarkupType { get; set; }

	[XmlElement(ElementName = "purchaseToOrder")]
	[DataMember(Name = "purchaseToOrder", Order = 11)]
	public bool PurchaseToOrder { get; set; }

	[XmlElement(ElementName = "purchaseUnitCostForeign")]
	[DataMember(Name = "purchaseUnitCostForeign", Order = 12)]
	public decimal PurchaseUnitCostForeign { get; set; }

	[XmlElement(ElementName = "supplierOrganizationID")]
	[DataMember(Name = "supplierOrganizationID", Order = 13)]
	public string SupplierOrganizationID { get; set; }

	[XmlElement(ElementName = "purchaseLocationID")]
	[DataMember(Name = "purchaseLocationID", Order = 14)]
	public string PurchaseLocationID { get; set; }

	[XmlElement(ElementName = "firm")]
	[DataMember(Name = "firm", Order = 15)]
	public bool Firm { get; set; }

	[XmlElement(ElementName = "projectID")]
	[DataMember(Name = "projectID", Order = 16)]
	public string ProjectID { get; set; }

	[XmlElement(ElementName = "projectAreaID")]
	[DataMember(Name = "projectAreaID", Order = 17)]
	public string ProjectAreaID { get; set; }

	[XmlElement(ElementName = "closed")]
	[DataMember(Name = "closed", Order = 18)]
	public bool Closed { get; set; }

	[XmlElement(ElementName = "createdBy")]
	[DataMember(Name = "createdBy", Order = 19)]
	public string CreatedBy { get; set; }

	[XmlElement(ElementName = "createdDate")]
	[DataMember(Name = "createdDate", Order = 20)]
	public DateTime? CreatedDate { get; set; }

	[XmlElement(ElementName = "uniqueID")]
	[DataMember(Name = "uniqueID", Order = 21)]
	public Guid UniqueID { get; set; }

	[XmlElement(ElementName = "rowVersion")]
	[DataMember(Name = "rowVersion", Order = 22)]
	public byte[] RowVersion { get; set; }
}
