using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPGLJournalLineInformationDto
{
	public int gllArPaymentHeaderID { get; set; }

	public int gllArPaymentSessionID { get; set; }

	public string gllCreatedBy { get; set; }

	public DateTime? gllCreatedDate { get; set; }

	public decimal gllCreditAmount { get; set; }

	public decimal gllDebitAmount { get; set; }

	public string gllDescription { get; set; }

	public Guid gllUniqueID { get; set; }

	public string gllGlAccountID { get; set; }

	public short gllGlFiscalYearID { get; set; }

	public byte gllGlFiscalYearPeriodID { get; set; }

	public int gllGlJournalID { get; set; }

	public bool gllPosted { get; set; }

	public int gllJobAssemblyID { get; set; }

	public string gllJobID { get; set; }

	public int gllJobMaterialComponentID { get; set; }

	public int gllJobMaterialID { get; set; }

	public int gllJobOperationID { get; set; }

	public string gllLocationID { get; set; }

	public string gllOrganizationID { get; set; }

	public int gllPartTransactionID { get; set; }

	public string gllReference { get; set; }

	public byte[] gllRowVersion { get; set; }

	public int gllGlJournalLineID { get; set; }

	public string gllSourceTableName { get; set; }

	public Guid gllSourceTableUniqueID { get; set; }

	public decimal gllTaxableAmount { get; set; }

	public string gllTaxCodeID { get; set; }

	public decimal gllTransactionAmount { get; set; }

	public DateTime? gllTransactionDate { get; set; }

	public byte gllTransactionType { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
