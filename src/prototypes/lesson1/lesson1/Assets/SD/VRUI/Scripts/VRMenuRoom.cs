using UnityEngine;
using System;
using System.Collections;
using VRStandardAssets.Utils;
using System.Collections.Generic;
using Assets.SD.VRMenuRoom.Scripts;
using UnityEngine.SceneManagement;
using System.Xml;


public class VRMenuRoom : MonoBehaviour {

    //[SerializeField] private SelectionSlider startControl;
	[SerializeField] private VRUIAnimationClick UI_Btn;
    [SerializeField] private VRCameraFade fader;
    [SerializeField] private AudioClip bgmMenu;
    [SerializeField] private GameObject[] dataControls;
    [SerializeField] private int lessonId;

    private string log = "";

    Util util = Util.Instance;

    // Use this for initialization
    void Start () {
		if(UI_Btn != null)
        {
			UI_Btn.OnAnimationComplete += HandleStartControl;
        }

        InitializeControls();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void InitializeControls()
    {
        for (int i = 0; i < dataControls.Length; i++)
        {
            IVRControl current = dataControls[i].GetComponent<IVRControl>();
            bool update = VRExperience.Instance.TryConfigurationValue(current.GetControlName());

            if (update)
            {
                current.SetControlValue(VRExperience.Instance.GetConfigurationValue<float>(current.GetControlName()), true);
            }
            else
            {
                current.SetControlValue(0.5f, true);
            }
        }
    }

    public IEnumerator OpenMenu()
    {
        yield return StartCoroutine(fader.BeginFadeIn(false));
    }

    public IEnumerator CloseMenu()
    {
        yield return StartCoroutine(fader.BeginFadeOut(false));
    }

    private IEnumerator StartExperience()
    {
        yield return StartCoroutine(CloseMenu());
        VRExperience.Instance.StartExperience(GetMenuSettings(), true);
    }

    private void HandleStartControl()
    {
		if (bgmMenu != null)
		{
			AudioSource source = gameObject.AddComponent<AudioSource>();
			source.clip = bgmMenu;
			//source.volume = 0.1f;
			//source.loop = true;
			source.Play();
		}
        StartCoroutine(StartExperience());
    }

    public Dictionary<string, object> GetMenuSettings()
    {

        Dictionary<string, object> settings=null;
        try
            {
            ConfigManager configManager = ConfigManager.Instance;
  
            if (configManager != null)
            {
                settings = configManager.Settings;
            
                if (settings.ContainsKey("lessonId"))
                {
                    settings.Remove("lessonId");
                }

                log = "Seteando lessonId=" + lessonId;
                settings.Add("lessonId", lessonId);

                util.ShowErrorPanel(log);
                
            }else{
                log += "ERROR. NO se puedo cargar la configuracion del sistema";
                util.ShowErrorPanel(log);
            }
           
        }
        catch (Exception e)
        {
            log += "Exception: " + e.Message+ "\n" +e.StackTrace;

            util.ShowErrorPanel(log);
        }
        return settings;
    }
}
