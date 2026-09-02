using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPBankStatementInformationDto
{
	public string glsBankAccountID { get; set; }

	public int glsBankStatementReference { get; set; }

	public string glsCashGlAccountID { get; set; }

	public string glsCreatedBy { get; set; }

	public DateTime? glsCreatedDate { get; set; }

	public string glsCurrencyRateID { get; set; }

	public decimal glsEndingBalance { get; set; }

	public decimal glsEndingBalanceForeign { get; set; }

	public DateTime? glsEndingDate { get; set; }

	public Guid glsUniqueID { get; set; }

	public decimal glsExchangeAmount { get; set; }

	public string glsExchangeGlAccountID { get; set; }

	public decimal glsExchangeRate { get; set; }

	public short glsGlFiscalYearID { get; set; }

	public bool glsCustomRate { get; set; }

	public bool glsPostedToGl { get; set; }

	public decimal glsOpeningBalance { get; set; }

	public decimal glsOpeningBalanceForeign { get; set; }

	public DateTime? glsOpeningDate { get; set; }

	public DateTime? glsPostedDate { get; set; }

	public byte[] glsRowVersion { get; set; }

	public int glsBankStatementID { get; set; }

	public bool glsShowTransactions { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
