using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using M1.Extensions;

namespace M1.Core;

public class QueryDefinition : IDisposable
{
	private class JoinInfo
	{
		public string JoinClause = string.Empty;

		public string ChildTable = string.Empty;

		public List<string> ParentFields;

		public List<string> ChildFields;

		public JoinInfo(string joinClause, string primaryPrefix, string primaryPrefixUser)
		{
			string text = joinClause.Replace('\t', ' ').Replace('\r', ' ').Replace('\n', ' ')
				.TrimStart(' ');
			int num = text.IndexOf(' ');
			if (num == -1)
			{
				return;
			}
			ChildTable = text.Substring(0, num);
			text = text.Substring(num + 1).TrimStart(' ');
			num = text.IndexOf("on ");
			if (num != -1)
			{
				text = text.Substring(num + 3).TrimStart(' ');
			}
			ParentFields = new List<string>();
			ChildFields = new List<string>();
			string[] array = text.ToLower().Split(new string[1] { " and " }, StringSplitOptions.RemoveEmptyEntries);
			foreach (string text2 in array)
			{
				num = text2.IndexOf('=');
				if (num != -1)
				{
					string item = text2.Substring(0, num).Trim();
					string text3 = text2.Substring(num + 1).Trim();
					if (text3.StartsWith(primaryPrefix, StringComparison.CurrentCultureIgnoreCase) || (primaryPrefixUser.Length != 0 && text3.StartsWith(primaryPrefixUser, StringComparison.CurrentCultureIgnoreCase)))
					{
						ParentFields.Add(text3);
						ChildFields.Add(item);
					}
					else
					{
						ParentFields.Add(item);
						ChildFields.Add(text3);
					}
				}
			}
		}
	}

	public SqlDataAdapter DataAdapter;

	private DataView _DataView;

	private bool _IsDirty;

	private string originalDescription = string.Empty;

	private bool _UseDataDictionary;

	private bool _UseCurrencyMode;

	private string _FieldList = string.Empty;

	private bool _CustomHeader = true;

	private bool _Custom = true;

	private string _DefaultFieldListProps = string.Empty;

	private string _FromClause = string.Empty;

	private string _WhereClause = string.Empty;

	public string QueryFormat;

	private string primaryTablePrefix = string.Empty;

	private string primaryTablePrefixUser = string.Empty;

	private string _AdditionalFields = string.Empty;

	private string _AdditionalFilterSqlSettings = string.Empty;

	private string _AdditionalFilterAdoSettings = string.Empty;

	private string _AdditionalFilterSql = string.Empty;

	private string _AdditionalFilterAdo = string.Empty;

	private string _DateField = string.Empty;

	private bool _AllUserShareThisDefinition;

	private string _Databases = string.Empty;

	[Browsable(false)]
	public string[] DatabasesResolved = new string[0];

	private string _OrderByGrid = string.Empty;

	private string _OrderByQuery = string.Empty;

	private string _GroupByClause = string.Empty;

	private string _GridID = string.Empty;

	private bool _NoPrimaryTable;

	private string _AdditionalFilter = string.Empty;

	private bool _AdditionalFilterOverride;

	private bool _ShowPreviewPane;

	private short _PreviewPaneSize;

	private bool _PrintOrientationPortrait;

	private byte _GridFreezeColumn;

	private bool _AllowEditingOfGrid;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool? AllowEditingOverride;

	private bool _ShowGroupByBox;

	private bool _ExpandAllGroups;

	private bool _ShowFindBox;

	private bool _LoadGridOnOpen;

	private bool _ShowFindBoxOnStartPage;

	private string _OpenWithID = string.Empty;

	private bool _LockFields;

	private bool _LockDatasets;

	private bool _LockGroupBy;

	private bool _LockOrderBy;

	private bool _LockOptions;

	private string _KPIGroup = string.Empty;

	private short _KPISequence;

	private string _KPIText = string.Empty;

	private string _KPICalc = string.Empty;

	private byte _WGShowOnWeb;

	private short _WGWebSequence;

	private bool _WGRMARequestGrid;

	private bool _WGOrgLocFilter;

	private bool _Style1Bold;

	private bool _Style1Italic;

	private int _Style1BackColor;

	private int _Style1ForeColor;

	private bool _Style2Bold;

	private bool _Style2Italic;

	private int _Style2BackColor;

	private int _Style2ForeColor;

	private bool _Style3Bold;

	private bool _Style3Italic;

	private int _Style3BackColor;

	private int _Style3ForeColor;

	private bool _Style4Bold;

	private bool _Style4Italic;

	private int _Style4BackColor;

	private int _Style4ForeColor;

	private bool _Style5Bold;

	private bool _Style5Italic;

	private int _Style5BackColor;

	private int _Style5ForeColor;

	private string _StyleFormula = string.Empty;

	public List<QueryFilterExpression> SQLExpressionList = new List<QueryFilterExpression>();

	public List<QueryFilterExpression> ADOExpressionList = new List<QueryFilterExpression>();

	private string _GridUserID = string.Empty;

	private bool _TreeVisible;

	private int _TreeSize = 200;

	private string _TreeSettings;

	private string savedWhere = string.Empty;

	[Browsable(false)]
	public bool IsDirty
	{
		get
		{
			return _IsDirty;
		}
		set
		{
			_IsDirty = value;
		}
	}

	[Browsable(false)]
	public DataView DataView
	{
		get
		{
			return _DataView;
		}
		set
		{
			_DataView = value;
		}
	}

	[Browsable(false)]
	[DefaultValue("")]
	public string Description { get; set; }

	public bool ReloadContainer { get; set; }

	[Browsable(false)]
	[DefaultValue(false)]
	public bool UseDataDictionary
	{
		get
		{
			return _UseDataDictionary;
		}
		set
		{
			_UseDataDictionary = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	public bool UseCurrencyMode
	{
		get
		{
			return _UseCurrencyMode;
		}
		set
		{
			if (_UseCurrencyMode != value)
			{
				_UseCurrencyMode = value;
				IsDirty = true;
			}
		}
	}

	[Category("Behavior")]
	[DefaultValue("*")]
	[Description("Indicates the list of fields that will be added to the query before it is sent to the database server.")]
	public string FieldList
	{
		get
		{
			return _FieldList;
		}
		set
		{
			if (_FieldList != value)
			{
				_FieldList = value;
				IsDirty = true;
			}
		}
	}

	[Browsable(false)]
	[DefaultValue(true)]
	[Description("Indicates if this is a custom ddgrid grid definition.")]
	public bool CustomHeader
	{
		get
		{
			return _CustomHeader;
		}
		set
		{
			_CustomHeader = value;
		}
	}

	[Browsable(false)]
	[DefaultValue(true)]
	[Description("Indicates if this is a custom grid definition.")]
	public bool Custom
	{
		get
		{
			return _Custom;
		}
		set
		{
			if (_Custom != value)
			{
				_Custom = value;
				IsDirty = true;
			}
		}
	}

	[Category("Behavior")]
	[DefaultValue("")]
	[Description("Indicates options that should always be added to certain fields in this grid. This allows users to remove fields and add them back and still have the same options (caption, width, search, editable).")]
	public string DefaultFieldListProps
	{
		get
		{
			return _DefaultFieldListProps;
		}
		set
		{
			if (_DefaultFieldListProps != value)
			{
				_DefaultFieldListProps = value;
				IsDirty = true;
			}
		}
	}

	[Category("Behavior")]
	[DefaultValue("")]
	[Description("Indicates the SQL from clause that will be sent to the database server.")]
	public string FromClause
	{
		get
		{
			return _FromClause;
		}
		set
		{
			if (_FromClause != value)
			{
				_FromClause = value;
				IsDirty = true;
			}
		}
	}

	[Category("Behavior")]
	[DefaultValue("")]
	[Description("Indicates the SQL where clause that will be sent to the database server.")]
	public string WhereClause
	{
		get
		{
			return _WhereClause;
		}
		set
		{
			if (_WhereClause != value)
			{
				_WhereClause = value;
				IsDirty = true;
			}
		}
	}

	[Browsable(false)]
	[DefaultValue("")]
	public string KeyFields { get; set; }

	[Category("Behavior")]
	[DefaultValue("")]
	[Browsable(false)]
	[Description("Indicates any additional fields that should be added to the query before sending it to the database server. This is to guarantee certain fields will be included in the grid, even if they are not being shown in the grid.")]
	public string AdditionalFields
	{
		get
		{
			return _AdditionalFields;
		}
		set
		{
			_AdditionalFields = value;
		}
	}

	[Category("Behavior")]
	[DefaultValue("")]
	[Browsable(false)]
	[Description("Indicates the filter settings to be used by the Sql portion of the filter box (the filter before the get data button).")]
	public string AdditionalFilterSqlSettings
	{
		get
		{
			return _AdditionalFilterSqlSettings;
		}
		set
		{
			_AdditionalFilterSqlSettings = value;
		}
	}

	[Category("Behavior")]
	[DefaultValue("")]
	[Browsable(false)]
	[Description("Indicates the filter settings to be used by the Ado portion of the filter box (the filter after the get data button).")]
	public string AdditionalFilterAdoSettings
	{
		get
		{
			return _AdditionalFilterAdoSettings;
		}
		set
		{
			if (_AdditionalFilterAdoSettings != value && _AdditionalFilterAdoSettings.Trim() != value.Trim())
			{
				_AdditionalFilterAdoSettings = value;
				IsDirty = true;
			}
		}
	}

	[Browsable(false)]
	[Category("Behavior")]
	[DefaultValue("")]
	[Description("An additional filter clause to be added to the Sql portion of the filter applied to this grid.")]
	public string AdditionalFilterSql
	{
		get
		{
			return _AdditionalFilterSql;
		}
		set
		{
			_AdditionalFilterSql = value;
		}
	}

	[Browsable(false)]
	[Category("Behavior")]
	[DefaultValue("")]
	[Description("An additional filter clause to be added to the Ado portion of the filter applied to this grid.")]
	public string AdditionalFilterAdo
	{
		get
		{
			return _AdditionalFilterAdo;
		}
		set
		{
			_AdditionalFilterAdo = value;
		}
	}

	[Browsable(false)]
	[DefaultValue("")]
	public string DateField
	{
		get
		{
			return _DateField;
		}
		set
		{
			if (_DateField != value)
			{
				_DateField = value;
				IsDirty = true;
			}
		}
	}

	[Category("Behavior")]
	[DefaultValue(false)]
	[Description("Indicates if all users should share this grid. When true, any changes made by one user will be seen by all users.")]
	public bool AllUserShareThisDefinition
	{
		get
		{
			return _AllUserShareThisDefinition;
		}
		set
		{
			if (_AllUserShareThisDefinition != value)
			{
				_AllUserShareThisDefinition = value;
				IsDirty = true;
			}
		}
	}

	[Category("Behavior")]
	[DefaultValue("")]
	[Description("Indicates the databases to be used when executing this query. The query will be modified to do a union across all the selected databases.")]
	public string Databases
	{
		get
		{
			return _Databases;
		}
		set
		{
			if (_Databases != value)
			{
				_Databases = value;
				IsDirty = true;
			}
		}
	}

	[Category("Behavior")]
	[DefaultValue("")]
	[Description("Indicates the columns that will be sorted in the grid.")]
	public string OrderByGrid
	{
		get
		{
			return _OrderByGrid;
		}
		set
		{
			if (_OrderByGrid != value)
			{
				_OrderByGrid = value;
				IsDirty = true;
			}
		}
	}

	[Category("Behavior")]
	[DefaultValue("")]
	[Description("Indicates the SQL order by clause that will be sent to the database server.")]
	public string OrderByQuery
	{
		get
		{
			return _OrderByQuery;
		}
		set
		{
			if (_OrderByQuery != value)
			{
				_OrderByQuery = value;
				IsDirty = true;
			}
		}
	}

	[Category("Behavior")]
	[DefaultValue("")]
	[Description("Indicates the SQL group by clause that will be sent to the database server.")]
	public string GroupByClause
	{
		get
		{
			return _GroupByClause;
		}
		set
		{
			if (_GroupByClause != value)
			{
				_GroupByClause = value;
				IsDirty = true;
			}
		}
	}

	[Browsable(false)]
	[DefaultValue("")]
	public string TableName { get; set; }

	[Browsable(false)]
	[Category("Behavior")]
	[DefaultValue("")]
	[Description("Indicates the unique id for this grid.")]
	public virtual string GridID
	{
		get
		{
			return _GridID;
		}
		set
		{
			if (_GridID != value)
			{
				_GridID = value;
				IsDirty = true;
			}
		}
	}

	[DefaultValue(false)]
	public bool NoPrimaryTable
	{
		get
		{
			return _NoPrimaryTable;
		}
		set
		{
			if (_NoPrimaryTable != value)
			{
				_NoPrimaryTable = value;
				IsDirty = true;
			}
		}
	}

	[Category("Behavior")]
	[DefaultValue("")]
	[Browsable(false)]
	public string AdditionalFilter
	{
		get
		{
			return _AdditionalFilter;
		}
		set
		{
			_AdditionalFilter = value;
		}
	}

	[Category("Behavior")]
	[DefaultValue(false)]
	[Browsable(false)]
	public bool AdditionalFilterOverride
	{
		get
		{
			return _AdditionalFilterOverride;
		}
		set
		{
			_AdditionalFilterOverride = value;
		}
	}

	[Category("Behavior")]
	[DefaultValue(null)]
	[Browsable(false)]
	public SqlCommand Command { get; set; }

	[Category("Appearance")]
	[DefaultValue(false)]
	[Description("Indicates if the preview pane should be shown when viewing this grid.")]
	public bool ShowPreviewPane
	{
		get
		{
			return _ShowPreviewPane;
		}
		set
		{
			if (_ShowPreviewPane != value)
			{
				_ShowPreviewPane = value;
				IsDirty = true;
			}
		}
	}

	[Category("Appearance")]
	[DefaultValue(0)]
	[Description("This is the height of the preview pane.")]
	public short PreviewPaneSize
	{
		get
		{
			return _PreviewPaneSize;
		}
		set
		{
			if (_PreviewPaneSize != value)
			{
				_PreviewPaneSize = value;
				IsDirty = true;
			}
		}
	}

	[Category("Behavior")]
	[DefaultValue(false)]
	[Description("Indicates if the print from the grid should default to portrait or landscape.")]
	public bool PrintOrientationPortrait
	{
		get
		{
			return _PrintOrientationPortrait;
		}
		set
		{
			if (_PrintOrientationPortrait != value)
			{
				_PrintOrientationPortrait = value;
				IsDirty = true;
			}
		}
	}

	[Category("Appearance")]
	[DefaultValue(0)]
	[Description("All columns to the left of this column number will not be scrolled horizontally.")]
	public byte GridFreezeColumn
	{
		get
		{
			return _GridFreezeColumn;
		}
		set
		{
			if (_GridFreezeColumn != value)
			{
				_GridFreezeColumn = value;
				IsDirty = true;
			}
		}
	}

	[Category("Behavior")]
	[DefaultValue(false)]
	[Description("Indicates if the data in the grid can be edited when viewing the grid. When this is true, the query is changed to load all fields from all the tables in the query, instead of just the fields that are showing.")]
	public bool AllowEditingOfGrid
	{
		get
		{
			return _AllowEditingOfGrid;
		}
		set
		{
			if (_AllowEditingOfGrid != value)
			{
				_AllowEditingOfGrid = value;
				IsDirty = true;
			}
		}
	}

	[Category("Appearance")]
	[DefaultValue(false)]
	[Description("Indicates if the group by box should be shown when viewing this grid. This allows you to select fields to group the data that shows in the grid.")]
	public bool ShowGroupByBox
	{
		get
		{
			return _ShowGroupByBox;
		}
		set
		{
			if (_ShowGroupByBox != value)
			{
				_ShowGroupByBox = value;
				IsDirty = true;
			}
		}
	}

	[Category("Behavior")]
	[DefaultValue(false)]
	[Description("Indicates if the all the groups should be expanded when loading this grid (if there are any groups defined on the grid).")]
	public bool ExpandAllGroups
	{
		get
		{
			return _ExpandAllGroups;
		}
		set
		{
			if (_ExpandAllGroups != value)
			{
				_ExpandAllGroups = value;
				IsDirty = true;
			}
		}
	}

	[Category("Appearance")]
	[DefaultValue(false)]
	[Description("Indicates if the filter box should be shown when viewing this grid.")]
	public bool ShowFindBox
	{
		get
		{
			return _ShowFindBox;
		}
		set
		{
			if (_ShowFindBox != value)
			{
				_ShowFindBox = value;
				IsDirty = true;
			}
		}
	}

	[Category("Behavior")]
	[DefaultValue(false)]
	[Description("Indicates if the grid should open with the data already loaded, or if you must click the get data button to populate the grid.")]
	public bool LoadGridOnOpen
	{
		get
		{
			return _LoadGridOnOpen;
		}
		set
		{
			if (_LoadGridOnOpen != value)
			{
				_LoadGridOnOpen = value;
				IsDirty = true;
			}
		}
	}

	[Category("Behavior")]
	[DefaultValue(false)]
	[Description("Indicates if the filter box for this grid should be visible when viewed on the start page.")]
	public bool ShowFindBoxOnStartPage
	{
		get
		{
			return _ShowFindBoxOnStartPage;
		}
		set
		{
			if (_ShowFindBoxOnStartPage != value)
			{
				_ShowFindBoxOnStartPage = value;
				IsDirty = true;
			}
		}
	}

	[Category("Behavior")]
	[DefaultValue("")]
	[Description("Indicates the open with to be used when double clicking a row in this grid. If empty, it will run the default object associated with the primary table.")]
	public string OpenWithID
	{
		get
		{
			return _OpenWithID;
		}
		set
		{
			if (_OpenWithID != value)
			{
				_OpenWithID = value;
				IsDirty = true;
			}
		}
	}

	[Category("Behavior")]
	[DefaultValue(false)]
	[Description("Indicates if fields can be added or removed from this grid.")]
	public bool LockFields
	{
		get
		{
			return _LockFields;
		}
		set
		{
			if (_LockFields != value)
			{
				_LockFields = value;
				IsDirty = true;
			}
		}
	}

	[Category("Behavior")]
	[DefaultValue(false)]
	[Description("Indicates if the selected datasets of this grid can be changed.")]
	public bool LockDatasets
	{
		get
		{
			return _LockDatasets;
		}
		set
		{
			if (_LockDatasets != value)
			{
				_LockDatasets = value;
				IsDirty = true;
			}
		}
	}

	[Category("Behavior")]
	[DefaultValue(false)]
	[Description("Indicates if the grouping of this grid can be changed.")]
	public bool LockGroupBy
	{
		get
		{
			return _LockGroupBy;
		}
		set
		{
			if (_LockGroupBy != value)
			{
				_LockGroupBy = value;
				IsDirty = true;
			}
		}
	}

	[Category("Behavior")]
	[DefaultValue(false)]
	[Description("Indicates if the sort order of this grid can be changed.")]
	public bool LockOrderBy
	{
		get
		{
			return _LockOrderBy;
		}
		set
		{
			if (_LockOrderBy != value)
			{
				_LockOrderBy = value;
				IsDirty = true;
			}
		}
	}

	[Category("Behavior")]
	[DefaultValue(false)]
	[Description("Indicates if the FilterBox, GroupByBox and PreviewPane options should be disabled for this grid.")]
	public bool LockOptions
	{
		get
		{
			return _LockOptions;
		}
		set
		{
			if (_LockOptions != value)
			{
				_LockOptions = value;
				IsDirty = true;
			}
		}
	}

	[Category("Behavior")]
	[DefaultValue("")]
	[Description("Indicates the group in the KPI section of the start page to show this grid.")]
	public string KPIGroup
	{
		get
		{
			return _KPIGroup;
		}
		set
		{
			if (_KPIGroup != value)
			{
				_KPIGroup = value;
				IsDirty = true;
			}
		}
	}

	[Category("Behavior")]
	[DefaultValue(0)]
	[Description("Indicates the sequence to show this grid within the group of the KPI section of the start page.")]
	public short KPISequence
	{
		get
		{
			return _KPISequence;
		}
		set
		{
			if (_KPISequence != value)
			{
				_KPISequence = value;
				IsDirty = true;
			}
		}
	}

	[Category("Behavior")]
	[DefaultValue("")]
	[Description("Indicates the description to show when viewing this grid in the KPI section of the start page.")]
	public string KPIText
	{
		get
		{
			return _KPIText;
		}
		set
		{
			if (_KPIText != value)
			{
				_KPIText = value;
				IsDirty = true;
			}
		}
	}

	[Category("Behavior")]
	[DefaultValue("")]
	[Description("Indicates any custom Sql calculations to use when showing this grid in the KPI section of the start page.")]
	public string KPICalc
	{
		get
		{
			return _KPICalc;
		}
		set
		{
			if (_KPICalc != value)
			{
				_KPICalc = value;
				IsDirty = true;
			}
		}
	}

	[Category("Behavior")]
	[DefaultValue(0)]
	[Description("Indicates if this grid should be shown in WebGear.")]
	public byte WGShowOnWeb
	{
		get
		{
			return _WGShowOnWeb;
		}
		set
		{
			if (_WGShowOnWeb != value)
			{
				_WGShowOnWeb = value;
				IsDirty = true;
			}
		}
	}

	[Category("Behavior")]
	[DefaultValue(0)]
	[Description("Indicates the ordering sequence of this grid should when it is shown in WebGear.")]
	public short WGWebSequence
	{
		get
		{
			return _WGWebSequence;
		}
		set
		{
			if (_WGWebSequence != value)
			{
				_WGWebSequence = value;
				IsDirty = true;
			}
		}
	}

	[Category("Behavior")]
	[DefaultValue(false)]
	[Description("Indicates if WebGear should treat this as an RMA Request definition. WebGear will add a link to the grid to allow users to create an RMA Request from this grid.")]
	public bool WGRMARequestGrid
	{
		get
		{
			return _WGRMARequestGrid;
		}
		set
		{
			if (_WGRMARequestGrid != value)
			{
				_WGRMARequestGrid = value;
				IsDirty = true;
			}
		}
	}

	[Browsable(false)]
	[Category("Behavior")]
	[DefaultValue(false)]
	public bool WGOrgLocFilter
	{
		get
		{
			return _WGOrgLocFilter;
		}
		set
		{
			if (_WGOrgLocFilter != value)
			{
				_WGOrgLocFilter = value;
				IsDirty = true;
			}
		}
	}

	[Category("Appearance")]
	[DefaultValue(false)]
	[Description("Indicates the bold setting for the predefined style Style1. The style for a given row is determined by the return value of the StyleFormula, which is a string and can be empty, \"STYLE1\", \"STYLE2\", \"STYLE3\", \"STYLE4\", or \"STYLE5\".")]
	public bool Style1Bold
	{
		get
		{
			return _Style1Bold;
		}
		set
		{
			if (_Style1Bold != value)
			{
				_Style1Bold = value;
				IsDirty = true;
			}
		}
	}

	[Category("Appearance")]
	[DefaultValue(false)]
	[Description("Indicates the italic setting for the predefined style Style1. The style for a given row is determined by the return value of the StyleFormula, which is a string and can be empty, \"STYLE1\", \"STYLE2\", \"STYLE3\", \"STYLE4\", or \"STYLE5\".")]
	public bool Style1Italic
	{
		get
		{
			return _Style1Italic;
		}
		set
		{
			if (_Style1Italic != value)
			{
				_Style1Italic = value;
				IsDirty = true;
			}
		}
	}

	[Category("Appearance")]
	[DefaultValue(0)]
	[Description("Indicates the backcolor for the predefined style Style1. The style for a given row is determined by the return value of the StyleFormula, which is a string and can be empty, \"STYLE1\", \"STYLE2\", \"STYLE3\", \"STYLE4\", or \"STYLE5\".")]
	public int Style1BackColor
	{
		get
		{
			return _Style1BackColor;
		}
		set
		{
			if (_Style1BackColor != value)
			{
				_Style1BackColor = value;
				IsDirty = true;
			}
		}
	}

	[Category("Appearance")]
	[DefaultValue(0)]
	[Description("Indicates the forecolor for the predefined style Style1. The style for a given row is determined by the return value of the StyleFormula, which is a string and can be empty, \"STYLE1\", \"STYLE2\", \"STYLE3\", \"STYLE4\", or \"STYLE5\".")]
	public int Style1ForeColor
	{
		get
		{
			return _Style1ForeColor;
		}
		set
		{
			if (_Style1ForeColor != value)
			{
				_Style1ForeColor = value;
				IsDirty = true;
			}
		}
	}

	[Category("Appearance")]
	[DefaultValue(false)]
	[Description("Indicates the bold setting for the predefined style Style2. The style for a given row is determined by the return value of the StyleFormula, which is a string and can be empty, \"STYLE1\", \"STYLE2\", \"STYLE3\", \"STYLE4\", or \"STYLE5\".")]
	public bool Style2Bold
	{
		get
		{
			return _Style2Bold;
		}
		set
		{
			if (_Style2Bold != value)
			{
				_Style2Bold = value;
				IsDirty = true;
			}
		}
	}

	[Category("Appearance")]
	[DefaultValue(false)]
	[Description("Indicates the italic setting for the predefined style Style2. The style for a given row is determined by the return value of the StyleFormula, which is a string and can be empty, \"STYLE1\", \"STYLE2\", \"STYLE3\", \"STYLE4\", or \"STYLE5\".")]
	public bool Style2Italic
	{
		get
		{
			return _Style2Italic;
		}
		set
		{
			if (_Style2Italic != value)
			{
				_Style2Italic = value;
				IsDirty = true;
			}
		}
	}

	[Category("Appearance")]
	[DefaultValue(0)]
	[Description("Indicates the backcolor for the predefined style Style2. The style for a given row is determined by the return value of the StyleFormula, which is a string and can be empty, \"STYLE1\", \"STYLE2\", \"STYLE3\", \"STYLE4\", or \"STYLE5\".")]
	public int Style2BackColor
	{
		get
		{
			return _Style2BackColor;
		}
		set
		{
			if (_Style2BackColor != value)
			{
				_Style2BackColor = value;
				IsDirty = true;
			}
		}
	}

	[Category("Appearance")]
	[DefaultValue(0)]
	[Description("Indicates the forecolor for the predefined style Style2. The style for a given row is determined by the return value of the StyleFormula, which is a string and can be empty, \"STYLE1\", \"STYLE2\", \"STYLE3\", \"STYLE4\", or \"STYLE5\".")]
	public int Style2ForeColor
	{
		get
		{
			return _Style2ForeColor;
		}
		set
		{
			if (_Style2ForeColor != value)
			{
				_Style2ForeColor = value;
				IsDirty = true;
			}
		}
	}

	[Category("Appearance")]
	[DefaultValue(false)]
	[Description("Indicates the bold setting for the predefined style Style3. The style for a given row is determined by the return value of the StyleFormula, which is a string and can be empty, \"STYLE1\", \"STYLE2\", \"STYLE3\", \"STYLE4\", or \"STYLE5\".")]
	public bool Style3Bold
	{
		get
		{
			return _Style3Bold;
		}
		set
		{
			if (_Style3Bold != value)
			{
				_Style3Bold = value;
				IsDirty = true;
			}
		}
	}

	[Category("Appearance")]
	[DefaultValue(false)]
	[Description("Indicates the italic setting for the predefined style Style3. The style for a given row is determined by the return value of the StyleFormula, which is a string and can be empty, \"STYLE1\", \"STYLE2\", \"STYLE3\", \"STYLE4\", or \"STYLE5\".")]
	public bool Style3Italic
	{
		get
		{
			return _Style3Italic;
		}
		set
		{
			if (_Style3Italic != value)
			{
				_Style3Italic = value;
				IsDirty = true;
			}
		}
	}

	[Category("Appearance")]
	[DefaultValue(0)]
	[Description("Indicates the backcolor for the predefined style Style3. The style for a given row is determined by the return value of the StyleFormula, which is a string and can be empty, \"STYLE1\", \"STYLE2\", \"STYLE3\", \"STYLE4\", or \"STYLE5\".")]
	public int Style3BackColor
	{
		get
		{
			return _Style3BackColor;
		}
		set
		{
			if (_Style3BackColor != value)
			{
				_Style3BackColor = value;
				IsDirty = true;
			}
		}
	}

	[Category("Appearance")]
	[DefaultValue(0)]
	[Description("Indicates the forecolor for the predefined style Style3. The style for a given row is determined by the return value of the StyleFormula, which is a string and can be empty, \"STYLE1\", \"STYLE2\", \"STYLE3\", \"STYLE4\", or \"STYLE5\".")]
	public int Style3ForeColor
	{
		get
		{
			return _Style3ForeColor;
		}
		set
		{
			if (_Style3ForeColor != value)
			{
				_Style3ForeColor = value;
				IsDirty = true;
			}
		}
	}

	[Category("Appearance")]
	[DefaultValue(false)]
	[Description("Indicates the bold setting for the predefined style Style4. The style for a given row is determined by the return value of the StyleFormula, which is a string and can be empty, \"STYLE1\", \"STYLE2\", \"STYLE3\", \"STYLE4\", or \"STYLE5\".")]
	public bool Style4Bold
	{
		get
		{
			return _Style4Bold;
		}
		set
		{
			if (_Style4Bold != value)
			{
				_Style4Bold = value;
				IsDirty = true;
			}
		}
	}

	[Category("Appearance")]
	[DefaultValue(false)]
	[Description("Indicates the italic setting for the predefined style Style4. The style for a given row is determined by the return value of the StyleFormula, which is a string and can be empty, \"STYLE1\", \"STYLE2\", \"STYLE3\", \"STYLE4\", or \"STYLE5\".")]
	public bool Style4Italic
	{
		get
		{
			return _Style4Italic;
		}
		set
		{
			if (_Style4Italic != value)
			{
				_Style4Italic = value;
				IsDirty = true;
			}
		}
	}

	[Category("Appearance")]
	[DefaultValue(0)]
	[Description("Indicates the backcolor for the predefined style Style4. The style for a given row is determined by the return value of the StyleFormula, which is a string and can be empty, \"STYLE1\", \"STYLE2\", \"STYLE3\", \"STYLE4\", or \"STYLE5\".")]
	public int Style4BackColor
	{
		get
		{
			return _Style4BackColor;
		}
		set
		{
			if (_Style4BackColor != value)
			{
				_Style4BackColor = value;
				IsDirty = true;
			}
		}
	}

	[Category("Appearance")]
	[DefaultValue(0)]
	[Description("Indicates the forecolor for the predefined style Style4. The style for a given row is determined by the return value of the StyleFormula, which is a string and can be empty, \"STYLE1\", \"STYLE2\", \"STYLE3\", \"STYLE4\", or \"STYLE5\".")]
	public int Style4ForeColor
	{
		get
		{
			return _Style4ForeColor;
		}
		set
		{
			if (_Style4ForeColor != value)
			{
				_Style4ForeColor = value;
				IsDirty = true;
			}
		}
	}

	[Category("Appearance")]
	[DefaultValue(false)]
	[Description("Indicates the bold setting for the predefined style Style5. The style for a given row is determined by the return value of the StyleFormula, which is a string and can be empty, \"STYLE1\", \"STYLE2\", \"STYLE3\", \"STYLE4\", or \"STYLE5\".")]
	public bool Style5Bold
	{
		get
		{
			return _Style5Bold;
		}
		set
		{
			if (_Style5Bold != value)
			{
				_Style5Bold = value;
				IsDirty = true;
			}
		}
	}

	[Category("Appearance")]
	[DefaultValue(false)]
	[Description("Indicates the italic setting for the predefined style Style5. The style for a given row is determined by the return value of the StyleFormula, which is a string and can be empty, \"STYLE1\", \"STYLE2\", \"STYLE3\", \"STYLE4\", or \"STYLE5\".")]
	public bool Style5Italic
	{
		get
		{
			return _Style5Italic;
		}
		set
		{
			if (_Style5Italic != value)
			{
				_Style5Italic = value;
				IsDirty = true;
			}
		}
	}

	[Category("Appearance")]
	[DefaultValue(0)]
	[Description("Indicates the backcolor for the predefined style Style5. The style for a given row is determined by the return value of the StyleFormula, which is a string and can be empty, \"STYLE1\", \"STYLE2\", \"STYLE3\", \"STYLE4\", or \"STYLE5\".")]
	public int Style5BackColor
	{
		get
		{
			return _Style5BackColor;
		}
		set
		{
			if (_Style5BackColor != value)
			{
				_Style5BackColor = value;
				IsDirty = true;
			}
		}
	}

	[Category("Appearance")]
	[DefaultValue(0)]
	[Description("Indicates the forecolor for the predefined style Style5. The style for a given row is determined by the return value of the StyleFormula, which is a string and can be empty, \"STYLE1\", \"STYLE2\", \"STYLE3\", \"STYLE4\", or \"STYLE5\".")]
	public int Style5ForeColor
	{
		get
		{
			return _Style5ForeColor;
		}
		set
		{
			if (_Style5ForeColor != value)
			{
				_Style5ForeColor = value;
				IsDirty = true;
			}
		}
	}

	[Category("Appearance")]
	[DefaultValue("")]
	[Description("The style for a given row is determined by the return value of this formula, which is a string and can be empty, \"STYLE1\", \"STYLE2\", \"STYLE3\", \"STYLE4\", or \"STYLE5\".")]
	public string StyleFormula
	{
		get
		{
			return _StyleFormula;
		}
		set
		{
			if (_StyleFormula != value)
			{
				_StyleFormula = value;
				IsDirty = true;
			}
		}
	}

	[Browsable(true)]
	[Category("Behavior")]
	[DefaultValue("")]
	[Description("Specifies the user that has customized this version of the grid definition.")]
	public string GridUserID
	{
		get
		{
			return _GridUserID;
		}
		set
		{
			if (_GridUserID != value)
			{
				_GridUserID = value;
				IsDirty = true;
			}
		}
	}

	[Browsable(false)]
	public bool TreeVisible
	{
		get
		{
			return _TreeVisible;
		}
		set
		{
			if (_TreeVisible != value)
			{
				_TreeVisible = value;
				IsDirty = true;
			}
		}
	}

	[Browsable(false)]
	public int TreeSize
	{
		get
		{
			return _TreeSize;
		}
		set
		{
			if (_TreeSize != value)
			{
				_TreeSize = value;
				IsDirty = true;
			}
		}
	}

	[Browsable(false)]
	public string TreeSettings
	{
		get
		{
			return _TreeSettings;
		}
		set
		{
			if (_TreeSettings != value)
			{
				_TreeSettings = value;
				IsDirty = true;
			}
		}
	}

	public event EventHandler Disposed;

	public QueryDefinition()
	{
		ResetAllProperties();
	}

	public QueryDefinition(IServiceProvider provider, string gridID, string table)
	{
		ResetAllProperties();
		Load(provider.GetService(typeof(M1User)) as M1User, provider.GetService(typeof(M1Database)) as M1Database, provider.GetService(typeof(M1DataDictionary)) as M1DataDictionary, provider.GetService(typeof(AppContext)) as AppContext, gridID, table);
	}

	public void CopyTo(QueryDefinition copyObj)
	{
		copyObj.GridID = GridID;
		copyObj.Custom = Custom;
		copyObj.CustomHeader = CustomHeader;
		copyObj.UseDataDictionary = UseDataDictionary;
		copyObj.UseCurrencyMode = UseCurrencyMode;
		copyObj.Description = Description;
		copyObj.FieldList = FieldList;
		copyObj.DefaultFieldListProps = DefaultFieldListProps;
		copyObj.FromClause = FromClause;
		copyObj.WhereClause = WhereClause;
		copyObj.KeyFields = KeyFields;
		copyObj.DateField = DateField;
		copyObj.AllUserShareThisDefinition = AllUserShareThisDefinition;
		copyObj.Databases = Databases;
		copyObj.OrderByGrid = OrderByGrid;
		copyObj.OrderByQuery = OrderByQuery;
		copyObj.GroupByClause = GroupByClause;
		copyObj.TableName = TableName;
		copyObj.AdditionalFilter = AdditionalFilter;
		copyObj.AdditionalFilterOverride = AdditionalFilterOverride;
		copyObj.LockFields = LockFields;
		copyObj.LockDatasets = LockDatasets;
		copyObj.LockGroupBy = LockGroupBy;
		copyObj.LockOrderBy = LockOrderBy;
		copyObj.LockOptions = LockOptions;
		copyObj.OpenWithID = OpenWithID;
		copyObj.ShowPreviewPane = ShowPreviewPane;
		copyObj.PreviewPaneSize = PreviewPaneSize;
		copyObj.PrintOrientationPortrait = PrintOrientationPortrait;
		copyObj.GridFreezeColumn = GridFreezeColumn;
		copyObj.AllowEditingOfGrid = AllowEditingOfGrid;
		copyObj.AllowEditingOverride = AllowEditingOverride;
		copyObj.ShowGroupByBox = ShowGroupByBox;
		copyObj.ExpandAllGroups = ExpandAllGroups;
		copyObj.ShowFindBox = ShowFindBox;
		copyObj.LoadGridOnOpen = LoadGridOnOpen;
		copyObj.ShowFindBoxOnStartPage = ShowFindBoxOnStartPage;
		copyObj.AdditionalFilterSqlSettings = AdditionalFilterSqlSettings;
		copyObj.AdditionalFilterAdoSettings = AdditionalFilterAdoSettings;
		copyObj.AdditionalFields = AdditionalFields;
		copyObj.NoPrimaryTable = NoPrimaryTable;
		copyObj.KPIGroup = KPIGroup;
		copyObj.KPISequence = KPISequence;
		copyObj.KPIText = KPIText;
		copyObj.KPICalc = KPICalc;
		copyObj.WGShowOnWeb = WGShowOnWeb;
		copyObj.WGWebSequence = WGWebSequence;
		copyObj.WGRMARequestGrid = WGRMARequestGrid;
		copyObj.WGOrgLocFilter = WGOrgLocFilter;
		copyObj.Style1Bold = Style1Bold;
		copyObj.Style1Italic = Style1Italic;
		copyObj.Style1BackColor = Style1BackColor;
		copyObj.Style1ForeColor = Style1ForeColor;
		copyObj.Style2Bold = Style2Bold;
		copyObj.Style2Italic = Style2Italic;
		copyObj.Style2BackColor = Style2BackColor;
		copyObj.Style2ForeColor = Style2ForeColor;
		copyObj.Style3Bold = Style3Bold;
		copyObj.Style3Italic = Style3Italic;
		copyObj.Style3BackColor = Style3BackColor;
		copyObj.Style3ForeColor = Style3ForeColor;
		copyObj.Style4Bold = Style4Bold;
		copyObj.Style4Italic = Style4Italic;
		copyObj.Style4BackColor = Style4BackColor;
		copyObj.Style4ForeColor = Style4ForeColor;
		copyObj.Style5Bold = Style5Bold;
		copyObj.Style5Italic = Style5Italic;
		copyObj.Style5BackColor = Style5BackColor;
		copyObj.Style5ForeColor = Style5ForeColor;
		copyObj.StyleFormula = StyleFormula;
		copyObj.GridUserID = GridUserID;
	}

	public void ResetAllProperties()
	{
		_UseDataDictionary = false;
		UseCurrencyMode = false;
		Description = string.Empty;
		FieldList = string.Empty;
		DefaultFieldListProps = string.Empty;
		FromClause = string.Empty;
		WhereClause = string.Empty;
		KeyFields = string.Empty;
		DateField = string.Empty;
		AllUserShareThisDefinition = false;
		Databases = string.Empty;
		OrderByGrid = string.Empty;
		OrderByQuery = string.Empty;
		GroupByClause = string.Empty;
		TableName = string.Empty;
		AdditionalFilter = string.Empty;
		AdditionalFilterOverride = false;
		LockFields = false;
		LockDatasets = false;
		LockGroupBy = false;
		LockOrderBy = false;
		LockOptions = false;
		OpenWithID = string.Empty;
		ShowPreviewPane = false;
		PreviewPaneSize = 0;
		PrintOrientationPortrait = false;
		GridFreezeColumn = 0;
		AllowEditingOfGrid = false;
		AllowEditingOverride = null;
		ShowGroupByBox = false;
		ExpandAllGroups = false;
		ShowFindBox = false;
		LoadGridOnOpen = false;
		ShowFindBoxOnStartPage = false;
		NoPrimaryTable = false;
		KPIGroup = string.Empty;
		KPISequence = 0;
		KPIText = string.Empty;
		KPICalc = string.Empty;
		WGShowOnWeb = 0;
		WGWebSequence = 0;
		WGRMARequestGrid = false;
		WGOrgLocFilter = false;
		Style1Bold = false;
		Style1Italic = false;
		Style1BackColor = 0;
		Style1ForeColor = 0;
		Style2Bold = false;
		Style2Italic = false;
		Style2BackColor = 0;
		Style2ForeColor = 0;
		Style3Bold = false;
		Style3Italic = false;
		Style3BackColor = 0;
		Style3ForeColor = 0;
		Style4Bold = false;
		Style4Italic = false;
		Style4BackColor = 0;
		Style4ForeColor = 0;
		Style5Bold = false;
		Style5Italic = false;
		Style5BackColor = 0;
		Style5ForeColor = 0;
		StyleFormula = string.Empty;
	}

	public Dictionary<string, string> SplitFieldListProps(string properties)
	{
		Dictionary<string, string> dictionary = null;
		if (properties.Length != 0)
		{
			List<string> list = M1Util.ParseFieldList(properties, ':');
			if (list.Count != 0)
			{
				dictionary = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);
				foreach (string item in list)
				{
					List<string> list2 = M1Util.ParseFieldList(item, '=');
					if (list2.Count < 2)
					{
						continue;
					}
					string key = list2[0].Trim();
					if (!dictionary.ContainsKey(key))
					{
						string text = list2[1].Trim();
						if (text.Length > 2 && ((text.StartsWith("'") && text.EndsWith("'")) || (text.StartsWith("\"") && text.EndsWith("\""))))
						{
							text = text.Substring(1, text.Length - 2);
						}
						dictionary.Add(key, text);
					}
				}
			}
		}
		return dictionary;
	}

	public Dictionary<string, string> GetFieldListProps(bool includeFieldsWithNoProps)
	{
		return GetFieldListProps(includeFieldsWithNoProps, FieldList);
	}

	public Dictionary<string, string> GetFieldListProps(bool includeFieldsWithNoProps, string fieldList)
	{
		Dictionary<string, string> dictionary = null;
		if (fieldList.Length != 0)
		{
			dictionary = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);
			int num = 0;
			foreach (string item in M1Util.ParseFieldList(fieldList, ','))
			{
				string text = item.Trim();
				num = text.IndexOf(":");
				if (num > 0)
				{
					string value = item.Substring(num);
					text = text.Substring(0, num).Trim();
					num = text.LastIndexOf(" as ", StringComparison.CurrentCultureIgnoreCase);
					if (num != -1)
					{
						text = text.Substring(num + 4).Trim();
					}
					if (!dictionary.ContainsKey(text))
					{
						dictionary.Add(text, value);
					}
				}
				else if (includeFieldsWithNoProps && !dictionary.ContainsKey(text))
				{
					dictionary.Add(text, string.Empty);
				}
			}
		}
		return dictionary;
	}

	public void SetDatabases(M1Database m1Database, AppContext context, string databases)
	{
		if (databases == null)
		{
			Databases = string.Empty;
		}
		else
		{
			Databases = databases;
		}
		if (m1Database == null)
		{
			DatabasesResolved = new string[1] { string.Empty };
		}
		else if (Databases == null || Databases.Length == 0 || Databases.Equals("CURRENT", StringComparison.CurrentCultureIgnoreCase))
		{
			DatabasesResolved = new string[1] { m1Database.ID };
		}
		else if (Databases.Equals("ALL", StringComparison.CurrentCultureIgnoreCase))
		{
			DatabasesResolved = getAllDatabases(context).Split(',');
		}
		else
		{
			DatabasesResolved = Databases.Split(',');
		}
	}

	public void Load(M1User m1User, M1Database m1Database, M1DataDictionary m1DataDictionary, AppContext context, string gridID, string table)
	{
		Load((m1User == null) ? string.Empty : m1User.ID, m1Database, m1DataDictionary, context, gridID, table, loadSpecificRecord: false);
	}

	public void Load(string userID, M1Database m1Database, M1DataDictionary m1DataDictionary, AppContext context, string gridID, string table, bool loadSpecificRecord)
	{
		DataRow dataRow = null;
		if (!string.IsNullOrWhiteSpace(gridID))
		{
			dataRow = getFieldsFromDD(gridID, userID, m1Database, m1DataDictionary, context, loadSpecificRecord);
			if (dataRow == null)
			{
				throw new M1GridIdDoesNotExistException("Grid definition " + gridID + " does not exist in DDGridDetails.");
			}
		}
		Load(userID, m1Database, m1DataDictionary, context, dataRow);
		if (GridUserID.Length == 0 || (GridUserID.Equals("DEFAULT", StringComparison.CurrentCultureIgnoreCase) && !AllUserShareThisDefinition))
		{
			_GridUserID = userID;
		}
		if (dataRow == null)
		{
			_GridID = GridID;
			_FromClause = table;
			TableName = table;
		}
		_IsDirty = false;
	}

	public void Load(string userID, M1Database m1Database, M1DataDictionary m1DataDictionary, AppContext context, DataRow gridRow)
	{
		if (gridRow == null)
		{
			_GridID = string.Empty;
			_Custom = true;
			CustomHeader = true;
			_UseCurrencyMode = false;
			SetDatabases(m1Database, context, string.Empty);
			_FieldList = "*";
			_DefaultFieldListProps = string.Empty;
			_FromClause = string.Empty;
			TableName = string.Empty;
			_WhereClause = string.Empty;
			_DateField = string.Empty;
			Description = "Dynamic Query";
			_GroupByClause = string.Empty;
			_OrderByGrid = string.Empty;
			_OrderByQuery = string.Empty;
			_LockFields = false;
			_LockDatasets = false;
			_LockGroupBy = false;
			_LockOrderBy = false;
			_LockOptions = false;
			_AllUserShareThisDefinition = false;
			_OpenWithID = string.Empty;
			_ShowPreviewPane = false;
			_PreviewPaneSize = 0;
			_PrintOrientationPortrait = false;
			_GridFreezeColumn = 0;
			_AllowEditingOfGrid = true;
			AllowEditingOverride = null;
			_ShowGroupByBox = false;
			_ExpandAllGroups = false;
			_ShowFindBox = false;
			_LoadGridOnOpen = false;
			_ShowFindBoxOnStartPage = false;
			_AdditionalFilterSqlSettings = string.Empty;
			_AdditionalFilterAdoSettings = string.Empty;
			KeyFields = string.Empty;
			AdditionalFields = string.Empty;
			_NoPrimaryTable = false;
			_KPIGroup = string.Empty;
			_KPISequence = 0;
			_KPIText = string.Empty;
			_KPICalc = string.Empty;
			_WGShowOnWeb = 0;
			_WGWebSequence = 0;
			_WGRMARequestGrid = false;
			_WGOrgLocFilter = false;
			_Style1Bold = false;
			_Style1Italic = false;
			_Style1BackColor = 0;
			_Style1ForeColor = 0;
			_Style2Bold = false;
			_Style2Italic = false;
			_Style2BackColor = 0;
			_Style2ForeColor = 0;
			_Style3Bold = false;
			_Style3Italic = false;
			_Style3BackColor = 0;
			_Style3ForeColor = 0;
			_Style4Bold = false;
			_Style4Italic = false;
			_Style4BackColor = 0;
			_Style4ForeColor = 0;
			_Style5Bold = false;
			_Style5Italic = false;
			_Style5BackColor = 0;
			_Style5ForeColor = 0;
			_StyleFormula = string.Empty;
			_GridUserID = userID;
		}
		else
		{
			_GridID = gridRow.Field<string>("djGridID");
			_Custom = gridRow.Field<bool>("dgCustom");
			CustomHeader = gridRow.Field<bool>("djCustom");
			SetDatabases(m1Database, context, gridRow.Field<string>("dgDatasets"));
			_FieldList = gridRow.Field<string>("dgflds");
			if (_FieldList == null)
			{
				_FieldList = string.Empty;
			}
			_DefaultFieldListProps = gridRow.Field<string>("dgreqopt");
			if (_DefaultFieldListProps == null)
			{
				_DefaultFieldListProps = string.Empty;
			}
			_FromClause = gridRow.Field<string>("dgfrom");
			if (_FromClause == null)
			{
				_FromClause = string.Empty;
			}
			_WhereClause = gridRow.Field<string>("dgwher");
			if (_WhereClause == null)
			{
				_WhereClause = string.Empty;
			}
			_DateField = gridRow.Field<string>("dgcaldatef");
			TableName = gridRow.Field<string>("djTable");
			if (TableName == null)
			{
				TableName = string.Empty;
			}
			Description = gridRow.Field<string>("djdesc");
			_GroupByClause = gridRow.Field<string>("dgGrp");
			if (_GroupByClause == null)
			{
				_GroupByClause = string.Empty;
			}
			_OrderByGrid = gridRow.Field<string>("dgOrd");
			if (_OrderByGrid == null)
			{
				_OrderByGrid = string.Empty;
			}
			_OrderByQuery = gridRow.Field<string>("dgSOrd");
			if (_OrderByQuery == null)
			{
				_OrderByQuery = string.Empty;
			}
			_LockFields = gridRow.Field<bool>("dgLockf");
			_LockDatasets = gridRow.Field<bool>("dgLockd");
			_LockGroupBy = gridRow.Field<bool>("dgLockg");
			_LockOrderBy = gridRow.Field<bool>("dgLocks");
			_LockOptions = gridRow.Field<bool>("dgLocko");
			_AllUserShareThisDefinition = gridRow.Field<bool>("dgShar");
			_ShowPreviewPane = gridRow.Field<bool>("dgPrePane");
			_PreviewPaneSize = gridRow.Field<short>("dgPaneSize");
			_PrintOrientationPortrait = gridRow.Field<bool>("dgPortrait");
			_GridFreezeColumn = gridRow.Field<byte>("dgFreeze");
			_AllowEditingOfGrid = gridRow.Field<bool>("dgEdit");
			_ShowGroupByBox = gridRow.Field<bool>("dggbox");
			_ExpandAllGroups = gridRow.Field<bool>("dgexp");
			_AdditionalFilterSqlSettings = gridRow.Field<string>("dgSQLSet");
			if (_AdditionalFilterSqlSettings == null)
			{
				_AdditionalFilterSqlSettings = string.Empty;
			}
			_AdditionalFilterAdoSettings = gridRow.Field<string>("dgADOSet");
			if (_AdditionalFilterAdoSettings == null)
			{
				_AdditionalFilterAdoSettings = string.Empty;
			}
			KeyFields = gridRow.Field<string>("dtKeyFields");
			if (KeyFields == null)
			{
				KeyFields = string.Empty;
			}
			primaryTablePrefix = gridRow.Field<string>("dtPrefix");
			primaryTablePrefixUser = gridRow.Field<string>("dtPrefixUser");
			AdditionalFields = string.Empty;
			if (AdditionalFields == null)
			{
				AdditionalFields = string.Empty;
			}
			_NoPrimaryTable = gridRow.Field<bool>("djNoPrimaryTable");
			SQLExpressionList = getExpressionFilterList(AdditionalFilterSqlSettings);
			ADOExpressionList = getExpressionFilterList(AdditionalFilterAdoSettings);
			_ShowFindBox = gridRow.Field<bool>("dgfbox");
			_LoadGridOnOpen = gridRow.Field<bool>("dglopt");
			_ShowFindBoxOnStartPage = gridRow.Field<bool>("dgfboxsp");
			_KPIGroup = gridRow.Field<string>("dgSPGroup");
			_KPISequence = gridRow.Field<short>("dgSPSeq");
			_KPIText = gridRow.Field<string>("dgSPText");
			_KPICalc = gridRow.Field<string>("dgSPCalc");
			if (_KPICalc == null)
			{
				_KPICalc = string.Empty;
			}
			_WGShowOnWeb = gridRow.Field<byte>("dgWebGrid");
			_WGWebSequence = gridRow.Field<short>("dgWebSeq");
			_WGRMARequestGrid = gridRow.Field<bool>("dgWGRMACS");
			_WGOrgLocFilter = gridRow.Field<bool>("dgWGFilt");
			_Style1Bold = gridRow.Field<bool>("dgS1Bold");
			_Style1Italic = gridRow.Field<bool>("dgS1Italic");
			_Style1BackColor = gridRow.Field<int>("dgS1BColor");
			_Style1ForeColor = gridRow.Field<int>("dgS1FColor");
			_Style2Bold = gridRow.Field<bool>("dgS2Bold");
			_Style2Italic = gridRow.Field<bool>("dgS2Italic");
			_Style2BackColor = gridRow.Field<int>("dgS2BColor");
			_Style2ForeColor = gridRow.Field<int>("dgS2FColor");
			_Style3Bold = gridRow.Field<bool>("dgS3Bold");
			_Style3Italic = gridRow.Field<bool>("dgS3Italic");
			_Style3BackColor = gridRow.Field<int>("dgS3BColor");
			_Style3ForeColor = gridRow.Field<int>("dgS3FColor");
			_Style4Bold = gridRow.Field<bool>("dgS4Bold");
			_Style4Italic = gridRow.Field<bool>("dgS4Italic");
			_Style4BackColor = gridRow.Field<int>("dgS4BColor");
			_Style4ForeColor = gridRow.Field<int>("dgS4FColor");
			_Style5Bold = gridRow.Field<bool>("dgS5Bold");
			_Style5Italic = gridRow.Field<bool>("dgS5Italic");
			_Style5BackColor = gridRow.Field<int>("dgS5BColor");
			_Style5ForeColor = gridRow.Field<int>("dgS5FColor");
			_StyleFormula = gridRow.Field<string>("dgSFormula");
			if (_StyleFormula == null)
			{
				_StyleFormula = string.Empty;
			}
			_OpenWithID = gridRow.Field<string>("dgOpenWithID");
			_TreeVisible = gridRow.Field<bool>("dgTreeVisible");
			_TreeSize = gridRow.Field<int>("dgTreeWidth");
			_TreeSettings = gridRow.Field<string>("dgTreeSettings");
			_GridUserID = gridRow.Field<string>("dgUserID");
			_UseCurrencyMode = gridRow.Field<bool>("dgUseCurrencyMode");
			if (TableName.StartsWith("DD", StringComparison.CurrentCultureIgnoreCase))
			{
				_UseDataDictionary = true;
			}
		}
		originalDescription = Description;
		_IsDirty = false;
	}

	private string getAllDatabases(AppContext context)
	{
		string text = string.Empty;
		foreach (DatabaseInfo installedDatabase in context.InstalledDatabases)
		{
			text = text + "," + installedDatabase.Name;
		}
		return text.Substring(1);
	}

	private DataRow getFieldsFromDD(string gridID, string userID, M1Database m1Database, M1DataDictionary m1DataDictionary, AppContext context, bool loadSpecificRecord)
	{
		gridID = gridID.Trim().ToUpper();
		DataRow result = null;
		if (gridID.Length != 0)
		{
			if (m1DataDictionary == null)
			{
				DataTable dataTable = DesignMode.DesignModeGetDataTable("select djGridID,djTable,djNoPrimaryTable,djCustom,IsNull(dtKeyFields,'') As dtKeyFields,IsNull(dtPrefix,'') As dtPrefix,IsNull(dtPrefixUser,'') As dtPrefixUser,djdesc,IsNull(dgUserID,'') As dgUserID,IsNull(dgflds,'') As dgflds,IsNull(dgreqopt,'') As dgreqopt,IsNull(dgfrom,'') As dgfrom,IsNull(dgwher,'') As dgwher,IsNull(dgGrp,'') As dgGrp, IsNull(dgOrd,'') As dgOrd,IsNull(dgSOrd,'') As dgSOrd,dgCustom,dgLockg,dgLocks,dgLockf,dgLockd,dgLocko,dgPrePane,dgPaneSize,dgPortrait,dgFreeze,dgEdit,dggbox,dgexp,dgshar,dgfbox,dglopt,dgfboxsp,IsNull(dgDatasets,'') As dgDatasets,dgUserID,IsNull(dgSQLSet,'') As dgSQLSet,IsNull(dgADOSet,'') As dgADOSet,dgS1Bold,dgS1Italic,dgS1BColor,dgS1FColor,dgS2Bold,dgS2Italic,dgS2BColor,dgS2FColor,dgS3Bold,dgS3Italic,dgS3BColor,dgS3FColor,dgS4Bold,dgS4Italic,dgS4BColor,dgS4FColor,dgS5Bold,dgS5Italic,dgS5BColor,dgS5FColor,IsNull(dgSFormula,'') As dgSFormula,dgSPGroup,dgSPText,dgSPSeq,IsNull(dgSPCalc,'') As dgSPCalc,dgcaldatef,dgWebGrid,dgWebSeq,dgWgRMACS,dgWGFilt,dgOpenWithID,dgTreeVisible,dgTreeWidth,dgTreeSettings,dgUseCurrencyMode from DDGridDetails With(NoLock) Inner Join DDGrids With(NoLock) On dgGridID=djGridID Left Outer Join DDTables With(NoLock) On djTable = dtTable where dgGridID = " + gridID.ToSql() + " and (dgUserID = 'DEFAULT' or dgUserID = '')");
				if (dataTable.Rows.Count > 0)
				{
					result = dataTable.Rows[0];
				}
			}
			else if (!loadSpecificRecord)
			{
				DataTable dataTable2 = m1DataDictionary.GetDataTable("select djGridID,djTable,djNoPrimaryTable,djCustom,IsNull(dtKeyFields,'') As dtKeyFields,IsNull(dtPrefix,'') As dtPrefix,IsNull(dtPrefixUser,'') As dtPrefixUser," + m1DataDictionary.Language.GetdjDescField(m1Database) + ",IsNull(dgUserID,'') As dgUserID,IsNull(dgflds,'') As dgflds,IsNull(dgreqopt,'') As dgreqopt,IsNull(dgfrom,'') As dgfrom,IsNull(dgwher,'') As dgwher,IsNull(dgGrp,'') As dgGrp, IsNull(dgOrd,'') As dgOrd,IsNull(dgSOrd,'') As dgSOrd,dgCustom,dgLockg,dgLocks,dgLockf,dgLockd,dgLocko,dgPrePane,dgPaneSize,dgPortrait,dgFreeze,dgEdit,dggbox,dgexp,dgshar,dgfbox,dglopt,dgfboxsp,IsNull(dgDatasets,'') AS dgDatasets,dgUserID,IsNull(dgSQLSet,'') As dgSQLSet,IsNull(dgADOSet,'') As dgADOSet,dgS1Bold,dgS1Italic,dgS1BColor,dgS1FColor,dgS2Bold,dgS2Italic,dgS2BColor,dgS2FColor,dgS3Bold,dgS3Italic,dgS3BColor,dgS3FColor,dgS4Bold,dgS4Italic,dgS4BColor,dgS4FColor,dgS5Bold,dgS5Italic,dgS5BColor,dgS5FColor,IsNull(dgSFormula,'') As dgSFormula,dgSPGroup,dgSPText,dgSPSeq,IsNull(dgSPCalc,'') As dgSPCalc,dgcaldatef,dgWebGrid,dgWebSeq,dgWgRMACS,dgWGFilt,dgOpenWithID,dgTreeVisible,dgTreeWidth,dgTreeSettings,dgUseCurrencyMode from DDGridDetails With(NoLock) Inner Join DDGrids With(NoLock) On dgGridID=djGridID Left Outer Join DDTables With(NoLock) On djTable = dtTable " + m1DataDictionary.Language.GetdjDescJoin(m1Database) + " where dgGridID = " + gridID.ToSql() + " and (dgUserID = " + userID.ToSql() + " or dgUserID = 'DEFAULT' or dgUserID = '')");
				if (dataTable2.Rows.Count > 0)
				{
					DataRow[] array = dataTable2.Select("dgUserID = " + userID.ToLinq());
					if (array.Length == 0)
					{
						array = dataTable2.Select("dgUserID = " + "DEFAULT".ToLinq());
						if (array.Length == 0)
						{
							array = dataTable2.Select("dgUserID = ''");
							if (array.Length == 0)
							{
								throw new M1GridIdDoesNotExistException("Grid definition " + gridID + " does not exist in DDGridDetails.");
							}
						}
					}
					result = array[0];
				}
			}
			else
			{
				DataTable dataTable3 = m1DataDictionary.GetDataTable("select djGridID,djTable,djNoPrimaryTable,djCustom,IsNull(dtKeyFields,'') As dtKeyFields,IsNull(dtPrefix,'') As dtPrefix,IsNull(dtPrefixUser,'') As dtPrefixUser," + m1DataDictionary.Language.GetdjDescField(m1Database) + ",IsNull(dgUserID,'') As dgUserID,IsNull(dgflds,'') As dgflds,IsNull(dgreqopt,'') As dgreqopt,IsNull(dgfrom,'') As dgfrom,IsNull(dgwher,'') As dgwher,IsNull(dgGrp,'') As dgGrp, IsNull(dgOrd,'') As dgOrd,IsNull(dgSOrd,'') As dgSOrd,dgCustom,dgLockg,dgLocks,dgLockf,dgLockd,dgLocko,dgPrePane,dgPaneSize,dgPortrait,dgFreeze,dgEdit,dggbox,dgexp,dgshar,dgfbox,dglopt,dgfboxsp,IsNull(dgDatasets,'') AS dgDatasets,dgUserID,IsNull(dgSQLSet,'') As dgSQLSet,IsNull(dgADOSet,'') As dgADOSet,dgS1Bold,dgS1Italic,dgS1BColor,dgS1FColor,dgS2Bold,dgS2Italic,dgS2BColor,dgS2FColor,dgS3Bold,dgS3Italic,dgS3BColor,dgS3FColor,dgS4Bold,dgS4Italic,dgS4BColor,dgS4FColor,dgS5Bold,dgS5Italic,dgS5BColor,dgS5FColor,IsNull(dgSFormula,'') As dgSFormula,dgSPGroup,dgSPText,dgSPSeq,dgSPCalc,dgcaldatef,dgWebGrid,dgWebSeq,dgWgRMACS,dgWGFilt,dgOpenWithID,dgTreeVisible,dgTreeWidth,dgTreeSettings,dgUseCurrencyMode from DDGridDetails With(NoLock) Inner Join DDGrids With(NoLock) On dgGridID=djGridID " + m1DataDictionary.Language.GetdjDescJoin(m1Database) + " Inner Join DDTables With(NoLock) On djTable = dtTable where dgGridID = " + gridID.ToSql() + " and dgUserID = " + userID.ToSql());
				if (dataTable3.Rows.Count > 0)
				{
					result = dataTable3.Rows[0];
				}
			}
		}
		return result;
	}

	public string GetStartPageWhereClause(M1Database m1Database, string groupTotalFieldName, FieldCollection fields)
	{
		string startPageWhereClause = GetStartPageWhereClause(m1Database, groupTotalFieldName, fields, AdditionalFilterSqlSettings);
		if (startPageWhereClause.Length == 0)
		{
			startPageWhereClause = GetStartPageWhereClause(m1Database, groupTotalFieldName, fields, AdditionalFilterAdoSettings);
		}
		return startPageWhereClause;
	}

	public string GetStartPageWhereClause(M1Database m1Database, string groupTotalFieldName, FieldCollection fields, string settings)
	{
		StringBuilder stringBuilder = new StringBuilder();
		List<QueryFilterExpression> expressionFilterList = getExpressionFilterList(settings);
		string empty = string.Empty;
		foreach (QueryFilterExpression item in expressionFilterList)
		{
			empty = ((groupTotalFieldName.Length == 0) ? item.FieldName : groupTotalFieldName);
			if (!fields.Contains(empty) || (!item.Operator.Equals(">") && !item.Operator.Equals(">=")))
			{
				continue;
			}
			FieldDefinition fieldDefinition = fields[empty];
			if (fieldDefinition.FieldType != FieldTypeEnum.Date && fieldDefinition.FieldType != FieldTypeEnum.DateTime)
			{
				continue;
			}
			if (stringBuilder.Length != 0)
			{
				stringBuilder.Append(" And ");
			}
			DateTime d = DateTime.Today;
			if (item.Operator2.Length != 0)
			{
				switch (item.GroupTypeIndicator)
				{
				case "D":
					stringBuilder.Append(empty + item.Operator + d.AddDays(-item.NumberOfGroups).ToSql() + " And ");
					break;
				case "W":
					d = d.AddDays((double)(7 - d.DayOfWeek));
					stringBuilder.Append(empty + item.Operator + d.AddDays(-item.NumberOfGroups * 7).ToSql() + " And ");
					break;
				case "M":
					d = d.AddDays(-(d.Day - 1)).AddMonths(1);
					stringBuilder.Append(empty + item.Operator + d.AddMonths(-item.NumberOfGroups).ToSql() + " And ");
					break;
				case "Q":
					switch (d.Month)
					{
					case 1:
					case 2:
					case 3:
						d = new DateTime(d.Year, 3, 31);
						break;
					case 4:
					case 5:
					case 6:
						d = new DateTime(d.Year, 6, 30);
						break;
					case 7:
					case 8:
					case 9:
						d = new DateTime(d.Year, 9, 30);
						break;
					case 10:
					case 11:
					case 12:
						d = new DateTime(d.Year, 12, 31);
						break;
					}
					stringBuilder.Append(empty + item.Operator + d.AddMonths(-item.NumberOfGroups * 3).ToSql() + " And ");
					break;
				case "Y":
					d = new DateTime(d.Year, 12, 31);
					stringBuilder.Append(empty + item.Operator + d.AddYears(-item.NumberOfGroups).ToSql() + " And ");
					break;
				}
				stringBuilder.Append(empty + item.Operator2 + d.ToSql());
			}
			else
			{
				stringBuilder.Append(empty + item.Operator + d.ToSql());
			}
		}
		return stringBuilder.ToString();
	}

	public string GetConstructedWhereClause(M1Database m1Database, string extraFilter)
	{
		string text = extraFilter;
		if (WhereClause != null && WhereClause.Length != 0)
		{
			text = ((text.Length != 0) ? ("(" + WhereClause + ") AND (" + text + ")") : ("(" + WhereClause + ")"));
		}
		if (AdditionalFilterSql != null && AdditionalFilterSql.Length != 0)
		{
			text = ((text.Length != 0) ? ("(" + text + ") AND (" + AdditionalFilterSql + ")") : ("(" + AdditionalFilterSql + ")"));
		}
		if (AdditionalFilter != null && AdditionalFilter.Length != 0)
		{
			text = ((text.Length != 0) ? ("(" + text + ") AND (" + AdditionalFilter + ")") : ("(" + AdditionalFilter + ")"));
		}
		if (AdditionalFilterOverride)
		{
			text = AdditionalFilter;
		}
		ConstructWhereEventArgs e = new ConstructWhereEventArgs(m1Database, TableName, extraFilter, text);
		m1Database.OnConstructWhere(e);
		return e.WhereClause;
	}

	private string getSqlExpressions(string fieldList)
	{
		StringBuilder stringBuilder = new StringBuilder();
		foreach (KeyValuePair<string, string> fieldListProp in GetFieldListProps(includeFieldsWithNoProps: false, fieldList))
		{
			if (fieldListProp.Value.Length != 0)
			{
				Dictionary<string, string> dictionary = SplitFieldListProps(fieldListProp.Value);
				if (dictionary.ContainsKey("SqlExpr"))
				{
					stringBuilder.Append(dictionary["SqlExpr"] + " As " + fieldListProp.Key);
				}
			}
		}
		if (stringBuilder.Length != 0)
		{
			return "," + stringBuilder.ToString();
		}
		return string.Empty;
	}

	protected string getJoinFields()
	{
		List<string> list = new List<string>();
		bool flag = false;
		string[] array = FromClause.Replace('\r', ' ').Replace('\n', ' ').ToLower()
			.Split(new string[5] { " left ", " right ", " outer ", " join ", " inner " }, StringSplitOptions.RemoveEmptyEntries);
		foreach (string text in array)
		{
			if (text.TrimStart().StartsWith("(select ", StringComparison.CurrentCultureIgnoreCase) && text.IndexOf(" group by ", StringComparison.CurrentCultureIgnoreCase) == -1)
			{
				flag = true;
			}
			if (!flag)
			{
				JoinInfo joinInfo = new JoinInfo(text, primaryTablePrefix, primaryTablePrefixUser);
				if (joinInfo.ChildTable.Length != 0 && joinInfo.ParentFields != null && joinInfo.ParentFields.Count != 0)
				{
					foreach (string parentField in joinInfo.ParentFields)
					{
						if (!parentField.StartsWith(primaryTablePrefix, StringComparison.CurrentCultureIgnoreCase) && (primaryTablePrefixUser.Length == 0 || !parentField.StartsWith(primaryTablePrefixUser, StringComparison.CurrentCultureIgnoreCase)) && !list.Contains(parentField, StringComparer.CurrentCultureIgnoreCase))
						{
							list.Add(parentField);
						}
					}
					foreach (string childField in joinInfo.ChildFields)
					{
						if (!childField.StartsWith(primaryTablePrefix, StringComparison.CurrentCultureIgnoreCase) && (primaryTablePrefixUser.Length == 0 || !childField.StartsWith(primaryTablePrefixUser, StringComparison.CurrentCultureIgnoreCase)) && !list.Contains(childField, StringComparer.CurrentCultureIgnoreCase))
						{
							list.Add(childField);
						}
					}
				}
			}
			if (flag && text.IndexOf(" group by ", StringComparison.CurrentCultureIgnoreCase) != -1)
			{
				flag = false;
			}
		}
		StringBuilder stringBuilder = new StringBuilder();
		foreach (string item in list)
		{
			if (item.IndexOf('.') == -1 && !char.IsNumber(item[0]) && item.IndexOf(' ') == -1 && item.IndexOf('(') == -1)
			{
				if (stringBuilder.Length != 0)
				{
					stringBuilder.Append(",");
				}
				stringBuilder.Append(item);
			}
		}
		return stringBuilder.ToString();
	}

	public bool IsEditable()
	{
		if (AllowEditingOfGrid)
		{
			if (AllowEditingOverride.HasValue)
			{
				return AllowEditingOverride == true;
			}
			return true;
		}
		return false;
	}

	public string GetConstructedSqlQuery(M1Database m1Database, string additionalFields, bool loadNow, string extraFilter)
	{
		if (NoPrimaryTable)
		{
			return AdditionalFilter;
		}
		string selectLoadOption = string.Empty;
		string extraFields = string.Empty;
		string selectNormal = string.Empty;
		string empty = string.Empty;
		if (IsEditable())
		{
			if (TableName.Length != 0)
			{
				empty = TableName + ".*";
				if (FieldList != "*")
				{
					string text = GetFieldsWithNoProps(FieldList, nonPrimaryFieldsOnly: true);
					string joinFields = getJoinFields();
					if (joinFields.Length != 0 && text.Length != 0)
					{
						text = text + "," + joinFields;
					}
					if (text.Length != 0)
					{
						empty = empty + "," + M1DataDictionary.RemoveDuplicateFields(text);
					}
				}
			}
			else
			{
				empty = "*";
			}
		}
		else
		{
			empty = GetFieldsWithNoProps(FieldList);
			if (additionalFields != null && additionalFields.Length != 0 && empty.Length != 0 && empty.Trim() != "*")
			{
				empty = empty + "," + additionalFields;
				empty = M1DataDictionary.RemoveDuplicateFields(empty);
			}
			string fieldsWithNoProps = GetFieldsWithNoProps(DefaultFieldListProps);
			if (!string.IsNullOrWhiteSpace(fieldsWithNoProps))
			{
				empty = empty + "," + fieldsWithNoProps;
				empty = M1DataDictionary.RemoveDuplicateFields(empty);
			}
		}
		if (OrderByQuery.Length == 0 && KeyFields != null)
		{
			m1Database.MakeSelectStatements(empty, FromClause, GetConstructedWhereClause(m1Database, extraFilter), string.Empty, KeyFields, Databases, loadNow, fromGrid: true, ref selectNormal, ref selectLoadOption, ref extraFields);
		}
		else
		{
			m1Database.MakeSelectStatements(empty, FromClause, GetConstructedWhereClause(m1Database, extraFilter), string.Empty, OrderByQuery, Databases, loadNow, fromGrid: true, ref selectNormal, ref selectLoadOption, ref extraFields);
		}
		if (!string.IsNullOrWhiteSpace(QueryFormat))
		{
			selectLoadOption = string.Format(QueryFormat, selectLoadOption);
		}
		return selectLoadOption;
	}

	public string GetSqlExprForField(string field)
	{
		foreach (KeyValuePair<string, string> fieldListProp in GetFieldListProps(includeFieldsWithNoProps: true, FieldList))
		{
			if (!fieldListProp.Key.Equals(field, StringComparison.CurrentCultureIgnoreCase))
			{
				continue;
			}
			if (fieldListProp.Value.Length != 0)
			{
				Dictionary<string, string> dictionary = SplitFieldListProps(fieldListProp.Value);
				if (dictionary.ContainsKey("SqlExpr"))
				{
					return dictionary["SqlExpr"];
				}
			}
			return string.Empty;
		}
		return string.Empty;
	}

	public string GetFieldsWithNoProps(string fieldList)
	{
		return GetFieldsWithNoProps(fieldList, nonPrimaryFieldsOnly: false);
	}

	public string GetFieldsWithNoProps(string fieldList, bool nonPrimaryFieldsOnly)
	{
		StringBuilder stringBuilder = new StringBuilder();
		List<string> list = new List<string>();
		List<string> list2 = new List<string>();
		if (!string.IsNullOrWhiteSpace(fieldList))
		{
			foreach (KeyValuePair<string, string> fieldListProp in GetFieldListProps(includeFieldsWithNoProps: true, fieldList))
			{
				string text = fieldListProp.Key;
				if (nonPrimaryFieldsOnly && ((primaryTablePrefix.Length != 0 && text.StartsWith(primaryTablePrefix, StringComparison.CurrentCultureIgnoreCase)) || (primaryTablePrefixUser.Length != 0 && text.StartsWith(primaryTablePrefixUser, StringComparison.CurrentCultureIgnoreCase))))
				{
					continue;
				}
				list.Add(text);
				if (fieldListProp.Value.Length != 0)
				{
					Dictionary<string, string> dictionary = SplitFieldListProps(fieldListProp.Value);
					if (dictionary.ContainsKey("SqlExpr"))
					{
						text = dictionary["SqlExpr"] + " As " + text;
					}
					if (dictionary.ContainsKey("VBExpr"))
					{
						foreach (string item in new ReferencedFieldsList(dictionary["VBExpr"]))
						{
							if ((!nonPrimaryFieldsOnly || ((primaryTablePrefix.Length == 0 || !item.StartsWith(primaryTablePrefix, StringComparison.CurrentCultureIgnoreCase)) && (primaryTablePrefixUser.Length == 0 || !item.StartsWith(primaryTablePrefixUser, StringComparison.CurrentCultureIgnoreCase)))) && !list2.Contains(item, StringComparer.CurrentCultureIgnoreCase))
							{
								list2.Add(item);
							}
						}
					}
				}
				if (stringBuilder.Length != 0)
				{
					stringBuilder.Append(',');
				}
				stringBuilder.Append(text);
			}
		}
		foreach (string item2 in list2)
		{
			if (!list.Contains(item2, StringComparer.CurrentCultureIgnoreCase))
			{
				if (stringBuilder.Length != 0)
				{
					stringBuilder.Append(',');
				}
				stringBuilder.Append(item2);
			}
		}
		return stringBuilder.ToString();
	}

	public string GetTablesInFromClause(string fromClause)
	{
		StringBuilder stringBuilder = new StringBuilder();
		string empty = string.Empty;
		int num = 0;
		string[] array = fromClause.ToUpper().Split(new string[1] { " JOIN " }, StringSplitOptions.RemoveEmptyEntries);
		for (int i = 0; i < array.Length; i++)
		{
			empty = array[i].Trim();
			if (empty.Length == 0)
			{
				continue;
			}
			for (num = 0; num < empty.Length; num++)
			{
				if (empty[num] == ' ' || empty[num] == ',')
				{
					empty = empty.Substring(0, num);
					break;
				}
			}
			num = empty.LastIndexOf(".");
			if (num > 0)
			{
				empty = empty.Substring(num + 1);
			}
			if (("," + stringBuilder?.ToString() + ",").IndexOf("," + empty + ",") == -1)
			{
				if (stringBuilder.Length != 0)
				{
					stringBuilder.Append(",");
				}
				stringBuilder.Append(empty);
			}
		}
		return stringBuilder.ToString();
	}

	public int GetGridDefinitionCount(M1DataDictionary dataDictionary)
	{
		SqlCommand sqlCommand = dataDictionary.NewSqlCommand("Select Count(*) From DDGridDetails Where dgGridID = @GridID And (dgUserID = '' Or dgUserID = 'DEFAULT')");
		sqlCommand.Parameters.Add(new SqlParameter("@GridID", SqlDbType.NVarChar)).Value = GridID;
		return (int)dataDictionary.ExecuteScalar(sqlCommand);
	}

	public void SaveItemToDDGridDetails(M1DataDictionary dataDictionary, M1User user)
	{
		SaveItemToDDGridDetails(dataDictionary, user.ID);
	}

	public void SaveItemToDDGridDetails(M1DataDictionary dataDictionary, string userID)
	{
		DataTable dataTable = null;
		DataRow dataRow = null;
		if (GridID.Length != 0 && dataDictionary != null)
		{
			dataTable = dataDictionary.GetDataTable("Select * From DDGridDetails Where dgGridID = " + GridID.ToSql() + " and dgUserID = " + userID.ToSql(), fillSchema: true, out var adapter);
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.AddBlankRow();
				IsDirty = false;
			}
			else
			{
				dataRow = dataTable.Rows[0];
			}
			FillRowFromProps(dataRow);
			dataRow.SetField("dgUserID", userID);
			dataRow.SetField("dgCustom", !string.IsNullOrWhiteSpace(userID));
			dataDictionary.UpdateData(new DataRow[1] { dataRow }, adapter);
			if (CustomHeader && !string.IsNullOrWhiteSpace(Description) && !Description.Equals(originalDescription))
			{
				SqlCommand sqlCommand = dataDictionary.NewSqlCommand("Update DDGrids Set djDesc = @Description Where djGridID = @GridID");
				sqlCommand.Parameters.Add(new SqlParameter("@Description", SqlDbType.NVarChar)).Value = Description;
				sqlCommand.Parameters.Add(new SqlParameter("@GridID", SqlDbType.NVarChar)).Value = GridID;
				dataDictionary.ExecuteCommand(sqlCommand);
				originalDescription = Description;
			}
		}
	}

	public void SaveItemToDDGrids(M1DataDictionary dataDictionary, string userID)
	{
		if (dataDictionary != null)
		{
			SqlCommand sqlCommand = dataDictionary.NewSqlCommand("INSERT INTO DDGrids(djGridID,djUserID,djTable,djDesc,djCustom) VALUES(@GridID,@UserID,@Table,@Description,@Custom)");
			sqlCommand.Parameters.Add(new SqlParameter("@GridID", SqlDbType.NVarChar)).Value = GridID;
			sqlCommand.Parameters.Add(new SqlParameter("@UserID", SqlDbType.NVarChar)).Value = userID;
			sqlCommand.Parameters.Add(new SqlParameter("@Table", SqlDbType.NVarChar)).Value = TableName;
			sqlCommand.Parameters.Add(new SqlParameter("@Description", SqlDbType.NVarChar)).Value = Description;
			sqlCommand.Parameters.Add(new SqlParameter("@Custom", SqlDbType.Bit)).Value = Custom;
			dataDictionary.ExecuteCommand(sqlCommand);
		}
	}

	protected void FillRowFromProps(DataRow row)
	{
		_ = string.Empty;
		row.BeginEdit();
		row.SetField("dgUserID", GridUserID);
		row.SetField("dgGridID", GridID);
		row.SetField("dgCustom", Custom);
		row.SetField("dgDatasets", (Databases == null || Databases.Trim().Length == 0) ? null : Databases.Trim());
		row.SetField("dgflds", (FieldList == null || FieldList.Trim().Length == 0) ? "*" : FieldList.Trim());
		row.SetField("dgreqopt", (DefaultFieldListProps == null || DefaultFieldListProps.Trim().Length == 0) ? null : DefaultFieldListProps.Trim());
		row.SetField("dgfrom", FromClause);
		row.SetField("dgwher", (WhereClause == null || WhereClause.Trim().Length == 0) ? null : WhereClause.Trim());
		row.SetField("dgcaldatef", DateField);
		row.SetField("dgGrp", (GroupByClause == null || GroupByClause.Trim().Length == 0) ? null : GroupByClause.Trim());
		row.SetField("dgOrd", (OrderByGrid == null || OrderByGrid.Trim().Length == 0) ? null : OrderByGrid.Trim());
		row.SetField("dgSOrd", (OrderByQuery == null || OrderByQuery.Trim().Length == 0) ? null : OrderByQuery.Trim());
		row.SetField("dgSQLSet", (AdditionalFilterSqlSettings == null || AdditionalFilterSqlSettings.Trim().Length == 0) ? null : AdditionalFilterSqlSettings.Trim());
		row.SetField("dgADOSet", (AdditionalFilterAdoSettings == null || AdditionalFilterAdoSettings.Trim().Length == 0) ? null : AdditionalFilterAdoSettings.Trim());
		row.SetField("dgLockf", LockFields);
		row.SetField("dgLockd", LockDatasets);
		row.SetField("dgLockg", LockGroupBy);
		row.SetField("dgLocks", LockOrderBy);
		row.SetField("dgLocko", LockOptions);
		row.SetField("dgPrePane", ShowPreviewPane);
		row.SetField("dgPaneSize", PreviewPaneSize);
		row.SetField("dgPortrait", PrintOrientationPortrait);
		row.SetField("dgFreeze", GridFreezeColumn);
		row.SetField("dgEdit", AllowEditingOfGrid);
		row.SetField("dggbox", ShowGroupByBox);
		row.SetField("dgexp", ExpandAllGroups);
		row.SetField("dgShar", AllUserShareThisDefinition);
		row.SetField("dgUseCurrencyMode", UseCurrencyMode);
		row.SetField("dgfbox", ShowFindBox);
		row.SetField("dglopt", LoadGridOnOpen);
		row.SetField("dgfboxsp", ShowFindBoxOnStartPage);
		row.SetField("dgSPGroup", KPIGroup);
		row.SetField("dgSPSeq", KPISequence);
		row.SetField("dgSPText", KPIText);
		row.SetField("dgSPCalc", (KPICalc == null || KPICalc.Trim().Length == 0) ? null : KPICalc.Trim());
		row.SetField("dgWebGrid", WGShowOnWeb);
		row.SetField("dgWebSeq", WGWebSequence);
		row.SetField("dgWGRMACS", WGRMARequestGrid);
		row.SetField("dgWGFilt", WGOrgLocFilter);
		row.SetField("dgS1Bold", Style1Bold);
		row.SetField("dgS1Italic", Style1Italic);
		row.SetField("dgS1BColor", Style1BackColor);
		row.SetField("dgS1FColor", Style1ForeColor);
		row.SetField("dgS2Bold", Style2Bold);
		row.SetField("dgS2Italic", Style2Italic);
		row.SetField("dgS2BColor", Style2BackColor);
		row.SetField("dgS2FColor", Style2ForeColor);
		row.SetField("dgS3Bold", Style3Bold);
		row.SetField("dgS3Italic", Style3Italic);
		row.SetField("dgS3BColor", Style3BackColor);
		row.SetField("dgS3FColor", Style3ForeColor);
		row.SetField("dgS4Bold", Style4Bold);
		row.SetField("dgS4Italic", Style4Italic);
		row.SetField("dgS4BColor", Style4BackColor);
		row.SetField("dgS4FColor", Style4ForeColor);
		row.SetField("dgS5Bold", Style5Bold);
		row.SetField("dgS5Italic", Style5Italic);
		row.SetField("dgS5BColor", Style5BackColor);
		row.SetField("dgS5FColor", Style5ForeColor);
		row.SetField("dgSFormula", (StyleFormula == null || StyleFormula.Trim().Length == 0) ? null : StyleFormula.Trim());
		row.SetField("dgOpenWithID", OpenWithID);
		row.SetField("dgTreeVisible", TreeVisible);
		row.SetField("dgTreeWidth", TreeSize);
		row.SetField("dgTreeSettings", TreeSettings);
		row.EndEdit();
	}

	public string RemoveInvalidFields(M1Database Database)
	{
		StringBuilder stringBuilder = new StringBuilder();
		try
		{
			string constructedSqlQuery = GetConstructedSqlQuery(Database, KeyFields, loadNow: false, string.Empty);
			Database.GetDataTable(Database.PrepareQuery(constructedSqlQuery), fillSchema: false, out DataAdapter);
		}
		catch (SqlException ex)
		{
			FieldList = RemoveInvalidFields(FieldList, ex, stringBuilder);
			FromClause = RemoveInvalidFieldsFromClause(FromClause, ex, stringBuilder);
			WhereClause = RemoveInvalidFieldsWhereClause(WhereClause, ex, stringBuilder);
		}
		return stringBuilder.ToString();
	}

	public static string RemoveInvalidFields(string fieldlist, SqlException ex, StringBuilder errorText)
	{
		string[] array = fieldlist.Split(',');
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = array[i].Trim();
		}
		string empty = string.Empty;
		int num = 0;
		foreach (SqlError error in ex.Errors)
		{
			if (error.Number != 207)
			{
				continue;
			}
			string[] array2 = error.Message.Split('\'');
			if (array2.Length <= 1)
			{
				continue;
			}
			if (errorText.Length == 0)
			{
				errorText.Append(array2[1]);
			}
			else
			{
				errorText.Append(", " + array2[1]);
			}
			for (int j = 0; j < array.Length; j++)
			{
				empty = array[j];
				num = empty.IndexOf(':');
				if (num != -1)
				{
					empty = empty.Substring(0, num);
				}
				if (empty.Equals(array2[1], StringComparison.CurrentCultureIgnoreCase))
				{
					array[j] = string.Empty;
				}
			}
		}
		string text = string.Empty;
		for (int k = 0; k < array.Length; k++)
		{
			if (array[k].Length != 0)
			{
				if (text.Length != 0)
				{
					text += ",";
				}
				text += array[k];
			}
		}
		return text;
	}

	public static string RemoveInvalidFieldsFromClause(string fromClause, SqlException ex, StringBuilder errorText)
	{
		string text = fromClause;
		foreach (SqlError error in ex.Errors)
		{
			if (error.Number != 207)
			{
				continue;
			}
			string[] array = error.Message.Split('\'');
			if (array.Length <= 1)
			{
				continue;
			}
			if (errorText.Length == 0)
			{
				errorText.Append(array[1]);
			}
			else
			{
				errorText.Append("," + array[1]);
			}
			string[] array2 = text.Split(new string[1] { "Left Outer Join" }, StringSplitOptions.None);
			string empty = string.Empty;
			text = array2[0].Trim();
			for (int i = 1; i < array2.Length; i++)
			{
				string[] array3 = null;
				empty = array2[i].Trim();
				if (empty.IndexOf(array[1], StringComparison.OrdinalIgnoreCase) > -1)
				{
					string[] array4 = empty.Split(new string[1] { "On" }, StringSplitOptions.None);
					if (array4[1].Contains(")"))
					{
						array3 = array4[1].Split(new string[1] { ")" }, StringSplitOptions.None);
						empty = RemoveErrorFieldFromCondition(array3[0].Trim(), array[1]);
						if (empty != null)
						{
							empty = $"{array4[0].Trim()} On {empty}){array3[1].TrimEnd()}";
						}
					}
					else
					{
						empty = RemoveErrorFieldFromCondition(array4[1].Trim(), array[1]);
						if (empty != null)
						{
							empty = $"{array4[0].Trim()} On {empty}";
						}
					}
				}
				if (empty != null)
				{
					text += $" Left Outer Join {empty}";
				}
				else if (array3 != null)
				{
					text += $"){array3[1].TrimEnd()}";
				}
			}
		}
		RemoveDuplicates(errorText);
		return text;
	}

	public static string RemoveInvalidFieldsWhereClause(string whereClause, SqlException ex, StringBuilder errorText)
	{
		string text = whereClause;
		foreach (SqlError error in ex.Errors)
		{
			if (text == null || error.Number != 207)
			{
				continue;
			}
			string[] array = error.Message.Split('\'');
			if (array.Length > 1)
			{
				if (errorText.Length == 0)
				{
					errorText.Append(array[1]);
				}
				else
				{
					errorText.Append("," + array[1]);
				}
				text = RemoveErrorFieldFromCondition(text, array[1]);
			}
		}
		RemoveDuplicates(errorText);
		return text;
	}

	private static void RemoveDuplicates(StringBuilder inputString)
	{
		List<string> list = new List<string>();
		if (inputString.Length <= 1)
		{
			return;
		}
		list = inputString.ToString().Split(new char[2] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
		inputString.Clear();
		list = list.Distinct(StringComparer.OrdinalIgnoreCase).ToList();
		for (int i = 0; i < list.Count; i++)
		{
			inputString.Append(list[i]);
			if (i < list.Count - 1)
			{
				inputString.Append(", ");
			}
		}
	}

	private static string RemoveErrorFieldFromCondition(string condition, string errorField)
	{
		if (condition.IndexOf(errorField, StringComparison.OrdinalIgnoreCase) < 0)
		{
			return condition;
		}
		string text = null;
		condition = condition.Replace('\n', ' ');
		string[] array = condition.Split(' ');
		string text2 = string.Empty;
		string[] array2 = array;
		foreach (string text3 in array2)
		{
			if (text3.Equals("And", StringComparison.OrdinalIgnoreCase) || text3.Equals("Or", StringComparison.OrdinalIgnoreCase))
			{
				int num = text2.IndexOf(errorField, StringComparison.OrdinalIgnoreCase);
				if (num > -1)
				{
					if (string.IsNullOrEmpty(text) || text.EndsWith("\n\n"))
					{
						text += text2.Substring(0, num);
						int num2 = text2.IndexOf(")", StringComparison.OrdinalIgnoreCase);
						if (num2 > -1)
						{
							text += text2.Substring(num2);
						}
						text2 = string.Empty;
						continue;
					}
					string text4 = text2.Substring(0, num);
					int num3 = text2.IndexOf(")", StringComparison.OrdinalIgnoreCase);
					if (num3 > -1)
					{
						text4 = text2.Substring(num3);
						text += text4.TrimEnd();
						text2 = $" {text3} ";
					}
					else
					{
						text2 = text4;
					}
				}
				else
				{
					text += text2.TrimEnd();
					text2 = $" {text3} ";
				}
				continue;
			}
			int num4 = text3.IndexOf("\n\n");
			if (num4 > -1)
			{
				if (num4 == 0)
				{
					text += string.Format("{0}{1}", text2, "\n\n");
					text2 = $"{text3.Substring(2, text3.Length - 2)} ";
				}
			}
			else
			{
				text2 += $"{text3} ";
			}
		}
		if (!string.IsNullOrEmpty(text2))
		{
			if (text2.IndexOf(errorField, StringComparison.OrdinalIgnoreCase) < 0)
			{
				text += text2.TrimEnd();
			}
			else
			{
				int num5 = text2.IndexOf(")", StringComparison.OrdinalIgnoreCase);
				if (num5 > -1)
				{
					text += text2.Substring(num5);
				}
			}
		}
		return text;
	}

	private List<QueryFilterExpression> getExpressionFilterList(string filterExpressionList)
	{
		List<QueryFilterExpression> list = new List<QueryFilterExpression>();
		if (filterExpressionList != null && filterExpressionList.Length != 0)
		{
			string[] array = filterExpressionList.Split(new char[1] { '\r' }, StringSplitOptions.RemoveEmptyEntries);
			foreach (string filterSetting in array)
			{
				list.Add(new QueryFilterExpression(filterSetting));
			}
		}
		return list;
	}

	private void OnDisposed()
	{
		if (this.Disposed != null)
		{
			this.Disposed(this, EventArgs.Empty);
		}
	}

	public void Dispose()
	{
		OnDisposed();
	}

	public static string ResetToDefault(M1ExceptionAction action)
	{
		string text = ((action.Data == null) ? string.Empty : action.Data.ToString());
		if (text.Length != 0)
		{
			string userID = "DEFAULT";
			QueryDefinition queryDefinition = new QueryDefinition();
			M1DataDictionary m1DataDictionary = action.Provider.GetService(typeof(M1DataDictionary)) as M1DataDictionary;
			AppContext context = action.Provider.GetService(typeof(AppContext)) as AppContext;
			M1User m1User = action.Provider.GetService(typeof(M1User)) as M1User;
			if (queryDefinition.GetGridDefinitionCount(m1DataDictionary) > 1 && MessageBox.Show("There is an M1 default and a custom default set for this grid definition. Would you like to restore the custom default settings? Clicking no will restore the M1 default settings.", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
			{
				userID = string.Empty;
			}
			queryDefinition.Load(userID, null, m1DataDictionary, context, text, string.Empty, loadSpecificRecord: false);
			queryDefinition.GridUserID = m1User.ID;
			queryDefinition.SaveItemToDDGridDetails(m1DataDictionary, m1User.ID);
			return "Reset Completed.";
		}
		return "No grid id specified.";
	}
}
