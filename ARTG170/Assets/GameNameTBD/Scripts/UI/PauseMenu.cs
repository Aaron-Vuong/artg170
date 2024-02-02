using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MenuManager
{
    [SerializeField] private UIManager _uiManager;

    [SerializeField] private Button _btnBack;
    [SerializeField] private Button _btnSettings;

    [SerializeField] private Logger _logger;

    protected override void InnerAwake()
    {
        menuType = GameMenu.Pause;
    }

    // Start is called before the first frame update
    void Start()
    {
        UnityEngine.Assertions.Assert.IsFalse(menuType == 0);
        UnityEngine.Assertions.Assert.IsNotNull(_btnBack);
        UnityEngine.Assertions.Assert.IsNotNull(_btnSettings);
        _btnBack.onClick.AddListener(_uiManager.GoBackToLastMenu);
        _btnSettings.onClick.AddListener(OnDisplayOptionsMenu);
    }
    private void Update()
    {
        /*
        // On User Escape, we go to the settings.
        if (Input.GetKeyDown(KeyCode.Escape) && _uiManager.getCurrentMenu() == GameMenu.Pause)
        {
            _uiManager.GoBackToLastMenu();
        }
        */
    }
    private void OnDisplayOptionsMenu()
    {
        _uiManager.GoToMenu(GameMenu.Settings);
    }
}
