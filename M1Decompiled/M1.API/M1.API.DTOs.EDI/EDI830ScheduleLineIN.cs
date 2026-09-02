using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using M1.Extensions;
using Newtonsoft.Json;

namespace M1.API.DTOs.EDI;

[DataContract(Name = "edI830ScheduleLine", Namespace = "")]
[XmlRoot(ElementName = "edI830ScheduleLine")]
public class EDI830ScheduleLineIN
{
	[XmlIgnore]
	[JsonIgnore]
	[Required(ErrorMessage = "ScheduleLineID is invalid or empty.")]
	public short? ScheduleLineID { get; set; }

	[XmlElement(ElementName = "scheduleLineID")]
	[DataMember(Name = "scheduleLineID")]
	public string ScheduleLineIDStr
	{
		get
		{
			return ScheduleLineID.ToString();
		}
		set
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				ScheduleLineID = null;
			}
			else if (M1Util.IsNumeric(value))
			{
				ScheduleLineID = short.Parse(value);
			}
			else
			{
				ScheduleLineID = null;
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

	[DataMember(Name = "orgPartShortDescription")]
	[XmlElement(ElementName = "orgPartShortDescription")]
	public string OrgPartShortDescription { get; set; }

	[DataMember(Name = "releaseNumber")]
	[XmlElement(ElementName = "releaseNumber")]
	public string ReleaseNumber { get; set; }

	[DataMember(Name = "edI830ForecastSchedules")]
	[XmlElement(ElementName = "edI830ForecastSchedules")]
	public EDI830ForecastSchedulesIN EDI830ForecastSchedules { get; set; }
}
