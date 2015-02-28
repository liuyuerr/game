using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Threading;

public class ResourceMgr : MonoBehaviour
{
    #region Instance
    // --------------------------------------------------------------
    // Instance
    // --------------------------------------------------------------
    private static ResourceMgr s_Instance = null;

    public static ResourceMgr instance
    {
        get { return s_Instance; }
    }

    public static bool IsNull()
    {
        return (s_Instance == null);
    }

    void Awake()
    {
        s_Instance = this;
    }

    #endregion


    #region Parameter
    // --------------------------------------------------------------
    // Parameter
    // --------------------------------------------------------------
    //LoadingThread loadThread = null;
    //Thread threadLoading = null;
    bool bRunLoadingCoroutine = true;
    public Queue queueRequest = new Queue();
    public Dictionary<int, ResData> dicResData = new Dictionary<int, ResData>();
    const int RES_COROUTINE_LOAD_LIMIT = 5;		//max load Num
    const int RESYCLE_UNUSE_TICK = 10;

    public enum RESTYPE
    {
        ICON = 0,
        MAP,
        MODEL,
        SOUND,
        EFFECT,
        FOOT,
        MEDIUM,
        ANIMATION,
        OTHER,
        MAX
    }

    public struct ResData
    {
        public int resIntegrationID;
        public float loadedTick;
        public int refCount;
        public UnityEngine.Object resObject;

        public ResData(int resIntID, UnityEngine.Object obj)
        {
            resIntegrationID = resIntID;
            loadedTick = Time.realtimeSinceStartup;
            refCount = 0;
            resObject = obj;
        }

        public int AddRef()
        {
            return ++refCount;
        }

        public int Release()
        {
            if (refCount > 0)
            {
                if (--refCount == 0 && resObject != null)
                {
                    GameObject.Destroy(resObject);
                    resObject = null;
                }
            }

            return refCount;
        }

        public int GetRef()
        {
            return refCount;
        }
    }

    const float RESYCLE_TIME = 1.0f;
    float curResycleTime = 0;

    #endregion


    #region SystemFunction
    // --------------------------------------------------------------
    // System function
    // --------------------------------------------------------------

    void Start()
    {
        //loadThread = new LoadingThread();
        //threadLoading = new Thread(new ThreadStart(loadThread.RunThread));
        //threadLoading.Priority = System.Threading.ThreadPriority.BelowNormal;
        //threadLoading.Start();

        StartCoroutine("LoadingCoroutine");
    }

    void OnDestroy()
    {
        //loadThread.Terminate();
        //if (threadLoading.IsAlive)
        //{
        //	threadLoading.Abort();
        //}

        bRunLoadingCoroutine = false;
    }

    bool IsRunLoadingCoroutine()
    {
        return bRunLoadingCoroutine;
    }

    void FixedUpdate()
    {
        curResycleTime += Time.deltaTime;
        if (curResycleTime >= RESYCLE_TIME)
        {
            curResycleTime = 0;
            CheckResycle();
        }
    }

    #endregion


    #region CustomFunction
    // --------------------------------------------------------------
    // Custom function
    // --------------------------------------------------------------

    public string GetResIconName(int iconID)
    {
        return "";
    }

    public int GetSoldierTableIDByHero(int heroTableID)
    {
        return heroTableID;
    }

    public UnityEngine.Object LoadResource(string strPath)
    {
        return Resources.Load(strPath);
    }

    private static GameObject load(string strResource)
    {
        UnityEngine.Object original = Resources.Load(strResource);
        if (original == null)
        {
            Debug.LogError("load resource failed: " + strResource);
            return null;
        }
        return (GameObject)UnityEngine.Object.Instantiate(original);
    }

    public static Texture2D LoadPicResource(string name)
    {
        Texture2D pic = Resources.Load("Images/" + name, typeof(Texture2D)) as Texture2D; ;
        return pic;
    }

    private UnityEngine.Object LoadDefaultResource(RESTYPE type, int ResID)
    {
        UnityEngine.Object resObject = null;

        switch (type)
        {
            case RESTYPE.ICON:
                resObject = Resources.Load("Icon/Icon_default");
                break;

            case RESTYPE.MAP:
                break;

            case RESTYPE.MODEL:
                resObject = Resources.Load("Prefabs/Actor/10000");
                break;

            case RESTYPE.SOUND:
                break;

            case RESTYPE.EFFECT:
                break;

            case RESTYPE.FOOT:
                break;

            case RESTYPE.OTHER:
                break;
        }

        return resObject;
    }

    public string GetResPath(RESTYPE type, int ResID)
    {
        string strPath = "";

        switch (type)
        {
            case RESTYPE.ICON:
                strPath = "Icon/" + ResID.ToString();
                break;

            case RESTYPE.MAP:
                strPath = string.Format("Assets/Zone/Zone{0}/Prefabs/Zone{0}", ResID);
                break;

			case RESTYPE.MODEL:
				//strPath = string.Format ("Assets/Models/temp/{0}", ResID);
				strPath = string.Format ("Assets/Models/{0}/Prefabs/{0}", ResID);
                break;

            case RESTYPE.SOUND:
                strPath = "Sounds/" + ResID.ToString();
                break;

            case RESTYPE.EFFECT:
                strPath = "Effect/" + ResID.ToString();
                break;

            case RESTYPE.FOOT:
                strPath = "Prefabs/Actor/foot/" + ResID.ToString();
                break;

            case RESTYPE.MEDIUM:            // todo for test
                strPath = "Assets/Effect/Prefabs/10013_dx";
                break;

            case RESTYPE.ANIMATION:
				strPath = "Assets/Models/Animation" + "/" + ResID + ".anim";
                break;

            case RESTYPE.OTHER:
                strPath = "Other/" + ResID.ToString();
                break;
        }

        return strPath;
    }

    public int GetResIntegrationID(RESTYPE type, int ResID)
    {
        return (int)type * 100000000 + ResID;
    }

    public RESTYPE GetResType(int integrationResID)
    {
        return (RESTYPE)(integrationResID / 100000000);
    }

    public int GetResID(int integrationResID)
    {
        return integrationResID % 100000000;
    }

    // 异步加载
    public bool AsyncLoad(int integrationResID)
    {
        if (IsAvailable(integrationResID))
        {
            LogMgr.instance.DebugLog("AsyncLoad request, the Object already available : " + integrationResID.ToString());
        }
        else if (!queueRequest.Contains(integrationResID))
        {
            queueRequest.Enqueue(integrationResID);
            return true;
        }
        else
        {
            LogMgr.instance.DebugLog("AsyncLoad request already exist : " + integrationResID.ToString());
        }
        return false;
    }

    // 同步加载 
    public ResData SyncLoad(int integrationResID)
    {
        //资源层的加载
        if (IsAvailable(integrationResID))
        {
            return dicResData[integrationResID];
        }

        RESTYPE type = GetResType(integrationResID);
        int ResID = GetResID(integrationResID);
        string path = GetResPath(type, ResID);
        UnityEngine.Object resObject = LoadResource(path) as UnityEngine.Object;

        // Load default resource
        if (resObject == null)
        {
            resObject = LoadDefaultResource(type, ResID);
        }

        // Add to resource data list
        ResData data = new ResData(integrationResID, resObject);
        dicResData.Add(integrationResID, data);

        return data;
    }

    public ResData GetResData(int integratinResID)
    {
        if (IsAvailable(integratinResID))
        {
            return dicResData[integratinResID];
        }
        else
        {
            return dicResData[0];
        }
    }

    public bool IsAvailable(int integrationResID)
    {
        return dicResData.ContainsKey(integrationResID);
    }

    public void ReleaseRes(int integrationResID)
    {
        if (IsAvailable(integrationResID))
        {
            ResData data = dicResData[integrationResID];
            data.Release();
        }
    }

    public bool AsyncLoad(RESTYPE type, int ResID)
    {
        return AsyncLoad(GetResIntegrationID(type, ResID));
    }

    public ResData SyncLoad(RESTYPE type, int ResID)
    {
        return SyncLoad(GetResIntegrationID(type, ResID));
    }

    public bool IsAvailable(RESTYPE type, int ResID)
    {
        return IsAvailable(GetResIntegrationID(type, ResID));
    }

    public void ReleaseRes(RESTYPE type, int ResID)
    {
        ReleaseRes(GetResIntegrationID(type, ResID));
    }

    public ResData GetResData(RESTYPE type, int ResID)
    {
        return GetResData(GetResIntegrationID(type, ResID));
    }

    public void CheckResycle()
    {
        float curTick = Time.realtimeSinceStartup;
        ArrayList list = new ArrayList();

        // Check if need to resycle
        foreach (KeyValuePair<int, ResData> pair in dicResData)
        {
            ResData data = pair.Value;
            if (data.refCount <= 0 && curTick - data.loadedTick >= RESYCLE_UNUSE_TICK)
            {
                list.Add(pair.Key);
            }
        }

        // Need to resycle
        if (list.Count > 0)
        {
            for (int i = 0; i < list.Count; i++)
            {
                dicResData.Remove((int)list[i]);
            }
            list.Clear();
        }
    }

    bool IsRequestQueueEmpty()
    {
        return queueRequest.Count == 0;
    }

    int DequeueRequestResID()
    {
        if (queueRequest.Count > 0)
        {
            return (int)queueRequest.Dequeue();
        }
        else
        {
            return 0;
        }
    }

    #endregion


    #region LoadingCoroutine
    //////////////////////////////////////////////////////////////////////////
    // Loading Coroutine
    //////////////////////////////////////////////////////////////////////////
    IEnumerator LoadingCoroutine()
    {
        while (IsRunLoadingCoroutine())
        {
            int nLoadCount = 0;
            while (!IsRequestQueueEmpty() && nLoadCount++ < RES_COROUTINE_LOAD_LIMIT)
            {
                SyncLoad(DequeueRequestResID());
            }

            // Wait for next frame
            yield return null;
        }
    }

    #endregion
}

//////////////////////////////////////////////////////////////////////////
//	Loading Thread
//////////////////////////////////////////////////////////////////////////
public class LoadingThread
{
    private bool bTerminate = false;

    public LoadingThread()
    {
        bTerminate = false;
    }

    public void Terminate()
    {
        bTerminate = true;
    }

    public bool IsTerminate()
    {
        return bTerminate;
    }

    public void RunThread()
    {
        while (!IsTerminate())
        {
            Thread.Sleep(1000);
        }
    }
}