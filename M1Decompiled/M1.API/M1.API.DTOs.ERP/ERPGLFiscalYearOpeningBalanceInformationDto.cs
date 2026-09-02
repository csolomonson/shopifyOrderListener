using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPGLFiscalYearOpeningBalanceInformationDto
{
	public string glyCreatedBy { get; set; }

	public DateTime? glyCreatedDate { get; set; }

	public Guid glyUniqueID { get; set; }

	public string glyGlAccountID { get; set; }

	public short glyGlFiscalYearID { get; set; }

	public byte[] glyRowVersion { get; set; }

	public decimal glyYearOpeningBalance { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
