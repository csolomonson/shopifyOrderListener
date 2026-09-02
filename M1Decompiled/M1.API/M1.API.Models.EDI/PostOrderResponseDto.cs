using System.Collections.Generic;
using System.Linq;
using M1.API.DTOs.Core;
using M1.API.DTOs.EDI;

namespace M1.API.Models.EDI;

public class PostOrderResponseDto
{
	public bool IsValidationOk
	{
		get
		{
			List<string> obj = M1OrderCollection?.SelectMany((CTMSalesOrderDto x) => x.M1SalesOrderValidatationInfo?.ErrorsList)?.ToList();
			bool flag = GeneralValidatationInfo?.IsValidationOk ?? true;
			return obj != null && obj.Count == 0 && flag;
		}
	}

	public IList<CTMSalesOrderDto> M1OrderCollection { get; set; }

	public APIValidationInfoDto GeneralValidatationInfo { get; set; }

	public PostOrderResponseDto()
	{
		M1OrderCollection = new List<CTMSalesOrderDto>();
		GeneralValidatationInfo = new APIValidationInfoDto();
	}
}
