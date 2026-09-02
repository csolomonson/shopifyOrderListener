using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.065", "Remove fields from PartUnitSalePrices table", "2014-01-25")]
public class v810065a
{
	public v810065a(DBConversionParms parms)
	{
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PartUnitSalePrices", "imhPartWarehouseLocationID"))
		{
			parms.Dmo.DropIndexes(null, parms.User, parms.DatabaseName, "PartUnitSalePrices", new DmoIndex[1]
			{
				new DmoIndex("IMHPARTID,IMHPARTREVISIONID,IMHPARTWAREHOUSELOCATIONID,IMHPARTBINID,IMHCURRENCYRATEID,IMHSTARTDATE", unique: true)
			}, parms.Messages);
			parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PartUnitSalePrices", "imhPartWarehouseLocationID", dropTriggers: true);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PartUnitSalePrices", "imhPartBinID"))
		{
			parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PartUnitSalePrices", "imhPartBinID", dropTriggers: true);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PartUnitSalePrices", "imhPartUnitSalePriceID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PartUnitSalePrices", "imhPartUnitSalePriceID", "smallint", 4, 0, verifyIndexes: false, dropTriggers: true, isNullable: false, parms.Messages);
			if (parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "PartUnitSalePricesTemp"))
			{
				parms.Dmo.DropTable(null, parms.User, parms.DatabaseName, "PartUnitSalePricesTemp");
			}
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "SELECT imhPartID,imhPartRevisionID,imhCurrencyRateID,imhStartDate,ROW_NUMBER() OVER (PARTITION BY imhPartID,imhPartRevisionID ORDER BY imhCurrencyRateID,imhStartDate) As RowFilter Into PartUnitSalePricesTemp FROM PartUnitSalePrices ORDER BY RowFilter");
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update PartUnitSalePrices Set imhPartUnitSalePriceID=b.RowFilter From PartUnitSalePrices a Inner Join PartUnitSalePricesTemp b On a.imhPartID=b.imhPartID And a.imhPartRevisionID=b.imhPartRevisionID And a.imhCurrencyRateID=b.imhCurrencyRateID And ((a.imhStartDate Is Null And b.imhStartDate Is Null) Or (a.imhStartDate=b.imhStartDate))");
			if (parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "PartUnitSalePricesTemp"))
			{
				parms.Dmo.DropTable(null, parms.User, parms.DatabaseName, "PartUnitSalePricesTemp");
			}
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PartUnitSalePrices", "imhPartUnitSalePriceID"))
		{
			parms.Dmo.RemoveDuplicates(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PartUnitSalePrices", parms.Messages);
			parms.Dmo.VerifyIndexes(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PartUnitSalePrices", new DmoIndex[1]
			{
				new DmoIndex("IMHPARTID,IMHPARTREVISIONID,imhPartUnitSalePriceID", unique: true)
			}, parms.Messages);
		}
	}
}
