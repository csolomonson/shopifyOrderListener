using System.Collections.Generic;
using System.Net;
using M1.API.Controllers;

namespace M1.API.DTOs.Core;

public class APIValidationInfoDto
{
	public bool IsValidationOk => ErrorsList.Count == 0;

	public ResponseMessageBuilderFunctions.ResponseContentHeaderStatus APIValidationStatusCode
	{
		get
		{
			List<string> errorsList = ErrorsList;
			if (errorsList != null && errorsList.Count > 0)
			{
				List<string> warningsList = WarningsList;
				if (warningsList != null && warningsList.Count > 0)
				{
					return ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.ErrorsAndWarnings;
				}
			}
			List<string> errorsList2 = ErrorsList;
			if (errorsList2 != null && errorsList2.Count > 0)
			{
				return ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Error;
			}
			List<string> warningsList2 = WarningsList;
			if (warningsList2 != null && warningsList2.Count > 0)
			{
				List<string> errorsList3 = ErrorsList;
				if (errorsList3 != null && errorsList3.Count == 0)
				{
					return ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.SuccessWithWarnings;
				}
			}
			return ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success;
		}
		private set
		{
		}
	}

	public List<string> ErrorsList { get; } = new List<string>();

	public List<string> WarningsList { get; } = new List<string>();

	public HttpStatusCode HttpValidationStatusCode { get; set; }

	public APIValidationInfoDto(IList<string> errorsList, IList<string> warningsList, HttpStatusCode httpValidationStatusCode)
	{
		ErrorsList = new List<string>(errorsList ?? new List<string>());
		WarningsList = new List<string>(warningsList ?? new List<string>());
		HttpValidationStatusCode = httpValidationStatusCode;
	}

	public APIValidationInfoDto(IList<string> errorsList, IList<string> warningsList)
	{
		ErrorsList = new List<string>(errorsList ?? new List<string>());
		WarningsList = new List<string>(warningsList ?? new List<string>());
	}

	public APIValidationInfoDto()
	{
		ErrorsList = new List<string>();
		WarningsList = new List<string>();
	}
}
