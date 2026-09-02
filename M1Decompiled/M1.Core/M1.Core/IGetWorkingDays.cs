using System;
using System.Collections.Generic;

namespace M1.Core;

public interface IGetWorkingDays : IDisposable
{
	Dictionary<DateTime, StartTimeAndHours> GetWorkingDaysInRange(DateTime startDate, DateTime endDate);

	List<DateTime> GetNonWorkingDaysInRange(DateTime startDate, DateTime endDate);
}
