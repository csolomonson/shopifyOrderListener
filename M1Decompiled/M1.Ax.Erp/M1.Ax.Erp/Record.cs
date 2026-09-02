using System;
using System.Xml.Serialization;

namespace M1.Ax.Erp;

[Serializable]
[XmlType("record", AnonymousType = true)]
public class Record
{
	[XmlElement("MessageLogInID")]
	public string MessageLogInId { get; set; }

	[XmlElement("MessageLogOutID")]
	public string MessageLogOutId { get; set; }

	[XmlElement("Time")]
	public string Time { get; set; }

	[XmlElement("conversationid")]
	public string ConversationId { get; set; }

	[XmlElement("employer_abn")]
	public string EmployerAbn { get; set; }

	[XmlElement("statuscode")]
	public string StatusCode { get; set; }

	[XmlElement("statusdescription")]
	public string StatusDescription { get; set; }

	[XmlElement("ato_response")]
	public AtoResponse AtoResponse { get; set; }

	[XmlElement("messageid")]
	public string MeessageId { get; set; }
}
