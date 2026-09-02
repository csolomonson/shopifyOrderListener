using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPSheetCalculatorInformationDto
{
	public decimal ccs0Rotation { get; set; }

	public decimal ccs90Rotation { get; set; }

	public Guid ccsSheetCalculatorID { get; set; }

	public string ccsCreatedBy { get; set; }

	public DateTime? ccsCreatedDate { get; set; }

	public Guid ccsUniqueID { get; set; }

	public bool ccsGrain { get; set; }

	public string ccsMeasurementType { get; set; }

	public decimal ccsPartSizeX { get; set; }

	public decimal ccsPartSizeY { get; set; }

	public decimal ccsPartSpacingX { get; set; }

	public decimal ccsPartSpacingY { get; set; }

	public byte[] ccsRowVersion { get; set; }

	public decimal ccsSheetSizeX { get; set; }

	public decimal ccsSheetSizeY { get; set; }

	public decimal ccsTotalTrimX { get; set; }

	public decimal ccsTotalTrimY { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
