using System.Runtime.InteropServices;

namespace M1.Core.Report;

[ComVisible(true)]
public enum ReportDisplayType : byte
{
	None,
	Filter,
	Prompt,
	DropDown,
	YearPeriod,
	CheckBoxGroup,
	Label,
	DropDownselect,
	DatasetSelect
}
