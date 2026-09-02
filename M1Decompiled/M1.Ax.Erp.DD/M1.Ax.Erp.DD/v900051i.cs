using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.051", "Add fields to SuperannuationFunds table", "2015-06-23")]
public class v900051i
{
	public v900051i(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "SuperannuationFunds"))
		{
			parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "SuperannuationFunds", new DmoField[8]
			{
				new DmoField("lnfSuperannuationFundID", "nvarchar", 10, 0, nullable: false),
				new DmoField("lnfSuperFundID", "nvarchar", 50, 0, nullable: false),
				new DmoField("lnfSuperFundName", "nvarchar", 60, 0, nullable: false),
				new DmoField("lnfSuperFundSpinID", "nvarchar", 50, 0, nullable: false),
				new DmoField("lnfSuperFundEmployerID", "nvarchar", 16, 0, nullable: false),
				new DmoField("lnfCreatedBy", "nvarchar", 20, 0, nullable: false),
				new DmoField("lnfCreatedDate", "datetime", 14, 0, nullable: true),
				new DmoField("lnfUniqueID", "uniqueidentifier", 16, 0, nullable: false)
			}, new DmoIndex[2]
			{
				new DmoIndex("lnfSuperannuationFundID", unique: true),
				new DmoIndex("lnfUniqueID", unique: true)
			});
		}
	}
}
