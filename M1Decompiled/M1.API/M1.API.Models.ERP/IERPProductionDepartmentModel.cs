using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPProductionDepartmentModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all ProductionDepartments with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ProductionDepartments to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllProductionDepartments(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving ProductionDepartment information based on the specified ProductionDepartment Unique Id.
	/// </summary>
	/// <param name="productionDepartmentId">The Unique Id of the ProductionDepartment.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetProductionDepartment(Guid productionDepartmentId);

	/// <summary>
	/// Validates the PUT request for creating or updating ProductionDepartment information based on the specified ProductionDepartment.
	/// </summary>
	/// <param name="productionDepartment">The ProductionDepartment details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutProductionDepartment(ERPProductionDepartmentDto productionDepartment);

	/// <summary>
	/// Processes the request to retrieve all ProductionDepartments with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ProductionDepartments to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of ProductionDepartments DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPProductionDepartmentDto>>> Process_GetAllProductionDepartments(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific ProductionDepartment.
	/// </summary>
	/// <param name="productionDepartmentId">The Unique Id of the ProductionDepartment to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the ProductionDepartment DTO.</returns>
	Task<ERPResponseMessageDto<ERPProductionDepartmentDto>> Process_GetProductionDepartment(Guid productionDepartmentId);

	/// <summary>
	/// Processes the creating or updating of a ProductionDepartment record.
	/// </summary>
	/// <param name="productionDepartment">The ProductionDepartment data transfer object (DTO) containing the details of the ProductionDepartment to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the ProductionDepartment details.</returns>
	Task<ERPResponseMessageDto<ERPProductionDepartmentDto>> Process_PutProductionDepartment(ERPProductionDepartmentDto productionDepartment);

	/// <summary>
	/// Validates the request for deleting a ProductionDepartment record.
	/// </summary>
	/// <param name="productionDepartmentId">The Unique Id of the ProductionDepartment.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteProductionDepartment(Guid productionDepartmentId);

	/// <summary>
	/// Processes the request to delete a ProductionDepartment record.
	/// </summary>
	/// <param name="productionDepartmentId">The Unique Id of the ProductionDepartment.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPProductionDepartmentDto>> Process_DeleteProductionDepartment(Guid productionDepartmentId);
}
