using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using M1.Core.Utility;
using M1.Script.Interfaces;

namespace M1.Core;

[ComVisible(true)]
public class M1UserSettings : IUserSettings, IServiceProvider, IDisposable
{
	private IServiceProvider _Provider;

	public Font LabelFont = new Font("Tahoma", 8f);

	public Font InputFont = new Font("Tahoma", 8f);

	public Font MemoFont = new Font("Tahoma", 10f);

	public Font RequiredFont = new Font("Tahoma", 8f);

	public Font CodeFont = new Font("Courier New", 10f);

	private Color _RequiredForeColor = SystemColors.WindowText;

	private Color _LabelForeColor = SystemColors.WindowText;

	private Color _InputForeColor = SystemColors.WindowText;

	public Color CodeForeColor = SystemColors.WindowText;

	private bool _LoadGridOnOpen;

	private bool _SpeedFormOpen;

	private int _DefaultLeaveScale;

	private bool _DefaultWindowSizeToM1Explorer;

	private string _EmailClientSingleEmail = string.Empty;

	private string _EmailClientMultipleEmail = string.Empty;

	private string _EMailFormat = string.Empty;

	private bool _EMailReview;

	private string _FaxFormat = string.Empty;

	private string _FaxNumberFormat = string.Empty;

	private bool _FaxReview;

	private bool _FaxIncludeFilterText;

	private string _FaxServer = string.Empty;

	private string _FaxCoverPage = string.Empty;

	private string _FaxAddress = string.Empty;

	private string _ReportExportFormat = string.Empty;

	private bool _AutoOpenDropDown;

	private bool _AutoCloseOnPrint;

	private bool _AutoCheckSpelling;

	public bool DeferGridScroll;

	private bool _SendOnBehalfOf;

	private short _IconSize;

	private string _StartPageOptions = string.Empty;

	private int _StartPageRefreshInterval;

	public Guid? StartUpFolder;

	public Guid? StartUpGroup;

	private string _FolderDisplayOption = string.Empty;

	private bool _ShowExplorerTree;

	public bool ShowExplorerStatusBar;

	private bool _ShowEntryShortcutBar;

	private bool _ShowEntryTree;

	public bool ShowEntryStatusBar;

	private string _EntryQuickAccessToolbarItems = string.Empty;

	private string _EntryQuickAccessToolbarLocation = string.Empty;

	private bool _EntryMinimizeRibbon = true;

	private int _InactiveBinsOptionSelectedAtPartRevisions;

	private int _InactiveBinsOptionSelectedAtWarehouses;

	private string _RunQueryQuickAccessToolbarItems = string.Empty;

	private string _RunQueryQuickAccessToolbarLocation = string.Empty;

	private bool _RunQueryMinimizeRibbon = true;

	private bool _ShowUserIDInTitleBar;

	private bool _OpenWithNewWindow;

	private int _ExplorerShortcutBarWidth;

	private int _ExplorerTreeWidth;

	private int _EntryShortcutBarWidth;

	private int _EntryTreeWidth;

	private bool _FixedEntryTreeWidth;

	private bool _EnterRunsDefaultButton;

	private bool _PromptToExitM1Explorer;

	private bool _ShowCustomReportsInExplorer;

	private bool _HideGroupButtons;

	private bool _SuppressKpiMessages;

	private bool _RemoveDisabledItemsInExplorer;

	private short _MaxLevelsInTree;

	private int _ReportPaperSize;

	private short _TreePrintOption;

	private int _EntryValidationBoxHeight;

	private bool _ShowValidationBox;

	private bool _PopupMemos;

	public bool TreeSearchBold;

	public Color TreeSearchColor = Color.Black;

	private short _EmailWebLinkOptions;

	private string _PhoneDeviceName = string.Empty;

	private bool _CheckIncomingCalls;

	private MailProviders _MailProvider = MailProviders.Outlook;

	private string _ProviderEmailAddress = string.Empty;

	private string _ProviderEmailPasswordEncrypted = string.Empty;

	private string _Signature = string.Empty;

	private bool _StartRemotingServer = true;

	private string _MapEngine = string.Empty;

	private int _LabelForeColorOle;

	private int _InputForeColorOle;

	private int _RequiredForeColorOle;

	private string _Theme = string.Empty;

	private int _ThemeLightColorOle;

	private int _ThemeMidColorOle;

	private int _ThemeDarkColorOle;

	private string _MyFolders = string.Empty;

	public string KPISizes = string.Empty;

	private bool _CacheViews;

	private bool _ShowExplorerShortcutBar;

	private bool _SpellCheckIgnoreInMixedCase;

	private bool _SpellCheckIgnoreInUpperCase;

	private bool _SpellCheckIgnoreWithNumbers;

	public bool ExpandReviewGroup;

	public bool ExpandActionGroup;

	public string BarGraphDefinition = string.Empty;

	public string PieGraphDefinition = string.Empty;

	private bool _UseM1DialingProperties;

	private string _CountryCode = string.Empty;

	private string _AreaCode = string.Empty;

	private string _OutsideLine = string.Empty;

	private bool _ShowOverlap;

	private string _PayflowDefaultType = string.Empty;

	private string _NET1DefaultType = string.Empty;

	public bool QueryUseFieldNames;

	public bool QueryUseActualFieldValues;

	public bool QueryUseWordWrap;

	public bool QueryViewWhiteSpace;

	public bool QueryShowWrappingMarks;

	public bool QueryShowSelectionArea;

	public bool QueryShowLineNumbers;

	public bool QueryShowChangedLineMarkings;

	public bool ShowDatainTextOnlyGrid;

	public string PrivateToken { get; set; } = string.Empty;

	public string CacheToken { get; set; } = string.Empty;

	public string AccountIdentifier { get; set; } = string.Empty;

	public object LabelFontObject => LabelFont;

	public object InputFontObject => InputFont;

	public object MemoFontObject => MemoFont;

	public object RequiredFontObject => RequiredFont;

	public Color RequiredForeColor
	{
		get
		{
			return _RequiredForeColor;
		}
		set
		{
			_RequiredForeColor = value;
			_RequiredForeColorOle = ColorTranslator.ToOle(_RequiredForeColor);
		}
	}

	public Color LabelForeColor
	{
		get
		{
			return _LabelForeColor;
		}
		set
		{
			_LabelForeColor = value;
			_LabelForeColorOle = ColorTranslator.ToOle(_LabelForeColor);
		}
	}

	public Color InputForeColor
	{
		get
		{
			return _InputForeColor;
		}
		set
		{
			_InputForeColor = value;
			_InputForeColorOle = ColorTranslator.ToOle(_InputForeColor);
		}
	}

	public bool LoadGridOnOpen
	{
		get
		{
			return _LoadGridOnOpen;
		}
		set
		{
			_LoadGridOnOpen = value;
		}
	}

	public bool SpeedFormOpen
	{
		get
		{
			return _SpeedFormOpen;
		}
		set
		{
			_SpeedFormOpen = value;
		}
	}

	public int DefaultLeaveScale
	{
		get
		{
			return _DefaultLeaveScale;
		}
		set
		{
			_DefaultLeaveScale = value;
		}
	}

	public bool DefaultWindowSizeToM1Explorer
	{
		get
		{
			return _DefaultWindowSizeToM1Explorer;
		}
		set
		{
			_DefaultWindowSizeToM1Explorer = value;
		}
	}

	public string EmailClientSingleEmail
	{
		get
		{
			return _EmailClientSingleEmail;
		}
		set
		{
			_EmailClientSingleEmail = value;
		}
	}

	public string EmailClientMultipleEmail
	{
		get
		{
			return _EmailClientMultipleEmail;
		}
		set
		{
			_EmailClientMultipleEmail = value;
		}
	}

	public string EMailFormat
	{
		get
		{
			return _EMailFormat;
		}
		set
		{
			_EMailFormat = value;
		}
	}

	public bool EMailReview
	{
		get
		{
			return _EMailReview;
		}
		set
		{
			_EMailReview = value;
		}
	}

	public string FaxFormat
	{
		get
		{
			return _FaxFormat;
		}
		set
		{
			_FaxFormat = value;
		}
	}

	public string FaxNumberFormat
	{
		get
		{
			return _FaxNumberFormat;
		}
		set
		{
			_FaxNumberFormat = value;
		}
	}

	public bool FaxReview
	{
		get
		{
			return _FaxReview;
		}
		set
		{
			_FaxReview = value;
		}
	}

	public bool FaxIncludeFilterText
	{
		get
		{
			return _FaxIncludeFilterText;
		}
		set
		{
			_FaxIncludeFilterText = value;
		}
	}

	public string FaxServer
	{
		get
		{
			return _FaxServer;
		}
		set
		{
			_FaxServer = value;
		}
	}

	public string FaxCoverPage
	{
		get
		{
			return _FaxCoverPage;
		}
		set
		{
			_FaxCoverPage = value;
		}
	}

	public string FaxAddress
	{
		get
		{
			return _FaxAddress;
		}
		set
		{
			_FaxAddress = value;
		}
	}

	public string ReportExportFormat
	{
		get
		{
			return _ReportExportFormat;
		}
		set
		{
			_ReportExportFormat = value;
		}
	}

	public bool AutoOpenDropDown
	{
		get
		{
			return _AutoOpenDropDown;
		}
		set
		{
			_AutoOpenDropDown = value;
		}
	}

	public bool AutoCloseOnPrint
	{
		get
		{
			return _AutoCloseOnPrint;
		}
		set
		{
			_AutoCloseOnPrint = value;
		}
	}

	public bool AutoCheckSpelling
	{
		get
		{
			return _AutoCheckSpelling;
		}
		set
		{
			_AutoCheckSpelling = value;
		}
	}

	public bool SendOnBehalfOf
	{
		get
		{
			return _SendOnBehalfOf;
		}
		set
		{
			_SendOnBehalfOf = value;
		}
	}

	public short IconSize
	{
		get
		{
			return _IconSize;
		}
		set
		{
			_IconSize = value;
			OnPropChanged(this.IconSizeChanged, EventArgs.Empty);
		}
	}

	public string StartPageOptions
	{
		get
		{
			return _StartPageOptions;
		}
		set
		{
			_StartPageOptions = value;
		}
	}

	public int StartPageRefreshInterval
	{
		get
		{
			return _StartPageRefreshInterval;
		}
		set
		{
			_StartPageRefreshInterval = value;
		}
	}

	public string FolderDisplayOption
	{
		get
		{
			return _FolderDisplayOption;
		}
		set
		{
			_FolderDisplayOption = value;
		}
	}

	public bool ShowExplorerTree
	{
		get
		{
			return _ShowExplorerTree;
		}
		set
		{
			_ShowExplorerTree = value;
			OnPropChanged(this.ShowExplorerTreeChanged, EventArgs.Empty);
		}
	}

	public bool ShowEntryShortcutBar
	{
		get
		{
			return _ShowEntryShortcutBar;
		}
		set
		{
			_ShowEntryShortcutBar = value;
		}
	}

	public bool ShowEntryTree
	{
		get
		{
			return _ShowEntryTree;
		}
		set
		{
			_ShowEntryTree = value;
		}
	}

	public string EntryQuickAccessToolbarItems
	{
		get
		{
			return _EntryQuickAccessToolbarItems;
		}
		set
		{
			_EntryQuickAccessToolbarItems = value;
			OnPropChanged(this.EntryQuickAccessToolbarItemsChanged, EventArgs.Empty);
		}
	}

	public string EntryQuickAccessToolbarLocation
	{
		get
		{
			return _EntryQuickAccessToolbarLocation;
		}
		set
		{
			_EntryQuickAccessToolbarLocation = value;
			OnPropChanged(this.EntryQuickAccessToolbarLocationChanged, EventArgs.Empty);
		}
	}

	public bool EntryMinimizeRibbon
	{
		get
		{
			return _EntryMinimizeRibbon;
		}
		set
		{
			_EntryMinimizeRibbon = value;
			OnPropChanged(this.EntryMinimizeRibbonChanged, EventArgs.Empty);
		}
	}

	public int InactiveBinsOptionSelectedAtPartRevisions
	{
		get
		{
			return _InactiveBinsOptionSelectedAtPartRevisions;
		}
		set
		{
			_InactiveBinsOptionSelectedAtPartRevisions = value;
		}
	}

	public int InactiveBinsOptionSelectedAtWarehouse
	{
		get
		{
			return _InactiveBinsOptionSelectedAtWarehouses;
		}
		set
		{
			_InactiveBinsOptionSelectedAtWarehouses = value;
		}
	}

	public string RunQueryQuickAccessToolbarItems
	{
		get
		{
			return _RunQueryQuickAccessToolbarItems;
		}
		set
		{
			_RunQueryQuickAccessToolbarItems = value;
			OnPropChanged(this.RunQueryQuickAccessToolbarItemsChanged, EventArgs.Empty);
		}
	}

	public string RunQueryQuickAccessToolbarLocation
	{
		get
		{
			return _RunQueryQuickAccessToolbarLocation;
		}
		set
		{
			_RunQueryQuickAccessToolbarLocation = value;
			OnPropChanged(this.RunQueryQuickAccessToolbarLocationChanged, EventArgs.Empty);
		}
	}

	public bool RunQueryMinimizeRibbon
	{
		get
		{
			return _RunQueryMinimizeRibbon;
		}
		set
		{
			_RunQueryMinimizeRibbon = value;
			OnPropChanged(this.RunQueryMinimizeRibbonChanged, EventArgs.Empty);
		}
	}

	public bool ShowUserIDInTitleBar
	{
		get
		{
			return _ShowUserIDInTitleBar;
		}
		set
		{
			_ShowUserIDInTitleBar = value;
		}
	}

	public bool OpenWithNewWindow
	{
		get
		{
			return _OpenWithNewWindow;
		}
		set
		{
			_OpenWithNewWindow = value;
		}
	}

	public int ExplorerShortcutBarWidth
	{
		get
		{
			return _ExplorerShortcutBarWidth;
		}
		set
		{
			_ExplorerShortcutBarWidth = value;
		}
	}

	public int ExplorerTreeWidth
	{
		get
		{
			return _ExplorerTreeWidth;
		}
		set
		{
			_ExplorerTreeWidth = value;
		}
	}

	public int EntryShortcutBarWidth
	{
		get
		{
			return _EntryShortcutBarWidth;
		}
		set
		{
			_EntryShortcutBarWidth = value;
		}
	}

	public int EntryTreeWidth
	{
		get
		{
			return _EntryTreeWidth;
		}
		set
		{
			_EntryTreeWidth = value;
		}
	}

	public bool FixedEntryTreeWidth
	{
		get
		{
			return _FixedEntryTreeWidth;
		}
		set
		{
			_FixedEntryTreeWidth = value;
		}
	}

	public bool EnterRunsDefaultButton
	{
		get
		{
			return _EnterRunsDefaultButton;
		}
		set
		{
			_EnterRunsDefaultButton = value;
		}
	}

	public bool PromptToExitM1Explorer
	{
		get
		{
			return _PromptToExitM1Explorer;
		}
		set
		{
			_PromptToExitM1Explorer = value;
		}
	}

	public bool ShowCustomReportsInExplorer
	{
		get
		{
			return _ShowCustomReportsInExplorer;
		}
		set
		{
			_ShowCustomReportsInExplorer = value;
		}
	}

	public bool HideGroupButtons
	{
		get
		{
			return _HideGroupButtons;
		}
		set
		{
			_HideGroupButtons = value;
			OnPropChanged(this.HideGroupButtonsChanged, EventArgs.Empty);
		}
	}

	public bool SuppressKpiMessages
	{
		get
		{
			return _SuppressKpiMessages;
		}
		set
		{
			_SuppressKpiMessages = value;
		}
	}

	public bool RemoveDisabledItemsInExplorer
	{
		get
		{
			return _RemoveDisabledItemsInExplorer;
		}
		set
		{
			_RemoveDisabledItemsInExplorer = value;
		}
	}

	public short MaxLevelsInTree
	{
		get
		{
			return _MaxLevelsInTree;
		}
		set
		{
			_MaxLevelsInTree = value;
		}
	}

	public int ReportPaperSize
	{
		get
		{
			return _ReportPaperSize;
		}
		set
		{
			_ReportPaperSize = value;
		}
	}

	public short TreePrintOption
	{
		get
		{
			return _TreePrintOption;
		}
		set
		{
			_TreePrintOption = value;
		}
	}

	public int EntryValidationBoxHeight
	{
		get
		{
			return _EntryValidationBoxHeight;
		}
		set
		{
			_EntryValidationBoxHeight = value;
		}
	}

	public bool ShowValidationBox
	{
		get
		{
			return _ShowValidationBox;
		}
		set
		{
			_ShowValidationBox = value;
			OnPropChanged(this.ShowValidationBoxChanged, EventArgs.Empty);
		}
	}

	public bool PopupMemos
	{
		get
		{
			return _PopupMemos;
		}
		set
		{
			_PopupMemos = value;
		}
	}

	public short EmailWebLinkOptions
	{
		get
		{
			return _EmailWebLinkOptions;
		}
		set
		{
			_EmailWebLinkOptions = value;
		}
	}

	public string PhoneDeviceName
	{
		get
		{
			return _PhoneDeviceName;
		}
		set
		{
			_PhoneDeviceName = value;
			OnPropChanged(this.PhoneDeviceNameChanged, EventArgs.Empty);
		}
	}

	public bool CheckIncomingCalls
	{
		get
		{
			return _CheckIncomingCalls;
		}
		set
		{
			_CheckIncomingCalls = value;
			OnPropChanged(this.CheckIncomingCallsChanged, EventArgs.Empty);
		}
	}

	public MailProviders MailProvider
	{
		get
		{
			return _MailProvider;
		}
		set
		{
			_MailProvider = value;
		}
	}

	public string ProviderEmailAddress
	{
		get
		{
			return _ProviderEmailAddress;
		}
		set
		{
			_ProviderEmailAddress = value;
		}
	}

	public string ProviderEmailPasswordEncrypted
	{
		get
		{
			return _ProviderEmailPasswordEncrypted;
		}
		set
		{
			_ProviderEmailPasswordEncrypted = value;
		}
	}

	public string Signature
	{
		get
		{
			return _Signature;
		}
		set
		{
			_Signature = value;
		}
	}

	public bool StartRemotingServer
	{
		get
		{
			return _StartRemotingServer;
		}
		set
		{
			_StartRemotingServer = value;
			OnPropChanged(this.StartRemotingServerChanged, EventArgs.Empty);
		}
	}

	public string MapEngine
	{
		get
		{
			return _MapEngine;
		}
		set
		{
			_MapEngine = value;
		}
	}

	public int LabelForeColorOle => _LabelForeColorOle;

	public int InputForeColorOle => _InputForeColorOle;

	public int RequiredForeColorOle => _RequiredForeColorOle;

	public string Theme
	{
		get
		{
			return _Theme;
		}
		set
		{
			_Theme = value;
			setTheme();
		}
	}

	public int ThemeLightColorOle => _ThemeLightColorOle;

	public int ThemeMidColorOle => _ThemeMidColorOle;

	public int ThemeDarkColorOle => _ThemeDarkColorOle;

	public string MyFolders
	{
		get
		{
			return _MyFolders;
		}
		set
		{
			_MyFolders = value;
			OnPropChanged(this.MyFoldersChanged, EventArgs.Empty);
		}
	}

	public bool CacheViews
	{
		get
		{
			return _CacheViews;
		}
		set
		{
			_CacheViews = value;
		}
	}

	public bool ShowExplorerShortcutBar
	{
		get
		{
			return _ShowExplorerShortcutBar;
		}
		set
		{
			_ShowExplorerShortcutBar = value;
			OnPropChanged(this.ShowExplorerShortcutBarChanged, EventArgs.Empty);
		}
	}

	public bool SpellCheckIgnoreInMixedCase
	{
		get
		{
			return _SpellCheckIgnoreInMixedCase;
		}
		set
		{
			_SpellCheckIgnoreInMixedCase = value;
		}
	}

	public bool SpellCheckIgnoreInUpperCase
	{
		get
		{
			return _SpellCheckIgnoreInUpperCase;
		}
		set
		{
			_SpellCheckIgnoreInUpperCase = value;
		}
	}

	public bool SpellCheckIgnoreWithNumbers
	{
		get
		{
			return _SpellCheckIgnoreWithNumbers;
		}
		set
		{
			_SpellCheckIgnoreWithNumbers = value;
		}
	}

	public bool UseM1DialingProperties
	{
		get
		{
			return _UseM1DialingProperties;
		}
		set
		{
			_UseM1DialingProperties = value;
		}
	}

	public string CountryCode
	{
		get
		{
			return _CountryCode;
		}
		set
		{
			_CountryCode = value;
		}
	}

	public string AreaCode
	{
		get
		{
			return _AreaCode;
		}
		set
		{
			_AreaCode = value;
		}
	}

	public string OutsideLine
	{
		get
		{
			return _OutsideLine;
		}
		set
		{
			_OutsideLine = value;
		}
	}

	public bool ShowOverlap
	{
		get
		{
			return _ShowOverlap;
		}
		set
		{
			_ShowOverlap = value;
		}
	}

	public string PayflowDefaultType
	{
		get
		{
			return _PayflowDefaultType;
		}
		set
		{
			_PayflowDefaultType = value;
		}
	}

	public string NET1DefaultType
	{
		get
		{
			return _NET1DefaultType;
		}
		set
		{
			_NET1DefaultType = value;
		}
	}

	public bool SBIncludePreviousOperation { get; set; }

	public bool SBIncludeSubsequentOperation { get; set; }

	public bool SBIncludeChildAssembly { get; set; }

	public bool SBIncludeParentAssemblyToBase { get; set; }

	public bool SBShowTooltip { get; set; }

	public string SBScaleOption { get; set; }

	public bool SBShowOverlap { get; set; }

	public bool SBShowProductionComplete { get; set; }

	public bool SBSizeToFit { get; set; }

	public bool SBHideTree { get; set; }

	public decimal SBReloadIntervalInMinutes { get; set; }

	public string SBOprSortBy { get; set; }

	public bool SBShowPartInGrid { get; set; }

	public bool SBShowCustomerInGrid { get; set; }

	public string SBGridId { get; set; }

	public string SBGridParentField { get; set; }

	public string SBPlantId { get; set; }

	public string SBGridColumns { get; set; }

	public bool SBShowIndentedTree { get; set; }

	public bool LotNumbersFilterUnallocated { get; set; }

	public string SOOperationType { get; set; }

	public bool SOIncludeSubsequent { get; set; }

	public bool SOIncludeChildAssembly { get; set; }

	public bool SOIgnoreMachines { get; set; }

	public bool SOIncludePreviousOperation { get; set; }

	public bool SOIncludeParentAssembly { get; set; }

	public string AngularNavTree { get; set; }

	public bool AngularNavIsHiddenTree { get; set; }

	public string AngularTabs { get; set; }

	public bool AngularNavIsHiddenToogle { get; set; }

	public string AngularMyM1FavoritesItemsSource { get; set; }

	public bool AngularShortcutIconsEnabled { get; set; }

	public int SplitCostOption { get; set; }

	public bool RemoveSourceJobFromSchedule { get; set; }

	public bool IncludePartForecasts { get; set; }

	public bool ConsolidatePartForecastJobs { get; set; }

	public bool PopupMemosIDEntered { get; set; }

	public event EventHandler IconSizeChanged;

	public event EventHandler ShowExplorerTreeChanged;

	public event EventHandler EntryQuickAccessToolbarItemsChanged;

	public event EventHandler EntryQuickAccessToolbarLocationChanged;

	public event EventHandler EntryMinimizeRibbonChanged;

	public event EventHandler RunQueryQuickAccessToolbarItemsChanged;

	public event EventHandler RunQueryQuickAccessToolbarLocationChanged;

	public event EventHandler RunQueryMinimizeRibbonChanged;

	public event EventHandler HideGroupButtonsChanged;

	public event EventHandler ShowValidationBoxChanged;

	public event EventHandler PhoneDeviceNameChanged;

	public event EventHandler CheckIncomingCallsChanged;

	public event EventHandler StartRemotingServerChanged;

	public event EventHandler MyFoldersChanged;

	public event EventHandler ShowExplorerShortcutBarChanged;

	public M1UserSettings(IServiceProvider provider)
	{
		_Provider = provider;
	}

	private void setTheme()
	{
		if (_Theme.Equals("CLASSIC", StringComparison.CurrentCultureIgnoreCase))
		{
			_ThemeLightColorOle = ColorTranslator.ToOle(Color.FromArgb(247, 247, 247));
			_ThemeMidColorOle = ColorTranslator.ToOle(Color.FromArgb(247, 247, 247));
			_ThemeDarkColorOle = ColorTranslator.ToOle(Color.FromArgb(173, 173, 173));
		}
		else if (_Theme.Equals("SILVER", StringComparison.CurrentCultureIgnoreCase))
		{
			_ThemeLightColorOle = ColorTranslator.ToOle(Color.FromArgb(247, 247, 247));
			_ThemeMidColorOle = ColorTranslator.ToOle(Color.FromArgb(247, 247, 247));
			_ThemeDarkColorOle = ColorTranslator.ToOle(Color.FromArgb(173, 173, 173));
		}
		else if (_Theme.Equals("WHITE", StringComparison.CurrentCultureIgnoreCase))
		{
			_ThemeLightColorOle = ColorTranslator.ToOle(Color.FromArgb(255, 255, 255));
			_ThemeMidColorOle = ColorTranslator.ToOle(Color.FromArgb(255, 255, 255));
			_ThemeDarkColorOle = ColorTranslator.ToOle(Color.FromArgb(173, 173, 173));
		}
		else if (_Theme.Equals("DARKGRAY", StringComparison.CurrentCultureIgnoreCase))
		{
			_ThemeLightColorOle = ColorTranslator.ToOle(Color.FromArgb(222, 222, 222));
			_ThemeMidColorOle = ColorTranslator.ToOle(Color.FromArgb(247, 247, 247));
			_ThemeDarkColorOle = ColorTranslator.ToOle(Color.FromArgb(173, 173, 173));
		}
		else if (_Theme.Equals("BLUE", StringComparison.CurrentCultureIgnoreCase))
		{
			_ThemeLightColorOle = ColorTranslator.ToOle(Color.FromArgb(191, 219, 255));
			_ThemeMidColorOle = ColorTranslator.ToOle(Color.FromArgb(164, 195, 238));
			_ThemeDarkColorOle = ColorTranslator.ToOle(Color.FromArgb(101, 147, 207));
		}
		else
		{
			_ThemeLightColorOle = ColorTranslator.ToOle(Color.FromArgb(247, 247, 247));
			_ThemeMidColorOle = ColorTranslator.ToOle(Color.FromArgb(247, 247, 247));
			_ThemeDarkColorOle = ColorTranslator.ToOle(Color.FromArgb(173, 173, 173));
		}
	}

	private void OnPropChanged(EventHandler handler, EventArgs e)
	{
		handler?.Invoke(this, e);
	}

	public string GetUserProperties(M1DataDictionary dataDictionary, string userID)
	{
		SqlCommand sqlCommand = dataDictionary.NewSqlCommand("Select duProperties from DDUsers Where duUserID = @User");
		sqlCommand.Parameters.Add(new SqlParameter("@User", SqlDbType.NVarChar)).Value = userID;
		return Convert.ToString(dataDictionary.ExecuteScalar(sqlCommand));
	}

	public void LoadSettings(string properties)
	{
		LoadDefaults();
		string familyName = LabelFont.Name;
		float emSize = LabelFont.SizeInPoints;
		bool flag = LabelFont.Bold;
		bool flag2 = LabelFont.Italic;
		string familyName2 = InputFont.Name;
		float emSize2 = InputFont.SizeInPoints;
		bool flag3 = InputFont.Bold;
		bool flag4 = InputFont.Italic;
		string familyName3 = MemoFont.Name;
		float emSize3 = MemoFont.SizeInPoints;
		bool flag5 = MemoFont.Bold;
		bool flag6 = MemoFont.Italic;
		string familyName4 = CodeFont.Name;
		float emSize4 = CodeFont.SizeInPoints;
		bool flag7 = CodeFont.Bold;
		bool flag8 = CodeFont.Italic;
		string familyName5 = RequiredFont.Name;
		float emSize5 = RequiredFont.SizeInPoints;
		bool flag9 = RequiredFont.Bold;
		bool flag10 = RequiredFont.Italic;
		if (properties != null)
		{
			string[] array = properties.Split('\r');
			foreach (string text in array)
			{
				int num = text.IndexOf("=");
				if (num <= 0)
				{
					continue;
				}
				string text2 = text.Substring(0, num - 1).Trim().ToUpper();
				string value = text.Substring(num + 1).Trim();
				switch (text2)
				{
				case "SPLITCOSTOPTION":
					SplitCostOption = convertPropToInt(value);
					break;
				case "REMOVESOURCEJOBFROMSCHEDULE":
					RemoveSourceJobFromSchedule = convertPropToBool(value);
					break;
				case "AUTOOPENDROPDOWN":
					AutoOpenDropDown = convertPropToBool(value);
					break;
				case "AUTOCLOSEONPRINT":
					AutoCloseOnPrint = convertPropToBool(value);
					break;
				case "AUTOCHECKSPELLING":
					AutoCheckSpelling = convertPropToBool(value);
					break;
				case "DEFERGRIDSCROLL":
					DeferGridScroll = convertPropToBool(value);
					break;
				case "REQUIREDFONTNAME":
					familyName5 = convertPropToString(value);
					break;
				case "REQUIREDFONTSIZE":
					emSize5 = (float)convertPropToDecimal(value);
					break;
				case "REQUIREDFONTBOLD":
					flag9 = convertPropToBool(value);
					break;
				case "REQUIREDFONTITALIC":
					flag10 = convertPropToBool(value);
					break;
				case "REQUIREDFONTFORECOLOR":
					RequiredForeColor = ColorTranslator.FromWin32(convertPropToInt(value));
					break;
				case "INPUTFONTNAME":
					familyName2 = convertPropToString(value);
					break;
				case "INPUTFONTSIZE":
					emSize2 = (float)convertPropToDecimal(value);
					break;
				case "INPUTFONTBOLD":
					flag3 = convertPropToBool(value);
					break;
				case "INPUTFONTITALIC":
					flag4 = convertPropToBool(value);
					break;
				case "INPUTFONTFORECOLOR":
					InputForeColor = ColorTranslator.FromWin32(convertPropToInt(value));
					break;
				case "MEMOFONTNAME":
					familyName3 = convertPropToString(value);
					break;
				case "MEMOFONTSIZE":
					emSize3 = (float)convertPropToDecimal(value);
					break;
				case "MEMOFONTBOLD":
					flag5 = convertPropToBool(value);
					break;
				case "MEMOFONTITALIC":
					flag6 = convertPropToBool(value);
					break;
				case "CODEFONTNAME":
					familyName4 = convertPropToString(value);
					break;
				case "CODEFONTSIZE":
					emSize4 = (float)convertPropToDecimal(value);
					break;
				case "CODEFONTBOLD":
					flag7 = convertPropToBool(value);
					break;
				case "CODEFONTITALIC":
					flag8 = convertPropToBool(value);
					break;
				case "CODEFONTFORECOLOR":
					CodeForeColor = ColorTranslator.FromWin32(convertPropToInt(value));
					break;
				case "LABELFONTNAME":
					familyName = convertPropToString(value);
					break;
				case "LABELFONTSIZE":
					emSize = (float)convertPropToDecimal(value);
					break;
				case "LABELFONTBOLD":
					flag = convertPropToBool(value);
					break;
				case "LABELFONTITALIC":
					flag2 = convertPropToBool(value);
					break;
				case "LABELFONTFORECOLOR":
					LabelForeColor = ColorTranslator.FromWin32(convertPropToInt(value));
					break;
				case "EMAILCLIENTSINGLEEMAIL":
					EmailClientSingleEmail = convertPropToString(value);
					break;
				case "EMAILCLIENTMULTIPLEEMAIL":
					EmailClientMultipleEmail = convertPropToString(value);
					break;
				case "EMAILFORMAT":
					EMailFormat = convertPropToString(value);
					break;
				case "EMAILREVIEW":
					EMailReview = convertPropToBool(value);
					break;
				case "SENDONBEHALFOF":
					SendOnBehalfOf = convertPropToBool(value);
					break;
				case "FAXFORMAT":
					FaxFormat = convertPropToString(value);
					break;
				case "FAXNUMBERFORMAT":
					FaxNumberFormat = convertPropToString(value);
					break;
				case "FAXREVIEW":
					FaxReview = convertPropToBool(value);
					break;
				case "FAXINCLUDEFILTERTEXT":
					FaxIncludeFilterText = convertPropToBool(value);
					break;
				case "FAXSERVER":
					FaxServer = convertPropToString(value);
					break;
				case "FAXCOVERPAGE":
					FaxCoverPage = convertPropToString(value);
					break;
				case "FAXADDRESS":
					FaxAddress = convertPropToString(value);
					break;
				case "REPORTEXPORTFORMAT":
					ReportExportFormat = convertPropToString(value);
					break;
				case "ICONSIZE":
					_IconSize = convertPropToShort(value);
					break;
				case "STARTPAGEOPTIONS":
					StartPageOptions = convertPropToString(value);
					break;
				case "STARTPAGEREFRESHINTERVAL":
					StartPageRefreshInterval = Convert.ToInt32(convertPropToDecimal(value));
					break;
				case "STARTUPFOLDER":
					StartUpFolder = convertPropToGuid(value);
					break;
				case "STARTUPGROUP":
					StartUpGroup = convertPropToGuid(value);
					break;
				case "FOLDERDISPLAYOPTION":
					FolderDisplayOption = convertPropToString(value);
					break;
				case "SHOWEXPLORERSHORTCUTBAR":
					_ShowExplorerShortcutBar = convertPropToBool(value);
					break;
				case "SHOWEXPLORERTREE":
					_ShowExplorerTree = convertPropToBool(value);
					break;
				case "SHOWEXPLORERSTATUSBAR":
					ShowExplorerStatusBar = convertPropToBool(value);
					break;
				case "SHOWENTRYSHORTCUTBAR":
					ShowEntryShortcutBar = convertPropToBool(value);
					break;
				case "SHOWENTRYTREE":
					ShowEntryTree = convertPropToBool(value);
					break;
				case "SHOWENTRYSTATUSBAR":
					ShowExplorerStatusBar = convertPropToBool(value);
					break;
				case "ENTRYQUICKACCESSTOOLBARITEMS":
					_EntryQuickAccessToolbarItems = convertPropToString(value);
					break;
				case "ENTRYQUICKACCESSTOOLBARLOCATION":
					_EntryQuickAccessToolbarLocation = convertPropToString(value);
					break;
				case "ENTRYMINIMIZERIBBON":
					_EntryMinimizeRibbon = convertPropToBool(value);
					break;
				case "RUNQUERYQUICKACCESSTOOLBARITEMS":
					_RunQueryQuickAccessToolbarItems = convertPropToString(value);
					break;
				case "RUNQUERYQUICKACCESSTOOLBARLOCATION":
					_RunQueryQuickAccessToolbarLocation = convertPropToString(value);
					break;
				case "RUNQUERYMINIMIZERIBBON":
					_RunQueryMinimizeRibbon = convertPropToBool(value);
					break;
				case "SHOWUSERIDINTITLEBAR":
					ShowUserIDInTitleBar = convertPropToBool(value);
					break;
				case "OPENWITHNEWWINDOW":
					OpenWithNewWindow = convertPropToBool(value);
					break;
				case "EXPLORERSHORTCUTBARWIDTH":
					ExplorerShortcutBarWidth = convertPropToInt(value);
					break;
				case "EXPLORERTREEWIDTH":
					ExplorerTreeWidth = convertPropToInt(value);
					break;
				case "ENTRYSHORTCUTBARWIDTH":
					EntryShortcutBarWidth = convertPropToInt(value);
					break;
				case "ENTRYTREEWIDTH":
					EntryTreeWidth = convertPropToInt(value);
					break;
				case "FIXEDENTRYTREEWIDTH":
					FixedEntryTreeWidth = convertPropToBool(value);
					break;
				case "ENTRYVALIDATIONBOXHEIGHT":
					EntryValidationBoxHeight = convertPropToInt(value);
					break;
				case "SHOWVALIDATIONBOX":
					_ShowValidationBox = convertPropToBool(value);
					break;
				case "SHOWMEMOS":
					_PopupMemos = convertPropToBool(value);
					break;
				case "ENTERRUNSDEFAULTBUTTON":
					EnterRunsDefaultButton = convertPropToBool(value);
					break;
				case "PROMPTTOEXITM1EXPLORER":
					PromptToExitM1Explorer = convertPropToBool(value);
					break;
				case "SHOWCUSTOMREPORTSINEXPLORER":
					ShowCustomReportsInExplorer = convertPropToBool(value);
					break;
				case "HIDEGROUPBUTTONS":
					HideGroupButtons = convertPropToBool(value);
					break;
				case "SUPPRESSKPIMESSAGES":
					SuppressKpiMessages = convertPropToBool(value);
					break;
				case "MAXLEVELSINTREE":
					MaxLevelsInTree = convertPropToShort(value);
					break;
				case "REPORTPAPERSIZE":
					ReportPaperSize = convertPropToInt(value);
					break;
				case "TREEPRINTOPTION":
					TreePrintOption = convertPropToShort(value);
					break;
				case "TREESEARCHBOLD":
					TreeSearchBold = convertPropToBool(value);
					break;
				case "TREESEARCHCOLOR":
					TreeSearchColor = ColorTranslator.FromWin32(convertPropToInt(value));
					break;
				case "EMAILWEBLINKOPTIONS":
					EmailWebLinkOptions = convertPropToShort(value);
					break;
				case "PHONEDEVICENAME":
					_PhoneDeviceName = convertPropToString(value);
					break;
				case "CHECKINCOMINGCALLS":
					_CheckIncomingCalls = convertPropToBool(value);
					break;
				case "USEMAPIEMAIL":
					if (convertPropToBool(value))
					{
						MailProvider = MailProviders.Mapi;
					}
					break;
				case "MAILPROVIDER":
					MailProvider = (MailProviders)Enum.Parse(typeof(MailProviders), value);
					break;
				case "PROVIDEREMAILADDRESS":
					ProviderEmailAddress = convertPropToString(value);
					break;
				case "PRIVATETOKEN":
					PrivateToken = convertPropToString(value);
					break;
				case "CACHETOKEN":
					CacheToken = convertPropToString(value);
					break;
				case "ACCOUNTIDENTIFIER":
					AccountIdentifier = convertPropToString(value);
					break;
				case "PROVIDEREMAILPASSWORDENCRYPTED":
					ProviderEmailPasswordEncrypted = convertPropToString(value);
					break;
				case "SIGNATURE":
					Signature = convertPropToString(value).Replace("\\r", "\r");
					break;
				case "STARTREMOTINGSERVER":
					_StartRemotingServer = convertPropToBool(value);
					break;
				case "MAPENGINE":
					MapEngine = convertPropToString(value);
					break;
				case "THEME":
					Theme = convertPropToString(value);
					break;
				case "MYFOLDERS":
					_MyFolders = convertPropToString(value);
					break;
				case "KPISIZES":
					KPISizes = convertPropToString(value);
					break;
				case "CACHEVIEWS":
					CacheViews = convertPropToBool(value);
					break;
				case "SPELLCHECKIGNOREINMIXEDCASE":
					SpellCheckIgnoreInMixedCase = convertPropToBool(value);
					break;
				case "SPELLCHECKIGNOREINUPPERCASE":
					SpellCheckIgnoreInUpperCase = convertPropToBool(value);
					break;
				case "SPELLCHECKIGNOREWITHNUMBERS":
					SpellCheckIgnoreWithNumbers = convertPropToBool(value);
					break;
				case "EXPANDACTIONGROUP":
					ExpandActionGroup = convertPropToBool(value);
					break;
				case "EXPANDREVIEWGROUP":
					ExpandReviewGroup = convertPropToBool(value);
					break;
				case "BARGRAPHDEFINITION":
					BarGraphDefinition = convertPropToString(value);
					break;
				case "PIEGRAPHDEFINITION":
					PieGraphDefinition = convertPropToString(value);
					break;
				case "USEM1DIALINGPROPERTIES":
					UseM1DialingProperties = convertPropToBool(value);
					break;
				case "COUNTRYCODE":
					CountryCode = convertPropToString(value);
					break;
				case "AREACODE":
					AreaCode = convertPropToString(value);
					break;
				case "OUTSIDELINE":
					OutsideLine = convertPropToString(value);
					break;
				case "DEFAULTWINDOWSIZETOM1EXPLORER":
					DefaultWindowSizeToM1Explorer = convertPropToBool(value);
					break;
				case "DEFAULTLEAVESCALE":
					DefaultLeaveScale = convertPropToInt(value);
					break;
				case "SHOWOVERLAP":
					ShowOverlap = convertPropToBool(value);
					break;
				case "PAYFLOWDEFAULTTYPE":
					PayflowDefaultType = convertPropToString(value);
					break;
				case "NET1DEFAULTTYPE":
					NET1DefaultType = convertPropToString(value);
					break;
				case "QUERYUSEFIELDNAMES":
					QueryUseFieldNames = convertPropToBool(value);
					break;
				case "QUERYUSEACTUALFIELDVALUES":
					QueryUseActualFieldValues = convertPropToBool(value);
					break;
				case "QUERYUSEWORDWRAP":
					QueryUseWordWrap = convertPropToBool(value);
					break;
				case "QUERYVIEWWHITESPACE":
					QueryViewWhiteSpace = convertPropToBool(value);
					break;
				case "QUERYSHOWWRAPPINGMARKS":
					QueryShowWrappingMarks = convertPropToBool(value);
					break;
				case "QUERYSHOWSELECTIONAREA":
					QueryShowSelectionArea = convertPropToBool(value);
					break;
				case "QUERYSHOWLINENUMBERS":
					QueryShowLineNumbers = convertPropToBool(value);
					break;
				case "QUERYSHOWCHANGEDLINEMARKINGS":
					QueryShowChangedLineMarkings = convertPropToBool(value);
					break;
				case "SHOWDATAINTEXLONLYGRID":
					ShowDatainTextOnlyGrid = convertPropToBool(value);
					break;
				case "SBSCALEOPTION":
					SBScaleOption = convertPropToString(value);
					break;
				case "SBSHOWTOOLTIP":
					SBShowTooltip = convertPropToBool(value);
					break;
				case "SBINCLUDEPREVIOUSOPERATION":
					SBIncludePreviousOperation = convertPropToBool(value);
					break;
				case "SBINCLUDESUBSEQUENTOPERATION":
					SBIncludeSubsequentOperation = convertPropToBool(value);
					break;
				case "SBINCLUDECHILDASSEMBLY":
					SBIncludeChildAssembly = convertPropToBool(value);
					break;
				case "SBINCLUDEPARENTASSEMBLYTOBASE":
					SBIncludeParentAssemblyToBase = convertPropToBool(value);
					break;
				case "SBSHOWOVERLAP":
					SBShowOverlap = convertPropToBool(value);
					break;
				case "SBSHOWPRODUCTIONCOMPLETE":
					SBShowProductionComplete = convertPropToBool(value);
					break;
				case "SBSIZETOFIT":
					SBSizeToFit = convertPropToBool(value);
					break;
				case "SBHIDETREE":
					SBHideTree = convertPropToBool(value);
					break;
				case "SBRELOADINTERVALINMINUTES":
					SBReloadIntervalInMinutes = convertPropToDecimal(value);
					break;
				case "SBOPRSORTBY":
					SBOprSortBy = convertPropToString(value);
					break;
				case "SBSHOWPARTINGRID":
					SBShowPartInGrid = convertPropToBool(value);
					break;
				case "SBSHOWCUSTOMERINGRID":
					SBShowCustomerInGrid = convertPropToBool(value);
					break;
				case "SBGRIDID":
					SBGridId = convertPropToString(value);
					break;
				case "SBGRIDPARENTFIELD":
					SBGridParentField = convertPropToString(value);
					break;
				case "SBPLANTID":
					SBPlantId = convertPropToString(value);
					break;
				case "SBGRIDCOLUMNS":
					SBGridColumns = convertPropToString(value);
					break;
				case "SBSHOWINDENTEDTREE":
					SBShowIndentedTree = convertPropToBool(value);
					break;
				case "LOTNUMBERSFILTERUNALLOCATED":
					LotNumbersFilterUnallocated = convertPropToBool(value);
					break;
				case "SOOPERATIONTYPE":
					SOOperationType = convertPropToString(value);
					break;
				case "SOINCLUDESUBSEQUENT":
					SOIncludeSubsequent = convertPropToBool(value);
					break;
				case "SOINCLUDECHILDASSEMBLY":
					SOIncludeChildAssembly = convertPropToBool(value);
					break;
				case "SOIGNOREMACHINES":
					SOIgnoreMachines = convertPropToBool(value);
					break;
				case "SOINCLUDEPREVIOUSOPERATIONS":
					SOIncludePreviousOperation = convertPropToBool(value);
					break;
				case "SOINCLUDEPARENTASSEMBLY":
					SOIncludeParentAssembly = convertPropToBool(value);
					break;
				case "ANGULARTREE":
					AngularNavTree = convertPropToString(value);
					break;
				case "ANGULARISHIDDENTREE":
					AngularNavIsHiddenTree = convertPropToBool(value);
					break;
				case "ANGULARTABS":
					AngularTabs = convertPropToString(value);
					break;
				case "ANGULARNAVISHIDDENTOOGLE":
					AngularNavIsHiddenToogle = convertPropToBool(value);
					break;
				case "ANGULARMYM1FAVORITESITEMSSOURCE":
					AngularMyM1FavoritesItemsSource = convertPropToString(value);
					break;
				case "ANGULARSHORTCUTICONSENABLED":
					AngularShortcutIconsEnabled = convertPropToBool(value);
					break;
				case "INACTIVEBINSOPTIONSELECTEDATPARTREVISIONS":
					InactiveBinsOptionSelectedAtPartRevisions = convertPropToInt(value);
					break;
				case "INACTIVEBINSOPTIONSELECTEDATWAREHOUSE":
					InactiveBinsOptionSelectedAtWarehouse = convertPropToInt(value);
					break;
				case "INCLUDEPARTFORECASTS":
					IncludePartForecasts = convertPropToBool(value);
					break;
				case "CONSOLIDATEPARTFORECASTJOBS":
					ConsolidatePartForecastJobs = convertPropToBool(value);
					break;
				case "SHOWMEMOSIDENTERED":
					PopupMemosIDEntered = convertPropToBool(value);
					break;
				}
			}
			OnPropChanged(this.IconSizeChanged, EventArgs.Empty);
			OnPropChanged(this.MyFoldersChanged, EventArgs.Empty);
			OnPropChanged(this.ShowExplorerShortcutBarChanged, EventArgs.Empty);
			OnPropChanged(this.ShowExplorerTreeChanged, EventArgs.Empty);
			OnPropChanged(this.CheckIncomingCallsChanged, EventArgs.Empty);
			OnPropChanged(this.PhoneDeviceNameChanged, EventArgs.Empty);
			OnPropChanged(this.StartRemotingServerChanged, EventArgs.Empty);
			OnPropChanged(this.EntryMinimizeRibbonChanged, EventArgs.Empty);
			OnPropChanged(this.EntryQuickAccessToolbarItemsChanged, EventArgs.Empty);
			OnPropChanged(this.EntryQuickAccessToolbarLocationChanged, EventArgs.Empty);
			OnPropChanged(this.RunQueryMinimizeRibbonChanged, EventArgs.Empty);
			OnPropChanged(this.RunQueryQuickAccessToolbarItemsChanged, EventArgs.Empty);
			OnPropChanged(this.RunQueryQuickAccessToolbarLocationChanged, EventArgs.Empty);
			OnPropChanged(this.ShowValidationBoxChanged, EventArgs.Empty);
			OnPropChanged(this.HideGroupButtonsChanged, EventArgs.Empty);
		}
		LabelFont = new Font(familyName, emSize, (FontStyle)((flag ? 1 : 0) | (flag2 ? 2 : 0)));
		RequiredFont = new Font(familyName5, emSize5, (FontStyle)((flag9 ? 1 : 0) | (flag10 ? 2 : 0)));
		InputFont = new Font(familyName2, emSize2, (FontStyle)((flag3 ? 1 : 0) | (flag4 ? 2 : 0)));
		MemoFont = new Font(familyName3, emSize3, (FontStyle)((flag5 ? 1 : 0) | (flag6 ? 2 : 0)));
		CodeFont = new Font(familyName4, emSize4, (FontStyle)((flag7 ? 1 : 0) | (flag8 ? 2 : 0)));
	}

	private bool convertPropToBool(string value)
	{
		return value.Trim().ToUpper() != "FALSE";
	}

	private decimal convertPropToDecimal(string value)
	{
		decimal result = default(decimal);
		if (decimal.TryParse(value, out result))
		{
			return result;
		}
		return 0m;
	}

	private short convertPropToShort(string value)
	{
		short result = 0;
		if (short.TryParse(value, out result))
		{
			return result;
		}
		return 0;
	}

	private int convertPropToInt(string value)
	{
		int result = 0;
		if (int.TryParse(value, out result))
		{
			return result;
		}
		return 0;
	}

	private string convertPropToString(string value)
	{
		value = value.Trim().Substring(1);
		value = value.Substring(0, value.Length - 1);
		return value;
	}

	private Guid? convertPropToGuid(string value)
	{
		if (value.Length >= 32)
		{
			return new Guid(value);
		}
		return null;
	}

	private string convertBoolToProp(bool value)
	{
		if (value)
		{
			return "True";
		}
		return "False";
	}

	private string convertGuidToProp(Guid? value)
	{
		if (value.HasValue)
		{
			return value.Value.ToString("b");
		}
		return string.Empty;
	}

	private string convertDecimalToProp(decimal value)
	{
		return value.ToString("G");
	}

	private string convertStringToProp(string value)
	{
		return "'" + value + "'";
	}

	public void LoadDefaults()
	{
		SplitCostOption = 1;
		LoadGridOnOpen = true;
		AutoOpenDropDown = false;
		AutoCloseOnPrint = true;
		AutoCheckSpelling = true;
		DeferGridScroll = false;
		LabelFont = new Font("Calibri", 9f);
		LabelForeColor = SystemColors.WindowText;
		RequiredFont = new Font("Calibri", 9f);
		RequiredForeColor = SystemColors.WindowText;
		InputFont = new Font("Calibri", 9f);
		InputForeColor = SystemColors.WindowText;
		MemoFont = new Font("Tahoma", 10f);
		CodeFont = new Font("Courier New", 10f);
		CodeForeColor = SystemColors.WindowText;
		EmailClientSingleEmail = string.Empty;
		EmailClientMultipleEmail = string.Empty;
		EMailFormat = "PDF";
		EMailReview = true;
		SendOnBehalfOf = false;
		FaxFormat = "PDF";
		FaxNumberFormat = "MSFAX";
		FaxReview = true;
		FaxIncludeFilterText = true;
		FaxServer = string.Empty;
		FaxCoverPage = string.Empty;
		FaxAddress = "fax";
		ReportExportFormat = "PDF";
		StartPageOptions = "ALL";
		StartPageRefreshInterval = 0;
		StartUpFolder = null;
		StartUpGroup = null;
		FolderDisplayOption = "FIXED";
		_ShowExplorerShortcutBar = true;
		_ShowExplorerTree = true;
		ShowExplorerStatusBar = true;
		ShowEntryShortcutBar = false;
		ShowEntryTree = true;
		ShowEntryStatusBar = true;
		_EntryQuickAccessToolbarItems = string.Empty;
		_EntryQuickAccessToolbarLocation = "AboveRibbon";
		_EntryMinimizeRibbon = false;
		_RunQueryQuickAccessToolbarItems = string.Empty;
		_RunQueryQuickAccessToolbarLocation = "AboveRibbon";
		_RunQueryMinimizeRibbon = false;
		ShowUserIDInTitleBar = false;
		OpenWithNewWindow = false;
		ShowValidationBox = true;
		PopupMemos = false;
		EntryValidationBoxHeight = -1;
		EntryTreeWidth = -1;
		ExplorerShortcutBarWidth = -1;
		ExplorerTreeWidth = -1;
		EntryShortcutBarWidth = -1;
		FixedEntryTreeWidth = false;
		EnterRunsDefaultButton = true;
		PromptToExitM1Explorer = true;
		ShowCustomReportsInExplorer = true;
		HideGroupButtons = false;
		SuppressKpiMessages = false;
		RemoveDisabledItemsInExplorer = true;
		MaxLevelsInTree = 0;
		ReportPaperSize = 0;
		TreePrintOption = 0;
		EmailWebLinkOptions = 0;
		TreeSearchBold = false;
		TreeSearchColor = Color.Black;
		_PhoneDeviceName = string.Empty;
		_CheckIncomingCalls = false;
		MailProvider = MailProviders.Outlook;
		ProviderEmailAddress = string.Empty;
		PrivateToken = string.Empty;
		CacheToken = string.Empty;
		AccountIdentifier = string.Empty;
		ProviderEmailPasswordEncrypted = string.Empty;
		Signature = string.Empty;
		_StartRemotingServer = true;
		MapEngine = "GOOGLE";
		Theme = "LIGHTGRAY";
		_MyFolders = string.Empty;
		KPISizes = string.Empty;
		CacheViews = true;
		SpellCheckIgnoreInMixedCase = false;
		SpellCheckIgnoreInUpperCase = false;
		SpellCheckIgnoreWithNumbers = false;
		_IconSize = 0;
		ExpandReviewGroup = true;
		ExpandActionGroup = true;
		BarGraphDefinition = "M1SHIPMENTSBYSHIPDATE";
		PieGraphDefinition = "M1SHIPMENTSBYSHIPDATE";
		UseM1DialingProperties = false;
		CountryCode = string.Empty;
		AreaCode = string.Empty;
		OutsideLine = string.Empty;
		DefaultLeaveScale = DateTime.Today.AddMonths(3).Subtract(DateTime.Today).Days;
		DefaultWindowSizeToM1Explorer = true;
		SpeedFormOpen = true;
		ShowOverlap = false;
		PayflowDefaultType = "S";
		NET1DefaultType = "NA";
		QueryUseFieldNames = false;
		QueryUseActualFieldValues = false;
		QueryUseWordWrap = false;
		QueryViewWhiteSpace = false;
		QueryShowWrappingMarks = false;
		QueryShowSelectionArea = false;
		QueryShowLineNumbers = false;
		QueryShowChangedLineMarkings = false;
		ShowDatainTextOnlyGrid = false;
		SBScaleOption = "day";
		SBShowTooltip = false;
		SBIncludePreviousOperation = false;
		SBIncludeSubsequentOperation = false;
		SBIncludeChildAssembly = false;
		SBIncludeParentAssemblyToBase = false;
		SBShowOverlap = false;
		SBShowProductionComplete = false;
		SBSizeToFit = false;
		SBHideTree = false;
		SBReloadIntervalInMinutes = 0m;
		SBOprSortBy = "d";
		SBShowPartInGrid = false;
		SBShowCustomerInGrid = false;
		SBGridId = string.Empty;
		SBGridParentField = string.Empty;
		SBPlantId = string.Empty;
		SBGridColumns = "jmoJobID,jmoJobAssemblyID,jmoJobOperationID,jmoStartDate:W=66,jmoStartHour:W=66,jmoWorkCenterID";
		SBShowIndentedTree = false;
		SOOperationType = "0";
		SOIncludeSubsequent = true;
		SOIncludeChildAssembly = true;
		SOIgnoreMachines = false;
		SOIncludePreviousOperation = true;
		SOIncludeParentAssembly = true;
		AngularNavTree = M1CoreConstants.DEFAULT_TREES;
		AngularNavIsHiddenTree = false;
		AngularTabs = M1CoreConstants.DEFAULT_TABS;
		AngularNavIsHiddenToogle = false;
		AngularMyM1FavoritesItemsSource = string.Empty;
		AngularShortcutIconsEnabled = true;
		LotNumbersFilterUnallocated = false;
		InactiveBinsOptionSelectedAtPartRevisions = 0;
		InactiveBinsOptionSelectedAtWarehouse = 0;
		IncludePartForecasts = false;
		ConsolidatePartForecastJobs = false;
		PopupMemosIDEntered = false;
	}

	public string GetSettingsString(DataRow userRow)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("SplitCostOption = " + convertDecimalToProp(SplitCostOption) + "\r");
		stringBuilder.Append("RemoveSourceJobFromSchedule = " + convertBoolToProp(RemoveSourceJobFromSchedule) + "\r");
		stringBuilder.Append("AutoOpenDropDown = " + convertBoolToProp(AutoOpenDropDown) + "\r");
		stringBuilder.Append("AutoCloseOnPrint = " + convertBoolToProp(AutoCloseOnPrint) + "\r");
		stringBuilder.Append("AutoCheckSpelling = " + convertBoolToProp(AutoCheckSpelling) + "\r");
		stringBuilder.Append("DeferGridScroll = " + convertBoolToProp(DeferGridScroll) + "\r");
		stringBuilder.Append("RequiredFontName = " + convertStringToProp(RequiredFont.Name) + "\r");
		stringBuilder.Append("RequiredFontSize = " + convertDecimalToProp((decimal)RequiredFont.SizeInPoints) + "\r");
		stringBuilder.Append("RequiredFontBold = " + convertBoolToProp(RequiredFont.Bold) + "\r");
		stringBuilder.Append("RequiredFontItalic = " + convertBoolToProp(RequiredFont.Italic) + "\r");
		stringBuilder.Append("RequiredFontForeColor = " + convertDecimalToProp(ColorTranslator.ToWin32(RequiredForeColor)) + "\r");
		stringBuilder.Append("InputFontName = " + convertStringToProp(InputFont.Name) + "\r");
		stringBuilder.Append("InputFontSize = " + convertDecimalToProp((decimal)InputFont.SizeInPoints) + "\r");
		stringBuilder.Append("InputFontBold = " + convertBoolToProp(InputFont.Bold) + "\r");
		stringBuilder.Append("InputFontItalic = " + convertBoolToProp(InputFont.Italic) + "\r");
		stringBuilder.Append("InputFontForeColor = " + convertDecimalToProp(ColorTranslator.ToWin32(InputForeColor)) + "\r");
		stringBuilder.Append("MemoFontName = " + convertStringToProp(MemoFont.Name) + "\r");
		stringBuilder.Append("MemoFontSize = " + convertDecimalToProp((decimal)MemoFont.SizeInPoints) + "\r");
		stringBuilder.Append("MemoFontBold = " + convertBoolToProp(MemoFont.Bold) + "\r");
		stringBuilder.Append("MemoFontItalic = " + convertBoolToProp(MemoFont.Italic) + "\r");
		stringBuilder.Append("CodeFontName = " + convertStringToProp(CodeFont.Name) + "\r");
		stringBuilder.Append("CodeFontSize = " + convertDecimalToProp((decimal)CodeFont.SizeInPoints) + "\r");
		stringBuilder.Append("CodeFontBold = " + convertBoolToProp(CodeFont.Bold) + "\r");
		stringBuilder.Append("CodeFontItalic = " + convertBoolToProp(CodeFont.Italic) + "\r");
		stringBuilder.Append("CodeFontForeColor = " + convertDecimalToProp(ColorTranslator.ToWin32(CodeForeColor)) + "\r");
		stringBuilder.Append("LabelFontName = " + convertStringToProp(LabelFont.Name) + "\r");
		stringBuilder.Append("LabelFontSize = " + convertDecimalToProp((decimal)LabelFont.SizeInPoints) + "\r");
		stringBuilder.Append("LabelFontBold = " + convertBoolToProp(LabelFont.Bold) + "\r");
		stringBuilder.Append("LabelFontItalic = " + convertBoolToProp(LabelFont.Italic) + "\r");
		stringBuilder.Append("LabelFontForeColor = " + convertDecimalToProp(ColorTranslator.ToWin32(LabelForeColor)) + "\r");
		stringBuilder.Append("EmailClientSingleEmail = " + convertStringToProp(EmailClientSingleEmail) + "\r");
		stringBuilder.Append("EmailClientMultipleEmail = " + convertStringToProp(EmailClientMultipleEmail) + "\r");
		stringBuilder.Append("EMailFormat = " + convertStringToProp(EMailFormat) + "\r");
		stringBuilder.Append("EMailReview = " + convertBoolToProp(EMailReview) + "\r");
		stringBuilder.Append("SendOnBehalfOf = " + convertBoolToProp(SendOnBehalfOf) + "\r");
		stringBuilder.Append("FaxFormat = " + convertStringToProp(FaxFormat) + "\r");
		stringBuilder.Append("FaxNumberFormat = " + convertStringToProp(FaxNumberFormat) + "\r");
		stringBuilder.Append("FaxReview = " + convertBoolToProp(FaxReview) + "\r");
		stringBuilder.Append("FaxIncludeFilterText = " + convertBoolToProp(FaxIncludeFilterText) + "\r");
		stringBuilder.Append("FaxServer = " + convertStringToProp(FaxServer) + "\r");
		stringBuilder.Append("FaxCoverPage = " + convertStringToProp(FaxCoverPage) + "\r");
		stringBuilder.Append("FaxAddress = " + convertStringToProp(FaxAddress) + "\r");
		stringBuilder.Append("ReportExportFormat = " + convertStringToProp(ReportExportFormat) + "\r");
		stringBuilder.Append("IconSize = " + convertDecimalToProp(IconSize) + "\r");
		stringBuilder.Append("StartPageOptions = " + convertStringToProp(StartPageOptions) + "\r");
		stringBuilder.Append("StartPageRefreshInterval = " + convertDecimalToProp(StartPageRefreshInterval) + "\r");
		stringBuilder.Append("StartUpFolder = " + convertGuidToProp(StartUpFolder) + "\r");
		stringBuilder.Append("StartUpGroup = " + convertGuidToProp(StartUpGroup) + "\r");
		stringBuilder.Append("FolderDisplayOption = " + convertStringToProp(FolderDisplayOption) + "\r");
		stringBuilder.Append("ShowExplorerShortcutBar = " + convertBoolToProp(ShowExplorerShortcutBar) + "\r");
		stringBuilder.Append("ShowExplorerTree = " + convertBoolToProp(ShowExplorerTree) + "\r");
		stringBuilder.Append("ShowExplorerStatusBar = " + convertBoolToProp(ShowExplorerStatusBar) + "\r");
		stringBuilder.Append("ShowEntryShortcutBar = " + convertBoolToProp(ShowEntryShortcutBar) + "\r");
		stringBuilder.Append("ShowEntryTree = " + convertBoolToProp(ShowEntryTree) + "\r");
		stringBuilder.Append("EntryMinimizeRibbon = " + convertBoolToProp(EntryMinimizeRibbon) + "\r");
		stringBuilder.Append("EntryQuickAccessToolbarItems = " + convertStringToProp(EntryQuickAccessToolbarItems) + "\r");
		stringBuilder.Append("EntryQuickAccessToolbarLocation = " + convertStringToProp(EntryQuickAccessToolbarLocation) + "\r");
		stringBuilder.Append("RunQueryMinimizeRibbon = " + convertBoolToProp(RunQueryMinimizeRibbon) + "\r");
		stringBuilder.Append("RunQueryQuickAccessToolbarItems = " + convertStringToProp(RunQueryQuickAccessToolbarItems) + "\r");
		stringBuilder.Append("RunQueryQuickAccessToolbarLocation = " + convertStringToProp(RunQueryQuickAccessToolbarLocation) + "\r");
		stringBuilder.Append("ShowUserIDInTitleBar = " + convertBoolToProp(ShowUserIDInTitleBar) + "\r");
		stringBuilder.Append("OpenWithNewWindow = " + convertBoolToProp(OpenWithNewWindow) + "\r");
		stringBuilder.Append("ShowEntryStatusBar = " + convertBoolToProp(ShowEntryStatusBar) + "\r");
		stringBuilder.Append("ExplorerShortcutBarWidth = " + convertDecimalToProp(ExplorerShortcutBarWidth) + "\r");
		stringBuilder.Append("ExplorerTreeWidth = " + convertDecimalToProp(ExplorerTreeWidth) + "\r");
		stringBuilder.Append("EntryShortcutBarWidth = " + convertDecimalToProp(EntryShortcutBarWidth) + "\r");
		stringBuilder.Append("EntryTreeWidth = " + convertDecimalToProp(EntryTreeWidth) + "\r");
		stringBuilder.Append("EntryValidationBoxHeight = " + convertDecimalToProp(EntryValidationBoxHeight) + "\r");
		stringBuilder.Append("ShowValidationBox = " + convertBoolToProp(ShowValidationBox) + "\r");
		stringBuilder.Append("ShowMemos = " + convertBoolToProp(PopupMemos) + "\r");
		stringBuilder.Append("FixedEntryTreeWidth = " + convertBoolToProp(FixedEntryTreeWidth) + "\r");
		stringBuilder.Append("EnterRunsDefaultButton = " + convertBoolToProp(EnterRunsDefaultButton) + "\r");
		stringBuilder.Append("PromptToExitM1Explorer = " + convertBoolToProp(PromptToExitM1Explorer) + "\r");
		stringBuilder.Append("ShowCustomReportsInExplorer = " + convertBoolToProp(ShowCustomReportsInExplorer) + "\r");
		stringBuilder.Append("HideGroupButtons = " + convertBoolToProp(HideGroupButtons) + "\r");
		stringBuilder.Append("SuppressKpiMessages = " + convertBoolToProp(SuppressKpiMessages) + "\r");
		stringBuilder.Append("MaxLevelsInTree = " + convertDecimalToProp(MaxLevelsInTree) + "\r");
		stringBuilder.Append("ReportPaperSize = " + convertDecimalToProp(ReportPaperSize) + "\r");
		stringBuilder.Append("TreePrintOption = " + convertDecimalToProp(TreePrintOption) + "\r");
		stringBuilder.Append("TreeSearchBold = " + convertBoolToProp(TreeSearchBold) + "\r");
		stringBuilder.Append("TreeSearchColor = " + convertDecimalToProp(ColorTranslator.ToWin32(TreeSearchColor)) + "\r");
		stringBuilder.Append("EmailWebLinkOptions = " + convertDecimalToProp(EmailWebLinkOptions) + "\r");
		stringBuilder.Append("PhoneDeviceName = " + convertStringToProp(PhoneDeviceName) + "\r");
		stringBuilder.Append("CheckIncomingCalls = " + convertBoolToProp(CheckIncomingCalls) + "\r");
		stringBuilder.Append("MailProvider = " + MailProvider.ToString() + "\r");
		stringBuilder.Append("ProviderEmailAddress = " + convertStringToProp(ProviderEmailAddress) + "\r");
		stringBuilder.Append("PrivateToken = " + convertStringToProp(PrivateToken) + "\r");
		stringBuilder.Append("CacheToken = " + convertStringToProp(CacheToken) + "\r");
		stringBuilder.Append("AccountIdentifier = " + convertStringToProp(AccountIdentifier) + "\r");
		stringBuilder.Append("ProviderEmailPasswordEncrypted = " + convertStringToProp(ProviderEmailPasswordEncrypted) + "\r");
		stringBuilder.Append("Signature = " + convertStringToProp(Signature.Replace("\r", "\\r")) + "\r");
		stringBuilder.Append("StartRemotingServer = " + convertBoolToProp(StartRemotingServer) + "\r");
		stringBuilder.Append("MapEngine = " + convertStringToProp(MapEngine) + "\r");
		stringBuilder.Append("Theme = " + convertStringToProp(Theme) + "\r");
		stringBuilder.Append("MyFolders = " + convertStringToProp(MyFolders) + "\r");
		stringBuilder.Append("KPISizes = " + convertStringToProp(KPISizes) + "\r");
		stringBuilder.Append("CacheViews = " + convertBoolToProp(CacheViews) + "\r");
		stringBuilder.Append("SpellCheckIgnoreInMixedCase = " + convertBoolToProp(SpellCheckIgnoreInMixedCase) + "\r");
		stringBuilder.Append("SpellCheckIgnoreInUpperCase = " + convertBoolToProp(SpellCheckIgnoreInUpperCase) + "\r");
		stringBuilder.Append("SpellCheckIgnoreWithNumbers = " + convertBoolToProp(SpellCheckIgnoreWithNumbers) + "\r");
		if (userRow != null)
		{
			stringBuilder.Append(retrieveCustomProperties(userRow.Field<string>("duProperties")));
		}
		stringBuilder.Append("ExpandReviewGroup = " + convertBoolToProp(ExpandReviewGroup) + "\r");
		stringBuilder.Append("ExpandActionGroup = " + convertBoolToProp(ExpandActionGroup) + "\r");
		stringBuilder.Append("BarGraphDefinition = " + convertStringToProp(BarGraphDefinition) + "\r");
		stringBuilder.Append("PieGraphDefinition = " + convertStringToProp(PieGraphDefinition) + "\r");
		stringBuilder.Append("UseM1DialingProperties = " + convertBoolToProp(UseM1DialingProperties) + "\r");
		stringBuilder.Append("CountryCode = " + convertStringToProp(CountryCode) + "\r");
		stringBuilder.Append("AreaCode = " + convertStringToProp(AreaCode) + "\r");
		stringBuilder.Append("OutsideLine = " + convertStringToProp(OutsideLine) + "\r");
		stringBuilder.Append("DefaultWindowSizeToM1Explorer = " + convertBoolToProp(DefaultWindowSizeToM1Explorer) + "\r");
		stringBuilder.Append("DefaultLeaveScale = " + convertDecimalToProp(DefaultLeaveScale) + "\r");
		stringBuilder.Append("ShowOverlap = " + convertBoolToProp(ShowOverlap) + "\r");
		stringBuilder.Append("PayflowDefaultType = " + convertStringToProp(PayflowDefaultType) + "\r");
		stringBuilder.Append("NET1DefaultType = " + convertStringToProp(NET1DefaultType) + "\r");
		stringBuilder.Append("QueryUseFieldNames = " + convertBoolToProp(QueryUseFieldNames) + "\r");
		stringBuilder.Append("QueryUseActualFieldValues = " + convertBoolToProp(QueryUseActualFieldValues) + "\r");
		stringBuilder.Append("QueryUseWordWrap = " + convertBoolToProp(QueryUseWordWrap) + "\r");
		stringBuilder.Append("QueryViewWhiteSpace = " + convertBoolToProp(QueryViewWhiteSpace) + "\r");
		stringBuilder.Append("QueryShowWrappingMarks = " + convertBoolToProp(QueryShowWrappingMarks) + "\r");
		stringBuilder.Append("QueryShowSelectionArea = " + convertBoolToProp(QueryShowSelectionArea) + "\r");
		stringBuilder.Append("QueryShowLineNumbers = " + convertBoolToProp(QueryShowLineNumbers) + "\r");
		stringBuilder.Append("QueryShowChangedLineMarkings = " + convertBoolToProp(QueryShowChangedLineMarkings) + "\r");
		stringBuilder.Append("ShowDatainTextOnlyGrid = " + convertBoolToProp(ShowDatainTextOnlyGrid) + "\r");
		stringBuilder.Append("SBScaleOption = " + convertStringToProp(SBScaleOption) + "\r");
		stringBuilder.Append("SBShowTooltip = " + convertBoolToProp(SBShowTooltip) + "\r");
		stringBuilder.Append("SBIncludePreviousOperation = " + convertBoolToProp(SBIncludePreviousOperation) + "\r");
		stringBuilder.Append("SBIncludeSubsequentOperation = " + convertBoolToProp(SBIncludeSubsequentOperation) + "\r");
		stringBuilder.Append("SBIncludeChildAssembly = " + convertBoolToProp(SBIncludeChildAssembly) + "\r");
		stringBuilder.Append("SBIncludeParentAssemblyToBase = " + convertBoolToProp(SBIncludeParentAssemblyToBase) + "\r");
		stringBuilder.Append("SBShowOverlap = " + convertBoolToProp(SBShowOverlap) + "\r");
		stringBuilder.Append("SBShowProductionComplete = " + convertBoolToProp(SBShowProductionComplete) + "\r");
		stringBuilder.Append("SBSizeToFit = " + convertBoolToProp(SBSizeToFit) + "\r");
		stringBuilder.Append("SBHideTree = " + convertBoolToProp(SBHideTree) + "\r");
		stringBuilder.Append("SBReloadIntervalInMinutes = " + convertDecimalToProp(SBReloadIntervalInMinutes) + "\r");
		stringBuilder.Append("SBOprSortBy = " + convertStringToProp(SBOprSortBy) + "\r");
		stringBuilder.Append("SBShowPartInGrid = " + convertBoolToProp(SBShowPartInGrid) + "\r");
		stringBuilder.Append("SBShowCustomerInGrid = " + convertBoolToProp(SBShowCustomerInGrid) + "\r");
		stringBuilder.Append("SBGridId = " + convertStringToProp(SBGridId) + "\r");
		stringBuilder.Append("SBGridParentField = " + convertStringToProp(SBGridParentField) + "\r");
		stringBuilder.Append("SBPlantId = " + convertStringToProp(SBPlantId) + "\r");
		stringBuilder.Append($"SBGridColumns = '{SBGridColumns.Trim('\'')}'{'\r'}");
		stringBuilder.Append("SBShowIndentedTree = " + convertBoolToProp(SBShowIndentedTree) + "\r");
		stringBuilder.Append("LotNumbersFilterUnallocated = " + convertBoolToProp(LotNumbersFilterUnallocated) + "\r");
		stringBuilder.Append("SOOperationType = " + convertStringToProp(SOOperationType) + "\r");
		stringBuilder.Append("SOIncludeSubsequent = " + convertBoolToProp(SOIncludeSubsequent) + "\r");
		stringBuilder.Append("SOIncludeChildAssembly = " + convertBoolToProp(SOIncludeChildAssembly) + "\r");
		stringBuilder.Append("SOIgnoreMachines = " + convertBoolToProp(SOIgnoreMachines) + "\r");
		stringBuilder.Append("SOIncludePreviousOperations = " + convertBoolToProp(SOIncludePreviousOperation) + "\r");
		stringBuilder.Append("SOIncludeParentAssembly = " + convertBoolToProp(SOIncludeParentAssembly) + "\r");
		stringBuilder.Append("AngularTree = " + convertStringToProp(AngularNavTree) + "\r");
		stringBuilder.Append("AngularIsHiddenTree = " + convertBoolToProp(AngularNavIsHiddenTree) + "\r");
		stringBuilder.Append("AngularTabs = " + convertStringToProp(AngularTabs) + "\r");
		stringBuilder.Append("AngularNavIsHiddenToogle = " + convertBoolToProp(AngularNavIsHiddenToogle) + "\r");
		stringBuilder.Append("AngularMyM1FavoritesItemsSource = " + convertStringToProp(AngularMyM1FavoritesItemsSource) + "\r");
		stringBuilder.Append("AngularShortcutIconsEnabled = " + convertBoolToProp(AngularShortcutIconsEnabled) + "\r");
		stringBuilder.Append("InactiveBinsOptionSelectedAtPartRevisions = " + convertDecimalToProp(InactiveBinsOptionSelectedAtPartRevisions) + "\r");
		stringBuilder.Append("InactiveBinsOptionSelectedAtWarehouse = " + convertDecimalToProp(InactiveBinsOptionSelectedAtWarehouse) + "\r");
		stringBuilder.Append("IncludePartForecasts = " + convertBoolToProp(IncludePartForecasts) + "\r");
		stringBuilder.Append("ConsolidatePartForecastJobs = " + convertBoolToProp(ConsolidatePartForecastJobs) + "\r");
		stringBuilder.Append("ShowMemosIDEntered = " + convertBoolToProp(PopupMemosIDEntered) + "\r");
		return stringBuilder.ToString();
	}

	public bool SaveSettings(DataRow userRow)
	{
		userRow.SetField("duProperties", GetSettingsString(userRow));
		return true;
	}

	public bool SaveSettings(M1DataDictionary dataDictionary, string userID)
	{
		bool result = false;
		DataSet dataSet = new DataSet();
		SqlCommand sqlCommand = dataDictionary.NewSqlCommand("Select * From DDUsers Where duUserID = @User");
		sqlCommand.Parameters.Add(new SqlParameter("@User", SqlDbType.NVarChar)).Value = userID;
		SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
		sqlDataAdapter.Fill(dataSet, "Users");
		if (dataSet.Tables["Users"].Rows.Count != 0)
		{
			SaveSettings(dataSet.Tables["Users"].Rows[0]);
			new SqlCommandBuilder(sqlDataAdapter);
			sqlDataAdapter.Update(dataSet.Tables["Users"].GetChanges());
			result = true;
		}
		return result;
	}

	private string retrieveCustomProperties(string props)
	{
		if (props != null)
		{
			StringBuilder stringBuilder = new StringBuilder();
			string[] array = props.Split('\r');
			foreach (string text in array)
			{
				if (text.IndexOf("=") > 0 && text.Trim().ToUpper().StartsWith("CUSTOM"))
				{
					stringBuilder.Append(text);
				}
			}
			return stringBuilder.ToString();
		}
		return string.Empty;
	}

	public void MyFoldersRemoveItem(Guid folderID)
	{
		string[] array = MyFolders.Split(',');
		string text = folderID.ToString("b");
		string text2 = string.Empty;
		_ = string.Empty;
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i] == text)
			{
				if (i + 1 < array.Length)
				{
					_ = array[i + 1];
				}
				else if (i - 1 >= 0)
				{
					_ = array[i - 1];
				}
			}
			else
			{
				text2 = text2 + "," + array[i];
			}
		}
		if (text2.StartsWith(","))
		{
			text2 = text2.Substring(1);
		}
		if (text2.EndsWith(","))
		{
			text2 = text2.Substring(0, text2.Length - 1);
		}
		MyFolders = text2;
	}

	public void MyFoldersAddItem(Guid folderID)
	{
		string text = folderID.ToString("b");
		string text2 = MyFolders;
		if (!("," + text2 + ",").Contains("," + text + ","))
		{
			if (text2.Length != 0)
			{
				text2 += ",";
			}
			text2 += text;
			MyFolders = text2;
		}
	}

	public void MyFoldersMoveItem(Guid folderID, short direction)
	{
		if (MyFolders.Length == 0)
		{
			return;
		}
		string text = folderID.ToString("b");
		string[] array = MyFolders.Split(',');
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i] == text && i + direction >= 0 && i + direction < array.Length)
			{
				array[i] = array[i + direction];
				array[i + direction] = text;
				break;
			}
		}
		string text2 = string.Empty;
		string[] array2 = array;
		foreach (string text3 in array2)
		{
			if (text3.Length != 0)
			{
				text2 = text2 + "," + text3;
			}
		}
		if (text2.StartsWith(","))
		{
			text2.Substring(1);
		}
		MyFolders = text2;
	}

	public object GetService(Type serviceType)
	{
		return _Provider.GetService(serviceType);
	}

	public void Dispose()
	{
		_Provider = null;
	}
}
