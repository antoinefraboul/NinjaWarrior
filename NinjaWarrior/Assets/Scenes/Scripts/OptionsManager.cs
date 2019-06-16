using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using System;
using UnityEngine.Networking;

public class OptionsManager : MonoBehaviour
{
    public static Options m_options;
    public Sound[] m_sounds;
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
    public Text m_colorRandomText;
    public Slider m_colorRandomSlider;
    public Text m_stepsText;
    public Slider m_stepsSlider;
    public Text m_randomStepsText;
    public Slider m_randomStepsSlider;
    public Text m_sideStepsText;
    public Slider m_sideStepsSlider;
    public Button m_loadLevel1;
    public Button m_loadLevel2;
    public Button m_loadLevel3;
    public Toggle m_lvl1Enable;
    public Toggle m_lvl3Enable;
    public Button m_resetLevel2;
    public Button m_resetAll;
   
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
        m_colorSlider.onValueChanged.AddListener(delegate { m_options.lvl2_nbColors=((int)m_colorSlider.value); m_colorRandomSlider.maxValue = 10 - m_options.lvl2_nbColors; SaveOptions(); });
        m_colorRandomSlider.onValueChanged.AddListener(delegate { m_options.lvl2_random_color_max = ((int)m_colorRandomSlider.value); SaveOptions(); });
        m_stepsSlider.onValueChanged.AddListener(delegate { m_options.lvl2_steps=((int)m_stepsSlider.value); SaveOptions(); });
        m_randomStepsSlider.onValueChanged.AddListener(delegate { m_options.lvl2_randomSteps=((int)m_randomStepsSlider.value); SaveOptions(); });
        m_sideStepsSlider.onValueChanged.AddListener(delegate { m_options.lvl2_sideSteps=((float)m_sideStepsSlider.value); SaveOptions(); });
        m_loadLevel1.onClick.AddListener(delegate { LoadLevel(1); });
        m_loadLevel2.onClick.AddListener(delegate { LoadLevel(2);});
        m_loadLevel3.onClick.AddListener(delegate { LoadLevel(3); });
        m_resetLevel2.onClick.AddListener(delegate { ResetOptions(2); });
        m_resetAll.onClick.AddListener(delegate { ResetOptions(); });
        m_lvl1Enable.onValueChanged.AddListener(delegate { m_options.lvl1_enable = (m_lvl1Enable.isOn); SaveOptions(); });
        m_lvl3Enable.onValueChanged.AddListener(delegate { m_options.lvl3_enable = (m_lvl3Enable.isOn); SaveOptions(); });
        //Sounds
        foreach (Sound s in m_sounds)
        {
            s.m_source = gameObject.AddComponent<AudioSource>();
            s.m_source.loop = s.m_loop;
            s.m_source.clip = s.m_clip;
            s.m_source.volume = s.m_volume;
            s.m_source.pitch = s.m_pitch;
        }
    }

    void Update()
    {
        UpdateUI();

        //Sounds
        foreach(Sound s in m_sounds)
        {
            if (s.m_source.isPlaying)
            {
                s.m_source.volume = s.m_volume*((float)m_options.volume/100);
                s.m_source.mute = m_options.volumeMuted;
            }
        }
 
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
        m_colorRandomText.text = m_options.lvl2_random_color_max.ToString();
        m_colorRandomSlider.value = m_options.lvl2_random_color_max;
        m_stepsText.text = m_options.lvl2_steps.ToString();
        m_stepsSlider.value = m_options.lvl2_steps;
        m_randomStepsText.text = m_options.lvl2_randomSteps.ToString();
        m_randomStepsSlider.value = m_options.lvl2_randomSteps;
        m_sideStepsText.text = m_options.lvl2_sideSteps.ToString();
        m_sideStepsSlider.value = m_options.lvl2_sideSteps;
        m_lvl1Enable.isOn = m_options.lvl1_enable;
        m_lvl3Enable.isOn = m_options.lvl3_enable;
    }

    public void SaveOptions()
    {
        string json = JsonUtility.ToJson(m_options);
        File.WriteAllText(m_path, json);

        LoadOptionsData();
    }

    public void LoadOptionsData()
    {
        m_options = new Options();
        string jsonString = File.ReadAllText(m_path);
        JsonUtility.FromJsonOverwrite(jsonString, m_options);

        //change common settings
        Resolution r = m_resolutions[m_options.resolution];
        Screen.SetResolution(r.width, r.height,m_options.fullScreen);
    }

    public void PlaySound(string name)
    {
        Sound s = Array.Find(m_sounds, m_sound => m_sound.m_clip.name==name);
        if(!s.m_source.isPlaying) s.m_source.Play();
    }

    public void StopSound(string name)
    {
        Sound s = Array.Find(m_sounds, m_sound => m_sound.m_clip.name == name);
        s.m_source.Stop();
    }

    public void StopSound()
    {
        foreach (Sound s in m_sounds)
        {
            if (s.m_source.isPlaying)
            {
                s.m_source.Stop();
            }
        }
    }

    public void LoadLevel(int n)
    {
        PlayerPrefs.SetFloat("timer", Time.time);
        if (n == 1) PlayerPrefs.SetString("scoreEnable", "true");
        else PlayerPrefs.SetString("scoreEnable", "false");
        SceneManager.LoadScene(n);
    }

    public void ResetOptions(int type=0)
    {
        if (type == 1) //reset lvl1
        {
            m_options.lvl1_enable = false;
        }
        else if (type == 2) //reset lvl2
        {
            m_options.lvl2_nbColors = 5;
            m_options.lvl2_steps = 4;
            m_options.lvl2_randomSteps = 4;
            m_options.lvl2_sideSteps = 10;
            m_options.lvl2_random_color_max = 0;
        }else if (type == 3) //reset lvl3
        {
            m_options.lvl3_enable = false;
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
    public float lvl2_sideSteps;
    public int lvl2_random_color_max;
    public bool lvl1_enable;
    public bool lvl3_enable;
    public Options(int res=0, bool fs=true,bool vm=false,int v=50, int c=5, int s=4, int r=4, float ss=10.0f,int rc=0, bool lvl1=false, bool lvl3=true)
    {
        resolution = res;
        fullScreen = fs;
        volumeMuted = vm;
        volume = v;
        lvl2_nbColors = c;
        lvl2_steps = s;
        lvl2_randomSteps = r;
        lvl2_sideSteps = ss;
        lvl2_random_color_max = rc;
        lvl1_enable = lvl1;
        lvl3_enable = lvl3;
    }
}

[System.Serializable]
public class Sound
{
    public AudioClip m_clip;
    [Range(0,1)]
    public float m_volume=1;
    public float m_pitch;
    public bool m_loop;

    [HideInInspector]
    public AudioSource m_source;

}