using System.Data;
using System.Data.SqlClient;
using M1.Core;

namespace M1.Ax.Erp.Financials.AIR;

public class AIRDataRepository
{
	private M1Database Database;

	private const string SELECT_ALL_FORM1094YEARS_FOR_YEAR_AND_PLANT = "SELECT * FROM Form1094Years Where hcpForm1094YearID=@p1 And hcpPlantID=@p2 And hcpClosed=0";

	private const string SELECT_ALL_FORM1094YEARMONTHS_FOR_YEAR_AND_PLANT = "SELECT * FROM Form1094YearMonths Where hcmForm1094YearID=@p1 And hcmPlantID=@p2 And hcmClosed=0";

	private const string SELECT_ALL_FORM1094YEARALEMEMBERS_FOR_YEAR_AND_PLANT = "SELECT * FROM FORM1094YEARALEMEMBERS Where hcaForm1094YearID=@p1 And hcaPlantID=@p2 And hcaClosed=0";

	private const string SELECT_ALL_FORM1094YEARTOTALS_FOR_YEAR_AND_PLANT = "SELECT * FROM Form1094YearTotals Where hctForm1094YearID=@p1 And hctPlantID=@p2 And hctClosed=0";

	private const string SELECT_ALL_FORM1094YEARTOTALLINES_FOR_YEAR_AND_PLANT = "SELECT * FROM Form1094YearTotalLines Where hclForm1094YearID=@p1 And hclPlantID=@p2 And hclForm1094YearTotalID=@p3 And hclClosed=0";

	public AIRDataRepository(M1Database database)
	{
		Database = database;
	}

	public DataTable GetForm1094Data(string yearId, string plantId)
	{
		SqlCommand sqlCommand = null;
		sqlCommand = new SqlCommand("SELECT * FROM Form1094Years Where hcpForm1094YearID=@p1 And hcpPlantID=@p2 And hcpClosed=0");
		sqlCommand.Parameters.AddWithValue("@p1", yearId);
		sqlCommand.Parameters.AddWithValue("@p2", plantId);
		DataTable dataTable = Database.GetDataTable(sqlCommand);
		sqlCommand.Dispose();
		return dataTable;
	}

	public DataTable GetForm1094MonthsData(string yearId, string plantId)
	{
		SqlCommand sqlCommand = null;
		sqlCommand = new SqlCommand("SELECT * FROM Form1094YearMonths Where hcmForm1094YearID=@p1 And hcmPlantID=@p2 And hcmClosed=0");
		sqlCommand.Parameters.AddWithValue("@p1", yearId);
		sqlCommand.Parameters.AddWithValue("@p2", plantId);
		DataTable dataTable = Database.GetDataTable(sqlCommand);
		sqlCommand.Dispose();
		return dataTable;
	}

	public DataTable GetForm1094YearALEMembersData(string yearId, string plantId)
	{
		SqlCommand sqlCommand = null;
		sqlCommand = new SqlCommand("SELECT * FROM FORM1094YEARALEMEMBERS Where hcaForm1094YearID=@p1 And hcaPlantID=@p2 And hcaClosed=0");
		sqlCommand.Parameters.AddWithValue("@p1", yearId);
		sqlCommand.Parameters.AddWithValue("@p2", plantId);
		DataTable dataTable = Database.GetDataTable(sqlCommand);
		sqlCommand.Dispose();
		return dataTable;
	}

	public DataTable GetForm1095Data(string yearId, string plantId)
	{
		SqlCommand sqlCommand = null;
		sqlCommand = new SqlCommand("SELECT * FROM Form1094YearTotals Where hctForm1094YearID=@p1 And hctPlantID=@p2 And hctClosed=0");
		sqlCommand.Parameters.AddWithValue("@p1", yearId);
		sqlCommand.Parameters.AddWithValue("@p2", plantId);
		DataTable dataTable = Database.GetDataTable(sqlCommand);
		sqlCommand.Dispose();
		return dataTable;
	}

	public DataTable GetForm1095LinesData(string yearId, string plantId, string yeatTotalId)
	{
		SqlCommand sqlCommand = null;
		sqlCommand = new SqlCommand("SELECT * FROM Form1094YearTotalLines Where hclForm1094YearID=@p1 And hclPlantID=@p2 And hclForm1094YearTotalID=@p3 And hclClosed=0");
		sqlCommand.Parameters.AddWithValue("@p1", yearId);
		sqlCommand.Parameters.AddWithValue("@p2", plantId);
		sqlCommand.Parameters.AddWithValue("@p3", yeatTotalId);
		DataTable dataTable = Database.GetDataTable(sqlCommand);
		sqlCommand.Dispose();
		return dataTable;
	}
}
