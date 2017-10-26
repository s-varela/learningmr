using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System;

public class VRExperience : MonoBehaviour {

    private static VRExperience _instance;

    private Dictionary<string, object> configuration = new Dictionary<string, object>();

   // private string[] videos; // = { "Lesson01-01.mp4", "Lesson01-02.mp4", "Lesson01-03.mp4", "Lesson01-04.mp4", "Lesson01-05.mp4" };
   // private string pathSubs = "/lesson1-data/subs/";

    private static int indiceVideo = -1;

    public string ResourcesPath { get; private set; }
    public string MatedataPath { get; private set; }
    public string AudioPath { get; private set; }
    public string VideosPath { get; private set; }


    public static VRExperience Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<VRExperience>();
                // DontDestroyOnLoad(_instance.gameObject);
            }
            return _instance;
        }
    }

    //public string[] Videos { get; private set; }
    public List<string> Videos { get; private set; }

    // Use this for initialization
    void Start() {
        indiceVideo = -1;
    }

    // Update is called once per frame
    void Update() {

    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            indiceVideo = -1;
            DontDestroyOnLoad(this);
        }
        else if (this != _instance)
        {
            Destroy(gameObject);
        }
    }

    public void StartExperience(Dictionary<string, object> config, bool overwriteSettings)
    {
        foreach (KeyValuePair<string, object> pair in config)
        {
            if (!configuration.ContainsKey(pair.Key) || overwriteSettings)
            {
                configuration[pair.Key] = pair.Value;
            }
        }

        Videos = (List<string>)configuration["videos"];
        ResourcesPath = (string)configuration["resourcesPath"];
        MatedataPath = (string)configuration["matedataPath"];
        AudioPath = (string)configuration["audioPath"];
        VideosPath = (string)configuration["videosPath"];

        SceneManager.LoadScene("Scenes/Experience", LoadSceneMode.Single);
    }

    public void StartExperience()
    {
        SceneManager.LoadScene("Scenes/Experience", LoadSceneMode.Single);
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("Scenes/MainMenu", LoadSceneMode.Single);
    }

    public bool TryConfigurationValue(string name)
    {
        return configuration != null && configuration.ContainsKey(name);
    }

    public T GetConfigurationValue<T>(string name)
    {
        try
        {
            return (T)configuration[name];
        }
        catch
        {
            return default(T);
        }
    }

    internal string NextVideo()
    {
        indiceVideo++;
        if (indiceVideo < Videos.ToArray().Length) {
            return Videos[indiceVideo];

        } else {
            indiceVideo = -1;
            return "End";
        }
    }
	internal string CurrentVideo()
	{
		if (indiceVideo < Videos.ToArray().Length) {
			return Videos[indiceVideo];

		} else {
			return "End";
		}
	}

	internal string SelectVideo(int indice)
	{
		if (indice < Videos.ToArray().Length) {
			indiceVideo = indice;
			return Videos[indiceVideo];
		} else {
			return "Error";
		}
	}

	internal int CountVideo()
	{
		return Videos.ToArray().Length;
	}

	internal int GetIndice()
	{
		return indiceVideo;
	}

	internal void ResetIndice()
	{
		indiceVideo = -1;
	}
}
