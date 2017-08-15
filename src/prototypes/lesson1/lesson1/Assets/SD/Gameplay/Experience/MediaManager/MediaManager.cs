using UnityEngine;
using System.Collections;
using System.Threading;
using UnityEngine.UI;
using System.Diagnostics;
using System.IO;

public class MediaManager : MonoBehaviour {

    private VRExperience experience = null;
    
    [SerializeField] private MediaPlayerCtrl media;
    [SerializeField] private AudioSource audioLeft;
    [SerializeField] private AudioSource audioRight;
    [SerializeField] private MediaManagerData data;
    [SerializeField] private VRGameMenu menu;

    private SubtitleReader subReader;
    private AudioManager audioManager;
    private string pathVideos = "/lesson1-data/videos/";
   // [SerializeField] private string SubtitleText;

    private AudioSource sfx;
    private Stopwatch stopwatch;
	private bool changeSub;
	//[SerializeField] public GUIText theGuiText;
	[SerializeField] private Text theText;
	[SerializeField] private TextMesh normalText;

    // Use this for initialization
    void Start ()
    {
		audioLeft = new AudioSource ();
        experience = VRExperience.Instance;
		changeSub = false;

        if (audioLeft != null)
        {
            audioLeft.volume = experience.GetConfigurationValue<float>(data.videoVolumeConfigValue);
            audioLeft.Play();
        }

        if (audioRight != null)
        {
            audioRight.volume = experience.GetConfigurationValue<float>(data.videoVolumeConfigValue);
            audioRight.Play();
        }

     /*   if (data.audioAssetKey != null)
        {
            sfx = gameObject.AddComponent<AudioSource>();

            sfx.clip = Resources.Load<AudioClip>(data.audioAssetKey);
           // sfx.volume = experience.GetConfigurationValue<float>(data.audioVolumeConfigValue);
            sfx.loop = false;
            sfx.Play();
        }*/

        menu.OnMenuShow += PauseMedia;
        menu.OnMenuHide += ResumeMedia;

        media.OnEnd += FinishLessonPart;
		//media.OnEnd += ManagerVideo;
        ManagerVideo();

        }

    void Awake()
    {
        if (media == null)
        {
			experience = VRExperience.Instance;
            stopwatch = new Stopwatch();
            stopwatch.Start();
			subReader = new SubtitleReader();
            audioManager = new AudioManager();
            media = FindObjectOfType<MediaPlayerCtrl>();
            if (media == null)
                throw new UnityException("No Media Player Ctrl object in scene");
        }
    }


    // Update is called once per frame
    void Update()
    {
        try
        {
			long seconds = stopwatch.ElapsedMilliseconds;
			// search if duration is in last subtitle second (in miliseconds)
			string theSub = subReader.GetHashSubValue(subReader.ReadSubtitleLine (seconds));

			//Si la la frase es nueva pauso el video y reproduce la frase nuevamente
			if (!theSub.Equals ("") && theSub!= normalText.text) {
				//normalText=GetComponent<TextMesh>();
				normalText.text = theSub;
			} 
        }
        catch (System.Exception ex)
        {
            //TODO logger
            UnityEngine.Debug.Log(ex.Message);
            normalText.text = ex.Message;
        }
    }

    private void PauseMedia()
    {
      /*  if (data.audioAssetKey != null)
        {
            sfx.Pause();
        }

        if (audioLeft != null)
        {
            audioLeft.Pause();
        }

        if (audioRight)
        {
            audioRight.Pause();
        }*/

        media.Pause();
	stopwatch.Stop ();
    }

    private void ResumeMedia()
    {
       /* if (data.audioAssetKey != null)
        {
            sfx.UnPause();
        }

        if (audioLeft != null)
        {
            audioLeft.UnPause();
        }

        if (audioRight)
        {
            audioRight.UnPause();
        }*/

        media.Play();
	stopwatch.Start();
    }

    [System.Serializable]
    public struct MediaManagerData
    {
        [SerializeField] public string audioAssetKey;
        [SerializeField] public string audioVolumeConfigValue;
        [SerializeField] public string videoAssetKey;
        [SerializeField] public string videoVolumeConfigValue;
    }

    private void FinishExperience()
    {
        //Debug.Log("Llegue a FinishExperience");
        experience.BackToMainMenu();
    }

    private void FinishLessonPart()
    {

        try
        {
			PauseMedia();
            stopwatch.Stop();
            stopwatch.Reset();
            stopwatch.Start();
            bool finishLesson = false;
            string theSub;
            Hashtable hashSub = null;

            while (!finishLesson)
            {
                long seconds = stopwatch.ElapsedMilliseconds;
                hashSub = subReader.ReadSubtitleLine(seconds);
                theSub= subReader.GetHashSubValue(hashSub);
              
				if (!theSub.Equals("") && theSub != normalText.text)
                {
                    normalText.text = theSub;
                    stopwatch.Stop();
                    PlayAudio(theSub);
                    Wait(3.0f);
                    stopwatch.Start();
                }

                if (subReader.IsLastSubtitle(hashSub))
                {
                    finishLesson = true;
                }
            }
            Wait(2.0f);
            ManagerVideo();
        }
        catch (System.Exception ex)
        {
            //TODO logger
            UnityEngine.Debug.Log(ex.Message);
            normalText.text = ex.Message;
        }
    }

    private void ManagerVideo()
    {
        try
        {
            if (experience == null)
            {
                experience = VRExperience.Instance;
            }
            string videoName = experience.NextVideo();

            stopwatch.Reset();
            if (!videoName.Equals("End"))
            {
                subReader.RestFileReader(videoName);
                media.Load("file://" + Application.persistentDataPath + pathVideos + videoName);
                media.Play();
                stopwatch.Start();
            }
            else
            {
                FinishExperience();
            }
        }
        catch (System.Exception ex)
        {
            //TODO logger
            UnityEngine.Debug.Log(ex.Message);
            normalText.text = ex.Message;
        }

    }

	private void PlayAudio(string subtilte)
    {
        try
        {
            sfx = gameObject.AddComponent<AudioSource>();
            if (subtilte != null && subtilte != "")
            {
                string pathSounds = audioManager.getAudioPathName(subtilte);
                if (pathSounds != null)
                {
                    sfx.clip = Resources.Load<AudioClip>(pathSounds) as AudioClip;
                    //sfx.volume = experience.GetConfigurationValue<float>(data.audioVolumeConfigValue);
                    sfx.loop = false;
                    sfx.volume = 1.0f;
                    sfx.ignoreListenerPause = true;
                    sfx.enabled = false;
                    sfx.enabled = true;
                    sfx.Play();
                }
            }
        }
        catch (System.Exception ex)
        {
            //TODO logger
            UnityEngine.Debug.Log(ex.Message);
            normalText.text = ex.Message;
        }
    }

	private void Wait (float waitTime)
	{
		float time = Time.realtimeSinceStartup;

		while (Time.realtimeSinceStartup - time <= waitTime);
	}
}
