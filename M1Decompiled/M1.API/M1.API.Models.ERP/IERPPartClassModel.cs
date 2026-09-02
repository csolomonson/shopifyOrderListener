using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPPartClassModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all PartClasses with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of PartClasses to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllPartClasses(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving PartClass information based on the specified PartClass Unique Id.
	/// </summary>
	/// <param name="partClassId">The Unique Id of the PartClass.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetPartClass(Guid partClassId);

	/// <summary>
	/// Validates the PUT request for creating or updating PartClass information based on the specified PartClass.
	/// </summary>
	/// <param name="partClass">The PartClass details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutPartClass(ERPPartClassDto partClass);

	/// <summary>
	/// Processes the request to retrieve all PartClasses with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of PartClasses to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of PartClasses DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPPartClassDto>>> Process_GetAllPartClasses(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific PartClass.
	/// </summary>
	/// <param name="partClassId">The Unique Id of the PartClass to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the PartClass DTO.</returns>
	Task<ERPResponseMessageDto<ERPPartClassDto>> Process_GetPartClass(Guid partClassId);

	/// <summary>
	/// Processes the creating or updating of a PartClass record.
	/// </summary>
	/// <param name="partClass">The PartClass data transfer object (DTO) containing the details of the PartClass to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the PartClass details.</returns>
	Task<ERPResponseMessageDto<ERPPartClassDto>> Process_PutPartClass(ERPPartClassDto partClass);

	/// <summary>
	/// Validates the request for deleting a PartClass record.
	/// </summary>
	/// <param name="partClassId">The Unique Id of the PartClass.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeletePartClass(Guid partClassId);

	/// <summary>
	/// Processes the request to delete a PartClass record.
	/// </summary>
	/// <param name="partClassId">The Unique Id of the PartClass.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPPartClassDto>> Process_DeletePartClass(Guid partClassId);
}
