using System;
using System.Xml.Serialization;

namespace M1.Ax.Erp;

[Serializable]
[XmlRoot("stp_log", Namespace = "")]
[XmlType(AnonymousType = true)]
public class StpLog
{
	[XmlElement("record")]
	public Record Record { get; set; }
}
