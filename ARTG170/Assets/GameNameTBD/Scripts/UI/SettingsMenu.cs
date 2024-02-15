using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MenuManager
{
    SettingsData settingsData;
    [SerializeField] private UIManager _uiManager;

    [SerializeField] private Button _btnBack;
    [SerializeField] private Button _btnFullScreen;

    [SerializeField] private Logger _logger;
    [SerializeField] private PlayerCam _playerCam;


    protected override void InnerAwake()
    {
        menuType = GameMenu.Settings;
    }

    // Start is called before the first frame update
    void Start()
    {
        UnityEngine.Assertions.Assert.IsFalse(menuType == 0);
        UnityEngine.Assertions.Assert.IsNotNull(_btnBack);
        UnityEngine.Assertions.Assert.IsNotNull(_btnFullScreen);
        _btnBack.onClick.AddListener(_uiManager.GoBackToLastMenu);
        _btnFullScreen.onClick.AddListener(toggleFullScreen);
    }

    private void Update()
    {

    }
    private void toggleFullScreen() {
        Screen.fullScreen = !Screen.fullScreen;
    }
}
