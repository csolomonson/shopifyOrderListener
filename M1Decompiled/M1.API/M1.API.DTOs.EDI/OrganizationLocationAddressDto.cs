using System.Runtime.Serialization;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace M1.API.DTOs.EDI;

[DataContract(Namespace = "")]
[KnownType(typeof(EDIOrganizationLocationAddressDto))]
public class OrganizationLocationAddressDto
{
	[XmlIgnore]
	[JsonIgnore]
	public string OrganizationID { get; set; }

	[DataMember(Name = "locationName")]
	public string LocationName { get; set; }

	[DataMember(Name = "contactID")]
	public string ContactID { get; set; }

	[DataMember(Name = "addressLine")]
	public string AddressLine { get; set; }

	[DataMember(Name = "country")]
	public string Country { get; set; }

	[DataMember(Name = "state")]
	public string State { get; set; }

	[DataMember(Name = "city")]
	public string City { get; set; }

	[DataMember(Name = "postCode")]
	public string PostCode { get; set; }

	[DataMember(Name = "phoneNumber")]
	public string PhoneNumber { get; set; }
}
