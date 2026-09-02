using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace M1.API.DTOs.EDI;

[DataContract(Name = "edI830ScheduleNote", Namespace = "")]
[XmlRoot(ElementName = "edI830ScheduleNote")]
public class EDI830ScheduleNoteIN
{
	[DataMember(Name = "noteType")]
	[XmlElement(ElementName = "noteType")]
	public string NoteType { get; set; }

	[XmlElement(ElementName = "noteText")]
	[DataMember(Name = "noteText")]
	public string NoteText { get; set; }
}
