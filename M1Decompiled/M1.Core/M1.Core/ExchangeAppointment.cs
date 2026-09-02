using System;
using Microsoft.Exchange.WebServices.Data;

namespace M1.Core;

public class ExchangeAppointment
{
	public bool IDChanged;

	private string _ID = string.Empty;

	private string _Subject = string.Empty;

	private string _Body = string.Empty;

	private string _MeetingLocation = string.Empty;

	public DateTime? Start;

	public DateTime? End;

	private TaskStatus _Status;

	private Importance _Importance = Importance.Normal;

	private bool _IsReminderSet;

	private DateTime? _ReminderDueBy;

	public Guid SourceUniqueID;

	public string ID
	{
		get
		{
			return _ID;
		}
		set
		{
			if (_ID == null || !_ID.Equals(value))
			{
				_ID = value;
				IDChanged = true;
			}
		}
	}

	public string Subject
	{
		get
		{
			return _Subject;
		}
		set
		{
			_Subject = value;
		}
	}

	public string Body
	{
		get
		{
			return _Body;
		}
		set
		{
			_Body = value;
		}
	}

	public string MeetingLocation
	{
		get
		{
			return _MeetingLocation;
		}
		set
		{
			_MeetingLocation = value;
		}
	}

	public TaskStatus Status
	{
		get
		{
			return _Status;
		}
		set
		{
			_Status = value;
		}
	}

	public Importance Importance
	{
		get
		{
			return _Importance;
		}
		set
		{
			_Importance = value;
		}
	}

	public bool IsReminderSet
	{
		get
		{
			return _IsReminderSet;
		}
		set
		{
			_IsReminderSet = value;
		}
	}

	public DateTime? ReminderDueBy
	{
		get
		{
			return _ReminderDueBy;
		}
		set
		{
			_ReminderDueBy = value;
		}
	}

	public ExchangeAppointment(string initialID)
	{
		_ID = initialID;
	}
}
