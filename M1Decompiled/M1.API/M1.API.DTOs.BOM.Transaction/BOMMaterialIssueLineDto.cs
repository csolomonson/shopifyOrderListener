using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace M1.API.DTOs.BOM.Transaction;

[Serializable]
[DataContract(Namespace = "", Name = "materialissueline")]
[XmlRoot(ElementName = "materialissueline")]
[XmlType(AnonymousType = true)]
public class BOMMaterialIssueLineDto
{
	[XmlElement(ElementName = "materialIssueID")]
	[DataMember(Name = "materialIssueID", Order = 1)]
	[Required(ErrorMessage = "MaterialIssueID is invalid or empty.")]
	public string MaterialIssueID { get; set; }

	[XmlElement(ElementName = "materialIssueLineID")]
	[DataMember(Name = "materialIssueLineID", Order = 2)]
	[Required(ErrorMessage = "MaterialIssueLineID is invalid or empty.")]
	public short MaterialIssueLineID { get; set; }

	[XmlElement(ElementName = "issueType")]
	[DataMember(Name = "issueType", Order = 3)]
	[Required(ErrorMessage = "IssueType is invalid or empty.")]
	public byte IssueType { get; set; }

	[XmlElement(ElementName = "jobID")]
	[DataMember(Name = "jobID", Order = 4)]
	public string JobID { get; set; }

	[XmlElement(ElementName = "jobAssemblyID")]
	[DataMember(Name = "jobAssemblyID", Order = 5)]
	public int JobAssemblyID { get; set; }

	[XmlElement(ElementName = "createJobSeq")]
	[DataMember(Name = "createJobSeq", Order = 6)]
	public bool CreateJobSeq { get; set; }

	[XmlElement(ElementName = "jobMaterialID")]
	[DataMember(Name = "jobMaterialID", Order = 7)]
	public int JobMaterialID { get; set; }

	[XmlElement(ElementName = "jobType")]
	[DataMember(Name = "jobType", Order = 8)]
	public byte JobType { get; set; }

	[XmlElement(ElementName = "estimatedQuantity")]
	[DataMember(Name = "estimatedQuantity", Order = 9)]
	public decimal EstimatedQuantity { get; set; }

	[XmlElement(ElementName = "jobOpenQuantity")]
	[DataMember(Name = "jobOpenQuantity", Order = 10)]
	public decimal JobOpenQuantity { get; set; }

	[XmlElement(ElementName = "issueComplete")]
	[DataMember(Name = "issueComplete", Order = 11)]
	public bool IssueComplete { get; set; }

	[XmlElement(ElementName = "partID")]
	[DataMember(Name = "partID", Order = 12)]
	[Required(ErrorMessage = "PartID is invalid or empty.")]
	public string PartID { get; set; }

	[XmlElement(ElementName = "partRevisionID")]
	[DataMember(Name = "partRevisionID", Order = 13)]
	[Required(ErrorMessage = "PartRevisionID is invalid or empty.")]
	public string PartRevisionID { get; set; }

	[XmlElement(ElementName = "partWarehouseLocationID")]
	[DataMember(Name = "partWarehouseLocationID", Order = 14)]
	[Required(ErrorMessage = "PartWarehouseLocationID is invalid or empty.")]
	public string PartWarehouseLocationID { get; set; }

	[XmlElement(ElementName = "partBinID")]
	[DataMember(Name = "partBinID", Order = 15)]
	[Required(ErrorMessage = "PartBinID is invalid or empty.")]
	public string PartBinID { get; set; }

	[XmlElement(ElementName = "invIssueQuantity")]
	[DataMember(Name = "invIssueQuantity", Order = 16)]
	public decimal InvIssueQuantity { get; set; }

	[XmlElement(ElementName = "invScrapQuantity")]
	[DataMember(Name = "invScrapQuantity", Order = 17)]
	public decimal InvScrapQuantity { get; set; }

	[XmlElement(ElementName = "reference")]
	[DataMember(Name = "reference", Order = 18)]
	public string Reference { get; set; }

	[XmlElement(ElementName = "heatLot")]
	[DataMember(Name = "heatLot", Order = 19)]
	public string HeatLot { get; set; }

	[XmlElement(ElementName = "miscIssueReasonID")]
	[DataMember(Name = "miscIssueReasonID", Order = 20)]
	public string MiscIssueReasonID { get; set; }

	[XmlElement(ElementName = "projectID")]
	[DataMember(Name = "projectID", Order = 21)]
	public string ProjectID { get; set; }

	[XmlElement(ElementName = "projectAreaID")]
	[DataMember(Name = "projectAreaID", Order = 22)]
	public string ProjectAreaID { get; set; }

	[XmlElement(ElementName = "jobMatScrapQuantity")]
	[DataMember(Name = "jobMatScrapQuantity", Order = 23)]
	public decimal JobMatScrapQuantity { get; set; }

	[XmlElement(ElementName = "jobAsmScrapQuantity")]
	[DataMember(Name = "jobAsmScrapQuantity", Order = 24)]
	public decimal JobAsmScrapQuantity { get; set; }

	[XmlElement(ElementName = "jobAsmIssueQuantity")]
	[DataMember(Name = "jobAsmIssueQuantity", Order = 25)]
	public decimal JobAsmIssueQuantity { get; set; }

	[XmlElement(ElementName = "jobMatIssueQuantity")]
	[DataMember(Name = "jobMatIssueQuantity", Order = 26)]
	public decimal JobMatIssueQuantity { get; set; }

	[XmlElement(ElementName = "jobMatReturnScrapQuantity")]
	[DataMember(Name = "jobMatReturnScrapQuantity", Order = 27)]
	public decimal JobMatReturnScrapQuantity { get; set; }

	[XmlElement(ElementName = "jobMatReturnIssueQuantity")]
	[DataMember(Name = "jobMatReturnIssueQuantity", Order = 28)]
	public decimal JobMatReturnIssueQuantity { get; set; }

	[XmlElement(ElementName = "reverseMaterialIssueID")]
	[DataMember(Name = "reverseMaterialIssueID", Order = 29)]
	public string ReverseMaterialIssueID { get; set; }

	[XmlElement(ElementName = "reverseMaterialIssueLineID")]
	[DataMember(Name = "reverseMaterialIssueLineID", Order = 30)]
	public short ReverseMaterialIssueLineID { get; set; }

	[XmlElement(ElementName = "reversed")]
	[DataMember(Name = "reversed", Order = 31)]
	public bool Reversed { get; set; }

	[XmlElement(ElementName = "kitPart")]
	[DataMember(Name = "kitPart", Order = 32)]
	public bool KitPart { get; set; }

	[XmlElement(ElementName = "posted")]
	[DataMember(Name = "posted", Order = 33)]
	public bool Posted { get; set; }

	[XmlElement(ElementName = "longDescriptionText")]
	[DataMember(Name = "longDescriptionText", Order = 34)]
	public string LongDescriptionText { get; set; }

	[XmlElement(ElementName = "plantID")]
	[DataMember(Name = "plantID", Order = 35)]
	public string PlantID { get; set; }

	[XmlElement(ElementName = "quantityAllocated")]
	[DataMember(Name = "quantityAllocated", Order = 36)]
	public decimal QuantityAllocated { get; set; }

	[XmlElement(ElementName = "quantityOnHand")]
	[DataMember(Name = "quantityOnHand", Order = 37)]
	public decimal QuantityOnHand { get; set; }

	[XmlElement(ElementName = "uniqueID")]
	[DataMember(Name = "uniqueID", Order = 9)]
	public Guid UniqueID { get; set; }

	[XmlElement(ElementName = "createdBy")]
	[DataMember(Name = "createdBy", Order = 38)]
	public string CreatedBy { get; set; }

	[XmlElement(ElementName = "createdDate")]
	[DataMember(Name = "createdDate", Order = 39)]
	public DateTime? CreatedDate { get; set; }

	[XmlElement(ElementName = "rowVersion")]
	[DataMember(Name = "rowVersion", Order = 34)]
	public byte[] RowVersion { get; set; }
}
