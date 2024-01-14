using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : SingletonBase<GameStateManager>
{
    [Header("Debug")]
    [SerializeField]
    Logger _logger;

    private void Start()
    {
        _logger.Log("Game has started!");
    }
}
