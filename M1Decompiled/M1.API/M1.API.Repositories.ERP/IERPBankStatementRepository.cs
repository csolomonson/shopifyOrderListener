using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPBankStatementRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a BankStatement with the specified Unique Id exists.
	/// </summary>
	/// <param name="bankStatementId">The Unique Id of the BankStatement to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the BankStatement exists or not.</returns>
	Task<bool> DoesBankStatementExist(Guid bankStatementId);

	/// <summary>
	/// Retrieves all BankStatements with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of BankStatements to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of BankStatements DTOs.</returns>
	Task<ICollection<ERPBankStatementInformationDto>> GetAllBankStatements(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific BankStatement.
	/// </summary>
	/// <param name="bankStatementId">The Unique Id of the BankStatement to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the BankStatement DTO.</returns>
	Task<ERPBankStatementInformationDto> GetBankStatement(Guid bankStatementId);

	/// <summary>
	/// Saves the provided ERP bankStatement.
	/// </summary>
	/// <param name="bankStatement">The ERP bankStatement to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveBankStatement(ERPBankStatementDto bankStatement);
}
