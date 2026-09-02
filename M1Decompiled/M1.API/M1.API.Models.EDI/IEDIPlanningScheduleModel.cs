using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.EDI;

namespace M1.API.Models.EDI;

public interface IEDIPlanningScheduleModel : IEDIBaseModel, IAPIBaseModel, IDisposable
{
	Task<PostOrderResponseDto> ValidateRequest_PostSchedule(List<EDI830ScheduleIN> salesOrders);

	Task<PostOrderResponseDto> Process_PostSchedule(PostOrderResponseDto postOrderResponseIn);
}
