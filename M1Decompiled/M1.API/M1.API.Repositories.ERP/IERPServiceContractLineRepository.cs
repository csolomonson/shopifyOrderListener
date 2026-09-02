using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPServiceContractLineRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a ServiceContractLine with the specified Unique Id exists.
	/// </summary>
	/// <param name="serviceContractLineId">The Unique Id of the ServiceContractLine to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the ServiceContractLine exists or not.</returns>
	Task<bool> DoesServiceContractLineExist(Guid serviceContractLineId);

	/// <summary>
	/// Retrieves all ServiceContractLines with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ServiceContractLines to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of ServiceContractLines DTOs.</returns>
	Task<ICollection<ERPServiceContractLineInformationDto>> GetAllServiceContractLines(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific ServiceContractLine.
	/// </summary>
	/// <param name="serviceContractLineId">The Unique Id of the ServiceContractLine to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the ServiceContractLine DTO.</returns>
	Task<ERPServiceContractLineInformationDto> GetServiceContractLine(Guid serviceContractLineId);

	/// <summary>
	/// Saves the provided ERP serviceContractLine.
	/// </summary>
	/// <param name="serviceContractLine">The ERP serviceContractLine to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveServiceContractLine(ERPServiceContractLineDto serviceContractLine);
}
