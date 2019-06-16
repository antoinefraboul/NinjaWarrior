using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject m_canv;
    public GameObject m_optionsMenu;
    public GameObject m_creditsMenu;
    public GameObject m_mainMenu;

    void Start()
    {
        PlayerPrefs.SetString("scoreEnable", "false");
        FindObjectOfType<OptionsManager>().PlaySound("bgm_menu");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O) && !m_creditsMenu.activeInHierarchy)
        {
            //Options
            m_optionsMenu.SetActive(!m_optionsMenu.activeInHierarchy);
            m_canv.SetActive(!m_optionsMenu.activeInHierarchy);
        }
        if (Input.GetKeyDown(KeyCode.C) && !m_optionsMenu.activeInHierarchy)
        {
            //Credits
            m_creditsMenu.SetActive(!m_creditsMenu.activeInHierarchy);
            m_mainMenu.SetActive(!m_creditsMenu.activeInHierarchy);
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            //Highscore
            LoadScore();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (m_optionsMenu.activeInHierarchy) m_optionsMenu.SetActive(!m_optionsMenu.activeInHierarchy);
            if (m_creditsMenu.activeInHierarchy) m_creditsMenu.SetActive(!m_creditsMenu.activeInHierarchy);
            m_canv.SetActive(true);
            m_mainMenu.SetActive(true);
        }
    }

    public void PlayGame()
    {
        Time.timeScale = 1f;
        PlayerPrefs.SetString("scoreEnable", "true");
        PlayerPrefs.SetFloat("timer", Time.time);


        if(FindObjectOfType<OptionsManager>().m_lvl1Enable.isOn)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); //load lvl1
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2); //load lvl2
        }
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
