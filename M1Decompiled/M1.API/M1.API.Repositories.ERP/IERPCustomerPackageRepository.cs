using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPCustomerPackageRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a CustomerPackage with the specified Unique Id exists.
	/// </summary>
	/// <param name="customerPackageId">The Unique Id of the CustomerPackage to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the CustomerPackage exists or not.</returns>
	Task<bool> DoesCustomerPackageExist(Guid customerPackageId);

	/// <summary>
	/// Retrieves all CustomerPackages with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of CustomerPackages to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of CustomerPackages DTOs.</returns>
	Task<ICollection<ERPCustomerPackageInformationDto>> GetAllCustomerPackages(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific CustomerPackage.
	/// </summary>
	/// <param name="customerPackageId">The Unique Id of the CustomerPackage to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the CustomerPackage DTO.</returns>
	Task<ERPCustomerPackageInformationDto> GetCustomerPackage(Guid customerPackageId);

	/// <summary>
	/// Saves the provided ERP customerPackage.
	/// </summary>
	/// <param name="customerPackage">The ERP customerPackage to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveCustomerPackage(ERPCustomerPackageDto customerPackage);
}
