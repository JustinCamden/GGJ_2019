using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class MainMenu : MonoBehaviour 
{
    [Header("Main Menu")]
    public Button bStart;
    public Button bExit;

    [Space]
    [Header("Settings Menu")]
    public AudioMixer masterMixer;
    public AudioSource sourceAudio;

    private void Start()
    {
        bStart.onClick.AddListener(() => StartGame());
        bExit.onClick.AddListener(() => QuitGame());
    }

    void StartGame()
    {
        SceneManager.LoadScene(1);
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
