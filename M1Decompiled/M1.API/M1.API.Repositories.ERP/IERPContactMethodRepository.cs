using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPContactMethodRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a ContactMethod with the specified Unique Id exists.
	/// </summary>
	/// <param name="contactMethodId">The Unique Id of the ContactMethod to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the ContactMethod exists or not.</returns>
	Task<bool> DoesContactMethodExist(Guid contactMethodId);

	/// <summary>
	/// Retrieves all ContactMethods with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ContactMethods to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of ContactMethods DTOs.</returns>
	Task<ICollection<ERPContactMethodInformationDto>> GetAllContactMethods(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific ContactMethod.
	/// </summary>
	/// <param name="contactMethodId">The Unique Id of the ContactMethod to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the ContactMethod DTO.</returns>
	Task<ERPContactMethodInformationDto> GetContactMethod(Guid contactMethodId);

	/// <summary>
	/// Saves the provided ERP contactMethod.
	/// </summary>
	/// <param name="contactMethod">The ERP contactMethod to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveContactMethod(ERPContactMethodDto contactMethod);
}
