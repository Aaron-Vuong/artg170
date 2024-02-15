using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public enum GameMenu
{
    Undefined,
    Main,
    GameHUD,
    Pause,
    Settings,
    Loading
}

public class UIManager : MonoBehaviour
{
    
    private Dictionary<GameMenu, GameObject> _menus;
    private GameMenu _currentMenu;
    private GameMenu _lastMenu;
    [SerializeField]
    private Logger _logger;
    [SerializeField] private PlayerCam _playerCam;
    // Start is called before the first frame update
    void Start()
    {

        _menus = new Dictionary<GameMenu, GameObject>();
        // SceneLoader will populate _menus
        GameObject[] menus = GameObject.FindGameObjectsWithTag("Menu");
        foreach (GameObject menu in menus)
        {
            if (menu.GetComponent<MenuManager>() == null)
                _logger.LogError($"No MenuManager found on {menu.name}");
            else
                _logger.Log($"MenuManager found on {menu.name}");

            _menus[menu.GetComponent<MenuManager>().menuType] = menu;
            menu.GetComponent<MenuManager>().CloseMenu(); //make sure all menus start closed
        }

        // TODO: Hardcoded to go to Main Menu on Start(). This should be fine?
        GoToMenu(GameMenu.Main);
    }

    private void CloseMenu(GameMenu Menu)
    {
        _menus[Menu].GetComponent<MenuManager>().CloseMenu();
    }
    private void OpenMenu(GameMenu Menu)
    {
        _menus[Menu].GetComponent<MenuManager>().OpenMenu();
    }
    public void GoToMenu(GameMenu Menu)
    {
        // 0 is the Undefined default, we want to instead default to GameMenu.Main.
        if (_currentMenu != 0)
        {
            CloseMenu(_currentMenu);
        }
        else
        {
            _currentMenu = GameMenu.Main;
        }
        _lastMenu = _currentMenu;
        _logger.Log($"Setting last menu to {_lastMenu}");
        _currentMenu = Menu;
        // Lock the cursor if we are in the GameHUD.
        if (_currentMenu == GameMenu.GameHUD) {
            _playerCam.lockCursor();
        }
        else {
            _playerCam.unlockCursor();
        }
        OpenMenu(_currentMenu);
    }

    public void GoBackToLastMenu()
    {
        _logger.Log($"Going back to last menu: {_lastMenu}!");
        if (_lastMenu != 0)
        {
            GoToMenu(_lastMenu);
        }
    }

    public GameMenu getCurrentMenu() { return _currentMenu; }
    public GameMenu getLastMenu() { return _lastMenu; }
}
