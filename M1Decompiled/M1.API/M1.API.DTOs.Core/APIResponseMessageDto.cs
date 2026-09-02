using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace M1.API.DTOs.Core;

[Serializable]
[DataContract(Namespace = "", Name = "responseSet")]
public class APIResponseMessageDto
{
	[XmlIgnore]
	[JsonIgnore]
	public int ResponseID { get; set; }

	[DataMember(Name = "status", Order = 1)]
	public string Status { get; set; }

	[DataMember(Name = "payloadID", Order = 2)]
	public string PayloadID { get; set; }

	[DataMember(Name = "description", Order = 3)]
	[JsonProperty(PropertyName = "description")]
	public string Description { get; set; } = string.Empty;

	[DataMember(EmitDefaultValue = false, Name = "errors", Order = 4)]
	public List<Error> Errors { get; } = new List<Error>();

	[DataMember(EmitDefaultValue = false, Name = "warnings", Order = 5)]
	public List<Warning> Warnings { get; } = new List<Warning>();

	[XmlIgnore]
	[JsonIgnore]
	public HttpStatusCode HttpErrorStatusCode { get; set; } = HttpStatusCode.OK;
}
