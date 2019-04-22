using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    bool gameHasEnded = false;
    public TextMeshProUGUI m_time_UI;
    float m_startTime;
    float m_time;
    bool m_gameIsPlaying = true;
    void Start()
    {
        m_startTime = Time.time;
    }

    void Update()
    {
        //Timer
        if (m_gameIsPlaying)
        {
            float t = Time.time - m_startTime;
            string minutes = ((int)t / 60).ToString();
            string seconds = (t % 60).ToString("f0");
            m_time_UI.SetText(minutes + ":" + seconds);
        }

    }

    public void Playing(bool state)
    {
        m_gameIsPlaying = state;
    }

    public bool isPlaying()
    {
        return m_gameIsPlaying;
    }

    public void EndGame()
    {
        if (gameHasEnded == false)
        {
            gameHasEnded = true;
            Debug.Log("game over");
            RestartScene();
        }
        
    }

    void RestartScene()
    {
        Debug.Log("restart");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
