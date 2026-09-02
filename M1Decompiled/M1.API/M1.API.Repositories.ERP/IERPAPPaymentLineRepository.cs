using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPAPPaymentLineRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a APPaymentLine with the specified Unique Id exists.
	/// </summary>
	/// <param name="aPPaymentLineId">The Unique Id of the APPaymentLine to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the APPaymentLine exists or not.</returns>
	Task<bool> DoesAPPaymentLineExist(Guid aPPaymentLineId);

	/// <summary>
	/// Retrieves all APPaymentLines with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of APPaymentLines to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of APPaymentLines DTOs.</returns>
	Task<ICollection<ERPAPPaymentLineInformationDto>> GetAllAPPaymentLines(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific APPaymentLine.
	/// </summary>
	/// <param name="aPPaymentLineId">The Unique Id of the APPaymentLine to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the APPaymentLine DTO.</returns>
	Task<ERPAPPaymentLineInformationDto> GetAPPaymentLine(Guid aPPaymentLineId);

	/// <summary>
	/// Saves the provided ERP aPPaymentLine.
	/// </summary>
	/// <param name="aPPaymentLine">The ERP aPPaymentLine to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveAPPaymentLine(ERPAPPaymentLineDto aPPaymentLine);
}
