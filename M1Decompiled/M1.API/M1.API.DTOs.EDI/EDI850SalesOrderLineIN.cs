using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using M1.Extensions;
using Newtonsoft.Json;

namespace M1.API.DTOs.EDI;

[Serializable]
[DataContract(Namespace = "", Name = "edI850SalesOrderLine")]
[XmlRoot(ElementName = "edI850SalesOrderLine")]
public class EDI850SalesOrderLineIN
{
	[XmlIgnore]
	[JsonIgnore]
	[Required(ErrorMessage = "SalesOrderLineID is invalid or empty.")]
	public short? SalesOrderLineID { get; set; }

	[XmlElement(ElementName = "salesOrderLineID")]
	[DataMember(Name = "salesOrderLineID")]
	public string SalesOrderLineIDStr
	{
		get
		{
			return SalesOrderLineID.ToString();
		}
		set
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				SalesOrderLineID = null;
			}
			else if (M1Util.IsNumeric(value))
			{
				SalesOrderLineID = short.Parse(value);
			}
			else
			{
				SalesOrderLineID = null;
			}
		}
	}

	[Required(ErrorMessage = "OrgPartID is invalid or empty.")]
	[DataMember(Name = "orgPartID")]
	[XmlElement(ElementName = "orgPartID")]
	public string OrgPartID { get; set; }

	[Required(AllowEmptyStrings = true, ErrorMessage = "PartRevisionID is invalid or empty.")]
	[DataMember(Name = "partRevisionID")]
	[XmlElement(ElementName = "partRevisionID")]
	public string PartRevisionID { get; set; }

	[DataMember(Name = "orgContractNo")]
	[XmlElement(ElementName = "orgContractNo")]
	public string OrgContractNo { get; set; }

	[DataMember(Name = "orgPartShortDescription")]
	[XmlElement(ElementName = "orgPartShortDescription")]
	public string OrgPartShortDescription { get; set; }

	[Range(0.0001, double.MaxValue, ErrorMessage = "OrderQuantity should be between {1} and {2}.")]
	[XmlIgnore]
	[JsonIgnore]
	public decimal? OrderQuantity { get; set; }

	[XmlElement(ElementName = "orderQuantity")]
	[DataMember(Name = "orderQuantity")]
	public string OrderQuantityStr
	{
		get
		{
			return OrderQuantity.ToString();
		}
		set
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				OrderQuantity = default(decimal);
			}
			else if (M1Util.IsNumeric(value, includeNegatives: false))
			{
				OrderQuantity = decimal.Parse(value);
			}
			else
			{
				OrderQuantity = default(decimal);
			}
		}
	}

	[XmlIgnore]
	[JsonIgnore]
	[Required(ErrorMessage = "FullUnitPriceBase is invalid.")]
	public decimal? FullUnitPriceBase { get; set; }

	[XmlElement(ElementName = "fullUnitPriceBase")]
	[DataMember(Name = "fullUnitPriceBase")]
	public string FullUnitPriceBaseStr
	{
		get
		{
			return FullUnitPriceBase.ToString();
		}
		set
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				FullUnitPriceBase = default(decimal);
			}
			else if (M1Util.IsNumeric(value, includeNegatives: false))
			{
				FullUnitPriceBase = decimal.Parse(value);
			}
			else
			{
				FullUnitPriceBase = null;
			}
		}
	}

	[DataMember(Name = "edI850SalesOrderDeliveries")]
	[XmlElement(ElementName = "edI850SalesOrderDeliveries")]
	public EDI850SalesOrderDeliveriesIN EDI850SalesOrderDeliveries { get; set; }
}
