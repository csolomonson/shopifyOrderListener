using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace M1.API.DTOs.BOM.Transaction;

[Serializable]
[DataContract(Namespace = "", Name = "mfgreceipt")]
[XmlRoot(ElementName = "mfgreceipt")]
[XmlType(AnonymousType = true)]
public class BOMMfgReceiptDto
{
	[XmlElement(ElementName = "mfgReceiptID")]
	[DataMember(Name = "mfgReceiptID", Order = 1)]
	public string MfgReceiptID { get; set; }

	[XmlElement(ElementName = "receiptType")]
	[DataMember(Name = "receiptType", Order = 2)]
	[Required(ErrorMessage = "ReceiptType is invalid or empty.")]
	public byte ReceiptType { get; set; }

	[XmlElement(ElementName = "receiptDate")]
	[DataMember(Name = "receiptDate", Order = 3)]
	[Required(ErrorMessage = "ReceiptDate is invalid or empty.")]
	public DateTime? ReceiptDate { get; set; }

	[XmlElement(ElementName = "partID")]
	[DataMember(Name = "partID", Order = 4)]
	[Required(ErrorMessage = "PartID is invalid or empty.")]
	public string PartID { get; set; }

	[XmlElement(ElementName = "partRevisionID")]
	[DataMember(Name = "partRevisionID", Order = 5)]
	[Required(ErrorMessage = "PartRevisionID is invalid or empty.")]
	public string PartRevisionID { get; set; }

	[XmlElement(ElementName = "partWarehouseLocationID")]
	[DataMember(Name = "partWarehouseLocationID", Order = 6)]
	[Required(ErrorMessage = "PartWarehouseLocationID is invalid or empty.")]
	public string PartWarehouseLocationID { get; set; }

	[XmlElement(ElementName = "partBinID")]
	[DataMember(Name = "partBinID", Order = 7)]
	[Required(ErrorMessage = "PartBinID is invalid or empty.")]
	public string PartBinID { get; set; }

	[XmlElement(ElementName = "posted")]
	[DataMember(Name = "posted", Order = 8)]
	public bool Posted { get; set; }

	[XmlElement(ElementName = "postedDate")]
	[DataMember(Name = "postedDate", Order = 9)]
	public DateTime? PostedDate { get; set; }

	[XmlElement(ElementName = "createdBy")]
	[DataMember(Name = "createdBy", Order = 10)]
	public string CreatedBy { get; set; }

	[XmlElement(ElementName = "createdDate")]
	[DataMember(Name = "createdDate", Order = 11)]
	public DateTime? CreatedDate { get; set; }

	[XmlElement(ElementName = "uniqueID")]
	[DataMember(Name = "uniqueID", Order = 12)]
	public Guid UniqueID { get; set; }

	[XmlElement(ElementName = "rowVersion")]
	[DataMember(Name = "rowVersion", Order = 13)]
	public byte[] RowVersion { get; set; }

	[XmlElement(ElementName = "projectID")]
	[DataMember(Name = "projectID", Order = 14)]
	public string ProjectID { get; set; }

	[XmlElement(ElementName = "projectAreaID")]
	[DataMember(Name = "projectAreaID", Order = 15)]
	public string ProjectAreaID { get; set; }

	[XmlElement(ElementName = "miscInvQuantityReceived")]
	[DataMember(Name = "miscInvQuantityReceived", Order = 16)]
	public decimal MiscInvQuantityReceived { get; set; }

	[XmlElement(ElementName = "inventoryQuantityReceived")]
	[DataMember(Name = "inventoryQuantityReceived", Order = 17)]
	public decimal InventoryQuantityReceived { get; set; }

	[XmlElement(ElementName = "jobOprQuantityReceived")]
	[DataMember(Name = "jobOprQuantityReceived", Order = 18)]
	public decimal JobOprQuantityReceived { get; set; }

	[XmlElement(ElementName = "jobAsmQuantityReceived")]
	[DataMember(Name = "jobAsmQuantityReceived", Order = 19)]
	public decimal JobAsmQuantityReceived { get; set; }

	[XmlElement(ElementName = "jobMatQuantityReceived")]
	[DataMember(Name = "jobMatQuantityReceived", Order = 20)]
	public decimal JobMatQuantityReceived { get; set; }

	[XmlElement(ElementName = "reference")]
	[DataMember(Name = "reference", Order = 21)]
	public string Reference { get; set; }

	[XmlElement(ElementName = "heatLot")]
	[DataMember(Name = "heatLot", Order = 22)]
	public string HeatLot { get; set; }
}
