using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Logger : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    bool _showLogs = true;

    public void Log(object message)
    {
        if (_showLogs)
        {
            Debug.Log($"{name}: {message}");
        }
    }
    public void LogError(object message)
    {
        if (_showLogs)
        {
            Debug.LogError($"{name}: {message}");
        }
    }
}
