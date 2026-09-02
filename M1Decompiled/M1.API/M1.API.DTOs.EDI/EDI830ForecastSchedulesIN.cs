using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using M1.API.Attributes;

namespace M1.API.DTOs.EDI;

[DataContract(Namespace = "", Name = "edI830ForecastSchedules")]
[XmlRoot(ElementName = "edI830ForecastSchedules")]
public class EDI830ForecastSchedulesIN
{
	[EnsureMinimumElements(1, ErrorMessage = "ForecastSchedule is invalid or empty.")]
	[XmlElement(ElementName = "edI830ForecastSchedule")]
	[DataMember(Name = "edI830ForecastSchedule", Order = 1)]
	public List<EDI830ForecastScheduleIN> EDI830ForecastScheduleSet { get; set; } = new List<EDI830ForecastScheduleIN>();
}
