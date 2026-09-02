using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using M1.Extensions;

namespace M1.Core;

public class ExplorerItemCollection : BindingList<ExplorerItem>
{
	private Guid? _CurrentParentUniqueID;

	public string Mode = string.Empty;

	private bool itemsLoaded;

	public M1User User;

	public M1Database Database;

	public M1DataDictionary DataDictionary;

	public AppContext Context;

	public Guid? CurrentParentUniqueID
	{
		get
		{
			return _CurrentParentUniqueID;
		}
		set
		{
			_CurrentParentUniqueID = value;
			OnCurrentParentUniqueIDChanged(EventArgs.Empty);
		}
	}

	public bool ItemsLoaded
	{
		get
		{
			return itemsLoaded;
		}
		private set
		{
			itemsLoaded = value;
		}
	}

	public event EventHandler CurrentParentUniqueIDChanged;

	public ExplorerItemCollection(M1User m1User, M1Database database, M1DataDictionary dataDictionary, AppContext context, string mode)
	{
		User = m1User;
		DataDictionary = dataDictionary;
		Database = database;
		Context = context;
		Mode = mode;
	}

	public new void OnListChanged(ListChangedEventArgs e)
	{
		base.OnListChanged(e);
	}

	public void OnCurrentParentUniqueIDChanged(EventArgs e)
	{
		this.CurrentParentUniqueIDChanged?.Invoke(this, e);
	}

	public bool Contains(Guid id)
	{
		foreach (ExplorerItem item in base.Items)
		{
			if (item.UniqueID == id)
			{
				return true;
			}
		}
		return false;
	}

	public ExplorerItem GetItemByID(Guid id)
	{
		foreach (ExplorerItem item in base.Items)
		{
			if (item.UniqueID == id)
			{
				return item;
			}
		}
		return null;
	}

	public int GetIndexByID(Guid id)
	{
		foreach (ExplorerItem item in base.Items)
		{
			if (item.UniqueID == id)
			{
				return IndexOf(item);
			}
		}
		return -1;
	}

	public void CopyItem(ExplorerItem curItem, Guid? parentUniqueID, string mode)
	{
		ExplorerItem explorerItem = new ExplorerItem(User, Database, DataDictionary);
		explorerItem.LoadFromExistingItem(curItem);
		explorerItem.Mode = mode;
		explorerItem.UserID = User.ID;
		explorerItem.ParentUniqueID = parentUniqueID;
		explorerItem.SaveItemToDDExplorer();
		Add(explorerItem);
	}

	protected override void ClearItems()
	{
		itemsLoaded = false;
		base.ClearItems();
	}

	public void LoadCustomForms()
	{
		foreach (DataRow row2 in DataDictionary.GetDataTable("select dmFormID," + DataDictionary.Language.GetdmCaptionField(Database) + ",dlObjectID from DDForms " + DataDictionary.Language.GetdmCaptionJoin(Database) + " left outer join DDObjectDetails on dmFormID = dlView where dmCustom <> 0 and dmTable = '' order by dmCaption").Rows)
		{
			if (row2["dlObjectID"] == DBNull.Value)
			{
				ExplorerItem explorerItem = new ExplorerItem
				{
					UniqueID = Guid.NewGuid(),
					Caption = row2.Field<string>("dmCaption").Trim(),
					Type = ExplorerType.Tool,
					IsCustomForm = true,
					User = User,
					DataDictionary = DataDictionary,
					Database = Database,
					Data = "Forms.Show.CustomForm " + row2.Field<string>("dmFormID").Trim().ToScript()
				};
				explorerItem.LoadComplete();
				Add(explorerItem);
			}
		}
		foreach (DataRow row3 in DataDictionary.GetDataTable("select doObjectID," + DataDictionary.Language.GetdoTitleField(Database) + " from DDObjects " + DataDictionary.Language.GetdoTitleJoin(Database) + " where doCustom <> 0 order by doTitle").Rows)
		{
			ExplorerItem explorerItem = new ExplorerItem
			{
				UniqueID = Guid.NewGuid(),
				Caption = row3.Field<string>("doTitle").Trim(),
				Type = ExplorerType.Entry,
				IsCustomForm = true,
				User = User,
				DataDictionary = DataDictionary,
				Database = Database,
				Data = "Forms.OpenObject " + row3.Field<string>("doObjectID").Trim().ToScript()
			};
			explorerItem.LoadComplete();
			Add(explorerItem);
		}
	}

	public void LoadCustomReports()
	{
		Clear();
		itemsLoaded = true;
		if (!Directory.Exists(Context.Reports.Location))
		{
			return;
		}
		loadReportsForPrefix("CR_", null);
		foreach (ExplorerItem item in Database.ExplorerItems.Where((ExplorerItem row) => row.Type == ExplorerType.CustomReport))
		{
			loadReportsForPrefix(item.Data, item.UniqueID);
		}
	}

	private void loadReportsForPrefix(string prefixToCheck, Guid? parentUniqueID)
	{
		int num = 0;
		int num2 = 0;
		DirectoryInfo[] directories = new DirectoryInfo(Context.Reports.Location).GetDirectories(prefixToCheck + "*.*");
		foreach (DirectoryInfo directoryInfo in directories)
		{
			num++;
			ExplorerItem explorerItem = new ExplorerItem(User, Database, DataDictionary);
			explorerItem.Data = "Forms.Report.Run " + directoryInfo.Name.ToScript();
			explorerItem.UniqueID = Guid.NewGuid();
			explorerItem.ParentUniqueID = parentUniqueID;
			explorerItem.Type = ExplorerType.Report;
			explorerItem.IsCustomReport = true;
			explorerItem.Sequence = num;
			explorerItem.Caption = Context.Reports.FormatReportName(directoryInfo.Name, prefixToCheck);
			explorerItem.LoadComplete();
			Add(explorerItem);
			List<FileInfo> reportsForTemplate = Context.Reports.GetReportsForTemplate(directoryInfo.Name, string.Empty);
			num2 = 0;
			foreach (FileInfo item in reportsForTemplate)
			{
				num2++;
				ExplorerItem explorerItem2 = new ExplorerItem(User, Database, DataDictionary);
				explorerItem2.Data = "Forms.Report.Run " + (directoryInfo.Name + "\\" + Path.GetFileNameWithoutExtension(item.Name)).ToScript();
				explorerItem2.Caption = Context.Reports.FormatReportName(item.Name, prefixToCheck);
				explorerItem2.Type = ExplorerType.Report;
				explorerItem2.IsCustomReport = true;
				explorerItem2.UniqueID = Guid.NewGuid();
				explorerItem2.Sequence = num2;
				explorerItem2.ParentUniqueID = explorerItem.UniqueID;
				explorerItem2.LoadComplete();
				Add(explorerItem2);
			}
		}
	}

	public void LoadItems()
	{
		Clear();
		itemsLoaded = true;
		SqlCommand sqlCommand = DataDictionary.NewSqlCommand("select dxUser,dxMode,dxUniqueID," + DataDictionary.Language.GetdxTextField(Database) + ",dxParentUniqueID,dxLinkedUniqueID,dxType,dxViewer,Convert(nvarchar(max),IsNull(dxextd,'')) As dxextd,dxGridID,dxVisualizerID,dxVisualizerType,dxSMod,dxSCom,dxImageLarge,dxImageSmall,dxSequence,dxDisabled,dxCollapse,dxRemoved,dxCustom, IsNull(djTable,'') As GridTable  from DDExplorer " + DataDictionary.Language.GetdxTextJoin(Database) + " left outer join DDGrids On DDExplorer.dxGridID <> '' And DDExplorer.dxGridID = djGridID  where (dxuser = '' and dxmode = @Mode And dxMode = 'TREE' And dxUniqueID Not In (Select dxLinkedUniqueID From DDExplorer Where dxUser = @User And dxMode = @Mode And Not dxLinkedUniqueID Is Null))  Union All select sub.dxUser,sub.dxMode,sub.dxUniqueID," + DataDictionary.Language.GetdxTextField(Database, "orig") + ",orig.dxParentUniqueID,sub.dxLinkedUniqueID,orig.dxType,orig.dxViewer,Convert(nvarchar(max),IsNull(orig.dxextd,'')) As dxextd,orig.dxGridID,orig.dxVisualizerID,orig.dxVisualizerType,orig.dxSMod,orig.dxSCom,orig.dxImageLarge,orig.dxImageSmall,sub.dxSequence,orig.dxDisabled,sub.dxCollapse,sub.dxRemoved,sub.dxCustom, IsNull(djTable,'') As GridTable  from DDExplorer sub Inner Join DDExplorer orig On sub.dxMode = orig.dxMode And sub.dxLinkedUniqueID = orig.dxUniqueID and orig.dxUser = '' " + DataDictionary.Language.GetdxTextJoin(Database, "orig") + " left outer join DDGrids On orig.dxGridID <> '' And orig.dxGridID = djGridID  where (sub.dxuser = @User and sub.dxmode = @Mode and Not sub.dxLinkedUniqueID Is Null)  Union All select dxUser,dxMode,dxUniqueID,dxText,dxParentUniqueID,dxLinkedUniqueID,dxType,dxViewer,Convert(nvarchar(max),IsNull(dxextd,'')) As dxextd,dxGridID,dxVisualizerID,dxVisualizerType,dxSMod,dxSCom,dxImageLarge,dxImageSmall,dxSequence,dxDisabled,dxCollapse,dxRemoved,dxCustom, IsNull(djTable,'') As GridTable  from DDExplorer  left outer join DDGrids On DDExplorer.dxGridID <> '' And DDExplorer.dxGridID = djGridID  where (dxuser = @User and dxmode = @Mode and dxLinkedUniqueID Is Null)  order by dxSequence");
		sqlCommand.Parameters.Add(new SqlParameter("@Mode", SqlDbType.NVarChar)).Value = Mode;
		sqlCommand.Parameters.Add(new SqlParameter("@User", SqlDbType.NVarChar)).Value = User.ID;
		foreach (DataRow row in DataDictionary.GetDataTable(sqlCommand).Rows)
		{
			Add(new ExplorerItem(User, Database, DataDictionary, row));
		}
		if (!Mode.Equals("TREE"))
		{
			return;
		}
		foreach (ExplorerItem item in this.Where((ExplorerItem row) => !row.ParentUniqueID.HasValue && row.Type != ExplorerType.MyFolders))
		{
			item.CheckSecurityForFolder(this);
		}
	}
}
