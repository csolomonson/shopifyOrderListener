using System;
using System.Runtime.Serialization;

namespace M1.API.DTOs.Core;

[Serializable]
[DataContract(Namespace = "")]
public abstract class SalesPeopleDto
{
	[IgnoreDataMember]
	public short SequenceID { get; set; }

	[DataMember(Name = "SalesEmployeeID", Order = 1)]
	public string SalesEmployeeID { get; set; }

	[DataMember(Name = "Percent", Order = 2)]
	public decimal Percent { get; set; }
}
