using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPStandardMessageRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a StandardMessage with the specified Unique Id exists.
	/// </summary>
	/// <param name="standardMessageId">The Unique Id of the StandardMessage to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the StandardMessage exists or not.</returns>
	Task<bool> DoesStandardMessageExist(Guid standardMessageId);

	/// <summary>
	/// Retrieves all StandardMessages with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of StandardMessages to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of StandardMessages DTOs.</returns>
	Task<ICollection<ERPStandardMessageInformationDto>> GetAllStandardMessages(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific StandardMessage.
	/// </summary>
	/// <param name="standardMessageId">The Unique Id of the StandardMessage to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the StandardMessage DTO.</returns>
	Task<ERPStandardMessageInformationDto> GetStandardMessage(Guid standardMessageId);

	/// <summary>
	/// Saves the provided ERP standardMessage.
	/// </summary>
	/// <param name="standardMessage">The ERP standardMessage to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveStandardMessage(ERPStandardMessageDto standardMessage);
}
