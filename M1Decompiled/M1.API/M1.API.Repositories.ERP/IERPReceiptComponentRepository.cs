using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPReceiptComponentRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a ReceiptComponent with the specified Unique Id exists.
	/// </summary>
	/// <param name="receiptComponentId">The Unique Id of the ReceiptComponent to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the ReceiptComponent exists or not.</returns>
	Task<bool> DoesReceiptComponentExist(Guid receiptComponentId);

	/// <summary>
	/// Retrieves all ReceiptComponents with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ReceiptComponents to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of ReceiptComponents DTOs.</returns>
	Task<ICollection<ERPReceiptComponentInformationDto>> GetAllReceiptComponents(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific ReceiptComponent.
	/// </summary>
	/// <param name="receiptComponentId">The Unique Id of the ReceiptComponent to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the ReceiptComponent DTO.</returns>
	Task<ERPReceiptComponentInformationDto> GetReceiptComponent(Guid receiptComponentId);

	/// <summary>
	/// Saves the provided ERP receiptComponent.
	/// </summary>
	/// <param name="receiptComponent">The ERP receiptComponent to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveReceiptComponent(ERPReceiptComponentDto receiptComponent);
}
