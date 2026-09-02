using System.Collections.Concurrent;
using WebApiContrib.Caching;

namespace M1.API.Utilities;

public class WebAPIThrottleStore : IThrottleStore
{
	private readonly ConcurrentDictionary<string, ThrottleEntry> webAPIThrottleStore = new ConcurrentDictionary<string, ThrottleEntry>();

	public void Clear()
	{
		webAPIThrottleStore.Clear();
	}

	public void IncrementRequests(string key)
	{
		webAPIThrottleStore.AddOrUpdate(key, (string k) => new ThrottleEntry
		{
			Requests = 1L
		}, delegate(string k, ThrottleEntry e)
		{
			e.Requests++;
			return e;
		});
	}

	public void Rollover(string key)
	{
		webAPIThrottleStore.TryRemove(key, out var _);
	}

	public bool TryGetValue(string key, out ThrottleEntry entry)
	{
		return webAPIThrottleStore.TryGetValue(key, out entry);
	}
}
