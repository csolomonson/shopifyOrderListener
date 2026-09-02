using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPCustomTableRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a CustomTable record with the specified Unique Id exists.
	/// </summary>
	/// <param name="tableName">The custom table name.</param>
	/// <param name="customTableUniqueId">The Unique Id of the CustomTable record to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the CustomTable exists or not.</returns>
	Task<bool> DoesCustomTableRecordExist(string tableName, Guid customTableUniqueId);

	/// <summary>
	/// Retrieves all CustomTables with optional pagination.
	/// </summary>
	/// <param name="tableName"></param>
	/// <param name="pageSize">The maximum number of CustomTables to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy"></param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of CustomTables DTOs.</returns>
	Task<ICollection<ERPCustomTableInformationDto>> GetAllCustomTableRecords(string tableName, int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific CustomTable record.
	/// </summary>
	/// <param name="tableName">The custom table name.</param>
	/// <param name="customTableId">The Unique Id of the CustomTable record to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the CustomTable DTO.</returns>
	Task<ERPCustomTableInformationDto> GetCustomTableRecord(string tableName, Guid customTableId);

	/// <summary>
	/// Saves the provided ERP CustomTable.
	/// </summary>
	/// <param name="tableName">The custom table name.</param>
	/// <param name="customTable">The ERP CustomTable to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveCustomTableRecord(string tableName, ERPCustomTableDto customTable);
}
