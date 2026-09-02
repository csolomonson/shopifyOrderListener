using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert QuoteLines to support unicode", "2013-10-17")]
public class v810RebuildQuoteLines
{
	public v810RebuildQuoteLines(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "QuoteLines", new DmoField[39]
		{
			new DmoField("qmlQuoteID", "nvarchar", 10, 0, nullable: false),
			new DmoField("qmlQuoteLineID", "smallint", 4, 0, nullable: false),
			new DmoField("qmlSourceMethodID", "nvarchar", 30, 0, nullable: false),
			new DmoField("qmlSourceRevisionID", "nvarchar", 15, 0, nullable: false),
			new DmoField("qmlPartID", "nvarchar", 30, 0, nullable: false),
			new DmoField("qmlOrgPartID", "nvarchar", 30, 0, nullable: false),
			new DmoField("qmlPartRevisionID", "nvarchar", 15, 0, nullable: false),
			new DmoField("qmlUnitOfMeasure", "nvarchar", 2, 0, nullable: false),
			new DmoField("qmlPartGroupID", "nvarchar", 5, 0, nullable: false),
			new DmoField("qmlPartShortDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("qmlOrgPartShortDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("qmlPartLongDescriptionRTF", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("qmlPartLongDescriptionText", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("qmlProductionNotesRTF", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("qmlProductionNotesText", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("qmlTaxCodeID", "nvarchar", 5, 0, nullable: false),
			new DmoField("qmlNonTaxReasonID", "nvarchar", 5, 0, nullable: false),
			new DmoField("qmlSecondTaxCodeID", "nvarchar", 5, 0, nullable: false),
			new DmoField("qmlTransferredToOrder", "bit", 1, 0, nullable: false),
			new DmoField("qmlResolutionReasonID", "nvarchar", 5, 0, nullable: false),
			new DmoField("qmlDocuments", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("qmlLeadID", "nvarchar", 10, 0, nullable: false),
			new DmoField("qmlLeadLineID", "smallint", 4, 0, nullable: false),
			new DmoField("qmlQuoteMarkupType", "tinyint", 1, 0, nullable: false),
			new DmoField("qmlMatrixCalculated", "bit", 1, 0, nullable: false),
			new DmoField("qmlTaxesCalculated", "bit", 1, 0, nullable: false),
			new DmoField("qmlClosed", "bit", 1, 0, nullable: false),
			new DmoField("qmlPurchaseToOrder", "bit", 1, 0, nullable: false),
			new DmoField("qmlProjectID", "nvarchar", 10, 0, nullable: false),
			new DmoField("qmlPurchaseUnitCostBase", "numeric", 15, 5, nullable: false),
			new DmoField("qmlProjectAreaID", "nvarchar", 15, 0, nullable: false),
			new DmoField("qmlPurchaseUnitCostForeign", "numeric", 15, 5, nullable: false),
			new DmoField("qmlSupplierOrganizationID", "nvarchar", 10, 0, nullable: false),
			new DmoField("qmlPurchaseLocationID", "nvarchar", 5, 0, nullable: false),
			new DmoField("qmlFirm", "bit", 1, 0, nullable: false),
			new DmoField("qmlCreatedFromMobile", "bit", 1, 0, nullable: false),
			new DmoField("qmlCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("qmlCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("qmlUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[17]
		{
			new DmoIndex("QMLQUOTEID,QMLQUOTELINEID", unique: true),
			new DmoIndex("QMLUNIQUEID", unique: true),
			new DmoIndex("qmlQuoteID", unique: false),
			new DmoIndex("qmlQuoteLineID", unique: false),
			new DmoIndex("qmlSourceMethodID", unique: false),
			new DmoIndex("qmlSourceRevisionID", unique: false),
			new DmoIndex("qmlPartID", unique: false),
			new DmoIndex("qmlOrgPartID", unique: false),
			new DmoIndex("qmlPartRevisionID", unique: false),
			new DmoIndex("qmlLeadID", unique: false),
			new DmoIndex("qmlLeadLineID", unique: false),
			new DmoIndex("qmlClosed", unique: false),
			new DmoIndex("qmlPurchaseToOrder", unique: false),
			new DmoIndex("qmlProjectID", unique: false),
			new DmoIndex("qmlProjectAreaID", unique: false),
			new DmoIndex("qmlSupplierOrganizationID", unique: false),
			new DmoIndex("qmlFirm", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
