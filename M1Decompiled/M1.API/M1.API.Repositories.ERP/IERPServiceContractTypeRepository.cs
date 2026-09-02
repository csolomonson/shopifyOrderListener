using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPServiceContractTypeRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a ServiceContractType with the specified Unique Id exists.
	/// </summary>
	/// <param name="serviceContractTypeId">The Unique Id of the ServiceContractType to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the ServiceContractType exists or not.</returns>
	Task<bool> DoesServiceContractTypeExist(Guid serviceContractTypeId);

	/// <summary>
	/// Retrieves all ServiceContractTypes with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ServiceContractTypes to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of ServiceContractTypes DTOs.</returns>
	Task<ICollection<ERPServiceContractTypeInformationDto>> GetAllServiceContractTypes(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific ServiceContractType.
	/// </summary>
	/// <param name="serviceContractTypeId">The Unique Id of the ServiceContractType to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the ServiceContractType DTO.</returns>
	Task<ERPServiceContractTypeInformationDto> GetServiceContractType(Guid serviceContractTypeId);
}
