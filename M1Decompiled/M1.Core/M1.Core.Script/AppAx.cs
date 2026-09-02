using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using M1.Script.Interfaces;

namespace M1.Core.Script;

[ComVisible(true)]
[ClassInterface(ClassInterfaceType.None)]
[ComDefaultInterface(typeof(IAx))]
public class AppAx : IDisposable, IAx
{
	private AppContext contextRef;

	private IServiceProvider _Provider;

	private Dictionary<string, object> _Functions;

	private Dictionary<string, Type> _FunctionTypes;

	[IndexerName("_Default")]
	[DispId(0)]
	public object this[string name] => getClass(name);

	public AppAx(IServiceProvider provider)
	{
		_Provider = provider;
		contextRef = provider.GetService(typeof(AppContext)) as AppContext;
	}

	private object getClass(string id)
	{
		if (_Functions == null)
		{
			_Functions = new Dictionary<string, object>(StringComparer.CurrentCultureIgnoreCase);
		}
		if (_Functions.ContainsKey(id))
		{
			return _Functions[id];
		}
		if (_FunctionTypes == null)
		{
			_FunctionTypes = new Dictionary<string, Type>(StringComparer.CurrentCultureIgnoreCase);
			foreach (AppExtension appExtension in (_Provider.GetService(typeof(M1DataDictionary)) as M1DataDictionary).AppExtensions)
			{
				Assembly codeAssembly = appExtension.GetCodeAssembly();
				if (!(codeAssembly != null))
				{
					continue;
				}
				Type[] types = codeAssembly.GetTypes();
				foreach (Type type in types)
				{
					object[] customAttributes = type.GetCustomAttributes(typeof(AxScriptAttribute), inherit: true);
					if (customAttributes.Length != 0 && customAttributes[0] is AxScriptAttribute axScriptAttribute)
					{
						_FunctionTypes.Add(axScriptAttribute.Value, type);
					}
				}
			}
		}
		if (_FunctionTypes.ContainsKey(id))
		{
			object obj = ((!(_FunctionTypes[id].GetConstructor(new Type[1] { typeof(IServiceProvider) }) != null)) ? Activator.CreateInstance(_FunctionTypes[id]) : Activator.CreateInstance(_FunctionTypes[id], _Provider));
			MethodInfo method = obj.GetType().GetMethod("SetReferences");
			if (method != null)
			{
				method.Invoke(obj, new object[2]
				{
					_Provider.GetService(typeof(ScriptApp)),
					_Provider.GetService(typeof(IForms))
				});
			}
			method = obj.GetType().GetMethod("SetPassword");
			if (method != null)
			{
				MethodInfo methodInfo = method;
				object[] parameters = new string[1] { contextRef.DBServerManager.sqlPassword };
				methodInfo.Invoke(obj, parameters);
			}
			_Functions.Add(id, obj);
			return obj;
		}
		return null;
	}

	public void Dispose()
	{
		if (_Functions != null)
		{
			foreach (KeyValuePair<string, object> function in _Functions)
			{
				if (function.Value is IDisposable)
				{
					((IDisposable)function.Value).Dispose();
				}
			}
			_Functions.Clear();
			_Functions = null;
		}
		if (_FunctionTypes != null)
		{
			_FunctionTypes.Clear();
		}
		_Provider = null;
		contextRef = null;
	}
}
