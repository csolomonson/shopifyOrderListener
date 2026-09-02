using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPEmployeePersonalDatumRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a EmployeePersonalDatum with the specified Unique Id exists.
	/// </summary>
	/// <param name="employeePersonalDatumId">The Unique Id of the EmployeePersonalDatum to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the EmployeePersonalDatum exists or not.</returns>
	Task<bool> DoesEmployeePersonalDatumExist(Guid employeePersonalDatumId);

	/// <summary>
	/// Retrieves all EmployeePersonalData with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of EmployeePersonalData to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of EmployeePersonalData DTOs.</returns>
	Task<ICollection<ERPEmployeePersonalDatumInformationDto>> GetAllEmployeePersonalData(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific EmployeePersonalDatum.
	/// </summary>
	/// <param name="employeePersonalDatumId">The Unique Id of the EmployeePersonalDatum to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the EmployeePersonalDatum DTO.</returns>
	Task<ERPEmployeePersonalDatumInformationDto> GetEmployeePersonalDatum(Guid employeePersonalDatumId);

	/// <summary>
	/// Saves the provided ERP employeePersonalDatum.
	/// </summary>
	/// <param name="employeePersonalDatum">The ERP employeePersonalDatum to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveEmployeePersonalDatum(ERPEmployeePersonalDatumDto employeePersonalDatum);
}
