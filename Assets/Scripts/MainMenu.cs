using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class MainMenu : MonoBehaviour 
{
    public Sprite logo;

    [Header("Main Menu")]
    public GameObject mMenu;
    public Button bStart;
    public Button bExit;
    public Button bSettings;

    [Space]
    [Header("Settings Menu")]
    public GameObject mSettings;
    public Button bFullscreen;
    public AudioMixer masterMixer;
    public AudioSource sourceAudio;

    private void Start()
    {
        bStart.onClick.AddListener(() => StartGame());
        bSettings.onClick.AddListener(() => switchToOption());
        bExit.onClick.AddListener(() => QuitGame());
    }

    private void Update()
    {

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

    public void SetSFXLevel(float sfxLvl)
    {
        masterMixer.SetFloat("volSFX", sfxLvl);
        sourceAudio.Play();
    }

    public void SetMusicLevel(float musicLvl)
    {
        masterMixer.SetFloat("volMusic", musicLvl);
    }

}
