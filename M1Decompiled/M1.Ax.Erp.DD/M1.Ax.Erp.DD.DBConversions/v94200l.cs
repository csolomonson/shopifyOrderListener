using M1.Core;

namespace M1.Ax.Erp.DD.DBConversions;

[DBConversion("9.4.200", "Alter column type in apfBox17 field from Form1099MISCYearTotals table", "2021-08-23")]
public class v94200l
{
	public v94200l(DBConversionParms parms)
	{
		string initialVersion = parms.InitialVersion;
		if (("8.10.050".CompareTo(initialVersion) == -1 || "8.10.050".CompareTo(initialVersion) == 0) && parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Form1099MISCYearTotals", "apfBox17"))
		{
			parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Form1099MISCYearTotals", "apfBox17", "money", 12, 2, isNullable: false, parms.Messages);
		}
	}
}
