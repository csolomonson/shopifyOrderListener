using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using M1.API.DTOs.Core;

namespace M1.API.DTOs.EDI;

[Serializable]
[DataContract(Namespace = "", Name = "salesOrder")]
public class EDISalesOrderResponseItemDto
{
	[DataMember(Name = "salesOrderID", Order = 1)]
	public string SalesOrderID { get; set; }

	[DataMember(Name = "status", Order = 2)]
	public string Status { get; set; }

	[DataMember(Name = "customerPO", Order = 3)]
	public string CustomerPO { get; set; }

	[DataMember(Name = "m1SalesOrderID", Order = 4)]
	public string M1SalesOrderID { get; set; }

	[DataMember(EmitDefaultValue = false, Name = "warnings", Order = 5)]
	public List<Warning> Warnings { get; set; }

	[DataMember(EmitDefaultValue = false, Name = "errors", Order = 6)]
	public List<Error> Errors { get; set; }
}
