using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Logger : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    bool _showLogs = true;

    // Loggers should be persistent across scenes.
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    public void Log(string message)
    {
        if (_showLogs)
        {
            Debug.Log($"{name}: {message}");
        }
    }
}
