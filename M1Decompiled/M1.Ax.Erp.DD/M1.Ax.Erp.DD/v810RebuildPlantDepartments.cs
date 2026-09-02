using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert PlantDepartments to support unicode", "2013-10-17")]
public class v810RebuildPlantDepartments
{
	public v810RebuildPlantDepartments(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PlantDepartments", new DmoField[22]
		{
			new DmoField("xavPlantID", "nvarchar", 5, 0, nullable: false),
			new DmoField("xavPlantDepartmentID", "nvarchar", 5, 0, nullable: false),
			new DmoField("xavName", "nvarchar", 50, 0, nullable: false),
			new DmoField("xavEstablishedDate", "date", 14, 0, nullable: true),
			new DmoField("xavUseProperties", "bit", 1, 0, nullable: false),
			new DmoField("xavARARGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("xavARCashGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("xavARFreightGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("xavARDiscountGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("xavARSalesGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("xavARBankAccountID", "nvarchar", 5, 0, nullable: false),
			new DmoField("xavAPAPGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("xavAPCashGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("xavAPFreightGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("xavAPDiscountGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("xavAPBankAccountID", "nvarchar", 5, 0, nullable: false),
			new DmoField("xavInactive", "bit", 1, 0, nullable: false),
			new DmoField("xavInactiveDate", "date", 14, 0, nullable: true),
			new DmoField("xavARDepositGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("xavCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("xavCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("xavUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[5]
		{
			new DmoIndex("XAVPLANTID,XAVPLANTDEPARTMENTID", unique: true),
			new DmoIndex("XAVUNIQUEID", unique: true),
			new DmoIndex("xavPlantID", unique: false),
			new DmoIndex("xavPlantDepartmentID", unique: false),
			new DmoIndex("xavInactive", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
