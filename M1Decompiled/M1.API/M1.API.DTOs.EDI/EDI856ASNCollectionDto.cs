using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace M1.API.DTOs.EDI;

[Serializable]
[DataContract(Namespace = "", Name = "edI856ShipmentCollection")]
public class EDI856ASNCollectionDto
{
	[DataMember(Name = "totalRecords", Order = 1)]
	public int TotalRecords { get; set; }

	[DataMember(Name = "pageSize", Order = 2)]
	public int PageSize { get; set; }

	[DataMember(Name = "totalPages", Order = 3)]
	public int TotalPages { get; set; }

	[DataMember(Name = "currentPageIndex", Order = 4)]
	public int CurrentPageIndex { get; set; }

	[DataMember(Name = "edI856Shipments", Order = 5)]
	public List<EDI856OutboundASN> EDI856ShipmentSet { get; set; }
}
