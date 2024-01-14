using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyFolder<T> : MonoBehaviour where T : DontDestroyFolder<T>
{
    private static DontDestroyFolder<T> _instance;
    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
