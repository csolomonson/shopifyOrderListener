using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPJobScenarioRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a JobScenario with the specified Unique Id exists.
	/// </summary>
	/// <param name="jobScenarioId">The Unique Id of the JobScenario to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the JobScenario exists or not.</returns>
	Task<bool> DoesJobScenarioExist(Guid jobScenarioId);

	/// <summary>
	/// Retrieves all JobScenarios with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of JobScenarios to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of JobScenarios DTOs.</returns>
	Task<ICollection<ERPJobScenarioInformationDto>> GetAllJobScenarios(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific JobScenario.
	/// </summary>
	/// <param name="jobScenarioId">The Unique Id of the JobScenario to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the JobScenario DTO.</returns>
	Task<ERPJobScenarioInformationDto> GetJobScenario(Guid jobScenarioId);

	/// <summary>
	/// Saves the provided ERP jobScenario.
	/// </summary>
	/// <param name="jobScenario">The ERP jobScenario to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveJobScenario(ERPJobScenarioDto jobScenario);
}
