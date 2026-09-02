using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using Newtonsoft.Json;

namespace M1.API.DTOs.EDI;

[Serializable]
[DataContract(Namespace = "", Name = "responseSet")]
public class EDIOrderResponseMessageDto
{
	[IgnoreDataMember]
	public int ResponseID { get; set; }

	[DataMember(EmitDefaultValue = false, IsRequired = false, Name = "status", Order = 1)]
	public string Status
	{
		get
		{
			List<Error> list = EDIOrderResponses?.SelectMany((EDISalesOrderResponseItemDto x) => x.Errors).ToList();
			List<Warning> list2 = EDIOrderResponses?.SelectMany((EDISalesOrderResponseItemDto x) => x.Warnings).ToList();
			list.AddRange(new List<Error>(Errors));
			list2.AddRange(new List<Warning>(Warnings));
			if (list != null && list.Count > 0 && list2 != null && list2.Count > 0)
			{
				return ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.ErrorsAndWarnings.ToString();
			}
			if (list != null && list.Count > 0)
			{
				return ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Error.ToString();
			}
			if (list2 != null && list2.Count > 0 && list != null && list.Count == 0)
			{
				return ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.SuccessWithWarnings.ToString();
			}
			return ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success.ToString();
		}
		set
		{
		}
	}

	[DataMember(EmitDefaultValue = false, IsRequired = false, Name = "payloadID", Order = 2)]
	public string PayloadID { get; set; }

	[DataMember(Name = "totalOrders", Order = 3)]
	public int TotalOrders { get; set; }

	[DataMember(Name = "ordersCreated", Order = 4)]
	public int OrdersCreated { get; set; }

	[DataMember(Name = "description", Order = 5)]
	[JsonProperty(PropertyName = "description")]
	public string Description { get; set; } = string.Empty;

	[DataMember(EmitDefaultValue = false, Name = "errors", Order = 6)]
	public List<Error> Errors { get; set; }

	[DataMember(EmitDefaultValue = false, Name = "warnings", Order = 7)]
	public List<Warning> Warnings { get; set; }

	[DataMember(EmitDefaultValue = false, Name = "salesOrders", Order = 8)]
	public List<EDISalesOrderResponseItemDto> EDIOrderResponses { get; set; }

	public bool ShouldSerializeTotalOrders()
	{
		return TotalOrders > 0;
	}

	public bool ShouldSerializeOrdersCreated()
	{
		return TotalOrders > 0;
	}

	public bool ShouldSerializePayloadID()
	{
		return !string.IsNullOrWhiteSpace(PayloadID);
	}
}
