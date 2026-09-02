using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPInspectionLineApprovalRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a InspectionLineApproval with the specified Unique Id exists.
	/// </summary>
	/// <param name="inspectionLineApprovalId">The Unique Id of the InspectionLineApproval to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the InspectionLineApproval exists or not.</returns>
	Task<bool> DoesInspectionLineApprovalExist(Guid inspectionLineApprovalId);

	/// <summary>
	/// Retrieves all InspectionLineApprovals with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of InspectionLineApprovals to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of InspectionLineApprovals DTOs.</returns>
	Task<ICollection<ERPInspectionLineApprovalInformationDto>> GetAllInspectionLineApprovals(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific InspectionLineApproval.
	/// </summary>
	/// <param name="inspectionLineApprovalId">The Unique Id of the InspectionLineApproval to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the InspectionLineApproval DTO.</returns>
	Task<ERPInspectionLineApprovalInformationDto> GetInspectionLineApproval(Guid inspectionLineApprovalId);

	/// <summary>
	/// Saves the provided ERP inspectionLineApproval.
	/// </summary>
	/// <param name="inspectionLineApproval">The ERP inspectionLineApproval to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveInspectionLineApproval(ERPInspectionLineApprovalDto inspectionLineApproval);
}
