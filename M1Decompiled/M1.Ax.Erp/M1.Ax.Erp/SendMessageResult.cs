using System;
using System.Xml.Serialization;

namespace M1.Ax.Erp;

[Serializable]
[XmlRoot("sendmessage_result", Namespace = "", IsNullable = false)]
public class SendMessageResult
{
	[XmlElement("result")]
	public string Result { get; set; }

	[XmlElement("error")]
	public SendMessageError Error { get; set; }

	[XmlElement]
	public string Description { get; set; }

	[XmlElement("messageloginid")]
	public long MessageLoginId { get; set; }
}
