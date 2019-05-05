using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class ScoreMenu : MonoBehaviour
{
    public GameObject m_SaveButton;
    public TextMeshProUGUI m_scoreText;
    public TMP_InputField m_input;
    public GameObject m_scoreEnablePanel;
    public ScoreManager m_scoreManager;
   
    float m_score;
    void Start()
    {
        m_score = Time.time - PlayerPrefs.GetFloat("timer");
        string minutes = ((int)m_score / 60).ToString();
        string seconds = (m_score % 60).ToString("f0");
        m_scoreText.SetText(minutes + ":" + seconds);
        //If a level has been skip, score canno't be save
        m_scoreEnablePanel.SetActive(PlayerPrefs.GetString("scoreEnable") == "true");
    }

    void Update()
    {
        m_SaveButton.GetComponent<Button>().enabled = m_input.text.Length > 0;

        if (Input.GetKeyDown(KeyCode.Escape))
            SceneManager.LoadScene(0);

        if (m_scoreEnablePanel.activeInHierarchy && Input.GetKeyDown(KeyCode.KeypadEnter) || (Input.GetKeyDown(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.S)) && m_input.text.Length > 0)
            Save();
    }

    public void Save()
    {
        m_scoreEnablePanel.SetActive(false);
        m_scoreManager.AddScore(m_input.text, m_score);
    }

    public void Clear()
    {
        m_scoreManager.ClearScore();
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene(0);
    }
}
