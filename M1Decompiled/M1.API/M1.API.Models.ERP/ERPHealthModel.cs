using System;
using System.Net;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public class ERPHealthModel : ERPBaseModel, IERPHealthModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public APIValidationInfoDto APIValidationIsTrueFunction()
	{
		return new APIValidationInfoDto();
	}

	public async Task<ERPResponseMessageDto<string>> APIProceessIsTrueFunction()
	{
		new ERPResponseMessageDto<string>();
		APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, HttpStatusCode.OK);
		return new ERPResponseMessageDto<string>
		{
			ValidationInfo = validationInfo,
			ReturnObject = "Connection Successful"
		};
	}
}
