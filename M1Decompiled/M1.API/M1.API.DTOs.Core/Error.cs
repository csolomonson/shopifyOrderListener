using System;
using System.Runtime.Serialization;

namespace M1.API.DTOs.Core;

[Serializable]
[DataContract(Namespace = "")]
public class Error
{
	[DataMember(Name = "innerText")]
	public string ResponseItem { get; set; }
}
