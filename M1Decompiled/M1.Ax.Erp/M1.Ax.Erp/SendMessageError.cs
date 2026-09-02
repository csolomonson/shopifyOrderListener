using System;
using System.Xml.Serialization;

namespace M1.Ax.Erp;

[Serializable]
public class SendMessageError
{
	[XmlElement("code")]
	public ushort Code { get; set; }
}
