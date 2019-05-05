using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using System.Linq;

public class ScoreManager : MonoBehaviour
{
    public GameObject m_prefab;
    string m_jsonFileName = "scoresData.json";
    string m_path;
    List<Score> m_scoreboard;

    void Start()
    {
        m_scoreboard = new List<Score>();
        m_path = Path.Combine(Application.streamingAssetsPath, m_jsonFileName);
        FillWithData();
    }

    public void AddScore(string s, float t)
    {
        string d = System.DateTime.Now.ToShortDateString() + " - " + System.DateTime.Now.ToShortTimeString();
        m_scoreboard.Add(new Score(d, s, t));
        Save();
        FillWithData();
    }

    public void ClearScore()
    {
        m_scoreboard = new List<Score>();
        while (this.transform.childCount > 0)
        {
            Transform c = this.transform.GetChild(0);
            c.SetParent(null);
            Destroy(c.gameObject);
        }
        Save();
    }

    void FillWithData()
    {
        //Clear UI
        while (this.transform.childCount > 0)
        {
            Transform c = this.transform.GetChild(0);
            c.SetParent(null);
            Destroy(c.gameObject);
        }
        //Load data from json
        if (File.Exists(m_path))
        {
            string jsonString = File.ReadAllText(m_path);
            List<Score> tmp = new List<Score>();
            Score[] score = JsonHelper.FromJson<Score>(jsonString);
            for (int i = 0; i<score.Length; i++)
            {
                tmp.Add(new Score(score[i].date, score[i].name, score[i].time));
            }
            m_scoreboard = tmp.OrderBy(ob => ob.time).ToList();
        }

        
        GameObject o;
        for (int i = 0; i< m_scoreboard.Count; i++)
        {
            o = (GameObject)Instantiate(m_prefab, this.transform);
            o.transform.Find("Date").GetComponent<TextMeshProUGUI>().SetText(m_scoreboard[i].date);
            o.transform.Find("Name").GetComponent<TextMeshProUGUI>().SetText(m_scoreboard[i].name);
            o.transform.Find("Time").GetComponent<TextMeshProUGUI>().SetText(TimeToString(m_scoreboard[i].time));
        }
    }

    string TimeToString(float t)
    {
        string minutes = ((int)t / 60).ToString();
        string seconds = (t % 60).ToString("f0");

        return minutes + ":" + seconds;
    }

    void Save()
    {
        string newJson = JsonHelper.ToJson(m_scoreboard.ToArray(), true);
        File.WriteAllText(m_path, newJson);
    }

}

[System.Serializable]
public class Score
{
    public string date;
    public string name;
    public float time;

    public Score(string d,string n,float t)
    {
        date = d;
        name = n;
        time = t;
    }
}

public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper);
    }

    public static string ToJson<T>(T[] array, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }

    [System.Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}