using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingMenu : MenuManager
{
    [SerializeField] private Image _progressBar;


    protected override void InnerAwake()
    {
        menuType = GameMenu.Loading;
    }
    void Start()
    {
        UnityEngine.Assertions.Assert.IsNotNull(_progressBar);
    }

    private void Update()
    {
        _progressBar.fillAmount = SceneChangeManager.GetLoadingProgress();
    }

}
