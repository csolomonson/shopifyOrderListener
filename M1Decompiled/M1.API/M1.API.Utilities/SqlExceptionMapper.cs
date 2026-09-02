using System;
using System.Collections;
using System.Data.SqlClient;
using System.Net;

namespace M1.API.Utilities;

public class SqlExceptionMapper
{
	public static SqlErrorResult GetHttpStatusCodeForSqlException(SqlException ex)
	{
		IEnumerator enumerator = ex.Errors.GetEnumerator();
		try
		{
			if (enumerator.MoveNext())
			{
				SqlError sqlError = (SqlError)enumerator.Current;
				return sqlError.Number switch
				{
					547 => new SqlErrorResult
					{
						StatusCode = HttpStatusCode.BadRequest,
						ErrorDescription = "Foreign key violation. The operation failed because of a related record in another table."
					}, 
					2627 => new SqlErrorResult
					{
						StatusCode = HttpStatusCode.BadRequest,
						ErrorDescription = "Unique constraint violation. The operation tried to insert a duplicate value where only unique values are allowed."
					}, 
					2601 => new SqlErrorResult
					{
						StatusCode = HttpStatusCode.BadRequest,
						ErrorDescription = "Cannot insert duplicate key. A record with this key already exists."
					}, 
					515 => new SqlErrorResult
					{
						StatusCode = HttpStatusCode.BadRequest,
						ErrorDescription = "Cannot insert null into a non-nullable column. One or more required fields are missing."
					}, 
					245 => new SqlErrorResult
					{
						StatusCode = HttpStatusCode.BadRequest,
						ErrorDescription = "Conversion failed. There was a type mismatch in the data."
					}, 
					207 => new SqlErrorResult
					{
						StatusCode = HttpStatusCode.BadRequest,
						ErrorDescription = "Invalid column name. The query references a column that does not exist."
					}, 
					208 => new SqlErrorResult
					{
						StatusCode = HttpStatusCode.BadRequest,
						ErrorDescription = "Invalid table name. The query references a table that does not exist."
					}, 
					1205 => new SqlErrorResult
					{
						StatusCode = HttpStatusCode.InternalServerError,
						ErrorDescription = "Deadlock occurred. Two or more processes are trying to access the same resources in a way that causes a deadlock."
					}, 
					4060 => new SqlErrorResult
					{
						StatusCode = HttpStatusCode.InternalServerError,
						ErrorDescription = "Invalid database. The specified database is not available or does not exist."
					}, 
					18456 => new SqlErrorResult
					{
						StatusCode = HttpStatusCode.InternalServerError,
						ErrorDescription = "Login failed. Authentication to the database failed."
					}, 
					53 => new SqlErrorResult
					{
						StatusCode = HttpStatusCode.InternalServerError,
						ErrorDescription = "Network-related or instance-specific error. Unable to connect to the SQL Server."
					}, 
					2 => new SqlErrorResult
					{
						StatusCode = HttpStatusCode.InternalServerError,
						ErrorDescription = "SQL Server not found. The server is either unavailable or incorrectly configured."
					}, 
					8152 => new SqlErrorResult
					{
						StatusCode = HttpStatusCode.BadRequest,
						ErrorDescription = "The data you are trying to insert or update is too large for the column size."
					}, 
					_ => new SqlErrorResult
					{
						StatusCode = HttpStatusCode.InternalServerError,
						ErrorDescription = $"An unknown SQL error occurred with error number {sqlError.Number}. {sqlError.Message}"
					}, 
				};
			}
		}
		finally
		{
			IDisposable disposable = enumerator as IDisposable;
			if (disposable != null)
			{
				disposable.Dispose();
			}
		}
		return new SqlErrorResult
		{
			StatusCode = HttpStatusCode.InternalServerError,
			ErrorDescription = "An unknown SQL error occurred."
		};
	}
}
