using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace M1.API.DTOs.BOM;

[Serializable]
[DataContract(Namespace = "", Name = "quoteoperation")]
[XmlRoot(ElementName = "quoteoperation")]
[XmlType(AnonymousType = true)]
public class BOMQuoteOperationDto
{
	[XmlElement(ElementName = "quoteID")]
	[DataMember(Name = "quoteID", Order = 1)]
	[Required(ErrorMessage = "QuoteID is invalid or empty.")]
	public string QuoteID { get; set; }

	[XmlElement(ElementName = "quoteLineID")]
	[DataMember(Name = "quoteLineID", Order = 2)]
	[Required(ErrorMessage = "QuoteLineID is invalid or empty.")]
	public short QuoteLineID { get; set; }

	[XmlElement(ElementName = "quoteAssemblyID")]
	[DataMember(Name = "quoteAssemblyID", Order = 3)]
	[Required(ErrorMessage = "QuoteAssemblyID is invalid or empty.")]
	public int QuoteAssemblyID { get; set; }

	[XmlElement(ElementName = "quoteOperationID")]
	[DataMember(Name = "quoteOperationID", Order = 4)]
	[Required(ErrorMessage = "QuoteOperationID is invalid or empty.")]
	public int QuoteOperationID { get; set; }

	[XmlElement(ElementName = "operationType")]
	[DataMember(Name = "operationType", Order = 5)]
	[Required(ErrorMessage = "OperationType is invalid or empty.")]
	public byte OperationType { get; set; }

	[XmlElement(ElementName = "workCenterID")]
	[DataMember(Name = "workCenterID", Order = 6)]
	[Required(ErrorMessage = "WorkCenterID is invalid or empty.")]
	public string WorkCenterID { get; set; }

	[XmlElement(ElementName = "processID")]
	[DataMember(Name = "processID", Order = 7)]
	[Required(ErrorMessage = "ProcessID is invalid or empty.")]
	public string ProcessID { get; set; }

	[XmlElement(ElementName = "processShortDescription")]
	[DataMember(Name = "processShortDescription", Order = 8)]
	[Required(ErrorMessage = "ProcessShortDescription is invalid or empty.")]
	public string ProcessShortDescription { get; set; }

	[XmlElement(ElementName = "processLongDescriptionRtf")]
	[DataMember(Name = "processLongDescriptionRtf", Order = 9)]
	public string ProcessLongDescriptionRtf { get; set; }

	[XmlElement(ElementName = "processLongDescriptionText")]
	[DataMember(Name = "processLongDescriptionText", Order = 10)]
	public string ProcessLongDescriptionText { get; set; }

	[XmlElement(ElementName = "quantityPerAssembly")]
	[DataMember(Name = "quantityPerAssembly", Order = 11)]
	[Required(ErrorMessage = "QuantityPerAssembly is invalid or empty.")]
	public decimal QuantityPerAssembly { get; set; }

	[XmlElement(ElementName = "queueTime")]
	[DataMember(Name = "queueTime", Order = 12)]
	public decimal QueueTime { get; set; }

	[XmlElement(ElementName = "setupHours")]
	[DataMember(Name = "setupHours", Order = 13)]
	public decimal SetupHours { get; set; }

	[XmlElement(ElementName = "moveTime")]
	[DataMember(Name = "moveTime", Order = 14)]
	public decimal MoveTime { get; set; }

	[XmlElement(ElementName = "quotingRate")]
	[DataMember(Name = "quotingRate", Order = 15)]
	public decimal QuotingRate { get; set; }

	[XmlElement(ElementName = "setupRate")]
	[DataMember(Name = "setupRate", Order = 16)]
	public decimal SetupRate { get; set; }

	[XmlElement(ElementName = "productionRate")]
	[DataMember(Name = "productionRate", Order = 17)]
	public decimal ProductionRate { get; set; }

	[XmlElement(ElementName = "overheadRate")]
	[DataMember(Name = "overheadRate", Order = 18)]
	public decimal OverheadRate { get; set; }

	[XmlElement(ElementName = "partID")]
	[DataMember(Name = "partID", Order = 19)]
	public string PartID { get; set; }

	[XmlElement(ElementName = "partRevisionID")]
	[DataMember(Name = "partRevisionID", Order = 20)]
	public string PartRevisionID { get; set; }

	[XmlElement(ElementName = "unitOfMeasure")]
	[DataMember(Name = "unitOfMeasure", Order = 21)]
	public string UnitOfMeasure { get; set; }

	[XmlElement(ElementName = "supplierOrganizationID")]
	[DataMember(Name = "supplierOrganizationID", Order = 22)]
	public string SupplierOrganizationID { get; set; }

	[XmlElement(ElementName = "standardFactor")]
	[DataMember(Name = "standardFactor", Order = 23)]
	[Required(ErrorMessage = "StandardFactor is invalid or empty.")]
	public string StandardFactor { get; set; }

	[XmlElement(ElementName = "productionStandard")]
	[DataMember(Name = "productionStandard", Order = 24)]
	public decimal ProductionStandard { get; set; }

	[XmlElement(ElementName = "closed")]
	[DataMember(Name = "closed", Order = 25)]
	public bool Closed { get; set; }

	[XmlElement(ElementName = "createdBy")]
	[DataMember(Name = "createdBy", Order = 26)]
	public string CreatedBy { get; set; }

	[XmlElement(ElementName = "createdDate")]
	[DataMember(Name = "createdDate", Order = 27)]
	public DateTime? CreatedDate { get; set; }

	[XmlElement(ElementName = "uniqueID")]
	[DataMember(Name = "uniqueID", Order = 28)]
	public Guid UniqueID { get; set; }

	[XmlElement(ElementName = "rowVersion")]
	[DataMember(Name = "rowVersion", Order = 29)]
	public byte[] RowVersion { get; set; }
}
