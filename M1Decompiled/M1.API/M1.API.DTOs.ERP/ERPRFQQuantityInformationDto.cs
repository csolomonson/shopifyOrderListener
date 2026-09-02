using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPRFQQuantityInformationDto
{
	public string rqqCreatedBy { get; set; }

	public DateTime? rqqCreatedDate { get; set; }

	public Guid rqqUniqueID { get; set; }

	public bool rqqClosed { get; set; }

	public short rqqLeadTime { get; set; }

	public decimal rqqPriceBase { get; set; }

	public decimal rqqPriceForeign { get; set; }

	public decimal rqqQuantity { get; set; }

	public string rqqRfqID { get; set; }

	public short rqqRfqLineID { get; set; }

	public short rqqRfqSupplierID { get; set; }

	public byte[] rqqRowVersion { get; set; }

	public short rqqRfqQuantityID { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
