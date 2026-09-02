using System;
using System.Xml.Serialization;

namespace M1.Ax.Erp;

[Serializable]
public class EventItemParameter
{
	[XmlElement("Parameter.Identifier")]
	public string ParameterIdentifier { get; set; }

	[XmlElement("Parameter.Text")]
	public string ParameterText { get; set; }
}
