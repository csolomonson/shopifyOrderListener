using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPSerialNumberRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a SerialNumber with the specified Unique Id exists.
	/// </summary>
	/// <param name="serialNumberId">The Unique Id of the SerialNumber to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the SerialNumber exists or not.</returns>
	Task<bool> DoesSerialNumberExist(Guid serialNumberId);

	/// <summary>
	/// Retrieves all SerialNumbers with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of SerialNumbers to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of SerialNumbers DTOs.</returns>
	Task<ICollection<ERPSerialNumberInformationDto>> GetAllSerialNumbers(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific SerialNumber.
	/// </summary>
	/// <param name="serialNumberId">The Unique Id of the SerialNumber to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the SerialNumber DTO.</returns>
	Task<ERPSerialNumberInformationDto> GetSerialNumber(Guid serialNumberId);

	/// <summary>
	/// Saves the provided ERP serialNumber.
	/// </summary>
	/// <param name="serialNumber">The ERP serialNumber to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveSerialNumber(ERPSerialNumberDto serialNumber);
}
