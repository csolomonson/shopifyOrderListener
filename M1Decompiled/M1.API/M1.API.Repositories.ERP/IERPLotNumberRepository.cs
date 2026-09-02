using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPLotNumberRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a LotNumber with the specified Unique Id exists.
	/// </summary>
	/// <param name="lotNumberId">The Unique Id of the LotNumber to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the LotNumber exists or not.</returns>
	Task<bool> DoesLotNumberExist(Guid lotNumberId);

	/// <summary>
	/// Retrieves all LotNumbers with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of LotNumbers to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of LotNumbers DTOs.</returns>
	Task<ICollection<ERPLotNumberInformationDto>> GetAllLotNumbers(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific LotNumber.
	/// </summary>
	/// <param name="lotNumberId">The Unique Id of the LotNumber to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the LotNumber DTO.</returns>
	Task<ERPLotNumberInformationDto> GetLotNumber(Guid lotNumberId);

	/// <summary>
	/// Saves the provided ERP lotNumber.
	/// </summary>
	/// <param name="lotNumber">The ERP lotNumber to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveLotNumber(ERPLotNumberDto lotNumber);
}
