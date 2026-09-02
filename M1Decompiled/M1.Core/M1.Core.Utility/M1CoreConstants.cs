using System.Collections.Generic;
using Newtonsoft.Json;

namespace M1.Core.Utility;

public class M1CoreConstants
{
	public static readonly string DEFAULT_TABS = GetEmptyList();

	public static readonly string DEFAULT_TREES = GetEmptyList();

	public static readonly List<string> DEFAULT_FAVORITES_ITEMS = GetDefaultFavoritesItems();

	private static string GetEmptyList()
	{
		return JsonConvert.SerializeObject(new List<object>());
	}

	private static List<string> GetDefaultFavoritesItems()
	{
		return new List<string> { "b037e115-0584-43ee-a3e7-0243957d1c2b", "825a2f9d-82d5-4a97-bb00-b26e6a852106", "d5c1905d-3508-491e-ad27-c6252358fa67", "f3bd682e-0bb8-4d4c-9efc-178f49d84ec0", "7e48d34a-0a67-4522-add7-35cb594f1270", "9241bdeb-818f-4b6f-8336-1a8b517500d3", "e8fd1ce8-9b4c-4fe4-82e9-4834f72557d3", "30f85e34-f9af-495a-8c9b-217f0cb3709a" };
	}
}
