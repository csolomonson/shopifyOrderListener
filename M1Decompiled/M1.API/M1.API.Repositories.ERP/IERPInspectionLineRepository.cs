using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPInspectionLineRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a InspectionLine with the specified Unique Id exists.
	/// </summary>
	/// <param name="inspectionLineId">The Unique Id of the InspectionLine to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the InspectionLine exists or not.</returns>
	Task<bool> DoesInspectionLineExist(Guid inspectionLineId);

	/// <summary>
	/// Retrieves all InspectionLines with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of InspectionLines to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of InspectionLines DTOs.</returns>
	Task<ICollection<ERPInspectionLineInformationDto>> GetAllInspectionLines(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific InspectionLine.
	/// </summary>
	/// <param name="inspectionLineId">The Unique Id of the InspectionLine to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the InspectionLine DTO.</returns>
	Task<ERPInspectionLineInformationDto> GetInspectionLine(Guid inspectionLineId);

	/// <summary>
	/// Saves the provided ERP inspectionLine.
	/// </summary>
	/// <param name="inspectionLine">The ERP inspectionLine to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveInspectionLine(ERPInspectionLineDto inspectionLine);
}
