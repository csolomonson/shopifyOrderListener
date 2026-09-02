using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using M1.API.Attributes;

namespace M1.API.DTOs.EDI;

[DataContract(Namespace = "", Name = "edI830ScheduleLines")]
[XmlRoot(ElementName = "edI830ScheduleLines")]
public class EDI830ScheduleLinesIN
{
	[EnsureMinimumElements(1, ErrorMessage = "EDI830ScheduleLines is invalid or empty.")]
	[XmlElement(ElementName = "edI830ScheduleLine")]
	[DataMember(Name = "edI830ScheduleLine", Order = 1)]
	public List<EDI830ScheduleLineIN> EDI830ScheduleLineSet { get; set; } = new List<EDI830ScheduleLineIN>();
}
