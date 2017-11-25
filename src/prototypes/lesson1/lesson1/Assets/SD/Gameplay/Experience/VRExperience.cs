using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System;

public class VRExperience : MonoBehaviour {

    private static VRExperience _instance;

    private Dictionary<string, object> configuration = new Dictionary<string, object>();
    private Dictionary<string, string> gameObjectsText = new Dictionary<string, string>();

    private String[] videos;
    private static int indiceVideo = -1;

    public string ResourcesPath { get; private set; }
    public string MatedataPath { get; private set; }
    public string AudioPath { get; private set; }
    public string VideosPath { get; private set; }

    private string log;

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

    public void StartExperience(Dictionary<string, object> config1, bool overwriteSettings)
    {
        Util util = Util.Instance;
        try
        {
            /*foreach (KeyValuePair<string, object> pair in config)
            {
                if (!configuration.ContainsKey(pair.Key) || overwriteSettings)
                {
                    configuration[pair.Key] = pair.Value;
                }
            }*/

       
            ConfigManager configManager = ConfigManager.Instance;
            Dictionary<string, object> config = configManager.Settings;

            log = "VRExperience.StartExperience \n";
            log += "lesson:" + (int)config["lessonId"] + " \n";

            util.ShowErrorPanel(log);

            if (config.ContainsKey("appSettingtMap"))
            {
                Dictionary<string, AppConfigType> appSettingtMap = (Dictionary<string, AppConfigType>)config["appSettingtMap"];
                int lessonId = (int)config["lessonId"];
                AppConfigType appConfig = appSettingtMap[lessonId.ToString()];

                log += "ResourcesPath:" + appConfig.ResourcesPath + " \n";
                log += "MatedataPath:" + appConfig.MetedataPath + " \n";
                log += "AudioPath:" + appConfig.AudioPath + " \n";
                log += "VideosPath:" + appConfig.VideosPath + " \n";
                util.ShowErrorPanel(log);

                videos = (String[])appConfig.Videos.ToArray(typeof(string));
                ResourcesPath = appConfig.ResourcesPath;
                MatedataPath = appConfig.MetedataPath;
                AudioPath = appConfig.AudioPath;
                VideosPath = appConfig.VideosPath;

                gameObjectsText = (Dictionary<string, string>)config["gameObjectsTexts"];

                SceneManager.LoadScene("Scenes/Experience", LoadSceneMode.Single);
            }
            else
            {
                log += "Error. No se puede cargar la leccion. appSettingtMap es nulo \n";
                util.ShowErrorPanel(log);
            }

            if (!config.ContainsKey("gameObjectsTexts"))
            {
                log += "Error. gameObjectsTexts es nulo \n";
                util.ShowErrorPanel(log);
            }

            if (!config.ContainsKey("userConfig"))
            {
                log += "Error. userConfig es nulo \n";
                util.ShowErrorPanel(log);
            }
        }
        catch (Exception e)
        {
            log += "Exception: " + e.Message + "\n" + e.StackTrace;
            util.ShowErrorPanel(log);
        }
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

    public string GetGameObjectText(string textObjectId)
    {
        if (gameObjectsText.ContainsKey(textObjectId))
        {
            return gameObjectsText[textObjectId];
        }
        return "";
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
