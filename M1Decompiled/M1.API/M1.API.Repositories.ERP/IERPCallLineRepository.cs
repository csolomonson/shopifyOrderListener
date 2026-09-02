using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPCallLineRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a CallLine with the specified Unique Id exists.
	/// </summary>
	/// <param name="callLineId">The Unique Id of the CallLine to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the CallLine exists or not.</returns>
	Task<bool> DoesCallLineExist(Guid callLineId);

	/// <summary>
	/// Retrieves all CallLines with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of CallLines to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of CallLines DTOs.</returns>
	Task<ICollection<ERPCallLineInformationDto>> GetAllCallLines(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific CallLine.
	/// </summary>
	/// <param name="callLineId">The Unique Id of the CallLine to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the CallLine DTO.</returns>
	Task<ERPCallLineInformationDto> GetCallLine(Guid callLineId);

	/// <summary>
	/// Saves the provided ERP callLine.
	/// </summary>
	/// <param name="callLine">The ERP callLine to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveCallLine(ERPCallLineDto callLine);
}
