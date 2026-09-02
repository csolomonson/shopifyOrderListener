using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPSerialNumberStatusRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a SerialNumberStatus with the specified Unique Id exists.
	/// </summary>
	/// <param name="serialNumberStatusId">The Unique Id of the SerialNumberStatus to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the SerialNumberStatus exists or not.</returns>
	Task<bool> DoesSerialNumberStatusExist(Guid serialNumberStatusId);

	/// <summary>
	/// Retrieves all SerialNumberStatuses with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of SerialNumberStatuses to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of SerialNumberStatuses DTOs.</returns>
	Task<ICollection<ERPSerialNumberStatusInformationDto>> GetAllSerialNumberStatuses(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific SerialNumberStatus.
	/// </summary>
	/// <param name="serialNumberStatusId">The Unique Id of the SerialNumberStatus to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the SerialNumberStatus DTO.</returns>
	Task<ERPSerialNumberStatusInformationDto> GetSerialNumberStatus(Guid serialNumberStatusId);

	/// <summary>
	/// Saves the provided ERP serialNumberStatus.
	/// </summary>
	/// <param name="serialNumberStatus">The ERP serialNumberStatus to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveSerialNumberStatus(ERPSerialNumberStatusDto serialNumberStatus);
}
