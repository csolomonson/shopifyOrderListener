using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using M1.Extensions;
using M1.Images;
using M1.Images.Properties;

namespace M1.Core;

public sealed class ExplorerItem
{
	private string _GridID = string.Empty;

	private string _GridTable = string.Empty;

	private string _SecurityComponent = string.Empty;

	private string _SecurityModule = string.Empty;

	private string _Mode = string.Empty;

	private bool _Enabled = true;

	public M1User User;

	public M1Database Database;

	public M1DataDictionary DataDictionary;

	public string ResolvedObjectID = string.Empty;

	public string ResolvedReportFolder = string.Empty;

	public string ResolvedReportName = string.Empty;

	public string ResolvedFormID = string.Empty;

	private SecurityAccessLevel? cachedGridAccessLevel;

	private bool _CanSaveItem = true;

	private bool _IsCustomReport;

	private bool _IsCustomForm;

	private string _VisualizeID = string.Empty;

	private VisualizerType _VisualizerType;

	private string _Data = string.Empty;

	public ExplorerType Type { get; set; }

	public string GridID
	{
		get
		{
			return _GridID;
		}
		set
		{
			_GridID = value;
		}
	}

	public string GridTable
	{
		get
		{
			return _GridTable;
		}
		set
		{
			_GridTable = value;
		}
	}

	public Guid UniqueID { get; set; }

	public DisplayType ViewerType { get; set; }

	public string Key { get; set; }

	public string Caption { get; set; }

	public string ImageLarge { get; set; }

	public string ImageSmall { get; set; }

	public string SecurityComponent
	{
		get
		{
			return _SecurityComponent;
		}
		set
		{
			_SecurityComponent = value;
		}
	}

	public string SecurityModule
	{
		get
		{
			return _SecurityModule;
		}
		set
		{
			_SecurityModule = value;
		}
	}

	public Guid? ParentUniqueID { get; set; }

	public Guid? LinkedUniqueID { get; set; }

	public string Mode
	{
		get
		{
			return _Mode;
		}
		set
		{
			_Mode = value;
		}
	}

	public string UserID { get; set; }

	public int Sequence { get; set; }

	public bool Custom { get; set; }

	public bool Enabled
	{
		get
		{
			return _Enabled;
		}
		set
		{
			_Enabled = value;
		}
	}

	public bool Collapsed { get; set; }

	public bool Removed { get; set; }

	public bool SecurityAccessIsAvailable { get; set; }

	public bool CanSaveItem
	{
		get
		{
			return _CanSaveItem;
		}
		set
		{
			_CanSaveItem = value;
		}
	}

	public bool IsCustomReport
	{
		get
		{
			return _IsCustomReport;
		}
		set
		{
			_IsCustomReport = value;
		}
	}

	public bool IsCustomForm
	{
		get
		{
			return _IsCustomForm;
		}
		set
		{
			_IsCustomForm = value;
		}
	}

	public string VisualizerID
	{
		get
		{
			return _VisualizeID;
		}
		set
		{
			_VisualizeID = value;
		}
	}

	public VisualizerType VisualizerType
	{
		get
		{
			return _VisualizerType;
		}
		set
		{
			_VisualizerType = value;
		}
	}

	public string Data
	{
		get
		{
			return _Data;
		}
		set
		{
			_Data = value;
			ResolvedObjectID = string.Empty;
			ResolvedReportName = string.Empty;
			ResolvedReportFolder = string.Empty;
			ResolvedFormID = string.Empty;
			if (_Data.Length == 0)
			{
				return;
			}
			string firstParameter = GetFirstParameter(_Data, "Forms.OpenObject");
			if (firstParameter.Length != 0)
			{
				ResolvedObjectID = firstParameter;
				return;
			}
			firstParameter = GetFirstParameter(_Data, "Forms.Report.Run");
			if (firstParameter.Length != 0)
			{
				int num = firstParameter.IndexOf('\\');
				if (num != -1)
				{
					ResolvedReportFolder = firstParameter.Substring(0, num);
					ResolvedReportName = firstParameter.Substring(num + 1);
				}
				else
				{
					ResolvedReportFolder = firstParameter;
					ResolvedReportName = string.Empty;
				}
			}
			else
			{
				firstParameter = GetFirstParameter(_Data, "Forms.OpenForm");
				if (firstParameter.Length != 0)
				{
					ResolvedFormID = firstParameter;
				}
			}
		}
	}

	public event EventHandler DataChanged;

	public ExplorerItem()
	{
	}

	public ExplorerItem(M1User user, M1Database database, M1DataDictionary dataDictionary)
	{
		loadData(user, database, dataDictionary, null);
	}

	public ExplorerItem(M1User user, M1Database database, M1DataDictionary dataDictionary, DataRow ddExplorerRow)
	{
		loadData(user, database, dataDictionary, ddExplorerRow);
	}

	private void loadData(M1User user, M1Database database, M1DataDictionary dataDictionary, DataRow ddExplorerRow)
	{
		User = user;
		Database = database;
		DataDictionary = dataDictionary;
		UniqueID = Guid.NewGuid();
		if (ddExplorerRow != null)
		{
			loadFromDDExplorerRow(ddExplorerRow);
		}
	}

	public static string GetFirstParameter(string codeToCheck, string functionToFind)
	{
		string text = string.Empty;
		int num = 0;
		string empty = string.Empty;
		codeToCheck = codeToCheck.Trim();
		functionToFind = functionToFind.Trim().ToUpper();
		if (codeToCheck.Length != 0)
		{
			if (codeToCheck.StartsWith(functionToFind, StringComparison.CurrentCultureIgnoreCase))
			{
				text = codeToCheck.Substring(functionToFind.Length).Trim();
			}
			else if (codeToCheck.StartsWith("CALL " + functionToFind, StringComparison.CurrentCultureIgnoreCase))
			{
				text = codeToCheck.Substring(functionToFind.Length + 5);
				if (text.Substring(0, 1) == "(")
				{
					text = text.Substring(1);
					text = text.Substring(0, text.Length - 1).Trim();
				}
			}
			if (text.Trim().Length != 0)
			{
				empty = text.Substring(0, 1);
				if (empty == "(")
				{
					text = text.Substring(1);
					empty = text.Substring(0, 1);
				}
				if (empty == "'" || empty == "\"")
				{
					text = text.Substring(1);
					num = text.IndexOf(empty, 0);
					if (num >= 0)
					{
						text = text.Substring(0, num);
					}
				}
				else
				{
					num = text.IndexOf(",", 0);
					if (num >= 0)
					{
						text = text.Substring(0, num);
					}
				}
			}
		}
		return text;
	}

	private void onDataChanged()
	{
		this.DataChanged?.Invoke(this, EventArgs.Empty);
	}

	public void LoadFromExistingItem(ExplorerItem existingItem)
	{
		UniqueID = Guid.NewGuid();
		ParentUniqueID = null;
		LinkedUniqueID = null;
		Caption = existingItem.Caption;
		ImageLarge = existingItem.ImageLarge;
		ImageSmall = existingItem.ImageSmall;
		Data = existingItem.Data;
		Type = existingItem.Type;
		ViewerType = existingItem.ViewerType;
		GridID = existingItem.GridID;
		GridTable = existingItem.GridTable;
		VisualizerID = existingItem.VisualizerID;
		VisualizerType = existingItem.VisualizerType;
		SecurityComponent = existingItem.SecurityComponent;
		SecurityModule = existingItem.SecurityModule;
		Mode = existingItem.Mode;
		UserID = existingItem.UserID;
		Sequence = 0;
		Enabled = true;
		Collapsed = false;
		Removed = false;
		Custom = true;
		Key = string.Empty;
		LoadComplete();
	}

	private void loadFromDDExplorerRow(DataRow ddExplorerRow)
	{
		UniqueID = ddExplorerRow.Field<Guid>("dxUniqueID");
		Caption = ddExplorerRow.Field<string>("dxtext");
		ImageLarge = ddExplorerRow.Field<string>("dxImageLarge");
		ImageSmall = ddExplorerRow.Field<string>("dxImageSmall");
		Data = ddExplorerRow.Field<string>("dxextd");
		Type = ddExplorerRow.Field<ExplorerType>("dxtype");
		ViewerType = ddExplorerRow.Field<DisplayType>("dxviewer");
		GridID = ddExplorerRow.Field<string>("dxgridid");
		GridTable = ddExplorerRow.Field<string>("GridTable");
		SecurityComponent = ddExplorerRow.Field<string>("dxscom");
		SecurityModule = ddExplorerRow.Field<string>("dxsmod");
		ParentUniqueID = ddExplorerRow.Field<Guid?>("dxParentUniqueID");
		LinkedUniqueID = ddExplorerRow.Field<Guid?>("dxLinkedUniqueID");
		Mode = ddExplorerRow.Field<string>("dxmode");
		UserID = ddExplorerRow.Field<string>("dxUser");
		Sequence = ddExplorerRow.Field<int>("dxSequence");
		Enabled = !ddExplorerRow.Field<bool>("dxDisabled");
		Collapsed = ddExplorerRow.Field<bool>("dxCollapse");
		Removed = ddExplorerRow.Field<bool>("dxRemoved");
		Custom = ddExplorerRow.Field<bool>("dxCustom");
		VisualizerID = ddExplorerRow.Field<string>("dxVisualizerID");
		VisualizerType = ddExplorerRow.Field<VisualizerType>("dxVisualizerType");
		cachedGridAccessLevel = null;
		if (Type == ExplorerType.Report && Data.Length != 0)
		{
			Key = Data.Replace('\'', ' ').Split(' ')[1].Substring(1, Data.Replace('\'', ' ').Split(' ')[1].Length - 2);
		}
		else
		{
			Key = string.Empty;
		}
		LoadComplete();
	}

	public void LoadComplete()
	{
		SecurityAccessIsAvailable = checkSecurity();
	}

	public void SetAsCustom()
	{
		if (!Custom)
		{
			LinkedUniqueID = UniqueID;
			UserID = User.ID;
			UniqueID = Guid.NewGuid();
			Custom = true;
		}
	}

	public void CheckSecurityForFolder(ExplorerItemCollection items)
	{
		if (!UniqueID.ToString("b").Equals("{804E1CBC-5A40-4615-BC03-C0D1967B0368}"))
		{
			return;
		}
		IEnumerable<ExplorerItem> enumerable = items.Where((ExplorerItem row) => row.ParentUniqueID == UniqueID);
		SecurityAccessIsAvailable = false;
		foreach (ExplorerItem item in enumerable)
		{
			if (item.SecurityAccessIsAvailable)
			{
				SecurityAccessIsAvailable = true;
				break;
			}
		}
	}

	private bool checkSecurity()
	{
		return CheckModuleStatus(checkForCustomReports: false);
	}

	private bool CheckModuleStatus(bool checkForCustomReports)
	{
		bool removeDisabledItemsInExplorer = User.Settings.RemoveDisabledItemsInExplorer;
		bool flag = false;
		if (SecurityModule.Length != 0 && Type != ExplorerType.Group)
		{
			flag = ((SecurityModule.Length <= 2) ? DataDictionary.ProductCode.IsModulePurchased(SecurityModule, Database) : (Database == null || Database.Security.IsInRole(SecurityModule)));
			if (flag && removeDisabledItemsInExplorer && Database != null && (Database.Security.GetModuleAccessLevel(SecurityModule) & SecurityAccessLevel.None) != SecurityAccessLevel.Default)
			{
				flag = false;
			}
		}
		else
		{
			flag = true;
		}
		if (flag && Removed)
		{
			flag = false;
		}
		if (flag && SecurityComponent.Length != 0 && Database != null && !Database.Security.IsInRole(SecurityComponent))
		{
			flag = false;
		}
		if (flag && removeDisabledItemsInExplorer)
		{
			switch (Type)
			{
			case ExplorerType.Entry:
			case ExplorerType.Wizard:
			case ExplorerType.Tool:
			case ExplorerType.Report:
			case ExplorerType.Maintenance:
			case ExplorerType.Explorer:
			case ExplorerType.Help:
			case ExplorerType.Dashboard:
				if (ResolvedObjectID.Length != 0)
				{
					if (Database != null && (Database.Security.GetObjectAccessLevel(ResolvedObjectID) & SecurityAccessLevel.None) != SecurityAccessLevel.Default)
					{
						flag = false;
					}
				}
				else if (ResolvedReportFolder.Length != 0)
				{
					if (checkForCustomReports)
					{
						if (Database != null && !Database.Security.CanReportBeRun(ResolvedReportFolder))
						{
							flag = false;
						}
					}
					else if (Database != null && Database.Security.GetReportAccessLevel(ResolvedReportFolder, ResolvedReportName) == SecurityAccessLevel.None)
					{
						flag = false;
					}
				}
				else if (ResolvedFormID.Length != 0 && Database != null && (Database.Security.GetFormAccessLevel(ResolvedFormID) & SecurityAccessLevel.None) != SecurityAccessLevel.Default)
				{
					flag = false;
				}
				break;
			case ExplorerType.DataViewer:
				if (!cachedGridAccessLevel.HasValue && Database != null)
				{
					if (GridTable.Length != 0)
					{
						cachedGridAccessLevel = Database.Security.GetTableAccessLevel(GridTable);
					}
					else
					{
						cachedGridAccessLevel = Database.Security.GetGridAccessLevel(GridID);
					}
				}
				if (cachedGridAccessLevel.HasValue && ((uint?)cachedGridAccessLevel & 1u) != 0)
				{
					flag = false;
				}
				break;
			}
		}
		return flag;
	}

	public void AddToDesktop()
	{
		if (Caption.Length > 0)
		{
			if (Data.Length > 0)
			{
				M1Util.CreateURLFile(Caption.Trim(), "M1:Script:" + Data.Trim());
			}
			else if (GridID.Length > 0)
			{
				M1Util.CreateURLFile(Caption.Trim(), "M1:Script:Forms.Show.Search " + GridID.Trim().ToScript());
			}
		}
	}

	public bool DeleteItemFromDDExplorer()
	{
		SqlCommand sqlCommand;
		if (Custom && !LinkedUniqueID.HasValue)
		{
			sqlCommand = DataDictionary.NewSqlCommand("DELETE FROM DDExplorer WHERE dxMode = @Mode and dxUser = @User and dxParentUniqueID In (Select dxUniqueId From DDExplorer Where dxMode = @Mode and dxUser = @User and dxParentUniqueID = @CurId)");
			sqlCommand.Parameters.Add(new SqlParameter("@Mode", SqlDbType.NVarChar)).Value = Mode;
			sqlCommand.Parameters.Add(new SqlParameter("@User", SqlDbType.NVarChar)).Value = UserID;
			sqlCommand.Parameters.Add(new SqlParameter("@CurId", SqlDbType.UniqueIdentifier)).Value = UniqueID;
			DataDictionary.ExecuteCommand(sqlCommand);
			sqlCommand = DataDictionary.NewSqlCommand("DELETE FROM DDExplorer WHERE dxMode = @Mode and dxUser = @User and dxParentUniqueID = @CurId");
			sqlCommand.Parameters.Add(new SqlParameter("@Mode", SqlDbType.NVarChar)).Value = Mode;
			sqlCommand.Parameters.Add(new SqlParameter("@User", SqlDbType.NVarChar)).Value = UserID;
			sqlCommand.Parameters.Add(new SqlParameter("@CurId", SqlDbType.UniqueIdentifier)).Value = UniqueID;
			DataDictionary.ExecuteCommand(sqlCommand);
		}
		sqlCommand = DataDictionary.NewSqlCommand("DELETE FROM DDExplorer WHERE dxMode = @Mode and dxUser = @User and dxUniqueID = @CurId");
		sqlCommand.Parameters.Add(new SqlParameter("@Mode", SqlDbType.NVarChar)).Value = Mode;
		sqlCommand.Parameters.Add(new SqlParameter("@User", SqlDbType.NVarChar)).Value = UserID;
		sqlCommand.Parameters.Add(new SqlParameter("@CurId", SqlDbType.UniqueIdentifier)).Value = UniqueID;
		DataDictionary.ExecuteCommand(sqlCommand);
		return true;
	}

	public void SaveItemToDDExplorer()
	{
		if (_CanSaveItem)
		{
			DataTable dataTable = null;
			SqlCommand sqlCommand;
			if (Sequence == 0)
			{
				sqlCommand = DataDictionary.NewSqlCommand("Select isnull(max(dxSequence),0) + 1 as dxSequence From DDExplorer Where dxmode = @Mode and (dxUser = @User or dxUser = '') and dxParentUniqueID = @ParentUniqueID");
				sqlCommand.Parameters.Add(new SqlParameter("@Mode", SqlDbType.NVarChar)).Value = Mode;
				sqlCommand.Parameters.Add(new SqlParameter("@User", SqlDbType.NVarChar)).Value = UserID;
				sqlCommand.Parameters.Add(new SqlParameter("@ParentUniqueID", SqlDbType.UniqueIdentifier)).Value = ((!ParentUniqueID.HasValue) ? new Guid?(Guid.Empty) : ParentUniqueID);
				Sequence = (int)DataDictionary.ExecuteScalar(sqlCommand);
				sqlCommand = null;
			}
			DataRow dataRow = null;
			sqlCommand = DataDictionary.NewSqlCommand("Select * From DDExplorer Where dxmode = @Mode and (dxUser = @User Or dxUser = '') And dxUniqueID = @CurId");
			sqlCommand.Parameters.Add(new SqlParameter("@Mode", SqlDbType.NVarChar)).Value = Mode;
			sqlCommand.Parameters.Add(new SqlParameter("@User", SqlDbType.NVarChar)).Value = UserID;
			sqlCommand.Parameters.Add(new SqlParameter("@CurId", SqlDbType.UniqueIdentifier)).Value = UniqueID;
			dataTable = DataDictionary.GetDataTable(sqlCommand, fillSchema: true, out var adapter);
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.AddBlankRow();
				UniqueID = Guid.NewGuid();
			}
			else
			{
				dataRow = dataTable.Rows[0];
			}
			dataRow.BeginEdit();
			dataRow.SetField("dxUser", UserID);
			dataRow.SetField("dxmode", Mode);
			dataRow.SetField("dxUniqueid", UniqueID);
			dataRow.SetField("dxtext", Caption);
			if (!string.IsNullOrWhiteSpace(ImageLarge))
			{
				dataRow.SetField("dxImageLarge", ImageLarge);
			}
			if (!string.IsNullOrWhiteSpace(ImageSmall))
			{
				dataRow.SetField("dxImageSmall", ImageSmall);
			}
			dataRow.SetField("dxextd", (Data.Length == 0) ? null : Data);
			dataRow.SetField("dxParentUniqueID", ParentUniqueID);
			dataRow.SetField("dxLinkedUniqueID", LinkedUniqueID);
			dataRow.SetField("dxtype", Type);
			dataRow.SetField("dxviewer", ViewerType);
			dataRow.SetField("dxSequence", Sequence);
			dataRow.SetField("dxDisabled", !Enabled);
			dataRow.SetField("dxGridID", GridID);
			dataRow.SetField("dxscom", SecurityComponent);
			dataRow.SetField("dxsmod", SecurityModule);
			dataRow.SetField("dxCollapse", Collapsed);
			dataRow.SetField("dxRemoved", Removed);
			dataRow.SetField("dxCustom", Custom);
			dataRow.SetField("dxVisualizerID", VisualizerID);
			dataRow.SetField("dxVisualizerType", VisualizerType);
			dataRow.EndEdit();
			DataDictionary.UpdateData(new DataRow[1] { dataRow }, adapter);
			SecurityAccessIsAvailable = checkSecurity();
			onDataChanged();
		}
	}

	public Image GetSmallImageForItem()
	{
		return GetImageForItem(largeImage: false);
	}

	public Image GetLargeImageForItem()
	{
		return GetImageForItem(largeImage: true);
	}

	public void GetUniqueID(Guid parentUniqueId)
	{
		SqlCommand sqlCommand = DataDictionary.NewSqlCommand("Select dxUniqueId From DDExplorer Where dxmode = @Mode and (dxUser = @User or User = '') and dxText = @Text And dxParentUniqueID=@ParentUniqueID");
		sqlCommand.Parameters.Add(new SqlParameter("@Mode", SqlDbType.NVarChar)).Value = Mode;
		sqlCommand.Parameters.Add(new SqlParameter("@User", SqlDbType.NVarChar)).Value = User.ID;
		sqlCommand.Parameters.Add(new SqlParameter("@Text", SqlDbType.NVarChar)).Value = Caption.Trim();
		sqlCommand.Parameters.Add(new SqlParameter("@ParentUniqueID", SqlDbType.UniqueIdentifier)).Value = parentUniqueId;
		DataTable dataTable = DataDictionary.GetDataTable(sqlCommand);
		if (dataTable.Rows.Count > 0)
		{
			UniqueID = dataTable.Rows[0].Field<Guid>("dxUniqueId");
		}
		dataTable.Clear();
	}

	public void GetUniqueID()
	{
		DataTable dataTable = DataDictionary.GetDataTable(DataDictionary.NewSqlCommand("Select dxUniqueId From DDExplorer Where dxmode = '" + Mode + "' and (dxUser ='" + User.ID + "' or dxUser ='') and dxText = '" + Caption.Trim() + "'"));
		if (dataTable.Rows.Count > 0)
		{
			UniqueID = dataTable.Rows[0].Field<Guid>("dxUniqueId");
		}
		dataTable.Clear();
	}

	private Image GetImageForItem(bool largeImage)
	{
		if (!string.IsNullOrEmpty(ImageLarge) && !string.IsNullOrEmpty(ImageSmall))
		{
			return M1.Images.Resources.GetEmbeddedImage(largeImage ? ImageLarge : ImageSmall);
		}
		switch (Type)
		{
		case ExplorerType.Dashboard:
			return M1.Images.Properties.Resources.areachart32a;
		case ExplorerType.Entry:
		case ExplorerType.Maintenance:
			if (largeImage)
			{
				return M1.Images.Properties.Resources.maintenance32;
			}
			return M1.Images.Properties.Resources.maintenance16;
		case ExplorerType.Explorer:
			if (largeImage)
			{
				return M1.Images.Properties.Resources.explorer32a;
			}
			return M1.Images.Properties.Resources.explorer16a;
		case ExplorerType.DataViewer:
			return GetImageByViewerType(ViewerType, largeImage);
		case ExplorerType.Folder:
		case ExplorerType.TopLevelFolder:
			if (largeImage)
			{
				return M1.Images.Properties.Resources.folder32a;
			}
			return M1.Images.Properties.Resources.folder16a;
		case ExplorerType.StartPage:
			if (largeImage)
			{
				return M1.Images.Properties.Resources.startPage32;
			}
			return M1.Images.Properties.Resources.startPage16;
		case ExplorerType.CustomReportsFolder:
		case ExplorerType.CustomReport:
		case ExplorerType.Report:
			if (largeImage)
			{
				return M1.Images.Properties.Resources.print32;
			}
			return M1.Images.Properties.Resources.print16a;
		case ExplorerType.CustomFormsFolder:
			if (largeImage)
			{
				return M1.Images.Properties.Resources.viewform32a;
			}
			return M1.Images.Properties.Resources.viewform16a;
		case ExplorerType.Wizard:
			if (largeImage)
			{
				return M1.Images.Properties.Resources.wizardwand32a;
			}
			return M1.Images.Properties.Resources.wizardwand16a;
		case ExplorerType.Tool:
			if (largeImage)
			{
				return M1.Images.Properties.Resources.tool32;
			}
			return M1.Images.Properties.Resources.tool16;
		case ExplorerType.Close:
			if (largeImage)
			{
				return M1.Images.Properties.Resources.tool32;
			}
			return M1.Images.Properties.Resources.tool16;
		case ExplorerType.Help:
			if (largeImage)
			{
				return M1.Images.Properties.Resources.help32;
			}
			return M1.Images.Properties.Resources.help16;
		case ExplorerType.Visualizer:
			switch (VisualizerType)
			{
			case VisualizerType.Trend:
				if (largeImage)
				{
					return M1.Images.Properties.Resources.areachart32a;
				}
				return M1.Images.Properties.Resources.areachart16a;
			case VisualizerType.Calendar:
				if (largeImage)
				{
					return M1.Images.Properties.Resources.calendar32a;
				}
				return M1.Images.Properties.Resources.calendar16a;
			case VisualizerType.GoogleMap:
				if (largeImage)
				{
					return M1.Images.Properties.Resources.mapweb32a;
				}
				return M1.Images.Properties.Resources.mapweb16a;
			case VisualizerType.MapPointMap:
				if (largeImage)
				{
					return M1.Images.Properties.Resources.mappoint32a;
				}
				return M1.Images.Properties.Resources.mappoint16a;
			case VisualizerType.PieChart:
				if (largeImage)
				{
					return M1.Images.Properties.Resources.piechart32a;
				}
				return M1.Images.Properties.Resources.piechart16a;
			default:
				if (largeImage)
				{
					return M1.Images.Properties.Resources.folder32a;
				}
				return M1.Images.Properties.Resources.folder16a;
			}
		default:
			if (largeImage)
			{
				return M1.Images.Properties.Resources.folder32a;
			}
			return M1.Images.Properties.Resources.folder16a;
		}
	}

	private Image GetImageByViewerType(DisplayType viewerType, bool largeImage)
	{
		switch (viewerType)
		{
		case DisplayType.Dashboard:
			return M1.Images.Properties.Resources.areachart32a;
		case DisplayType.PieChart:
		case DisplayType.PieChart3D:
		case DisplayType.DoughnutChart:
		case DisplayType.DoughnutChart3D:
			if (largeImage)
			{
				return M1.Images.Properties.Resources.piechart32a;
			}
			return M1.Images.Properties.Resources.piechart16a;
		case DisplayType.ColumnChart:
		case DisplayType.ColumnChart3D:
		case DisplayType.StackColumnChart:
		case DisplayType.Stack3DColumnChart:
		case DisplayType.CylinderStackColumnChart3D:
		case DisplayType.CylinderColumnChart3D:
			if (largeImage)
			{
				return M1.Images.Properties.Resources.columnchart32a;
			}
			return M1.Images.Properties.Resources.columnchart16a;
		case DisplayType.BarChart:
		case DisplayType.BarChart3D:
		case DisplayType.StackBarChart:
		case DisplayType.Stack3DBarChart:
		case DisplayType.CylinderStackBarChart3D:
		case DisplayType.CylinderBarChart3D:
			if (largeImage)
			{
				return M1.Images.Properties.Resources.barchart32a;
			}
			return M1.Images.Properties.Resources.barchart16a;
		case DisplayType.LineChart:
		case DisplayType.AreaChart3D:
		case DisplayType.LineChart3D:
		case DisplayType.SplineChart:
		case DisplayType.SplineChart3D:
		case DisplayType.SplineAreaChart3D:
			if (largeImage)
			{
				return M1.Images.Properties.Resources.areachart32a;
			}
			return M1.Images.Properties.Resources.areachart16a;
		case DisplayType.FunnelChart:
		case DisplayType.FunnelChart3D:
		case DisplayType.PyramidChart:
		case DisplayType.PyramidChart3D:
		case DisplayType.ConeChart3D:
			if (largeImage)
			{
				return M1.Images.Properties.Resources.chart_bubble32;
			}
			return M1.Images.Properties.Resources.chart_bubble16;
		case DisplayType.QuarterCalendar:
		case DisplayType.MonthCalendar:
		case DisplayType.WeekCalendar:
		case DisplayType.DayCalendar:
			if (largeImage)
			{
				return M1.Images.Properties.Resources.calendar32a;
			}
			return M1.Images.Properties.Resources.calendar16a;
		case DisplayType.GoogleMap:
			if (largeImage)
			{
				return M1.Images.Properties.Resources.mapweb32a;
			}
			return M1.Images.Properties.Resources.mapweb16a;
		case DisplayType.MapPointMap:
			if (largeImage)
			{
				return M1.Images.Properties.Resources.mappoint32a;
			}
			return M1.Images.Properties.Resources.mappoint16a;
		case DisplayType.Grid:
			if (largeImage)
			{
				return M1.Images.Properties.Resources.search32a;
			}
			return M1.Images.Properties.Resources.search16a;
		default:
			if (largeImage)
			{
				return M1.Images.Properties.Resources.search32a;
			}
			return M1.Images.Properties.Resources.search16a;
		}
	}
}
