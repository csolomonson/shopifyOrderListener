using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPRFQRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a RFQ with the specified Unique Id exists.
	/// </summary>
	/// <param name="rFQId">The Unique Id of the RFQ to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the RFQ exists or not.</returns>
	Task<bool> DoesRFQExist(Guid rFQId);

	/// <summary>
	/// Retrieves all RFQs with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of RFQs to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of RFQs DTOs.</returns>
	Task<ICollection<ERPRFQInformationDto>> GetAllRFQs(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific RFQ.
	/// </summary>
	/// <param name="rFQId">The Unique Id of the RFQ to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the RFQ DTO.</returns>
	Task<ERPRFQInformationDto> GetRFQ(Guid rFQId);

	/// <summary>
	/// Saves the provided ERP rFQ.
	/// </summary>
	/// <param name="rFQ">The ERP rFQ to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveRFQ(ERPRFQDto rFQ);
}
