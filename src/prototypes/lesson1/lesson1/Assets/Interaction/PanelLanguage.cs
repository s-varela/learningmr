using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelLanguage : MonoBehaviour
{
    [SerializeField] private VRUIAnimationClick flag1;
    [SerializeField] private VRUIAnimationClick flag2;

    [SerializeField] private Material MatFlag1;
    [SerializeField] private Material MatFlag1BW;
    [SerializeField] private Material MatFlag2;
    [SerializeField] private Material MatFlag2BW;

    [SerializeField] private GameObject errorPanel;
    private string log;
    // Use this for initialization
    private Util util = Util.Instance;

    // Use this for initialization
    void Start()
    {
        if (flag1 != null)
        {
            flag1.OnAnimationComplete += Flag1OnClik;
        }
        if (flag2 != null)
        {
            flag2.OnAnimationComplete += Flag2OnClik;
        }
        ConfigManager configManager = ConfigManager.Instance;
        Dictionary<string, object> settings = configManager.Settings;
        if (settings != null)
        {
            if (settings.ContainsKey("userConfig"))
            {
                UserConfigType userConfig = (UserConfigType)settings["userConfig"];
                if (userConfig.UserLanguage == "esp")
                {
                    GameObject.Find("UI_Lan1").GetComponent<Renderer>().material = MatFlag1;
                    GameObject.Find("UI_Lan2").GetComponent<Renderer>().material = MatFlag2BW;

                }
                else if (userConfig.UserLanguage == "fre")
                {
                    GameObject.Find("UI_Lan1").GetComponent<Renderer>().material = MatFlag1BW;
                    GameObject.Find("UI_Lan2").GetComponent<Renderer>().material = MatFlag2;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void Flag1OnClik()
    {
        try
        {
            GameObject.Find("UI_Lan1").GetComponent<Renderer>().material = MatFlag1;
            GameObject.Find("UI_Lan2").GetComponent<Renderer>().material = MatFlag2BW;

            ConfigManager configManager = ConfigManager.Instance;
            Dictionary<string, object> settings = configManager.Settings;
            if (settings != null)
            {
                if (settings.ContainsKey("userConfig"))
                {
                    UserConfigType userConfig = (UserConfigType)settings["userConfig"];
                    userConfig.UserLanguage = "esp";
                    configManager.SaveUserConfig(userConfig);
                    util.ReLoadMenuTexts(errorPanel);
                }
            }
        }
        catch (System.Exception e)
        {
            log = "Exception: " + e.Message + "\n" + e.StackTrace;
            util.ShowErrorPanelByRef(errorPanel, log);
        }
    }

    private void Flag2OnClik()
    {
        try
        {
            GameObject.Find("UI_Lan1").GetComponent<Renderer>().material = MatFlag1BW;
            GameObject.Find("UI_Lan2").GetComponent<Renderer>().material = MatFlag2;

            ConfigManager configManager = ConfigManager.Instance;
            Dictionary<string, object> settings = configManager.Settings;
            if (settings != null)
            {
                if (settings.ContainsKey("userConfig"))
                {
                    UserConfigType userConfig = (UserConfigType)settings["userConfig"];
                    userConfig.UserLanguage = "fre";
                    configManager.SaveUserConfig(userConfig);
                    util.ReLoadMenuTexts(errorPanel);
                }
            }
        }
        catch (System.Exception e)
        {
            log = "Exception: " + e.Message + "\n" + e.StackTrace;
            util.ShowErrorPanelByRef(errorPanel, log);
        }
    }
}
