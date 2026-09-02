using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPARPaymentLineRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a ARPaymentLine with the specified Unique Id exists.
	/// </summary>
	/// <param name="aRPaymentLineId">The Unique Id of the ARPaymentLine to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the ARPaymentLine exists or not.</returns>
	Task<bool> DoesARPaymentLineExist(Guid aRPaymentLineId);

	/// <summary>
	/// Retrieves all ARPaymentLines with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ARPaymentLines to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of ARPaymentLines DTOs.</returns>
	Task<ICollection<ERPARPaymentLineInformationDto>> GetAllARPaymentLines(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific ARPaymentLine.
	/// </summary>
	/// <param name="aRPaymentLineId">The Unique Id of the ARPaymentLine to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the ARPaymentLine DTO.</returns>
	Task<ERPARPaymentLineInformationDto> GetARPaymentLine(Guid aRPaymentLineId);

	/// <summary>
	/// Saves the provided ERP aRPaymentLine.
	/// </summary>
	/// <param name="aRPaymentLine">The ERP aRPaymentLine to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveARPaymentLine(ERPARPaymentLineDto aRPaymentLine);
}
