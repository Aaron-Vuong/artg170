using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeManager : SingletonBase<SceneChangeManager>
{


    [Header("Debug")]
    [SerializeField]
    Logger _logger;



    private void Start()
    {
        _logger.Log("Loading SceneManager!");
        
    }
}
