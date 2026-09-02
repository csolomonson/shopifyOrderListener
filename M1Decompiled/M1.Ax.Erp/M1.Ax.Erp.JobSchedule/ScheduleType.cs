using System.Collections.Generic;

namespace M1.Ax.Erp.JobSchedule;

public class ScheduleType : List<ScheduleTypeBucket>
{
	public static byte QueueStart = 1;

	public static byte SetupStart = 2;

	public static byte ProductionStart = 3;

	public static byte ProductionEnd = 4;

	public static byte MoveEnd = 5;

	public byte TypeID => 1;

	public ScheduleType()
	{
		Add(new ScheduleTypeBucket(1, 1, 1, "QueueStart", requiresMachine: false));
		Add(new ScheduleTypeBucket(1, 2, 2, "SetupStart", requiresMachine: true));
		Add(new ScheduleTypeBucket(1, 3, 3, "ProductionStart", requiresMachine: true));
		Add(new ScheduleTypeBucket(1, 4, 4, "ProductionEnd", requiresMachine: false));
		Add(new ScheduleTypeBucket(1, 5, 5, "MoveEnd", requiresMachine: false));
	}

	public ScheduleType(byte typeId)
	{
		Add(new ScheduleTypeBucket(typeId, 1, 1, "QueueStart", requiresMachine: false));
		Add(new ScheduleTypeBucket(typeId, 2, 2, "SetupStart", requiresMachine: true));
		Add(new ScheduleTypeBucket(typeId, 3, 3, "ProductionStart", requiresMachine: true));
		Add(new ScheduleTypeBucket(typeId, 4, 4, "ProductionEnd", requiresMachine: false));
		Add(new ScheduleTypeBucket(typeId, 5, 5, "MoveEnd", requiresMachine: false));
	}

	public ScheduleTypeBucket GetByID(byte id)
	{
		return base[id - 1];
	}
}
