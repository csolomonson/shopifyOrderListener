using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPAPInvoiceExpenseAccountInformationDto
{
	public decimal apxAmount { get; set; }

	public string apxApInvoiceID { get; set; }

	public short apxApInvoiceLineID { get; set; }

	public string apxCreatedBy { get; set; }

	public DateTime? apxCreatedDate { get; set; }

	public Guid apxUniqueID { get; set; }

	public string apxExpenseGlAccountID { get; set; }

	public bool apxPostedToGl { get; set; }

	public decimal apxPercent { get; set; }

	public byte[] apxRowVersion { get; set; }

	public short apxApInvoiceExpenseAccountID { get; set; }

	public string apxSourceTableName { get; set; }

	public Guid apxSourceTableUniqueID { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
