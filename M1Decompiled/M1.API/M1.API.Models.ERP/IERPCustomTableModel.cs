using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPCustomTableModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all CustomTable records for the specified table with optional pagination.
	/// </summary>
	/// <param name="tableName">The custom table name.</param>
	/// <param name="pageSize">The maximum number of CustomTables records to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter to apply to the record set.</param>
	/// <param name="orderBy">The order by clause to apply to the record set.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllCustomTableRecords(string tableName, int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving a CustomTable record based on the specified CustomTable Unique Id.
	/// </summary>
	/// <param name="tableName">The custom table name.</param>
	/// <param name="customTableUniqueId">The Unique Id of the CustomTable record.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetCustomTableRecord(string tableName, Guid customTableUniqueId);

	/// <summary>
	/// Validates the PUT request for creating or updating a CustomTable record based on the specified CustomTable.
	/// </summary>
	/// <param name="tableName">The custom table name.</param>
	/// <param name="customTable">The CustomTable details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutCustomTableRecord(string tableName, ERPCustomTableDto customTable);

	/// <summary>
	/// Processes the request to retrieve all CustomTable records for the specified table with optional pagination.
	/// </summary>
	/// <param name="tableName">The custom table name.</param>
	/// <param name="pageSize">The maximum number of CustomTables records to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter to apply to the record set.</param>
	/// <param name="orderBy">The order by clause to apply to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of CustomTables DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPCustomTableDto>>> Process_GetAllCustomTableRecords(string tableName, int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific CustomTable.
	/// </summary>
	/// <param name="tableName">The custom table name.</param>
	/// <param name="customTableId">The Unique Id of the CustomTable record to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the CustomTable DTO.</returns>
	Task<ERPResponseMessageDto<ERPCustomTableDto>> Process_GetCustomTableRecord(string tableName, Guid customTableId);

	/// <summary>
	/// Processes the creating or updating of a CustomTable record.
	/// </summary>
	/// <param name="tableName">The custom table name.</param>
	/// <param name="customTableObject">The CustomTable data transfer object (DTO) containing the details of the CustomTable to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the CustomTable details.</returns>
	Task<ERPResponseMessageDto<ERPCustomTableDto>> Process_PutCustomTableRecord(string tableName, ERPCustomTableDto customTableObject);

	/// <summary>
	/// Validates the request for deleting a CustomTable record.
	/// </summary>
	/// <param name="tableName">The custom table name.</param>
	/// <param name="customTableUniqueId">The Unique Id of the CustomTable.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteCustomTableRecord(string tableName, Guid customTableUniqueId);

	/// <summary>
	/// Processes the request to delete a CustomTable record.
	/// </summary>
	/// <param name="tableName">The custom table name.</param>
	/// <param name="customTableUniqueId">The Unique Id of the CustomTable.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPCustomTableDto>> Process_DeleteCustomTableRecord(string tableName, Guid customTableUniqueId);
}
