using M1.Core;

namespace M1.Ax.Erp.DD.DBConversions;

[DBConversion("9.3.100", "Add machine intelligence views", "2021-03-10")]
public class v93100e
{
	public v93100e(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesViewExist(null, parms.User, parms.DatabaseName, "API_Department"))
		{
			parms.Dmo.VerifyTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "API_Department", parms.Messages, null);
		}
		if (!parms.Dmo.DoesViewExist(null, parms.User, parms.DatabaseName, "API_Employee"))
		{
			parms.Dmo.VerifyTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "API_Employee", parms.Messages, null);
		}
		if (!parms.Dmo.DoesViewExist(null, parms.User, parms.DatabaseName, "API_Job"))
		{
			parms.Dmo.VerifyTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "API_Job", parms.Messages, null);
		}
		if (!parms.Dmo.DoesViewExist(null, parms.User, parms.DatabaseName, "API_JobOperation"))
		{
			parms.Dmo.VerifyTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "API_JobOperation", parms.Messages, null);
		}
		if (!parms.Dmo.DoesViewExist(null, parms.User, parms.DatabaseName, "API_PartOperation"))
		{
			parms.Dmo.VerifyTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "API_PartOperation", parms.Messages, null);
		}
		if (!parms.Dmo.DoesViewExist(null, parms.User, parms.DatabaseName, "API_PartRevision"))
		{
			parms.Dmo.VerifyTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "API_PartRevision", parms.Messages, null);
		}
		if (!parms.Dmo.DoesViewExist(null, parms.User, parms.DatabaseName, "API_Process"))
		{
			parms.Dmo.VerifyTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "API_Process", parms.Messages, null);
		}
		if (!parms.Dmo.DoesViewExist(null, parms.User, parms.DatabaseName, "API_SalesOrderDelivery"))
		{
			parms.Dmo.VerifyTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "API_SalesOrderDelivery", parms.Messages, null);
		}
		if (!parms.Dmo.DoesViewExist(null, parms.User, parms.DatabaseName, "API_SalesOrderJob"))
		{
			parms.Dmo.VerifyTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "API_SalesOrderJob", parms.Messages, null);
		}
		if (!parms.Dmo.DoesViewExist(null, parms.User, parms.DatabaseName, "API_SalesOrderLine"))
		{
			parms.Dmo.VerifyTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "API_SalesOrderLine", parms.Messages, null);
		}
		if (!parms.Dmo.DoesViewExist(null, parms.User, parms.DatabaseName, "API_Shift"))
		{
			parms.Dmo.VerifyTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "API_Shift", parms.Messages, null);
		}
		if (!parms.Dmo.DoesViewExist(null, parms.User, parms.DatabaseName, "API_ShiftDay"))
		{
			parms.Dmo.VerifyTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "API_ShiftDay", parms.Messages, null);
		}
		if (!parms.Dmo.DoesViewExist(null, parms.User, parms.DatabaseName, "API_Timecard"))
		{
			parms.Dmo.VerifyTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "API_Timecard", parms.Messages, null);
		}
		if (!parms.Dmo.DoesViewExist(null, parms.User, parms.DatabaseName, "API_TimecardLine"))
		{
			parms.Dmo.VerifyTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "API_TimecardLine", parms.Messages, null);
		}
		if (!parms.Dmo.DoesViewExist(null, parms.User, parms.DatabaseName, "API_TimeZone"))
		{
			parms.Dmo.VerifyTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "API_TimeZone", parms.Messages, null);
		}
		if (!parms.Dmo.DoesViewExist(null, parms.User, parms.DatabaseName, "API_WorkCenter"))
		{
			parms.Dmo.VerifyTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "API_WorkCenter", parms.Messages, null);
		}
		if (!parms.Dmo.DoesViewExist(null, parms.User, parms.DatabaseName, "API_ProductionCalendar"))
		{
			parms.Dmo.VerifyTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "API_ProductionCalendar", parms.Messages, null);
		}
	}
}
