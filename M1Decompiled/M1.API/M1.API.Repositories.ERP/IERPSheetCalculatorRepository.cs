using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPSheetCalculatorRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a SheetCalculator with the specified Unique Id exists.
	/// </summary>
	/// <param name="sheetCalculatorId">The Unique Id of the SheetCalculator to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the SheetCalculator exists or not.</returns>
	Task<bool> DoesSheetCalculatorExist(Guid sheetCalculatorId);

	/// <summary>
	/// Retrieves all SheetCalculators with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of SheetCalculators to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of SheetCalculators DTOs.</returns>
	Task<ICollection<ERPSheetCalculatorInformationDto>> GetAllSheetCalculators(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific SheetCalculator.
	/// </summary>
	/// <param name="sheetCalculatorId">The Unique Id of the SheetCalculator to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the SheetCalculator DTO.</returns>
	Task<ERPSheetCalculatorInformationDto> GetSheetCalculator(Guid sheetCalculatorId);

	/// <summary>
	/// Saves the provided ERP sheetCalculator.
	/// </summary>
	/// <param name="sheetCalculator">The ERP sheetCalculator to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveSheetCalculator(ERPSheetCalculatorDto sheetCalculator);
}
