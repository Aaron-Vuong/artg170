using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameMenu
{
    Undefined,
    Main,
    GameHUD,
    Pause,
    Settings
}

public class UIManager : MonoBehaviour
{
    
    private Dictionary<GameMenu, GameObject> _menus;
    private GameMenu _currentMenu;
    private GameMenu _lastMenu;
    [SerializeField]
    private Logger _logger;
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
            _logger.Log($"Done loading {menu.name} menu");
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
        if (_currentMenu != 0)
        {
            CloseMenu(_currentMenu);
        }
        else
        {
            _currentMenu = GameMenu.Main;
        }
        _lastMenu = _currentMenu;
        _currentMenu = Menu;
        OpenMenu(_currentMenu);
    }

    public void GoBackToLastMenu()
    {
        if (_lastMenu != 0)
        {
            GoToMenu(_lastMenu);
        }
    }
}
