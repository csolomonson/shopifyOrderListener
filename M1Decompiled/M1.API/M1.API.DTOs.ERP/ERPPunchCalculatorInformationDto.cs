using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPPunchCalculatorInformationDto
{
	public Guid ccuPunchCalculatorId { get; set; }

	public string ccuCreatedBy { get; set; }

	public DateTime? ccuCreatedDate { get; set; }

	public Guid ccuUniqueID { get; set; }

	public int ccuHitRate { get; set; }

	public int ccuHitsPerPart { get; set; }

	public decimal ccuPartsPerHour { get; set; }

	public int ccuPartsPerSheet { get; set; }

	public int ccuRepositions { get; set; }

	public decimal ccuRepositionTime { get; set; }

	public int ccuRepositionTimeSec { get; set; }

	public byte[] ccuRowVersion { get; set; }

	public decimal ccuSheetLoadTime { get; set; }

	public int ccuSheetLoadTimeSec { get; set; }

	public decimal ccuSheetsPerHour { get; set; }

	public decimal ccuTimeToPiece { get; set; }

	public int ccuToolChangeTimeSec { get; set; }

	public int ccuToolChangeTimeTotal { get; set; }

	public int ccuTools { get; set; }

	public decimal ccuTotalTimeMinutes { get; set; }

	public int ccuTotalTimeSeconds { get; set; }

	public int ccuTurns { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
