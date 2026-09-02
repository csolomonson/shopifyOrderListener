using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace M1.API.DTOs.Core;

[Serializable]
[XmlRoot("SalesOrderObject")]
[DataContract(Namespace = "", Name = "SalesOrderObject")]
public class SalesOrderDto
{
	[DataMember(Name = "SalesOrderID", Order = 1)]
	public string SalesOrderID { get; set; }

	[DataMember(Name = "CustomerPO", Order = 2)]
	public string CustomerPO { get; set; }

	[DataMember(Name = "RequestedShipDate", Order = 3)]
	public DateTime? RequestedShipDate { get; set; }

	[DataMember(Name = "OrderDate", Order = 4)]
	public DateTime OrderDate { get; set; }

	[DataMember(Name = "CustomerOrganizationID", Order = 5)]
	public string CustomerOrganizationID { get; set; }

	[DataMember(Name = "ShipOrganizationID", Order = 6)]
	public string ShipOrganizationID { get; set; }

	[DataMember(Name = "ShipLocationID", Order = 7)]
	public string ShipLocationID { get; set; }

	[DataMember(Name = "ShipContactID", Order = 8)]
	public string ShipContactID { get; set; } = string.Empty;

	[DataMember(Name = "ARInvoiceLocationID", Order = 9)]
	public string ARInvoiceLocationID { get; set; }

	[DataMember(Name = "ARInvoiceContactID", Order = 10)]
	public string ARInvoiceContactID { get; set; } = string.Empty;

	[DataMember(Name = "PaymentTermID", Order = 11)]
	public string PaymentTermID { get; set; }

	[DataMember(Name = "CurrencyRateID", Order = 12)]
	public string CurrencyRateID { get; set; }

	[DataMember(Name = "ExchangeRate", Order = 13)]
	public decimal ExchangeRate { get; set; }

	[DataMember(Name = "FullOrderSubtotalBase", Order = 14)]
	public decimal FullOrderSubtotalBase { get; set; }

	[DataMember(Name = "FullOrderSubtotalForeign", Order = 15)]
	public decimal FullOrderSubtotalForeign { get; set; }

	[DataMember(Name = "FreightAmountBase", Order = 16)]
	public decimal FreightAmountBase { get; set; }

	[DataMember(Name = "FreightAmountForeign", Order = 17)]
	public decimal FreightAmountForeign { get; set; }

	[DataMember(Name = "FreightTotalBase", Order = 18)]
	public decimal FreightTotalBase { get; set; }

	[DataMember(Name = "FreightTotalForeign", Order = 19)]
	public decimal FreightTotalForeign { get; set; }

	[DataMember(Name = "OrderSubtotalBase", Order = 20)]
	public decimal OrderSubtotalBase { get; set; }

	[DataMember(Name = "OrderSubTotalForeign", Order = 21)]
	public decimal OrderSubTotalForeign { get; set; }

	[DataMember(Name = "OrderTaxAmountBase", Order = 22)]
	public decimal OrderTaxAmountBase { get; set; }

	[DataMember(Name = "OrderTaxAmountForeign", Order = 23)]
	public decimal OrderTaxAmountForeign { get; set; }

	[DataMember(Name = "OrderTotalBase", Order = 24)]
	public decimal OrderTotalBase { get; set; }

	[DataMember(Name = "OrderTotalForeign", Order = 25)]
	public decimal OrderTotalForeign { get; set; }

	[DataMember(Name = "Status", Order = 26)]
	public byte Status { get; set; }

	[DataMember(Name = "OrderCommentsText", Order = 27)]
	public string OrderCommentsText { get; set; }

	[DataMember(Name = "OrderCommentsRTF", Order = 28)]
	public string OrderCommentsRTF { get; set; }

	[XmlIgnore]
	public string CreatedBy { get; set; }

	[XmlIgnore]
	public DateTime? CreatedDate { get; set; }

	[DataMember(Name = "CreatedByEDI", Order = 29)]
	public bool CreatedByEDI { get; set; }

	[DataMember(Name = "TotalOrderWeight", Order = 30)]
	public decimal TotalOrderWeight { get; set; }

	[XmlIgnore]
	public bool CreatedFromWeb { get; set; }

	[DataMember(Name = "EasyOrderID", Order = 32)]
	public string EasyOrderID { get; set; }

	[DataMember(Name = "EasyOrderEnabled", Order = 33)]
	public bool EasyOrderEnabled { get; set; }

	[DataMember(Name = "EasyOrderStatus", Order = 34)]
	public byte EasyOrderStatus { get; set; }

	[DataMember(Name = "FreightTaxCodeID", Order = 35)]
	public string FreightTaxCodeID { get; set; } = string.Empty;

	[DataMember(Name = "SecondFreightTaxCodeID", Order = 36)]
	public string SecondFreightTaxCodeID { get; set; } = string.Empty;

	[DataMember(Name = "PaidFromEasyOrder", Order = 37)]
	public int PaidFromEasyOrder { get; set; }

	[DataMember(Name = "PlantID", Order = 38)]
	public string PlantID { get; set; } = string.Empty;

	[DataMember(Name = "ShippingMethodID", Order = 39)]
	public string ShippingMethodID { get; set; } = string.Empty;

	[DataMember(Name = "FreeOnBoardDescription", Order = 40)]
	public string FreeOnBoardDescription { get; set; } = string.Empty;

	[DataMember(Name = "ShippingPaymentTypeID", Order = 41)]
	public string ShippingPaymentTypeID { get; set; } = string.Empty;

	[DataMember(Name = "SalesOrderLines", Order = 91)]
	public List<SalesOrderLineDto> SalesOrderLines { get; set; } = new List<SalesOrderLineDto>();

	[DataMember(Name = "SalesOrderSalesPeople", Order = 92)]
	public List<SalesOrderSalespeopleDto> SalesOrderSalesPeople { get; set; } = new List<SalesOrderSalespeopleDto>();

	public bool ShouldSerializeCreatedByEDI()
	{
		return CreatedByEDI;
	}

	public bool ShouldSerializeEasyOrderID()
	{
		return EasyOrderEnabled;
	}

	public bool ShouldSerializeEasyOrderEnabled()
	{
		return EasyOrderEnabled;
	}

	public bool ShouldSerializeEasyOrderStatus()
	{
		return EasyOrderEnabled;
	}

	public bool ShouldSerializePaidFromEasyOrder()
	{
		return EasyOrderEnabled;
	}
}
