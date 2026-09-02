using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPServiceContractOwnerRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a ServiceContractOwner with the specified Unique Id exists.
	/// </summary>
	/// <param name="serviceContractOwnerId">The Unique Id of the ServiceContractOwner to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the ServiceContractOwner exists or not.</returns>
	Task<bool> DoesServiceContractOwnerExist(Guid serviceContractOwnerId);

	/// <summary>
	/// Retrieves all ServiceContractOwners with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ServiceContractOwners to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of ServiceContractOwners DTOs.</returns>
	Task<ICollection<ERPServiceContractOwnerInformationDto>> GetAllServiceContractOwners(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific ServiceContractOwner.
	/// </summary>
	/// <param name="serviceContractOwnerId">The Unique Id of the ServiceContractOwner to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the ServiceContractOwner DTO.</returns>
	Task<ERPServiceContractOwnerInformationDto> GetServiceContractOwner(Guid serviceContractOwnerId);

	/// <summary>
	/// Saves the provided ERP serviceContractOwner.
	/// </summary>
	/// <param name="serviceContractOwner">The ERP serviceContractOwner to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveServiceContractOwner(ERPServiceContractOwnerDto serviceContractOwner);
}
