using System;
using System.Runtime.Serialization;

namespace M1.API.DTOs.Core;

[Serializable]
[DataContract(Namespace = "")]
public class SalesOrderSalespeopleDto : SalesPeopleDto
{
	[DataMember(Name = "SalesOrderID", Order = 3)]
	public string SalesOrderID { get; set; }
}
