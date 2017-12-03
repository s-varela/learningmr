using System.Collections.Generic;
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

    public void ShowErrorPanelByRef(GameObject errorPanel,string msg)
    {
        errorPanel.SetActive(true);
        string msgFinal="";
        TextMesh textObject = GameObject.Find("UI_ErrorDialogText").GetComponent<TextMesh>();
        string[] splitMsg = msg.Split(' ');
        int i = 0;
        foreach (string msgPart in splitMsg)
        {
            if (i != 5)
            {
          
                i++;
                if (msgPart.Length > 35)
                {
                    msgFinal = msgFinal.Substring(0, 35) + "\n" + msgFinal.Substring(35, msgFinal.Length - 35);
                }else
                {
                    msgFinal += msgPart + " ";
                }
            }
            else
            {
                msgFinal += msgPart + " \n";
                i = 0;
            }
         
        }
        textObject.text = msgFinal;
    }

    public void ReLoadMenuTexts(GameObject errorPanel)
    {
        string log = "";
        ConfigManager configManager = ConfigManager.Instance;

        System.Collections.Generic.Dictionary<string, object> settings = configManager.Settings;
        if (settings != null)
        {
            if (settings.ContainsKey("userConfig"))
            {
                UserConfigType userConfig = (UserConfigType)settings["userConfig"];
                /* CARGO PANEL DE USUARIO

                if (GameObject.Find("UI_UserMenuFirstName").activeSelf)
                {
                    GameObject.Find("UI_UserMenuFirstName").GetComponent<TextMesh>().text = userConfig.UserFirstName;
                }
                if (GameObject.Find("UI_UserMenuLanguage").activeSelf)
                {
                    GameObject.Find("UI_UserMenuLanguage").GetComponent<TextMesh>().text = userConfig.UserLanguage;
                }
                */
                /* CARGO BOTONES DEL MENU*/
            }

            if (settings.ContainsKey("gameObjectsTexts"))
            {
                Dictionary<string, string> gameObjectsTextsMap = (Dictionary<string, string>)settings["gameObjectsTexts"];

                GameObject.Find("UI_Btn1Text").GetComponent<TextMesh>().text = (string)gameObjectsTextsMap["UI_Btn1Text"];
                GameObject.Find("UI_Btn2Text").GetComponent<TextMesh>().text = (string)gameObjectsTextsMap["UI_Btn2Text"];
                GameObject.Find("UI_Btn3Text").GetComponent<TextMesh>().text = (string)gameObjectsTextsMap["UI_Btn3Text"];
                GameObject.Find("UI_Btn4Text").GetComponent<TextMesh>().text = (string)gameObjectsTextsMap["UI_Btn4Text"];
                //GameObject.Find("UI_UserMenuTitle").GetComponent<TextMesh>().text = (string)gameObjectsTextsMap["UI_UserMenuTitle"];
            }
            else
            {
                log = "ERROR: La configuracion de los textos no está cargada en el sistema.\n";
                this.ShowErrorPanelByRef(errorPanel, log);
            }
        }
        else
        {
            log = "ERROR: No se pudo cargar la configuracion del sistema \n";
            this.ShowErrorPanelByRef(errorPanel, log);
        }
    }

}