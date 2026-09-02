using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using M1.API.Utilities;
using Newtonsoft.Json;

namespace M1.API.DTOs.EDI;

[DataContract(Name = "edI830ScheduleIn", Namespace = "")]
[XmlRoot(ElementName = "edI830ScheduleIn")]
public class EDI830ScheduleIN
{
	[Required(ErrorMessage = "ScheduleID is invalid or empty.")]
	[DataMember(Name = "scheduleID")]
	[XmlElement(ElementName = "scheduleID")]
	public string ScheduleID { get; set; }

	[Required(ErrorMessage = "Purpose is invalid or empty.")]
	[DataMember(Name = "purpose")]
	[XmlElement(ElementName = "purpose")]
	public string Purpose { get; set; }

	[DataMember(Name = "releaseNumber")]
	[XmlElement(ElementName = "releaseNumber")]
	public string ReleaseNumber { get; set; }

	[XmlIgnore]
	[JsonIgnore]
	[Required(ErrorMessage = "ForecastCreateDate is invalid or empty.")]
	public DateTime? ForecastCreateDate { get; set; }

	[XmlElement(ElementName = "forecastCreateDate")]
	[DataMember(Name = "forecastCreateDate")]
	public string ForecastCreateDateStr
	{
		get
		{
			return ForecastCreateDate.ToString();
		}
		set
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				ForecastCreateDate = null;
			}
			else
			{
				ForecastCreateDate = APICommonFunctions.GetDateConvertedValue(value);
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

	[DataMember(Name = "shippingMethodID")]
	[XmlElement(ElementName = "shippingMethodID")]
	public string ShippingMethodID { get; set; }

	[DataMember(Name = "plantID")]
	[XmlElement(ElementName = "plantID")]
	public string PlantID { get; set; }

	[DataMember(Name = "fobDescription")]
	[XmlElement(ElementName = "fobDescription")]
	public string FOBDescription { get; set; }

	[DataMember(Name = "edI830ScheduleNotes")]
	[XmlElement(ElementName = "edI830ScheduleNotes")]
	public EDI830ScheduleNotesIN EDI830ScheduleNotes { get; set; }

	[DataMember(Name = "edI830ScheduleLines")]
	[XmlElement(ElementName = "edI830ScheduleLines")]
	public EDI830ScheduleLinesIN EDI830ScheduleLines { get; set; }
}
