using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    void Start()
    {
        PlayerPrefs.SetString("scoreEnable", "false");
    }

    public void PlayGame()
    {
        Time.timeScale = 1f;
        PlayerPrefs.SetString("scoreEnable", "true");
        PlayerPrefs.SetFloat("timer", Time.time);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void LoadScore()
    {
        SceneManager.LoadScene(SceneManager.sceneCountInBuildSettings-1); //Last scene is scoreboard
    }
}
