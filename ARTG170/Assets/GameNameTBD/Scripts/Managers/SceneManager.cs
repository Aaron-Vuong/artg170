using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : SingletonBase<SceneManager>
{
    [Header("Debug")]
    [SerializeField]
    Logger _logger;

    private void Start()
    {
        _logger.Log("Loading SceneManager!");
    }
}
