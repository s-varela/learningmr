using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelLanguage : MonoBehaviour
{

    [SerializeField] private VRUIAnimationClick flag1;
    [SerializeField] private VRUIAnimationClick flag2;
    [SerializeField] private VRUIAnimationClick flag3;

    [SerializeField] private Material MatFlag1;
    [SerializeField] private Material MatFlag1BW;
    [SerializeField] private Material MatFlag2;
    [SerializeField] private Material MatFlag2BW;
    [SerializeField] private Material MatFlag3;
    [SerializeField] private Material MatFlag3BW;

    [SerializeField] private GameObject errorPanel;
    private string log;
    // Use this for initialization
    private Util util = Util.Instance;

	private static PanelLanguage instance;

	public static PanelLanguage Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new PanelLanguage();
			}
			return instance;
		}
	}

    // Use this for initialization
    public void Start()
    {
        if (flag1 != null)
        {
            flag1.OnAnimationComplete += Flag1OnClik;
        }
        if (flag2 != null)
        {
            flag2.OnAnimationComplete += Flag2OnClik;
        }

        if (flag3 != null)
        {
            flag3.OnAnimationComplete += Flag3OnClik;
        }

        ConfigManager configManager = ConfigManager.Instance;
        Dictionary<string, object> settings = configManager.Settings;
        if (settings != null)
        {
            if (settings.ContainsKey("userConfig"))
            {
                UserConfigType userConfig = (UserConfigType)settings["userConfig"];
				if (userConfig.UserLanguage.Equals("esp"))
                {
                    GameObject.Find("UI_Lan1").GetComponent<Renderer>().material = MatFlag1;
                    GameObject.Find("UI_Lan2").GetComponent<Renderer>().material = MatFlag2BW;
                    GameObject.Find("UI_Lan3").GetComponent<Renderer>().material = MatFlag3BW;

                }
				else if (userConfig.UserLanguage.Equals("fre"))
                {
                    GameObject.Find("UI_Lan1").GetComponent<Renderer>().material = MatFlag1BW;
                    GameObject.Find("UI_Lan2").GetComponent<Renderer>().material = MatFlag2;
                    GameObject.Find("UI_Lan3").GetComponent<Renderer>().material = MatFlag3BW;
                }
				else if (userConfig.UserLanguage.Equals("ita"))
                {
                    GameObject.Find("UI_Lan1").GetComponent<Renderer>().material = MatFlag1BW;
                    GameObject.Find("UI_Lan2").GetComponent<Renderer>().material = MatFlag2BW;
                    GameObject.Find("UI_Lan3").GetComponent<Renderer>().material = MatFlag3;
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
            GameObject.Find("UI_Lan3").GetComponent<Renderer>().material = MatFlag3BW;

            ConfigManager configManager = ConfigManager.Instance;
            Dictionary<string, object> settings = configManager.Settings;
            if (settings != null)
            {
                if (settings.ContainsKey("userConfig"))
                {
                    UserConfigType userConfig = (UserConfigType)settings["userConfig"];
                    userConfig.UserLanguage = "esp";
                    configManager.SaveUserConfig(userConfig);
                    util.ReLoadMenuTexts(errorPanel, false);
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
            GameObject.Find("UI_Lan3").GetComponent<Renderer>().material = MatFlag3BW;

            ConfigManager configManager = ConfigManager.Instance;
            Dictionary<string, object> settings = configManager.Settings;
            if (settings != null)
            {
                if (settings.ContainsKey("userConfig"))
                {
                    UserConfigType userConfig = (UserConfigType)settings["userConfig"];
                    userConfig.UserLanguage = "fre";
                    configManager.SaveUserConfig(userConfig);
					util.ReLoadMenuTexts(errorPanel, false);
                }
            }
        }
        catch (System.Exception e)
        {
            log = "Exception: " + e.Message + "\n" + e.StackTrace;
            util.ShowErrorPanelByRef(errorPanel, log);
        }
    }

    private void Flag3OnClik()
    {
        try
        {
            GameObject.Find("UI_Lan1").GetComponent<Renderer>().material = MatFlag1BW;
            GameObject.Find("UI_Lan2").GetComponent<Renderer>().material = MatFlag2BW;
            GameObject.Find("UI_Lan3").GetComponent<Renderer>().material = MatFlag3;
            ConfigManager configManager = ConfigManager.Instance;
            Dictionary<string, object> settings = configManager.Settings;
            if (settings != null)
            {
                if (settings.ContainsKey("userConfig"))
                {
                    UserConfigType userConfig = (UserConfigType)settings["userConfig"];
                    userConfig.UserLanguage = "ita";
                    configManager.SaveUserConfig(userConfig);
					util.ReLoadMenuTexts(errorPanel, false);
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
