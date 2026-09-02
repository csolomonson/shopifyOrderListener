using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.EDI;

namespace M1.API.Models.EDI;

public interface IEDISalesOrderModel : IEDIBaseModel, IAPIBaseModel, IDisposable
{
	Task<APIValidationInfoDto> ValidateRequest_GetOrder(string m1SalesOrderId);

	Task<GetOrderResponseDto> Process_GetOrder(string m1SalesOrderId);

	Task<PostOrderResponseDto> ValidateRequest_PostOrder(IList<EDI850SalesOrderIN> salesOrders);

	Task<PostOrderResponseDto> Process_PostOrder(PostOrderResponseDto postOrderResponseIn);
}
