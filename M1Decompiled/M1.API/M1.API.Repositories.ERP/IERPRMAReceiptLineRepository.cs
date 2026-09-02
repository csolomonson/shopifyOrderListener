using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPRMAReceiptLineRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a RMAReceiptLine with the specified Unique Id exists.
	/// </summary>
	/// <param name="rMAReceiptLineId">The Unique Id of the RMAReceiptLine to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the RMAReceiptLine exists or not.</returns>
	Task<bool> DoesRMAReceiptLineExist(Guid rMAReceiptLineId);

	/// <summary>
	/// Retrieves all RMAReceiptLines with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of RMAReceiptLines to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of RMAReceiptLines DTOs.</returns>
	Task<ICollection<ERPRMAReceiptLineInformationDto>> GetAllRMAReceiptLines(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific RMAReceiptLine.
	/// </summary>
	/// <param name="rMAReceiptLineId">The Unique Id of the RMAReceiptLine to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the RMAReceiptLine DTO.</returns>
	Task<ERPRMAReceiptLineInformationDto> GetRMAReceiptLine(Guid rMAReceiptLineId);

	/// <summary>
	/// Saves the provided ERP rMAReceiptLine.
	/// </summary>
	/// <param name="rMAReceiptLine">The ERP rMAReceiptLine to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveRMAReceiptLine(ERPRMAReceiptLineDto rMAReceiptLine);
}
