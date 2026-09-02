using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPServiceContractRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a ServiceContract with the specified Unique Id exists.
	/// </summary>
	/// <param name="serviceContractId">The Unique Id of the ServiceContract to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the ServiceContract exists or not.</returns>
	Task<bool> DoesServiceContractExist(Guid serviceContractId);

	/// <summary>
	/// Retrieves all ServiceContracts with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ServiceContracts to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of ServiceContracts DTOs.</returns>
	Task<ICollection<ERPServiceContractInformationDto>> GetAllServiceContracts(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific ServiceContract.
	/// </summary>
	/// <param name="serviceContractId">The Unique Id of the ServiceContract to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the ServiceContract DTO.</returns>
	Task<ERPServiceContractInformationDto> GetServiceContract(Guid serviceContractId);

	/// <summary>
	/// Saves the provided ERP serviceContract.
	/// </summary>
	/// <param name="serviceContract">The ERP serviceContract to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveServiceContract(ERPServiceContractDto serviceContract);
}
