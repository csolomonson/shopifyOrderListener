using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert SuperannuationFunds to support unicode", "2013-10-17")]
public class v810RebuildSuperannuationFunds
{
	public v810RebuildSuperannuationFunds(DBConversionParms parms)
	{
		if (parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "SuperannuationFunds"))
		{
			parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "SuperannuationFunds", new DmoField[15]
			{
				new DmoField("lnfSuperannuationFundID", "nvarchar", 10, 0, nullable: false),
				new DmoField("lnfSuperFundID", "nvarchar", 50, 0, nullable: false),
				new DmoField("lnfSuperFundName", "nvarchar", 60, 0, nullable: false),
				new DmoField("lnfSuperFundSpinID", "nvarchar", 50, 0, nullable: false),
				new DmoField("lnfSuperFundEmployerID", "nvarchar", 16, 0, nullable: false),
				new DmoField("lnfSMSF", "bit", 1, 0, nullable: false),
				new DmoField("lnfSMSFABN", "nvarchar", 11, 0, nullable: false),
				new DmoField("lnfSMSFName", "nvarchar", byte.MaxValue, 0, nullable: false),
				new DmoField("lnfSMSFServiceAddress", "nvarchar", 16, 0, nullable: false),
				new DmoField("lnfSMSFBSB", "nvarchar", 6, 0, nullable: false),
				new DmoField("lnfSMSFAccountNumber", "nvarchar", 10, 0, nullable: false),
				new DmoField("lnfSMSFAccountName", "nvarchar", byte.MaxValue, 0, nullable: false),
				new DmoField("lnfCreatedBy", "nvarchar", 20, 0, nullable: false),
				new DmoField("lnfCreatedDate", "datetime", 14, 0, nullable: true),
				new DmoField("lnfUniqueID", "uniqueidentifier", 16, 0, nullable: false)
			}, new DmoIndex[2]
			{
				new DmoIndex("lnfSuperannuationFundID", unique: true),
				new DmoIndex("lnfUniqueID", unique: true)
			}, mergeCustomFields: true);
		}
	}
}
