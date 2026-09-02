using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPJobAssemblyRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a JobAssembly with the specified Unique Id exists.
	/// </summary>
	/// <param name="jobAssemblyId">The Unique Id of the JobAssembly to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the JobAssembly exists or not.</returns>
	Task<bool> DoesJobAssemblyExist(Guid jobAssemblyId);

	/// <summary>
	/// Retrieves all JobAssemblies with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of JobAssemblies to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of JobAssemblies DTOs.</returns>
	Task<ICollection<ERPJobAssemblyInformationDto>> GetAllJobAssemblies(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific JobAssembly.
	/// </summary>
	/// <param name="jobAssemblyId">The Unique Id of the JobAssembly to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the JobAssembly DTO.</returns>
	Task<ERPJobAssemblyInformationDto> GetJobAssembly(Guid jobAssemblyId);

	/// <summary>
	/// Saves the provided ERP jobAssembly.
	/// </summary>
	/// <param name="jobAssembly">The ERP jobAssembly to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveJobAssembly(ERPJobAssemblyDto jobAssembly);
}
