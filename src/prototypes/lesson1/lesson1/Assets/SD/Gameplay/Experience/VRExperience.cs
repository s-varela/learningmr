using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System;

public class VRExperience : MonoBehaviour
{
    [SerializeField] private GameObject errorPanel;
    private static VRExperience _instance;

    private Dictionary<string, object> configuration = new Dictionary<string, object>();
    private Dictionary<string, string> gameObjectsText = new Dictionary<string, string>();
    private String[] videos;
    private static int indiceVideo = -1;
    private UserQualificationType userQualification = new UserQualificationType();
    private UserConfigType userConfig;

    public string ResourcesPath { get; private set; }
    public string MatedataPath { get; private set; }
    public string AudioPath { get; private set; }
    public string VideosPath { get; private set; }
    public UserQualificationType UserQualification { get;  set; }
    public UserConfigType UserConfig { get; private set; }


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
    void Start()
    {
        indiceVideo = -1;
    }

    // Update is called once per frame
    void Update()
    {

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

            ConfigManager configManager = ConfigManager.Instance;
            Dictionary<string, object> config = configManager.Settings;

            if (config.ContainsKey("appSettingtMap"))
            {
                Dictionary<string, AppConfigType> appSettingtMap = (Dictionary<string, AppConfigType>)config["appSettingtMap"];

                if (config.ContainsKey("lessonId")){
                    int lessonId = (int)config["lessonId"];
                    AppConfigType appConfig = appSettingtMap[lessonId.ToString()];

                    videos = (String[])appConfig.Videos.ToArray(typeof(string));
                    ResourcesPath = appConfig.ResourcesPath;
                    MatedataPath = appConfig.MetedataPath;
                    AudioPath = appConfig.AudioPath;
                    VideosPath = appConfig.VideosPath;

                    //El puntaje por cada usuario se resetea cuando se inicia una leccion
                    userConfig = (UserConfigType)config["userConfig"];

                    userQualification.LessonId = lessonId.ToString();
                    userQualification.UserId = userConfig.Id;
         

                    if (config.ContainsKey("gameObjectsTexts"))
                    {
                        gameObjectsText = (Dictionary<string, string>)config["gameObjectsTexts"];
                    }
                    else
                    {
                        log = "Error no se puedieron cargar las lecciones.\n Comprobar que el archivo app-text.xml exista en \n la carpeta de instalacion";
                        util.ShowErrorPanelByRef(errorPanel, log);
                    }
                    SceneManager.LoadScene("Scenes/Experience", LoadSceneMode.Single);
                }else
                {
                    log = "Error no se puedo cargar la leccion.";
                    util.ShowErrorPanelByRef(errorPanel, log);
                }
            }else
            {
                log = "Error no se puedieron cargar las lecciones.\n Comprobar que el archivo app-config.xml exista en \n la carpeta de instalacion";
                util.ShowErrorPanelByRef(errorPanel, log);
            }
        }
        catch (Exception e)
        {
            log = "Exception: " + e.Message + "\n" + e.StackTrace;
            util.ShowErrorPanelByRef(errorPanel, log);
        }
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
        if (indiceVideo < videos.Length)
        {
            return videos[indiceVideo];

        }
        else
        {
            indiceVideo = -1;
            return "End";
        }
    }
    internal string CurrentVideo()
    {
        if (indiceVideo < videos.Length)
        {
            return videos[indiceVideo];

        }
        else
        {
            return "End";
        }
    }

    internal string SelectVideo(int indice)
    {
        if (indice < videos.Length)
        {
            indiceVideo = indice;
            return videos[indiceVideo];
        }
        else
        {
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
