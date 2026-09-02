using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPCustomerPackageModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all CustomerPackages with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of CustomerPackages to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllCustomerPackages(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving CustomerPackage information based on the specified CustomerPackage Unique Id.
	/// </summary>
	/// <param name="customerPackageId">The Unique Id of the CustomerPackage.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetCustomerPackage(Guid customerPackageId);

	/// <summary>
	/// Validates the PUT request for creating or updating CustomerPackage information based on the specified CustomerPackage.
	/// </summary>
	/// <param name="customerPackage">The CustomerPackage details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutCustomerPackage(ERPCustomerPackageDto customerPackage);

	/// <summary>
	/// Processes the request to retrieve all CustomerPackages with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of CustomerPackages to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of CustomerPackages DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPCustomerPackageDto>>> Process_GetAllCustomerPackages(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific CustomerPackage.
	/// </summary>
	/// <param name="customerPackageId">The Unique Id of the CustomerPackage to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the CustomerPackage DTO.</returns>
	Task<ERPResponseMessageDto<ERPCustomerPackageDto>> Process_GetCustomerPackage(Guid customerPackageId);

	/// <summary>
	/// Processes the creating or updating of a CustomerPackage record.
	/// </summary>
	/// <param name="customerPackage">The CustomerPackage data transfer object (DTO) containing the details of the CustomerPackage to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the CustomerPackage details.</returns>
	Task<ERPResponseMessageDto<ERPCustomerPackageDto>> Process_PutCustomerPackage(ERPCustomerPackageDto customerPackage);

	/// <summary>
	/// Validates the request for deleting a CustomerPackage record.
	/// </summary>
	/// <param name="customerPackageId">The Unique Id of the CustomerPackage.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteCustomerPackage(Guid customerPackageId);

	/// <summary>
	/// Processes the request to delete a CustomerPackage record.
	/// </summary>
	/// <param name="customerPackageId">The Unique Id of the CustomerPackage.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPCustomerPackageDto>> Process_DeleteCustomerPackage(Guid customerPackageId);
}
