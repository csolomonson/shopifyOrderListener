using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace M1.Core;

[ComVisible(true)]
[ComDefaultInterface(typeof(IOpenWithParameters))]
public class OpenWithParameters : IOpenWithParameters
{
	private List<OpenWithDBInfo> _Databases = new List<OpenWithDBInfo>();

	private string _TopLevelTable = string.Empty;

	public string _ViewMode = string.Empty;

	public string _CurrentField = string.Empty;

	private int _hWnd;

	private IntPtr _Handle = IntPtr.Zero;

	private M1BindingSource _BindingSource;

	private bool _RefreshEnabled;

	private bool _SaveData;

	public List<OpenWithDBInfo> Databases
	{
		get
		{
			return _Databases;
		}
		set
		{
			_Databases = value;
		}
	}

	public string TopLevelTable
	{
		get
		{
			return _TopLevelTable;
		}
		set
		{
			_TopLevelTable = value;
		}
	}

	private string _RelatedTable { get; set; }

	public string RelatedTable
	{
		get
		{
			return _RelatedTable;
		}
		set
		{
			_RelatedTable = value;
		}
	}

	public string ViewMode
	{
		get
		{
			return _ViewMode;
		}
		set
		{
			_ViewMode = value;
		}
	}

	public string CurrentField
	{
		get
		{
			return _CurrentField;
		}
		set
		{
			_CurrentField = value;
		}
	}

	public int hWnd
	{
		get
		{
			return _hWnd;
		}
		set
		{
			_hWnd = value;
		}
	}

	public IntPtr Handle
	{
		get
		{
			return _Handle;
		}
		set
		{
			_Handle = value;
		}
	}

	public M1BindingSource BindingSource
	{
		get
		{
			return _BindingSource;
		}
		set
		{
			_BindingSource = value;
		}
	}

	public bool RefreshEnabled
	{
		get
		{
			return _RefreshEnabled;
		}
		set
		{
			_RefreshEnabled = value;
		}
	}

	public bool SaveData
	{
		get
		{
			return _SaveData;
		}
		set
		{
			_SaveData = value;
		}
	}
}
