using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace M1.API.DTOs.BOM;

[Serializable]
[DataContract(Namespace = "", Name = "quotequantity")]
[XmlRoot(ElementName = "quotequantity")]
[XmlType(AnonymousType = true)]
public class BOMQuoteQuantityDto
{
	[XmlElement(ElementName = "quoteID")]
	[DataMember(Name = "quoteID", Order = 1)]
	[Required(ErrorMessage = "QuoteID is invalid or empty.")]
	public string QuoteID { get; set; }

	[XmlElement(ElementName = "quoteLineID")]
	[DataMember(Name = "quoteLineID", Order = 2)]
	[Required(ErrorMessage = "QuoteLineID is invalid or empty.")]
	public short QuoteLineID { get; set; }

	[XmlElement(ElementName = "quoteQuantityID")]
	[DataMember(Name = "quoteQuantityID", Order = 3)]
	[Required(ErrorMessage = "QuoteQuantityID is invalid or empty.")]
	public byte QuoteQuantityID { get; set; }

	[XmlElement(ElementName = "quoteQuantity")]
	[DataMember(Name = "quoteQuantity", Order = 4)]
	[Required(ErrorMessage = "QuoteQuantity is invalid or empty.")]
	public decimal QuoteQuantity { get; set; }

	[XmlElement(ElementName = "scrapPercent")]
	[DataMember(Name = "scrapPercent", Order = 5)]
	public decimal ScrapPercent { get; set; }

	[XmlElement(ElementName = "totalRunQuantity")]
	[DataMember(Name = "totalRunQuantity", Order = 6)]
	public decimal TotalRunQuantity { get; set; }

	[XmlElement(ElementName = "quoteMarkupType")]
	[DataMember(Name = "quoteMarkupType", Order = 7)]
	[Required(ErrorMessage = "QuoteMarkupType is invalid or empty.")]
	public byte QuoteMarkupType { get; set; }

	[XmlElement(ElementName = "purchaseToOrder")]
	[DataMember(Name = "purchaseToOrder", Order = 8)]
	public bool PurchaseToOrder { get; set; }

	[XmlElement(ElementName = "setupHours")]
	[DataMember(Name = "setupHours", Order = 9)]
	public decimal SetupHours { get; set; }

	[XmlElement(ElementName = "productionHours")]
	[DataMember(Name = "productionHours", Order = 10)]
	public decimal ProductionHours { get; set; }

	[XmlElement(ElementName = "materialCost")]
	[DataMember(Name = "materialCost", Order = 11)]
	public decimal MaterialCost { get; set; }

	[XmlElement(ElementName = "materialMarkupPercent")]
	[DataMember(Name = "materialMarkupPercent", Order = 12)]
	public decimal MaterialMarkupPercent { get; set; }

	[XmlElement(ElementName = "materialPrice")]
	[DataMember(Name = "materialPrice", Order = 13)]
	public decimal MaterialPrice { get; set; }

	[XmlElement(ElementName = "subcontractPrice")]
	[DataMember(Name = "subcontractPrice", Order = 14)]
	public decimal SubcontractPrice { get; set; }

	[XmlElement(ElementName = "laborCost")]
	[DataMember(Name = "laborCost", Order = 15)]
	public decimal LaborCost { get; set; }

	[XmlElement(ElementName = "laborMarkupPercent")]
	[DataMember(Name = "laborMarkupPercent", Order = 16)]
	public decimal LaborMarkupPercent { get; set; }

	[XmlElement(ElementName = "laborPrice")]
	[DataMember(Name = "laborPrice", Order = 17)]
	public decimal LaborPrice { get; set; }

	[XmlElement(ElementName = "overheadCost")]
	[DataMember(Name = "overheadCost", Order = 18)]
	public decimal OverheadCost { get; set; }

	[XmlElement(ElementName = "overheadMarkupPercent")]
	[DataMember(Name = "overheadMarkupPercent", Order = 19)]
	public decimal OverheadMarkupPercent { get; set; }

	[XmlElement(ElementName = "overheadPrice")]
	[DataMember(Name = "overheadPrice", Order = 20)]
	public decimal OverheadPrice { get; set; }

	[XmlElement(ElementName = "quotingPrice")]
	[DataMember(Name = "quotingPrice", Order = 21)]
	public decimal QuotingPrice { get; set; }

	[XmlElement(ElementName = "purchaseUnitCostBase")]
	[DataMember(Name = "purchaseUnitCostBase", Order = 22)]
	public decimal PurchaseUnitCostBase { get; set; }

	[XmlElement(ElementName = "purchaseToOrderCost")]
	[DataMember(Name = "purchaseToOrderCost", Order = 23)]
	public decimal PurchaseToOrderCost { get; set; }

	[XmlElement(ElementName = "purToOrderMarkupPercent")]
	[DataMember(Name = "purToOrderMarkupPercent", Order = 24)]
	public decimal PurToOrderMarkupPercent { get; set; }

	[XmlElement(ElementName = "purchaseToOrderPrice")]
	[DataMember(Name = "purchaseToOrderPrice", Order = 25)]
	public decimal PurchaseToOrderPrice { get; set; }

	[XmlElement(ElementName = "additionalCostAmount")]
	[DataMember(Name = "additionalCostAmount", Order = 26)]
	public decimal AdditionalCostAmount { get; set; }

	[XmlElement(ElementName = "additionalMarkupPercent")]
	[DataMember(Name = "additionalMarkupPercent", Order = 27)]
	public decimal AdditionalMarkupPercent { get; set; }

	[XmlElement(ElementName = "additionalCostPrice")]
	[DataMember(Name = "additionalCostPrice", Order = 28)]
	public decimal AdditionalCostPrice { get; set; }

	[XmlElement(ElementName = "totalCost")]
	[DataMember(Name = "totalCost", Order = 29)]
	public decimal TotalCost { get; set; }

	[XmlElement(ElementName = "totalPrice")]
	[DataMember(Name = "totalPrice", Order = 30)]
	public decimal TotalPrice { get; set; }

	[XmlElement(ElementName = "totalUnitCost")]
	[DataMember(Name = "totalUnitCost", Order = 31)]
	public decimal TotalUnitCost { get; set; }

	[XmlElement(ElementName = "totalMarkupPercent")]
	[DataMember(Name = "totalMarkupPercent", Order = 32)]
	public decimal TotalMarkupPercent { get; set; }

	[XmlElement(ElementName = "totalUnitPrice")]
	[DataMember(Name = "totalUnitPrice", Order = 33)]
	public decimal TotalUnitPrice { get; set; }

	[XmlElement(ElementName = "calculatedUnitPrice")]
	[DataMember(Name = "calculatedUnitPrice", Order = 34)]
	public decimal CalculatedUnitPrice { get; set; }

	[XmlElement(ElementName = "fullRevisedUnitPriceForeign")]
	[DataMember(Name = "fullRevisedUnitPriceForeign", Order = 35)]
	public decimal FullRevisedUnitPriceForeign { get; set; }

	[XmlElement(ElementName = "discountPercent")]
	[DataMember(Name = "discountPercent", Order = 36)]
	public decimal DiscountPercent { get; set; }

	[XmlElement(ElementName = "unitDiscountForeign")]
	[DataMember(Name = "unitDiscountForeign", Order = 37)]
	public decimal UnitDiscountForeign { get; set; }

	[XmlElement(ElementName = "revisedUnitPriceForeign")]
	[DataMember(Name = "revisedUnitPriceForeign", Order = 38)]
	public decimal RevisedUnitPriceForeign { get; set; }

	[XmlElement(ElementName = "additionalChargeForeign")]
	[DataMember(Name = "additionalChargeForeign", Order = 39)]
	public decimal AdditionalChargeForeign { get; set; }

	[XmlElement(ElementName = "additionalChargeDescription")]
	[DataMember(Name = "additionalChargeDescription", Order = 40)]
	public string AdditionalChargeDescription { get; set; }

	[XmlElement(ElementName = "leadTime")]
	[DataMember(Name = "leadTime", Order = 41)]
	public string LeadTime { get; set; }

	[XmlElement(ElementName = "closed")]
	[DataMember(Name = "closed", Order = 42)]
	public bool Closed { get; set; }

	[XmlElement(ElementName = "createdBy")]
	[DataMember(Name = "createdBy", Order = 43)]
	public string CreatedBy { get; set; }

	[XmlElement(ElementName = "createdDate")]
	[DataMember(Name = "createdDate", Order = 44)]
	public DateTime? CreatedDate { get; set; }

	[XmlElement(ElementName = "uniqueID")]
	[DataMember(Name = "uniqueID", Order = 45)]
	public Guid UniqueID { get; set; }

	[XmlElement(ElementName = "rowVersion")]
	[DataMember(Name = "rowVersion", Order = 46)]
	public byte[] RowVersion { get; set; }
}
