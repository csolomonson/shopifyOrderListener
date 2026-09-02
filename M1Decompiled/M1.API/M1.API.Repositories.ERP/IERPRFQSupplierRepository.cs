using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPRFQSupplierRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a RFQSupplier with the specified Unique Id exists.
	/// </summary>
	/// <param name="rFQSupplierId">The Unique Id of the RFQSupplier to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the RFQSupplier exists or not.</returns>
	Task<bool> DoesRFQSupplierExist(Guid rFQSupplierId);

	/// <summary>
	/// Retrieves all RFQSuppliers with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of RFQSuppliers to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of RFQSuppliers DTOs.</returns>
	Task<ICollection<ERPRFQSupplierInformationDto>> GetAllRFQSuppliers(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific RFQSupplier.
	/// </summary>
	/// <param name="rFQSupplierId">The Unique Id of the RFQSupplier to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the RFQSupplier DTO.</returns>
	Task<ERPRFQSupplierInformationDto> GetRFQSupplier(Guid rFQSupplierId);

	/// <summary>
	/// Saves the provided ERP rFQSupplier.
	/// </summary>
	/// <param name="rFQSupplier">The ERP rFQSupplier to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveRFQSupplier(ERPRFQSupplierDto rFQSupplier);
}
