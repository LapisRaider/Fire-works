﻿using UnityEngine;

/// <summary>
/// Inherit from this base class to create a singleton.
/// e.g. public class MyClassName : SingletonBase<MyClassName> {}
/// </summary>
public class SingletonBase<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField]
    private bool bDontDestroyOnLoad = false;

    private static object m_Lock = new object();
    private static T m_Instance;

    public virtual void Awake()
    {
        //make sure wont have 2
        if (bDontDestroyOnLoad)
        {
            DontDestroyOnLoad(gameObject);
            if (Instance != this)
            {
                Destroy(gameObject);
            }
        }
    }

    /// <summary>
    /// Access singleton instance through this propriety.
    /// </summary>
    public static T Instance
    {
        get
        {
            lock (m_Lock)
            {
                if (m_Instance == null)
                {
                    // Search for existing instance.
                    m_Instance = (T)FindObjectOfType(typeof(T));

                    // Create new instance if one doesn't already exist.
                    if (m_Instance == null)
                    {
                        // Need to create a new GameObject to attach the singleton to.
                        var singletonObject = new GameObject();
                        m_Instance = singletonObject.AddComponent<T>();
                        singletonObject.name = typeof(T).ToString() + " (Singleton)";

                        // Make instance persistent.
                        DontDestroyOnLoad(singletonObject);
                    }
                }

                return m_Instance;
            }
        }
    }
}