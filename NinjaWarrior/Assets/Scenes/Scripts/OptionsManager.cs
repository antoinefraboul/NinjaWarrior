using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using UnityEngine.SceneManagement;

public class OptionsManager : MonoBehaviour
{
    static Options m_options;
    string m_jsonFileName = "optionsData.json";
    string m_path;
    List<Resolution> m_resolutions;

    public Dropdown m_resolutionDropDown;
    public Toggle m_fullScreenToggle;
    public Toggle m_volumeToggle;
    public Text m_volumeText;
    public Slider m_volumeSlider;
    public Text m_colorText;
    public Slider m_colorSlider;
    public Text m_stepsText;
    public Slider m_stepsSlider;
    public Text m_randomStepsText;
    public Slider m_randomStepsSlider;
    public Text m_sideStepsText;
    public Slider m_sideStepsSlider;

    // Start is called before the first frame update
    void Start()
    {
        m_path = System.IO.Path.Combine(Application.streamingAssetsPath, m_jsonFileName);
    
        Resolution[] reso = Screen.resolutions;
        m_resolutions = new List<Resolution>();
        List<string> resoString = new List<string>();
        for (int i = 0; i < reso.Length; i++)
        {
            resoString.Add(reso[i].width + " x " + reso[i].height);
            m_resolutions.Add(reso[i]);
        }
        resoString.Reverse();
        m_resolutions.Reverse();
        m_resolutionDropDown.ClearOptions();
        m_resolutionDropDown.AddOptions(resoString);

        LoadOptionsData();

        //Listner
        m_resolutionDropDown.onValueChanged.AddListener(delegate { m_options.resolution = (m_resolutionDropDown.value); SaveOptions(); });
        m_fullScreenToggle.onValueChanged.AddListener(delegate { m_options.fullScreen=(m_fullScreenToggle.isOn); SaveOptions(); });
        m_volumeToggle.onValueChanged.AddListener(delegate { m_options.volumeMuted = (m_volumeToggle.isOn); SaveOptions(); });
        m_volumeSlider.onValueChanged.AddListener(delegate {m_options.volume=((int)m_volumeSlider.value); SaveOptions();});
        m_colorSlider.onValueChanged.AddListener(delegate { m_options.lvl2_nbColors=((int)m_colorSlider.value); SaveOptions(); });
        m_stepsSlider.onValueChanged.AddListener(delegate { m_options.lvl2_steps=((int)m_stepsSlider.value); SaveOptions(); });
        m_randomStepsSlider.onValueChanged.AddListener(delegate { m_options.lvl2_randomSteps=((int)m_randomStepsSlider.value); SaveOptions(); });
        m_sideStepsSlider.onValueChanged.AddListener(delegate { m_options.lvl2_sideSteps=((int)m_sideStepsSlider.value); SaveOptions(); });
    }

    void Update()
    {
        UpdateUI();
    }

    void UpdateUI()
    {
        m_resolutionDropDown.value = m_options.resolution;
        m_resolutionDropDown.RefreshShownValue();
        m_fullScreenToggle.isOn = m_options.fullScreen;
        m_volumeToggle.isOn = m_options.volumeMuted;
        m_volumeText.text = m_options.volume.ToString();
        m_volumeSlider.value = m_options.volume;
        m_colorText.text = m_options.lvl2_nbColors.ToString();
        m_colorSlider.value = m_options.lvl2_nbColors;
        m_stepsText.text = m_options.lvl2_steps.ToString();
        m_stepsSlider.value = m_options.lvl2_steps;
        m_randomStepsText.text = m_options.lvl2_randomSteps.ToString();
        m_randomStepsSlider.value = m_options.lvl2_randomSteps;
        m_sideStepsText.text = m_options.lvl2_sideSteps.ToString();
        m_sideStepsSlider.value = m_options.lvl2_sideSteps;
    }

    public void LoadOptionsData()
    {
        m_options = new Options();
        string jsonString = File.ReadAllText(m_path);
        JsonUtility.FromJsonOverwrite(jsonString, m_options);

        //change settings
        Resolution r = m_resolutions[m_options.resolution];
        Screen.SetResolution(r.width, r.height,m_options.fullScreen);
        //popup pour avertir

    }

    public void SaveOptions()
    {
        string json = JsonUtility.ToJson(m_options);
        File.WriteAllText(m_path, json);
    }

    public void LoadLevel(int n)
    {
        PlayerPrefs.SetFloat("timer", Time.time);
        PlayerPrefs.SetString("scoreEnable", "false");
        SceneManager.LoadScene(n);
    }

    public void ResetOptions(int type=0)
    {
        if (type == 1) //reset lvl1
        {

        }else if (type == 2) //reset lvl2
        {
            m_options.lvl2_nbColors = 5;
            m_options.lvl2_steps = 4;
            m_options.lvl2_randomSteps = 4;
            m_options.lvl2_sideSteps = 20;
        }
        else
        {
            m_options = new Options();
        }
   
        SaveOptions();
        LoadOptionsData();
    }
}


//********Options
[System.Serializable]
public class Options
{
    public int resolution;
    public bool fullScreen;
    public bool volumeMuted;
    public int volume;
    public int lvl2_nbColors;
    public int lvl2_steps;
    public int lvl2_randomSteps;
    public int lvl2_sideSteps;

    public Options(int res=0, bool fs=true,bool vm=false,int v=50, int c=5, int s=4, int r=4, int ss=20)
    {
        resolution = res;
        fullScreen = fs;
        volumeMuted = vm;
        volume = v;
        lvl2_nbColors = c;
        lvl2_steps = s;
        lvl2_randomSteps = r;
        lvl2_sideSteps = ss;
    }
}