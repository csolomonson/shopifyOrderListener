using System;
using System.Data.SqlClient;

namespace M1.Core;

public class ImportProcessingParms
{
	public SqlTransaction SqlTransaction;

	public M1DataDictionary DataDictionary;

	public M1Database Database;

	public string[,] sColumnMap;

	public string TempTable;

	public ImportProcessingParms(string tempTable, M1DataDictionary dataDictionary, M1Database database, string[,] sColumnMap)
	{
		TempTable = tempTable;
		DataDictionary = dataDictionary;
		Database = database;
		SqlTransaction = null;
		this.sColumnMap = sColumnMap;
	}

	public bool IsFieldInMap(string sField)
	{
		sField = sField.Trim().ToUpper();
		for (int i = sColumnMap.GetLowerBound(0); i < sColumnMap.GetUpperBound(0); i++)
		{
			if (sColumnMap[i, 1].Equals(sField, StringComparison.CurrentCultureIgnoreCase))
			{
				return true;
			}
		}
		return false;
	}
}
