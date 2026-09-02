using System;
using System.Xml.Serialization;

namespace M1.Ax.Erp.Financials.AIR;

[Serializable]
public class ACATransmitterBusinessHeaderRequestDTO
{
	[XmlElement("UniqueTransmissionId")]
	public string A01_UniqueTransmissionId { get; set; }

	[XmlElement("Timestamp", Namespace = "urn:us:gov:treasury:irs:common")]
	public string A02_Timestamp { get; set; }

	[XmlAttribute("Id", Namespace = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd")]
	public int A03_Id { get; set; }
}
