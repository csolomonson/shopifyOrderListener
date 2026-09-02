using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPEmployeePersonalDatumModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all EmployeePersonalData with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of EmployeePersonalData to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllEmployeePersonalData(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving EmployeePersonalDatum information based on the specified EmployeePersonalDatum Unique Id.
	/// </summary>
	/// <param name="employeePersonalDatumId">The Unique Id of the EmployeePersonalDatum.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetEmployeePersonalDatum(Guid employeePersonalDatumId);

	/// <summary>
	/// Validates the PUT request for creating or updating EmployeePersonalDatum information based on the specified EmployeePersonalDatum.
	/// </summary>
	/// <param name="employeePersonalDatum">The EmployeePersonalDatum details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutEmployeePersonalDatum(ERPEmployeePersonalDatumDto employeePersonalDatum);

	/// <summary>
	/// Processes the request to retrieve all EmployeePersonalData with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of EmployeePersonalData to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of EmployeePersonalData DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPEmployeePersonalDatumDto>>> Process_GetAllEmployeePersonalData(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific EmployeePersonalDatum.
	/// </summary>
	/// <param name="employeePersonalDatumId">The Unique Id of the EmployeePersonalDatum to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the EmployeePersonalDatum DTO.</returns>
	Task<ERPResponseMessageDto<ERPEmployeePersonalDatumDto>> Process_GetEmployeePersonalDatum(Guid employeePersonalDatumId);

	/// <summary>
	/// Processes the creating or updating of a EmployeePersonalDatum record.
	/// </summary>
	/// <param name="employeePersonalDatum">The EmployeePersonalDatum data transfer object (DTO) containing the details of the EmployeePersonalDatum to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the EmployeePersonalDatum details.</returns>
	Task<ERPResponseMessageDto<ERPEmployeePersonalDatumDto>> Process_PutEmployeePersonalDatum(ERPEmployeePersonalDatumDto employeePersonalDatum);

	/// <summary>
	/// Validates the request for deleting a EmployeePersonalDatum record.
	/// </summary>
	/// <param name="employeePersonalDatumId">The Unique Id of the EmployeePersonalDatum.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteEmployeePersonalDatum(Guid employeePersonalDatumId);

	/// <summary>
	/// Processes the request to delete a EmployeePersonalDatum record.
	/// </summary>
	/// <param name="employeePersonalDatumId">The Unique Id of the EmployeePersonalDatum.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPEmployeePersonalDatumDto>> Process_DeleteEmployeePersonalDatum(Guid employeePersonalDatumId);
}
