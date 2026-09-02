using System;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPHealthModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	APIValidationInfoDto APIValidationIsTrueFunction();

	Task<ERPResponseMessageDto<string>> APIProceessIsTrueFunction();
}
