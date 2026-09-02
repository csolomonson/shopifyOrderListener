using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using M1.API.Utilities;
using M1.Extensions;
using Newtonsoft.Json;

namespace M1.API.DTOs.EDI;

[DataContract(Name = "edI830ForecastSchedule", Namespace = "")]
[XmlRoot(ElementName = "edI830ForecastSchedule")]
public class EDI830ForecastScheduleIN
{
	[XmlIgnore]
	[JsonIgnore]
	[Required(ErrorMessage = "ForecastQuantity is invalid or empty.")]
	[Range(0.0001, double.MaxValue, ErrorMessage = "ForecastQuantity should be between {1} and {2}.")]
	public decimal? ForecastQuantity { get; set; }

	[XmlElement(ElementName = "forecastQuantity")]
	[DataMember(Name = "forecastQuantity")]
	public string ForecastQuantityStr
	{
		get
		{
			return ForecastQuantity.ToString();
		}
		set
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				ForecastQuantity = null;
			}
			else if (M1Util.IsNumeric(value, includeNegatives: false))
			{
				ForecastQuantity = decimal.Parse(value);
			}
			else
			{
				ForecastQuantity = null;
			}
		}
	}

	[Required(ErrorMessage = "ForecastQualifier is invalid or empty.")]
	[DataMember(Name = "forecastQualifier")]
	[XmlElement(ElementName = "forecastQualifier")]
	public string ForecastQualifier { get; set; }

	[XmlIgnore]
	[JsonIgnore]
	[Required(ErrorMessage = "ForecastDate is invalid or empty.")]
	public DateTime? ForecastDate { get; set; }

	[XmlElement(ElementName = "forecastDate")]
	[DataMember(Name = "forecastDate")]
	public string ForecastDateStr
	{
		get
		{
			return ForecastDate.ToString();
		}
		set
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				ForecastDate = null;
			}
			else
			{
				ForecastDate = APICommonFunctions.GetDateConvertedValue(value);
			}
		}
	}
}
