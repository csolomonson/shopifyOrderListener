using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace M1.API.DTOs.EDI;

[DataContract(Namespace = "", Name = "edI830Schedules")]
[XmlRoot(ElementName = "edI830Schedules")]
public class EDI830SchedulesIN
{
	[XmlElement(ElementName = "edI830Schedule")]
	[DataMember(Name = "edI830Schedule")]
	public List<EDI830ScheduleIN> EDI830ScheduleSet { get; set; } = new List<EDI830ScheduleIN>();
}
