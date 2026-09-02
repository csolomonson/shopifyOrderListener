using System.Data;
using System.Data.SqlClient;
using M1.Core;

namespace M1.Ax.Erp;

public class MfgReceiptEstimatedJobCostCalculator
{
	private readonly M1Database _database;

	public MfgReceiptEstimatedJobCostCalculator(M1Database database)
	{
		_database = database;
	}

	public decimal CalculateUnitCost(string jobId, int jobAssemblyId, string estimatedCostType, string query)
	{
		decimal result = default(decimal);
		foreach (DataRow row in RetrieveJobAssembly(jobId, jobAssemblyId).Rows)
		{
			bool flag = row.Field<bool>("PullAllFromStock");
			decimal num = row.Field<decimal>("jmaQuantityToMake");
			SqlCommand sqlCommand = _database.NewSqlCommand(query);
			sqlCommand.Parameters.Add(new SqlParameter("@jmaPullAllFromStock", SqlDbType.Bit)).Value = flag;
			result = CalculateWithNestedAssemblies(jobId, jobAssemblyId, estimatedCostType, sqlCommand);
			if (jobAssemblyId == 0)
			{
				foreach (DataRow row2 in RetrieveJob(jobId).Rows)
				{
					decimal num2 = row2.Field<decimal>("jmpProductionQuantity");
					if (!(num2 == 0m))
					{
						result /= num2;
					}
				}
			}
			else if (num != 0m)
			{
				result /= num;
			}
			if (estimatedCostType.Equals("UnitEstMaterialCost"))
			{
				SqlCommand command = _database.NewSqlCommand("SELECT ISNULL((SELECT SUM(jmaEstimatedUnitCost * jmaQuantityToPull) FROM JobAssemblies b WHERE b.jmaJobID=@jobID AND b.jmaParentAssemblyID=@jobAssemblyID AND b.jmaJobAssemblyID!=@jobAssemblyID), 0) AS UnitEstMaterialCost");
				result += CalculateWithNestedAssemblies(jobId, jobAssemblyId, estimatedCostType, command);
			}
		}
		return result;
	}

	public decimal CalculateWithNestedAssemblies(string jobId, int jobAssemblyId, string estimatedCostType, SqlCommand command)
	{
		command.Parameters.Add(new SqlParameter("@JobAssemblyID", SqlDbType.VarChar)).Value = jobAssemblyId;
		command.Parameters.Add(new SqlParameter("@jobID", SqlDbType.VarChar)).Value = jobId;
		decimal result = _database.GetDataTable(command, null).Rows[0].Field<decimal>(estimatedCostType);
		foreach (DataRow row in RetrieveNestedAssemblies(jobId, jobAssemblyId).Rows)
		{
			int jobAssemblyId2 = row.Field<int>("jmaJobAssemblyID");
			SqlParameter value = command.Parameters["@jobID"];
			SqlParameter value2 = command.Parameters["@JobAssemblyID"];
			command.Parameters.Remove(value);
			command.Parameters.Remove(value2);
			result += CalculateWithNestedAssemblies(jobId, jobAssemblyId2, estimatedCostType, command);
		}
		return result;
	}

	private DataTable RetrieveJob(string jobId)
	{
		SqlCommand sqlCommand = _database.NewSqlCommand("SELECT jmpProductionQuantity FROM Jobs WHERE jmpJobID=@jobID");
		sqlCommand.Parameters.Add(new SqlParameter("@jobID", SqlDbType.VarChar)).Value = jobId;
		return _database.GetDataTable(sqlCommand, null);
	}

	private DataTable RetrieveJobAssembly(string jobId, int jobAssemblyId)
	{
		SqlCommand sqlCommand = _database.NewSqlCommand("SELECT ISNULL(jmaPullAllFromStock,0) AS PullAllFromStock, jmaParentAssemblyID, jmaQuantityToMake\r\n                                                         FROM JobAssemblies b WHERE b.jmaJobID=@jobID AND b.jmaJobAssemblyID=@jobAssemblyID ");
		sqlCommand.Parameters.Add(new SqlParameter("@jobID", SqlDbType.VarChar)).Value = jobId;
		sqlCommand.Parameters.Add(new SqlParameter("@JobAssemblyID", SqlDbType.VarChar)).Value = jobAssemblyId;
		return _database.GetDataTable(sqlCommand, null);
	}

	private DataTable RetrieveNestedAssemblies(string jobId, int parentAssemblyId)
	{
		SqlCommand sqlCommand = _database.NewSqlCommand("SELECT jmaJobAssemblyID, jmaQuantityPerParent, jmaQuantityToPull\r\n                                                    FROM JobAssemblies b WHERE b.jmaJobID=@jobID AND b.jmaParentAssemblyID=@parentAssemblyID AND b.jmaJobAssemblyID!=@parentAssemblyID");
		sqlCommand.Parameters.Add(new SqlParameter("@jobID", SqlDbType.VarChar)).Value = jobId;
		sqlCommand.Parameters.Add(new SqlParameter("@parentAssemblyID", SqlDbType.VarChar)).Value = parentAssemblyId;
		return _database.GetDataTable(sqlCommand, null);
	}
}
