using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace M1.API.DTOs.BOM.Job;

[Serializable]
[DataContract(Namespace = "", Name = "jobmaterial")]
[XmlRoot(ElementName = "jobmaterial")]
[XmlType(AnonymousType = true)]
public class BOMJobMaterialDto
{
	[XmlElement(ElementName = "jobID")]
	[DataMember(Name = "jobID", Order = 1)]
	[Required(ErrorMessage = "JobID is invalid or empty.")]
	public string JobID { get; set; }

	[XmlElement(ElementName = "jobAssemblyID")]
	[DataMember(Name = "jobAssemblyID", Order = 2)]
	[Required(ErrorMessage = "JobAssemblyID is invalid or empty.")]
	public int JobAssemblyID { get; set; }

	[XmlElement(ElementName = "jobMaterialID")]
	[DataMember(Name = "jobMaterialID", Order = 3)]
	[Required(ErrorMessage = "JobMaterialID is invalid or empty.")]
	public int JobMaterialID { get; set; }

	[XmlElement(ElementName = "partID")]
	[DataMember(Name = "partID", Order = 4)]
	[Required(ErrorMessage = "PartID is invalid or empty.")]
	public string PartID { get; set; }

	[XmlElement(ElementName = "partRevisionID")]
	[DataMember(Name = "partRevisionID", Order = 5)]
	public string PartRevisionID { get; set; }

	[XmlElement(ElementName = "partWarehouseLocationID")]
	[DataMember(Name = "partWarehouseLocationID", Order = 6)]
	public string PartWarehouseLocationID { get; set; }

	[XmlElement(ElementName = "partBinID")]
	[DataMember(Name = "partBinID", Order = 7)]
	public string PartBinID { get; set; }

	[XmlElement(ElementName = "unitOfMeasure")]
	[DataMember(Name = "unitOfMeasure", Order = 8)]
	public string UnitOfMeasure { get; set; }

	[XmlElement(ElementName = "partShortDescription")]
	[DataMember(Name = "partShortDescription", Order = 9)]
	[Required(ErrorMessage = "PartShortDescription is invalid or empty.")]
	public string PartShortDescription { get; set; }

	[XmlElement(ElementName = "quantityPerAssembly")]
	[DataMember(Name = "quantityPerAssembly", Order = 10)]
	[Required(ErrorMessage = "QuantityPerAssembly is invalid or empty.")]
	public decimal QuantityPerAssembly { get; set; }

	[XmlElement(ElementName = "scrapPercent")]
	[DataMember(Name = "scrapPercent", Order = 11)]
	public decimal ScrapPercent { get; set; }

	[XmlElement(ElementName = "scrapQuantity")]
	[DataMember(Name = "scrapQuantity", Order = 12)]
	public decimal ScrapQuantity { get; set; }

	[XmlElement(ElementName = "estimatedQuantity")]
	[DataMember(Name = "estimatedQuantity", Order = 13)]
	public decimal EstimatedQuantity { get; set; }

	[XmlElement(ElementName = "estimatedUnitCost")]
	[DataMember(Name = "estimatedUnitCost", Order = 14)]
	public decimal EstimatedUnitCost { get; set; }

	[XmlElement(ElementName = "calculatedUnitCost")]
	[DataMember(Name = "calculatedUnitCost", Order = 15)]
	public decimal CalculatedUnitCost { get; set; }

	[XmlElement(ElementName = "firm")]
	[DataMember(Name = "firm", Order = 16)]
	public bool Firm { get; set; }

	[XmlElement(ElementName = "supplierOrganizationID")]
	[DataMember(Name = "supplierOrganizationID", Order = 17)]
	public string SupplierOrganizationID { get; set; }

	[XmlElement(ElementName = "purchaseLocationID")]
	[DataMember(Name = "purchaseLocationID", Order = 18)]
	public string PurchaseLocationID { get; set; }

	[XmlElement(ElementName = "purchaseOrderID")]
	[DataMember(Name = "purchaseOrderID", Order = 19)]
	public string PurchaseOrderID { get; set; }

	[XmlElement(ElementName = "leadTime")]
	[DataMember(Name = "leadTime", Order = 20)]
	public short LeadTime { get; set; }

	[XmlElement(ElementName = "minimumCharge")]
	[DataMember(Name = "minimumCharge", Order = 21)]
	public decimal MinimumCharge { get; set; }

	[XmlElement(ElementName = "dueInDate")]
	[DataMember(Name = "dueInDate", Order = 22)]
	public DateTime? DueInDate { get; set; }

	[XmlElement(ElementName = "requiredDate")]
	[DataMember(Name = "requiredDate", Order = 23)]
	public DateTime? RequiredDate { get; set; }

	[XmlElement(ElementName = "quantityAllocated")]
	[DataMember(Name = "quantityAllocated", Order = 24)]
	public decimal QuantityAllocated { get; set; }

	[XmlElement(ElementName = "quantityReceived")]
	[DataMember(Name = "quantityReceived", Order = 25)]
	public decimal QuantityReceived { get; set; }

	[XmlElement(ElementName = "scrapQuantityReceived")]
	[DataMember(Name = "scrapQuantityReceived", Order = 26)]
	public decimal ScrapQuantityReceived { get; set; }

	[XmlElement(ElementName = "quantityToInspect")]
	[DataMember(Name = "quantityToInspect", Order = 27)]
	public decimal QuantityToInspect { get; set; }

	[XmlElement(ElementName = "quantityToReturn")]
	[DataMember(Name = "quantityToReturn", Order = 28)]
	public decimal QuantityToReturn { get; set; }

	[XmlElement(ElementName = "receivedComplete")]
	[DataMember(Name = "receivedComplete", Order = 29)]
	public bool ReceivedComplete { get; set; }

	[XmlElement(ElementName = "purchaseToJobQuantity")]
	[DataMember(Name = "purchaseToJobQuantity", Order = 30)]
	public decimal PurchaseToJobQuantity { get; set; }

	[XmlElement(ElementName = "pullAllFromStock")]
	[DataMember(Name = "pullAllFromStock", Order = 31)]
	public bool PullAllFromStock { get; set; }

	[XmlElement(ElementName = "pullFromStockQuantity")]
	[DataMember(Name = "pullFromStockQuantity", Order = 32)]
	public decimal PullFromStockQuantity { get; set; }

	[XmlElement(ElementName = "closed")]
	[DataMember(Name = "closed", Order = 33)]
	public bool Closed { get; set; }

	[XmlElement(ElementName = "uniqueID")]
	[DataMember(Name = "uniqueID", Order = 34)]
	public Guid UniqueID { get; set; }

	[XmlElement(ElementName = "rowVersion")]
	[DataMember(Name = "rowVersion", Order = 35)]
	public byte[] RowVersion { get; set; }

	[XmlElement(ElementName = "relatedJobOperationID")]
	[DataMember(Name = "relatedJobOperationID", Order = 36)]
	[Required(ErrorMessage = "RelatedJobOperationID is invalid or empty.")]
	public int RelatedJobOperationID { get; set; }
}
