using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace M1.API.DTOs.EDI;

[DataContract(Namespace = "", Name = "edI830ScheduleNotes")]
[XmlRoot(ElementName = "edI830ScheduleNotes")]
public class EDI830ScheduleNotesIN
{
	[XmlElement(ElementName = "edI830ScheduleNote")]
	[DataMember(Name = "edI830ScheduleNote", Order = 1)]
	public List<EDI830ScheduleNoteIN> EDI830ScheduleNoteSet { get; set; } = new List<EDI830ScheduleNoteIN>();
}
