using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPIndustryTypeRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a IndustryType with the specified Unique Id exists.
	/// </summary>
	/// <param name="industryTypeId">The Unique Id of the IndustryType to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the IndustryType exists or not.</returns>
	Task<bool> DoesIndustryTypeExist(Guid industryTypeId);

	/// <summary>
	/// Retrieves all IndustryTypes with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of IndustryTypes to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of IndustryTypes DTOs.</returns>
	Task<ICollection<ERPIndustryTypeInformationDto>> GetAllIndustryTypes(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific IndustryType.
	/// </summary>
	/// <param name="industryTypeId">The Unique Id of the IndustryType to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the IndustryType DTO.</returns>
	Task<ERPIndustryTypeInformationDto> GetIndustryType(Guid industryTypeId);

	/// <summary>
	/// Saves the provided ERP industryType.
	/// </summary>
	/// <param name="industryType">The ERP industryType to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveIndustryType(ERPIndustryTypeDto industryType);
}
