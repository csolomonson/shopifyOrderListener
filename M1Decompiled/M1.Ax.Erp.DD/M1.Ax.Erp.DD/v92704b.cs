using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.704", "Alter MRPDemands table", "2018-05-18")]
public class v92704b
{
	public v92704b(DBConversionParms parms)
	{
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "MRPRequirements", "mrrSupplyType"))
		{
			parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "MRPRequirements", "mrrSupplyType", dropTriggers: true);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "MRPRequirements", "mrrMaximumQuantity"))
		{
			parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "MRPRequirements", "mrrMaximumQuantity", dropTriggers: true);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "MRPRequirements", "mrrQuantityAvailable"))
		{
			parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "MRPRequirements", "mrrQuantityAvailable", dropTriggers: true);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "MRPRequirements", "mrrMinimumQuantity"))
		{
			parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "MRPRequirements", "mrrMinimumQuantity", dropTriggers: true);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "MRPRequirements", "mrrRequirementType"))
		{
			parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "MRPRequirements", "mrrRequirementType", dropTriggers: true);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "MRPRequirements", "mrrManufacturingLotSize"))
		{
			parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "MRPRequirements", "mrrManufacturingLotSize", dropTriggers: true);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "MRPRequirements", "mrrQuantityAllocated"))
		{
			parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "MRPRequirements", "mrrQuantityAllocated", dropTriggers: true);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "MRPRequirements", "mrrQuantityOnHand"))
		{
			parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "MRPRequirements", "mrrQuantityOnHand", dropTriggers: true);
		}
		if (parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "MRPRequirements"))
		{
			parms.Dmo.RenameTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "MRPRequirements", "MRPDemands");
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "MRPDemands", "mrrQuantityRequired"))
		{
			parms.Dmo.RenameColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "MRPDemands", "mrrQuantityRequired", "mrrOriginalQuantity", dropTriggers: true);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "MRPDemands", "mrrRequirementID"))
		{
			parms.Dmo.RenameColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "MRPDemands", "mrrRequirementID", "mrrDemandID", dropTriggers: true);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "MRPDemands", "mrrDueDate"))
		{
			parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "MRPDemands", "mrrDueDate", "date", 14, 0, isNullable: true, parms.Messages);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "MRPDemands", "mrrDemandType"))
		{
			parms.Dmo.RenameColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "MRPDemands", "mrrDemandType", "mrrType", dropTriggers: true);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "MRPDemands", "mrrQuantityResolved"))
		{
			parms.Dmo.RenameColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "MRPDemands", "mrrQuantityResolved", "mrrQuantityShipped", dropTriggers: true);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "MRPDemands", "mrrDemandQuantity"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "MRPDemands", "mrrDemandQuantity", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		parms.Dmo.DropIndexes(null, parms.User, parms.DatabaseName, "MRPDemands", new DmoIndex[1]
		{
			new DmoIndex("mrrSessionID,mrrLineID,mrrRequirementID", unique: true)
		}, parms.Messages);
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "MRPDemands", "mrrDemandID"))
		{
			parms.Dmo.VerifyIndexes(null, parms.User, parms.DataDictionary, parms.DatabaseName, "MRPDemands", new DmoIndex[1]
			{
				new DmoIndex("mrrSessionID,mrrLineID,mrrDemandID", unique: true)
			}, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "MRPDemands", "mrrShipOrganizationID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "MRPDemands", "mrrShipOrganizationID", "nvarchar", 10, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "MRPDemands", "mrrCustomerOrganizationID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "MRPDemands", "mrrCustomerOrganizationID", "nvarchar", 10, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "MRPDemands", "mrrShipLocationID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "MRPDemands", "mrrShipLocationID", "nvarchar", 5, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
