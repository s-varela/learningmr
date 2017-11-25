using UnityEngine;

public class Util :MonoBehaviour
{
    private static Util instance;

    public static Util Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new Util();
            }
            return instance;
        }
    }


    public void ShowErrorPanel(string msg)
    {
        GameObject panelErrorObj = GameObject.Find("ErrorMenu");
        TextMesh textObject = GameObject.Find("UI_ErrorDialogText").GetComponent<TextMesh>();
        textObject.text = msg;
    }


    public void ShowConfig(System.Collections.Generic.Dictionary<string, object> config)
    {
        string log ="** Show config ** \n \n";
        try
        {
            foreach (var key in config.Keys)
            {

                log += key + "\n";

                /* object obj = config[value];
                 if (obj.GetType() == typeof(UserConfigType))
                 {
                     UserConfigType user = (UserConfigType)obj;
                     log += "( user.UserFirstName," + user.UserFirstName + " ) \n";
                     log += "( user.UserLastName," + user.UserLastName + " ) \n";
                     log += "( user.UserLanguage," + user.UserLanguage + " ) \n";
                     log += "( user.UserEmail," + user.UserEmail + " ) \n";
                 }else if (obj.GetType() == typeof(AppConfigType))
                 {
                     log += "( MetedataPath," + ((AppConfigType)obj).MetedataPath + " ) \n";
                     log += "( ResourcesPath," + ((AppConfigType)obj).ResourcesPath + " ) \n";
                     log += "( VideosPath," + ((AppConfigType)obj).VideosPath + " ) \n";
                 }else 
                     log += "( " + value + ","+ (string)config[value]+ " ) \n";*/
            }

            ShowErrorPanel(log);

        }catch (System.Exception e)
        {
            ShowErrorPanel("Exception: "+e.Message);
        }
    }
}