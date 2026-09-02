namespace M1.Ax.Erp.JobSchedule;

public class ScheduleTypeBucket
{
	public byte TypeID;

	public byte ID;

	public short Sequence;

	public string Text;

	public bool RequiresMachine;

	public int Color;

	public ScheduleTypeBucket(byte typeid, byte id, short sequence, string text, bool requiresMachine)
	{
		TypeID = typeid;
		ID = id;
		Sequence = sequence;
		Text = text;
		RequiresMachine = requiresMachine;
	}
}
