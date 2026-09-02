using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.1.008", "Alter created date nullable status in multiple tables", "2016-01-28")]
public class v91008a
{
	public v91008a(DBConversionParms parms)
	{
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PayrollNZYearSchedules", "nzsCreatedDate"))
		{
			parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PayrollNZYearSchedules", "nzsCreatedDate", "datetime", 14, 0, isNullable: true, parms.Messages);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "STATEUITAXYEARQUARTERS", "puqCreatedDate"))
		{
			parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "STATEUITAXYEARQUARTERS", "puqCreatedDate", "datetime", 14, 0, isNullable: true, parms.Messages);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "FORM940YEARS", "pfyCreatedDate"))
		{
			parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "FORM940YEARS", "pfyCreatedDate", "datetime", 14, 0, isNullable: true, parms.Messages);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "FORM940YEARTOTALSTATES", "pfsCreatedDate"))
		{
			parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "FORM940YEARTOTALSTATES", "pfsCreatedDate", "datetime", 14, 0, isNullable: true, parms.Messages);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PayrollNZYears", "nzpCreatedDate"))
		{
			parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PayrollNZYears", "nzpCreatedDate", "datetime", 14, 0, isNullable: true, parms.Messages);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PayrollNZYearDeductions", "nzdCreatedDate"))
		{
			parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PayrollNZYearDeductions", "nzdCreatedDate", "datetime", 14, 0, isNullable: true, parms.Messages);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "COUNTYCODES", "xccCreatedDate"))
		{
			parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "COUNTYCODES", "xccCreatedDate", "datetime", 14, 0, isNullable: true, parms.Messages);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "STATEUITAXYEARQUARTERTOTALS", "putCreatedDate"))
		{
			parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "STATEUITAXYEARQUARTERTOTALS", "putCreatedDate", "datetime", 14, 0, isNullable: true, parms.Messages);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PayrollNZYearScheduleLines", "nzlCreatedDate"))
		{
			parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PayrollNZYearScheduleLines", "nzlCreatedDate", "datetime", 14, 0, isNullable: true, parms.Messages);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "SHIPMENTPACKAGEDETAILS", "spdCreatedDate"))
		{
			parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "SHIPMENTPACKAGEDETAILS", "spdCreatedDate", "datetime", 14, 0, isNullable: true, parms.Messages);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "SHIPMENTPACKAGES", "spaCreatedDate"))
		{
			parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "SHIPMENTPACKAGES", "spaCreatedDate", "datetime", 14, 0, isNullable: true, parms.Messages);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "FORM940YEARTOTALS", "pftCreatedDate"))
		{
			parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "FORM940YEARTOTALS", "pftCreatedDate", "datetime", 14, 0, isNullable: true, parms.Messages);
		}
	}
}
