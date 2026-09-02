using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace M1.API.DTOs.Core;

[Serializable]
[DataContract(Namespace = "")]
public class SalesOrderLineDto
{
	[DataMember(Name = "SalesOrderID", Order = 1)]
	public string SalesOrderID { get; set; }

	[DataMember(Name = "SalesOrderLineID", Order = 2)]
	public short SalesOrderLineID { get; set; }

	[DataMember(Name = "PartID", Order = 3)]
	public string PartID { get; set; }

	[DataMember(Name = "OrgPartID", Order = 4)]
	public string OrgPartID { get; set; }

	[DataMember(Name = "PartRevisionID", Order = 5)]
	public string PartRevisionID { get; set; }

	[DataMember(Name = "UnitOfMeasure", Order = 6)]
	public string UnitOfMeasure { get; set; }

	[DataMember(Name = "PartGroupID", Order = 7)]
	public string PartGroupID { get; set; }

	[DataMember(Name = "PartShortDescription", Order = 8)]
	public string PartShortDescription { get; set; }

	[DataMember(Name = "OrgPartShortDescription", Order = 9)]
	public string OrgPartShortDescription { get; set; }

	[DataMember(Name = "PartLongDescriptionText", Order = 10)]
	public string PartLongDescriptionText { get; set; }

	[DataMember(Name = "PartLongDescriptionRTF", Order = 11)]
	public string PartLongDescriptionRTF { get; set; }

	[DataMember(Name = "OrderQuantity", Order = 12)]
	public decimal OrderQuantity { get; set; }

	[DataMember(Name = "FullUnitPriceBase", Order = 13)]
	public decimal FullUnitPriceBase { get; set; }

	[DataMember(Name = "FullUnitPriceForeign", Order = 14)]
	public decimal FullUnitPriceForeign { get; set; }

	[DataMember(Name = "UnitPriceBase", Order = 15)]
	public decimal UnitPriceBase { get; set; }

	[DataMember(Name = "UnitPriceForeign", Order = 16)]
	public decimal UnitPriceForeign { get; set; }

	[DataMember(Name = "FullExtendedPriceBase", Order = 17)]
	public decimal FullExtendedPriceBase { get; set; }

	[DataMember(Name = "FullExtendedPriceForeign", Order = 18)]
	public decimal FullExtendedPriceForeign { get; set; }

	[DataMember(Name = "ExtendedPriceBase", Order = 19)]
	public decimal ExtendedPriceBase { get; set; }

	[DataMember(Name = "ExtendedPriceForeign", Order = 20)]
	public decimal ExtendedPriceForeign { get; set; }

	[DataMember(Name = "TaxCodeID", Order = 21)]
	public string TaxCodeID { get; set; }

	[DataMember(Name = "SecondTaxCodeID", Order = 22)]
	public string SecondTaxCodeID { get; set; }

	[DataMember(Name = "TaxAmountBase", Order = 23)]
	public decimal TaxAmountBase { get; set; }

	[DataMember(Name = "TaxAmountForeign", Order = 24)]
	public decimal TaxAmountForeign { get; set; }

	[DataMember(Name = "Weight", Order = 25)]
	public decimal Weight { get; set; }

	[DataMember(Name = "DiscountPercent", Order = 26)]
	public decimal DiscountPercent { get; set; }

	[DataMember(Name = "SecondTaxAmountForeign", Order = 27)]
	public decimal SecondTaxAmountForeign { get; set; }

	[DataMember(Name = "SecondTaxAmountBase", Order = 28)]
	public decimal SecondTaxAmountBase { get; set; }

	[DataMember(Name = "NonTaxReasonID", Order = 29)]
	public string NonTaxReasonID { get; set; }

	[DataMember(Name = "ReleaseNumber", Order = 30)]
	public string ReleaseNumber { get; set; } = string.Empty;

	[XmlIgnore]
	public decimal UnitDiscountBase { get; set; }

	[XmlIgnore]
	public decimal UnitDiscountForeign { get; set; }

	[XmlIgnore]
	public string CreatedBy { get; set; }

	[XmlIgnore]
	public DateTime? CreatedDate { get; set; }

	[XmlIgnore]
	public string SourceUniqueID { get; set; }

	[DataMember(Name = "SalesOrderDeliveries", Order = 32)]
	public List<SalesOrderDeliveryDto> SalesOrderDeliveries { get; set; } = new List<SalesOrderDeliveryDto>();
}
