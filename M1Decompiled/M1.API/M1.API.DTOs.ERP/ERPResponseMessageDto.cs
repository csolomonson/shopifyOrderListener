using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using M1.API.DTOs.Core;
using Newtonsoft.Json;

namespace M1.API.DTOs.ERP;

[Serializable]
[DataContract(Namespace = "", Name = "responseSet")]
[XmlType(AnonymousType = true)]
public class ERPResponseMessageDto<T>
{
	[DataMember(Name = "responseInfo", Order = 1, EmitDefaultValue = true)]
	public APIResponseMessageDto APIResponseMessage { get; set; }

	[DataMember(Name = "returnObject", Order = 2)]
	public T ReturnObject { get; set; }

	[DataMember(Name = "recordCount", Order = 3, EmitDefaultValue = false)]
	public int? RecordCount { get; set; }

	[XmlIgnore]
	[JsonIgnore]
	public APIValidationInfoDto ValidationInfo { get; set; }

	public bool ShouldSerializeAPIResponseMessage()
	{
		return APIResponseMessage != null;
	}

	public bool? ShouldSerializeReturnObject()
	{
		T returnObject = ReturnObject;
		if (returnObject == null)
		{
			return null;
		}
		return !returnObject.Equals(default(T));
	}
}
