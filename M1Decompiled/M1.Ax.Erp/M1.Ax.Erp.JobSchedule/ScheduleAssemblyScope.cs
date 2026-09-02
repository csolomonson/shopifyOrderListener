namespace M1.Ax.Erp.JobSchedule;

public enum ScheduleAssemblyScope : byte
{
	CurrentAssembly = 1,
	ParentAssemblies = 2,
	ChildAssemblies = 4
}
