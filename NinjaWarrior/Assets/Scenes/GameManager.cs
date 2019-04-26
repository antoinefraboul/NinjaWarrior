using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI m_time_UI;
    float m_startTime;
    float m_time;
    public GameObject pauseMenuUI;
    public GameObject retryMenuUI;
    public GameObject nextLevelUI;

    void Start()
    {
        m_startTime = PlayerPrefs.GetFloat("timer");
    }

    void Update()
    {
        //Pause
        if (Input.GetKeyDown(KeyCode.Escape))
        {

            if (Time.timeScale > 0)
            {
                Pause();
            }
            else
            {
                Resume();
            }
        }
        //Timer
        if (Time.timeScale > 0)
        {
            float t = Time.time - m_startTime;
            string minutes = ((int)t / 60).ToString();
            string seconds = (t % 60).ToString("f0");
            m_time_UI.SetText(minutes + ":" + seconds);
        }
    }

    public void GameOver()
    {
        Time.timeScale = 0f;
        retryMenuUI.SetActive(true);
    }
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void SkipLevel()
    {
        PlayerPrefs.SetInt("scoreEnable", 1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Win()
    {
        nextLevelUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
