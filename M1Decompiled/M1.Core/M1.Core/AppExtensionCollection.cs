using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Text;

namespace M1.Core;

public class AppExtensionCollection : KeyedCollection<string, AppExtension>
{
	protected M1DataDictionary currentDataDictionary;

	protected AppContext context;

	protected DmoDD dmoDD;

	protected string databaseName = string.Empty;

	public event EventHandler ListRefreshed;

	public AppExtensionCollection(M1DataDictionary dataDictionary)
		: base((IEqualityComparer<string>)StringComparer.CurrentCultureIgnoreCase)
	{
		currentDataDictionary = dataDictionary;
		context = currentDataDictionary.GetService(typeof(AppContext)) as AppContext;
	}

	public AppExtensionCollection(DmoDD dmo, AppContext currentContext, string databaseName)
		: base((IEqualityComparer<string>)StringComparer.CurrentCultureIgnoreCase)
	{
		context = currentContext;
		dmoDD = dmo;
		this.databaseName = databaseName;
	}

	public Type GetTypeFromCodeAssemblies(string name)
	{
		Type type = null;
		if (name.StartsWith("M1.Core.", StringComparison.CurrentCultureIgnoreCase))
		{
			return GetType().Assembly.GetType(name);
		}
		using (IEnumerator<AppExtension> enumerator = GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				Assembly codeAssembly = enumerator.Current.GetCodeAssembly();
				if (codeAssembly != null)
				{
					type = codeAssembly.GetType(name);
					if (type != null)
					{
						return type;
					}
				}
			}
		}
		return null;
	}

	public Type GetTypeFromFormsAssemblies(string name)
	{
		Type type = null;
		using (IEnumerator<AppExtension> enumerator = GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				Assembly formsAssembly = enumerator.Current.GetFormsAssembly();
				if (formsAssembly != null)
				{
					type = formsAssembly.GetType(name);
					if (type != null)
					{
						return type;
					}
				}
			}
		}
		return null;
	}

	public List<T> GetProcessHooksForTable<T>(string table, Type attributeType)
	{
		List<T> list = new List<T>();
		using IEnumerator<AppExtension> enumerator = GetEnumerator();
		while (enumerator.MoveNext())
		{
			Assembly codeAssembly = enumerator.Current.GetCodeAssembly();
			if (!(codeAssembly != null))
			{
				continue;
			}
			Type[] types = codeAssembly.GetTypes();
			foreach (Type type in types)
			{
				if (typeof(T).IsAssignableFrom(type))
				{
					object[] customAttributes = type.GetCustomAttributes(attributeType, inherit: false);
					if (customAttributes != null && customAttributes.Length != 0 && ((ProcessingAttribute)customAttributes[0]).Table.Equals(table, StringComparison.CurrentCultureIgnoreCase))
					{
						list.Add((T)Activator.CreateInstance(type));
					}
				}
			}
		}
		return list;
	}

	protected override string GetKeyForItem(AppExtension item)
	{
		return item.AppID;
	}

	public void OnListRefreshed(EventArgs e)
	{
		this.ListRefreshed?.Invoke(this, e);
	}

	public virtual void Refresh(string extensionName)
	{
		if (Contains(extensionName))
		{
			Remove(extensionName);
		}
		SqlCommand sqlCommand = currentDataDictionary.NewSqlCommand("Select * From DDAppExtensions Where dpAppExtensionID = @AppID");
		sqlCommand.Parameters.Add(new SqlParameter("@AppID", SqlDbType.NVarChar)).Value = extensionName;
		DataTable dataTable = currentDataDictionary.GetDataTable(sqlCommand);
		if (dataTable.Rows.Count != 0)
		{
			foreach (DataRow row in dataTable.Rows)
			{
				Add(new AppExtension(row, getExtFolder()));
			}
		}
		OnListRefreshed(EventArgs.Empty);
	}

	private string getExtFolder()
	{
		return context.Client.Location + "Tools\\Assemblies\\";
	}

	public virtual void Refresh()
	{
		Clear();
		DataTable dataTable = ((currentDataDictionary != null) ? currentDataDictionary.GetDataTable("Select * From DDAppExtensions Order By dpCaption,dpAppExtensionID") : dmoDD.GetDataTable(databaseName, "Select * From DDAppExtensions Order By dpCaption,dpAppExtensionID"));
		if (dataTable.Rows.Count != 0)
		{
			foreach (DataRow row in dataTable.Rows)
			{
				Add(new AppExtension(row, getExtFolder()));
			}
		}
		OnListRefreshed(EventArgs.Empty);
	}

	public string GetVersionString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		using (IEnumerator<AppExtension> enumerator = GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				AppExtension current = enumerator.Current;
				if (current.DDAssembly.Length != 0)
				{
					stringBuilder.AppendLine(Path.GetFileName(current.DDAssembly) + "|" + current.DDAssemblyVersion);
				}
			}
		}
		return stringBuilder.ToString();
	}
}
