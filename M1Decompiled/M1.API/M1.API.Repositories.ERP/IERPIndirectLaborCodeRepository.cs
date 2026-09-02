using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPIndirectLaborCodeRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a IndirectLaborCode with the specified Unique Id exists.
	/// </summary>
	/// <param name="indirectLaborCodeId">The Unique Id of the IndirectLaborCode to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the IndirectLaborCode exists or not.</returns>
	Task<bool> DoesIndirectLaborCodeExist(Guid indirectLaborCodeId);

	/// <summary>
	/// Retrieves all IndirectLaborCodes with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of IndirectLaborCodes to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of IndirectLaborCodes DTOs.</returns>
	Task<ICollection<ERPIndirectLaborCodeInformationDto>> GetAllIndirectLaborCodes(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific IndirectLaborCode.
	/// </summary>
	/// <param name="indirectLaborCodeId">The Unique Id of the IndirectLaborCode to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the IndirectLaborCode DTO.</returns>
	Task<ERPIndirectLaborCodeInformationDto> GetIndirectLaborCode(Guid indirectLaborCodeId);

	/// <summary>
	/// Saves the provided ERP indirectLaborCode.
	/// </summary>
	/// <param name="indirectLaborCode">The ERP indirectLaborCode to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveIndirectLaborCode(ERPIndirectLaborCodeDto indirectLaborCode);
}
