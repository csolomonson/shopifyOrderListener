using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPLotNumberStatusRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a LotNumberStatus with the specified Unique Id exists.
	/// </summary>
	/// <param name="lotNumberStatusId">The Unique Id of the LotNumberStatus to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the LotNumberStatus exists or not.</returns>
	Task<bool> DoesLotNumberStatusExist(Guid lotNumberStatusId);

	/// <summary>
	/// Retrieves all LotNumberStatuses with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of LotNumberStatuses to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of LotNumberStatuses DTOs.</returns>
	Task<ICollection<ERPLotNumberStatusInformationDto>> GetAllLotNumberStatuses(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific LotNumberStatus.
	/// </summary>
	/// <param name="lotNumberStatusId">The Unique Id of the LotNumberStatus to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the LotNumberStatus DTO.</returns>
	Task<ERPLotNumberStatusInformationDto> GetLotNumberStatus(Guid lotNumberStatusId);

	/// <summary>
	/// Saves the provided ERP lotNumberStatus.
	/// </summary>
	/// <param name="lotNumberStatus">The ERP lotNumberStatus to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveLotNumberStatus(ERPLotNumberStatusDto lotNumberStatus);
}
