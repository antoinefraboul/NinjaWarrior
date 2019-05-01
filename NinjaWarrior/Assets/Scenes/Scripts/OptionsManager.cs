using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using UnityEngine.SceneManagement;

public class OptionsManager : MonoBehaviour
{
    public Options m_options;
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
        m_resolutionDropDown.onValueChanged.AddListener(delegate { m_options.resolution = (m_resolutionDropDown.value); });
        m_fullScreenToggle.onValueChanged.AddListener(delegate { m_options.fullScreen=(m_fullScreenToggle.isOn); });
        m_volumeToggle.onValueChanged.AddListener(delegate { m_options.volumeMuted = (m_volumeToggle.isOn); });
        m_volumeSlider.onValueChanged.AddListener(delegate { m_options.volume=((int)m_volumeSlider.value); });
        m_colorSlider.onValueChanged.AddListener(delegate { m_options.nbColors=((int)m_colorSlider.value); });
        m_stepsSlider.onValueChanged.AddListener(delegate { m_options.steps=((int)m_stepsSlider.value); });
        m_randomStepsSlider.onValueChanged.AddListener(delegate { m_options.randomSteps=((int)m_randomStepsSlider.value); });
        m_sideStepsSlider.onValueChanged.AddListener(delegate { m_options.sideSteps=((int)m_sideStepsSlider.value); });
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
        m_colorText.text = m_options.nbColors.ToString();
        m_colorSlider.value = m_options.nbColors;
        m_stepsText.text = m_options.steps.ToString();
        m_stepsSlider.value = m_options.steps;
        m_randomStepsText.text = m_options.randomSteps.ToString();
        m_randomStepsSlider.value = m_options.randomSteps;
        m_sideStepsText.text = m_options.sideSteps.ToString();
        m_sideStepsSlider.value = m_options.sideSteps;
    }

    public void LoadOptionsData()
    {
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadLevel(int n)
    {
        SceneManager.LoadScene(n);
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
    public int nbColors;
    public int steps;
    public int randomSteps;
    public int sideSteps;

    public Options(int res, bool fs,bool vm,int v, int c, int s, int r, int ss)
    {
        resolution = res;
        fullScreen = fs;
        volumeMuted = vm;
        volume = v;
        nbColors = c;
        steps = s;
        randomSteps = r;
        sideSteps = ss;
    }
}