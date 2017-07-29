using UnityEngine;
using System.Threading;
using UnityEngine.UI;
using System.Diagnostics;

public class MediaManager : MonoBehaviour {

    private VRExperience experience = null;
    
    [SerializeField] private MediaPlayerCtrl media;
    [SerializeField] private AudioSource audioLeft;
    [SerializeField] private AudioSource audioRight;
    [SerializeField] private MediaManagerData data;
    [SerializeField] private VRGameMenu menu;
    private SubtitleReader subReader;
    //[SerializeField] private string SubtitleText;

    private AudioSource sfx;
    private Stopwatch stopwatch;
	[SerializeField] public GUIText theGuiText;

    // Use this for initialization
    void Start ()
    {
		audioLeft = new AudioSource ();
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
        media.Play();
    }

    void Awake()
    {
        if (media == null)
        {
            stopwatch = new Stopwatch();
            stopwatch.Start();
            subReader = new SubtitleReader();
            media = FindObjectOfType<MediaPlayerCtrl>();
            if (media == null)
                throw new UnityException("No Media Player Ctrl object in scene");
        }
    }


    // Update is called once per frame
    void Update ()
    {

        int seconds = (int)stopwatch.ElapsedMilliseconds;
        // search if duration is in last subtitle second (in miliseconds)
        // PauseMedia();
		UnityEngine.Debug.Log("seconds: " + seconds);

		theGuiText.text = subReader.ReadSubtitleLine(seconds);

	/*	if (!theGuiText.text.Equals ("")) {
			PauseMedia ();
			//TODO: reproducir audio de subtitulo
			Thread.Sleep(2000);
			ResumeMedia();
		}*/

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
        string videoName=experience.NextVideo();
        stopwatch.Reset();
        if (!videoName.Equals("End"))
        {
            media.UnLoad();
            media.Load(videoName);
            media.Play();
        }
        else
        {
            FinishExperience();
        }
     
    }
}
