using System.Xml.Serialization;

namespace M1.Ax.Erp.Financials.AIR;

public class USAddressGrpType
{
	[XmlElement("AddressLine1Txt")]
	public string AddressLine1Txt { get; set; }

	[XmlElement("AddressLine2Txt")]
	public string AddressLine2Txt { get; set; }

	[XmlElement("CityNm", Namespace = "urn:us:gov:treasury:irs:common")]
	public string CityNm { get; set; }

	[XmlElement("USStateCd")]
	public string USStateCd { get; set; }

	[XmlElement("USZIPCd", Namespace = "urn:us:gov:treasury:irs:common")]
	public string USZIPCd { get; set; }

	[XmlElement("USZIPExtensionCd", Namespace = "urn:us:gov:treasury:irs:common")]
	public string USZIPExtensionCd { get; set; }
}
