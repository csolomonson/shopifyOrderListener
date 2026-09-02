using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPCycleCodeRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a CycleCode with the specified Unique Id exists.
	/// </summary>
	/// <param name="cycleCodeId">The Unique Id of the CycleCode to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the CycleCode exists or not.</returns>
	Task<bool> DoesCycleCodeExist(Guid cycleCodeId);

	/// <summary>
	/// Retrieves all CycleCodes with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of CycleCodes to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of CycleCodes DTOs.</returns>
	Task<ICollection<ERPCycleCodeInformationDto>> GetAllCycleCodes(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific CycleCode.
	/// </summary>
	/// <param name="cycleCodeId">The Unique Id of the CycleCode to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the CycleCode DTO.</returns>
	Task<ERPCycleCodeInformationDto> GetCycleCode(Guid cycleCodeId);

	/// <summary>
	/// Saves the provided ERP cycleCode.
	/// </summary>
	/// <param name="cycleCode">The ERP cycleCode to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveCycleCode(ERPCycleCodeDto cycleCode);
}
