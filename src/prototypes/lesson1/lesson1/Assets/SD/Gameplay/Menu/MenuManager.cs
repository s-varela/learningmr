using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject errorPanel;

    private string log;
	// Use this for initialization
	private Util util = Util.Instance;

	void Start()
	{
		LoadConfigMenu();
	}

	private void LoadConfigMenu()
	{
		try
		{
			ConfigManager configManager = ConfigManager.Instance;

			Dictionary<string, object> settings = configManager.Settings;
			if (settings != null)
			{
                if (settings.ContainsKey("userConfig"))
				{

					UserConfigType userConfig = (UserConfigType)settings["userConfig"];
                    /* CARGO PANEL DE USUARIO*/

                  
                    if (GameObject.Find("UI_UserMenuFirstName").activeSelf)
					{
						GameObject.Find("UI_UserMenuFirstName").GetComponent<TextMesh>().text = userConfig.UserFirstName;
					}
					if (GameObject.Find("UI_UserMenuLanguage").activeSelf)
					{
						GameObject.Find("UI_UserMenuLanguage").GetComponent<TextMesh>().text = userConfig.UserLanguage;
					}

					/* CARGO BOTONES DEL MENU*/
				}

				if (settings.ContainsKey("gameObjectsTexts"))
				{

					Dictionary<string, string> gameObjectsTextsMap = (Dictionary<string, string>)settings["gameObjectsTexts"];

					GameObject.Find("UI_Btn1Text").GetComponent<TextMesh>().text = (string)gameObjectsTextsMap["UI_Btn1Text"];

					GameObject.Find("UI_Btn2Text").GetComponent<TextMesh>().text = (string)gameObjectsTextsMap["UI_Btn2Text"];

					GameObject.Find("UI_Btn3Text").GetComponent<TextMesh>().text = (string)gameObjectsTextsMap["UI_Btn3Text"];

					GameObject.Find("UI_Btn4Text").GetComponent<TextMesh>().text = (string)gameObjectsTextsMap["UI_Btn4Text"];

                    GameObject.Find("UI_UserMenuTitle").GetComponent<TextMesh>().text = (string)gameObjectsTextsMap["UI_UserMenuTitle"];
                    
                }
				else
				{
					log = "ERROR: La configuracion de los textos no está cargada en el sistema.\n";
                    util.ShowErrorPanelByRef(errorPanel, log);
                }
			}
			else
			{
				log = "ERROR: No se pudo cargar la configuracion del sistema \n";
                util.ShowErrorPanelByRef(errorPanel, log);
            }

		}
		catch (Exception e)
		{
			log = "Exception: " + e.Message + "\n" + e.StackTrace;
            util.ShowErrorPanelByRef(errorPanel, log);
        }
	}

	private void Awake()
	{
		//LoadConfigMenu();
	}

	// Update is called once per frame
	void Update()
	{

	}
}
