using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.014", "Add fields to RMAClaimLines table", "2014-12-15")]
public class v900014d
{
	public v900014d(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "RMAClaimLines", "ralKitPart"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "RMAClaimLines", "ralKitPart", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "RMAClaimLines", "ralRequiresInspection"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "RMAClaimLines", "ralRequiresInspection", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "RMAClaimLines", "ralSalesUnitOfMeasure"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "RMAClaimLines", "ralSalesUnitOfMeasure", "nvarchar", 2, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "RMAClaimLines", "ralConversionFactor"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "RMAClaimLines", "ralConversionFactor", "numeric", 14, 8, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "RMAClaimLines", "ralSalesQuantity"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "RMAClaimLines", "ralSalesQuantity", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "RMAClaimLines", "ralUnitPriceForeign"))
		{
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update RMAClaimLines Set ralUnitPriceForeign = ralFullUnitPriceForeign-ralUnitDiscountForeign");
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "RMAClaimLines", "ralExtendedCostForeign"))
		{
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update RMAClaimLines Set ralExtendedCostForeign = ralQuantity*ralUnitCostForeign");
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "RMAClaimLines", "ralFullExtendedPriceForeign"))
		{
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update RMAClaimLines Set ralFullExtendedPriceForeign = ralQuantity*ralFullUnitPriceForeign");
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "RMAClaimLines", "ralPartGroupID"))
		{
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update RMAClaimLines Set ralPartGroupID = impPartGroupID from RMAClaimLines inner join Parts on ralPartID = impPartID");
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "RMAClaimLines", "ralExtendedDiscountForeign"))
		{
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update RMAClaimLines Set ralExtendedDiscountForeign = ralQuantity*ralUnitDiscountForeign");
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "RMAClaimLines", "ralExtendedPriceForeign"))
		{
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update RMAClaimLines Set ralExtendedPriceForeign = ralFullExtendedPriceForeign-ralExtendedDiscountForeign");
		}
	}
}
