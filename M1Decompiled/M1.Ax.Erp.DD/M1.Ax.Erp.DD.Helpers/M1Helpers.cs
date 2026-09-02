namespace M1.Ax.Erp.DD.Helpers;

public class M1Helpers
{
	public static string GetTimeZoneAbbreviation(string timeZoneName)
	{
		string text = string.Empty;
		string[] array = timeZoneName.Split(' ');
		foreach (string text2 in array)
		{
			text = ((text2[0] == '(') ? (text + text2) : (text + text2[0]));
		}
		return text;
	}

	public static string ConvertFloatColumnToNumeric(string tableName, string fieldName)
	{
		return "IF (\r\n                                    SELECT DATA_TYPE\r\n                                    FROM INFORMATION_SCHEMA.COLUMNS\r\n                                    WHERE TABLE_NAME= '" + tableName + "' AND COLUMN_NAME  = '" + fieldName + "' ) = 'float'\r\n                                    BEGIN\r\n                                        DECLARE @def_name sysname;\r\n                                        SET @def_name = (SELECT\r\n                                            default_constraints.name\r\n                                            FROM\r\n                                                sys.all_columns\r\n                                                   INNER JOIN\r\n                                                sys.tables\r\n                                                    ON all_columns.object_id = tables.object_id\r\n                                                   INNER JOIN\r\n                                                sys.schemas\r\n                                                    ON tables.schema_id = schemas.schema_id\r\n                                                   INNER JOIN\r\n                                                sys.default_constraints\r\n                                                    ON all_columns.default_object_id = default_constraints.object_id\r\n                                           WHERE\r\n                                                    schemas.name = 'dbo'\r\n                                                AND tables.name = '" + tableName + "'\r\n                                                AND all_columns.name = '" + fieldName + "')\r\n\r\n                                        DECLARE @query nvarchar(2000);\r\n                                        SET @query = N'ALTER TABLE [dbo].[" + tableName + "] DROP CONSTRAINT [' + @def_name + ']';\r\n\r\n                                        exec sp_executesql @query;\r\n\r\n                                         ALTER TABLE " + tableName + " ALTER column [" + fieldName + "]DECIMAL(6,2) NOT NULL\r\n                                         ALTER TABLE [dbo].[" + tableName + "] ADD  DEFAULT ((0)) FOR [" + fieldName + "]\r\n\r\n                                    END";
	}

	public static string UpdateWarehouseBinsQohQtiFlagToTrue()
	{
		return "UPDATE WarehouseBins \r\n                                SET inbHasQOHQTI = vHasQOHorQTI \r\n                            FROM WarehouseBins \r\n\t                            inner join (select inbWarehouseID As vWarehouseID, inbWarehouseBinID as vWarehouseBinID, 1 As vHasQOHorQTI \r\n\t\t\t\t                            From WarehouseBins \r\n\t\t\t\t\t                            inner join partBins on imbWarehouseID = inbWarehouseID and imbPartBinID = inbWarehouseBinID And (imbQuantityOnHand > 0 OR imbQuantityToInspect > 0) \r\n\t\t\t\t                            Group by inbWarehouseID,inbWarehouseBinID) V on vWarehouseID = inbWarehouseID and vWarehouseBinId = inbWarehouseBinID";
	}
}
