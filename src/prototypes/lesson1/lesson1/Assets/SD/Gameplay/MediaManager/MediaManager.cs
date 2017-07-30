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
    private SubReader subReader;
	private string pathVideos = "/lesson1-data/videos/";
   // [SerializeField] private string SubtitleText;

    //public TextMesh textObject;

    private AudioSource sfx;

    private int i = 0;

    private Stopwatch stopwatch;

    // Use this for initialization
    void Start ()
    {

        experience = VRExperience.Instance;

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

        if (data.audioAssetKey != null)
        {
            sfx = gameObject.AddComponent<AudioSource>();

            sfx.clip = Resources.Load<AudioClip>(data.audioAssetKey);
           // sfx.volume = experience.GetConfigurationValue<float>(data.audioVolumeConfigValue);
            sfx.loop = false;
            sfx.Play();
        }

        menu.OnMenuShow += PauseMedia;
        menu.OnMenuHide += ResumeMedia;

        media.OnEnd += ManagerVideo;
       // media.Play();

        ManagerVideo();

    }

    void Awake()
    {
        if (subReader==null)
        {
            subReader = new SubReader();
        }
        if (stopwatch == null)
        {
            stopwatch = new Stopwatch();
        }

        if (media == null)
        {
            media = FindObjectOfType<MediaPlayerCtrl>();

            if (media == null)
                throw new UnityException("No Media Player Ctrl object in scene");
        }

		//ManagerVideo();
    }


    // Update is called once per frame
    void Update ()
    {

        long seconds = stopwatch.ElapsedMilliseconds;
        // search if duration is in last subtitle second (in miliseconds)

        int duration = media.GetDuration();
        //textObject.text= subReader.ReadSubtitleLine(duration);

        //textObject.text = "Seg:"+seconds +" duracion:"+duration+ " sub: XXXX";
      // textObject.text = subReader.ReadSubtitleLine(duration);

        //i++;
        // textObject.text = i.ToString();

        /*if (duration >= 7000 && duration <= 8000)
            {
                Debug.Log("Hello, my name is Michael");
                PauseMedia();
                textObject.text = "Hello, my name is Michael";
                Thread.Sleep(2000);
                ResumeMedia();
            }
            else
            {
                Debug.Log("Sigue");
                textObject.text = "Sigue";
        }*/


        /* [0:00:04.100,0:00:07.000]-Hello, my name is Michael.
 [0:00:06.100, 0:00:08.000]-What's your name?
 [0:00:08.100,0:00:10.000]-My name is Johnny.
 [0:00:10.100, 0:00:12.000]-What's your name?*/

    }

    private void PauseMedia()
    {
        if (data.audioAssetKey != null)
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
        }

        media.Pause();
    }

    private void ResumeMedia()
    {
        if (data.audioAssetKey != null)
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
        }

        media.Play();
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

    private void ManagerVideo()
    {
		if (experience == null) {
			experience = VRExperience.Instance;
		}
        string videoName=experience.NextVideo();
		print("file://"+ Application.persistentDataPath+ pathVideos+ videoName);
        stopwatch.Reset();
        if (!videoName.Equals("End"))
        {
			media.Load("file://"+ Application.persistentDataPath+ pathVideos+ videoName);
            media.Play();
			stopwatch.Start();
        }
        else
        {
            FinishExperience();
        }
     
    }
}
