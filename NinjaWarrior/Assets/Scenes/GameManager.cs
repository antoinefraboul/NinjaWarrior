using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI m_time_UI;
    float m_startTime;
    float m_time;
    public static bool m_gameIsPlaying = true;
    public GameObject pauseMenuUI;
    void Start()
    {
        m_startTime = Time.time;
    }

    void Update()
    {
        //Pause
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (m_gameIsPlaying)
            {
                Pause();
            }
            else
            {
                Resume();
            }
        }
        //Timer
        if (m_gameIsPlaying)
        {
            float t = Time.time - m_startTime;
            string minutes = ((int)t / 60).ToString();
            string seconds = (t % 60).ToString("f0");
            m_time_UI.SetText(minutes + ":" + seconds);
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        m_gameIsPlaying = true;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        m_gameIsPlaying = false;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void SkipLevel()
    {
        Debug.Log("skip lvl");
    }
     
    void RestartScene()
    {
        Debug.Log("restart");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
