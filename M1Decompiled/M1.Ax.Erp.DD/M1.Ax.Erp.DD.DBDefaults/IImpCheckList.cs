using System;

namespace M1.Ax.Erp.DD.DBDefaults;

public interface IImpCheckList
{
	int ID { get; set; }

	string Name { get; set; }

	string Code { get; set; }

	int PercentDone { get; set; }

	string AssignedTo { get; set; }

	string CreatedBy { get; set; }

	int ParentID { get; set; }

	DateTime CreatedDate { get; set; }
}
