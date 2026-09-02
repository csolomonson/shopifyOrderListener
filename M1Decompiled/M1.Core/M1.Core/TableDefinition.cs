using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing.Design;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using M1.Core.Script;
using M1.Extensions;

namespace M1.Core;

[ComVisible(true)]
[DebuggerDisplay("{TableName} - {Caption}")]
public class TableDefinition : IDisposable, IProcessCodeBindings
{
	public enum AutoIncrementUserEnum : byte
	{
		None,
		False,
		True
	}

	public enum CurrencyUpdateTypeEnum : byte
	{
		Default,
		UpdateBase,
		UpdateForeign
	}

	public class ExchangeRateChangedEventArgs : FieldDefinition.FieldValueChangedEventArgs
	{
		public bool UpdateBaseCurrencyFields;

		public ExchangeRateChangedEventArgs(FieldDefinition.FieldValueChangedEventArgs e)
			: base(e.Database, e.Row, e.IsCurrentRow, e.PreviousValue, null)
		{
		}
	}

	public class ParentBindingSourceChangedEventArgs : EventArgs
	{
		public M1BindingSource OldBindingSource;

		public M1BindingSource NewBindingSource;

		public ParentBindingSourceChangedEventArgs(M1BindingSource oldBindingSource, M1BindingSource newBindingSource)
		{
			OldBindingSource = oldBindingSource;
			NewBindingSource = newBindingSource;
		}
	}

	public class KeyChangeEventArgs : EventArgs
	{
		public DataRow Row;

		public object[] PreviousValues;
	}

	public class M1ScriptComponentCollection : Dictionary<string, IComponent>, IScriptContainsRef
	{
		public M1ScriptComponentCollection()
			: base((IEqualityComparer<string>)StringComparer.CurrentCultureIgnoreCase)
		{
		}

		public object ContainsRef(string id)
		{
			if (ContainsKey(id))
			{
				return base[id];
			}
			return null;
		}
	}

	private M1BindingSource manuallyLoadedParentBindingSource;

	private ScriptingEventBinding scriptEngine;

	private bool allowEditingOverride;

	protected Guid? _UniqueID;

	private string _AppExtensionID = string.Empty;

	private string _TableName = string.Empty;

	public string TableNameFormatted = string.Empty;

	private string _ParentTableName = string.Empty;

	private string _Caption = string.Empty;

	private string _DefaultFormCollectionID = string.Empty;

	private string _DefaultGridID = string.Empty;

	private string _QuickSearchFields = string.Empty;

	private string _QuickSearchFieldsUser = string.Empty;

	private string _KeyFields = string.Empty;

	[Browsable(false)]
	public string FirstEditableKeyField = string.Empty;

	[Browsable(false)]
	public string[] KeyFieldsArray = new string[0];

	[Browsable(false)]
	public string LastKeyField = string.Empty;

	private string _Module = string.Empty;

	private string _EnterInSequenceField = string.Empty;

	private string _AdditionalField1 = string.Empty;

	private string _AdditionalField2 = string.Empty;

	private string _AdditionalField3 = string.Empty;

	private string _AdditionalFieldUser1 = string.Empty;

	private string _AdditionalFieldUser2 = string.Empty;

	private string _AdditionalFieldUser3 = string.Empty;

	private string _ColorExpression = string.Empty;

	private string _ColorExpressionUser = string.Empty;

	private TableKeyNumericOnlyEnum _NumericOnlyKeys;

	private bool _LastKeyCanBeEmpty;

	private bool _EmptyKeyCanBeEdited;

	private byte _KeysAtThisLevel;

	private bool _AutoIncrement;

	private AutoIncrementUserEnum _AutoIncrementUser;

	private short _IncrementAmount;

	private short _IncrementAmountUser;

	private string _InitialValue = string.Empty;

	private string _OverrideDelete = string.Empty;

	private string _OverrideDeleteEnabledExpression = string.Empty;

	public ReferencedFieldsList OverrideDeleteEnabledExpressionReferencedFields = new ReferencedFieldsList();

	private string _FieldPrefix = string.Empty;

	private string _FieldPrefixUser = string.Empty;

	private string _ReadOnlyExpression = string.Empty;

	public ReferencedFieldsList ReadOnlyExpressionReferencedFields = new ReferencedFieldsList();

	private string _ReadOnlyExpressionUser = string.Empty;

	private string _DisableAddNewExpression = string.Empty;

	private string _DisableAddNewExpressionUser = string.Empty;

	private string _DisableDeleteExpression = string.Empty;

	public ReferencedFieldsList DisableDeleteExpressionReferencedFields = new ReferencedFieldsList();

	private string _DisableDeleteExpressionUser = string.Empty;

	private string _DisableChangeIDExpression = string.Empty;

	private string _DisableChangeIDExpressionUser = string.Empty;

	public bool Custom;

	private string _ChangeDetailIdsFilter = string.Empty;

	private bool _AllowEditingInGrid;

	private bool _AllowChangeId;

	private bool _AllowSaveAs;

	private bool _AllowImport;

	private bool _AllowMailMerge;

	private bool _AllowMap;

	private string _PrimaryContactField = string.Empty;

	[Browsable(true)]
	[DefaultValue("")]
	[Category("Definition")]
	[Description("Indicates the organization contact field in this table to be used for retrieving the default correspondence method on entry screens. The correspondence method will only be checked if this has been set.")]
	private string _ForeignKeyDeleteFilter = string.Empty;

	private string _PromptOnAddField = string.Empty;

	private string _CurrencyModeLocationField = string.Empty;

	private string _CurrencyRateIdField = string.Empty;

	private string _CurrencyCustomRateField = string.Empty;

	private string _CurrencyExchangeRateField = string.Empty;

	private string _DocumentDateField = string.Empty;

	private string _PlantIdField = string.Empty;

	private CurrencyUpdateTypeEnum _CurrencyUpdateType;

	private string _ClosedField = string.Empty;

	private string _ClosedValue = string.Empty;

	private string _ClosedDateField = string.Empty;

	private string _ClosedExtraSetExpression = string.Empty;

	private string _ClosedIncludeOptionText = string.Empty;

	private string _ClosedIncludeOptionSqlExpression = string.Empty;

	private string _ClosedCutoffDateField = string.Empty;

	private string _ClosedRoleCheck = string.Empty;

	private string _ClosedHelpLink = string.Empty;

	private string _PurgeCutoffDateField = string.Empty;

	private string _PurgeHelpLink = string.Empty;

	private TableQuickSearchOption _QuickSearchOption;

	private bool _SqlView;

	private string _UniqueField = string.Empty;

	private string _FieldToCheckOnUpdate = string.Empty;

	public M1DatabaseTableSecurityCollection Databases = new M1DatabaseTableSecurityCollection();

	public List<ChildReferenceTableLink> ChildReferenceTableLinks = new List<ChildReferenceTableLink>();

	public List<ChildCurrencyLink> ChildCurrencyLinks = new List<ChildCurrencyLink>();

	protected List<string> ChildDeleteReferenceTableLinks;

	private ValidationInfo errorList = new ValidationInfo(null, null, null, null);

	public static Dictionary<string, string[]> AlwaysUseRelatedTableForOpenWith = new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase) { 
	{
		"ProductionCalendars",
		new string[1] { "jmlWorkCenterID" }
	} };

	public ReferencedFieldsList ValidCodeReferencedFields = new ReferencedFieldsList();

	private bool getChildRowCountReferenced;

	private bool _DisableAddNewOverride;

	private bool _DisableDeleteOverride;

	private bool _ReadOnlyOverride;

	private M1BindingSource _BindingSource;

	private string _CurrencyRateIdForeign = string.Empty;

	private string _CurrencySymbolForeign = string.Empty;

	private string _ParentBindingTableName;

	private string[] _ParentBindingKeyFieldsArray;

	public FieldDefinition ParentTableLinkField;

	private M1BindingSource _ParentBindingSource;

	public bool SettingKeysToSameAsParent;

	private string loadedParentKeyFields = string.Empty;

	private string[] parentKeyFieldsArray;

	public string TopLevelTable = string.Empty;

	protected string TopLevelDateField = string.Empty;

	protected string TopLevelPlantIdField = string.Empty;

	public string TopLevelKeyFields = string.Empty;

	public bool CurrencyChecked;

	private bool _SaveInProgress;

	public List<string> ValidCodeReferencedBsTables;

	[Browsable(false)]
	[Category("Behavior")]
	[DefaultValue(null)]
	[Description("Indicates the unique id of this field.")]
	[ReadOnly(true)]
	public Guid? UniqueID => _UniqueID;

	[Browsable(true)]
	[Category("Behavior")]
	[DefaultValue("")]
	[Description("Indicates the application extension for this object.")]
	public virtual string AppExtensionID
	{
		get
		{
			return _AppExtensionID;
		}
		set
		{
			_AppExtensionID = value;
		}
	}

	[Browsable(false)]
	[DefaultValue("")]
	public virtual string TableName
	{
		get
		{
			return _TableName;
		}
		set
		{
			_TableName = value;
		}
	}

	[Browsable(false)]
	[DefaultValue("")]
	public virtual string ParentTableName
	{
		get
		{
			return _ParentTableName;
		}
		set
		{
			_ParentTableName = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Category("Definition")]
	[ParenthesizePropertyName(true)]
	[Description("Indicates a short description for this table that will be shown in grids.")]
	public string Caption
	{
		get
		{
			return _Caption;
		}
		set
		{
			_Caption = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Category("Behavior")]
	[Description("Indicates the Form Collection ID to be used when double clicking a record from this table from within a grid control.")]
	public virtual string DefaultFormCollectionID
	{
		get
		{
			return _DefaultFormCollectionID;
		}
		set
		{
			_DefaultFormCollectionID = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Category("Behavior")]
	[Description("Indicates the Grid Definition ID to be used when showing this table if no Grid Definition has been specified. This will be used on entry screen searches that are shown in the table of contents tree, unless a grid id has been specified as part of the Form Collection definition.")]
	public virtual string DefaultGridID
	{
		get
		{
			return _DefaultGridID;
		}
		set
		{
			_DefaultGridID = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Category("Definition")]
	[Description("Indicates the fields in this table that are searched by the quick search provider when finding records when typing in an input field.")]
	[Editor("M1.Forms.Design.DD.Editor.MultiFieldEditor, M1.Forms.Design.DD", typeof(UITypeEditor))]
	public virtual string QuickSearchFields
	{
		get
		{
			return _QuickSearchFields;
		}
		set
		{
			_QuickSearchFields = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Category("Definition")]
	[Description("Indicates the fields in this table that are searched by the quick search provider when finding records when typing in an input field.")]
	[Editor("M1.Forms.Design.DD.Editor.MultiFieldEditor, M1.Forms.Design.DD", typeof(UITypeEditor))]
	public virtual string QuickSearchFieldsUser
	{
		get
		{
			return _QuickSearchFieldsUser;
		}
		set
		{
			_QuickSearchFieldsUser = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Category("Definition")]
	[ParenthesizePropertyName(true)]
	[Description("Indicates the fields in this table that make up the primary (unique) index in the database.")]
	public virtual string KeyFields
	{
		get
		{
			return _KeyFields;
		}
		set
		{
			_KeyFields = value;
			if (_KeyFields.Length == 0)
			{
				KeyFieldsArray = new string[0];
			}
			else
			{
				KeyFieldsArray = _KeyFields.Split(',');
			}
			if (KeyFieldsArray.Length != 0)
			{
				LastKeyField = KeyFieldsArray[KeyFieldsArray.Length - 1];
			}
			else
			{
				LastKeyField = string.Empty;
			}
			checkFirstEditableKey();
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Category("Behavior")]
	[Description("Indicates the module required for using this table. If this module is not available, this table will not be shown.")]
	public string Module
	{
		get
		{
			return _Module;
		}
		set
		{
			_Module = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Category("Behavior")]
	[Description("Indicates a field that must be entered in ascending order (the same order as the last key field).")]
	public virtual string EnterInSequenceField
	{
		get
		{
			return _EnterInSequenceField;
		}
		set
		{
			_EnterInSequenceField = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Category("Definition")]
	[Description("Indicates an additional field to be shown in the entry screen table of contents tree.")]
	public virtual string AdditionalField1
	{
		get
		{
			return _AdditionalField1;
		}
		set
		{
			_AdditionalField1 = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Category("Definition")]
	[Description("Indicates an additional field to be shown in the entry screen table of contents tree.")]
	public virtual string AdditionalField2
	{
		get
		{
			return _AdditionalField2;
		}
		set
		{
			_AdditionalField2 = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Category("Definition")]
	[Description("Indicates an additional field to be shown in the entry screen table of contents tree.")]
	public virtual string AdditionalField3
	{
		get
		{
			return _AdditionalField3;
		}
		set
		{
			_AdditionalField3 = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Category("Definition")]
	[Description("Indicates an additional field to be shown in the entry screen table of contents tree. This overrides the AdditionalField1 property, which will be read only on built-in objects.")]
	public virtual string AdditionalFieldUser1
	{
		get
		{
			return _AdditionalFieldUser1;
		}
		set
		{
			_AdditionalFieldUser1 = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Category("Definition")]
	[Description("Indicates an additional field to be shown in the entry screen table of contents tree. This overrides the AdditionalField2 property, which will be read only on built-in objects.")]
	public virtual string AdditionalFieldUser2
	{
		get
		{
			return _AdditionalFieldUser2;
		}
		set
		{
			_AdditionalFieldUser2 = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Category("Definition")]
	[Description("Indicates an additional field to be shown in the entry screen table of contents tree. This overrides the AdditionalField3 property, which will be read only on built-in objects.")]
	public virtual string AdditionalFieldUser3
	{
		get
		{
			return _AdditionalFieldUser3;
		}
		set
		{
			_AdditionalFieldUser3 = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Category("Expression")]
	[Description("VBScript expression that is evaluated to get the RGB color value to be used for the foreground color of the node in the entry screen table of contents tree for the current record.")]
	public virtual string ColorExpression
	{
		get
		{
			return _ColorExpression;
		}
		set
		{
			_ColorExpression = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Category("Expression")]
	[Description("VBScript expression that is evaluated to get the RGB color value to be used for the foreground color of the node in the entry screen table of contents tree for the current record. This overrides the ColorExpression property, which will be read only on built-in objects.")]
	public virtual string ColorExpressionUser
	{
		get
		{
			return _ColorExpressionUser;
		}
		set
		{
			_ColorExpressionUser = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(0)]
	[Category("Behavior")]
	[Description("Indicates if the last key field for this table should be treated as a number, with options to treat as a number for next id only or for next id and user input.")]
	public TableKeyNumericOnlyEnum NumericOnlyKeys
	{
		get
		{
			return _NumericOnlyKeys;
		}
		set
		{
			_NumericOnlyKeys = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[Category("Behavior")]
	[Description("Indicates if the last key field for this table can be empty.")]
	public bool LastKeyCanBeEmpty
	{
		get
		{
			return _LastKeyCanBeEmpty;
		}
		set
		{
			_LastKeyCanBeEmpty = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[Category("Behavior")]
	[Description("Indicates if a row from this table can be edited if the last key is empty.")]
	public bool EmptyKeyCanBeEdited
	{
		get
		{
			return _EmptyKeyCanBeEdited;
		}
		set
		{
			_EmptyKeyCanBeEdited = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[Category("Behavior")]
	[Description("Indicates if this table has multiple key fields defined at this level. Only need to set this when the value is greater than one.")]
	public byte KeysAtThisLevel
	{
		get
		{
			return _KeysAtThisLevel;
		}
		set
		{
			_KeysAtThisLevel = value;
			checkFirstEditableKey();
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[Category("Behavior")]
	[Description("Indicates if the last key field for this table should be automatically incremented when creating a new record in an entry screen. If not set, you will have to click Next ID or type in a value to fill in the last key field.")]
	public bool AutoIncrement
	{
		get
		{
			return _AutoIncrement;
		}
		set
		{
			_AutoIncrement = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(AutoIncrementUserEnum.None)]
	[Category("Behavior")]
	[Description("Indicates if the last key field for this table should be automatically incremented when creating a new record in an entry screen. If set, this overrides the AutoIncrement property, which will be read only on built-in objects.")]
	public AutoIncrementUserEnum AutoIncrementUser
	{
		get
		{
			return _AutoIncrementUser;
		}
		set
		{
			_AutoIncrementUser = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(0)]
	[Category("Behavior")]
	[Description("Indicates how many numbers should be skipped when using next id.")]
	public short IncrementAmount
	{
		get
		{
			return _IncrementAmount;
		}
		set
		{
			_IncrementAmount = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(0)]
	[Category("Behavior")]
	[Description("Indicates how many numbers should be skipped when using next id. This overrides the IncrementAmount property, which will be read only on built-in objects.")]
	public short IncrementAmountUser
	{
		get
		{
			return _IncrementAmountUser;
		}
		set
		{
			_IncrementAmountUser = value;
		}
	}

	[Browsable(false)]
	public bool AutoIncrementResolved
	{
		get
		{
			if (AutoIncrementUser == AutoIncrementUserEnum.False)
			{
				return false;
			}
			if (AutoIncrementUser == AutoIncrementUserEnum.True)
			{
				return true;
			}
			return AutoIncrement;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Category("Behavior")]
	[Description("Indicates the value to be used for next id the first time next id is used for this table.")]
	public string InitialValue
	{
		get
		{
			return _InitialValue;
		}
		set
		{
			_InitialValue = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Category("Behavior")]
	[Description("Specifies the open with id to run when the delete button is pressed on the entry form. Use the open with EnabledExpression to control when to run the override or the standard delete.")]
	public virtual string OverrideDelete
	{
		get
		{
			return _OverrideDelete;
		}
		set
		{
			_OverrideDelete = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Category("Expression")]
	[Description("The loaded OverrideDeleteEnabledExpression to control when to run the override or the standard delete.")]
	public virtual string OverrideDeleteEnabledExpression
	{
		get
		{
			return _OverrideDeleteEnabledExpression;
		}
		set
		{
			_OverrideDeleteEnabledExpression = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Category("Definition")]
	[ParenthesizePropertyName(true)]
	[Description("Indicates the three letter prefix that all standard fields in this table must have. For custom tables this must start with U.")]
	public virtual string FieldPrefix
	{
		get
		{
			return _FieldPrefix;
		}
		set
		{
			_FieldPrefix = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Category("Definition")]
	[ParenthesizePropertyName(true)]
	[Description("Indicates the four letter prefix that all custom fields in this table must have. This is generally U plus the standard prefix. This must be set to be able to add custom fields to this table.")]
	public virtual string FieldPrefixUser
	{
		get
		{
			return _FieldPrefixUser;
		}
		set
		{
			_FieldPrefixUser = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Category("Expression")]
	[Description("VBScript expression that is used to determine if a row in this table should be readonly. This allows you to make a row readonly based on values in the row by accessing the Fields() collection. This will be evaluated and Or'd with the ReadOnlyExpressionUser to determine a value.")]
	public virtual string ReadOnlyExpression
	{
		get
		{
			return _ReadOnlyExpression;
		}
		set
		{
			_ReadOnlyExpression = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Category("Expression")]
	[Description("VBScript expression that is used to determine if a row in this table should be readonly. This allows you to make a row readonly based on values in the row by accessing the Fields() collection. This will be evaluated and Or'd with the ReadOnlyExpression to determine a value.")]
	public virtual string ReadOnlyExpressionUser
	{
		get
		{
			return _ReadOnlyExpressionUser;
		}
		set
		{
			_ReadOnlyExpressionUser = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Category("Expression")]
	[Description("VBScript expression that indicates if the add button on the entry screens and grids should be disabled. This will be evaluated and Or'd with the DisableAddNewExpressionUser to determine a value.")]
	public virtual string DisableAddNewExpression
	{
		get
		{
			return _DisableAddNewExpression;
		}
		set
		{
			_DisableAddNewExpression = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Category("Expression")]
	[Description("VBScript expression that indicates if the add button on the entry screens and grids should be disabled. This will be evaluated and Or'd with the DisableAddNewExpression to determine a value.")]
	public virtual string DisableAddNewExpressionUser
	{
		get
		{
			return _DisableAddNewExpressionUser;
		}
		set
		{
			_DisableAddNewExpressionUser = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Category("Expression")]
	[Description("VBScript expression that indicates if the delete button on the entry screens and grids should be disabled. This will be evaluated and Or'd with the DisableDeleteExpressionUser to determine a value.")]
	public virtual string DisableDeleteExpression
	{
		get
		{
			return _DisableDeleteExpression;
		}
		set
		{
			_DisableDeleteExpression = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Category("Expression")]
	[Description("VBScript expression that indicates if the delete button on the entry screens and grids should be disabled. This will be evaluated and Or'd with the DisableDeleteExpression to determine a value.")]
	public virtual string DisableDeleteExpressionUser
	{
		get
		{
			return _DisableDeleteExpressionUser;
		}
		set
		{
			_DisableDeleteExpressionUser = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Category("Expression")]
	[Description("VBScript expression that indicates if the change id button on the entry screens should be disabled. This will be evaluated and Or'd with the DisableChangeIDExpressionUser to determine a value.")]
	public virtual string DisableChangeIDExpression
	{
		get
		{
			return _DisableChangeIDExpression;
		}
		set
		{
			_DisableChangeIDExpression = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Category("Expression")]
	[Description("VBScript expression that indicates if the change id button on the entry screens should be disabled. This will be evaluated and Or'd with the DisableChangeIDExpression to determine a value.")]
	public virtual string DisableChangeIDExpressionUser
	{
		get
		{
			return _DisableChangeIDExpressionUser;
		}
		set
		{
			_DisableChangeIDExpressionUser = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Category("Behavior")]
	[Description("This sql expression is added to the where clause of the change detail ids query. This is used to ignore the final assembly on the various assemblies tables.")]
	public virtual string ChangeDetailIdsFilter
	{
		get
		{
			return _ChangeDetailIdsFilter;
		}
		set
		{
			_ChangeDetailIdsFilter = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[Category("Behavior")]
	[Description("Indicates if this table may be edited from with searches. This may be turned off when validation or save logic is not available in the data dictionary triggers code.")]
	public bool AllowEditingInGrid
	{
		get
		{
			return _AllowEditingInGrid;
		}
		set
		{
			_AllowEditingInGrid = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[Category("Behavior")]
	[Description("Indicates if the \"Change Id\" functionality in entry screens should be available for this table. This may be turned off if the standard code would cause data to be changed to inappropriate values.")]
	public bool AllowChangeId
	{
		get
		{
			return _AllowChangeId;
		}
		set
		{
			_AllowChangeId = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[Category("Behavior")]
	[Description("Indicates if the \"Save As\" functionality in entry screens should be available for this table. This may be turned off if the standard code would cause data to be changed to inappropriate values.")]
	public bool AllowSaveAs
	{
		get
		{
			return _AllowSaveAs;
		}
		set
		{
			_AllowSaveAs = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[Category("Behavior")]
	[Description("Indicates if the DataMap import functionality should be available for this table. This may be turned off if the standard code would cause data to be changed to inappropriate values.")]
	public bool AllowImport
	{
		get
		{
			return _AllowImport;
		}
		set
		{
			_AllowImport = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[Category("Behavior")]
	[Description("Indicates if the mail merge functionality in entry screens should be available for this table. This is generally only turned on for tables that have an organization id field in the table or one of it's parent tables.")]
	public bool AllowMailMerge
	{
		get
		{
			return _AllowMailMerge;
		}
		set
		{
			_AllowMailMerge = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[Category("Behavior")]
	[Description("Indicates if the map functionality in entry screens should be available for this table. This is generally only turned on for tables that have an organization id field in the table or one of it's parent tables.")]
	public bool AllowMap
	{
		get
		{
			return _AllowMap;
		}
		set
		{
			_AllowMap = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Category("Behavior")]
	[Description("This sql expression is added to the where clause of the where used query. This is used to ignore closed rows when checking to see if an item is being used throughout the system.")]
	public virtual string ForeignKeyDeleteFilter
	{
		get
		{
			return _ForeignKeyDeleteFilter;
		}
		set
		{
			_ForeignKeyDeleteFilter = value;
		}
	}

	public virtual string PrimaryContactField
	{
		get
		{
			return _PrimaryContactField;
		}
		set
		{
			_PrimaryContactField = value;
		}
	}

	[Browsable(false)]
	[DefaultValue("")]
	[Category("Behavior")]
	[Description("This is not currently used.")]
	public string PromptOnAddField
	{
		get
		{
			return _PromptOnAddField;
		}
		set
		{
			_PromptOnAddField = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Category("Currency")]
	[Description("Indicates the location id field in the table that will be used to retrieve the currency rate id for this row. If the rate id does not match the default currency rate id for the database, the information for the row will be shown in the Foreign currency by default.")]
	public virtual string CurrencyModeLocationField
	{
		get
		{
			return _CurrencyModeLocationField;
		}
		set
		{
			_CurrencyModeLocationField = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Category("Currency")]
	[Description("Indicates the currency rate id field for this table. This must be set to enable automatic exchange rate translation for currency fields in this table or any child tables.")]
	public virtual string CurrencyRateIdField
	{
		get
		{
			return _CurrencyRateIdField;
		}
		set
		{
			_CurrencyRateIdField = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Category("Currency")]
	[Description("Indicates the currency custom rate field for this table. This must be set to enable automatic exchange rate translation for currency fields in this table or any child tables.")]
	public virtual string CurrencyCustomRateField
	{
		get
		{
			return _CurrencyCustomRateField;
		}
		set
		{
			_CurrencyCustomRateField = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Category("Currency")]
	[Description("Indicates the currency exhange rate field for this table. This must be set to enable automatic exchange rate translation for currency fields in this table or any child tables.")]
	public virtual string CurrencyExchangeRateField
	{
		get
		{
			return _CurrencyExchangeRateField;
		}
		set
		{
			_CurrencyExchangeRateField = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Category("Currency")]
	[Description("Indicates the date field to be used for the transaction date for this row. This is used for retrieving the exchange rate as well as the current part revision. This must be set to enable automatic exchange rate translation for currency fields in this table or any child tables.")]
	public virtual string DocumentDateField
	{
		get
		{
			return _DocumentDateField;
		}
		set
		{
			_DocumentDateField = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Category("Currency")]
	[Description("Indicates the field to be used for returning the plant id for this row. This is used for retrieving the current part revision.")]
	public virtual string DocumentPlantIdField
	{
		get
		{
			return _PlantIdField;
		}
		set
		{
			_PlantIdField = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Category("Currency")]
	[Description("Indicates whether the base or foreign field should get updated when refreshing the currency fields after the exchange rate changes.")]
	public virtual CurrencyUpdateTypeEnum CurrencyUpdateType
	{
		get
		{
			return _CurrencyUpdateType;
		}
		set
		{
			_CurrencyUpdateType = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Category("Close")]
	[Description("Indicates the field that specifies if this row is closed.")]
	public virtual string ClosedField
	{
		get
		{
			return _ClosedField;
		}
		set
		{
			_ClosedField = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Category("Close")]
	[Description("Indicates the value the ClosedField needs to be to evaluate as closed. Boolean fields will default to True being closed. For any other types or values, you must specify the value here. The value must be a valid sql expression.")]
	public virtual string ClosedValue
	{
		get
		{
			return _ClosedValue;
		}
		set
		{
			_ClosedValue = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Category("Close")]
	[Description("Indicates the date field that specifies when this row has been closed.")]
	public virtual string ClosedDateField
	{
		get
		{
			return _ClosedDateField;
		}
		set
		{
			_ClosedDateField = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Category("Close")]
	[Description("An expression that is added to the set part of the update query that is run when closing rows for this table. The App object can be referenced using the {!expr!} syntax.")]
	public virtual string ClosedExtraSetExpression
	{
		get
		{
			return _ClosedExtraSetExpression;
		}
		set
		{
			_ClosedExtraSetExpression = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Category("Close")]
	[Description("The text to put on a checkbox for prompting if the ClosedIncludeOptionSqlExpression should be used in the close processing.")]
	public virtual string ClosedIncludeOptionText
	{
		get
		{
			return _ClosedIncludeOptionText;
		}
		set
		{
			_ClosedIncludeOptionText = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Category("Close")]
	[Description("An expression that is added to the where clause of the update query when closing rows for this table. This can be used to optionally exclude some rows in the processing.")]
	public virtual string ClosedIncludeOptionSqlExpression
	{
		get
		{
			return _ClosedIncludeOptionSqlExpression;
		}
		set
		{
			_ClosedIncludeOptionSqlExpression = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Category("Close")]
	[Description("Indicates the date field that is prompted for and used as the cutoff in the close processing.")]
	public virtual string ClosedCutoffDateField
	{
		get
		{
			return _ClosedCutoffDateField;
		}
		set
		{
			_ClosedCutoffDateField = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Category("Close")]
	[Description("Indicates a component security role id to be checked before running the close processing.")]
	public virtual string ClosedRoleCheck
	{
		get
		{
			return _ClosedRoleCheck;
		}
		set
		{
			_ClosedRoleCheck = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Category("Close")]
	[Description("Indicates the help link for the close processing form for this table.")]
	public virtual string ClosedHelpLink
	{
		get
		{
			return _ClosedHelpLink;
		}
		set
		{
			_ClosedHelpLink = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Category("Close")]
	[Description("Indicates the date field that is prompted for and used as the cutoff in the purge processing.")]
	public virtual string PurgeCutoffDateField
	{
		get
		{
			return _PurgeCutoffDateField;
		}
		set
		{
			_PurgeCutoffDateField = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Category("Close")]
	[Description("Indicates the help link for the purge processing form for this table.")]
	public virtual string PurgeHelpLink
	{
		get
		{
			return _PurgeHelpLink;
		}
		set
		{
			_PurgeHelpLink = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(TableQuickSearchOption.None)]
	[Category("Behavior")]
	[Description("Indicates if the quick search should ignore this table.")]
	public virtual TableQuickSearchOption QuickSearchOption
	{
		get
		{
			return _QuickSearchOption;
		}
		set
		{
			_QuickSearchOption = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[Category("Definition")]
	[Description("Indicates if this is a virtual table defined by a Sql Select statement. When the table is created it will use the query specified in the SqlViewDefinition property to run a Create View command on the Sql Server.")]
	public bool SqlView
	{
		get
		{
			return _SqlView;
		}
		set
		{
			_SqlView = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Category("Definition")]
	[Description("Indicates the UniqueIdentifier field for this table. Generally this field has a name ending in UniqueID. This must be set to make change logging available for this table.")]
	public virtual string UniqueField
	{
		get
		{
			return _UniqueField;
		}
		set
		{
			_UniqueField = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Category("Behavior")]
	[Description("Indicates the field that should be checked on the table before doing an update. If the original row value for this field is false and the current database value is true, then do not allow the save. Generally used for posted/closed flags. Also checks the parent to see if the record has been deleted, and stops the save then as well.")]
	public virtual string FieldToCheckOnUpdate
	{
		get
		{
			return _FieldToCheckOnUpdate;
		}
		set
		{
			_FieldToCheckOnUpdate = value;
		}
	}

	[Browsable(false)]
	public bool ReadOnlyResolved { get; private set; }

	[Browsable(false)]
	public bool NoAccessResolved { get; private set; }

	[Browsable(false)]
	public bool DisableAddNewResolved { get; private set; }

	[Browsable(false)]
	public bool DisableDeleteResolved { get; private set; }

	[Browsable(false)]
	public bool ReadOnlyExpressionResolved { get; private set; }

	[Browsable(false)]
	public bool OverrideDeleteResolved { get; private set; }

	public bool IsDefaultFormCollectionIDSerialOrLotExplorer
	{
		get
		{
			if (!string.IsNullOrEmpty(DefaultFormCollectionID))
			{
				if (!DefaultFormCollectionID.Equals("serialnumberexplorer", StringComparison.CurrentCultureIgnoreCase))
				{
					return DefaultFormCollectionID.Equals("lotnumberexplorer", StringComparison.CurrentCultureIgnoreCase);
				}
				return true;
			}
			return false;
		}
	}

	[Browsable(false)]
	public M1BindingSource BindingSource
	{
		get
		{
			return _BindingSource;
		}
		set
		{
			if (_BindingSource != null)
			{
				_BindingSource.RowUpdateDeleteBefore -= BindingSource_RowUpdateDeleteBefore;
				_BindingSource.RowUpdateDeleteAfter -= BindingSource_RowUpdateDeleteAfter;
				_BindingSource.RowUpdateAddBefore -= BindingSource_RowUpdateAddBefore;
				_BindingSource.RowUpdateSaveBefore -= BindingSource_RowUpdateSaveBefore;
				_BindingSource.RowUpdateAddAfter -= BindingSource_RowUpdateAddAfter;
				_BindingSource.RowUpdateSaveAfter -= BindingSource_RowUpdateSaveAfter;
				_BindingSource.CurrentChanged -= _BindingSource_CurrentChanged;
				_BindingSource.SaveDataStarted -= BindingSource_SaveDataStarted_UpdateManuallyAdded;
				_BindingSource.SaveDataCompleted -= BindingSource_SaveDataCompleted_UpdateManuallyAdded;
				_BindingSource.RowActivated -= BindingSource_RowActivated_UpdateManuallyAdded;
				_BindingSource.CacheCleared -= BindingSource_CacheCleared_UpdateManuallyAdded;
				_BindingSource.EditCancelled -= BindingSource_EditCancelled_UpdateManuallyAdded;
			}
			_BindingSource = value;
		}
	}

	[Browsable(false)]
	public string CurrencyRateIdForeign => _CurrencyRateIdForeign;

	[Browsable(false)]
	public string CurrencySymbolForeign
	{
		get
		{
			return _CurrencySymbolForeign;
		}
		set
		{
			if (_CurrencySymbolForeign != value)
			{
				_CurrencySymbolForeign = value;
			}
		}
	}

	[Browsable(false)]
	public M1BindingSource ParentBindingSource
	{
		get
		{
			return _ParentBindingSource;
		}
		set
		{
			if (_ParentBindingSource != value)
			{
				ParentBindingSourceChangedEventArgs e = new ParentBindingSourceChangedEventArgs(_ParentBindingSource, value);
				_ParentBindingSource = value;
				OnParentBindingSourceChanged(e);
			}
		}
	}

	[Browsable(false)]
	public int EditMode
	{
		get
		{
			int result = 0;
			DataRow currentDataRowForProcessing = GetCurrentDataRowForProcessing();
			if (currentDataRowForProcessing != null)
			{
				result = ((currentDataRowForProcessing.RowState == DataRowState.Added) ? 2 : ((currentDataRowForProcessing.RowState != DataRowState.Detached) ? 1 : 0));
			}
			return result;
		}
	}

	[Browsable(false)]
	public string EntryMode => ".NET";

	[Browsable(false)]
	public bool SaveInProgress
	{
		get
		{
			return _SaveInProgress;
		}
		set
		{
			_SaveInProgress = value;
		}
	}

	public object Table => this;

	public event EventHandler DisableAddNewChanged;

	public event EventHandler DisableDeleteChanged;

	public event EventHandler OverrideDeleteEnabledChanged;

	public event EventHandler<DbAndRowEventArgs> NoAccessChanged;

	public event EventHandler<DbAndRowEventArgs> ReadOnlyChanged;

	public event EventHandler<ExchangeRateChangedEventArgs> ExchangeRateChanged;

	[Description("This event runs when the row becomes the current row on the binding source.")]
	public event EventHandler<CurrentChangedEventArgs> CurrentChanged;

	[ProcessCodeBindings(true)]
	[Description("This event runs when validating the row. The e parameter is a ValidationInfo object, which allows you to add errors and warnings to the validation list.")]
	public event EventHandler<ValidEventArgs> Valid;

	public event EventHandler CurrencyRateIdForeignChanged;

	[Description("This event runs before a row has been removed from the binding source (and not yet updated to the database).")]
	public event EventHandler<RemoveEventArgs> RemoveStarted;

	[Description("This event runs after a row has been removed from the binding source (but not yet updated to the database).")]
	public event EventHandler<RemoveEventArgs> RemoveCompleted;

	[Description("This event runs when the system needs to get the next id for a row. The return value will be the value that is used for the next id.")]
	public event EventHandler<GetNextIDEventArgs> GetNextID;

	[Description("This event runs before a row is about to be added to the database.")]
	public event EventHandler<RowUpdateEventArgs> UpdateStarted;

	[Description("This event runs after a row has been added to the database.")]
	public event EventHandler<RowUpdateEventArgs> UpdateCompleted;

	[Description("This event runs before a row is about to be removed from the database.")]
	public event EventHandler<RowUpdateEventArgs> DeleteStarted;

	[Description("This event runs after a row has been removed from the database.")]
	public event EventHandler<RowUpdateEventArgs> DeleteCompleted;

	[Description("This event runs after a row has been save in the database.")]
	public event EventHandler<SaveDataCompletedEventArgs> SaveDataCompleted;

	public event EventHandler<ParentBindingSourceChangedEventArgs> ParentBindingSourceChanged;

	private event EventHandler<KeyChangeEventArgs> KeyChange;

	[Description("This event runs after a row has been added to the binding source (not the database). This runs after the row has been added to the internal data table.")]
	public event EventHandler<AddNewCompletedEventArgs> AddNewCompleted;

	[Description("This event runs after a row has been added to the binding source (not the database). This runs before the row has been added to the internal data table. This allows you to set default values for the row.")]
	public event EventHandler<DbAndRowEventArgs> SetDefaultValues;

	public event EventHandler Disposed;

	private void checkFirstEditableKey()
	{
		if (KeysAtThisLevel > 1 && KeyFieldsArray != null && KeysAtThisLevel <= KeyFieldsArray.Length)
		{
			FirstEditableKeyField = KeyFieldsArray[KeyFieldsArray.Length - KeysAtThisLevel];
		}
		else
		{
			FirstEditableKeyField = LastKeyField;
		}
	}

	public bool GetAutoIncrement(M1Database database)
	{
		return database.NextIDs.GetNextIDInfo(TableName).AutoIncrement switch
		{
			DatabaseAutoIncrement.SystemDefault => AutoIncrementResolved, 
			DatabaseAutoIncrement.NoAutoIncrement => false, 
			DatabaseAutoIncrement.AutoIncrement => true, 
			_ => false, 
		};
	}

	public void ResetAllProperties()
	{
		TableName = string.Empty;
		TableNameFormatted = string.Empty;
		Caption = string.Empty;
		ValidCodeReferencedFields.Clear();
		getChildRowCountReferenced = false;
		DefaultFormCollectionID = string.Empty;
		DefaultGridID = string.Empty;
		OverrideDelete = string.Empty;
		OverrideDeleteEnabledExpression = string.Empty;
		OverrideDeleteResolved = false;
		QuickSearchFields = string.Empty;
		QuickSearchFieldsUser = string.Empty;
		KeyFields = string.Empty;
		Module = string.Empty;
		AdditionalField1 = string.Empty;
		AdditionalField2 = string.Empty;
		AdditionalField3 = string.Empty;
		AdditionalFieldUser1 = string.Empty;
		AdditionalFieldUser2 = string.Empty;
		AdditionalFieldUser3 = string.Empty;
		ColorExpression = string.Empty;
		ColorExpressionUser = string.Empty;
		NumericOnlyKeys = TableKeyNumericOnlyEnum.No;
		ForeignKeyDeleteFilter = string.Empty;
		AutoIncrement = false;
		AutoIncrementUser = AutoIncrementUserEnum.None;
		IncrementAmount = 0;
		IncrementAmountUser = 0;
		InitialValue = string.Empty;
		FieldPrefix = string.Empty;
		FieldPrefixUser = string.Empty;
		ReadOnlyExpression = string.Empty;
		ReadOnlyExpressionUser = string.Empty;
		ReadOnlyExpressionReferencedFields.Clear();
		ReadOnlyResolved = false;
		ReadOnlyExpressionResolved = false;
		NoAccessResolved = false;
		DisableAddNewExpression = string.Empty;
		DisableAddNewExpressionUser = string.Empty;
		DisableAddNewResolved = false;
		DisableDeleteExpression = string.Empty;
		DisableDeleteExpressionReferencedFields.Clear();
		DisableDeleteExpressionUser = string.Empty;
		DisableDeleteResolved = false;
		Custom = false;
		ChangeDetailIdsFilter = string.Empty;
		AllowEditingInGrid = false;
		AllowChangeId = false;
		AllowSaveAs = false;
		AllowImport = false;
		AllowMailMerge = false;
		AllowMap = false;
		PrimaryContactField = string.Empty;
		PromptOnAddField = string.Empty;
		CurrencyModeLocationField = string.Empty;
		CurrencyRateIdField = string.Empty;
		CurrencyCustomRateField = string.Empty;
		CurrencyExchangeRateField = string.Empty;
		DocumentDateField = string.Empty;
		DocumentPlantIdField = string.Empty;
		ClosedField = string.Empty;
		ClosedValue = string.Empty;
		ClosedDateField = string.Empty;
		ClosedExtraSetExpression = string.Empty;
		ClosedIncludeOptionText = string.Empty;
		ClosedIncludeOptionSqlExpression = string.Empty;
		ClosedCutoffDateField = string.Empty;
		ClosedRoleCheck = string.Empty;
		ClosedHelpLink = string.Empty;
		PurgeCutoffDateField = string.Empty;
		PurgeHelpLink = string.Empty;
		SqlView = false;
		UniqueField = string.Empty;
		FieldToCheckOnUpdate = string.Empty;
		ChildReferenceTableLinks.Clear();
		ChildDeleteReferenceTableLinks = null;
		Databases.Clear();
	}

	public void Load(DataRow row, M1DataDictionary dataDictionary, DataRow[] childReferences, bool allowEditing)
	{
		allowEditingOverride = allowEditing;
		_UniqueID = row.Field<Guid?>("dtUniqueID");
		_AppExtensionID = row.Field<string>("dtAppExtensionID");
		TableName = row.Field<string>("dtTable");
		TableNameFormatted = row.Field<string>("dtDisplayName");
		ParentTableName = row.Field<string>("dtParentTable");
		loadedParentKeyFields = row.Field<string>("parentKeyFields");
		if (row.Table.Columns.Contains("TopLevelTable"))
		{
			TopLevelTable = row.Field<string>("TopLevelTable");
			TopLevelDateField = row.Field<string>("TopLevelDateField");
			TopLevelKeyFields = row.Field<string>("TopLevelKeyFields");
			TopLevelPlantIdField = row.Field<string>("TopLevelPlantIdField");
		}
		Caption = row.Field<string>("dtCaption");
		ValidCodeReferencedFields.Clear();
		getChildRowCountReferenced = false;
		DefaultFormCollectionID = row.Field<string>("dtDefaultObjectId");
		DefaultGridID = row.Field<string>("dtGridID");
		OverrideDelete = row.Field<string>("dtOverrideDelete");
		OverrideDeleteEnabledExpression = row.Field<string>("dtOverrideDeleteEnabledExpression");
		OverrideDeleteEnabledExpressionReferencedFields.Clear();
		if (OverrideDeleteEnabledExpression != null && OverrideDeleteEnabledExpression.Length != 0)
		{
			OverrideDeleteEnabledExpressionReferencedFields.ParseCodeForFields(OverrideDeleteEnabledExpression);
		}
		QuickSearchFields = row.Field<string>("dtQuickSearchFields");
		QuickSearchFieldsUser = row.Field<string>("dtQuickSearchFieldsUser");
		KeyFields = row.Field<string>("dtKeyFields");
		Module = row.Field<string>("dtModule");
		EnterInSequenceField = row.Field<string>("dtEnterInSequenceField");
		AdditionalField1 = row.Field<string>("dtAddFld1");
		AdditionalField2 = row.Field<string>("dtAddFld2");
		AdditionalField3 = row.Field<string>("dtAddFld3");
		AdditionalFieldUser1 = row.Field<string>("dtUAddFld1");
		AdditionalFieldUser2 = row.Field<string>("dtUAddFld2");
		AdditionalFieldUser3 = row.Field<string>("dtUAddFld3");
		ColorExpression = row.Field<string>("dtColorExpression");
		ColorExpressionUser = row.Field<string>("dtColorExpressionUser");
		NumericOnlyKeys = row.Field<TableKeyNumericOnlyEnum>("dtNumericOnly");
		LastKeyCanBeEmpty = row.Field<bool>("dtLastKeyCanBeEmpty");
		EmptyKeyCanBeEdited = row.Field<bool>("dtEmptyKeyCanBeEdited");
		KeysAtThisLevel = row.Field<byte>("dtKeysAtThisLevel");
		ForeignKeyDeleteFilter = row.Field<string>("dtForeignKeyDeleteFilter");
		AutoIncrement = row.Field<bool>("dtAutoIncrement");
		AutoIncrementUser = row.Field<AutoIncrementUserEnum>("dtAutoIncrementUser");
		IncrementAmount = row.Field<short>("dtIncrementAmount");
		IncrementAmountUser = row.Field<short>("dtIncrementAmountUser");
		InitialValue = row.Field<string>("dtInitialValue");
		FieldPrefix = row.Field<string>("dtPrefix");
		FieldPrefixUser = row.Field<string>("dtPrefixUser");
		ReadOnlyExpression = row.Field<string>("dtReadonlyExpression");
		ReadOnlyExpressionUser = row.Field<string>("dtReadonlyExpressionUser");
		ReadOnlyExpressionReferencedFields.Clear();
		if (ReadOnlyExpression != null && ReadOnlyExpression.Length != 0)
		{
			ReadOnlyExpressionReferencedFields.ParseCodeForFields(ReadOnlyExpression);
		}
		if (ReadOnlyExpressionUser != null && ReadOnlyExpressionUser.Length != 0)
		{
			ReadOnlyExpressionReferencedFields.ParseCodeForFields(ReadOnlyExpressionUser);
		}
		DisableAddNewExpression = row.Field<string>("dtDisableAddNewExpression");
		DisableAddNewExpressionUser = row.Field<string>("dtDisableAddNewExpressionUser");
		DisableDeleteExpression = row.Field<string>("dtDisableDeleteExpression");
		DisableDeleteExpressionUser = row.Field<string>("dtDisableDeleteExpressionUser");
		DisableChangeIDExpression = row.Field<string>("dtDisableChangeIDExpression");
		DisableChangeIDExpressionUser = row.Field<string>("dtDisableChangeIDExpressionUser");
		DisableDeleteExpressionReferencedFields.Clear();
		if (DisableDeleteExpression != null && DisableDeleteExpression.Length != 0)
		{
			DisableDeleteExpressionReferencedFields.ParseCodeForFields(DisableDeleteExpression);
		}
		if (DisableDeleteExpressionUser != null && DisableDeleteExpressionUser.Length != 0)
		{
			DisableDeleteExpressionReferencedFields.ParseCodeForFields(DisableDeleteExpressionUser);
		}
		Custom = row.Field<bool>("dtCustom");
		ChangeDetailIdsFilter = row.Field<string>("dtChangeDetailIdsFilter");
		AllowEditingInGrid = row.Field<bool>("dtGridEdit");
		AllowChangeId = row.Field<bool>("dtChangeId");
		AllowSaveAs = row.Field<bool>("dtSaveAs");
		AllowImport = row.Field<bool>("dtImport");
		AllowMailMerge = row.Field<bool>("dtMailMerge");
		AllowMap = row.Field<bool>("dtMap");
		PrimaryContactField = row.Field<string>("dtContactField");
		PromptOnAddField = row.Field<string>("dtPromptOnAddField");
		CurrencyModeLocationField = row.Field<string>("dtCurrencyModeLocationField");
		CurrencyRateIdField = row.Field<string>("dtCurrencyRateIdField");
		CurrencyCustomRateField = row.Field<string>("dtCurrencyCustomRateField");
		CurrencyExchangeRateField = row.Field<string>("dtCurrencyExchangeRateField");
		DocumentDateField = row.Field<string>("dtDocumentDateField");
		DocumentPlantIdField = row.Field<string>("dtDocumentPlantIdField");
		CurrencyUpdateType = row.Field<CurrencyUpdateTypeEnum>("dtCurrencyUpdateType");
		ClosedField = row.Field<string>("dtClosedField");
		ClosedValue = row.Field<string>("dtClosedValue");
		ClosedDateField = row.Field<string>("dtClosedDateField");
		ClosedExtraSetExpression = row.Field<string>("dtClosedExtraSetExpression");
		ClosedIncludeOptionText = row.Field<string>("dtClosedIncludeOptionText");
		ClosedIncludeOptionSqlExpression = row.Field<string>("dtClosedIncludeOptionSqlExpr");
		ClosedCutoffDateField = row.Field<string>("dtClosedCutoffDateField");
		ClosedRoleCheck = row.Field<string>("dtClosedRoleCheck");
		ClosedHelpLink = row.Field<string>("dtClosedHelpLink");
		PurgeCutoffDateField = row.Field<string>("dtPurgeCutoffDateField");
		PurgeHelpLink = row.Field<string>("dtPurgeHelpLink");
		QuickSearchOption = row.Field<TableQuickSearchOption>("dtQuickSearchOption");
		SqlView = row.Field<bool>("dtSqlView");
		UniqueField = row.Field<string>("dtUniqueField");
		FieldToCheckOnUpdate = row.Field<string>("dtFieldToCheckOnUpdate");
		if (childReferences != null && childReferences.Length != 0)
		{
			foreach (DataRow row2 in childReferences)
			{
				ChildReferenceTableLinks.Add(new ChildReferenceTableLink(row2));
			}
		}
		ChildCurrencyLinks.Clear();
		if (CurrencyExchangeRateField.Length == 0 || !allowEditing || dataDictionary == null)
		{
			return;
		}
		SqlCommand sqlCommand = dataDictionary.NewSqlCommand("select dtParentTable As ParentTable,dfTable As ChildTable,dfField As ChildField,dfDecimals As ChildFieldDecimals,dtKeyFields As ChildKeyFields,dfCurrencyType As ChildCurrencyType,dfCurrencyRelatedField As ChildRelatedCurrencyField,dfHasChangeCode As CodeExists From DDFields Inner Join DDTables On dfTable = dtTable Where dfTable In     (Select dfTable From DDFields         Inner Join DDTables a On dfTable = dtTable         Where dfRelatedTable = @tablename And dfTable <> @tablename And dtKeyFields LIKE '%' + RTrim(dfField) + '%') And dfCurrencyType <> 0 And dfCurrencyUpdateRelatedField <> 0");
		sqlCommand.Parameters.Add(new SqlParameter("@tablename", SqlDbType.NVarChar, TableName.Length)).Value = TableName;
		foreach (DataRow row3 in dataDictionary.GetDataTable(sqlCommand).Rows)
		{
			ChildCurrencyLinks.Add(new ChildCurrencyLink(row3));
		}
	}

	public void LoadDatabase(string databaseName, DataRow row, TableSecurityExpressions securityExpressions)
	{
		Databases.Add(new M1DatabaseTableSecurity(databaseName, row, securityExpressions));
	}

	public void RelatedReadOnlyFieldValueChanged(object sender, FieldDefinition.FieldValueChangedEventArgs e)
	{
		if (e.IsCurrentRow)
		{
			EvaluateReadOnlyExpression(e.Database, e.Row, e.SqlTransaction);
		}
	}

	public void EvaluateDisableAddNewExpression(M1Database database, DataRow row, SqlTransaction transaction)
	{
		bool disableAddNewExpression = GetDisableAddNewExpression(database, row, transaction);
		if (disableAddNewExpression != DisableAddNewResolved)
		{
			DisableAddNewResolved = disableAddNewExpression;
			OnDisableAddNewChanged(EventArgs.Empty);
		}
	}

	public bool GetDisableAddNewExpression(M1Database database, DataRow row, SqlTransaction transaction)
	{
		bool flag = _DisableAddNewOverride;
		if (!allowEditingOverride)
		{
			flag = true;
		}
		if (!flag)
		{
			M1DatabaseTableSecurity securityObject = getSecurityObject(database);
			if (securityObject != null && (securityObject.ResolvedAccessLevel & SecurityAccessLevel.Add) == 0)
			{
				flag = true;
			}
			if (securityObject != null && !flag && securityObject.SecurityExpressions != null && !string.IsNullOrWhiteSpace(securityObject.SecurityExpressions.Add) && !EvaluateScriptExpressionBool(securityObject.SecurityExpressions.Add, database, row, transaction))
			{
				flag = true;
			}
		}
		if (BindingSource != null && !flag)
		{
			flag = EvaluateScriptExpressionBool(DisableAddNewExpression, DisableAddNewExpressionUser, database, row, transaction);
		}
		return flag;
	}

	public void SetDisableAddNewOverride(bool value, M1Database database, DataRow row, SqlTransaction transaction)
	{
		if (_DisableAddNewOverride != value)
		{
			_DisableAddNewOverride = value;
		}
		EvaluateDisableAddNewExpression(database, row, transaction);
	}

	protected virtual void OnDisableAddNewChanged(EventArgs e)
	{
		this.DisableAddNewChanged?.Invoke(this, e);
	}

	public void EvaluateDisableDeleteExpression(M1Database database, DataRow row, SqlTransaction transaction)
	{
		bool disableDeleteExpression = GetDisableDeleteExpression(database, row, transaction);
		if (disableDeleteExpression != DisableDeleteResolved)
		{
			DisableDeleteResolved = disableDeleteExpression;
			OnDisableDeleteChanged(EventArgs.Empty);
		}
	}

	public void EvaluateOverrideDeleteEnabledExpression(M1Database database, DataRow row, SqlTransaction transaction)
	{
		bool flag = IsOverrideDeleteProcess(database, row, transaction);
		if (flag != OverrideDeleteResolved)
		{
			OverrideDeleteResolved = flag;
			OnOverrideDeleteEnabledChanged(EventArgs.Empty);
		}
	}

	public bool IsOverrideDeleteProcess(M1Database database, DataRow row, SqlTransaction transaction)
	{
		if (!string.IsNullOrWhiteSpace(OverrideDelete))
		{
			if (!string.IsNullOrWhiteSpace(OverrideDeleteEnabledExpression))
			{
				return EvaluateScriptExpressionBool(OverrideDeleteEnabledExpression, string.Empty, database, row, transaction);
			}
			return true;
		}
		return false;
	}

	public bool GetDisableDeleteExpression(M1Database database, DataRow row, SqlTransaction transaction)
	{
		bool flag = _DisableDeleteOverride;
		if (!allowEditingOverride || row == null)
		{
			flag = true;
		}
		if (!flag)
		{
			flag = IsSecurityDisabled(database, row);
		}
		if (BindingSource != null && !flag)
		{
			if (!string.IsNullOrWhiteSpace(OverrideDelete) && !string.IsNullOrWhiteSpace(OverrideDeleteEnabledExpression))
			{
				flag = !EvaluateScriptExpressionBool(OverrideDeleteEnabledExpression, string.Empty, database, row, transaction);
				if (flag)
				{
					flag = EvaluateScriptExpressionBool(DisableDeleteExpression, DisableDeleteExpressionUser, database, row, transaction);
				}
			}
			else
			{
				flag = EvaluateScriptExpressionBool(DisableDeleteExpression, DisableDeleteExpressionUser, database, row, transaction);
			}
		}
		return flag;
	}

	public bool IsSecurityDisabled(M1Database database, DataRow row)
	{
		bool flag = false;
		M1DatabaseTableSecurity securityObject = getSecurityObject(database);
		if (securityObject != null && (securityObject.ResolvedAccessLevel & SecurityAccessLevel.Delete) == 0)
		{
			flag = true;
		}
		if (securityObject != null && !flag && securityObject.SecurityExpressions != null && !string.IsNullOrWhiteSpace(securityObject.SecurityExpressions.Delete) && !EvaluateScriptExpressionBool(securityObject.SecurityExpressions.Delete, database, row))
		{
			flag = true;
		}
		return flag;
	}

	public void SetDisableDeleteOverride(bool value, M1Database database, DataRow row, SqlTransaction transaction)
	{
		if (_DisableDeleteOverride != value)
		{
			_DisableDeleteOverride = value;
		}
		EvaluateDisableDeleteExpression(database, row, transaction);
	}

	protected virtual void OnDisableDeleteChanged(EventArgs e)
	{
		this.DisableDeleteChanged?.Invoke(this, e);
	}

	protected virtual void OnOverrideDeleteEnabledChanged(EventArgs e)
	{
		this.OverrideDeleteEnabledChanged?.Invoke(this, e);
	}

	private M1DatabaseTableSecurity getSecurityObject(M1Database database)
	{
		if (Databases.Count > 0 && database != null)
		{
			foreach (M1DatabaseTableSecurity database2 in Databases)
			{
				if (database2.Database.Equals(database.ID, StringComparison.CurrentCultureIgnoreCase))
				{
					return database2;
				}
			}
		}
		return null;
	}

	public string GetReadOnlyReasons(M1Database database, DataRow row)
	{
		StringBuilder stringBuilder = new StringBuilder();
		M1DatabaseTableSecurity securityObject = getSecurityObject(database);
		if (securityObject != null)
		{
			if ((securityObject.ResolvedAccessLevel & SecurityAccessLevel.None) != SecurityAccessLevel.Default || securityObject.ResolvedAccessLevel == SecurityAccessLevel.View)
			{
				stringBuilder.Append(securityObject.GetReadOnlyReasons(this));
			}
			else if (securityObject.SecurityExpressions != null && !string.IsNullOrWhiteSpace(securityObject.SecurityExpressions.Edit) && !EvaluateScriptExpressionBool(securityObject.SecurityExpressions.Edit, database, row))
			{
				stringBuilder.AppendLine($"DD: Table {TableNameFormatted} User Administration Security Edit Expression evaluated to false.");
			}
		}
		else
		{
			stringBuilder.AppendLine($"SEC: The security access level for database {database.ID} could not be determined, so a default of no access was used.");
		}
		if (BindingSource != null && row != null)
		{
			if (EvaluateScriptExpressionBool(ReadOnlyExpression, string.Empty, database, row, null))
			{
				stringBuilder.AppendLine($"DD: Table {TableNameFormatted} ReadOnlyExpression evaluated to true.");
			}
			if (EvaluateScriptExpressionBool(ReadOnlyExpressionUser, string.Empty, database, row, null))
			{
				stringBuilder.AppendLine($"DD: Table {TableNameFormatted} ReadOnlyExpressionUser evaluated to true.");
			}
		}
		return stringBuilder.ToString();
	}

	protected virtual void OnNoAccessChanged(DbAndRowEventArgs e)
	{
		this.NoAccessChanged?.Invoke(this, e);
	}

	public void EvaluateNoAccess(M1Database database, DataRow row, SqlTransaction transaction)
	{
		bool flag = false;
		M1DatabaseTableSecurity securityObject = getSecurityObject(database);
		flag = securityObject == null || (securityObject.ResolvedAccessLevel & SecurityAccessLevel.None) != 0;
		if (!flag && Module.Length != 0 && (!BindingSource.DataDictionary.ProductCode.IsModulePurchased(Module, database) || database.Security.GetModuleAccessLevel(Module) == SecurityAccessLevel.None))
		{
			flag = true;
		}
		if (flag != NoAccessResolved)
		{
			NoAccessResolved = flag;
			OnNoAccessChanged(new DbAndRowEventArgs(database, row, transaction));
		}
	}

	public void EvaluateReadOnlyExpression(M1Database database, DataRow row, SqlTransaction transaction)
	{
		bool readOnlyExpression = GetReadOnlyExpression(database, row, transaction);
		if (readOnlyExpression != ReadOnlyResolved)
		{
			ReadOnlyResolved = readOnlyExpression;
			OnReadOnlyChanged(new DbAndRowEventArgs(database, row, transaction));
		}
	}

	public bool GetReadOnlyExpression(M1Database database, DataRow row, SqlTransaction transaction)
	{
		ReadOnlyExpressionResolved = false;
		bool flag = _ReadOnlyOverride;
		bool flag2 = false;
		if (!flag)
		{
			M1DatabaseTableSecurity securityObject = getSecurityObject(database);
			if (securityObject != null)
			{
				flag = (securityObject.ResolvedAccessLevel & SecurityAccessLevel.None) != SecurityAccessLevel.Default || securityObject.ResolvedAccessLevel == SecurityAccessLevel.View;
				if (!flag && securityObject.SecurityExpressions != null && !string.IsNullOrWhiteSpace(securityObject.SecurityExpressions.Edit) && !EvaluateScriptExpressionBool(securityObject.SecurityExpressions.Edit, database, row, null))
				{
					flag = true;
				}
			}
			else
			{
				flag = true;
			}
		}
		if (BindingSource != null && row != null)
		{
			try
			{
				flag2 = EvaluateScriptExpressionBool(ReadOnlyExpression, ReadOnlyExpressionUser, database, row, transaction);
			}
			catch (Exception ex)
			{
				if (!ex.Message.Contains("The given key was not present in the dictionary."))
				{
					throw;
				}
				flag2 = true;
			}
		}
		if (flag || flag2)
		{
			ReadOnlyExpressionResolved = true;
		}
		return ReadOnlyExpressionResolved;
	}

	public bool GetHasSecurityExpression(M1Database database, DataRow row)
	{
		bool result = false;
		M1DatabaseTableSecurity securityObject = getSecurityObject(database);
		if (securityObject.SecurityExpressions != null && !string.IsNullOrWhiteSpace(securityObject.SecurityExpressions.Edit) && !EvaluateScriptExpressionBool(securityObject.SecurityExpressions.Edit, database, row, null))
		{
			result = true;
		}
		return result;
	}

	public void SetReadOnlyOverride(bool value, M1Database database, DataRow row)
	{
		if (_ReadOnlyOverride != value)
		{
			_ReadOnlyOverride = value;
		}
		EvaluateReadOnlyExpression(database, row, null);
	}

	protected virtual void OnReadOnlyChanged(DbAndRowEventArgs e)
	{
		this.ReadOnlyChanged?.Invoke(this, e);
	}

	public bool ShouldCurrencyRefreshUpdateBase(M1Database database, DataRow row, SqlTransaction sqlTransaction)
	{
		switch (CurrencyUpdateType)
		{
		case CurrencyUpdateTypeEnum.UpdateBase:
			return true;
		case CurrencyUpdateTypeEnum.UpdateForeign:
			return false;
		default:
		{
			string defaultCurrencyRateIdForRow = GetDefaultCurrencyRateIdForRow(database, row, sqlTransaction);
			if (!string.IsNullOrWhiteSpace(CurrencyRateIdField) && defaultCurrencyRateIdForRow.Equals(row.Field<string>(CurrencyRateIdField).Trim(), StringComparison.CurrentCultureIgnoreCase) && defaultCurrencyRateIdForRow.Length != 0 && !defaultCurrencyRateIdForRow.Equals(database.HomeCurrencyID, StringComparison.CurrentCultureIgnoreCase))
			{
				return true;
			}
			return false;
		}
		}
	}

	private void OnExchangeRateChanged(ExchangeRateChangedEventArgs e)
	{
		this.ExchangeRateChanged?.Invoke(this, e);
	}

	private void Field_ValueChanged_CurrencyUpdateExchangeRate(object sender, FieldDefinition.FieldValueChangedEventArgs e)
	{
		if (CurrencyExchangeRateField.Length == 0 || DocumentDateField.Length == 0 || CurrencyRateIdField.Length == 0 || (CurrencyCustomRateField.Length != 0 && Convert.ToDecimal(e.Row[CurrencyCustomRateField]) != 0m))
		{
			return;
		}
		string empty = string.Empty;
		empty = e.Row.Field<string>(CurrencyRateIdField).Trim();
		decimal value;
		if (empty.Length == 0)
		{
			value = 1m;
		}
		else
		{
			DateTime? dateToUse = null;
			if (DocumentDateField.StartsWith(FieldPrefix, StringComparison.CurrentCultureIgnoreCase) || DocumentDateField.StartsWith(FieldPrefixUser, StringComparison.CurrentCultureIgnoreCase))
			{
				dateToUse = e.Row.Field<DateTime?>(DocumentDateField);
			}
			else
			{
				string[] keyFieldsArray = KeyFieldsArray;
				foreach (string key in keyFieldsArray)
				{
					if (BindingSource.Fields[key].RelatedTableKeyFields.StartsWith(DocumentDateField.Substring(0, 3), StringComparison.CurrentCultureIgnoreCase))
					{
						dateToUse = BindingSource.Fields[key].RelatedTableGetDataRow(DocumentDateField).Field<DateTime?>(DocumentDateField);
						break;
					}
				}
			}
			value = e.Database.GetExchangeRate(empty, dateToUse, e.SqlTransaction);
		}
		if (!Convert.ToDecimal(e.Row[CurrencyExchangeRateField]).Equals(value))
		{
			if (e.Row.RowState == DataRowState.Modified || e.Row.RowState == DataRowState.Unchanged)
			{
				VerifyChildBindingSourcesForCurrencyLinks(ChildCurrencyLinks);
			}
			e.Row.SetField(CurrencyExchangeRateField, value);
		}
	}

	private void CurrencyRateIdField_ValueChanged(object sender, FieldDefinition.FieldValueChangedEventArgs e)
	{
		if (e.Row.RowState != DataRowState.Modified)
		{
			_ = e.Row.RowState;
			_ = 2;
		}
		if (CurrencyCustomRateField.Length != 0 && Convert.ToDecimal(e.Row[CurrencyCustomRateField]) != 0m)
		{
			e.Row.SetField(CurrencyCustomRateField, 0m);
		}
		Field_ValueChanged_CurrencyUpdateExchangeRate(sender, e);
		if (e.IsCurrentRow)
		{
			setCurrencyRateIdForeign(e.Row.Field<string>(CurrencyRateIdField).Trim(), e.Database);
		}
	}

	public void CheckFieldAndParentForSave(M1Database database, DataRow row, ValidationInfo validInfo)
	{
		if (KeyFields.Length == 0 || M1Util.IsNullOrEmpty(row[KeyFieldsArray[0]]))
		{
			return;
		}
		bool flag = false;
		object obj = 0;
		if (FieldToCheckOnUpdate.Length != 0)
		{
			obj = ((row.RowState == DataRowState.Added) ? ((object)false) : ((!row.HasVersion(DataRowVersion.Original)) ? row[FieldToCheckOnUpdate] : row[FieldToCheckOnUpdate, DataRowVersion.Original]));
			if (obj.Equals(false))
			{
				if (BindingSource.Fields[FieldToCheckOnUpdate].BoundParentField.Length != 0 && BindingSource.Fields[FieldToCheckOnUpdate].BoundParentFieldType == FieldDefinition.BoundParentFieldTypeEnum.FromParent && KeyFieldsArray.Length > 1 && BindingSource.Fields[KeyFieldsArray[KeyFieldsArray.Length - 2]].RelatedTable.Length != 0)
				{
					FieldDefinition fieldDefinition = ((!string.IsNullOrWhiteSpace(BindingSource.Fields[FieldToCheckOnUpdate].BoundParentFieldProxy)) ? BindingSource.Fields[BindingSource.Fields[FieldToCheckOnUpdate].BoundParentFieldProxy] : BindingSource.Fields[KeyFieldsArray[KeyFieldsArray.Length - 2]]);
					object obj2 = database.ExecuteScalar("Select " + BindingSource.Fields[FieldToCheckOnUpdate].BoundParentField + " From " + fieldDefinition.RelatedTable + " Where " + fieldDefinition.RelatedTableGetWhereClause(zeroRecordsIfFirstFieldIsEmpty: false, includeLastField: true, row), BindingSource.Transaction);
					flag = true;
					if (obj2 == null)
					{
						validInfo.AddError($"{BindingSource.Fields[LastKeyField].RelatedFieldsFormatCaptionAndCurrentValues(row)}  cannot be saved because it's parent no longer exists");
					}
					else if (!obj2.Equals(obj))
					{
						validInfo.AddError(BindingSource.Fields[LastKeyField].RelatedFieldsFormatCaptionAndCurrentValues(row) + " cannot be saved because it has been " + BindingSource.Fields[FieldToCheckOnUpdate].Caption.Replace("?", string.Empty).ToLower() + " while editing the data");
					}
				}
				else if (row.RowState != DataRowState.Added)
				{
					object obj2 = database.ExecuteScalar("Select " + FieldToCheckOnUpdate + " From " + TableName + " Where " + GetFilterForCurrentRow(row), BindingSource.Transaction);
					if (obj2 == null)
					{
						validInfo.AddError(BindingSource.Fields[LastKeyField].RelatedFieldsFormatCaptionAndCurrentValues(row) + " cannot be saved because it has been deleted from the database");
					}
					else if (!obj2.Equals(obj))
					{
						validInfo.AddError(BindingSource.Fields[LastKeyField].RelatedFieldsFormatCaptionAndCurrentValues(row) + " cannot be saved because it has been " + BindingSource.Fields[FieldToCheckOnUpdate].Caption.Replace("?", string.Empty).ToLower() + " while editing the data");
					}
				}
			}
		}
		int num = 1;
		if (KeysAtThisLevel > 1)
		{
			num = KeysAtThisLevel;
		}
		if (flag || KeyFieldsArray.Length - num <= 0 || BindingSource.Fields[KeyFieldsArray[KeyFieldsArray.Length - num - 1]].RelatedTable.Length == 0)
		{
			return;
		}
		if (ParentTableName.Length != 0)
		{
			parentKeyFieldsArray = getParentKeyFieldsArray();
			if (database.ExecuteScalar("Select 1 as dummy From " + ParentTableName + " Where " + GetPersistentParentWhereClause(row), BindingSource.Transaction) == null)
			{
				validInfo.AddError(BindingSource.Fields[LastKeyField].RelatedFieldsFormatCaptionAndCurrentValues(row) + " cannot be saved because it's parent no longer exists");
			}
		}
		else if (database.ExecuteScalar("Select 1 as dummy From " + BindingSource.Fields[KeyFieldsArray[KeyFieldsArray.Length - num - 1]].RelatedTable + " Where " + BindingSource.Fields[KeyFieldsArray[KeyFieldsArray.Length - num - 1]].RelatedTableGetWhereClause(zeroRecordsIfFirstFieldIsEmpty: false, includeLastField: true, row), BindingSource.Transaction) == null)
		{
			validInfo.AddError(BindingSource.Fields[LastKeyField].RelatedFieldsFormatCaptionAndCurrentValues(row) + " cannot be saved because it's parent no longer exists");
		}
	}

	public string GetFilterForCurrentRow(DataRow dataRow)
	{
		StringBuilder stringBuilder = new StringBuilder();
		string[] keyFieldsArray = KeyFieldsArray;
		foreach (string text in keyFieldsArray)
		{
			if (stringBuilder.Length == 0)
			{
				stringBuilder.AppendFormat("{0} = {1}", text, dataRow[text].ToSql());
			}
			else
			{
				stringBuilder.AppendFormat(" AND {0} = {1}", text, dataRow[text].ToSql());
			}
		}
		return stringBuilder.ToString();
	}

	public void OnCurrentChanged(CurrentChangedEventArgs e)
	{
		this.CurrentChanged?.Invoke(this, e);
	}

	protected void OnValid(ValidEventArgs e)
	{
		this.Valid?.Invoke(this, e);
	}

	public void Validate(M1Database database, DataRow row, SqlTransaction transaction, bool isTopLevel, bool isCurrentRow)
	{
		if (!allowEditingOverride)
		{
			return;
		}
		if (manuallyLoadedParentBindingSource != null)
		{
			manuallyLoadedParentBindingSource.OnValidate(new M1BindingSource.ValidateArgs
			{
				Errors = BindingSource.Errors
			});
		}
		if (row == null)
		{
			return;
		}
		errorList.Clear();
		errorList.BindingSource = BindingSource;
		errorList.Database = database;
		errorList.Row = row;
		OnValid(new ValidEventArgs(errorList, database, row, transaction));
		if (isTopLevel)
		{
			CheckFieldAndParentForSave(database, row, errorList);
		}
		if (BindingSource.Errors != null)
		{
			BindingSource.Errors.SetRowFieldErrorList(row, null, errorList);
		}
		foreach (FieldDefinition field in BindingSource.Fields)
		{
			if (field.Table == this)
			{
				field.Validate(database, row, transaction, isCurrentRow);
			}
		}
	}

	private void fieldEntryOrder_Valid(object sender, ValidEventArgs e)
	{
		CheckFieldForEntryOrder(e);
		GenerateFieldValidateOnNextRow(e);
	}

	private void GenerateFieldValidateOnNextRow(ValidEventArgs e)
	{
		DataTable dataTable = BindingSource.GetDataTable();
		if (dataTable.Rows.Count <= 1)
		{
			return;
		}
		foreach (DataRow row in dataTable.Rows)
		{
			if (row.RowState == DataRowState.Detached)
			{
				return;
			}
		}
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < KeyFieldsArray.Length - 1; i++)
		{
			if (stringBuilder.Length == 0)
			{
				stringBuilder.Append(KeyFieldsArray[i] + " = " + e.Row[KeyFieldsArray[i]].ToLinq());
			}
			else
			{
				stringBuilder.Append(" And " + KeyFieldsArray[i] + " = " + e.Row[KeyFieldsArray[i]].ToLinq());
			}
		}
		if (stringBuilder.Length == 0)
		{
			stringBuilder.Append(LastKeyField + " > " + e.Row[LastKeyField].ToLinq());
		}
		else
		{
			stringBuilder.Append(" And " + LastKeyField + " > " + e.Row[LastKeyField].ToLinq());
		}
		DataRow[] array = dataTable.Select(stringBuilder.ToString(), LastKeyField + " Asc");
		if (array.Length != 0 && BindingSource.shouldValidateRow(e.Database, array[0]))
		{
			BindingSource.Fields[EnterInSequenceField].Validate(e.Database, array[0], e.SqlTransaction, array[0] == BindingSource.CurrentAsDataRow, isolateInfo: true);
		}
	}

	protected void CheckFieldForEntryOrder(ValidEventArgs e)
	{
		DataTable dataTable = BindingSource.GetDataTable();
		if (dataTable.Rows.Count <= 1)
		{
			return;
		}
		foreach (DataRow row in dataTable.Rows)
		{
			if (row.RowState == DataRowState.Detached)
			{
				return;
			}
		}
		string enterInSequenceField = EnterInSequenceField;
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < KeyFieldsArray.Length - 1; i++)
		{
			if (stringBuilder.Length == 0)
			{
				stringBuilder.Append(KeyFieldsArray[i] + " = " + e.Row[KeyFieldsArray[i]].ToLinq());
			}
			else
			{
				stringBuilder.Append(" And " + KeyFieldsArray[i] + " = " + e.Row[KeyFieldsArray[i]].ToLinq());
			}
		}
		if (stringBuilder.Length == 0)
		{
			stringBuilder.Append(LastKeyField + " < " + e.Row[LastKeyField].ToLinq());
		}
		else
		{
			stringBuilder.Append(" And " + LastKeyField + " < " + e.Row[LastKeyField].ToLinq());
		}
		DataRow[] array = dataTable.Select(stringBuilder.ToString(), LastKeyField + " Desc");
		if (array.Length != 0)
		{
			decimal num = Convert.ToDecimal(e.Row[enterInSequenceField]);
			decimal num2 = Convert.ToDecimal(array[0][enterInSequenceField]);
			if (num <= num2)
			{
				e.AddError(BindingSource.Fields[enterInSequenceField].Caption + " must be entered in ascending order by " + BindingSource.Fields[LastKeyField].Caption);
			}
		}
	}

	public decimal GetExchangeRateForRow(M1Database database, DataRow row, SqlTransaction sqlTransaction)
	{
		for (int num = KeyFieldsArray.Length - 1; num >= 0; num--)
		{
			FieldDefinition fieldDefinition = BindingSource.Fields[KeyFieldsArray[num]];
			if (fieldDefinition.RelatedTableCurrencyExchangeRateField.Length != 0)
			{
				DataRow dataRow = ((!fieldDefinition.RelatedTable.Equals(TableName, StringComparison.CurrentCultureIgnoreCase) && (row == null || !row.Table.Columns.Contains(fieldDefinition.RelatedTableCurrencyExchangeRateField))) ? fieldDefinition.RelatedTableGetDataRow(fieldDefinition.RelatedTableCurrencyExchangeRateField, database, row, alwaysReturnValidRow: false, sqlTransaction) : row);
				if (dataRow == null)
				{
					break;
				}
				return dataRow.Field<decimal>(fieldDefinition.RelatedTableCurrencyExchangeRateField);
			}
		}
		return 1m;
	}

	public string GetForeignCurrencyRateIdForRow(M1Database database, DataRow row)
	{
		string text = string.Empty;
		if (CurrencyRateIdField.Length != 0)
		{
			return row.Field<string>(CurrencyRateIdField).Trim();
		}
		for (int num = KeyFieldsArray.Length - 1; num >= 0; num--)
		{
			FieldDefinition fieldDefinition = BindingSource.Fields[KeyFieldsArray[num]];
			if (fieldDefinition.RelatedTableCurrencyRateIdField.Length != 0)
			{
				DataRow dataRow = ((!fieldDefinition.RelatedTable.Equals(TableName, StringComparison.CurrentCultureIgnoreCase)) ? fieldDefinition.RelatedTableGetDataRow(fieldDefinition.RelatedTableCurrencyRateIdField, database, row) : row);
				if (dataRow != null)
				{
					text = dataRow.Field<string>(fieldDefinition.RelatedTableCurrencyRateIdField).Trim();
					if (text.Length == 0)
					{
						text = database.HomeCurrencyID;
					}
				}
				break;
			}
		}
		return text;
	}

	public string GetDefaultCurrencyRateIdForRow(M1Database database, DataRow row, SqlTransaction sqlTransaction)
	{
		string result = database.HomeCurrencyID;
		bool flag = false;
		for (int num = KeyFieldsArray.Length - 1; num >= 0; num--)
		{
			FieldDefinition fieldDefinition = BindingSource.Fields[KeyFieldsArray[num]];
			if (fieldDefinition.RelatedTableCurrencyModeLocationField.Length != 0)
			{
				DataRow dataRow = ((fieldDefinition.Table != this || !fieldDefinition.RelatedTable.Equals(TableName, StringComparison.CurrentCultureIgnoreCase)) ? fieldDefinition.RelatedTableGetDataRow(fieldDefinition.RelatedTableCurrencyModeLocationAndRelatedFields) : row);
				if (dataRow != null && fieldDefinition.RelatedTableCurrencyModeLocationAndRelatedFieldsArray.Length == 2 && dataRow.Table.Columns.Contains(fieldDefinition.RelatedTableCurrencyModeLocationAndRelatedFieldsArray[0]) && dataRow.Table.Columns.Contains(fieldDefinition.RelatedTableCurrencyModeLocationAndRelatedFieldsArray[1]) && dataRow.Field<string>(fieldDefinition.RelatedTableCurrencyModeLocationAndRelatedFieldsArray[0]).Trim().Length != 0)
				{
					SqlCommand sqlCommand = database.NewSqlCommand("Select cmlCurrencyRateID From OrganizationLocations Where cmlOrganizationID = @OrgID And cmlLocationID = @LocID");
					sqlCommand.Parameters.Add(new SqlParameter("@OrgID", SqlDbType.NVarChar)).Value = dataRow.Field<string>(fieldDefinition.RelatedTableCurrencyModeLocationAndRelatedFieldsArray[0]);
					sqlCommand.Parameters.Add(new SqlParameter("@LocID", SqlDbType.NVarChar)).Value = dataRow.Field<string>(fieldDefinition.RelatedTableCurrencyModeLocationAndRelatedFieldsArray[1]);
					string text = (string)database.ExecuteScalar(sqlCommand, sqlTransaction);
					if (text != null)
					{
						result = text.Trim();
						flag = true;
					}
				}
				break;
			}
		}
		if (!flag)
		{
			for (int num2 = KeyFieldsArray.Length - 1; num2 >= 0; num2--)
			{
				FieldDefinition fieldDefinition = BindingSource.Fields[KeyFieldsArray[num2]];
				if (fieldDefinition.RelatedTableCurrencyRateIdField.Length != 0)
				{
					DataRow dataRow2 = ((fieldDefinition.Table != this || !fieldDefinition.RelatedTable.Equals(TableName, StringComparison.CurrentCultureIgnoreCase)) ? fieldDefinition.RelatedTableGetDataRow(fieldDefinition.RelatedTableCurrencyRateIdField) : row);
					if (dataRow2 != null && dataRow2.Table.Columns.Contains(fieldDefinition.RelatedTableCurrencyRateIdField))
					{
						result = dataRow2.Field<string>(fieldDefinition.RelatedTableCurrencyRateIdField);
					}
					break;
				}
			}
		}
		return result;
	}

	public string GetCurrencyRateIdForRow(M1Database database, DataRow row, SqlTransaction sqlTransaction)
	{
		string result = database.HomeCurrencyID;
		for (int num = KeyFieldsArray.Length - 1; num >= 0; num--)
		{
			FieldDefinition fieldDefinition = BindingSource.Fields[KeyFieldsArray[num]];
			if (fieldDefinition.RelatedTableCurrencyRateIdField.Length != 0)
			{
				DataRow dataRow = ((fieldDefinition.Table != this || !fieldDefinition.RelatedTable.Equals(TableName, StringComparison.CurrentCultureIgnoreCase)) ? fieldDefinition.RelatedTableGetDataRow(fieldDefinition.RelatedTableCurrencyRateIdField) : row);
				if (dataRow != null && dataRow.Table.Columns.Contains(fieldDefinition.RelatedTableCurrencyRateIdField))
				{
					result = dataRow.Field<string>(fieldDefinition.RelatedTableCurrencyRateIdField);
				}
				break;
			}
		}
		return result;
	}

	public string GetDefaultCurrencyModeForRow(M1Database database, DataRow row, SqlTransaction sqlTransaction)
	{
		string result = "HOME";
		if (database != null)
		{
			string defaultCurrencyRateIdForRow = GetDefaultCurrencyRateIdForRow(database, row, sqlTransaction);
			if (defaultCurrencyRateIdForRow.Length != 0 && !database.HomeCurrencyID.Equals(defaultCurrencyRateIdForRow, StringComparison.CurrentCultureIgnoreCase))
			{
				result = "FOREIGN";
			}
		}
		return result;
	}

	public string GetCurrencyModeForRow(M1Database database, DataRow row, SqlTransaction sqlTransaction)
	{
		string result = "HOME";
		if (database != null)
		{
			string currencyRateIdForRow = GetCurrencyRateIdForRow(database, row, sqlTransaction);
			if (currencyRateIdForRow != null && currencyRateIdForRow.Length != 0 && !database.HomeCurrencyID.Equals(currencyRateIdForRow, StringComparison.CurrentCultureIgnoreCase))
			{
				result = "FOREIGN";
			}
		}
		return result;
	}

	public void RecursiveDelete(M1DataDictionary dataDictionary, string table, M1Database database, object[] keyValues, SqlTransaction transaction, M1BindingSource bindingSource)
	{
		StringBuilder stringBuilder = new StringBuilder();
		SqlCommand sqlCommand = dataDictionary.NewSqlCommand("Select drCTable,drCField,dtKeyFields From DDRelations With(Nolock) Inner Join DDTables With(NoLock) On drCTable = dtTable Where drPTable = @ParentTable And drPersist <> 0");
		sqlCommand.Parameters.Add(new SqlParameter("@ParentTable", SqlDbType.NVarChar)).Value = table;
		foreach (DataRow row in dataDictionary.GetDataTable(sqlCommand).Rows)
		{
			string text = row.Field<string>("drCTable");
			string[] array = row.Field<string>("drCField").Split(',');
			if (!row.Field<string>("dtKeyFields").Split(',')[0].Equals(array[0], StringComparison.CurrentCultureIgnoreCase))
			{
				continue;
			}
			stringBuilder.Length = 0;
			SqlCommand sqlCommand2 = database.NewSqlCommand(string.Empty);
			for (int i = 0; i < keyValues.Length; i++)
			{
				if (stringBuilder.Length != 0)
				{
					stringBuilder.Append(" And ");
				}
				stringBuilder.AppendFormat("{0}=@{0}", array[i]);
				if (keyValues[i].GetType() == typeof(string))
				{
					sqlCommand2.Parameters.Add(new SqlParameter($"@{array[i]}", SqlDbType.NVarChar)).Value = keyValues[i];
				}
				else
				{
					sqlCommand2.Parameters.Add(new SqlParameter($"@{array[i]}", SqlDbType.Decimal)).Value = keyValues[i];
				}
			}
			sqlCommand2.CommandText = $"Delete From {text}  Where {stringBuilder.ToString()} ";
			RecursiveDelete(dataDictionary, text, database, keyValues, transaction, null);
			database.ExecuteCommand(sqlCommand2, transaction);
		}
	}

	public void RecursiveDelete(M1DataDictionary dataDictionary, string table, M1Database database, string filter, SqlTransaction transaction, string topLevelTable, string topLevelKeyField)
	{
		SqlCommand sqlCommand = dataDictionary.NewSqlCommand("Select drCTable,drCField,dtKeyFields From DDRelations With(Nolock) Inner Join DDTables With(NoLock) On drCTable = dtTable Where drPTable = @ParentTable And drPersist <> 0");
		sqlCommand.Parameters.Add(new SqlParameter("@ParentTable", SqlDbType.NVarChar)).Value = table;
		foreach (DataRow row in dataDictionary.GetDataTable(sqlCommand).Rows)
		{
			string text = row.Field<string>("drCTable");
			string[] array = row.Field<string>("drCField").Split(',');
			if (row.Field<string>("dtKeyFields").Split(',')[0].Equals(array[0], StringComparison.CurrentCultureIgnoreCase))
			{
				RecursiveDelete(dataDictionary, text, database, filter, transaction, topLevelTable, topLevelKeyField);
				SqlCommand sqlCommand2 = database.NewSqlCommand("Delete " + text + " From " + text + " Inner Join " + topLevelTable + " On " + topLevelKeyField + " = " + array[0] + "  Where " + filter);
				database.ExecuteCommand(sqlCommand2, transaction);
			}
		}
	}

	public string ForeignKeyCheck(DataRow row, M1DataDictionary dataDictionary, M1Database database, FieldCollection Fields, bool checkDeleteFilter)
	{
		string text = string.Empty;
		int num = 0;
		int num2 = 0;
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		string[] keyFieldsArray = KeyFieldsArray;
		foreach (string columnName in keyFieldsArray)
		{
			if (!M1Util.IsNullOrEmpty(row[columnName]))
			{
				flag3 = true;
				break;
			}
		}
		if (flag3)
		{
			SqlCommand sqlCommand = null;
			if (TableName.Equals("WAREHOUSEBINS", StringComparison.CurrentCultureIgnoreCase))
			{
				sqlCommand = dataDictionary.NewSqlCommand(string.Format("select dfTable, dfRelatedFields, dfField, dtForeignKeyDeleteFilter,Convert(nvarchar(30),'') As sourceField,{0},{2}  From DDFields Inner Join DDTables On dfTable = dtTable {1} {3} Where dfRelatedTable = @Table And dfBoundParentFieldType = 0 And Not Exists(Select * From DDRelations Where drpTable = @Table And drcTable = dfTable And (drPersist <> 0 Or (drPersist = 0 And drDFilter = '0=1')))Union All select drPTable,Convert(nvarchar(30),''),drPField,drDFilter,drCField,{0},{2} from DDRelations Inner Join DDTables On DDTables.dtTable = drPTable Inner Join DDTables childTable On drCTable = childTable.dtTable Inner Join DDFields On childTable.dtUniqueField = dfField {1} {3} Where drcTable = @Table And drForeign <> 0 And drDFilter <> '0=1' And drCField = childTable.dtUniqueField", dataDictionary.Language.GetdtCaptionField(database, removeAsClause: false, "DDTables"), dataDictionary.Language.GetdtCaptionJoin(database), dataDictionary.Language.GetdfCaptionField(database, "DDFields", removeAsClause: false), dataDictionary.Language.GetdfCaptionJoin(database)));
				sqlCommand.Parameters.Add(new SqlParameter("@Table", SqlDbType.NVarChar)).Value = TableName;
			}
			else if (TableName.Equals("WAREHOUSES", StringComparison.CurrentCultureIgnoreCase))
			{
				sqlCommand = dataDictionary.NewSqlCommand(string.Format("select dfTable, dfRelatedFields, dfField, dtForeignKeyDeleteFilter,Convert(nvarchar(30),'') As sourceField, {0}  From DDFields Inner Join DDTables On dfTable = dtTable {1} Where dfRelatedTable = @Table And dfBoundParentFieldType = 0 And dtKeyFields Not Like '%' + RTrim(dfField) + '%' And Not Exists(Select * From DDRelations Where drpTable = @Table And drcTable = dfTable And (drPersist <> 0 Or (drPersist = 0 And drDFilter = '0=1')))Union All select drPTable,Convert(nvarchar(30),''),drPField,drDFilter,drCField,{0} from DDRelations Inner Join DDTables On DDTables.dtTable = drPTable Inner Join DDTables childTable On drCTable = childTable.dtTable Inner Join DDFields On childTable.dtUniqueField = dfField {1} Where drcTable = @Table And drForeign <> 0 And drDFilter <> '0=1' And drCField = childTable.dtUniqueField Union All select dfTable,Convert(nvarchar(30),''), dfField, dtForeignKeyDeleteFilter,Convert(nvarchar(30),'') As sourceField, {0}  From DDFields Inner Join DDTables On dfTable = dtTable {1} Where dfRelatedTable = @PartWHTable And dfBoundParentFieldType = 0 And Not Exists(Select * From DDRelations Where drpTable = @PartWHTable And drcTable = dfTable And (drPersist <> 0 Or (drPersist = 0 And drDFilter = '0=1')))Union All select drPTable,Convert(nvarchar(30),''),drPField,drDFilter,drCField,{0} from DDRelations Inner Join DDTables On DDTables.dtTable = drPTable Inner Join DDTables childTable On drCTable = childTable.dtTable Inner Join DDFields On childTable.dtUniqueField = dfField {1} Where drcTable = @PartBinTable And drForeign <> 0 And drDFilter <> '0=1' And drCField = childTable.dtUniqueField Union All select dfTable,Convert(nvarchar(30),''), dfField, dtForeignKeyDeleteFilter,Convert(nvarchar(30),'') As sourceField, {0}  From DDFields Inner Join DDTables On dfTable = dtTable {1} Where dfRelatedTable = @Table AND (dfTable =@PartBinTable)", dataDictionary.Language.GetdtCaptionField(database, removeAsClause: false, "DDTables"), dataDictionary.Language.GetdtCaptionJoin(database)));
				sqlCommand.Parameters.Add(new SqlParameter("@Table", SqlDbType.NVarChar)).Value = TableName;
				sqlCommand.Parameters.Add(new SqlParameter("@PartWHTable", SqlDbType.NVarChar)).Value = "PartWarehouseLocations";
				sqlCommand.Parameters.Add(new SqlParameter("@PartBinTable", SqlDbType.NVarChar)).Value = "PartBins";
			}
			else if (TableName.Equals("WORKCENTERS", StringComparison.CurrentCultureIgnoreCase) || TableName.Equals("PLANTS", StringComparison.CurrentCultureIgnoreCase))
			{
				sqlCommand = dataDictionary.NewSqlCommand(string.Format("select dfTable, dfRelatedFields, dfField, dtForeignKeyDeleteFilter,Convert(nvarchar(30),'') As sourceField,{0},{2}  From DDFields Inner Join DDTables On dfTable = dtTable {1} {3} Where dfRelatedTable = @Table And dfBoundParentFieldType = 0 And dfTable <> dfRelatedTable And Not Exists(Select * From DDRelations Where drpTable = @Table And drcTable = dfTable And (drPersist <> 0 Or (drPersist = 0 And drDFilter = '0=1')))Union All select drPTable,Convert(nvarchar(30),''),drPField,drDFilter,drCField,{0},{2} from DDRelations Inner Join DDTables On DDTables.dtTable = drPTable Inner Join DDTables childTable On drCTable = childTable.dtTable Inner Join DDFields On childTable.dtUniqueField = dfField {1} {3} Where drcTable = @Table And drForeign <> 0 And drDFilter <> '0=1' And drCField = childTable.dtUniqueField", dataDictionary.Language.GetdtCaptionField(database, removeAsClause: false, "DDTables"), dataDictionary.Language.GetdtCaptionJoin(database), dataDictionary.Language.GetdfCaptionField(database, "DDFields", removeAsClause: false), dataDictionary.Language.GetdfCaptionJoin(database)));
				sqlCommand.Parameters.Add(new SqlParameter("@Table", SqlDbType.NVarChar)).Value = TableName;
			}
			else
			{
				string text2 = string.Empty;
				if (TableName.Equals("SALESORDERS", StringComparison.CurrentCultureIgnoreCase) || TableName.Equals("SALESORDERLINES", StringComparison.CurrentCultureIgnoreCase) || TableName.Equals("SALESORDERDELIVERIES", StringComparison.CurrentCultureIgnoreCase) || TableName.Equals("JOBS", StringComparison.CurrentCultureIgnoreCase) || TableName.Equals("JOBASSEMBLIES", StringComparison.CurrentCultureIgnoreCase))
				{
					string salesOrderID = string.Empty;
					string salesOrderLineID = string.Empty;
					string salesOrderDeliveryID = string.Empty;
					string jobID = string.Empty;
					_ = string.Empty;
					if (TableName.Equals("SALESORDERS", StringComparison.CurrentCultureIgnoreCase))
					{
						salesOrderID = row[0].ToString();
					}
					if (TableName.Equals("SALESORDERLINES", StringComparison.CurrentCultureIgnoreCase))
					{
						salesOrderID = row[0].ToString();
						salesOrderLineID = row[1].ToString();
					}
					if (TableName.Equals("SALESORDERDELIVERIES", StringComparison.CurrentCultureIgnoreCase))
					{
						salesOrderID = row[0].ToString();
						salesOrderLineID = row[1].ToString();
						salesOrderDeliveryID = row[2].ToString();
					}
					if (TableName.Equals("JOBS", StringComparison.CurrentCultureIgnoreCase) || TableName.Equals("JOBASSEMBLIES", StringComparison.CurrentCultureIgnoreCase))
					{
						jobID = row[0].ToString();
						List<string> salesOrderLinesKeys = GetSalesOrderLinesKeys(database, jobID);
						if (salesOrderLinesKeys.Count != 0)
						{
							salesOrderID = salesOrderLinesKeys[0];
							salesOrderLineID = salesOrderLinesKeys[1];
							jobID = string.Empty;
						}
					}
					num = NumberRowsExistInDemandsTable(database, salesOrderID, salesOrderLineID, salesOrderDeliveryID, jobID);
					num2 = NumberRowsExistInJobDetailsTable(database, salesOrderID, salesOrderLineID, salesOrderDeliveryID, jobID);
					bool num3 = IsMrpSessionCompleted(database, salesOrderID, salesOrderLineID, salesOrderDeliveryID, jobID);
					flag = num3 && num > 0 && num2 > 0;
					flag2 = num3 && (num > 0 || num2 > 0);
					if (flag || flag2)
					{
						text2 = "AND dfTable NOT IN ('MRPDemands', 'MRPJobDetails')";
					}
				}
				sqlCommand = dataDictionary.NewSqlCommand(string.Format("select dfTable, dfRelatedFields, dfField, dtForeignKeyDeleteFilter,Convert(nvarchar(30),'') As sourceField,{0},{2}  \r\n                            From DDFields \r\n                            Inner Join DDTables On dfTable = dtTable {1} {3} \r\n                            Where dfRelatedTable = @Table And \r\n                                dfBoundParentFieldType = 0 And \r\n                                dtKeyFields Not Like '%' + RTrim(dfField) + '%' And \r\n                                Not Exists(Select * \r\n                                            From DDRelations \r\n                                            Where drpTable = @Table And \r\n                                                drcTable = dfTable AND \r\n                                                dfField = case \r\n                                                            when charindex(',', drCField) > 0 then dfField \r\n                                                          Else drCField End And \r\n                                                (drPersist <> 0 Or (drPersist = 0 And drDFilter = '0=1'))\r\n                                            ) {4}Union All select drPTable,Convert(nvarchar(30),''),drPField,drDFilter,drCField,{0},{2} from DDRelations Inner Join DDTables On DDTables.dtTable = drPTable Inner Join DDTables childTable On drCTable = childTable.dtTable Inner Join DDFields On childTable.dtUniqueField = dfField {1} {3} Where drcTable = @Table And    drForeign <> 0 And    drDFilter <> '0=1' And    drCField = childTable.dtUniqueField    {4}", dataDictionary.Language.GetdtCaptionField(database, removeAsClause: false, "DDTables"), dataDictionary.Language.GetdtCaptionJoin(database), dataDictionary.Language.GetdfCaptionField(database, "DDFields", removeAsClause: false), dataDictionary.Language.GetdfCaptionJoin(database), text2));
				sqlCommand.Parameters.Add(new SqlParameter("@Table", SqlDbType.NVarChar)).Value = TableName;
			}
			DataTable dataTable = dataDictionary.GetDataTable(sqlCommand);
			StringBuilder stringBuilder = new StringBuilder();
			foreach (DataRow row2 in dataTable.Rows)
			{
				string text3 = row2.Field<string>("dfRelatedFields");
				if (text3.Length != 0)
				{
					text3 += ",";
				}
				text3 += row2.Field<string>("dfField");
				string[] array = text3.Split(',');
				stringBuilder.Length = 0;
				string[] array2 = ((!string.IsNullOrWhiteSpace(row2.Field<string>("sourceField"))) ? row2.Field<string>("sourceField").Split(',') : KeyFieldsArray);
				for (int j = 0; j < array2.Length; j++)
				{
					if (stringBuilder.Length != 0)
					{
						stringBuilder.Append(" And ");
					}
					stringBuilder.AppendFormat("{0} = {1}", array[j], row[array2[j]].ToSql());
				}
				if (checkDeleteFilter && row2.Field<string>("dtForeignKeyDeleteFilter").Length != 0)
				{
					stringBuilder.AppendFormat(" And ({0})", row2.Field<string>("dtForeignKeyDeleteFilter"));
				}
				int num5;
				if (TableName.Equals("JOBASSEMBLIES", StringComparison.InvariantCultureIgnoreCase) && array2.Length.Equals(2) && array.Length.Equals(2))
				{
					object o = row[array2[0]];
					object o2 = row[array2[1]];
					int num4 = new Random().Next(10000);
					StringBuilder stringBuilder2 = new StringBuilder();
					stringBuilder2.Append("SET NOCOUNT ON\r");
					stringBuilder2.Append("DECLARE @nSelectedAsm int, @cJob varchar(20)\r");
					stringBuilder2.Append("SET @cJob = " + o.ToSql() + "\r");
					stringBuilder2.Append("SET @nSelectedAsm = " + o2.ToSql() + "\r");
					stringBuilder2.Append($"SELECT jmaJobID,jmaJobAssemblyID INTO #TempJobQuery{num4} FROM JobAssemblies WHERE 0=1\r");
					stringBuilder2.Append($"INSERT INTO #TempJobQuery{num4} (jmaJobID, jmaJobAssemblyID) VALUES (@cJob, @nSelectedAsm)\r");
					stringBuilder2.Append($"INSERT INTO #TempJobQuery{num4} (jmaJobID, jmaJobAssemblyID) SELECT jmaJobID,jmaJobAssemblyID FROM JobAssemblies WHERE jmaJobID = @cJob And jmaParentAssemblyID IN (SELECT jmaJobAssemblyID FROM #TempJobQuery{num4}) AND jmaJobAssemblyID NOT IN (SELECT jmaJobAssemblyID FROM #TempJobQuery{num4})\r");
					stringBuilder2.Append($"INSERT INTO #TempJobQuery{num4} (jmaJobID, jmaJobAssemblyID) SELECT jmaJobID,jmaJobAssemblyID FROM JobAssemblies WHERE jmaJobID = @cJob And jmaParentAssemblyID IN (SELECT jmaJobAssemblyID FROM #TempJobQuery{num4}) AND jmaJobAssemblyID NOT IN (SELECT jmaJobAssemblyID FROM #TempJobQuery{num4})\r");
					stringBuilder2.Append($"INSERT INTO #TempJobQuery{num4} (jmaJobID, jmaJobAssemblyID) SELECT jmaJobID,jmaJobAssemblyID FROM JobAssemblies WHERE jmaJobID = @cJob And jmaParentAssemblyID IN (SELECT jmaJobAssemblyID FROM #TempJobQuery{num4}) AND jmaJobAssemblyID NOT IN (SELECT jmaJobAssemblyID FROM #TempJobQuery{num4})\r");
					stringBuilder2.Append($"INSERT INTO #TempJobQuery{num4} (jmaJobID, jmaJobAssemblyID) SELECT jmaJobID,jmaJobAssemblyID FROM JobAssemblies WHERE jmaJobID = @cJob And jmaParentAssemblyID IN (SELECT jmaJobAssemblyID FROM #TempJobQuery{num4}) AND jmaJobAssemblyID NOT IN (SELECT jmaJobAssemblyID FROM #TempJobQuery{num4})\r");
					stringBuilder2.Append($"INSERT INTO #TempJobQuery{num4} (jmaJobID, jmaJobAssemblyID) SELECT jmaJobID,jmaJobAssemblyID FROM JobAssemblies WHERE jmaJobID = @cJob And jmaParentAssemblyID IN (SELECT jmaJobAssemblyID FROM #TempJobQuery{num4}) AND jmaJobAssemblyID NOT IN (SELECT jmaJobAssemblyID FROM #TempJobQuery{num4})\r");
					stringBuilder2.Append($"INSERT INTO #TempJobQuery{num4} (jmaJobID, jmaJobAssemblyID) SELECT jmaJobID,jmaJobAssemblyID FROM JobAssemblies WHERE jmaJobID = @cJob And jmaParentAssemblyID IN (SELECT jmaJobAssemblyID FROM #TempJobQuery{num4}) AND jmaJobAssemblyID NOT IN (SELECT jmaJobAssemblyID FROM #TempJobQuery{num4})\r");
					stringBuilder2.Append($"INSERT INTO #TempJobQuery{num4} (jmaJobID, jmaJobAssemblyID) SELECT jmaJobID,jmaJobAssemblyID FROM JobAssemblies WHERE jmaJobID = @cJob And jmaParentAssemblyID IN (SELECT jmaJobAssemblyID FROM #TempJobQuery{num4}) AND jmaJobAssemblyID NOT IN (SELECT jmaJobAssemblyID FROM #TempJobQuery{num4})\r");
					stringBuilder2.Append($"INSERT INTO #TempJobQuery{num4} (jmaJobID, jmaJobAssemblyID) SELECT jmaJobID,jmaJobAssemblyID FROM JobAssemblies WHERE jmaJobID = @cJob And jmaParentAssemblyID IN (SELECT jmaJobAssemblyID FROM #TempJobQuery{num4}) AND jmaJobAssemblyID NOT IN (SELECT jmaJobAssemblyID FROM #TempJobQuery{num4})\r");
					stringBuilder2.Append($"INSERT INTO #TempJobQuery{num4} (jmaJobID, jmaJobAssemblyID) SELECT jmaJobID,jmaJobAssemblyID FROM JobAssemblies WHERE jmaJobID = @cJob And jmaParentAssemblyID IN (SELECT jmaJobAssemblyID FROM #TempJobQuery{num4}) AND jmaJobAssemblyID NOT IN (SELECT jmaJobAssemblyID FROM #TempJobQuery{num4})\r");
					stringBuilder2.Append("SET NOCOUNT OFF\r");
					stringBuilder2.Append(string.Format("SELECT SUM(TableCount) As TableCount From (SELECT IsNull((select count(*) from {0} Where {1}=#TempJobQuery{2}.jmaJobID and {3}=#TempJobQuery{4}.jmaJobAssemblyID),0) as TableCount\r", row2.Field<string>("dfTable"), array[0], num4, array[1], num4));
					stringBuilder2.Append($"FROM #TempJobQuery{num4} INNER JOIN JobAssemblies ON JobAssemblies.jmaJobID = #TempJobQuery{num4}.jmaJobID And JobAssemblies.jmaJobAssemblyID = #TempJobQuery{num4}.jmaJobAssemblyID inner join Jobs on JobAssemblies.jmaJobID = jmpJobID) as test\r");
					stringBuilder2.Append($"DROP TABLE #TempJobQuery{num4};\r");
					num5 = 0;
					try
					{
						num5 = (int)database.ExecuteScalar(stringBuilder2.ToString());
					}
					catch
					{
						stringBuilder2 = new StringBuilder();
						stringBuilder2.Append($"DROP TABLE #TempJobQuery{num4};\r");
						database.ExecuteScalar(stringBuilder2.ToString());
					}
				}
				else
				{
					num5 = (int)database.ExecuteScalar(string.Format("Select IsNull(Count(*),0) From {0} Where {1}", row2.Field<string>("dfTable"), stringBuilder));
				}
				if (num5 != 0)
				{
					text = ((TableName.Equals("ORGANIZATIONS", StringComparison.CurrentCultureIgnoreCase) || TableName.Equals("ORGANIZATIONLOCATIONS", StringComparison.CurrentCultureIgnoreCase)) ? (text + num5 + " " + row2.Field<string>("dtCaption") + " (" + row2.Field<string>("dfCaption") + ")\r\n") : (text + num5 + " " + row2.Field<string>("dtCaption") + "\r\n"));
				}
			}
			if (text.Length != 0)
			{
				text = Fields[LastKeyField].RelatedFieldsFormatCaptionAndCurrentValues(row) + " is used in the following places:\r\n" + text;
			}
		}
		if (((text == string.Empty && flag) || (text == string.Empty && flag2)) && MessageBox.Show(string.Concat(Fields[LastKeyField].RelatedFieldsFormatCaptionAndCurrentValues(row) + " is used in Completed MRP Session(s):\r\n" + BuildMessageForRowsThatExistsInMRPTables(num, num2), "\r\nAre you sure?"), "Confirm Remove?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
		{
			text = "Skip";
		}
		return text;
	}

	private string BuildMessageForRowsThatExistsInMRPTables(int numberOfRowsExistsInDemandTables, int numberOfRowsExistsInJobDetailsTable)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (numberOfRowsExistsInDemandTables != 0)
		{
			stringBuilder.AppendLine($"{numberOfRowsExistsInDemandTables} Mfg Req Planner Demands");
		}
		if (numberOfRowsExistsInJobDetailsTable != 0)
		{
			stringBuilder.AppendLine($"{numberOfRowsExistsInJobDetailsTable} Mfg Req Planner Job Details");
		}
		return stringBuilder.ToString();
	}

	private bool IsMrpSessionCompleted(M1Database database, string salesOrderID, string salesOrderLineID, string salesOrderDeliveryID, string jobID)
	{
		string text = (string.IsNullOrEmpty(salesOrderID) ? string.Empty : "WHERE mrrSalesOrderID = @SalesOrderID");
		string text2 = (string.IsNullOrEmpty(salesOrderLineID) ? string.Empty : "AND mrrSalesOrderLineID = @salesOrderLineID");
		string text3 = (string.IsNullOrEmpty(salesOrderDeliveryID) ? string.Empty : "AND mrrSalesOrderDeliveryID = @salesOrderDeliveryID");
		string text4 = (string.IsNullOrEmpty(salesOrderID) ? string.Empty : "WHERE mrjSalesOrderID = @SalesOrderID");
		string text5 = (string.IsNullOrEmpty(salesOrderLineID) ? string.Empty : "AND mrjSalesOrderLineID = @salesOrderLineID");
		string text6 = (string.IsNullOrEmpty(salesOrderDeliveryID) ? string.Empty : "AND mrjSalesOrderDeliveryID = @salesOrderDeliveryID");
		string text7 = (string.IsNullOrEmpty(jobID) ? string.Empty : "WHERE mrrJobID = @JobID");
		string text8 = (string.IsNullOrEmpty(jobID) ? string.Empty : "WHERE mrjJobID = @JobID");
		SqlCommand sqlCommand = database.NewSqlCommand(string.Format(" SELECT COUNT(*)\r\n                                                    FROM MRPSessions s\r\n                                                    INNER JOIN (\r\n\t\t\t                                                    SELECT DISTINCT a1.sessionID\r\n\t\t\t                                                    FROM(\r\n\t\t\t\t\t                                                    SELECT mrrSessionID AS sessionID FROM MRPDemands {0} {1} {2} {6}\r\n\t\t\t\t\t                                                    UNION ALL\r\n\t\t\t\t\t                                                    SELECT mrjSessionID AS sessionID FROM MRPJobDetails {3} {4} {5} {7}\r\n\t\t\t                                                    ) a1\r\n                                                    ) a2 ON a2.sessionID = s.mrpSessionID\r\n                                                    WHERE s.mrpCompleted = 1", text, text2, text3, text4, text5, text6, text7, text8));
		sqlCommand.Parameters.Add(new SqlParameter("@SalesOrderID", SqlDbType.NVarChar)).Value = salesOrderID;
		sqlCommand.Parameters.Add(new SqlParameter("@SalesOrderLineID", SqlDbType.NVarChar)).Value = salesOrderLineID;
		sqlCommand.Parameters.Add(new SqlParameter("@SalesOrderDeliveryID", SqlDbType.NVarChar)).Value = salesOrderDeliveryID;
		sqlCommand.Parameters.Add(new SqlParameter("@JobID", SqlDbType.NVarChar)).Value = jobID;
		return Convert.ToInt32(database.ExecuteScalar(sqlCommand)) > 0;
	}

	private int NumberRowsExistInDemandsTable(M1Database database, string salesOrderID, string salesOrderLineID, string salesOrderDeliveryID, string jobID)
	{
		string text = (string.IsNullOrEmpty(salesOrderID) ? string.Empty : "WHERE mrrSalesOrderID = @SalesOrderID");
		string text2 = (string.IsNullOrEmpty(salesOrderLineID) ? string.Empty : "AND mrrSalesOrderLineID = @salesOrderLineID");
		string text3 = (string.IsNullOrEmpty(salesOrderDeliveryID) ? string.Empty : "AND mrrSalesOrderDeliveryID = @salesOrderDeliveryID");
		string text4 = (string.IsNullOrEmpty(jobID) ? string.Empty : "WHERE mrrJobID = @JobID");
		SqlCommand sqlCommand = database.NewSqlCommand($"SELECT COUNT(*) FROM MRPDemands {text} {text2} {text3} {text4}");
		sqlCommand.Parameters.Add(new SqlParameter("@SalesOrderID", SqlDbType.NVarChar)).Value = salesOrderID;
		sqlCommand.Parameters.Add(new SqlParameter("@SalesOrderLineID", SqlDbType.NVarChar)).Value = salesOrderLineID;
		sqlCommand.Parameters.Add(new SqlParameter("@SalesOrderDeliveryID", SqlDbType.NVarChar)).Value = salesOrderDeliveryID;
		sqlCommand.Parameters.Add(new SqlParameter("@JobID", SqlDbType.NVarChar)).Value = jobID;
		return Convert.ToInt32(database.ExecuteScalar(sqlCommand));
	}

	private int NumberRowsExistInJobDetailsTable(M1Database database, string salesOrderID, string salesOrderLineID, string salesOrderDeliveryID, string jobID)
	{
		string text = (string.IsNullOrEmpty(salesOrderID) ? string.Empty : "WHERE mrjSalesOrderID = @SalesOrderID");
		string text2 = (string.IsNullOrEmpty(salesOrderLineID) ? string.Empty : "AND mrjSalesOrderLineID = @salesOrderLineID");
		string text3 = (string.IsNullOrEmpty(salesOrderDeliveryID) ? string.Empty : "AND mrjSalesOrderDeliveryID = @salesOrderDeliveryID");
		string text4 = (string.IsNullOrEmpty(jobID) ? string.Empty : "WHERE mrjJobID = @JobID");
		SqlCommand sqlCommand = database.NewSqlCommand($"SELECT COUNT(*) FROM MRPJobDetails {text} {text2} {text3} {text4}");
		sqlCommand.Parameters.Add(new SqlParameter("@SalesOrderID", SqlDbType.NVarChar)).Value = salesOrderID;
		sqlCommand.Parameters.Add(new SqlParameter("@SalesOrderLineID", SqlDbType.NVarChar)).Value = salesOrderLineID;
		sqlCommand.Parameters.Add(new SqlParameter("@SalesOrderDeliveryID", SqlDbType.NVarChar)).Value = salesOrderDeliveryID;
		sqlCommand.Parameters.Add(new SqlParameter("@JobID", SqlDbType.NVarChar)).Value = jobID;
		return Convert.ToInt32(database.ExecuteScalar(sqlCommand));
	}

	private List<string> GetSalesOrderLinesKeys(M1Database database, string jobID)
	{
		List<string> list = new List<string>();
		SqlCommand sqlCommand = database.NewSqlCommand("SELECT omjSalesOrderID, omjSalesOrderLineID FROM SalesOrderJobLinks WHERE omjJobID = @JobID");
		sqlCommand.Parameters.Add(new SqlParameter("@JobID", SqlDbType.NVarChar)).Value = jobID;
		DataTable dataTable = database.GetDataTable(sqlCommand);
		if (dataTable.Rows.Count != 0)
		{
			list.Add(dataTable.Rows[0].Field<string>("omjSalesOrderID"));
			list.Add(dataTable.Rows[0].Field<short>("omjSalesOrderLineID").ToString());
		}
		return list;
	}

	private void _BindingSource_CurrentChanged(object sender, EventArgs e)
	{
		if (BindingSource.Position != -1)
		{
			DataRow currentAsDataRow = BindingSource.CurrentAsDataRow;
			M1Database currentDatabase = BindingSource.CurrentDatabase;
			EvaluateNoAccess(currentDatabase, currentAsDataRow, BindingSource.Transaction);
			EvaluateReadOnlyExpression(currentDatabase, currentAsDataRow, BindingSource.Transaction);
			EvaluateOverrideDeleteEnabledExpression(currentDatabase, currentAsDataRow, BindingSource.Transaction);
			EvaluateDisableAddNewExpression(currentDatabase, currentAsDataRow, BindingSource.Transaction);
			if (BindingSource.IsTopLevel)
			{
				setCurrencyRateIdForeign(GetForeignCurrencyRateIdForRow(currentDatabase, currentAsDataRow), currentDatabase);
			}
		}
	}

	private void setCurrencyRateIdForeign(string rateId, M1Database database)
	{
		if (!_CurrencyRateIdForeign.Equals(rateId))
		{
			_CurrencyRateIdForeign = rateId;
			CurrencySymbolForeign = database.GetForeignCurrencySymbolForRateId(rateId);
			OnCurrencyRateIdForeignChanged(EventArgs.Empty);
		}
	}

	private void setCurrencyRateIdForeign(string rateId, string currencySymbol)
	{
		if (!_CurrencyRateIdForeign.Equals(rateId))
		{
			_CurrencyRateIdForeign = rateId;
			CurrencySymbolForeign = currencySymbol;
			OnCurrencyRateIdForeignChanged(EventArgs.Empty);
		}
	}

	private void OnCurrencyRateIdForeignChanged(EventArgs e)
	{
		this.CurrencyRateIdForeignChanged?.Invoke(this, e);
	}

	public void LoadComplete(FieldCollection fields, FieldDefinition field, bool allowEditing)
	{
		field.LoadComplete(fields, allowEditing);
	}

	public void LoadComplete(FieldCollection fields, bool allowEditing, bool isDesignMode)
	{
		scriptEngine = new ScriptingEventBinding(BindingSource.Database);
		scriptEngine.LoadEnvironment(useConnectionProxy: true);
		scriptEngine.AddObject("Fields", fields);
		scriptEngine.AddObject("Record", this);
		scriptEngine.AddObject("ParentInfo", this);
		if (BindingSource.Database != null && !isDesignMode && allowEditing)
		{
			ReferencedFieldsList referencedFieldsList = new ReferencedFieldsList();
			scriptEngine.BindCodeEvents(BindingSource.DataDictionary, "DDTables", UniqueID.Value, TableName, this, BindingSource.Fields, referencedFieldsList);
			FieldDefinition.ProcessSubFieldReferences(fields, referencedFieldsList.SubFieldReferences);
		}
		if (BindingSource != null && Databases.Count != 0 && BindingSource.Database != null)
		{
			EvaluateNoAccess(BindingSource.Database, null, BindingSource.Transaction);
			EvaluateReadOnlyExpression(BindingSource.Database, null, BindingSource.Transaction);
		}
		if (allowEditing)
		{
			foreach (FieldDefinition field in fields)
			{
				if (field.Table != this || field.FieldExtensions == null)
				{
					continue;
				}
				foreach (FieldExtension fieldExtension in field.FieldExtensions)
				{
					fieldExtension.LoadComplete(fields, allowEditing);
				}
			}
		}
		foreach (FieldDefinition field2 in fields)
		{
			if (field2.Table == this)
			{
				LoadComplete(fields, field2, allowEditing);
			}
		}
		if (!allowEditing)
		{
			return;
		}
		if (BindingSource.BoundFieldDefinition != null && EnterInSequenceField.Length != 0 && BindingSource.Fields.Contains(EnterInSequenceField))
		{
			fields[EnterInSequenceField].Valid -= fieldEntryOrder_Valid;
			fields[EnterInSequenceField].Valid += fieldEntryOrder_Valid;
		}
		foreach (string validCodeReferencedField in ValidCodeReferencedFields)
		{
			FieldDefinition fieldDefinition = fields[validCodeReferencedField];
			fieldDefinition.ValueChanged -= field_ReferencedValidCodeField_ValueChanged;
			fieldDefinition.ValueChanged += field_ReferencedValidCodeField_ValueChanged;
		}
		if (getChildRowCountReferenced)
		{
			BindingSource.AddNewCompleted -= ChildRowCount_BindingSource_AddNewCompleted;
			BindingSource.AddNewCompleted += ChildRowCount_BindingSource_AddNewCompleted;
		}
		foreach (string readOnlyExpressionReferencedField in ReadOnlyExpressionReferencedFields)
		{
			FieldDefinition fieldDefinition2 = fields[readOnlyExpressionReferencedField];
			fieldDefinition2.ValueChanged -= RelatedReadOnlyFieldValueChanged;
			fieldDefinition2.ValueChanged += RelatedReadOnlyFieldValueChanged;
		}
		FieldDefinition.ProcessSubFieldReferences(fields, ReadOnlyExpressionReferencedFields.SubFieldReferences);
		if (!string.IsNullOrWhiteSpace(ReadOnlyExpression) && ReadOnlyExpression.IndexOf(".EditMode", StringComparison.CurrentCultureIgnoreCase) != -1)
		{
			BindingSource.SaveDataCompleted -= BindingSource_SaveDataCompleted_EditModeCheck;
			BindingSource.SaveDataCompleted += BindingSource_SaveDataCompleted_EditModeCheck;
		}
		foreach (string disableDeleteExpressionReferencedField in DisableDeleteExpressionReferencedFields)
		{
			FieldDefinition fieldDefinition3 = fields[disableDeleteExpressionReferencedField];
			fieldDefinition3.ValueChanged -= field_DisableDeleteExpression_ValueChanged;
			fieldDefinition3.ValueChanged += field_DisableDeleteExpression_ValueChanged;
		}
		foreach (string overrideDeleteEnabledExpressionReferencedField in OverrideDeleteEnabledExpressionReferencedFields)
		{
			FieldDefinition fieldDefinition4 = fields[overrideDeleteEnabledExpressionReferencedField];
			fieldDefinition4.ValueChanged -= field_OverrideDeleteEnabledExpression_ValueChanged;
			fieldDefinition4.ValueChanged += field_OverrideDeleteEnabledExpression_ValueChanged;
		}
		BindingSource.RowUpdateDeleteBefore -= BindingSource_RowUpdateDeleteBefore;
		BindingSource.RowUpdateDeleteBefore += BindingSource_RowUpdateDeleteBefore;
		BindingSource.RowUpdateDeleteAfter -= BindingSource_RowUpdateDeleteAfter;
		BindingSource.RowUpdateDeleteAfter += BindingSource_RowUpdateDeleteAfter;
		BindingSource.RemoveStarted -= BindingSource_RemoveStarted;
		BindingSource.RemoveStarted += BindingSource_RemoveStarted;
		BindingSource.RemoveCompleted -= BindingSource_RemoveCompleted;
		BindingSource.RemoveCompleted += BindingSource_RemoveCompleted;
		BindingSource.RowUpdateAddBefore -= BindingSource_RowUpdateAddBefore;
		BindingSource.RowUpdateAddBefore += BindingSource_RowUpdateAddBefore;
		BindingSource.RowUpdateSaveBefore -= BindingSource_RowUpdateSaveBefore;
		BindingSource.RowUpdateSaveBefore += BindingSource_RowUpdateSaveBefore;
		BindingSource.RowUpdateAddAfter -= BindingSource_RowUpdateAddAfter;
		BindingSource.RowUpdateAddAfter += BindingSource_RowUpdateAddAfter;
		BindingSource.RowUpdateSaveAfter -= BindingSource_RowUpdateSaveAfter;
		BindingSource.RowUpdateSaveAfter += BindingSource_RowUpdateSaveAfter;
		BindingSource.SaveDataCompleted -= BindingSource_SaveDataCompleted;
		BindingSource.SaveDataCompleted += BindingSource_SaveDataCompleted;
		BindingSource.CurrentChanged -= _BindingSource_CurrentChanged;
		BindingSource.CurrentChanged += _BindingSource_CurrentChanged;
		if (CurrencyRateIdField.Length != 0 && BindingSource.Fields.Contains(CurrencyRateIdField))
		{
			BindingSource.Fields[CurrencyRateIdField].ValueChanged -= CurrencyRateIdField_ValueChanged;
			BindingSource.Fields[CurrencyRateIdField].ValueChanged += CurrencyRateIdField_ValueChanged;
		}
		if (DocumentDateField.Length != 0 && BindingSource.Fields.Contains(DocumentDateField))
		{
			BindingSource.Fields[DocumentDateField].ValueChanged -= Field_ValueChanged_CurrencyUpdateExchangeRate;
			BindingSource.Fields[DocumentDateField].ValueChanged += Field_ValueChanged_CurrencyUpdateExchangeRate;
		}
		if (CurrencyExchangeRateField.Length != 0 && BindingSource.Fields.Contains(CurrencyExchangeRateField))
		{
			BindingSource.Fields[CurrencyExchangeRateField].ValueChanged -= ExchangeRateField_ValueChanged;
			BindingSource.Fields[CurrencyExchangeRateField].ValueChanged += ExchangeRateField_ValueChanged;
		}
	}

	private void BindingSource_SaveDataCompleted_EditModeCheck(object sender, EventArgs e)
	{
		DataRow currentAsDataRow = BindingSource.CurrentAsDataRow;
		M1Database currentDatabase = BindingSource.CurrentDatabase;
		if (currentAsDataRow != null && currentDatabase != null)
		{
			EvaluateReadOnlyExpression(currentDatabase, currentAsDataRow, BindingSource.Transaction);
		}
	}

	private void ChildRowCount_BindingSource_AddNewCompleted(object sender, DbAndRowEventArgs e)
	{
		DataRow[] array = BindingSource.GetDataTable().Select(string.Empty, string.Empty, DataViewRowState.Added | DataViewRowState.ModifiedCurrent);
		foreach (DataRow dataRow in array)
		{
			Validate(BindingSource.GetDatabaseForRow(dataRow), dataRow, e.SqlTransaction, BindingSource.IsTopLevel, e.Row == dataRow);
		}
	}

	private void OnRemoveStarted(RemoveEventArgs e)
	{
		this.RemoveStarted?.Invoke(this, e);
	}

	protected void OnRemoveCompleted(RemoveEventArgs e)
	{
		this.RemoveCompleted?.Invoke(this, e);
	}

	private void BindingSource_RemoveStarted(object sender, RemoveEventArgs e)
	{
		OnRemoveStarted(e);
	}

	private void BindingSource_RemoveCompleted(object sender, RemoveEventArgs e)
	{
		OnRemoveCompleted(e);
	}

	public void OnGetNextID(GetNextIDEventArgs e)
	{
		this.GetNextID?.Invoke(this, e);
	}

	private void ExchangeRateField_ValueChanged(object sender, FieldDefinition.FieldValueChangedEventArgs e)
	{
		if (e.Row.RowState == DataRowState.Modified || e.Row.RowState == DataRowState.Unchanged)
		{
			VerifyChildBindingSourcesForCurrencyLinks(ChildCurrencyLinks);
		}
		ExchangeRateChangedEventArgs e2 = new ExchangeRateChangedEventArgs(e);
		e2.UpdateBaseCurrencyFields = ShouldCurrencyRefreshUpdateBase(e.Database, e.Row, null);
		OnExchangeRateChanged(e2);
	}

	protected void OnUpdateStarted(RowUpdateEventArgs e)
	{
		this.UpdateStarted?.Invoke(this, e);
	}

	protected void OnUpdateCompleted(RowUpdateEventArgs e)
	{
		this.UpdateCompleted?.Invoke(this, e);
	}

	protected void OnDeleteStarted(RowUpdateEventArgs e)
	{
		this.DeleteStarted?.Invoke(this, e);
	}

	protected void OnDeleteCompleted(RowUpdateEventArgs e)
	{
		this.DeleteCompleted?.Invoke(this, e);
	}

	protected void OnSaveDataCompleted(SaveDataCompletedEventArgs e)
	{
		this.SaveDataCompleted?.Invoke(this, e);
	}

	private void BindingSource_RowUpdateSaveAfter(object sender, RowUpdateEventArgs e)
	{
		OnUpdateCompleted(e);
	}

	private void BindingSource_RowUpdateAddAfter(object sender, RowUpdateEventArgs e)
	{
		OnUpdateCompleted(e);
	}

	private void BindingSource_RowUpdateSaveBefore(object sender, RowUpdateEventArgs e)
	{
		SaveDataRowCheckReferenceLinks(e.Database, e.Row, e.SqlTransaction);
		OnUpdateStarted(e);
	}

	private void BindingSource_RowUpdateAddBefore(object sender, RowUpdateEventArgs e)
	{
		OnUpdateStarted(e);
	}

	private void BindingSource_RowUpdateDeleteBefore(object sender, RowUpdateEventArgs e)
	{
		OnDeleteStarted(e);
	}

	private void BindingSource_RowUpdateDeleteAfter(object sender, RowUpdateEventArgs e)
	{
		OnDeleteCompleted(e);
	}

	private void BindingSource_SaveDataCompleted(object sender, SaveDataCompletedEventArgs e)
	{
		OnSaveDataCompleted(e);
	}

	private void field_ReferencedValidCodeField_ValueChanged(object sender, FieldDefinition.FieldValueChangedEventArgs e)
	{
		Validate(e.Database, e.Row, e.SqlTransaction, BindingSource.IsTopLevel, e.IsCurrentRow);
	}

	private void field_DisableDeleteExpression_ValueChanged(object sender, FieldDefinition.FieldValueChangedEventArgs e)
	{
		if (e.IsCurrentRow)
		{
			EvaluateDisableDeleteExpression(e.Database, e.Row, e.SqlTransaction);
		}
	}

	private void field_OverrideDeleteEnabledExpression_ValueChanged(object sender, FieldDefinition.FieldValueChangedEventArgs e)
	{
		if (e.IsCurrentRow)
		{
			EvaluateOverrideDeleteEnabledExpression(e.Database, e.Row, e.SqlTransaction);
		}
	}

	public DbAndRowEventArgs GetCurrentDataRowForProcessingQuick()
	{
		if (scriptEngine != null)
		{
			return scriptEngine.ProcessingArgs;
		}
		return null;
	}

	public DataRow GetCurrentDataRowForProcessing()
	{
		if (scriptEngine == null || scriptEngine.ProcessingArgs == null || scriptEngine.ProcessingArgs.Row == null)
		{
			if (BindingSource == null)
			{
				return null;
			}
			return BindingSource.CurrentAsDataRow;
		}
		return scriptEngine.ProcessingArgs.Row;
	}

	public DbAndRowEventArgs SetCurrentDataRowForProcessingQuick(DbAndRowEventArgs arg)
	{
		return scriptEngine.SetCurrentDataRowForProcessingQuick(arg);
	}

	public object EvaluateScriptExpression(string expr, M1Database database, DataRow row)
	{
		return EvaluateScriptExpression(expr, database, row, null);
	}

	public object EvaluateScriptExpression(string expr, M1Database database, DataRow row, SqlTransaction transaction)
	{
		DbAndRowEventArgs currentDataRowForProcessingQuick = scriptEngine.SetCurrentDataRowForProcessingQuick(new DbAndRowEventArgs(database, row, transaction));
		try
		{
			return scriptEngine.Eval(expr);
		}
		finally
		{
			scriptEngine.SetCurrentDataRowForProcessingQuick(currentDataRowForProcessingQuick);
		}
	}

	public void ExecuteEvent(string eventName, object eventSender, object eventArgs)
	{
		scriptEngine.ExecuteEvent(eventName, eventSender, eventArgs);
	}

	public void ExecuteScript(string code, M1Database database, DataRow row)
	{
		DbAndRowEventArgs currentDataRowForProcessingQuick = scriptEngine.SetCurrentDataRowForProcessingQuick(new DbAndRowEventArgs(database, row, null));
		try
		{
			scriptEngine.ExecuteStatement(code);
		}
		finally
		{
			scriptEngine.SetCurrentDataRowForProcessingQuick(currentDataRowForProcessingQuick);
		}
	}

	public bool EvaluateScriptExpressionBool(string expr, M1Database database, DataRow row)
	{
		DbAndRowEventArgs currentDataRowForProcessingQuick = scriptEngine.SetCurrentDataRowForProcessingQuick(new DbAndRowEventArgs(database, row, null));
		try
		{
			return !M1Util.IsNullOrEmpty(scriptEngine.Eval(expr));
		}
		finally
		{
			scriptEngine.SetCurrentDataRowForProcessingQuick(currentDataRowForProcessingQuick);
		}
	}

	public bool EvaluateScriptExpressionBool(string expr, M1Database database, DataRow row, SqlTransaction transaction)
	{
		DbAndRowEventArgs currentDataRowForProcessingQuick = scriptEngine.SetCurrentDataRowForProcessingQuick(new DbAndRowEventArgs(database, row, transaction));
		try
		{
			return !M1Util.IsNullOrEmpty(scriptEngine.Eval(expr));
		}
		finally
		{
			scriptEngine.SetCurrentDataRowForProcessingQuick(currentDataRowForProcessingQuick);
		}
	}

	public bool EvaluateScriptExpressionBool(string expr, string exprUserToOr, M1Database database, DataRow row, SqlTransaction transaction)
	{
		expr = expr.Trim();
		exprUserToOr = exprUserToOr.Trim();
		if (exprUserToOr != null && exprUserToOr.Length != 0)
		{
			expr = ((expr != null && expr.Length != 0) ? $"({expr}) Or ({exprUserToOr})" : ((expr != null) ? (expr + exprUserToOr) : exprUserToOr));
		}
		if (expr != null && expr.Length != 0)
		{
			if (expr.Equals("True", StringComparison.CurrentCultureIgnoreCase))
			{
				return true;
			}
			return EvaluateScriptExpressionBool(expr, database, row, transaction);
		}
		return false;
	}

	public TableDefinition GetHighestLoadedTopLevelTable()
	{
		TableDefinition tableDefinition = this;
		while (tableDefinition.ParentTableLinkField != null && tableDefinition.ParentBindingSource != null)
		{
			tableDefinition = tableDefinition.ParentBindingSource.Tables[tableDefinition.ParentTableName];
			if (tableDefinition.BindingSource.IsTopLevel)
			{
				break;
			}
		}
		return tableDefinition;
	}

	protected void OnParentBindingSourceChanged(ParentBindingSourceChangedEventArgs e)
	{
		this.ParentBindingSourceChanged?.Invoke(this, e);
	}

	private FieldDefinition getParentLinkField(M1BindingSource parentBs)
	{
		if (BindingSource.ChildLinkField.Length != 0)
		{
			return BindingSource.Fields[BindingSource.ChildLinkField];
		}
		if (KeyFieldsArray.Length >= 2)
		{
			if (parentBs != null)
			{
				for (int num = KeyFieldsArray.Length - 2; num >= 0; num--)
				{
					if (BindingSource.Fields[KeyFieldsArray[num]].RelatedTable.Equals(parentBs.PrimaryTable.TableName, StringComparison.CurrentCultureIgnoreCase))
					{
						return BindingSource.Fields[KeyFieldsArray[num]];
					}
				}
			}
			return BindingSource.Fields[KeyFieldsArray[KeyFieldsArray.Length - 2]];
		}
		return null;
	}

	public void VerifyParentBindingSource(M1BindingSource mainParentBs, bool forceParentLoad, bool overrideForceLoad = false)
	{
		if (!allowEditingOverride)
		{
			return;
		}
		ParentTableLinkField = getParentLinkField(mainParentBs);
		if (ParentTableLinkField == null)
		{
			return;
		}
		if (mainParentBs == null || !ParentTableLinkField.IsPartOfKey || overrideForceLoad)
		{
			bool flag = false;
			foreach (FieldDefinition field in BindingSource.Fields)
			{
				if (field.Table == this && field.BoundParentField.Length != 0 && field.BoundParentFieldType == FieldDefinition.BoundParentFieldTypeEnum.ToParent)
				{
					flag = true;
					break;
				}
			}
			if (flag || forceParentLoad || overrideForceLoad)
			{
				manuallyLoadedParentBindingSource = new M1BindingSource(BindingSource.Database, isManuallyAdded: true)
				{
					PrimaryBindingSource = BindingSource.PrimaryBindingSource
				};
				((ISupportInitialize)manuallyLoadedParentBindingSource).BeginInit();
				manuallyLoadedParentBindingSource.DataSourceTable = ParentTableLinkField.RelatedTable;
				((ISupportInitialize)manuallyLoadedParentBindingSource).EndInit();
				_ParentBindingTableName = ParentTableLinkField.RelatedTable;
				_ParentBindingKeyFieldsArray = ParentTableLinkField.RelatedTableKeyFieldsArray;
				BindingSource.SaveDataStarted += BindingSource_SaveDataStarted_UpdateManuallyAdded;
				BindingSource.SaveDataCompleted += BindingSource_SaveDataCompleted_UpdateManuallyAdded;
				BindingSource.RowActivated += BindingSource_RowActivated_UpdateManuallyAdded;
				BindingSource.CacheCleared += BindingSource_CacheCleared_UpdateManuallyAdded;
				BindingSource.EditCancelled += BindingSource_EditCancelled_UpdateManuallyAdded;
				ParentBindingSource = manuallyLoadedParentBindingSource;
				if (BindingSource.CurrentDatabase != null && BindingSource.CurrentAsDataRow != null)
				{
					ReloadManuallyLoadedParentBindingSource();
				}
			}
		}
		else
		{
			ParentBindingSource = mainParentBs;
			_ParentBindingTableName = ParentBindingSource.PrimaryTable.TableName;
			_ParentBindingKeyFieldsArray = ParentBindingSource.PrimaryTable.KeyFieldsArray;
		}
		if (ParentBindingSource != null && ParentTableLinkField != null)
		{
			ParentBindingSource.Tables[_ParentBindingTableName].CurrencyRateIdForeignChanged += ParentTable_CurrencyRateIdForeignChanged;
			ParentBindingSource.Tables[_ParentBindingTableName].ExchangeRateChanged += ParentTable_ExchangeRateChanged;
			ParentBindingSource.Fields[_ParentBindingKeyFieldsArray[_ParentBindingKeyFieldsArray.Length - 1]].ValueChanged += ParentLastKeyField_ValueChanged;
			ParentBindingSource.Tables[_ParentBindingTableName].KeyChange += ParentTable_KeyChange;
			checkBsForCountInValid();
		}
	}

	public void ReloadManuallyLoadedParentBindingSource()
	{
		manuallyLoadedParentBindingSource?.NavigateTo(BindingSource.CurrentDatabase, getFilterForParentFieldsUsingCurrentTable(BindingSource.CurrentAsDataRow, BindingSource, manuallyLoadedParentBindingSource), string.Empty);
	}

	private void ParentTable_KeyChange(object sender, KeyChangeEventArgs e)
	{
		setKeysToSameAsParent(e.Row, e.PreviousValues);
	}

	internal void ParentLastKeyField_ValueChanged(object sender, FieldDefinition.FieldValueChangedEventArgs e)
	{
		if (ParentBindingSource != null && !ParentBindingSource.Tables[_ParentBindingTableName].SettingKeysToSameAsParent)
		{
			string[] keyFieldsArray = ParentBindingSource.Tables[_ParentBindingTableName].KeyFieldsArray;
			object[] array = new object[keyFieldsArray.Length];
			for (int i = 0; i < keyFieldsArray.Length; i++)
			{
				array[i] = e.Row[keyFieldsArray[i]];
			}
			array[keyFieldsArray.Length - 1] = e.PreviousValue;
			setKeysToSameAsParent(e.Row, array);
		}
	}

	private void OnKeyChange(KeyChangeEventArgs e)
	{
		this.KeyChange?.Invoke(this, e);
	}

	private void setKeysToSameAsParent(DataRow parentDataRow, object[] previousValues)
	{
		bool settingKeysToSameAsParent = SettingKeysToSameAsParent;
		SettingKeysToSameAsParent = true;
		string[] array = ((!KeyFieldsArray.Contains(ParentTableLinkField.FieldName, StringComparer.CurrentCultureIgnoreCase)) ? ParentTableLinkField.RelatedFieldsAndCurrentFieldArray : KeyFieldsArray);
		DataTable dataTable = BindingSource.GetDataTable();
		if (dataTable != null)
		{
			bool flag = true;
			KeyChangeEventArgs e = new KeyChangeEventArgs();
			object[] array2 = new object[array.Length];
			object[] array3 = new object[array.Length];
			foreach (DataRow row in dataTable.Rows)
			{
				if (row.RowState == DataRowState.Deleted)
				{
					continue;
				}
				flag = true;
				for (int i = 0; i < _ParentBindingKeyFieldsArray.Length; i++)
				{
					if ((previousValues[i] == DBNull.Value && row[array[i]] != DBNull.Value) || (previousValues[i] == null && row[array[i]] != null) || !previousValues[i].Equals(row[array[i]]))
					{
						flag = false;
						break;
					}
				}
				if (!flag)
				{
					continue;
				}
				DbAndRowEventArgs currentDataRowForProcessingQuick = scriptEngine.SetCurrentDataRowForProcessingQuick(new DbAndRowEventArgs(BindingSource.GetDatabaseForRow(row), row, null));
				try
				{
					for (int j = 0; j < array.Length; j++)
					{
						array2[j] = row[array[j]];
						array3[j] = row[array[j]];
					}
					for (int k = 0; k < _ParentBindingKeyFieldsArray.Length; k++)
					{
						array3[k] = parentDataRow[_ParentBindingKeyFieldsArray[k]];
					}
					BindingSource.SetFields(row, array, array3);
					e.Row = row;
					e.PreviousValues = array2;
					OnKeyChange(e);
				}
				finally
				{
					scriptEngine.SetCurrentDataRowForProcessingQuick(currentDataRowForProcessingQuick);
				}
			}
		}
		SettingKeysToSameAsParent = settingKeysToSameAsParent;
	}

	private void ParentTable_ExchangeRateChanged(object sender, ExchangeRateChangedEventArgs e)
	{
		DataTable dataTable = BindingSource.GetDataTable();
		if (dataTable != null)
		{
			ExchangeRateChangedEventArgs e2 = new ExchangeRateChangedEventArgs(e);
			e2.Database = e.Database;
			e2.UpdateBaseCurrencyFields = e.UpdateBaseCurrencyFields;
			DataRow currentAsDataRow = BindingSource.CurrentAsDataRow;
			DataRow[] array = dataTable.Select(GetFilterForParentRowUsingCurrentFieldNames(e.Row));
			for (int i = 0; i < array.Length; i++)
			{
				e2.IsCurrentRow = (e2.Row = array[i]) == currentAsDataRow;
				OnExchangeRateChanged(e2);
			}
		}
	}

	private void ParentTable_CurrencyRateIdForeignChanged(object sender, EventArgs e)
	{
		if (sender is TableDefinition tableDefinition)
		{
			setCurrencyRateIdForeign(tableDefinition.CurrencyRateIdForeign, tableDefinition.CurrencySymbolForeign);
		}
	}

	private void BindingSource_SaveDataStarted_UpdateManuallyAdded(object sender, SaveDataStartedEventArgs e)
	{
		manuallyLoadedParentBindingSource?.SaveData(e);
	}

	private void BindingSource_SaveDataCompleted_UpdateManuallyAdded(object sender, SaveDataCompletedEventArgs e)
	{
		manuallyLoadedParentBindingSource?.OnSaveDataCompleted(e);
	}

	private void BindingSource_RowActivated_UpdateManuallyAdded(object sender, M1BindingSource.QueryDatabaseEventArgs e)
	{
		manuallyLoadedParentBindingSource?.NavigateTo(e, getFilterForParentFieldsUsingCurrentTable(e.TopLevelDataRow, e.TopLevelBindingSource, manuallyLoadedParentBindingSource), string.Empty);
	}

	private void BindingSource_EditCancelled_UpdateManuallyAdded(object sender, EventArgs e)
	{
		manuallyLoadedParentBindingSource?.CancelEdit();
	}

	private void BindingSource_CacheCleared_UpdateManuallyAdded(object sender, EventArgs e)
	{
		manuallyLoadedParentBindingSource?.ClearCache();
	}

	public string GetFilterForParentRowUsingCurrentFieldNames(DataRow parentDataRow)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (parentDataRow != null)
		{
			string empty = string.Empty;
			string empty2 = string.Empty;
			FieldDefinition childLinkField = BindingSource.GetChildLinkField();
			string[] array = ((BindingSource.BoundFieldDefinition == null) ? childLinkField.RelatedTableKeyFieldsArray : BindingSource.BoundFieldDefinition.RelatedFieldsAndCurrentFieldArray);
			for (int i = 0; i < childLinkField.RelatedTableKeyFieldsArray.Length; i++)
			{
				if (i < array.Length)
				{
					empty = array[i];
					empty2 = childLinkField.RelatedFieldsAndCurrentFieldArray[i];
					if (stringBuilder.Length == 0)
					{
						stringBuilder.AppendFormat("{0} = {1}", empty2, parentDataRow[empty].ToLinq());
					}
					else
					{
						stringBuilder.AppendFormat(" And {0} = {1} ", empty2, parentDataRow[empty].ToLinq());
					}
				}
				else
				{
					empty2 = childLinkField.RelatedFieldsAndCurrentFieldArray[i];
					if (stringBuilder.Length == 0)
					{
						stringBuilder.AppendFormat("{0} = ''", empty2);
					}
					else
					{
						stringBuilder.AppendFormat(" And {0}  = ''", empty2);
					}
				}
			}
		}
		return stringBuilder.ToString();
	}

	private string getFilterForParentFieldsUsingCurrentTable(DataRow dataRow, M1BindingSource dataRowBs, M1BindingSource parentBs)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (dataRow != null)
		{
			string empty = string.Empty;
			string empty2 = string.Empty;
			string[] keyFieldsArray = dataRowBs.PrimaryTable.KeyFieldsArray;
			if (parentBs.PrimaryTable.KeyFieldsArray.Length >= 1)
			{
				FieldDefinition fieldDefinition = dataRowBs.Fields[keyFieldsArray[parentBs.PrimaryTable.KeyFieldsArray.Length - 1]];
				for (int i = 0; i < fieldDefinition.RelatedFieldsAndCurrentFieldArray.Length; i++)
				{
					if (i < keyFieldsArray.Length)
					{
						empty = _ParentBindingKeyFieldsArray[i];
						empty2 = keyFieldsArray[i];
						if (stringBuilder.Length == 0)
						{
							stringBuilder.AppendFormat("{0} = {1}", empty, dataRow[empty2].ToSql());
						}
						else
						{
							stringBuilder.AppendFormat(" And {0} = {1}", empty, dataRow[empty2].ToSql());
						}
					}
				}
			}
		}
		else
		{
			stringBuilder.Append("0=1");
		}
		return stringBuilder.ToString();
	}

	public string GetPersistentParentWhereClause(DataRow dataRow)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (dataRow != null)
		{
			string empty = string.Empty;
			string empty2 = string.Empty;
			string[] keyFieldsArray = KeyFieldsArray;
			if (keyFieldsArray.Length >= 2)
			{
				if (parentKeyFieldsArray == null)
				{
					parentKeyFieldsArray = getParentKeyFieldsArray();
				}
				if (parentKeyFieldsArray != null)
				{
					for (int i = 0; i < parentKeyFieldsArray.Length; i++)
					{
						if (i < keyFieldsArray.Length)
						{
							empty = parentKeyFieldsArray[i];
							empty2 = keyFieldsArray[i];
							if (stringBuilder.Length == 0)
							{
								stringBuilder.AppendFormat("{0} = {1}", empty, dataRow[empty2].ToSql());
							}
							else
							{
								stringBuilder.AppendFormat(" And {0} = {1}", empty, dataRow[empty2].ToSql());
							}
						}
					}
				}
			}
		}
		else
		{
			stringBuilder.Append("0=1");
		}
		return stringBuilder.ToString();
	}

	public void CheckBindToParentFields(M1BindingSource mainParentBs)
	{
		M1BindingSource m1BindingSource = ((manuallyLoadedParentBindingSource == null) ? mainParentBs : manuallyLoadedParentBindingSource);
		if (m1BindingSource == null)
		{
			return;
		}
		foreach (FieldDefinition field in BindingSource.Fields)
		{
			if (field.Table == this && field.BoundParentField.Length != 0 && field.BoundParentFieldType == FieldDefinition.BoundParentFieldTypeEnum.FromParent && m1BindingSource.Fields.Contains(field.BoundParentField))
			{
				m1BindingSource.Fields[field.BoundParentField].ValueChanged += field.BoundParentFieldValueChanged;
			}
		}
		if ((DisableAddNewExpression.Length == 0 && DisableAddNewExpressionUser.Length == 0) || (DisableAddNewExpression.IndexOf("RELATEDTABLEGETADORECORD", StringComparison.CurrentCultureIgnoreCase) == -1 && DisableAddNewExpressionUser.IndexOf("RELATEDTABLEGETADORECORD", StringComparison.CurrentCultureIgnoreCase) == -1))
		{
			return;
		}
		foreach (FieldDefinition field2 in m1BindingSource.Fields)
		{
			if (field2.FieldName.Length != 0 && (DisableAddNewExpression.IndexOf(field2.FieldName, StringComparison.CurrentCultureIgnoreCase) != -1 || DisableAddNewExpressionUser.IndexOf(field2.FieldName, StringComparison.CurrentCultureIgnoreCase) != -1))
			{
				field2.ValueChanged += parentFieldDef_DisableAddNewExpression_ValueChanged;
			}
		}
	}

	public string GetAllReferencedFields()
	{
		List<string> list = new List<string>();
		foreach (string disableDeleteExpressionReferencedField in DisableDeleteExpressionReferencedFields)
		{
			if (!list.Contains(disableDeleteExpressionReferencedField))
			{
				list.Add(disableDeleteExpressionReferencedField);
			}
		}
		foreach (string readOnlyExpressionReferencedField in ReadOnlyExpressionReferencedFields)
		{
			if (!list.Contains(readOnlyExpressionReferencedField))
			{
				list.Add(readOnlyExpressionReferencedField);
			}
		}
		foreach (string validCodeReferencedField in ValidCodeReferencedFields)
		{
			if (!list.Contains(validCodeReferencedField))
			{
				list.Add(validCodeReferencedField);
			}
		}
		StringBuilder stringBuilder = new StringBuilder();
		foreach (string item in list)
		{
			if (stringBuilder.Length != 0)
			{
				stringBuilder.Append(",");
			}
			stringBuilder.Append(item);
		}
		return stringBuilder.ToString();
	}

	private void parentFieldDef_DisableAddNewExpression_ValueChanged(object sender, FieldDefinition.FieldValueChangedEventArgs e)
	{
		EvaluateDisableAddNewExpression(e.Database, e.Row, e.SqlTransaction);
	}

	public DataRow GetParentDataRow(DataRow row)
	{
		if (ParentBindingSource != null)
		{
			DataRow currentAsDataRow = ParentBindingSource.CurrentAsDataRow;
			if (currentAsDataRow != null)
			{
				bool flag = false;
				DataRowVersion version = ((row.RowState == DataRowState.Deleted) ? DataRowVersion.Original : ((row.RowState != DataRowState.Detached) ? DataRowVersion.Current : ((!row.HasVersion(DataRowVersion.Proposed)) ? DataRowVersion.Original : DataRowVersion.Proposed)));
				for (int i = 0; i < ParentBindingSource.PrimaryTable.KeyFieldsArray.Length; i++)
				{
					if (!currentAsDataRow[ParentBindingSource.PrimaryTable.KeyFieldsArray[i]].Equals(row[KeyFieldsArray[i], version]))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					return ParentBindingSource.CurrentAsDataRow;
				}
			}
			return ParentBindingSource.GetDataRow(GetParentKeyValuesUsingCurrentTable(row));
		}
		return null;
	}

	public M1AdoLookupRow GetParentRow(DataRow row)
	{
		return new M1AdoLookupRow
		{
			Row = GetParentDataRow(row)
		};
	}

	private string[] getParentKeyFieldsArray()
	{
		if (parentKeyFieldsArray == null && KeyFieldsArray.Length >= 2)
		{
			FieldDefinition fieldDefinition = BindingSource.Fields[KeyFieldsArray[KeyFieldsArray.Length - 2]];
			parentKeyFieldsArray = fieldDefinition.RelatedTableKeyFieldsArray;
			if (fieldDefinition.RelatedTable.Length != 0 && ParentTableName.Length != 0 && !fieldDefinition.RelatedTable.Equals(ParentTableName, StringComparison.CurrentCultureIgnoreCase) && loadedParentKeyFields != null && loadedParentKeyFields.Length != 0)
			{
				parentKeyFieldsArray = loadedParentKeyFields.Split(',');
			}
		}
		return parentKeyFieldsArray;
	}

	public object[] GetParentKeyValuesUsingCurrentTable(DataRow dataRow)
	{
		if (dataRow != null)
		{
			FieldDefinition fieldDefinition = ParentTableLinkField;
			if (fieldDefinition == null && KeyFieldsArray.Length >= 2)
			{
				fieldDefinition = BindingSource.Fields[KeyFieldsArray[KeyFieldsArray.Length - 2]];
			}
			if (fieldDefinition != null)
			{
				object[] array = new object[fieldDefinition.RelatedTableKeyFieldsArray.Length];
				for (int i = 0; i < fieldDefinition.RelatedTableKeyFieldsArray.Length; i++)
				{
					if (i < KeyFieldsArray.Length)
					{
						if (dataRow.RowState == DataRowState.Deleted)
						{
							array[i] = dataRow[KeyFieldsArray[i], DataRowVersion.Original];
						}
						else
						{
							array[i] = dataRow[KeyFieldsArray[i]];
						}
					}
				}
				return array;
			}
		}
		return null;
	}

	public object[] GetParentKeyValuesUsingParentBindingSource()
	{
		if (ParentBindingSource != null)
		{
			DataRow currentAsDataRow = ParentBindingSource.CurrentAsDataRow;
			List<object> list = new List<object>();
			if (currentAsDataRow != null)
			{
				string[] keyFieldsArray = ParentBindingSource.PrimaryTable.KeyFieldsArray;
				foreach (string columnName in keyFieldsArray)
				{
					list.Add(currentAsDataRow[columnName]);
				}
				return list.ToArray();
			}
		}
		return null;
	}

	public void SaveDataRowCheckReferenceLinks(M1Database database, DataRow row, SqlTransaction transaction)
	{
		if (row.RowState == DataRowState.Added)
		{
			return;
		}
		StringBuilder stringBuilder = new StringBuilder();
		if (ChildReferenceTableLinks.Count != 0)
		{
			foreach (ChildReferenceTableLink childReferenceTableLink in ChildReferenceTableLinks)
			{
				if (childReferenceTableLink.BindingType == FieldDefinition.BoundParentFieldTypeEnum.FromParent && !row[childReferenceTableLink.ParentField, DataRowVersion.Original].Equals(row[childReferenceTableLink.ParentField]) && childReferenceTableLink.ChildTableDefinition == null)
				{
					if (!childReferenceTableLink.CodeExists)
					{
						processSaveNextChildLinkLevel(isClosedFieldLink: (childReferenceTableLink.ParentField.Equals(ClosedField, StringComparison.CurrentCultureIgnoreCase) && !(row[ClosedField] is string) && Convert.ToBoolean(row[ClosedField])) ? true : false, row: row, curLink: childReferenceTableLink, parentField: childReferenceTableLink.ParentField, parentTable: this, updateStatement: stringBuilder);
						continue;
					}
					MessageBox.Show("The link from " + childReferenceTableLink.ParentTable + "." + childReferenceTableLink.ParentField + " to child table " + childReferenceTableLink.ChildTable + "." + childReferenceTableLink.ChildField + " requires a binding source so the child field change event code can be run");
				}
			}
		}
		if (CurrencyExchangeRateField.Length != 0 && ChildCurrencyLinks.Count != 0 && BindingSource.Fields[CurrencyExchangeRateField].HasValueChanged(row))
		{
			bool flag = ShouldCurrencyRefreshUpdateBase(database, row, transaction);
			decimal exchangeRateForRow = GetExchangeRateForRow(database, row, transaction);
			foreach (ChildCurrencyLink childCurrencyLink in ChildCurrencyLinks)
			{
				if (childCurrencyLink.ChildTableDefinition != null || childCurrencyLink.ChildRelatedCurrencyField.Length == 0)
				{
					continue;
				}
				if (flag && childCurrencyLink.ChildCurrencyType == M1CurrencyStyle.Foreign)
				{
					stringBuilder.Append("Update " + childCurrencyLink.ChildTable + " Set " + childCurrencyLink.ChildRelatedCurrencyField + " = Round(" + childCurrencyLink.ChildField + " / " + exchangeRateForRow.ToSql() + "," + childCurrencyLink.ChildFieldDecimals.ToSql() + ") Where ");
					for (int i = 0; i < KeyFieldsArray.Length; i++)
					{
						stringBuilder.Append(((i == 0) ? string.Empty : " And ") + childCurrencyLink.ChildKeyFieldsArray[i] + " = " + row[KeyFieldsArray[i]].ToSql());
					}
					stringBuilder.Append("\r\n");
				}
				else if (!flag && childCurrencyLink.ChildCurrencyType == M1CurrencyStyle.Base)
				{
					stringBuilder.Append("Update " + childCurrencyLink.ChildTable + " Set " + childCurrencyLink.ChildRelatedCurrencyField + " = Round(" + childCurrencyLink.ChildField + " * " + exchangeRateForRow.ToSql() + "," + childCurrencyLink.ChildFieldDecimals.ToSql() + ") Where ");
					for (int j = 0; j < KeyFieldsArray.Length; j++)
					{
						stringBuilder.Append(((j == 0) ? string.Empty : " And ") + childCurrencyLink.ChildKeyFieldsArray[j] + " = " + row[KeyFieldsArray[j]].ToSql());
					}
					stringBuilder.Append("\r\n");
				}
			}
		}
		if (stringBuilder.Length != 0)
		{
			database.ExecuteCommand(stringBuilder.ToString(), transaction);
		}
	}

	public string GenerateUpdateForClosedFields(DateTime? cutoffDate, bool excludeExtraFilter)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (cutoffDate.HasValue)
		{
			string closedNewValue = getClosedNewValue();
			string text = GenerateWhereForClosedFields(cutoffDate, excludeExtraFilter);
			foreach (ChildReferenceTableLink childReferenceTableLink in ChildReferenceTableLinks)
			{
				if (childReferenceTableLink.ParentField.Equals(ClosedField, StringComparison.CurrentCultureIgnoreCase))
				{
					processNextClosedLevel(childReferenceTableLink, stringBuilder, text, closedNewValue);
				}
			}
			stringBuilder.Append("Update " + TableName + " Set " + ClosedField + " = " + closedNewValue);
			if (ClosedDateField.Length != 0)
			{
				stringBuilder.Append(", " + ClosedDateField + " = " + DateTime.Today.ToSql(dateOnly: true));
			}
			if (!string.IsNullOrEmpty(ClosedExtraSetExpression))
			{
				stringBuilder.Append(", " + ClosedExtraSetExpression);
			}
			stringBuilder.Append(" Where " + text + "\r\n");
		}
		return BindingSource.Database.PrepareQuery(stringBuilder.ToString());
	}

	private string getClosedNewValue()
	{
		string result = "1";
		if (ClosedValue.Length != 0)
		{
			result = ClosedValue;
		}
		return result;
	}

	public string GenerateBaseWhereForClosed(string oper)
	{
		return ClosedField + oper + getClosedNewValue();
	}

	public string GenerateWhereForClosedFields(DateTime? cutoffDate, bool excludeExtraFilter)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(GenerateBaseWhereForClosed(" <> "));
		if (cutoffDate.HasValue)
		{
			stringBuilder.Append(" And " + ClosedCutoffDateField + " < " + cutoffDate.Value.ToSql(dateOnly: true));
		}
		if (!string.IsNullOrEmpty(ClosedIncludeOptionSqlExpression) && !excludeExtraFilter)
		{
			stringBuilder.Append(" And " + ClosedIncludeOptionSqlExpression);
		}
		return stringBuilder.ToString();
	}

	private void processNextClosedLevel(ChildReferenceTableLink curLink, StringBuilder updateStatement, string filter, string newValue)
	{
		if (curLink.BindingType == FieldDefinition.BoundParentFieldTypeEnum.FromParent)
		{
			updateStatement.Append("Update " + curLink.ChildTable + " Set " + curLink.ChildField + " = " + newValue);
			if (!string.IsNullOrEmpty(curLink.ChildClosedSetExpression))
			{
				updateStatement.Append(", " + curLink.ChildClosedSetExpression);
			}
			updateStatement.Append(" From " + curLink.ChildTable + " Inner Join " + TableName + " On ");
			for (int i = 0; i < KeyFieldsArray.Length; i++)
			{
				updateStatement.Append(((i == 0) ? string.Empty : " And ") + curLink.ChildKeyFieldsArray[i] + " = " + KeyFieldsArray[i]);
			}
			updateStatement.Append(" Where " + filter + "\r\n");
		}
	}

	private void processSaveNextChildLinkLevel(DataRow row, ChildReferenceTableLink curLink, string parentField, TableDefinition parentTable, StringBuilder updateStatement, bool isClosedFieldLink)
	{
		if (curLink.BindingType == FieldDefinition.BoundParentFieldTypeEnum.FromParent)
		{
			updateStatement.Append("Update " + curLink.ChildTable + " Set " + curLink.ChildField + " = " + row[parentField].ToSql());
			if (isClosedFieldLink && !string.IsNullOrEmpty(curLink.ChildClosedSetExpression))
			{
				updateStatement.Append(", " + curLink.ChildClosedSetExpression);
			}
			updateStatement.Append(" Where ");
			for (int i = 0; i < parentTable.KeyFieldsArray.Length; i++)
			{
				updateStatement.Append(((i == 0) ? string.Empty : " And ") + curLink.ChildKeyFieldsArray[i] + " = " + row[parentTable.KeyFieldsArray[i]].ToSql());
			}
			updateStatement.Append("\r\n");
		}
		foreach (ChildReferenceTableLink childReferenceTableLink in curLink.ChildReferenceTableLinks)
		{
			if (childReferenceTableLink.BindingType == FieldDefinition.BoundParentFieldTypeEnum.FromParent)
			{
				processSaveNextChildLinkLevel(row, childReferenceTableLink, parentField, parentTable, updateStatement, isClosedFieldLink);
			}
		}
	}

	private M1BindingSource getTableInChildBindingSources(string table, List<M1BindingSource> childBindingSources)
	{
		foreach (M1BindingSource childBindingSource in childBindingSources)
		{
			if (childBindingSource.PrimaryTable != null && childBindingSource.PrimaryTable.TableName.Equals(table, StringComparison.CurrentCultureIgnoreCase))
			{
				return childBindingSource;
			}
		}
		return null;
	}

	public bool ChildBindingSourceExists(string table)
	{
		return getTableInChildBindingSources(table, BindingSource.ChildBindingSources) != null;
	}

	public M1BindingSource GetParentOrSiblingBindingSource(string table)
	{
		if (ParentBindingSource != null)
		{
			if (ParentBindingSource.PrimaryTable.TableName.Equals(table, StringComparison.CurrentCultureIgnoreCase))
			{
				return ParentBindingSource;
			}
			return ParentBindingSource.PrimaryTable.GetChildBindingSource(table);
		}
		return null;
	}

	public M1BindingSource GetChildBindingSource(string table)
	{
		return GetChildBindingSource(table, string.Empty);
	}

	public M1BindingSource GetChildBindingSource(string table, string additionalWhere)
	{
		M1BindingSource tableInChildBindingSources = getTableInChildBindingSources(table, BindingSource.ChildBindingSources);
		if (tableInChildBindingSources != null)
		{
			if (!string.IsNullOrWhiteSpace(tableInChildBindingSources.Query.GridID) && !tableInChildBindingSources.Query.AllowEditingOfGrid)
			{
				throw new M1MissingOrInvalidDataException($"Grid {tableInChildBindingSources.Query.GridID} is not an editable grid. Grids that are called from GetChildBindingSource must have this option set.");
			}
			return tableInChildBindingSources;
		}
		M1BindingSource m1BindingSource = new M1BindingSource(BindingSource.Database);
		((ISupportInitialize)m1BindingSource).BeginInit();
		m1BindingSource.DataSourceTable = table;
		m1BindingSource.DataBindings.Add(new Binding("ParentFieldValue", BindingSource, LastKeyField, formattingEnabled: true, DataSourceUpdateMode.OnPropertyChanged));
		m1BindingSource.AdditionalWhere = additionalWhere;
		((ISupportInitialize)m1BindingSource).EndInit();
		return m1BindingSource;
	}

	private void checkBsForCountInValid()
	{
		if (ParentBindingSource != null && ParentBindingSource.PrimaryTable.ValidCodeReferencedBsTables != null && ParentBindingSource.PrimaryTable.ValidCodeReferencedBsTables.Contains(TableName, StringComparer.CurrentCultureIgnoreCase))
		{
			BindingSource.AddNewCompleted += childBs_AddNewCompleted_RefreshValid;
			BindingSource.RemoveCompleted += childBs_RemoveCompleted_RefreshValid;
		}
	}

	public int GetChildRowCountForParent()
	{
		if (ParentBindingSource != null && manuallyLoadedParentBindingSource == null)
		{
			return BindingSource.Count;
		}
		DataRow currentAsDataRow = BindingSource.CurrentAsDataRow;
		if (currentAsDataRow != null)
		{
			FieldDefinition parentLinkField = getParentLinkField(null);
			if (parentLinkField != null)
			{
				SqlCommand sqlCommand = BindingSource.CurrentDatabase.NewSqlCommand(string.Empty);
				StringBuilder stringBuilder = new StringBuilder();
				StringBuilder stringBuilder2 = new StringBuilder();
				string[] relatedFieldsAndCurrentFieldArray = parentLinkField.RelatedFieldsAndCurrentFieldArray;
				foreach (string text in relatedFieldsAndCurrentFieldArray)
				{
					if (stringBuilder2.Length != 0)
					{
						stringBuilder2.Append(" And ");
						stringBuilder.Append(" And ");
					}
					stringBuilder2.Append(text + "=@" + text);
					stringBuilder.Append(text + "=" + currentAsDataRow[text].ToLinq());
					sqlCommand.Parameters.Add(new SqlParameter("@" + text, BindingSource.Fields[text].GetSqlDbType())).Value = currentAsDataRow[text];
				}
				DataTable dataTable = BindingSource.GetDataTable();
				int num = dataTable.Select(stringBuilder.ToString(), string.Empty, DataViewRowState.Added).Length;
				int num2 = dataTable.Select(stringBuilder.ToString(), string.Empty, DataViewRowState.Deleted).Length;
				sqlCommand.CommandText = "Select Count(*) From " + TableName + " Where " + stringBuilder2.ToString();
				return Convert.ToInt32(BindingSource.CurrentDatabase.ExecuteScalar(sqlCommand)) + num - num2;
			}
		}
		return 0;
	}

	private void childBs_RemoveCompleted_RefreshValid(object sender, RemoveEventArgs e)
	{
		ParentBindingSource.PrimaryTable.Validate(ParentBindingSource.CurrentDatabase, ParentBindingSource.CurrentAsDataRow, ParentBindingSource.Transaction, ParentBindingSource.IsTopLevel, isCurrentRow: true);
	}

	private void childBs_AddNewCompleted_RefreshValid(object sender, DbAndRowEventArgs e)
	{
		ParentBindingSource.PrimaryTable.Validate(ParentBindingSource.CurrentDatabase, ParentBindingSource.CurrentAsDataRow, ParentBindingSource.Transaction, ParentBindingSource.IsTopLevel, isCurrentRow: true);
	}

	public DateTime? GetDocumentDate(M1Database database, DataRow row, SqlTransaction transaction)
	{
		return GetDocumentDate(database, row, transaction, DataRowVersion.Current);
	}

	public DateTime? GetDocumentDate(M1Database database, DataRow row, SqlTransaction transaction, DataRowVersion rowVersion)
	{
		if (string.IsNullOrWhiteSpace(TopLevelDateField))
		{
			return null;
		}
		if (BindingSource.Fields.Contains(TopLevelDateField))
		{
			return row.Field<DateTime?>(TopLevelDateField, rowVersion);
		}
		return BindingSource.Fields[KeyFieldsArray[0]].RelatedTableGetDataRow(TopLevelDateField, database, row, alwaysReturnValidRow: true, transaction).Field<DateTime?>(TopLevelDateField, rowVersion);
	}

	public string GetDocumentPlantID(M1Database database, DataRow row, SqlTransaction transaction)
	{
		return GetDocumentPlantID(database, row, transaction, DataRowVersion.Current);
	}

	public string GetDocumentPlantID(M1Database database, DataRow row, SqlTransaction transaction, DataRowVersion rowVersion)
	{
		if (!string.IsNullOrWhiteSpace(DocumentPlantIdField))
		{
			if (BindingSource.Fields.Contains(DocumentPlantIdField))
			{
				return row.Field<string>(DocumentPlantIdField, rowVersion);
			}
			return BindingSource.Fields[KeyFieldsArray[0]].RelatedTableGetDataRow(DocumentPlantIdField, database, row, alwaysReturnValidRow: true, transaction).Field<string>(DocumentPlantIdField, rowVersion);
		}
		if (string.IsNullOrWhiteSpace(TopLevelPlantIdField))
		{
			M1BindingSource parentBindingSource = BindingSource.PrimaryTable.GetParentBindingSource(row);
			if (parentBindingSource != null && parentBindingSource.Fields.Contains(parentBindingSource.PrimaryTable.FieldPrefix + "PlantID") && parentBindingSource.CurrentAsDataRow != null)
			{
				return parentBindingSource.CurrentAsDataRow.Field<string>(parentBindingSource.PrimaryTable.FieldPrefix + "PlantID", rowVersion);
			}
			return string.Empty;
		}
		if (BindingSource.Fields.Contains(TopLevelPlantIdField))
		{
			return row.Field<string>(TopLevelPlantIdField, rowVersion);
		}
		return BindingSource.Fields[KeyFieldsArray[0]].RelatedTableGetDataRow(TopLevelPlantIdField, database, row, alwaysReturnValidRow: true, transaction).Field<string>(TopLevelPlantIdField, rowVersion);
	}

	public M1BindingSource GetParentBindingSource(DataRow row)
	{
		if (ParentBindingSource == null)
		{
			if (BindingSource.BoundFieldDefinition?.BindingSource != null)
			{
				VerifyParentBindingSource(BindingSource.BoundFieldDefinition.BindingSource, forceParentLoad: true);
			}
			else
			{
				VerifyParentBindingSource(null, forceParentLoad: true);
			}
		}
		DataRow parentDataRow = GetParentDataRow(row);
		if (parentDataRow != null && ParentBindingSource.CurrentAsDataRow != parentDataRow)
		{
			ParentBindingSource.SetPositionByDataRow(parentDataRow);
		}
		return ParentBindingSource;
	}

	public void VerifyChildBindingSources(List<M1BindingSource> childBindingSources)
	{
		bool flag = false;
		foreach (ChildReferenceTableLink childReferenceTableLink in ChildReferenceTableLinks)
		{
			M1BindingSource tableInChildBindingSources = getTableInChildBindingSources(childReferenceTableLink.ChildTable, childBindingSources);
			if (tableInChildBindingSources != null)
			{
				childReferenceTableLink.ChildBindingSource = tableInChildBindingSources;
				childReferenceTableLink.ChildTableDefinition = tableInChildBindingSources.PrimaryTable;
				childReferenceTableLink.ChildBindingSource.BindingSourceLinks.Add(M1BindingSource.BindingSourceLinkTypeEnum.BoundParentField);
			}
			else if (BindingSource.PrimaryBindingSource != null && BindingSource.PrimaryBindingSource.PrimaryTable.TableName.Equals(childReferenceTableLink.ChildTable, StringComparison.CurrentCultureIgnoreCase))
			{
				childReferenceTableLink.ChildBindingSource = BindingSource.PrimaryBindingSource;
				childReferenceTableLink.ChildTableDefinition = BindingSource.PrimaryBindingSource.PrimaryTable;
				childReferenceTableLink.ChildBindingSource.BindingSourceLinks.Add(M1BindingSource.BindingSourceLinkTypeEnum.BoundParentField);
			}
			if (childReferenceTableLink.ChildBindingSource == null && ((childReferenceTableLink.BindingType == FieldDefinition.BoundParentFieldTypeEnum.FromParent && childReferenceTableLink.CodeExists) || processNextChildReferenceLink(childReferenceTableLink)))
			{
				flag = true;
				childReferenceTableLink.ChildBindingSource = new M1BindingSource(BindingSource.Database);
				childReferenceTableLink.ChildBindingSource.BindingSourceLinks.Clear();
				((ISupportInitialize)childReferenceTableLink.ChildBindingSource).BeginInit();
				childReferenceTableLink.ChildBindingSource.DataBindings.Add(new Binding("ParentFieldValue", BindingSource, LastKeyField, formattingEnabled: true, DataSourceUpdateMode.OnPropertyChanged));
				childReferenceTableLink.ChildBindingSource.DataSourceTable = childReferenceTableLink.ChildTable;
				((ISupportInitialize)childReferenceTableLink.ChildBindingSource).EndInit();
				childReferenceTableLink.ChildTableDefinition = childReferenceTableLink.ChildBindingSource.PrimaryTable;
			}
		}
		if (!flag)
		{
			return;
		}
		foreach (ChildReferenceTableLink childReferenceTableLink2 in ChildReferenceTableLinks)
		{
			if (childReferenceTableLink2.ChildTableDefinition == null)
			{
				M1BindingSource tableInChildBindingSources = getTableInChildBindingSources(childReferenceTableLink2.ChildTable, childBindingSources);
				if (tableInChildBindingSources != null)
				{
					childReferenceTableLink2.ChildBindingSource = tableInChildBindingSources;
					childReferenceTableLink2.ChildTableDefinition = tableInChildBindingSources.PrimaryTable;
					childReferenceTableLink2.ChildBindingSource.BindingSourceLinks.Add(M1BindingSource.BindingSourceLinkTypeEnum.BoundParentField);
				}
			}
		}
	}

	public void VerifyChildBindingSourcesForCurrencyLinks(List<ChildCurrencyLink> childCurrencyLinks)
	{
		if (CurrencyChecked)
		{
			return;
		}
		CurrencyChecked = true;
		if (childCurrencyLinks.Count == 0)
		{
			return;
		}
		bool flag = false;
		List<string> list = new List<string>();
		foreach (ChildCurrencyLink childCurrencyLink in childCurrencyLinks)
		{
			if (!childCurrencyLink.ParentTable.Equals(TableName, StringComparison.CurrentCultureIgnoreCase))
			{
				continue;
			}
			if (!list.Contains(childCurrencyLink.ChildTable))
			{
				list.Add(childCurrencyLink.ChildTable);
			}
			M1BindingSource tableInChildBindingSources = getTableInChildBindingSources(childCurrencyLink.ChildTable, BindingSource.ChildBindingSources);
			if (tableInChildBindingSources != null)
			{
				childCurrencyLink.ChildBindingSource = tableInChildBindingSources;
				childCurrencyLink.ChildTableDefinition = tableInChildBindingSources.PrimaryTable;
				if (!childCurrencyLink.ChildBindingSource.BindingSourceLinks.Contains(M1BindingSource.BindingSourceLinkTypeEnum.CurrencyLink))
				{
					childCurrencyLink.ChildBindingSource.BindingSourceLinks.Add(M1BindingSource.BindingSourceLinkTypeEnum.CurrencyLink);
				}
				childCurrencyLink.ChildBindingSource.CheckQueryDatabaseForCurrencyLink();
			}
			if (childCurrencyLink.ChildBindingSource == null && childCurrencyLink.CodeExists)
			{
				flag = true;
				childCurrencyLink.ChildBindingSource = new M1BindingSource(BindingSource.Database);
				childCurrencyLink.ChildBindingSource.BindingSourceLinks.Clear();
				((ISupportInitialize)childCurrencyLink.ChildBindingSource).BeginInit();
				childCurrencyLink.ChildBindingSource.DataBindings.Add(new Binding("ParentFieldValue", BindingSource, LastKeyField, formattingEnabled: true, DataSourceUpdateMode.OnPropertyChanged));
				childCurrencyLink.ChildBindingSource.DataSourceTable = childCurrencyLink.ChildTable;
				((ISupportInitialize)childCurrencyLink.ChildBindingSource).EndInit();
				childCurrencyLink.ChildTableDefinition = childCurrencyLink.ChildBindingSource.PrimaryTable;
			}
		}
		if (flag)
		{
			foreach (ChildCurrencyLink childCurrencyLink2 in childCurrencyLinks)
			{
				if (childCurrencyLink2.ChildTableDefinition != null || !childCurrencyLink2.ParentTable.Equals(TableName, StringComparison.CurrentCultureIgnoreCase))
				{
					continue;
				}
				M1BindingSource tableInChildBindingSources = getTableInChildBindingSources(childCurrencyLink2.ChildTable, BindingSource.ChildBindingSources);
				if (tableInChildBindingSources != null)
				{
					childCurrencyLink2.ChildBindingSource = tableInChildBindingSources;
					childCurrencyLink2.ChildTableDefinition = tableInChildBindingSources.PrimaryTable;
					if (!childCurrencyLink2.ChildBindingSource.BindingSourceLinks.Contains(M1BindingSource.BindingSourceLinkTypeEnum.CurrencyLink))
					{
						childCurrencyLink2.ChildBindingSource.BindingSourceLinks.Add(M1BindingSource.BindingSourceLinkTypeEnum.CurrencyLink);
					}
					childCurrencyLink2.ChildBindingSource.CheckQueryDatabaseForCurrencyLink();
				}
			}
		}
		foreach (string item in list)
		{
			getTableInChildBindingSources(item, BindingSource.ChildBindingSources)?.PrimaryTable.VerifyChildBindingSourcesForCurrencyLinks(childCurrencyLinks);
		}
	}

	public void VerifyChildBindingSourcesForDelete()
	{
		if (ChildDeleteReferenceTableLinks != null)
		{
			return;
		}
		ChildDeleteReferenceTableLinks = new List<string>();
		SqlCommand sqlCommand = BindingSource.DataDictionary.NewSqlCommand("Select drCTable,drFilter From DDRelations With(Nolock) Inner Join DDTables With(NoLock) On drCTable = dtTable Where drPTable = @ParentTable And drPersist <> 0 And dtHasDeleteCode <> 0");
		sqlCommand.Parameters.Add(new SqlParameter("@ParentTable", SqlDbType.NVarChar)).Value = TableName;
		foreach (DataRow row in BindingSource.DataDictionary.GetDataTable(sqlCommand).Rows)
		{
			string text = row.Field<string>("drCTable");
			if (!ChildDeleteReferenceTableLinks.Contains(text, StringComparer.CurrentCultureIgnoreCase) && !text.Equals("JobAssemblies", StringComparison.CurrentCultureIgnoreCase) && !text.Equals("JobOperations", StringComparison.CurrentCultureIgnoreCase) && !text.Equals("QuoteAssemblies", StringComparison.CurrentCultureIgnoreCase) && !text.Equals("PartAssemblies", StringComparison.CurrentCultureIgnoreCase) && !TableName.Equals("QuoteAssemblies", StringComparison.CurrentCultureIgnoreCase))
			{
				GetChildBindingSource(text, row.Field<string>("drFilter"));
				ChildDeleteReferenceTableLinks.Add(text);
			}
		}
	}

	private bool processNextChildReferenceLink(ChildReferenceTableLink curLink)
	{
		SqlCommand sqlCommand = BindingSource.DataDictionary.NewSqlCommand("select parentFields.dfTable as ParentTable,parentFields.dfField As ParentField,childFields.dfTable as ChildTable,childFields.dfField as ChildField,childTable.dtKeyFields as ChildKeyFields,childTable.dtClosedExtraSetExpression as ChildClosedSetExpression,childFields.dfBoundParentFieldType as BindingType,childFields.dfHasChangeCode As CodeExists from DDFields parentFields Inner Join DDFields childFields on parentFields.dfField = childFields.dfBoundParentField And (childFields.dfBoundParentFieldType = 1 Or childFields.dfBoundParentFieldType = 2) Inner Join DDTables childTable With(NoLock) on childFields.dfTable = childTable.dtTable Where parentFields.dfField = @fieldname");
		sqlCommand.Parameters.Add(new SqlParameter("@fieldname", SqlDbType.NVarChar, curLink.ChildField.Length)).Value = curLink.ChildField;
		DataTable dataTable = BindingSource.DataDictionary.GetDataTable(sqlCommand);
		if (dataTable.Rows.Count != 0)
		{
			DataRow[] array = dataTable.Select("ParentTable = " + curLink.ChildTable.ToLinq() + " And ParentField = " + curLink.ChildField.ToLinq());
			for (int i = 0; i < array.Length; i++)
			{
				ChildReferenceTableLink childReferenceTableLink = new ChildReferenceTableLink(array[i]);
				if ((childReferenceTableLink.BindingType == FieldDefinition.BoundParentFieldTypeEnum.FromParent && childReferenceTableLink.CodeExists) || processNextChildReferenceLink(childReferenceTableLink))
				{
					return true;
				}
				curLink.ChildReferenceTableLinks.Add(childReferenceTableLink);
			}
		}
		return false;
	}

	public void OnAddNewCompleted(AddNewCompletedEventArgs e)
	{
		this.AddNewCompleted?.Invoke(this, e);
	}

	public void OnSetDefaultValues(DbAndRowEventArgs e)
	{
		if (allowEditingOverride)
		{
			this.SetDefaultValues?.Invoke(this, e);
		}
	}

	public void RunParentFieldValidation(string parentFieldList)
	{
		M1BindingSource parentBindingSource = ParentBindingSource;
		if (parentBindingSource == null || parentBindingSource.Errors == null)
		{
			return;
		}
		parentFieldList = parentFieldList.Replace(" ", "");
		string[] array = parentFieldList.Split(new string[1] { "," }, StringSplitOptions.RemoveEmptyEntries);
		foreach (FieldDefinition field in parentBindingSource.Fields)
		{
			bool flag = true;
			string text = field.Name.Trim();
			if ((array.Length != 0 || text.Length == 0) && !array.Contains(text, StringComparer.CurrentCultureIgnoreCase))
			{
				flag = false;
			}
			if (flag)
			{
				field.Validate(parentBindingSource.Database, parentBindingSource.CurrentAsDataRow, parentBindingSource.Transaction, isCurrentRow: true);
			}
		}
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
		if (scriptEngine != null)
		{
			scriptEngine.Dispose();
			scriptEngine = null;
		}
		OnDisposed();
		if (Databases != null)
		{
			foreach (M1DatabaseTableSecurity database in Databases)
			{
				database.Dispose();
			}
			Databases.Clear();
			Databases = null;
		}
		if (ChildDeleteReferenceTableLinks != null)
		{
			ChildDeleteReferenceTableLinks.Clear();
			ChildDeleteReferenceTableLinks = null;
		}
		if (ChildReferenceTableLinks != null)
		{
			foreach (ChildReferenceTableLink childReferenceTableLink in ChildReferenceTableLinks)
			{
				childReferenceTableLink.Dispose();
			}
			ChildReferenceTableLinks.Clear();
			ChildReferenceTableLinks = null;
		}
		if (ChildCurrencyLinks != null)
		{
			foreach (ChildCurrencyLink childCurrencyLink in ChildCurrencyLinks)
			{
				childCurrencyLink.Dispose();
			}
			ChildCurrencyLinks.Clear();
			ChildCurrencyLinks = null;
		}
		if (ValidCodeReferencedFields != null)
		{
			ValidCodeReferencedFields.Clear();
			ValidCodeReferencedFields = null;
		}
		if (ReadOnlyExpressionReferencedFields != null)
		{
			ReadOnlyExpressionReferencedFields.Clear();
			ReadOnlyExpressionReferencedFields = null;
		}
		if (DisableDeleteExpressionReferencedFields != null)
		{
			DisableDeleteExpressionReferencedFields.Clear();
			DisableDeleteExpressionReferencedFields = null;
		}
		errorList = null;
		BindingSource = null;
		_BindingSource = null;
		manuallyLoadedParentBindingSource = null;
		this.OverrideDeleteEnabledChanged = null;
		this.CurrentChanged = null;
		this.Valid = null;
		this.RemoveStarted = null;
		this.RemoveCompleted = null;
		this.GetNextID = null;
		this.UpdateStarted = null;
		this.UpdateCompleted = null;
		this.DeleteStarted = null;
		this.DeleteCompleted = null;
		this.SaveDataCompleted = null;
		this.ParentBindingSourceChanged = null;
		this.AddNewCompleted = null;
		this.SetDefaultValues = null;
		this.DisableAddNewChanged = null;
		this.DisableDeleteChanged = null;
		this.NoAccessChanged = null;
		this.ReadOnlyChanged = null;
		this.ExchangeRateChanged = null;
		this.CurrencyRateIdForeignChanged = null;
		this.KeyChange = null;
	}

	[Browsable(false)]
	public object[] ParentKeyValues()
	{
		return GetParentKeyValuesUsingCurrentTable(GetCurrentDataRowForProcessing());
	}

	[Browsable(false)]
	public int ParentKeyCount()
	{
		if (ParentTableLinkField != null)
		{
			return _ParentBindingKeyFieldsArray.Length;
		}
		return 0;
	}

	[Browsable(false)]
	public string ParentTable()
	{
		if (ParentTableLinkField != null)
		{
			return _ParentBindingTableName;
		}
		return string.Empty;
	}

	public void ProcessCodeBindings(string eventName, StringBuilder code)
	{
		if (!eventName.Equals("Valid", StringComparison.CurrentCultureIgnoreCase))
		{
			return;
		}
		string text = code.ToString();
		ValidCodeReferencedFields.ParseCodeForFields(text);
		if (text.IndexOf("Record.Table.GetChildRowCountForParent(", StringComparison.CurrentCultureIgnoreCase) != -1)
		{
			getChildRowCountReferenced = true;
		}
		if (text.IndexOf("Record.Table.GetChildBindingSource(\"", StringComparison.CurrentCultureIgnoreCase) == -1 || (text.IndexOf(".Count", StringComparison.CurrentCultureIgnoreCase) == -1 && text.IndexOf(".RecordCount", StringComparison.CurrentCultureIgnoreCase) == -1))
		{
			return;
		}
		if (ValidCodeReferencedBsTables == null)
		{
			ValidCodeReferencedBsTables = new List<string>();
		}
		int num = text.IndexOf("Record.Table.GetChildBindingSource(\"");
		while (num != -1)
		{
			text = text.Substring(num + 36);
			num = text.IndexOf('"');
			if (num != -1)
			{
				string text2 = text.Substring(0, num).Trim();
				if (text2.Length != 0 && !ValidCodeReferencedBsTables.Contains(text2, StringComparer.CurrentCultureIgnoreCase))
				{
					ValidCodeReferencedBsTables.Add(text2);
				}
				text = text.Substring(num + 1);
				num = text.IndexOf("Record.Table.GetChildBindingSource(\"");
			}
		}
	}
}
