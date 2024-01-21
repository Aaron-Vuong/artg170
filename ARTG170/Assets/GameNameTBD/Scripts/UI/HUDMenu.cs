using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDMenu : MenuManager
{
    [SerializeField] private UIManager _uiManager;
    protected override void InnerAwake()
    {
        menuType = GameMenu.GameHUD;
    }
    private void Update()
    {
        // On User Escape, we go to the settings.
        if (Input.GetKeyDown(KeyCode.Escape) && _uiManager.getCurrentMenu() == GameMenu.GameHUD)
        {
            _uiManager.GoToMenu(GameMenu.Settings);
        }
    }
}
