using System.Xml.Serialization;

namespace M1.Ax.Erp.Financials.AIR;

public class MailingAddressGrpType
{
	[XmlElement("USAddressGroup")]
	public USAddressGrpType USAddressGroup { get; set; }
}
