using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace M1.API.DTOs.BOM.Job;

[Serializable]
[DataContract(Namespace = "", Name = "jobassembly")]
[XmlRoot(ElementName = "jobassembly")]
[XmlType(AnonymousType = true)]
public class BOMJobAssemblyDto
{
	[XmlElement(ElementName = "jobID")]
	[DataMember(Name = "jobID", Order = 1)]
	[Required(ErrorMessage = "JobID is invalid or empty.")]
	public string JobID { get; set; }

	[XmlElement(ElementName = "jobAssemblyID")]
	[DataMember(Name = "jobAssemblyID", Order = 2)]
	[Required(ErrorMessage = "JobAssemblyID is invalid or empty.")]
	public int JobAssemblyID { get; set; }

	[XmlElement(ElementName = "level")]
	[DataMember(Name = "level", Order = 3)]
	[Required(ErrorMessage = "Level is invalid or empty.")]
	public short Level { get; set; }

	[XmlElement(ElementName = "partWareHouseLocationID")]
	[DataMember(Name = "partWareHouseLocationID", Order = 4)]
	public string PartWareHouseLocationID { get; set; }

	[XmlElement(ElementName = "partBinID")]
	[DataMember(Name = "partBinID", Order = 5)]
	public string PartBinID { get; set; }

	[XmlElement(ElementName = "parentAssemblyID")]
	[DataMember(Name = "parentAssemblyID", Order = 6)]
	[Required(ErrorMessage = "ParentAssemblyID is invalid or empty.")]
	public int ParentAssemblyID { get; set; }

	[XmlElement(ElementName = "partID")]
	[DataMember(Name = "partID", Order = 7)]
	[Required(ErrorMessage = "PartID is invalid or empty.")]
	public string PartID { get; set; }

	[XmlElement(ElementName = "partRevisionID")]
	[DataMember(Name = "partRevisionID", Order = 8)]
	public string PartRevisionID { get; set; }

	[XmlElement(ElementName = "partShortDescription")]
	[DataMember(Name = "partShortDescription", Order = 9)]
	[Required(ErrorMessage = "PartShortDescription is invalid or empty.")]
	public string PartShortDescription { get; set; }

	[XmlElement(ElementName = "sourceMethodID")]
	[DataMember(Name = "sourceMethodID", Order = 10)]
	public string SourceMethodID { get; set; }

	[XmlElement(ElementName = "sourceRevisionID")]
	[DataMember(Name = "sourceRevisionID", Order = 11)]
	public string SourceRevisionID { get; set; }

	[XmlElement(ElementName = "quantityPerParent")]
	[DataMember(Name = "quantityPerParent", Order = 12)]
	[Required(ErrorMessage = "quantityPerParent is invalid or empty.")]
	public decimal QuantityPerParent { get; set; }

	[XmlElement(ElementName = "quantityToReturn")]
	[DataMember(Name = "quantityToReturn", Order = 13)]
	public decimal QuantityToReturn { get; set; }

	[XmlElement(ElementName = "unitOfMeasure")]
	[DataMember(Name = "unitOfMeasure", Order = 14)]
	public string UnitOfMeasure { get; set; }

	[XmlElement(ElementName = "orderQuantity")]
	[DataMember(Name = "orderQuantity", Order = 15)]
	public decimal OrderQuantity { get; set; }

	[XmlElement(ElementName = "productionQuantity")]
	[DataMember(Name = "productionQuantity", Order = 16)]
	public decimal ProductionQuantity { get; set; }

	[XmlElement(ElementName = "quantityToMake")]
	[DataMember(Name = "quantityToMake", Order = 17)]
	public decimal QuantityToMake { get; set; }

	[XmlElement(ElementName = "quantityToPull")]
	[DataMember(Name = "quantityToPull", Order = 18)]
	public decimal QuantityToPull { get; set; }

	[XmlElement(ElementName = "scrapQuantity")]
	[DataMember(Name = "scrapQuantity", Order = 19)]
	public decimal ScrapQuantity { get; set; }

	[XmlElement(ElementName = "estimatedUnitCost")]
	[DataMember(Name = "estimatedUnitCost", Order = 20)]
	public decimal EstimatedUnitCost { get; set; }

	[XmlElement(ElementName = "overlapOperationID")]
	[DataMember(Name = "overlapOperationID", Order = 21)]
	public int OverlapOperationID { get; set; }

	[XmlElement(ElementName = "overlapType")]
	[DataMember(Name = "overlapType", Order = 22)]
	public byte OverlapType { get; set; }

	[XmlElement(ElementName = "closed")]
	[DataMember(Name = "closed", Order = 23)]
	public bool Closed { get; set; }

	[XmlElement(ElementName = "scheduledDueDate")]
	[DataMember(Name = "scheduledDueDate", Order = 24)]
	public DateTime? DueDate { get; set; }
}
