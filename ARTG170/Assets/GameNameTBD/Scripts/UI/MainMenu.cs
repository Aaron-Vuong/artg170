using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MenuManager
{
    [SerializeField] private UIManager _uiManager;


    [SerializeField] private Button _btnStart;
    [SerializeField] private Button _btnSettings;
    [SerializeField] private Button _btnExit;

    [SerializeField] private Logger _logger;

    protected override void InnerAwake()
    {
        menuType = GameMenu.Main;
    }

    // Start is called before the first frame update
    void Start()
    {
        _logger.Log("Hellp" + menuType);
        UnityEngine.Assertions.Assert.IsFalse(menuType == 0);
        UnityEngine.Assertions.Assert.IsNotNull(_btnStart);
        UnityEngine.Assertions.Assert.IsNotNull(_btnSettings);
        UnityEngine.Assertions.Assert.IsNotNull(_btnExit);

        _btnStart.onClick.AddListener(OnStartGame);
        _btnSettings.onClick.AddListener(OnDisplayOptionsMenu);
        _btnExit.onClick.AddListener(OnExitGame);
    }

    private void OnStartGame()
    {
        // TODO: Loading Screen Scene -> Main Game Scene.
        _uiManager.GoToMenu(GameMenu.GameHUD);
    }

    private void OnExitGame()
    {
        Application.Quit();
    }

    private void OnDisplayOptionsMenu()
    {
        _uiManager.GoToMenu(GameMenu.Settings);
    }
}
