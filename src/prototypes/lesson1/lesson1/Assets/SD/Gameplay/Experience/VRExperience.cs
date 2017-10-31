﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System;

public class VRExperience : MonoBehaviour {

    private static VRExperience _instance;

    private Dictionary<string, object> configuration = new Dictionary<string, object>();

    private string[] videos;
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

        videos = ((List<string>)configuration["videos"]).ToArray();
        ResourcesPath = (string)configuration["resourcesPath"];
        MatedataPath = (string)configuration["matedataPath"];
        AudioPath = (string)configuration["audioPath"];
        VideosPath = (string)configuration["videosPath"];

        SceneManager.LoadScene("Scenes/Experience", LoadSceneMode.Single);
    }

    private void ShowErrorPanel(string msg)
    {
        GameObject panelErrorObj = GameObject.Find("ErrorMenu");
        TextMesh textObject = GameObject.Find("UI_ErrorDialogText").GetComponent<TextMesh>();
        textObject.text = msg;
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
        if (indiceVideo < videos.Length) {
            return videos[indiceVideo];

        } else {
            indiceVideo = -1;
            return "End";
        }
    }
	internal string CurrentVideo()
	{
		if (indiceVideo < videos.Length) {
			return videos[indiceVideo];

		} else {
			return "End";
		}
	}

	internal string SelectVideo(int indice)
	{
		if (indice < videos.Length) {
			indiceVideo = indice;
			return videos[indiceVideo];
		} else {
			return "Error";
		}
	}

	internal int CountVideo()
	{
		return videos.Length;
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
