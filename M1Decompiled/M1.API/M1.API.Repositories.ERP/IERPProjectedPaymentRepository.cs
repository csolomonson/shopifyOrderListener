using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPProjectedPaymentRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a ProjectedPayment with the specified Unique Id exists.
	/// </summary>
	/// <param name="projectedPaymentId">The Unique Id of the ProjectedPayment to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the ProjectedPayment exists or not.</returns>
	Task<bool> DoesProjectedPaymentExist(Guid projectedPaymentId);

	/// <summary>
	/// Retrieves all ProjectedPayments with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ProjectedPayments to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of ProjectedPayments DTOs.</returns>
	Task<ICollection<ERPProjectedPaymentInformationDto>> GetAllProjectedPayments(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific ProjectedPayment.
	/// </summary>
	/// <param name="projectedPaymentId">The Unique Id of the ProjectedPayment to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the ProjectedPayment DTO.</returns>
	Task<ERPProjectedPaymentInformationDto> GetProjectedPayment(Guid projectedPaymentId);

	/// <summary>
	/// Saves the provided ERP projectedPayment.
	/// </summary>
	/// <param name="projectedPayment">The ERP projectedPayment to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveProjectedPayment(ERPProjectedPaymentDto projectedPayment);
}
