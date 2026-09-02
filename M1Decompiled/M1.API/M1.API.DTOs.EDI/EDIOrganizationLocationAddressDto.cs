using System.Runtime.Serialization;

namespace M1.API.DTOs.EDI;

[DataContract(Namespace = "")]
public class EDIOrganizationLocationAddressDto : OrganizationLocationAddressDto
{
	[DataMember(Name = "m1LocationID")]
	public string M1LocationID { get; set; }

	[DataMember(Name = "edILocationID")]
	public string EDILocationID { get; set; }
}
