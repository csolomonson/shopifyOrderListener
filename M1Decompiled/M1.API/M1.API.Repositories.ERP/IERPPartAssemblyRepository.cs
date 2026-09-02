using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPPartAssemblyRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a PartAssembly with the specified Unique Id exists.
	/// </summary>
	/// <param name="partAssemblyId">The Unique Id of the PartAssembly to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the PartAssembly exists or not.</returns>
	Task<bool> DoesPartAssemblyExist(Guid partAssemblyId);

	/// <summary>
	/// Retrieves all PartAssemblies with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of PartAssemblies to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of PartAssemblies DTOs.</returns>
	Task<ICollection<ERPPartAssemblyInformationDto>> GetAllPartAssemblies(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific PartAssembly.
	/// </summary>
	/// <param name="partAssemblyId">The Unique Id of the PartAssembly to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the PartAssembly DTO.</returns>
	Task<ERPPartAssemblyInformationDto> GetPartAssembly(Guid partAssemblyId);

	/// <summary>
	/// Saves the provided ERP partAssembly.
	/// </summary>
	/// <param name="partAssembly">The ERP partAssembly to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SavePartAssembly(ERPPartAssemblyDto partAssembly);
}
