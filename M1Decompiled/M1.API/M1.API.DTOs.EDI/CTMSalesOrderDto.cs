using M1.API.DTOs.Core;

namespace M1.API.DTOs.EDI;

public class CTMSalesOrderDto
{
	public SalesOrderDto M1SalesOrder { get; set; }

	public APIValidationInfoDto M1SalesOrderValidatationInfo { get; set; }

	public bool DoesOrderCreated { get; set; }

	public bool DoesRequestProcessed { get; set; }

	public bool DoesRequestValidated { get; set; }

	public string EDIOrderID { get; set; }

	public string EDIPurpose { get; set; }

	public string CurrentM1SalesorderIDs { get; set; } = string.Empty;

	public CTMSalesOrderDto()
	{
		M1SalesOrder = new SalesOrderDto();
		M1SalesOrderValidatationInfo = new APIValidationInfoDto();
	}
}
