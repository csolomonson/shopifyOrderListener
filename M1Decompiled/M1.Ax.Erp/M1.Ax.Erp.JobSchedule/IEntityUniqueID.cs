using System;

namespace M1.Ax.Erp.JobSchedule;

public interface IEntityUniqueID
{
	Guid? UniqueID { get; set; }
}
