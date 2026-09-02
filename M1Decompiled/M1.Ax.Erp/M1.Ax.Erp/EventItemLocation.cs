using System;
using System.Xml.Serialization;

namespace M1.Ax.Erp;

[Serializable]
public class EventItemLocation
{
	[XmlElement("Location.Instance.Identifier")]
	public string LocationInstanceIdentifier { get; set; }

	[XmlElement("Location.Path.Text")]
	public string LocationPathText { get; set; }
}
