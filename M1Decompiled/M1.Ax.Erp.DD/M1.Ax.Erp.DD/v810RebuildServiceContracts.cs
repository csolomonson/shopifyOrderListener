using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert ServiceContracts to support unicode", "2013-10-17")]
public class v810RebuildServiceContracts
{
	public v810RebuildServiceContracts(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ServiceContracts", new DmoField[21]
		{
			new DmoField("kbsServiceContractID", "nvarchar", 10, 0, nullable: false),
			new DmoField("kbsOrganizationID", "nvarchar", 10, 0, nullable: false),
			new DmoField("kbsServiceContractTypeID", "nvarchar", 5, 0, nullable: false),
			new DmoField("kbsProjectID", "nvarchar", 10, 0, nullable: false),
			new DmoField("kbsDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("kbsLongDescriptionRTF", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("kbsLongDescriptionText", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("kbsSerialNumberID", "nvarchar", 30, 0, nullable: false),
			new DmoField("kbsPartID", "nvarchar", 30, 0, nullable: false),
			new DmoField("kbsPartRevisionID", "nvarchar", 15, 0, nullable: false),
			new DmoField("kbsPartShortDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("kbsResellerOrganizationID", "nvarchar", 10, 0, nullable: false),
			new DmoField("kbsContractLength", "smallint", 4, 0, nullable: false),
			new DmoField("kbsContractLengthType", "nvarchar", 1, 0, nullable: false),
			new DmoField("kbsStartDate", "date", 14, 0, nullable: true),
			new DmoField("kbsEndDate", "date", 14, 0, nullable: true),
			new DmoField("kbsContractAmount", "money", 12, 2, nullable: false),
			new DmoField("kbsProjectAreaID", "nvarchar", 15, 0, nullable: false),
			new DmoField("kbsCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("kbsCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("kbsUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[12]
		{
			new DmoIndex("KBSSERVICECONTRACTID", unique: true),
			new DmoIndex("KBSUNIQUEID", unique: true),
			new DmoIndex("kbsOrganizationID", unique: false),
			new DmoIndex("kbsServiceContractTypeID", unique: false),
			new DmoIndex("kbsProjectID", unique: false),
			new DmoIndex("kbsSerialNumberID", unique: false),
			new DmoIndex("kbsPartID", unique: false),
			new DmoIndex("kbsPartRevisionID", unique: false),
			new DmoIndex("kbsResellerOrganizationID", unique: false),
			new DmoIndex("kbsStartDate", unique: false),
			new DmoIndex("kbsEndDate", unique: false),
			new DmoIndex("kbsProjectAreaID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
