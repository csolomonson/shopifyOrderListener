using System;
using System.Runtime.Serialization;
using M1.API.DTOs.Core;

namespace M1.API.DTOs.BOM;

[Serializable]
[DataContract(Namespace = "", Name = "responseInfo")]
public class BOMPartRevisionMessageResponseDto
{
	public APIValidationInfoDto ValidationInfo { get; set; }

	public CTMBOMPartRevisionDto PartRevisionDto { get; set; }
}
