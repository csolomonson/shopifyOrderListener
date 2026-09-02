using System;
using System.Runtime.Serialization;

namespace M1.API.DTOs.Core;

[Serializable]
[DataContract(Namespace = "")]
public class OrganizationLocationSalespeopleDto : SalesPeopleDto
{
	[DataMember(Name = "OrganizationID", Order = 3)]
	public string OrganizationID { get; set; }

	[DataMember(Name = "LocationID", Order = 4)]
	public string LocationID { get; set; }
}
