using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert PartGroupPlants to support unicode", "2013-10-17")]
public class v810RebuildPartGroupPlants
{
	public v810RebuildPartGroupPlants(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PartGroupPlants", new DmoField[12]
		{
			new DmoField("imvPartGroupID", "nvarchar", 5, 0, nullable: false),
			new DmoField("imvPartGroupPlantID", "nvarchar", 5, 0, nullable: false),
			new DmoField("imvSalesGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("imvDiscountGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("imvARDepositGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("imvCOGSLaborGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("imvCOGSMaterialGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("imvCOGSSubcontractGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("imvCOGSOverheadGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("imvCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("imvCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("imvUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[4]
		{
			new DmoIndex("IMVPARTGROUPID,IMVPARTGROUPPLANTID", unique: true),
			new DmoIndex("IMVUNIQUEID", unique: true),
			new DmoIndex("imvPartGroupID", unique: false),
			new DmoIndex("imvPartGroupPlantID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
