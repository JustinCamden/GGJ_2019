using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour 
{
    public Sprite logo;

    [Header("Main Menu")]
    public GameObject mMenu;
    public Button bStart;
    public Button bExit;
    public Button bSettings;

    [Space]
    [Header("Potato")]
    public Button bFullscreen;

    private void Start()
    {
        bStart.onClick.AddListener(() => StartGame());
        bSettings.onClick.AddListener(() => switchToOption());
        bExit.onClick.AddListener(() => QuitGame());
    }

    void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    void switchToOption()
    {
        mMenu.SetActive(false);
    }

    void QuitGame()
    {
        Application.Quit();
    }

    //----------Settings


}
