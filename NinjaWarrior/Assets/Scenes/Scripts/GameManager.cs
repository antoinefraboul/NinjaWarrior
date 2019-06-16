using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI m_time_UI;
    float m_startTime;
    float m_time;
    public GameObject pauseMenuUI;
    public GameObject retryMenuUI;
    public GameObject nextLevelUI;
    public GameObject helpUI;
    public GameObject optionsUI;
    public Button helpButton;
    public bool m_update = true;

    void Start()
    {
        m_startTime = PlayerPrefs.GetFloat("timer");
    }

    void Update()
    {
        if (m_update)
        {
            helpButton.enabled = !optionsUI.activeInHierarchy;
            if (Time.timeScale > 0)
            {
                //Game is playing
                float t = Time.time - m_startTime;
                string minutes = ((int)t / 60).ToString();
                string seconds = (t % 60).ToString("f0");
                m_time_UI.SetText(minutes + ":" + seconds);

                if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
                {
                    HideMenu();
                    pauseMenuUI.SetActive(true);
                    Stop();
                }
                else if (Input.GetKeyDown(KeyCode.H))
                {
                    HideMenu();
                    helpUI.SetActive(true);
                    Stop();
                }
                else if (Input.GetKeyDown(KeyCode.O))
                {
                    HideMenu();
                    optionsUI.SetActive(true);
                    Stop();
                }
                else if (Input.GetKeyDown(KeyCode.R))
                {
                    RestartScene();
                }
            }
            else
            {
                //Game in pause
                if (Input.GetKeyDown(KeyCode.Escape) && nextLevelUI.activeInHierarchy)
                {
                    //Win
                    NextLevel();
                }
                else if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.R)) && retryMenuUI.activeInHierarchy)
                {
                    //Over
                    RestartScene();
                }
                else if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.H)) && helpUI.activeInHierarchy)
                {
                    //Help
                    HideMenu();
                    Resume();
                }
                else if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.O)) && optionsUI.activeInHierarchy)
                {
                    //Options
                    HideMenu();
                    Resume();
                }
                else if (Input.GetKeyDown(KeyCode.Escape) && pauseMenuUI.activeInHierarchy)
                {
                    //Pause

                    HideMenu();
                    Resume();
                }
            }
        }
    }

    public void HideMenu()
    {
        pauseMenuUI.SetActive(false);
        retryMenuUI.SetActive(false);
        nextLevelUI.SetActive(false);
        helpUI.SetActive(false);
        optionsUI.SetActive(false);
    }


    public void Win()
    {
        HideMenu();
        Stop();
        nextLevelUI.SetActive(true);
    }


    public void GameOver()
    {
        HideMenu();
        Stop();
        retryMenuUI.SetActive(true);
    }

    public void Resume()
    {
        Time.timeScale = 1f;
    }

    public void Stop()
    {
        Time.timeScale = 0f;
    }


    public void LoadMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void SkipLevel()
    {
        PlayerPrefs.SetString("scoreEnable", "false");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextLevel()
    {
        if(SceneManager.GetActiveScene().buildIndex==2 && !FindObjectOfType<OptionsManager>().m_lvl3Enable.isOn)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2); //do not load lvl 3
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        
    }

    public void LoadSceneIndex(int s)
    {
        SceneManager.LoadScene(s);
    }
}
