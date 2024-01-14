using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : SingletonBase<SettingsManager>
{
    [Header("Debug")]
    [SerializeField]
    Logger _logger;

    [Header("Settings")]
    public int audioLevel;

    private void Start()
    {
        _logger.Log("Settings are loaded!");
    }
}
