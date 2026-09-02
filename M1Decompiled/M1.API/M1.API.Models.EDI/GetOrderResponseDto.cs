using M1.API.DTOs.Core;

namespace M1.API.Models.EDI;

public class GetOrderResponseDto
{
	public APIValidationInfoDto ValidationInfo { get; set; }

	public SalesOrderDto SalesOrder { get; set; }
}
