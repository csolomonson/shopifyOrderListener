using System;
using M1.Ax.Erp.DD.Helpers;
using M1.Core;

namespace M1.Ax.Erp.DD.DBConversions;

[DBConversion("9.3.100", "Add fields to DatasetProperties table", "2021-02-11")]
public class v93100a
{
	public v93100a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "DatasetProperties", "xadTimeZone"))
		{
			string text = M1Helpers.GetTimeZoneAbbreviation(TimeZone.CurrentTimeZone.StandardName) + TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).ToString();
			if (parms.User.Context.IsHosted)
			{
				text = "CST-06:00:00";
			}
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "DatasetProperties", "xadTimeZone", "nvarchar", 100, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update DatasetProperties Set xadTimeZone = '" + text + "';");
		}
	}
}
