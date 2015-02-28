//C# Unity event manager that uses strings in a hashtable over delegates and events in order to
//allow use of events without knowing where and when they're declared/defined.


using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/*
 	// Include

 	
 	// Add listerner in function ""
	void OnStart()
	{
		EventMgr.instance.AddListener(this, null, EVENT_ID, OnGameEvent);
		
		// if attend to some Object, you wiOnStartll only attend to the event of the target Object, use this:
		EventMgr.instance.AddListener(this, target, EVENT_ID, OnGameEvent);
		
	}
	
	// if you had attend to some Object, you may use this in processing, use this:
	EventMgr.instance.DetachListener(this, EVENT_ID, OnGameEvent);
 
 	// Detach listerner in function "OnDestroy"
	void OnDestroy()
	{
		EventMgr.DetachAllListener(this);
	}
			
	// Define event handle function, use the defined delegate function mode
	// If event finished, return EVENT_HANDLE.END, else return EVENT_HANDLE.CONTINUE
	public EVENT_HANDLE OnGameEvent( IEvent evt )
	{
		return EVENT_HANDLE.CONTINUE;
	}
 
 	// The event trigger methods
 	// 1. triggered when called
	EventMgr.instance.SendEvent(Object sender, int eventID, object objParam1 , object objParam2 );
	// 2. triggered when next updated
	EventMgr.instance.PostEvent(Object sender, int eventID, object objParam1 , object objParam2 );
	// 3. triggered when time finish
	EventMgr.instance.PostLateEvent(Object sender, int eventID, float second, object objParam1 , object objParam2 );
	// 4. triggered when called
	EventMgr.instance.SendEventTo(Object sender, Object target, int eventID, object objParam1 , object objParam2 );
	// 5. triggered when next updated
	EventMgr.instance.PostEventTo(Object sender, Object target, int eventID, object objParam1 , object objParam2 );
	// 6. triggered when time finish
	EventMgr.instance.PostLateEventTo(Object sender, Object target, int eventID, float second, object objParam1 , object objParam2 );
	
*/

#region Define
// --------------------------------------------------------------
// Define
// --------------------------------------------------------------
public enum EVENT_HANDLE
{
	CONTINUE = 0,
	END
}

public delegate EVENT_HANDLE GameEventHandle(IEvent evt);

public struct EventHandleListenerData
{
	public object objListener;
	public object objTarget;
	public GameEventHandle eventHandle;

	public EventHandleListenerData(object objLstner, object objTrg, GameEventHandle eventHdl )
	{
		objListener = objLstner;
		objTarget = objTrg;
		eventHandle = eventHdl;
	}
}

public struct EventHandleData
{
	public int eventKey;
	public GameEventHandle eventHandle;
	
	public EventHandleData(int key, GameEventHandle handle)
	{
		eventKey = key;
		eventHandle = handle;
	}	
}
	
public interface IEvent
{
    int GetKey();
    object GetParam();
	object GetSender();
	object GetReceiver();
}


public class GameEvent : IEvent
{
	public GameEvent(object s, object r, int k, object p)
    {
		sender = s;
		receiver = r;
        key = k;
        param = p;
    }
	
	private object sender;
	private object receiver;
    private int key;
    private object param;

    int IEvent.GetKey()
    {
        return key;
    }

    object IEvent.GetParam()
    {
        return param;
    }
	
	object IEvent.GetSender()
	{
		return sender;
	}
	
	object IEvent.GetReceiver()
	{
		return receiver;
	}
}

public struct LateEvent
{
	public float startTime;
	public float lateTime;
	public IEvent lateEvent;
	
	public LateEvent(IEvent evt, float second)
	{
		startTime = Time.realtimeSinceStartup;
		lateTime = second;
		lateEvent = evt;
	}
}

#endregion


public class EventMgr : MonoBehaviour
{
	#region Parameters
	// --------------------------------------------------------------
	// Parameters
	// --------------------------------------------------------------
    public bool LimitQueueProcesing = true;
    public float QueueProcessTime = 0.0f;
	
	private Hashtable m_listenerTable = new Hashtable();
    private Queue m_eventQueue = new Queue();
	private List<LateEvent> m_listQueueTiming = new List<LateEvent>();

    private static EventMgr s_Instance = null;
	
	private Hashtable m_tableEventListenerHandle = new Hashtable();
	private ArrayList lstEventHandleCalled = new ArrayList();
	
	#endregion
	
	
	#region SystemFunction
	// --------------------------------------------------------------
	// SystemFunction
	// --------------------------------------------------------------
    public static EventMgr instance
    {
        get{  return s_Instance;}
    }

    public static bool IsNull()
    {
        return (s_Instance == null);
    }

    void Awake()
    {
        s_Instance = this;
    }
	
	
	void OnDestroy()
	{
		OnFinalize();
	}
	
	public void OnApplicationQuit()
    {
		OnFinalize();
	}
	
	
	//Every update cycle the queue is processed, if the queue processing is limited,
    //a maximum processing time per update can be set after which the events will have
    //to be processed next update loop.
    void Update()
    {
		float timer = 0.0f;
        while (m_eventQueue.Count > 0)
        {
            if (LimitQueueProcesing)
            {
                if (timer > QueueProcessTime)
                    return;
            }

            IEvent evt = m_eventQueue.Dequeue() as IEvent;
            if (!SendEventTo(evt.GetSender(), evt.GetReceiver(), evt.GetKey(), evt.GetParam()))
                LogMgr.instance.DebugLog("Error when processing event: " + evt.GetKey().ToString());

            if (LimitQueueProcesing)
                timer += Time.deltaTime;
        }
		
		if (m_listQueueTiming.Count > 0)
		{
			float curTickSinceStart = Time.realtimeSinceStartup;
			List<int> indexDel = new List<int>();
			for (int i = 0; i < m_listQueueTiming.Count; i++)
			{
				LateEvent lateEvent = m_listQueueTiming[i];
				if (curTickSinceStart - lateEvent.startTime >= lateEvent.lateTime)
				{
					if (!SendEventTo(lateEvent.lateEvent.GetSender(), lateEvent.lateEvent.GetReceiver(), lateEvent.lateEvent.GetKey(), lateEvent.lateEvent.GetParam()))
	            	    LogMgr.instance.DebugLog("Error when processing late event: " + lateEvent.lateEvent.GetKey().ToString());
					
					indexDel.Add(i);
				}
			}
			
			if (indexDel.Count > 0)
			{
				for (int i = indexDel.Count - 1; i >= 0; i--)
				{
					m_listQueueTiming.RemoveAt(indexDel[i]);
				}
			}
		}
    }
	
	#endregion
	
	
	#region CustomFunction
	// --------------------------------------------------------------
	// CustomFunction
	// --------------------------------------------------------------
    //Add a listener to the event manager that will receive any events of the supplied event key.
	public bool AttachListener(object listener, object attendedTarget, int eventkey, GameEventHandle eventHandle)
    {
        if (listener == null)
        {
            LogMgr.instance.DebugLog("Event Manager: AddListener failed due to no listener or event key specified.");
            return false;
        }

        if (!m_listenerTable.ContainsKey(eventkey))
            m_listenerTable.Add(eventkey, new ArrayList());

        ArrayList listenerList = m_listenerTable[eventkey] as ArrayList;		
		foreach (EventHandleListenerData data in listenerList)
		{
			if (data.eventHandle != null && data.eventHandle == eventHandle)
			{
				LogMgr.instance.DebugLog("Event Manager: Listener: " + listener.GetType().ToString() + " is already in list for event: " + eventkey.ToString());
            	return false; //listener already in list
			}
		}
		listenerList.Add(new EventHandleListenerData(listener, attendedTarget, eventHandle));
		
		if (!m_tableEventListenerHandle.ContainsKey(listener))
			m_tableEventListenerHandle.Add(listener, new ArrayList());
		
		ArrayList handleList = m_tableEventListenerHandle[listener] as ArrayList;
		foreach (EventHandleData data in handleList)
		{
			if (data.eventHandle == eventHandle)
			{
				LogMgr.instance.DebugLog("Event Manager: Listener: " + listener.GetType().ToString() + " is already in handle list for event: " + eventkey.ToString());
           	 	return false; //listener already in list
			}
		}
		handleList.Add(new EventHandleData(eventkey, eventHandle));

        return true;
    }

    //Remove a listener from the subscribed to event.
    public bool DetachListener(Object listener, int eventKey, GameEventHandle eventHandle)
    {
		bool bDetached = false;
        if (!m_listenerTable.ContainsKey(eventKey))
            return false;
		
		ArrayList listenerList = m_listenerTable[eventKey] as ArrayList;	
		foreach (EventHandleListenerData listenerData in listenerList)
		{
			if (listenerData.eventHandle == eventHandle)
			{
				listenerList.Remove(listenerData);
				bDetached = true;
				break;
			}
		}
       		
		if (!m_tableEventListenerHandle.ContainsKey(listener))
			return false;
		
		ArrayList handleList = m_tableEventListenerHandle[listener] as ArrayList;
		foreach (EventHandleData data in handleList)
		{
			if (data.eventHandle == eventHandle)
			{
				handleList.Remove(data);
				bDetached = true;
				break;
			}
		}
		
        return bDetached;
    }

    //Trigger the event instantly, this should only be used in specific circumstances,
    //the QueueEvent function is usually fast enough for the vast majority of uses.
	public bool SendEvent(object sender, int eventID, object objParam)
    {
		return SendEventTo(sender, null, eventID, objParam);
    }
	
	public bool SendEventTo(object sender, object target, int eventID, object objParam)
    {
		IEvent evt = new GameEvent(sender, target, eventID, objParam);
        int eventKey = evt.GetKey();
        if (!m_listenerTable.ContainsKey(eventKey))
        {
            //LogMgr.instance.DebugLog("Event Manager: Event \"" + eventKey.ToString() + "\" triggered has no listeners!");
            return false; //No listeners for event so ignore it
        }

        ArrayList listenerList = m_listenerTable[eventKey] as ArrayList;
        // LogMgr.instance.DebugLog("Event Manager: Event \"" + eventKey.ToString() + "\" triggered has " + listenerList.Count.ToString() + " listeners!");
  
		// For list changed when event handle called.
		lstEventHandleCalled.Clear();
		foreach (EventHandleListenerData data in listenerList)
        {
			lstEventHandleCalled.Add(data);
        }
		
		foreach (EventHandleListenerData data in lstEventHandleCalled)
        {
			if ((data.objTarget == null || data.objTarget == sender)
			    && (target == null || target == data.objListener)
            	&& data.eventHandle(evt) == EVENT_HANDLE.END)
			{
                return true; //Event consumed.
			}
        }

        return true;
    }

    //Inserts the event into the current queue.
	public bool PostEvent(object sender, int eventID)
	{
		return PostEventTo(sender, null, eventID, null);
	}
	public bool PostEvent(object sender, int eventID, object objParam)
	{
		return PostEventTo(sender, null, eventID, objParam);
	}
	public bool PostEventTo(object sender, object target, int eventID)
	{
		return PostEventTo(sender, target, eventID, null);
	}
	public bool PostEventTo(object sender, object target, int eventID, object objParam)
    {
		IEvent evt = new GameEvent(sender, target, eventID, objParam);
        if (!m_listenerTable.ContainsKey(evt.GetKey()))
        {
            //LogMgr.instance.DebugLog("EventMgr: QueueEvent failed due to no listeners for event: " + evt.GetKey().ToString());
            return false;
        }

        m_eventQueue.Enqueue(evt);
        return true;
    }
	
	public bool PostLateEvent(object sender, int eventID, float second, object objParam)
	{
		return PostLateEventTo(sender, null, eventID, second, objParam);
	}
	
	public bool PostLateEventTo(object sender, object target, int eventID, float second, object objParam)
	{
		IEvent evt = new GameEvent(sender, target, eventID, objParam);
		if (!m_listenerTable.ContainsKey(evt.GetKey()))
		{
			LogMgr.instance.DebugLog("EventMgr: QueueEvent failed due to no listeners for late event : " + evt.GetKey().ToString());
			return false;
		}
		
		m_listQueueTiming.Add(new LateEvent(evt, second));
		return true;
	}
	
	static public bool DetachAllListener(object obj)
	{
		if (!IsNull())
		{
			return EventMgr.instance.DetachListener(obj);
		}
		return false;
	}
	
	// Call when object destroyed
	private bool DetachListener(object obj)
	{
		if (!m_tableEventListenerHandle.ContainsKey(obj))
		{
			return false;
		}
		
		ArrayList listListenerHandle = m_tableEventListenerHandle[obj] as ArrayList;
		foreach (EventHandleData data in listListenerHandle)
		{
			if (m_listenerTable.ContainsKey(data.eventKey))
			{		
				ArrayList listenerList = m_listenerTable[data.eventKey] as ArrayList;
				foreach (EventHandleListenerData listenerData in listenerList)
				{
					if (listenerData.eventHandle == data.eventHandle)
					{
						listenerList.Remove(listenerData);
						break;
					}
				}
			}
		}
		m_tableEventListenerHandle.Remove(obj);		
		return true;
	}
	
	void OnFinalize()
	{
		m_listenerTable.Clear();
        m_eventQueue.Clear();
		m_listQueueTiming.Clear();
		m_tableEventListenerHandle.Clear();
        s_Instance = null;
	}
	#endregion
}