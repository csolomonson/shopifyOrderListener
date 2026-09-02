using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert PartGroups to support unicode", "2013-10-17")]
public class v810RebuildPartGroups
{
	public v810RebuildPartGroups(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PartGroups", new DmoField[31]
		{
			new DmoField("imuPartGroupID", "nvarchar", 5, 0, nullable: false),
			new DmoField("imuDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("imuSalesGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("imuDiscountGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("imuCommissionType", "tinyint", 1, 0, nullable: false),
			new DmoField("imuCommissionRate", "numeric", 5, 2, nullable: false),
			new DmoField("imuNextSerialNumberIDFormula", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("imuNextSerialNumberOption", "tinyint", 1, 0, nullable: false),
			new DmoField("imuNextSerialNumberValue", "nvarchar", 30, 0, nullable: false),
			new DmoField("imuShowGroupOnWeb", "bit", 1, 0, nullable: false),
			new DmoField("imuPartImageFileName", "nvarchar", 70, 0, nullable: false),
			new DmoField("imuParentPartGroupID", "nvarchar", 5, 0, nullable: false),
			new DmoField("imuQMMaterialMarkup", "numeric", 6, 2, nullable: false),
			new DmoField("imuQMSubcontractMarkup", "numeric", 6, 2, nullable: false),
			new DmoField("imuQMLaborMarkup", "numeric", 6, 2, nullable: false),
			new DmoField("imuQMOverHeadMarkup", "numeric", 6, 2, nullable: false),
			new DmoField("imuQMQuotingMarkup", "numeric", 6, 2, nullable: false),
			new DmoField("imuQMQuoteMarkupType", "tinyint", 1, 0, nullable: false),
			new DmoField("imuQMMarkupOption", "tinyint", 1, 0, nullable: false),
			new DmoField("imuQMPurchaseToOrderMarkup", "numeric", 6, 2, nullable: false),
			new DmoField("imuCOGSLaborGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("imuCOGSMaterialGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("imuCOGSSubcontractGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("imuCOGSOverheadGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("imuInactive", "bit", 1, 0, nullable: false),
			new DmoField("imuInactiveDate", "date", 14, 0, nullable: true),
			new DmoField("imuARDepositGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("imuAvalaraTaxCodeID", "nvarchar", 10, 0, nullable: false),
			new DmoField("imuCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("imuCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("imuUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[4]
		{
			new DmoIndex("IMUPARTGROUPID", unique: true),
			new DmoIndex("IMUUNIQUEID", unique: true),
			new DmoIndex("imuParentPartGroupID", unique: false),
			new DmoIndex("imuInactive", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
