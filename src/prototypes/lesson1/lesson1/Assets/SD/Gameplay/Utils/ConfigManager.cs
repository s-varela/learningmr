using UnityEngine;
using System.Xml;
using System.Collections.Generic;

public class ConfigManager : MonoBehaviour
{
    private static ConfigManager instance;
    private Dictionary<string, object> settings;
    public string resourcesPath;
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

    /*private ConfigManager()
    {
        log += "Config Manager. Ini Start() : ";
        IniConfigManager();
        log += "Config Manager. Fin Start() : ";

        util.ShowErrorPanel(log);
    }*/

    private void IniConfigManager()
    {
        settings = new Dictionary<string, object>();
        try
        {
            resourcesPath = Application.persistentDataPath + "/app-data";
          
            log += "Ini LoadUserInfo \n";
            LoadUserInfo();
            log += "Ini LoadAppText \n";
            LoadAppText();
            log += "Ini LoadAppSetting \n";
            LoadAppSettings();
            util.ShowErrorPanel(log);
        }
        catch (System.Exception e)
        {
            log += "Exception: " + e.Message + "\n" + e.StackTrace;
            util.ShowErrorPanel(log);
        }

    }

    public void Start()
    {
        if (settings == null) {
            log += "Config Manager. Ini Start() : \n";
            IniConfigManager();
            log += "Config Manager. Fin Start() : \n";
            util.ShowErrorPanel(log);
        }
    }

   /* public void Awake()
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
    }*/


    private void LoadUserInfo()
    {
        log += "Ini LoadUserInfo";
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
        log += "fin LoadUserInfo() \n ";
        util.ShowErrorPanel(log);

    }

    private void LoadAppText()
    {
         Dictionary<string, string> gameObjectTextMap = new Dictionary<string, string>();
        log += "Ini LoadAppText";
        if (Settings.ContainsKey("userConfig"))
        {

            log += "Settings.ContainsKey(userConfig)=true";
            UserConfigType userConfig = (UserConfigType)Settings["userConfig"];

            XmlDocument newXml = new XmlDocument();
            newXml.Load(resourcesPath + "/app-text/" + userConfig.UserLanguage + "/app-text.xml");
            //log += "Cargando archivo: " + resourcesPath + "/app-text/" + userConfig.UserLanguage + "/app-text.xml \n";
            util.ShowErrorPanel(log);

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
        else {
            log += "ERROR: La configuracion del usuario no está cargada en el sistema \n";
            //util.ShowErrorPanel(log);
        }
        log += "FIN: LoadAppText \n";
        util.ShowErrorPanel(log);
    }

    private void LoadAppSettings()
    {
        log += "INI: LoadAppSettings \n";
        util.ShowErrorPanel(log);

        Dictionary<string, AppConfigType> appSettingtMap = new Dictionary<string, AppConfigType>();
        XmlDocument newXml = new XmlDocument();

        newXml.Load(resourcesPath + "/app-config.xml");

        log += "Cargando archivo: " + resourcesPath + "/app-config.xml \n";

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
                    log += "audioPath: " + appConfigType.AudioPath + "\n";
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

        log += "Carga de configuracion de la aplicacion finalizada! \n";
        util.ShowErrorPanel(log);

    }

}