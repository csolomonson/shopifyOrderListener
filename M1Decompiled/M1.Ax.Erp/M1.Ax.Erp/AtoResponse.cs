using System;
using System.Xml.Serialization;

namespace M1.Ax.Erp;

[Serializable]
[XmlType(AnonymousType = true)]
public class AtoResponse
{
	[XmlElement("Event", Namespace = "http://sbr.gov.au/comn/event.02.data")]
	public Event[] Event { get; set; }
}
