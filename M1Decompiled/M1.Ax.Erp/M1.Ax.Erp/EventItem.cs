using System;
using System.Xml.Serialization;

namespace M1.Ax.Erp;

[Serializable]
public class EventItem
{
	[XmlElement("Error.Code")]
	public string ErrorCode { get; set; }

	[XmlElement("Severity.Code")]
	public string SeverityCode { get; set; }

	[XmlElement("Short.Description")]
	public string ShortDescription { get; set; }

	[XmlElement("Detailed.Description")]
	public string DetailedDescription { get; set; }

	[XmlArray("Parameters")]
	[XmlArrayItem("Parameter")]
	public EventItemParameter[] Parameters { get; set; }

	[XmlArray("Locations")]
	[XmlArrayItem("Location")]
	public EventItemLocation[] Locations { get; set; }
}
