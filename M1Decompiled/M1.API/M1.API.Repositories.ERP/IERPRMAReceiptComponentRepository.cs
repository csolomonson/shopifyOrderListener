using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPRMAReceiptComponentRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a RMAReceiptComponent with the specified Unique Id exists.
	/// </summary>
	/// <param name="rMAReceiptComponentId">The Unique Id of the RMAReceiptComponent to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the RMAReceiptComponent exists or not.</returns>
	Task<bool> DoesRMAReceiptComponentExist(Guid rMAReceiptComponentId);

	/// <summary>
	/// Retrieves all RMAReceiptComponents with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of RMAReceiptComponents to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of RMAReceiptComponents DTOs.</returns>
	Task<ICollection<ERPRMAReceiptComponentInformationDto>> GetAllRMAReceiptComponents(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific RMAReceiptComponent.
	/// </summary>
	/// <param name="rMAReceiptComponentId">The Unique Id of the RMAReceiptComponent to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the RMAReceiptComponent DTO.</returns>
	Task<ERPRMAReceiptComponentInformationDto> GetRMAReceiptComponent(Guid rMAReceiptComponentId);

	/// <summary>
	/// Saves the provided ERP rMAReceiptComponent.
	/// </summary>
	/// <param name="rMAReceiptComponent">The ERP rMAReceiptComponent to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveRMAReceiptComponent(ERPRMAReceiptComponentDto rMAReceiptComponent);
}
