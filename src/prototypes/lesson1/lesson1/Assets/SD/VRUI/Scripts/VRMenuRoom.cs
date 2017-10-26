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

        //TextMesh textObject = GameObject.Find("log").GetComponent<TextMesh>();
        Dictionary<string, object> settings = new Dictionary<string, object>();
        string log = "";

        try
        {
            
            for (int i = 0; i < dataControls.Length; i++)
            {
                IVRControl current = dataControls[i].GetComponent<IVRControl>();
                settings.Add(current.GetControlName(), current.GetControlValue());
            }

            string matedataPath = "";
            string audioPath = "";
            string videosPath = "";
            string resourcesPath = Application.persistentDataPath;

          
            XmlDocument newXml = new XmlDocument();
            newXml.Load(resourcesPath + "/app-config.xml");

            log += "Cargando archivo: " + resourcesPath + "/app-config.xml \n";

            XmlNode root = newXml.DocumentElement;
            XmlNode nodeLesson = root.SelectSingleNode("//lesson-data[id="+ lessonId+"]");
           
            List<string> videos = new List<string>();

            foreach (XmlNode nodeChild in nodeLesson.ChildNodes)
            {
                log += nodeChild.Name+": "+nodeChild.InnerText + "\n";

                if (nodeChild.Name.Equals("metadataPath"))
                {
                    matedataPath = nodeChild.InnerText;
                }

                if (nodeChild.Name.Equals("audioPath"))
                {
                    audioPath = nodeChild.InnerText;
                }

                if (nodeChild.Name.Equals("videosPath"))
                {
                    videosPath = nodeChild.InnerText;
                }

                if (nodeChild.Name.Equals("videos"))
                {
                    XmlNodeList nodeListVideos = nodeChild.SelectNodes("name");
                    foreach (XmlNode nodeVideoName in nodeListVideos)
                    {
                        videos.Add(nodeVideoName.InnerText);
                        log += nodeVideoName.InnerText + "\n";
                    }
                }
            }

            log += "matedataPath: " + matedataPath + "\n";
            log += "audioPath: " + audioPath + "\n";
            log += "videosPath: " + videosPath + "\n";

            //textObject.text = log;
            settings.Add("resourcesPath", resourcesPath);
            settings.Add("matedataPath", matedataPath);
            settings.Add("audioPath", audioPath);
            settings.Add("videosPath", videosPath);
            settings.Add("videos", videos);

        }catch (Exception e)
        {
            log += "Exception: " + e.Message+ "\n" +e.StackTrace;
            //textObject.text = log;
        }
        return settings;
    }
}
