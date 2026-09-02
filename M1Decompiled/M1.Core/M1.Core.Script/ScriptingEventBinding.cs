using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;
using M1.Extensions;

namespace M1.Core.Script;

public class ScriptingEventBinding(IServiceProvider provider) : ScriptingBase(provider)
{
	protected Dictionary<string, VBEventHandlerInfo> EventBindings = new Dictionary<string, VBEventHandlerInfo>(StringComparer.CurrentCultureIgnoreCase);

	private readonly Dictionary<string, TimerDelegate> _timerDelegates = new Dictionary<string, TimerDelegate>();

	public override void Dispose()
	{
		if (_timerDelegates != null)
		{
			foreach (KeyValuePair<string, TimerDelegate> timerDelegate in _timerDelegates)
			{
				EventInfo eventInfo = timerDelegate.Value.EventInfo;
				object component = timerDelegate.Value.Component;
				Delegate handlerDelegate = timerDelegate.Value.HandlerDelegate;
				eventInfo.RemoveEventHandler(component, handlerDelegate);
			}
		}
		if (EventBindings != null)
		{
			foreach (KeyValuePair<string, VBEventHandlerInfo> eventBinding in EventBindings)
			{
				eventBinding.Value.Dispose();
			}
			EventBindings.Clear();
			EventBindings = null;
		}
		base.Dispose();
	}

	public void BindCodeEvents(M1DataDictionary dataDictionary, string sourceTable, Guid sourceUniqueID, string containerName, object containerRef, IScriptContainsRef containedItems, ReferencedFieldsList referencedFields)
	{
		SqlCommand sqlCommand = dataDictionary.NewSqlCommand("Select dkAppExtensionID,dkCode From DDCode Where dkSourceUniqueID = @UniqueID And dkSourceTable = @SourceTable");
		sqlCommand.Parameters.Add(new SqlParameter("@UniqueID", SqlDbType.UniqueIdentifier)).Value = sourceUniqueID;
		sqlCommand.Parameters.Add(new SqlParameter("@SourceTable", SqlDbType.NVarChar)).Value = sourceTable;
		BindCodeEvents(dataDictionary.GetDataTable(sqlCommand), containerName, containerRef, containedItems, referencedFields);
	}

	public void BindCodeEvents(DataTable codeData, string containerName, object containerRef, IScriptContainsRef containedItems, ReferencedFieldsList referencedFields)
	{
		if (codeData.Rows.Count == 0)
		{
			return;
		}
		StringBuilder stringBuilder = new StringBuilder();
		int num = 0;
		foreach (DataRow row in codeData.Rows)
		{
			if (row["dkCode"] == DBNull.Value)
			{
				continue;
			}
			string text = row.Field<string>("dkCode");
			if (!string.IsNullOrWhiteSpace(text))
			{
				referencedFields?.ParseCodeForFields(text);
				string text2 = row.Field<string>("dkAppExtensionID");
				string text3;
				string text4;
				if (string.IsNullOrWhiteSpace(text2))
				{
					text3 = "Class" + num;
					text4 = "var" + text3;
				}
				else
				{
					text3 = "Class" + text2;
					text4 = text2;
				}
				stringBuilder.AppendLine("Class " + text3);
				stringBuilder.AppendLine(text);
				stringBuilder.AppendLine("End Class");
				stringBuilder.AppendLine("Dim " + text4);
				stringBuilder.AppendLine("Set " + text4 + " = New " + text3);
				num++;
				ProcessSingleClassBindings(text, row.Field<string>("dkAppExtensionID"), text4, containerName, containerRef, containedItems);
			}
		}
		if (stringBuilder != null && stringBuilder.Length != 0)
		{
			try
			{
				AddCode(stringBuilder.ToString());
			}
			catch (Exception ex)
			{
				throw new M1Exception("Exception '" + ex.Message + "' while adding code for " + containerName + ".", ex);
			}
		}
	}

	private void ProcessSingleClassBindings(string code, string appExtID, string varName, string containerName, object containerRef, IScriptContainsRef containedItems)
	{
		string[] array = code.Replace("\r\n", "\r").Split('\r');
		if (array == null || array.Length == 0)
		{
			return;
		}
		string methodType = string.Empty;
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < array.Length; i++)
		{
			string methodNameInText = M1Util.GetMethodNameInText(array[i], ref methodType);
			if (methodNameInText.Length == 0)
			{
				continue;
			}
			int num = methodNameInText.LastIndexOf('_');
			if (num == -1)
			{
				continue;
			}
			string text = methodNameInText.Substring(0, num);
			string text2 = methodNameInText.Substring(num + 1);
			num = text.IndexOf(' ');
			if (num != -1)
			{
				text = text.Substring(num + 1).TrimStart();
			}
			if (text.Length == 0 || text2.Length == 0)
			{
				continue;
			}
			object obj = null;
			obj = ((!containerName.Equals(text, StringComparison.CurrentCultureIgnoreCase) && !text.Equals("this", StringComparison.CurrentCultureIgnoreCase)) ? containedItems.ContainsRef(text) : (containerRef ?? containedItems.ContainsRef("")));
			if (obj == null)
			{
				continue;
			}
			EventInfo eventInfo = obj.GetType().GetEvent(text2, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);
			if (!(eventInfo != null))
			{
				continue;
			}
			if (obj is IProcessCodeBindings)
			{
				object[] customAttributes = eventInfo.GetCustomAttributes(typeof(ProcessCodeBindingsAttribute), inherit: true);
				if (customAttributes != null && customAttributes.Length != 0 && ((ProcessCodeBindingsAttribute)customAttributes[0]).Value)
				{
					string value = "End " + methodType;
					stringBuilder.Length = 0;
					for (int j = i + 1; j < array.Length; j++)
					{
						if (array[j].IndexOf(value, StringComparison.CurrentCultureIgnoreCase) != -1 && array[j].TrimStart(' ', '\t').StartsWith(value, StringComparison.CurrentCultureIgnoreCase))
						{
							i = j + 1;
							break;
						}
						stringBuilder.AppendLine(array[j]);
					}
					((IProcessCodeBindings)obj).ProcessCodeBindings(text2, stringBuilder);
				}
			}
			VBEventHandlerInfo vBEventHandlerInfo;
			if (EventBindings.ContainsKey(text + "_" + text2))
			{
				vBEventHandlerInfo = EventBindings[text + "_" + text2];
			}
			else
			{
				vBEventHandlerInfo = new VBEventHandlerInfo(this);
				EventBindings.Add(text + "_" + text2, vBEventHandlerInfo);
				MethodInfo method = vBEventHandlerInfo.GetType().GetMethod("InfoHandlerDelegate");
				Delegate obj2 = Delegate.CreateDelegate(eventInfo.EventHandlerType, vBEventHandlerInfo, method);
				MethodInfo addMethod = eventInfo.GetAddMethod();
				if (string.Equals(text2, "Tick", StringComparison.CurrentCultureIgnoreCase))
				{
					SaveTimerDelegate(text, obj, obj2, eventInfo);
				}
				addMethod.Invoke(obj, new object[1] { obj2 });
			}
			vBEventHandlerInfo.AddMethodToRun(varName + "." + text + "_" + text2, appExtID);
		}
	}

	private void SaveTimerDelegate(string objectName, object component, Delegate handlerDelegate, EventInfo eventInfo)
	{
		TimerDelegate value = new TimerDelegate
		{
			EventInfo = eventInfo,
			Component = component,
			HandlerDelegate = handlerDelegate
		};
		_timerDelegates.Add(objectName, value);
	}
}
