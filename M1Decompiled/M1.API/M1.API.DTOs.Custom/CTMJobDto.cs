using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace M1.API.DTOs.Custom;

[Serializable]
[DataContract(Namespace = "", Name = "job")]
[XmlRoot]
[XmlType(AnonymousType = true)]
public class CTMJobDto
{
	[XmlElement(ElementName = "jobID")]
	[DataMember(Name = "jobID", Order = 1)]
	[Required(ErrorMessage = "JobID is invalid or empty.")]
	public string JobID { get; set; }

	[XmlElement(ElementName = "jobAssemblyID")]
	[DataMember(Name = "jobAssemblyID", Order = 2)]
	[Required(ErrorMessage = "JobAssemblyID is invalid or empty.")]
	public int JobAssemblyID { get; set; }

	[XmlElement(ElementName = "jobAssemblyLevel")]
	[DataMember(Name = "jobAssemblyLevel", Order = 3)]
	[Required(ErrorMessage = "Job Assembly Level is invalid or empty.")]
	public short JobAssemblyLevel { get; set; }

	[XmlElement(ElementName = "parentAssemblyID")]
	[DataMember(Name = "parentAssemblyID", Order = 4)]
	[Required(ErrorMessage = "ParentAssemblyID is invalid or empty.")]
	public int ParentAssemblyID { get; set; }

	[XmlElement(ElementName = "customerOrganizationID")]
	[DataMember(Name = "customerOrganizationID", Order = 5)]
	public string CustomerOrganizationID { get; set; }

	[XmlElement(ElementName = "partID")]
	[DataMember(Name = "partID", Order = 6)]
	[Required(ErrorMessage = "PartID is invalid or empty.")]
	public string PartID { get; set; }

	[XmlElement(ElementName = "partRevisionID")]
	[DataMember(Name = "partRevisionID", Order = 7)]
	[Required(ErrorMessage = "PartRevisionID is invalid or empty.")]
	public string PartRevisionID { get; set; }

	[XmlElement(ElementName = "partWareHouseLocationID")]
	[DataMember(Name = "partWareHouseLocationID", Order = 8)]
	public string PartWareHouseLocationID { get; set; }

	[XmlElement(ElementName = "partBinID")]
	[DataMember(Name = "partBinID", Order = 9)]
	public string PartBinID { get; set; }

	[XmlElement(ElementName = "orderQuantity")]
	[DataMember(Name = "orderQuantity", Order = 10)]
	public decimal OrderQuantity { get; set; }

	[XmlElement(ElementName = "inventoryQuantity")]
	[DataMember(Name = "inventoryQuantity", Order = 11)]
	public decimal InventoryQuantity { get; set; }

	[XmlElement(ElementName = "scrapQuantity")]
	[DataMember(Name = "scrapQuantity", Order = 12)]
	public decimal ScrapQuantity { get; set; }

	[XmlElement(ElementName = "reworkQuantity")]
	[DataMember(Name = "reworkQuantity", Order = 13)]
	public decimal ReworkQuantity { get; set; }

	[XmlElement(ElementName = "productionDueDate")]
	[DataMember(Name = "productionDueDate", Order = 14)]
	public DateTime? ProductionDueDate { get; set; }

	[XmlElement(ElementName = "nestlinkProcessed")]
	[DataMember(Name = "nestlinkProcessed", Order = 30)]
	public bool NestlinkProcessed { get; set; }
}
