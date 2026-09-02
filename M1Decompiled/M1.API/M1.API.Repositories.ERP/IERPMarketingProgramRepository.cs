using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPMarketingProgramRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a MarketingProgram with the specified Unique Id exists.
	/// </summary>
	/// <param name="marketingProgramId">The Unique Id of the MarketingProgram to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the MarketingProgram exists or not.</returns>
	Task<bool> DoesMarketingProgramExist(Guid marketingProgramId);

	/// <summary>
	/// Retrieves all MarketingPrograms with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of MarketingPrograms to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of MarketingPrograms DTOs.</returns>
	Task<ICollection<ERPMarketingProgramInformationDto>> GetAllMarketingPrograms(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific MarketingProgram.
	/// </summary>
	/// <param name="marketingProgramId">The Unique Id of the MarketingProgram to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the MarketingProgram DTO.</returns>
	Task<ERPMarketingProgramInformationDto> GetMarketingProgram(Guid marketingProgramId);

	/// <summary>
	/// Saves the provided ERP marketingProgram.
	/// </summary>
	/// <param name="marketingProgram">The ERP marketingProgram to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveMarketingProgram(ERPMarketingProgramDto marketingProgram);
}
