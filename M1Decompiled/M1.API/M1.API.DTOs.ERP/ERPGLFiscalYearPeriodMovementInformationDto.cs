using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPGLFiscalYearPeriodMovementInformationDto
{
	public string gliCreatedBy { get; set; }

	public DateTime? gliCreatedDate { get; set; }

	public Guid gliUniqueID { get; set; }

	public string gliGlAccountID { get; set; }

	public short gliGlFiscalYearID { get; set; }

	public byte gliGlFiscalYearPeriodID { get; set; }

	public byte[] gliRowVersion { get; set; }

	public decimal gliTotalCredits { get; set; }

	public decimal gliTotalDebits { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
