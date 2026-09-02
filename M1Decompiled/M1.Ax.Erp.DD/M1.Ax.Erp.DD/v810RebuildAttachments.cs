using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert Attachments to support unicode", "2013-10-17")]
public class v810RebuildAttachments
{
	public v810RebuildAttachments(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Attachments", new DmoField[45]
		{
			new DmoField("cmaAttachmentID", "nvarchar", 10, 0, nullable: false),
			new DmoField("cmaOrganizationID", "nvarchar", 10, 0, nullable: false),
			new DmoField("cmaLocationID", "nvarchar", 5, 0, nullable: false),
			new DmoField("cmaContactID", "nvarchar", 5, 0, nullable: false),
			new DmoField("cmaAttachmentTypeID", "nvarchar", 5, 0, nullable: false),
			new DmoField("cmaDate", "date", 14, 0, nullable: true),
			new DmoField("cmaShortDescription", "nvarchar", 70, 0, nullable: false),
			new DmoField("cmaLongDescriptionRTF", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("cmaLongDescriptionText", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("cmaFileLocation", "nvarchar", byte.MaxValue, 0, nullable: false),
			new DmoField("cmaFilename", "nvarchar", byte.MaxValue, 0, nullable: false),
			new DmoField("cmaBLOB", "image", 4, 0, nullable: true),
			new DmoField("cmaPrintDefault", "bit", 1, 0, nullable: false),
			new DmoField("cmaEmailDefault", "bit", 1, 0, nullable: false),
			new DmoField("cmaCallID", "nvarchar", 10, 0, nullable: false),
			new DmoField("cmaLeadID", "nvarchar", 10, 0, nullable: false),
			new DmoField("cmaQuoteID", "nvarchar", 10, 0, nullable: false),
			new DmoField("cmaSalesOrderID", "nvarchar", 10, 0, nullable: false),
			new DmoField("cmaShipmentID", "nvarchar", 10, 0, nullable: false),
			new DmoField("cmaARInvoiceID", "nvarchar", 10, 0, nullable: false),
			new DmoField("cmaPurchaseOrderID", "nvarchar", 10, 0, nullable: false),
			new DmoField("cmaReceiptID", "nvarchar", 10, 0, nullable: false),
			new DmoField("cmaAPInvoiceID", "nvarchar", 10, 0, nullable: false),
			new DmoField("cmaProjectID", "nvarchar", 10, 0, nullable: false),
			new DmoField("cmaDMRClaimID", "nvarchar", 10, 0, nullable: false),
			new DmoField("cmaRMAClaimID", "nvarchar", 10, 0, nullable: false),
			new DmoField("cmaChangeRequestID", "nvarchar", 10, 0, nullable: false),
			new DmoField("cmaKnowledgeBasePageID", "nvarchar", 10, 0, nullable: false),
			new DmoField("cmaUploadedFromWeb", "bit", 1, 0, nullable: false),
			new DmoField("cmaReviewed", "bit", 1, 0, nullable: false),
			new DmoField("cmaJobID", "nvarchar", 20, 0, nullable: false),
			new DmoField("cmaProjectAreaID", "nvarchar", 15, 0, nullable: false),
			new DmoField("cmaPartID", "nvarchar", 30, 0, nullable: false),
			new DmoField("cmaDoNotAllowDownload", "bit", 1, 0, nullable: false),
			new DmoField("cmaCustomerGroupID", "nvarchar", 5, 0, nullable: false),
			new DmoField("cmaQualityRegisterID", "nvarchar", 10, 0, nullable: false),
			new DmoField("cmaNonConformanceID", "nvarchar", 10, 0, nullable: false),
			new DmoField("cmaInspectionID", "nvarchar", 10, 0, nullable: false),
			new DmoField("cmaInspectionLineID", "smallint", 4, 0, nullable: false),
			new DmoField("cmaRFQID", "nvarchar", 10, 0, nullable: false),
			new DmoField("cmaWorkFlowID", "nvarchar", 10, 0, nullable: false),
			new DmoField("cmaWorkFlowLineID", "smallint", 4, 0, nullable: false),
			new DmoField("cmaCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("cmaCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("cmaUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[32]
		{
			new DmoIndex("CMAATTACHMENTID", unique: true),
			new DmoIndex("CMAUNIQUEID", unique: true),
			new DmoIndex("cmaOrganizationID", unique: false),
			new DmoIndex("cmaLocationID", unique: false),
			new DmoIndex("cmaContactID", unique: false),
			new DmoIndex("cmaAttachmentTypeID", unique: false),
			new DmoIndex("cmaDate", unique: false),
			new DmoIndex("cmaCallID", unique: false),
			new DmoIndex("cmaLeadID", unique: false),
			new DmoIndex("cmaQuoteID", unique: false),
			new DmoIndex("cmaSalesOrderID", unique: false),
			new DmoIndex("cmaShipmentID", unique: false),
			new DmoIndex("cmaARInvoiceID", unique: false),
			new DmoIndex("cmaPurchaseOrderID", unique: false),
			new DmoIndex("cmaReceiptID", unique: false),
			new DmoIndex("cmaAPInvoiceID", unique: false),
			new DmoIndex("cmaProjectID", unique: false),
			new DmoIndex("cmaDMRClaimID", unique: false),
			new DmoIndex("cmaRMAClaimID", unique: false),
			new DmoIndex("cmaChangeRequestID", unique: false),
			new DmoIndex("cmaKnowledgeBasePageID", unique: false),
			new DmoIndex("cmaReviewed", unique: false),
			new DmoIndex("cmaJobID", unique: false),
			new DmoIndex("cmaProjectAreaID", unique: false),
			new DmoIndex("cmaPartID", unique: false),
			new DmoIndex("cmaQualityRegisterID", unique: false),
			new DmoIndex("cmaNonConformanceID", unique: false),
			new DmoIndex("cmaInspectionID", unique: false),
			new DmoIndex("cmaInspectionLineID", unique: false),
			new DmoIndex("cmaRFQID", unique: false),
			new DmoIndex("cmaWorkFlowID", unique: false),
			new DmoIndex("cmaWorkFlowLineID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
