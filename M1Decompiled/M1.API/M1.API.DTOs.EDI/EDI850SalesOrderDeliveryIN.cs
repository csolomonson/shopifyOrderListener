using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using M1.API.Utilities;
using M1.Extensions;
using Newtonsoft.Json;

namespace M1.API.DTOs.EDI;

[DataContract(Namespace = "", Name = "edI850SalesOrderDelivery")]
[XmlRoot(ElementName = "edI850SalesOrderDelivery")]
public class EDI850SalesOrderDeliveryIN
{
	[XmlIgnore]
	[JsonIgnore]
	[Required(ErrorMessage = "SalesOrderDeliveryID is invalid or empty.")]
	public short? SalesOrderDeliveryID { get; set; }

	[XmlElement(ElementName = "salesOrderDeliveryID")]
	[DataMember(Name = "salesOrderDeliveryID")]
	public string SalesOrderDeliveryIDStr
	{
		get
		{
			return SalesOrderDeliveryID.ToString();
		}
		set
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				SalesOrderDeliveryID = null;
			}
			else if (M1Util.IsNumeric(value))
			{
				SalesOrderDeliveryID = short.Parse(value);
			}
			else
			{
				SalesOrderDeliveryID = null;
			}
		}
	}

	[Range(0.0001, double.MaxValue, ErrorMessage = "DeliveryQuantity should be between {1} and {2}.")]
	[XmlIgnore]
	[JsonIgnore]
	public decimal? DeliveryQuantity { get; set; }

	[XmlElement(ElementName = "deliveryQuantity")]
	[DataMember(Name = "deliveryQuantity")]
	public string DeliveryQuantityStr
	{
		get
		{
			return DeliveryQuantity.ToString();
		}
		set
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				DeliveryQuantity = default(decimal);
			}
			else if (M1Util.IsNumeric(value, includeNegatives: false))
			{
				DeliveryQuantity = decimal.Parse(value);
			}
			else
			{
				DeliveryQuantity = default(decimal);
			}
		}
	}

	[Required(ErrorMessage = "DeliveryDate is invalid or empty.")]
	[XmlIgnore]
	[JsonIgnore]
	public DateTime? DeliveryDate { get; set; }

	[XmlElement(ElementName = "deliveryDate")]
	[DataMember(Name = "deliveryDate")]
	public string DeliveryDateStr
	{
		get
		{
			return DeliveryDate.ToString();
		}
		set
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				DeliveryDate = null;
			}
			else
			{
				DeliveryDate = APICommonFunctions.GetDateConvertedValue(value);
			}
		}
	}
}
