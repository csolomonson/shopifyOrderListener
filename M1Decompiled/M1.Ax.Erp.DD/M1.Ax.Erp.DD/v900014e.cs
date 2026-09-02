using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.014", "Add fields to DMRShipmentLines table", "2014-12-15")]
public class v900014e
{
	public v900014e(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "DMRShipmentLines", "dslJobAssemblyID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "DMRShipmentLines", "dslJobAssemblyID", "int", 5, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "DMRShipmentLines", "dslConversionFactor"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "DMRShipmentLines", "dslConversionFactor", "numeric", 14, 8, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "DMRShipmentLines", "dslJobMaterialID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "DMRShipmentLines", "dslJobMaterialID", "int", 5, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "DMRShipmentLines", "dslJobMatQuantityShipped"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "DMRShipmentLines", "dslJobMatQuantityShipped", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "DMRShipmentLines", "dslInventoryUnitOfMeasure"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "DMRShipmentLines", "dslInventoryUnitOfMeasure", "nvarchar", 2, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "DMRShipmentLines", "dslJobOprQuantityShipped"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "DMRShipmentLines", "dslJobOprQuantityShipped", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "DMRShipmentLines", "dslKitPart"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "DMRShipmentLines", "dslKitPart", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "DMRShipmentLines", "dslJobID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "DMRShipmentLines", "dslJobID", "nvarchar", 20, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "DMRShipmentLines", "dslInventoryQuantityShipped"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "DMRShipmentLines", "dslInventoryQuantityShipped", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "DMRShipmentLines", "dslJobOperationID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "DMRShipmentLines", "dslJobOperationID", "int", 5, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "DMRShipmentLines", "dslDescription"))
		{
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update DMRShipmentLines Set dslDescription = imrShortDescription from DMRShipmentLines inner join PartRevisions on dslPartID = imrPartID and dslPartRevisionID = imrPartRevisionID");
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "DMRShipmentLines", "dslUnitOfMeasure"))
		{
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update DMRShipmentLines Set dslUnitOfMeasure = imrInventoryUnitOfMeasure from DMRShipmentLines inner join PartRevisions on dslPartID = imrPartID and dslPartRevisionID = imrPartRevisionID");
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "DMRShipmentLines", "dslConversionFactor"))
		{
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update DMRShipmentLines Set dslConversionFactor = case when dslConversionFactor = 0 then 1 else dslConversionFactor end");
		}
	}
}
