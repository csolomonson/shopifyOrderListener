using System;
using System.Collections.Generic;

namespace M1.Ax.Erp.JobSchedule;

public interface IResourceGroup : IDisposable
{
	object DisplayID { get; }

	ResourceCalendarDefinition Calendar { get; }

	Guid GroupID { get; }

	byte ResourceType { get; }

	string PlantID { get; }

	string ProcessID { get; }

	bool InfiniteCapacity { get; }

	short PeoplePerMachineSetup { get; }

	short PeoplePerMachineProduction { get; }

	List<Guid> ResourceGuids { get; }
}
