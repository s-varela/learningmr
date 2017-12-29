using UnityEngine;
using System.Xml;
using System.Collections.Generic;
using System;

public class ConfigManager : MonoBehaviour
{
    [SerializeField] private GameObject errorPanel;

    private static ConfigManager instance;
    private Dictionary<string, object> settings;
    private string resourcesPath;
    private string log = "";
  


    private Util util = Util.Instance;

    public static ConfigManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ConfigManager>();
                //instance = new ConfigManager();
            }
            return instance;
        }
    }


    public Dictionary<string, object> Settings
    {
        get
        {
            if (settings == null)
            {
                IniConfigManager();
            }
            return this.settings;
        }
        set
        {
            this.settings = value;
        }
    }

    private void IniConfigManager()
    {
        settings = new Dictionary<string, object>();
        try
        {
            resourcesPath = Application.persistentDataPath + "/app-data";
          
            LoadUserInfo();
            LoadAppText();
            LoadAppSettings();

            //util.ShowErrorPanel(log);
        }
        catch (System.Exception e)
        {
            log = "Exception: [IniConfigManager] " + e.Message + "\n" + e.StackTrace;
            util.ShowErrorPanelByRef(errorPanel, log);

        }
    }

    public void Start()
    {
        if (settings == null) {
            IniConfigManager();
        }
    }

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else if (this != instance)
        {
            Destroy(gameObject);
        }
    }

    private void LoadUserInfo()
    {
        try
        {
            //log += "Ini LoadUserInfo";
            XmlDocument newXml = new XmlDocument();
            newXml.Load(resourcesPath + "/user-config.xml");

            //log += "Cargando archivo: " + resourcesPath + "/user-config.xml \n";

            XmlNode root = newXml.DocumentElement;
            XmlNode nodeUser = root.SelectSingleNode("//user");
            UserConfigType userConfig = new UserConfigType();

            foreach (XmlNode nodeChild in nodeUser.ChildNodes)
            {
                if (nodeChild.Name.Equals("firstName"))
                {
                    userConfig.UserFirstName = nodeChild.InnerText;
                    //log += "userFirstName: " + userConfig.UserFirstName + "\n";
                }

                if (nodeChild.Name.Equals("lastName"))
                {
                    userConfig.UserLastName = nodeChild.InnerText;
                    //log += "lastName: " + userConfig.UserLastName + "\n";
                }

                if (nodeChild.Name.Equals("email"))
                {
                    userConfig.UserEmail = nodeChild.InnerText;
                    //log += "email: " + userConfig.UserEmail + "\n";
                }

                if (nodeChild.Name.Equals("language"))
                {
                    userConfig.UserLanguage = nodeChild.InnerText;
                    //log += "language: " + userConfig.UserLanguage +"\n";
                }
            }
            Settings.Add("userConfig", userConfig);
            //log += "fin LoadUserInfo() \n ";
            //util.ShowErrorPanel(log);

        }catch (System.Exception ex)
        {
            log = "Excepcion: [LoadUserInfo] Error no se puedieron cargar los datos del usuario.\n Comprobar que el archivo user-config.xml exista en la carpeta de instalacion";
            util.ShowErrorPanelByRef(errorPanel, log);
        }
    }


    public void SaveUserConfig(UserConfigType userInfo)
    {
        try
        {
            //log = "Ini SaveUserConfig";
            //util.ShowErrorPanelByRef(errorPanel, log);

            XmlDocument newXml = new XmlDocument();
            newXml.Load(resourcesPath + "/user-config.xml");

            //log += "Cargando archivo: " + resourcesPath + "/user-config.xml \n";
            //util.ShowErrorPanelByRef(errorPanel, log);

            XmlNode root = newXml.DocumentElement;
            XmlNode nodeUser = root.SelectSingleNode("//user");

            foreach (XmlNode nodeChild in nodeUser.ChildNodes)
            {
                if (nodeChild.Name.Equals("firstName"))
                {
                    if (userInfo.UserFirstName != null)
                    {
                        nodeChild.InnerText= userInfo.UserFirstName;
                        //log += "userFirstName: " + userInfo.UserFirstName + "\n";
                        //util.ShowErrorPanelByRef(errorPanel, log);
                    }
                }

                if (nodeChild.Name.Equals("lastName"))
                {
                    if (userInfo.UserLastName != null)
                    {
                        nodeChild.InnerText = userInfo.UserLastName;
                        //log += "lastName: " + userInfo.UserLastName + "\n";
                        //util.ShowErrorPanelByRef(errorPanel, log);
                    }
                }

                if (nodeChild.Name.Equals("email"))
                {
                    if (userInfo.UserEmail != null)
                    {
                        nodeChild.InnerText= userInfo.UserEmail;
                        //log += "email: " + userInfo.UserEmail + "\n";
                    }
                }

                if (nodeChild.Name.Equals("language"))
                {
                    if (userInfo.UserLanguage != null)
                    {
                        nodeChild.InnerText= userInfo.UserLanguage;
                        //log += "language: " + userInfo.UserLanguage +"\n";
                        //util.ShowErrorPanelByRef(errorPanel, log);
                    }
                }
            }
 
            newXml.Save(resourcesPath + "/user-config.xml");

            IniConfigManager();

        }
        catch (System.Exception ex)
        {
            log = "Excepcion: [SaveUserConfig] Error no se puedieron actualizar los datos del usuario.";
            util.ShowErrorPanelByRef(errorPanel, log);
        }
    }

    internal void SaveLessonScore(UserQualificationType userQualification)
    {
        try
        {
            UnityEngine.Debug.Log("[ConfigManager][SaveLessonScore]. Inicio");
            XmlDocument scoreXml = new XmlDocument();
            scoreXml.Load(resourcesPath + "/lessons-score.xml");

            UnityEngine.Debug.Log("[ConfigManager][SaveLessonScore] Archivo lessons-score.xml cargado correctamente");

            XmlNode root = scoreXml.DocumentElement;
            // XmlNode nodeScore = root.SelectSingleNode("//lesson-score/lessonId[text()='" + userQualification.LessonId + "']");
            //XmlNode nodeScore = root.SelectSingleNode("//lesson-score");
            //XmlNodeList nodeScoreList = root.SelectNodes("//lesson-score");

            XmlNodeList nodeList = root.SelectNodes("descendant::lesson-score[lessonId='"+ userQualification.LessonId + "']");

            UnityEngine.Debug.Log("userQualification.LessonId="+ userQualification.LessonId);

            foreach (XmlNode nodeChild in nodeList)
            {
                if (nodeChild.Name.Equals("successCount"))
                {
                    nodeChild.InnerText = userQualification.SuccessCount.ToString();
                    UnityEngine.Debug.Log("userQualification.SuccessCount="+ userQualification.SuccessCount.ToString());
                }
                if (nodeChild.Name.Equals("skipCount"))
                {
                    nodeChild.InnerText = userQualification.SkipCount.ToString();
                    UnityEngine.Debug.Log("userQualification.SkipCount=" + userQualification.SkipCount.ToString());
                }
                if (nodeChild.Name.Equals("helpCount"))
                {
                    nodeChild.InnerText = userQualification.HelpCount.ToString();
                    UnityEngine.Debug.Log("userQualification.HelpCount=" + userQualification.HelpCount.ToString());
                }
                if (nodeChild.Name.Equals("repeatCount"))
                {
                    nodeChild.InnerText = userQualification.RepeatCount.ToString();
                    UnityEngine.Debug.Log("userQualification.RepeatCount=" + userQualification.RepeatCount.ToString());
                }
            }

            scoreXml.Save(resourcesPath + "/lessons-score.xml");
            UnityEngine.Debug.Log("[ConfigManager][SaveLessonScore] Archivo lessons-score.xml guardado correctamente");
            UnityEngine.Debug.Log("[ConfigManager][SaveLessonScore]. Fin");
        }
        catch (System.Exception ex)
        {
            log = "Excepcion: [SaveLessonScore] No se pudo guardar el puntaje del jugador. "+ex.Message;
            util.ShowErrorPanelByRef(errorPanel, log);
        }
    }

    private void LoadAppText()
    {
         Dictionary<string, string> gameObjectTextMap = new Dictionary<string, string>();
        //log += "Ini LoadAppText";
        try
        {
            if (Settings.ContainsKey("userConfig"))
            {

                //log += "Settings.ContainsKey(userConfig)=true";
                UserConfigType userConfig = (UserConfigType)Settings["userConfig"];

                XmlDocument newXml = new XmlDocument();
                newXml.Load(resourcesPath + "/app-text/" + userConfig.UserLanguage + "/app-text.xml");
                //log += "Cargando archivo: " + resourcesPath + "/app-text/" + userConfig.UserLanguage + "/app-text.xml \n";

                XmlNode root = newXml.DocumentElement;
                XmlNodeList gameObjectXmlList = root.SelectNodes("//gameobject");

                List<string> videos = new List<string>();

                foreach (XmlNode gameobject in gameObjectXmlList)
                {
                    string id = "";
                    string text = "";
                    foreach (XmlNode nodeChild in gameobject.ChildNodes)
                    {

                        if (nodeChild.Name.Equals("id"))
                        {
                            id = nodeChild.InnerText;
                        }
                        if (nodeChild.Name.Equals("text"))
                        {
                            text = nodeChild.InnerText;
                        }
                    }
                    //log += " id:" + id + " - " + text + "\n";
                    //util.ShowErrorPanel(log);
                    gameObjectTextMap.Add(id, text);
                }

                //util.ShowErrorPanel(log);
                Settings.Add("gameObjectsTexts", gameObjectTextMap);
            }
            else
            {
                log = "ERROR: La configuracion del usuario no está cargada en el sistema \n";
                util.ShowErrorPanelByRef(errorPanel, log);
            }
            //log += "FIN: LoadAppText \n";
            //util.ShowErrorPanel(log);

        }catch (System.Exception ex)
        {
            log = "Excepcion: [LoadAppText] No se puedieron cargar los textos de la aplicacion.\n Comprobar que el archivo app-text.xml exista en la carpeta de instalacion";
            util.ShowErrorPanelByRef(errorPanel, log);
        }
    }

    private void LoadAppSettings()
    {
        try
        {
            //log += "INI: LoadAppSettings \n";
      
            Dictionary<string, AppConfigType> appSettingtMap = new Dictionary<string, AppConfigType>();
            XmlDocument newXml = new XmlDocument();

            newXml.Load(resourcesPath + "/app-config.xml");

            //log += "Cargando archivo: " + resourcesPath + "/app-config.xml \n";

            XmlNode root = newXml.DocumentElement;
            XmlNodeList nodesLessonData = root.SelectNodes("//lesson-data");

            List<string> videos = new List<string>();

            foreach (XmlNode nodeLessonData in nodesLessonData)
            {
                AppConfigType appConfigType = new AppConfigType();
                appConfigType.ResourcesPath = resourcesPath;

                foreach (XmlNode nodeChild in nodeLessonData.ChildNodes)
                {
                    //log += nodeChild.Name + ": " + nodeChild.InnerText + "\n";
                    // util.ShowErrorPanel(log);

                    if (nodeChild.Name.Equals("id"))
                    {
                        appConfigType.Id = nodeChild.InnerText;
                        //log += "id: " + appConfigType.Id + "\n";
                    }

                    if (nodeChild.Name.Equals("metadataPath"))
                    {
                        appConfigType.MetedataPath = nodeChild.InnerText;
                        //log += "metadataPath: " + appConfigType.MetedataPath + "\n";
                    }

                    if (nodeChild.Name.Equals("audioPath"))
                    {
                        appConfigType.AudioPath = nodeChild.InnerText;
                        //log += "audioPath: " + appConfigType.AudioPath + "\n";
                    }

                    if (nodeChild.Name.Equals("videosPath"))
                    {
                        appConfigType.VideosPath = nodeChild.InnerText;
                        //log += "videosPath: " + appConfigType.VideosPath + "\n";
                    }

                    if (nodeChild.Name.Equals("videos"))
                    {
                        XmlNodeList nodeListVideos = nodeChild.SelectNodes("name");
                        foreach (XmlNode nodeVideoName in nodeListVideos)
                        {
                            appConfigType.Videos.Add(nodeVideoName.InnerText);
                            //log += nodeVideoName.InnerText + "\n";
                        }
                    }
                }

                appSettingtMap.Add(appConfigType.Id, appConfigType);
            }

            Settings.Add("appSettingtMap", appSettingtMap);

            //log += "Carga de configuracion de la aplicacion finalizada! \n";
            //util.ShowErrorPanelByRef(errorPanel, log);
        }
        catch (System.Exception ex)
        {
            log = "Excepcion: [LoadAppSettings] No se puedieron cargar las lecciones.\n Comprobar que el archivo app-config.xml exista en la carpeta de instalacion";
            util.ShowErrorPanelByRef(errorPanel, log);
        }
    }

 
}