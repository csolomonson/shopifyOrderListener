using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPMRPSupplyRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a MRPSupply with the specified Unique Id exists.
	/// </summary>
	/// <param name="mRPSupplyId">The Unique Id of the MRPSupply to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the MRPSupply exists or not.</returns>
	Task<bool> DoesMRPSupplyExist(Guid mRPSupplyId);

	/// <summary>
	/// Retrieves all MRPSupply with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of MRPSupply to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of MRPSupply DTOs.</returns>
	Task<ICollection<ERPMRPSupplyInformationDto>> GetAllMRPSupply(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific MRPSupply.
	/// </summary>
	/// <param name="mRPSupplyId">The Unique Id of the MRPSupply to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the MRPSupply DTO.</returns>
	Task<ERPMRPSupplyInformationDto> GetMRPSupply(Guid mRPSupplyId);

	/// <summary>
	/// Saves the provided ERP mRPSupply.
	/// </summary>
	/// <param name="mRPSupply">The ERP mRPSupply to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveMRPSupply(ERPMRPSupplyDto mRPSupply);
}
