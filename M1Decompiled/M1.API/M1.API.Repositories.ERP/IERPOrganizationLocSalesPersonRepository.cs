using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPOrganizationLocSalesPersonRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a OrganizationLocSalesPerson with the specified Unique Id exists.
	/// </summary>
	/// <param name="organizationLocSalesPersonId">The Unique Id of the OrganizationLocSalesPerson to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the OrganizationLocSalesPerson exists or not.</returns>
	Task<bool> DoesOrganizationLocSalesPersonExist(Guid organizationLocSalesPersonId);

	/// <summary>
	/// Retrieves all OrganizationLocSalesPeople with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of OrganizationLocSalesPeople to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of OrganizationLocSalesPeople DTOs.</returns>
	Task<ICollection<ERPOrganizationLocSalesPersonInformationDto>> GetAllOrganizationLocSalesPeople(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific OrganizationLocSalesPerson.
	/// </summary>
	/// <param name="organizationLocSalesPersonId">The Unique Id of the OrganizationLocSalesPerson to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the OrganizationLocSalesPerson DTO.</returns>
	Task<ERPOrganizationLocSalesPersonInformationDto> GetOrganizationLocSalesPerson(Guid organizationLocSalesPersonId);

	/// <summary>
	/// Saves the provided ERP organizationLocSalesPerson.
	/// </summary>
	/// <param name="organizationLocSalesPerson">The ERP organizationLocSalesPerson to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveOrganizationLocSalesPerson(ERPOrganizationLocSalesPersonDto organizationLocSalesPerson);
}
