using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPARPaymentHeaderRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a ARPaymentHeader with the specified Unique Id exists.
	/// </summary>
	/// <param name="aRPaymentHeaderId">The Unique Id of the ARPaymentHeader to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the ARPaymentHeader exists or not.</returns>
	Task<bool> DoesARPaymentHeaderExist(Guid aRPaymentHeaderId);

	/// <summary>
	/// Retrieves all ARPaymentHeaders with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ARPaymentHeaders to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of ARPaymentHeaders DTOs.</returns>
	Task<ICollection<ERPARPaymentHeaderInformationDto>> GetAllARPaymentHeaders(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific ARPaymentHeader.
	/// </summary>
	/// <param name="aRPaymentHeaderId">The Unique Id of the ARPaymentHeader to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the ARPaymentHeader DTO.</returns>
	Task<ERPARPaymentHeaderInformationDto> GetARPaymentHeader(Guid aRPaymentHeaderId);

	/// <summary>
	/// Saves the provided ERP aRPaymentHeader.
	/// </summary>
	/// <param name="aRPaymentHeader">The ERP aRPaymentHeader to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveARPaymentHeader(ERPARPaymentHeaderDto aRPaymentHeader);
}
