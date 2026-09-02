using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPGLJournalInformationDto
{
	public string glpApInvoiceID { get; set; }

	public int glpApPaymentHeaderID { get; set; }

	public int glpApPaymentSessionID { get; set; }

	public string glpArInvoiceID { get; set; }

	public int glpArPaymentHeaderID { get; set; }

	public int glpArPaymentSessionID { get; set; }

	public int glpAssetAdjustmentID { get; set; }

	public string glpAssetID { get; set; }

	public int glpBankStatementID { get; set; }

	public string glpCreatedBy { get; set; }

	public DateTime? glpCreatedDate { get; set; }

	public string glpDescription { get; set; }

	public byte glpDetailSource { get; set; }

	public string glpDmrShipmentID { get; set; }

	public Guid glpUniqueID { get; set; }

	public short glpGlFiscalYearID { get; set; }

	public byte glpGlFiscalYearPeriodID { get; set; }

	public bool glpPosted { get; set; }

	public bool glpReversingEntry { get; set; }

	public int glpJobAssemblyID { get; set; }

	public string glpJobID { get; set; }

	public string glpLandedCostID { get; set; }

	public string glpLocationID { get; set; }

	public string glpLongDescriptionRtf { get; set; }

	public string glpLongDescriptionText { get; set; }

	public string glpOrganizationID { get; set; }

	public DateTime? glpPostedDate { get; set; }

	public string glpReceiptID { get; set; }

	public string glpReference { get; set; }

	public string glpRmaReceiptID { get; set; }

	public byte[] glpRowVersion { get; set; }

	public int glpGlJournalID { get; set; }

	public string glpShipmentID { get; set; }

	public byte glpSource { get; set; }

	public int glpTimecardID { get; set; }

	public decimal glpTotalCredits { get; set; }

	public decimal glpTotalDebits { get; set; }

	public DateTime? glpTransactionDate { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
