using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventManager
{
    public delegate void EventHandle();
	public delegate void EventHandle<T>(T value);
	public delegate void EventHandle<T1,T2>(T1 value1,T2 value2);
	public delegate void EventHandle<T1,T2,T3>(T1 value1,T2 value2,T3 value3);

	private static Dictionary<EventID,Delegate> eventHandles = new Dictionary<EventID, Delegate>();
    #region AddListener
    /// <summary>
    /// 添加无参的事件
    /// </summary>
    /// <param name="id"></param>
    /// <param name="handle"></param>
    public static void AddListener(EventID id, EventHandle handle)
	{
		OnListeningAdd(id, handle);
		eventHandles[id] = (EventHandle)eventHandles[id] + handle;
    }
	/// <summary>
	/// 添加一个参数的事件
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="id"></param>
	/// <param name="handle"></param>
	public static void AddListener<T>(EventID id, EventHandle<T> handle)
	{
		OnListeningAdd(id, handle);
		eventHandles[id] = (EventHandle<T>)eventHandles[id] + handle;
	}
	/// <summary>
	/// 添加俩个参数的事件
	/// </summary>
	/// <typeparam name="T1"></typeparam>
	/// <typeparam name="T2"></typeparam>
	/// <param name="id"></param>
	/// <param name="handle"></param>
	public static void AddListener<T1,T2>(EventID id, EventHandle<T1, T2> handle)
	{
		OnListeningAdd(id, handle);
		eventHandles[id] = (EventHandle<T1, T2>)eventHandles[id] + handle;
	}
	/// <summary>
	/// 添加三个参数的事件
	/// </summary>
	/// <typeparam name="T1"></typeparam>
	/// <typeparam name="T2"></typeparam>
	/// <typeparam name="T3"></typeparam>
	/// <param name="id"></param>
	/// <param name="handle"></param>
	public static void AddListener<T1,T2,T3>(EventID id, EventHandle<T1, T2, T3> handle)
	{
		OnListeningAdd(id, handle);
		eventHandles[id] = (EventHandle<T1, T2, T3>)eventHandles[id] + handle;
	}
	#endregion
	#region RemoveListener
	public static void RemoveListener(EventID id, EventHandle handle)
	{
		if (eventHandles == null)
		{
			return;
		}
		if (!eventHandles.ContainsKey(id)) return;
		OnListeningRemove(id, handle);
		eventHandles[id] = (EventHandle)eventHandles[id] - handle;
	}
	public static void RemoveListener<T>(EventID id, EventHandle<T> handle)
	{
		if (eventHandles == null)
		{
			return;
		}
		if (!eventHandles.ContainsKey(id)) return;
		OnListeningRemove(id, handle);
		eventHandles[id] = (EventHandle<T>)eventHandles[id] - handle;
	}
	public static void RemoveListener<T1,T2>(EventID id, EventHandle<T1, T2> handle)
	{
		if (eventHandles == null)
		{
			return;
		}
		if (!eventHandles.ContainsKey(id)) return;
		OnListeningRemove(id, handle);
		eventHandles[id] = (EventHandle<T1, T2>)eventHandles[id] - handle;
	}
	public static void RemoveListener<T1,T2,T3>(EventID id, EventHandle<T1, T2, T3> handle)
	{
		if (eventHandles == null)
		{
			return;
		}
		if (!eventHandles.ContainsKey(id)) return;
		OnListeningRemove(id, handle);
		eventHandles[id] = (EventHandle<T1, T2, T3>)eventHandles[id] - handle;
	}
	#endregion
	#region 发送事件ExecuteEvent
	public static void ExecuteEvent(EventID id)
    {
		if (eventHandles == null) return;
		Delegate d;
		if (eventHandles.TryGetValue(id, out d))
		{
			if (d == null) return;
			EventHandle call = d as EventHandle;
			if (call != null)
			{
				call();
			}else
			{
				throw new Exception(string.Format("事件{0}包含着不同类型的委托", id));
			}
		}
	}
	public static void ExecuteEvent<T>(EventID id, T value)
	{
		if (eventHandles == null) return;
		Delegate d;
		if (eventHandles.TryGetValue(id, out d))
		{
			if (d == null) return;
			EventHandle<T> call = d as EventHandle<T>;
			if (call != null)
			{
				call(value);
			}
			else
			{
				throw new Exception(string.Format("事件{0}包含着不同类型的委托{1}", id, d.GetType()));
			}
		}
	}
	public static void ExecuteEvent<T1,T2>(EventID id, T1 value1, T2 value2)
	{
		if (eventHandles == null) return;
		Delegate d;
		if (eventHandles.TryGetValue(id, out d))
		{
			if (d == null) return;
			EventHandle<T1, T2> call = d as EventHandle<T1, T2>;
			if (call != null)
			{
				call(value1, value2);
			}
			else
			{
				throw new Exception(string.Format("事件{0}包含着不同类型的委托{1}", id, d.GetType()));
			}
		}
	}
	public static void ExecuteEvent<T1,T2,T3>(EventID id, T1 value1, T2 value2, T3 value3)
	{
		if (eventHandles == null) return;
		Delegate d;
		if (eventHandles.TryGetValue(id, out d))
		{
			if (d == null) return;
			EventHandle<T1, T2, T3> call = d as EventHandle<T1, T2, T3>;
			if (call != null)
			{
				call(value1,value2,value3);
			}
			else
			{
				throw new Exception(string.Format("事件{0}包含着不同类型的委托{1}", id, d.GetType()));
			}
		}
	}
	#endregion
	#region 移除事件
	public static void RemoveEvent(EventID id,bool needRemoveDic = true)
    {
		if (eventHandles == null)
		{
			return;
		}
		if (eventHandles.ContainsKey(id))
		{
			var callback = eventHandles[id];
			Delegate[] invokeList = callback.GetInvocationList();
			foreach (var invokeItem in invokeList)
			{
				Delegate.Remove(callback, invokeItem);
			}

			if (needRemoveDic) eventHandles.Remove(id);
		}
	}
	public static void RemoveAllEvent()
	{
		if (eventHandles == null)
		{
			return;
		}
		foreach (KeyValuePair<EventID, Delegate> eventItem in eventHandles)
		{
			RemoveEvent(eventItem.Key, false);
		}
		eventHandles.Clear();
	}
	#endregion
	private static void OnListeningAdd(EventID id,Delegate callback)
    {
		if (eventHandles == null)
			eventHandles = new Dictionary<EventID, Delegate>();
		if (!eventHandles.ContainsKey(id))
		{
			eventHandles.Add(id, null);
		}
		Delegate d = eventHandles[id];
		if (d != null && d.GetType() != callback.GetType())
		{
			throw new Exception(string.Format("尝试添加两种不同类型的委托,委托1为{0}，委托2为{1}", d.GetType(), callback.GetType()));
		}
	}
	private static void OnListeningRemove(EventID id,Delegate callback)
    {
		if (eventHandles.ContainsKey(id))
		{
			Delegate d = eventHandles[id];
			if (d != null && d.GetType() != callback.GetType())
			{
				throw new Exception(string.Format("尝试移除不同类型的事件，事件名{0},已存储的委托类型{1},当前事件委托{2}", id, d.GetType(), callback.GetType()));
			}
		}
		else
		{
			throw new Exception(string.Format("没有事件名{0}", id));
		}
	}
}
