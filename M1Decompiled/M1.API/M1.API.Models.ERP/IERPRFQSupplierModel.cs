using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPRFQSupplierModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all RFQSuppliers with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of RFQSuppliers to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllRFQSuppliers(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving RFQSupplier information based on the specified RFQSupplier Unique Id.
	/// </summary>
	/// <param name="rFQSupplierId">The Unique Id of the RFQSupplier.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetRFQSupplier(Guid rFQSupplierId);

	/// <summary>
	/// Validates the PUT request for creating or updating RFQSupplier information based on the specified RFQSupplier.
	/// </summary>
	/// <param name="rFQSupplier">The RFQSupplier details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutRFQSupplier(ERPRFQSupplierDto rFQSupplier);

	/// <summary>
	/// Processes the request to retrieve all RFQSuppliers with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of RFQSuppliers to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of RFQSuppliers DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPRFQSupplierDto>>> Process_GetAllRFQSuppliers(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific RFQSupplier.
	/// </summary>
	/// <param name="rFQSupplierId">The Unique Id of the RFQSupplier to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the RFQSupplier DTO.</returns>
	Task<ERPResponseMessageDto<ERPRFQSupplierDto>> Process_GetRFQSupplier(Guid rFQSupplierId);

	/// <summary>
	/// Processes the creating or updating of a RFQSupplier record.
	/// </summary>
	/// <param name="rFQSupplier">The RFQSupplier data transfer object (DTO) containing the details of the RFQSupplier to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the RFQSupplier details.</returns>
	Task<ERPResponseMessageDto<ERPRFQSupplierDto>> Process_PutRFQSupplier(ERPRFQSupplierDto rFQSupplier);

	/// <summary>
	/// Validates the request for deleting a RFQSupplier record.
	/// </summary>
	/// <param name="rFQSupplierId">The Unique Id of the RFQSupplier.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteRFQSupplier(Guid rFQSupplierId);

	/// <summary>
	/// Processes the request to delete a RFQSupplier record.
	/// </summary>
	/// <param name="rFQSupplierId">The Unique Id of the RFQSupplier.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPRFQSupplierDto>> Process_DeleteRFQSupplier(Guid rFQSupplierId);
}
