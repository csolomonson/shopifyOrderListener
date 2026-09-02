using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace M1.API.DTOs.BOM.Inventory;

[Serializable]
[DataContract(Namespace = "", Name = "partbindetail")]
[XmlRoot(ElementName = "partbindetail")]
[XmlType(AnonymousType = true)]
public class BOMPartBinDetailDto
{
	[XmlElement(ElementName = "partID")]
	[DataMember(Name = "partID", Order = 1)]
	[Required(ErrorMessage = "PartID is invalid or empty.")]
	public string PartID { get; set; } = string.Empty;

	[XmlElement(ElementName = "partRevisionID")]
	[DataMember(Name = "partRevisionID", Order = 2)]
	public string PartRevisionID { get; set; } = string.Empty;

	[XmlElement(ElementName = "partBinID")]
	[DataMember(Name = "partBinID", Order = 3)]
	public string PartBinID { get; set; } = string.Empty;

	[XmlElement(ElementName = "partBinDetailID")]
	[DataMember(Name = "partBinDetailID", Order = 4)]
	public int PartBinDetailID { get; set; }

	[XmlElement(ElementName = "warehouseID")]
	[DataMember(Name = "warehouseID", Order = 21)]
	public string WarehouseID { get; set; }

	[XmlElement(ElementName = "transactionDate")]
	[DataMember(Name = "transactionDate", Order = 5)]
	public DateTime? TransactionDate { get; set; }

	[XmlElement(ElementName = "quantityType")]
	[DataMember(Name = "quantityType", Order = 6)]
	public short? QuantityType { get; set; }

	[XmlElement(ElementName = "originalQuantity")]
	[DataMember(Name = "originalQuantity", Order = 7)]
	public decimal? OriginalQuantity { get; set; }

	[XmlElement(ElementName = "remainingQuantity")]
	[DataMember(Name = "remainingQuantity", Order = 8)]
	public decimal? RemainingQuantity { get; set; }

	[XmlElement(ElementName = "unitCost")]
	[DataMember(Name = "unitCost", Order = 9)]
	public decimal? UnitCost { get; set; }

	[XmlElement(ElementName = "sourceTableName")]
	[DataMember(Name = "sourceTableName", Order = 10)]
	public string SourceTableName { get; set; }

	[XmlElement(ElementName = "createdBy")]
	[DataMember(Name = "createdBy", Order = 11)]
	public string CreatedBy { get; set; } = string.Empty;

	[XmlElement(ElementName = "createdDate")]
	[DataMember(Name = "createdDate", Order = 12)]
	public DateTime? CreatedDate { get; set; }

	[XmlElement(ElementName = "uniqueID")]
	[DataMember(Name = "uniqueID", Order = 13)]
	public Guid UniqueID { get; set; }

	[XmlElement(ElementName = "rowVersion")]
	[DataMember(Name = "rowVersion", Order = 14)]
	public byte[] RowVersion { get; set; }
}
