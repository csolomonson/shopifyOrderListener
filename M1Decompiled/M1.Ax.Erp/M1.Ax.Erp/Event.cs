using System;
using System.Xml.Serialization;

namespace M1.Ax.Erp;

[Serializable]
[XmlType(AnonymousType = true, Namespace = "http://sbr.gov.au/comn/event.02.data")]
[XmlRoot(Namespace = "http://sbr.gov.au/comn/event.02.data", IsNullable = false)]
public class Event
{
	[XmlElement("MaximumSeverity.Code")]
	public string MaximumSeverityCode { get; set; }

	[XmlArray("EventItems")]
	[XmlArrayItem("EventItem")]
	public EventItem[] EventItems { get; set; }
}
