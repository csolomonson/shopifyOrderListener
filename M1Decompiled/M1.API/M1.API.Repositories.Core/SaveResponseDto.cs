using System.Collections.Generic;

namespace M1.API.Repositories.Core;

public class SaveResponseDto
{
	public bool IsSuccess { get; set; }

	public IList<string> SavingErrors { get; set; } = new List<string>();

	public string SalesOrder { get; set; }

	public SaveResponseDto(bool isSuccess, string salesOrder, IList<string> savingErrors)
	{
		IsSuccess = isSuccess;
		SalesOrder = salesOrder;
		SavingErrors = new List<string>(savingErrors);
	}

	public SaveResponseDto(bool isSuccess, string salesOrder, string savingError)
	{
		IsSuccess = isSuccess;
		SalesOrder = salesOrder;
		SavingErrors.Add(savingError);
	}
}
