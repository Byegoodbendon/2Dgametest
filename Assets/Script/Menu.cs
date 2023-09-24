using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class Menu : MonoBehaviour
{
    public GameObject Pausegame;
    public GameObject PauseButton;
    public GameObject DeathPanel;
    public AudioMixer audioMixer;

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void UIenable()
    {
        GameObject.Find("Canvas/Menu/UI").SetActive(true);
    }
    public void PauseGame()
    {
        Pausegame.SetActive(true);
        PauseButton.SetActive(false);
        Time.timeScale = 0.0f;
    }
    public void ResumeGame()
    {
        Pausegame.SetActive(false);
        PauseButton.SetActive(true);
        Time.timeScale = 1.0f;
    }
    public void SetVolume(float value)
    {
        audioMixer.SetFloat("MainVolume", value);
    }
    public void GameOver()
    {
        PauseButton.SetActive(false);
        DeathPanel.SetActive(true);
        Time.timeScale = 0.0f;
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        DeathPanel.SetActive(false);
        Time.timeScale = 1.0f;
    }
}
