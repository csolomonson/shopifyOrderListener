using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPCorrectiveActionCodeRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a CorrectiveActionCode with the specified Unique Id exists.
	/// </summary>
	/// <param name="correctiveActionCodeId">The Unique Id of the CorrectiveActionCode to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the CorrectiveActionCode exists or not.</returns>
	Task<bool> DoesCorrectiveActionCodeExist(Guid correctiveActionCodeId);

	/// <summary>
	/// Retrieves all CorrectiveActionCodes with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of CorrectiveActionCodes to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of CorrectiveActionCodes DTOs.</returns>
	Task<ICollection<ERPCorrectiveActionCodeInformationDto>> GetAllCorrectiveActionCodes(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific CorrectiveActionCode.
	/// </summary>
	/// <param name="correctiveActionCodeId">The Unique Id of the CorrectiveActionCode to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the CorrectiveActionCode DTO.</returns>
	Task<ERPCorrectiveActionCodeInformationDto> GetCorrectiveActionCode(Guid correctiveActionCodeId);

	/// <summary>
	/// Saves the provided ERP correctiveActionCode.
	/// </summary>
	/// <param name="correctiveActionCode">The ERP correctiveActionCode to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveCorrectiveActionCode(ERPCorrectiveActionCodeDto correctiveActionCode);
}
