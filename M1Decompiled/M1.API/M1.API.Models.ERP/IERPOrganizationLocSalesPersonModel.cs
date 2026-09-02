using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPOrganizationLocSalesPersonModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all OrganizationLocSalesPeople with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of OrganizationLocSalesPeople to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllOrganizationLocSalesPeople(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving OrganizationLocSalesPerson information based on the specified OrganizationLocSalesPerson Unique Id.
	/// </summary>
	/// <param name="organizationLocSalesPersonId">The Unique Id of the OrganizationLocSalesPerson.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetOrganizationLocSalesPerson(Guid organizationLocSalesPersonId);

	/// <summary>
	/// Validates the PUT request for creating or updating OrganizationLocSalesPerson information based on the specified OrganizationLocSalesPerson.
	/// </summary>
	/// <param name="organizationLocSalesPerson">The OrganizationLocSalesPerson details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutOrganizationLocSalesPerson(ERPOrganizationLocSalesPersonDto organizationLocSalesPerson);

	/// <summary>
	/// Processes the request to retrieve all OrganizationLocSalesPeople with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of OrganizationLocSalesPeople to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of OrganizationLocSalesPeople DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPOrganizationLocSalesPersonDto>>> Process_GetAllOrganizationLocSalesPeople(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific OrganizationLocSalesPerson.
	/// </summary>
	/// <param name="organizationLocSalesPersonId">The Unique Id of the OrganizationLocSalesPerson to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the OrganizationLocSalesPerson DTO.</returns>
	Task<ERPResponseMessageDto<ERPOrganizationLocSalesPersonDto>> Process_GetOrganizationLocSalesPerson(Guid organizationLocSalesPersonId);

	/// <summary>
	/// Processes the creating or updating of a OrganizationLocSalesPerson record.
	/// </summary>
	/// <param name="organizationLocSalesPerson">The OrganizationLocSalesPerson data transfer object (DTO) containing the details of the OrganizationLocSalesPerson to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the OrganizationLocSalesPerson details.</returns>
	Task<ERPResponseMessageDto<ERPOrganizationLocSalesPersonDto>> Process_PutOrganizationLocSalesPerson(ERPOrganizationLocSalesPersonDto organizationLocSalesPerson);

	/// <summary>
	/// Validates the request for deleting a OrganizationLocSalesPerson record.
	/// </summary>
	/// <param name="organizationLocSalesPersonId">The Unique Id of the OrganizationLocSalesPerson.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteOrganizationLocSalesPerson(Guid organizationLocSalesPersonId);

	/// <summary>
	/// Processes the request to delete a OrganizationLocSalesPerson record.
	/// </summary>
	/// <param name="organizationLocSalesPersonId">The Unique Id of the OrganizationLocSalesPerson.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPOrganizationLocSalesPersonDto>> Process_DeleteOrganizationLocSalesPerson(Guid organizationLocSalesPersonId);
}
