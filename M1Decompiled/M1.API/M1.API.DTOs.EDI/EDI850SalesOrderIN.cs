using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using M1.API.Utilities;
using Newtonsoft.Json;

namespace M1.API.DTOs.EDI;

[DataContract(Namespace = "", Name = "edI850SalesOrder")]
[XmlRoot(ElementName = "edI850SalesOrder")]
public class EDI850SalesOrderIN
{
	[Required(ErrorMessage = "SalesOrderID is invalid or empty.")]
	[DataMember(Name = "salesOrderID")]
	[XmlElement(ElementName = "salesOrderID")]
	public string SalesOrderID { get; set; }

	[DataMember(Name = "shippingMethodID")]
	[XmlElement(ElementName = "shippingMethodID")]
	public string ShippingMethodID { get; set; }

	[XmlIgnore]
	[JsonIgnore]
	[Required(ErrorMessage = "OrderDate is invalid or empty.")]
	public DateTime? OrderDate { get; set; }

	[XmlElement(ElementName = "orderDate")]
	[DataMember(Name = "orderDate")]
	public string OrderDateStr
	{
		get
		{
			return OrderDate.ToString();
		}
		set
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				OrderDate = null;
			}
			else
			{
				OrderDate = APICommonFunctions.GetDateConvertedValue(value);
			}
		}
	}

	[Required(ErrorMessage = "CustomerPO is invalid or empty.")]
	[DataMember(Name = "customerPO")]
	[XmlElement(ElementName = "customerPO")]
	public string CustomerPO { get; set; }

	[Required(ErrorMessage = "CustomerOrganizationID is invalid or empty.")]
	[DataMember(Name = "customerOrganizationID")]
	[XmlElement(ElementName = "customerOrganizationID")]
	public string CustomerOrganizationID { get; set; }

	[DataMember(Name = "shipLocationID")]
	[XmlElement(ElementName = "shipLocationID")]
	public M1Location ShipLocationID { get; set; }

	[DataMember(Name = "arInvoiceLocationID")]
	[XmlElement(ElementName = "arInvoiceLocationID")]
	public M1Location ARInvoiceLocationID { get; set; }

	[XmlIgnore]
	[JsonIgnore]
	public DateTime? RequestedShipDate { get; set; }

	[XmlElement(ElementName = "requestedShipDate")]
	[DataMember(Name = "requestedShipDate")]
	public string RequestedShipDateStr
	{
		get
		{
			return RequestedShipDate.ToString();
		}
		set
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				RequestedShipDate = null;
			}
			else
			{
				RequestedShipDate = APICommonFunctions.GetDateConvertedValue(value);
			}
		}
	}

	[DataMember(Name = "orderCommentsText")]
	[XmlElement(ElementName = "orderCommentsText")]
	public string OrderCommentsText { get; set; }

	[DataMember(Name = "edI850SalesOrderLines")]
	[XmlElement(ElementName = "edI850SalesOrderLines")]
	public EDI850SalesOrderLinesIN EDI850SalesOrderLines { get; set; }
}
