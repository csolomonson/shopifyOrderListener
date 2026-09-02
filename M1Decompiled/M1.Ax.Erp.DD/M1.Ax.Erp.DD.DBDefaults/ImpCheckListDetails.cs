using System;
using System.Collections.Generic;

namespace M1.Ax.Erp.DD.DBDefaults;

public class ImpCheckListDetails : IImpCheckList
{
	public int ID { get; set; }

	public string Name { get; set; }

	public string Code { get; set; }

	public int PercentDone { get; set; }

	public string AssignedTo { get; set; }

	public string CreatedBy { get; set; }

	public int ParentID { get; set; }

	public List<ImpCheckListDetailsChild> ChildNodes { get; set; }

	public DateTime CreatedDate { get; set; }
}
