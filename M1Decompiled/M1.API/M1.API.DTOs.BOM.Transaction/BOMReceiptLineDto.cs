using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace M1.API.DTOs.BOM.Transaction;

[Serializable]
[DataContract(Namespace = "", Name = "receiptline")]
[XmlRoot(ElementName = "receiptline")]
[XmlType(AnonymousType = true)]
public class BOMReceiptLineDto
{
	[XmlElement(ElementName = "receiptLineID")]
	[DataMember(Name = "receiptLineID", Order = 1)]
	public short ReceiptLineID { get; set; }

	[XmlElement(ElementName = "receiptID")]
	[DataMember(Name = "receiptID", Order = 2)]
	public string ReceiptID { get; set; }

	[XmlElement(ElementName = "purchaseOrderID")]
	[DataMember(Name = "purchaseOrderID", Order = 3)]
	public string PurchaseOrderID { get; set; }

	[XmlElement(ElementName = "purchaseOrderLineID")]
	[DataMember(Name = "purchaseOrderLineID", Order = 4)]
	public short PurchaseOrderLineID { get; set; }

	[XmlElement(ElementName = "jobID")]
	[DataMember(Name = "jobID", Order = 5)]
	public string JobID { get; set; }

	[XmlElement(ElementName = "jobAssemblyID")]
	[DataMember(Name = "jobAssemblyID", Order = 6)]
	public int JobAssemblyID { get; set; }

	[XmlElement(ElementName = "jobType")]
	[DataMember(Name = "jobType", Order = 7)]
	public byte JobType { get; set; }

	[XmlElement(ElementName = "jobMaterialID")]
	[DataMember(Name = "jobMaterialID", Order = 8)]
	public int JobMaterialID { get; set; }

	[XmlElement(ElementName = "jobOperationID")]
	[DataMember(Name = "jobOperationID", Order = 9)]
	public int JobOperationID { get; set; }

	[XmlElement(ElementName = "partID")]
	[DataMember(Name = "partID", Order = 10)]
	public string PartID { get; set; }

	[XmlElement(ElementName = "partRevisionID")]
	[DataMember(Name = "partRevisionID", Order = 11)]
	public string PartRevisionID { get; set; }

	[XmlElement(ElementName = "orgPartID")]
	[DataMember(Name = "orgPartID", Order = 12)]
	public string OrgPartID { get; set; }

	[XmlElement(ElementName = "orgPartShortDescription")]
	[DataMember(Name = "orgPartShortDescription", Order = 13)]
	public string OrgPartShortDescription { get; set; }

	[XmlElement(ElementName = "description")]
	[DataMember(Name = "description", Order = 14)]
	public string Description { get; set; }

	[XmlElement(ElementName = "partWarehouseLocationID")]
	[DataMember(Name = "partWarehouseLocationID", Order = 15)]
	public string PartWarehouseLocationID { get; set; }

	[XmlElement(ElementName = "partBinID")]
	[DataMember(Name = "partBinID", Order = 16)]
	public string PartBinID { get; set; }

	[XmlElement(ElementName = "purchaseQuantityReceived")]
	[DataMember(Name = "purchaseQuantityReceived", Order = 17)]
	public decimal PurchaseQuantityReceived { get; set; }

	[XmlElement(ElementName = "purchaseUnitOfMeasure")]
	[DataMember(Name = "purchaseUnitOfMeasure", Order = 18)]
	public string PurchaseUnitOfMeasure { get; set; }

	[XmlElement(ElementName = "purchaseUnitCost")]
	[DataMember(Name = "purchaseUnitCost", Order = 19)]
	public decimal PurchaseUnitCost { get; set; }

	[XmlElement(ElementName = "setupCharge")]
	[DataMember(Name = "setupCharge", Order = 20)]
	public decimal SetupCharge { get; set; }

	[XmlElement(ElementName = "conversionFactor")]
	[DataMember(Name = "conversionFactor", Order = 21)]
	public decimal ConversionFactor { get; set; }

	[XmlElement(ElementName = "inventoryUnitOfMeasure")]
	[DataMember(Name = "inventoryUnitOfMeasure", Order = 22)]
	public string InventoryUnitOfMeasure { get; set; }

	[XmlElement(ElementName = "inventoryUnitCost")]
	[DataMember(Name = "inventoryUnitCost", Order = 23)]
	public decimal InventoryUnitCost { get; set; }

	[XmlElement(ElementName = "poReceivedComplete")]
	[DataMember(Name = "poReceivedComplete", Order = 24)]
	public bool PoReceivedComplete { get; set; }

	[XmlElement(ElementName = "jobReceivedComplete")]
	[DataMember(Name = "jobReceivedComplete", Order = 25)]
	public bool JobReceivedComplete { get; set; }

	[XmlElement(ElementName = "requiresInspection")]
	[DataMember(Name = "requiresInspection", Order = 26)]
	public bool RequiresInspection { get; set; }

	[XmlElement(ElementName = "reference")]
	[DataMember(Name = "reference", Order = 27)]
	public string Reference { get; set; }

	[XmlElement(ElementName = "heatLot")]
	[DataMember(Name = "heatLot", Order = 28)]
	public string HeatLot { get; set; }

	[XmlElement(ElementName = "projectID")]
	[DataMember(Name = "projectID", Order = 29)]
	public string ProjectID { get; set; }

	[XmlElement(ElementName = "projectAreaID")]
	[DataMember(Name = "projectAreaID", Order = 30)]
	public string ProjectAreaID { get; set; }

	[XmlElement(ElementName = "closed")]
	[DataMember(Name = "closed", Order = 31)]
	public bool Closed { get; set; }

	[XmlElement(ElementName = "postedToGl")]
	[DataMember(Name = "postedToGl", Order = 32)]
	public bool PostedToGl { get; set; }

	[XmlElement(ElementName = "reversed")]
	[DataMember(Name = "reversed", Order = 33)]
	public bool Reversed { get; set; }

	[XmlElement(ElementName = "reverseReceiptID")]
	[DataMember(Name = "reverseReceiptID", Order = 34)]
	public string ReverseReceiptID { get; set; }

	[XmlElement(ElementName = "reverseReceiptLineID")]
	[DataMember(Name = "reverseReceiptLineID", Order = 35)]
	public short ReverseReceiptLineID { get; set; }

	[XmlElement(ElementName = "jobOprQuantityReceived")]
	[DataMember(Name = "jobOprQuantityReceived", Order = 36)]
	public decimal JobOprQuantityReceived { get; set; }

	[XmlElement(ElementName = "jobMatQuantityReceived")]
	[DataMember(Name = "jobMatQuantityReceived", Order = 37)]
	public decimal JobMatQuantityReceived { get; set; }

	[XmlElement(ElementName = "uniqueID")]
	[DataMember(Name = "uniqueID", Order = 38)]
	public Guid UniqueID { get; set; }

	[XmlElement(ElementName = "createdBy")]
	[DataMember(Name = "createdBy", Order = 39)]
	public string CreatedBy { get; set; }

	[XmlElement(ElementName = "createdDate")]
	[DataMember(Name = "createdDate", Order = 40)]
	public DateTime? CreatedDate { get; set; }

	[XmlElement(ElementName = "rowVersion")]
	[DataMember(Name = "rowVersion", Order = 41)]
	public byte[] RowVersion { get; set; }
}
