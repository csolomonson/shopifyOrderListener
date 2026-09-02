using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.575b", "Add fields to ProductionProperties table", "2017-10-25")]
public class v92575b
{
	[DBConversion("9.2.583", "Add fields to PurchasePlannerSessions table", "2017-11-27")]
	public class v92583a
	{
		public v92583a(DBConversionParms parms)
		{
			if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PurchasePlannerSessions", "ppsShowAllDemandForPartsOnJobs"))
			{
				parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PurchasePlannerSessions", "ppsShowAllDemandForPartsOnJobs", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
			}
		}
	}

	[DBConversion("9.2.587", "Create MRPRequirements table", "2017-11-29")]
	public class v92587a
	{
		public v92587a(DBConversionParms parms)
		{
			if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "MRPRequirements"))
			{
				parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "MRPRequirements", new DmoField[30]
				{
					new DmoField("mrrSessionID", "nvarchar", 10, 0, nullable: false),
					new DmoField("mrrLineID", "int", 7, 0, nullable: false),
					new DmoField("mrrRequirementID", "int", 4, 0, nullable: false),
					new DmoField("mrrPartID", "nvarchar", 30, 0, nullable: false),
					new DmoField("mrrPartRevisionID", "nvarchar", 15, 0, nullable: false),
					new DmoField("mrrPartWarehouseLocationID", "nvarchar", 5, 0, nullable: false),
					new DmoField("mrrPartBinID", "nvarchar", 15, 0, nullable: false),
					new DmoField("mrrSalesOrderID", "nvarchar", 10, 0, nullable: false),
					new DmoField("mrrSalesOrderLineID", "smallint", 4, 0, nullable: false),
					new DmoField("mrrSalesOrderDeliveryID", "smallint", 4, 0, nullable: false),
					new DmoField("mrrJobID", "nvarchar", 20, 0, nullable: false),
					new DmoField("mrrJobAssemblyID", "int", 5, 0, nullable: false),
					new DmoField("mrrJobMaterialID", "int", 5, 0, nullable: false),
					new DmoField("mrrDueDate", "datetime", 14, 0, nullable: true),
					new DmoField("mrrQuantityOnHand", "numeric", 15, 5, nullable: false),
					new DmoField("mrrQuantityAllocated", "numeric", 15, 5, nullable: false),
					new DmoField("mrrQuantityAvailable", "numeric", 15, 5, nullable: false),
					new DmoField("mrrMinimumQuantity", "numeric", 15, 5, nullable: false),
					new DmoField("mrrMaximumQuantity", "numeric", 15, 5, nullable: false),
					new DmoField("mrrManufacturingLotSize", "numeric", 15, 5, nullable: false),
					new DmoField("mrrQuantityRequired", "numeric", 15, 5, nullable: false),
					new DmoField("mrrQuantityReceived", "numeric", 15, 5, nullable: false),
					new DmoField("mrrQuantityResolved", "numeric", 15, 5, nullable: false),
					new DmoField("mrrSource", "nvarchar", 20, 0, nullable: false),
					new DmoField("mrrRequirementType", "nvarchar", 20, 0, nullable: false),
					new DmoField("mrrDemandType", "nvarchar", 20, 0, nullable: false),
					new DmoField("mrrSupplyType", "nvarchar", 20, 0, nullable: false),
					new DmoField("mrrCreatedBy", "nvarchar", 20, 0, nullable: false),
					new DmoField("mrrCreatedDate", "datetime", 14, 0, nullable: true),
					new DmoField("mrrUniqueID", "uniqueidentifier", 16, 0, nullable: false)
				}, new DmoIndex[2]
				{
					new DmoIndex("mrrSessionID,mrrLineID,mrrRequirementID", unique: true),
					new DmoIndex("mrrUniqueID", unique: true)
				});
			}
		}
	}

	[DBConversion("9.2.587", "Create MRPJobDetails table", "2017-11-29")]
	public class v92587b
	{
		public v92587b(DBConversionParms parms)
		{
			if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "MRPJobDetails"))
			{
				parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "MRPJobDetails", new DmoField[22]
				{
					new DmoField("mrjSessionID", "nvarchar", 10, 0, nullable: false),
					new DmoField("mrjLineID", "int", 4, 0, nullable: false),
					new DmoField("mrjJobDetailID", "int", 4, 0, nullable: false),
					new DmoField("mrjCustomerOrganizationID", "nvarchar", 10, 0, nullable: false),
					new DmoField("mrjPartID", "nvarchar", 30, 0, nullable: false),
					new DmoField("mrjPartRevisionID", "nvarchar", 15, 0, nullable: false),
					new DmoField("mrjPartWarehouseLocationID", "nvarchar", 5, 0, nullable: false),
					new DmoField("mrjPartBinID", "nvarchar", 15, 0, nullable: false),
					new DmoField("mrjOrderQuantity", "numeric", 15, 5, nullable: false),
					new DmoField("mrjInventoryQuantity", "numeric", 15, 5, nullable: false),
					new DmoField("mrjProductionDueDate", "date", 14, 0, nullable: true),
					new DmoField("mrjGetPartMethod", "bit", 1, 0, nullable: false),
					new DmoField("mrjCompleted", "bit", 1, 0, nullable: false),
					new DmoField("mrjSalesOrderID", "nvarchar", 10, 0, nullable: false),
					new DmoField("mrjSalesOrderLineID", "smallint", 4, 0, nullable: false),
					new DmoField("mrjSalesOrderDeliveryID", "smallint", 4, 0, nullable: false),
					new DmoField("mrjJobID", "nvarchar", 20, 0, nullable: false),
					new DmoField("mrjJobAssemblyID", "int", 5, 0, nullable: false),
					new DmoField("mrjJobMaterialID", "int", 5, 0, nullable: false),
					new DmoField("mrjCreatedBy", "nvarchar", 20, 0, nullable: false),
					new DmoField("mrjCreatedDate", "datetime", 14, 0, nullable: true),
					new DmoField("mrjUniqueID", "uniqueidentifier", 16, 0, nullable: false)
				}, new DmoIndex[2]
				{
					new DmoIndex("mrjSessionID,mrjLineID,mrjJobDetailID", unique: true),
					new DmoIndex("mrjUniqueID", unique: true)
				});
			}
		}
	}

	[DBConversion("9.2.587", "Create MRPLines table", "2017-11-29")]
	public class v92587c
	{
		public v92587c(DBConversionParms parms)
		{
			if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "MRPLines"))
			{
				parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "MRPLines", new DmoField[16]
				{
					new DmoField("mrlSessionID", "nvarchar", 10, 0, nullable: false),
					new DmoField("mrlLineID", "int", 4, 0, nullable: false),
					new DmoField("mrlPlantID", "nvarchar", 5, 0, nullable: false),
					new DmoField("mrlWarehouseID", "nvarchar", 5, 0, nullable: false),
					new DmoField("mrlPartID", "nvarchar", 30, 0, nullable: false),
					new DmoField("mrlPartRevisionID", "nvarchar", 15, 0, nullable: false),
					new DmoField("mrlPartShortDescription", "nvarchar", 50, 0, nullable: false),
					new DmoField("mrlLotSize", "numeric", 15, 5, nullable: false),
					new DmoField("mrlMinimumQuantity", "numeric", 15, 5, nullable: false),
					new DmoField("mrlMaximumQuantity", "numeric", 15, 5, nullable: false),
					new DmoField("mrlQuantityOnHand", "numeric", 15, 5, nullable: false),
					new DmoField("mrlCompleted", "bit", 1, 0, nullable: false),
					new DmoField("mrlDataMissing", "bit", 1, 0, nullable: false),
					new DmoField("mrlCreatedBy", "nvarchar", 20, 0, nullable: false),
					new DmoField("mrlCreatedDate", "datetime", 14, 0, nullable: true),
					new DmoField("mrlUniqueID", "uniqueidentifier", 16, 0, nullable: false)
				}, new DmoIndex[2]
				{
					new DmoIndex("mrlSessionID,mrlLineID", unique: true),
					new DmoIndex("mrlUniqueID", unique: true)
				});
			}
		}
	}

	public v92575b(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ProductionProperties", "xapSFEEndJobCompletionCode"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ProductionProperties", "xapSFEEndJobCompletionCode", "numeric", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ProductionProperties", "xapSFEActiveJobQueueFields"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ProductionProperties", "xapSFEActiveJobQueueFields", "nvarchar(max)", 50, 0, verifyIndexes: true, dropTriggers: true, isNullable: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ProductionProperties", "xapSFETouchScreen"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ProductionProperties", "xapSFETouchScreen", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ProductionProperties", "xapSFEBarcodeScanner"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ProductionProperties", "xapSFEBarcodeScanner", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ProductionProperties", "xapSFEJobSearchSelect"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ProductionProperties", "xapSFEJobSearchSelect", "nvarchar(max)", 50, 0, verifyIndexes: true, dropTriggers: true, isNullable: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ProductionProperties", "xapSFEEndJobScrapQty"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ProductionProperties", "xapSFEEndJobScrapQty", "nvarchar(max)", 50, 0, verifyIndexes: true, dropTriggers: true, isNullable: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ProductionProperties", "xapSFEIssueMaterialQty"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ProductionProperties", "xapSFEIssueMaterialQty", "nvarchar(max)", 50, 0, verifyIndexes: true, dropTriggers: true, isNullable: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ProductionProperties", "xapSFESetupPercentage"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ProductionProperties", "xapSFESetupPercentage", "nvarchar", 3, 0, verifyIndexes: true, dropTriggers: true, isNullable: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ProductionProperties", "xapSFEWorkQueueFields"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ProductionProperties", "xapSFEWorkQueueFields", "nvarchar(max)", 50, 0, verifyIndexes: true, dropTriggers: true, isNullable: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ProductionProperties", "xapSFETCAuditReport"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ProductionProperties", "xapSFETCAuditReport", "nvarchar(max)", 50, 0, verifyIndexes: true, dropTriggers: true, isNullable: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ProductionProperties", "xapSFEJobTraveller"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ProductionProperties", "xapSFEJobTraveller", "nvarchar(max)", 50, 0, verifyIndexes: true, dropTriggers: true, isNullable: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ProductionProperties", "xapSFEWorkQueueSort"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ProductionProperties", "xapSFEWorkQueueSort", "nvarchar(max)", 50, 0, verifyIndexes: true, dropTriggers: true, isNullable: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ProductionProperties", "xapSFEAllowSuspend"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ProductionProperties", "xapSFEAllowSuspend", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ProductionProperties", "xapSFEStartJobWorkCode"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ProductionProperties", "xapSFEStartJobWorkCode", "numeric", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ProductionProperties", "xapSFEAddPartSelect"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ProductionProperties", "xapSFEAddPartSelect", "nvarchar(max)", 50, 0, verifyIndexes: true, dropTriggers: true, isNullable: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ProductionProperties", "xapSFEEndJobGoodQty"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ProductionProperties", "xapSFEEndJobGoodQty", "nvarchar(max)", 50, 0, verifyIndexes: true, dropTriggers: true, isNullable: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ProductionProperties", "xapSFEAsmSearchFields"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ProductionProperties", "xapSFEAsmSearchFields", "nvarchar(max)", 50, 0, verifyIndexes: true, dropTriggers: true, isNullable: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ProductionProperties", "xapSFEOprSearchFields"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ProductionProperties", "xapSFEOprSearchFields", "nvarchar(max)", 50, 0, verifyIndexes: true, dropTriggers: true, isNullable: true, parms.Messages);
		}
	}
}
