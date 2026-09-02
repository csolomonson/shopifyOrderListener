using System;

namespace M1.Ax.Erp.JobSchedule;

public interface IEntityCreated
{
	string CreatedBy { get; set; }

	DateTime? CreatedDate { get; set; }
}
