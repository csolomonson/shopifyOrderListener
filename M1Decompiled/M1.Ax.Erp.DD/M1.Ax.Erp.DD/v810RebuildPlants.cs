using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert Plants to support unicode", "2013-10-17")]
public class v810RebuildPlants
{
	public v810RebuildPlants(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Plants", new DmoField[50]
		{
			new DmoField("xauPlantID", "nvarchar", 5, 0, nullable: false),
			new DmoField("xauName", "nvarchar", 50, 0, nullable: false),
			new DmoField("xauAddressLine1", "nvarchar", 50, 0, nullable: false),
			new DmoField("xauAddressLine2", "nvarchar", 50, 0, nullable: false),
			new DmoField("xauAddressLine3", "nvarchar", 50, 0, nullable: false),
			new DmoField("xauCity", "nvarchar", 30, 0, nullable: false),
			new DmoField("xauState", "nvarchar", 3, 0, nullable: false),
			new DmoField("xauPostCode", "nvarchar", 10, 0, nullable: false),
			new DmoField("xauCountry", "nvarchar", 20, 0, nullable: false),
			new DmoField("xauPhoneNumber", "nvarchar", 20, 0, nullable: false),
			new DmoField("xauFaxNumber", "nvarchar", 20, 0, nullable: false),
			new DmoField("xauEmailAddress", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("xauEstablishedDate", "date", 14, 0, nullable: true),
			new DmoField("xauUseProperties", "bit", 1, 0, nullable: false),
			new DmoField("xauARARGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("xauARCashGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("xauARFreightGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("xauARDiscountGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("xauARSalesGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("xauARBankAccountID", "nvarchar", 5, 0, nullable: false),
			new DmoField("xauAPAPGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("xauAPCashGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("xauAPFreightGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("xauAPDiscountGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("xauAPBankAccountID", "nvarchar", 5, 0, nullable: false),
			new DmoField("xauSVarLaborGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("xauSVarMaterialGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("xauSVarSubcontractGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("xauSVarOverheadGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("xauPurchaseVarianceGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("xauWIPLaborGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("xauWIPMaterialGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("xauWIPSubcontractGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("xauWIPOverheadGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("xauAccruedCreditorsGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("xauLaborClearingGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("xauOverheadClearingGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("xauStockRevaluationGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("xauInactive", "bit", 1, 0, nullable: false),
			new DmoField("xauInactiveDate", "date", 14, 0, nullable: true),
			new DmoField("xauARDepositGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("xauShipAwaitInvoiceGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("xauStockInTransitGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("xauFederalID", "nvarchar", 20, 0, nullable: false),
			new DmoField("xauCompanyLogo", "image", 4, 0, nullable: true),
			new DmoField("xauAvalaraAddressValidated", "bit", 1, 0, nullable: false),
			new DmoField("xauCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("xauCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("xauUniqueID", "uniqueidentifier", 16, 0, nullable: false),
			new DmoField("xauCountryCode", "nvarchar", 5, 0, nullable: false)
		}, new DmoIndex[3]
		{
			new DmoIndex("XAUPLANTID", unique: true),
			new DmoIndex("XAUUNIQUEID", unique: true),
			new DmoIndex("xauInactive", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
