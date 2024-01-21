using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MenuManager
{
    SettingsData settingsData;
    [SerializeField] private UIManager _uiManager;

    [SerializeField] private Button _btnBack;

    [SerializeField] private Logger _logger;

    protected override void InnerAwake()
    {
        menuType = GameMenu.Settings;
    }

    // Start is called before the first frame update
    void Start()
    {
        UnityEngine.Assertions.Assert.IsFalse(menuType == 0);
        UnityEngine.Assertions.Assert.IsNotNull(_btnBack);
        _btnBack.onClick.AddListener(_uiManager.GoBackToLastMenu);
    }

    private void Update()
    {
        // On User Escape, we go to the settings.
        /*
        if (Input.GetKeyDown(KeyCode.Escape) && _uiManager.getCurrentMenu() == GameMenu.Settings)
        {
            _uiManager.GoBackToLastMenu();
        }
        */
    }
}
