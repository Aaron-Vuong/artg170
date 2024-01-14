using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class SingletonBase<T> :  MonoBehaviour where T : SingletonBase<T>
{
    private static SingletonBase<T> _instance;
    private void Awake()
    {
        /*
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
        */
    }
}
