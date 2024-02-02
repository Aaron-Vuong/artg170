using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;
    [Header("Debug")]
    [SerializeField]
    Logger _logger;

    [Header("Global Variables")]
    public HUDMenu hudMenu;
    public MainMenu mainMenu;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        _logger.Log("Game has started!");
    }
}
