using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPRFQLineRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a RFQLine with the specified Unique Id exists.
	/// </summary>
	/// <param name="rFQLineId">The Unique Id of the RFQLine to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the RFQLine exists or not.</returns>
	Task<bool> DoesRFQLineExist(Guid rFQLineId);

	/// <summary>
	/// Retrieves all RFQLines with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of RFQLines to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of RFQLines DTOs.</returns>
	Task<ICollection<ERPRFQLineInformationDto>> GetAllRFQLines(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific RFQLine.
	/// </summary>
	/// <param name="rFQLineId">The Unique Id of the RFQLine to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the RFQLine DTO.</returns>
	Task<ERPRFQLineInformationDto> GetRFQLine(Guid rFQLineId);

	/// <summary>
	/// Saves the provided ERP rFQLine.
	/// </summary>
	/// <param name="rFQLine">The ERP rFQLine to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveRFQLine(ERPRFQLineDto rFQLine);
}
