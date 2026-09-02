using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.1.040", "Add fields to Form1094YearTotals table", "2016-03-06")]
public class v91040b
{
	public v91040b(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "Form1094YearTotals"))
		{
			parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Form1094YearTotals", new DmoField[58]
			{
				new DmoField("hctForm1094YearID", "smallint", 4, 0, nullable: false),
				new DmoField("hctPlantID", "nvarchar", 5, 0, nullable: false),
				new DmoField("hctForm1094YearTotalID", "smallint", 4, 0, nullable: false),
				new DmoField("hctCorrectedIndicator", "bit", 1, 0, nullable: false),
				new DmoField("hctEmployeeID", "nvarchar", 10, 0, nullable: false),
				new DmoField("hctEmployeeFirstName", "nvarchar", 20, 0, nullable: false),
				new DmoField("hctEmployeeMiddleName", "nvarchar", 20, 0, nullable: false),
				new DmoField("hctEmployeeLastName", "nvarchar", 20, 0, nullable: false),
				new DmoField("hctEmployeeSSN", "nvarchar", 11, 0, nullable: false),
				new DmoField("hctEmployeeAddressLine1", "nvarchar", 50, 0, nullable: false),
				new DmoField("hctEmployeeAddressLine2", "nvarchar", 50, 0, nullable: false),
				new DmoField("hctEmployeeCity", "nvarchar", 30, 0, nullable: false),
				new DmoField("hctEmployeeState", "nvarchar", 3, 0, nullable: false),
				new DmoField("hctEmployeePostCode", "nvarchar", 10, 0, nullable: false),
				new DmoField("hctStartMonthNumber", "tinyint", 2, 0, nullable: false),
				new DmoField("hctAnnualOfferOfCoverage", "nvarchar", 2, 0, nullable: false),
				new DmoField("hctAnnualShareLowestCost", "numeric", 19, 2, nullable: false),
				new DmoField("hctAnnualSafeHarbor", "nvarchar", 2, 0, nullable: false),
				new DmoField("hctJanOfferOfCoverage", "nvarchar", 2, 0, nullable: false),
				new DmoField("hctJanShareLowestCost", "numeric", 19, 2, nullable: false),
				new DmoField("hctJanSafeHarbor", "nvarchar", 2, 0, nullable: false),
				new DmoField("hctFebOfferOfCoverage", "nvarchar", 2, 0, nullable: false),
				new DmoField("hctFebShareLowestCost", "numeric", 19, 2, nullable: false),
				new DmoField("hctFebSafeHarbor", "nvarchar", 2, 0, nullable: false),
				new DmoField("hctMarOfferOfCoverage", "nvarchar", 2, 0, nullable: false),
				new DmoField("hctMarShareLowestCost", "numeric", 19, 2, nullable: false),
				new DmoField("hctMarSafeHarbor", "nvarchar", 2, 0, nullable: false),
				new DmoField("hctAprOfferOfCoverage", "nvarchar", 2, 0, nullable: false),
				new DmoField("hctAprShareLowestCost", "numeric", 19, 2, nullable: false),
				new DmoField("hctAprSafeHarbor", "nvarchar", 2, 0, nullable: false),
				new DmoField("hctMayOfferOfCoverage", "nvarchar", 2, 0, nullable: false),
				new DmoField("hctMayShareLowestCost", "numeric", 19, 2, nullable: false),
				new DmoField("hctMaySafeHarbor", "nvarchar", 2, 0, nullable: false),
				new DmoField("hctJunOfferOfCoverage", "nvarchar", 2, 0, nullable: false),
				new DmoField("hctJunShareLowestCost", "numeric", 19, 2, nullable: false),
				new DmoField("hctJunSafeHarbor", "nvarchar", 2, 0, nullable: false),
				new DmoField("hctJulOfferOfCoverage", "nvarchar", 2, 0, nullable: false),
				new DmoField("hctJulShareLowestCost", "numeric", 19, 2, nullable: false),
				new DmoField("hctJulSafeHarbor", "nvarchar", 2, 0, nullable: false),
				new DmoField("hctAugOfferOfCoverage", "nvarchar", 2, 0, nullable: false),
				new DmoField("hctAugShareLowestCost", "numeric", 19, 2, nullable: false),
				new DmoField("hctAugSafeHarbor", "nvarchar", 2, 0, nullable: false),
				new DmoField("hctSeptOfferOfCoverage", "nvarchar", 2, 0, nullable: false),
				new DmoField("hctSeptShareLowestCost", "numeric", 19, 2, nullable: false),
				new DmoField("hctSeptSafeHarbor", "nvarchar", 2, 0, nullable: false),
				new DmoField("hctOctOfferOfCoverage", "nvarchar", 2, 0, nullable: false),
				new DmoField("hctOctShareLowestCost", "numeric", 19, 2, nullable: false),
				new DmoField("hctOctSafeHarbor", "nvarchar", 2, 0, nullable: false),
				new DmoField("hctNovOfferOfCoverage", "nvarchar", 2, 0, nullable: false),
				new DmoField("hctNovShareLowestCost", "numeric", 19, 2, nullable: false),
				new DmoField("hctNovSafeHarbor", "nvarchar", 2, 0, nullable: false),
				new DmoField("hctDecOfferOfCoverage", "nvarchar", 2, 0, nullable: false),
				new DmoField("hctDecShareLowestCost", "numeric", 19, 2, nullable: false),
				new DmoField("hctDecSafeHarbor", "nvarchar", 2, 0, nullable: false),
				new DmoField("hctClosed", "bit", 1, 0, nullable: false),
				new DmoField("hctCreatedBy", "nvarchar", 20, 0, nullable: false),
				new DmoField("hctCreatedDate", "datetime", 14, 0, nullable: true),
				new DmoField("hctUniqueID", "uniqueidentifier", 16, 0, nullable: false)
			}, new DmoIndex[6]
			{
				new DmoIndex("HCTFORM1094YEARID,HCTPLANTID,HCTFORM1094YEARTOTALID", unique: true),
				new DmoIndex("HCTUNIQUEID", unique: true),
				new DmoIndex("hctForm1094YearID", unique: false),
				new DmoIndex("hctPlantID", unique: false),
				new DmoIndex("hctForm1094YearTotalID", unique: false),
				new DmoIndex("hctEmployeeID", unique: false)
			});
		}
	}
}
