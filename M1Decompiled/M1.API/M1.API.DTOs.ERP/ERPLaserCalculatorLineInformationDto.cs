using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPLaserCalculatorLineInformationDto
{
	public string cclCreatedBy { get; set; }

	public DateTime? cclCreatedDate { get; set; }

	public decimal cclCutTime { get; set; }

	public string cclDescription { get; set; }

	public Guid cclUniqueID { get; set; }

	public Guid cclLaserCalculatorID { get; set; }

	public decimal ccllength { get; set; }

	public decimal cclQuantity { get; set; }

	public decimal cclRate { get; set; }

	public byte[] cclRowVersion { get; set; }

	public int cclLaserCalculatorLineID { get; set; }

	public decimal cclWidth { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
