using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPPunchCalculatorRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a PunchCalculator with the specified Unique Id exists.
	/// </summary>
	/// <param name="punchCalculatorId">The Unique Id of the PunchCalculator to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the PunchCalculator exists or not.</returns>
	Task<bool> DoesPunchCalculatorExist(Guid punchCalculatorId);

	/// <summary>
	/// Retrieves all PunchCalculators with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of PunchCalculators to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of PunchCalculators DTOs.</returns>
	Task<ICollection<ERPPunchCalculatorInformationDto>> GetAllPunchCalculators(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific PunchCalculator.
	/// </summary>
	/// <param name="punchCalculatorId">The Unique Id of the PunchCalculator to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the PunchCalculator DTO.</returns>
	Task<ERPPunchCalculatorInformationDto> GetPunchCalculator(Guid punchCalculatorId);

	/// <summary>
	/// Saves the provided ERP punchCalculator.
	/// </summary>
	/// <param name="punchCalculator">The ERP punchCalculator to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SavePunchCalculator(ERPPunchCalculatorDto punchCalculator);
}
