﻿using UnityEngine;
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
	private string pathVideos = "/lesson1-data/videos/";
   // [SerializeField] private string SubtitleText;

    private AudioSource sfx;
    private Stopwatch stopwatch;
	//[SerializeField] public GUIText theGuiText;
	[SerializeField] private Text theText;
	[SerializeField] private TextMesh normalText;

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
            media = FindObjectOfType<MediaPlayerCtrl>();
            if (media == null)
                throw new UnityException("No Media Player Ctrl object in scene");
        }
    }


    // Update is called once per frame
    void Update()
	{
		long seconds = stopwatch.ElapsedMilliseconds;
		// search if duration is in last subtitle second (in miliseconds)
		string theSub = subReader.ReadSubtitleLine (seconds);
		//theGuiText.text = theSub;
		normalText.text = theSub;

		/*if (!normalText.text.Equals ("")) {
			PauseMedia ();
			//TODO: reproducir audio de subtitulo
			Thread.Sleep (2000);
			ResumeMedia ();
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

    /*private void ManagerAudio(string audioName)
    {
        sfx = gameObject.AddComponent<AudioSource>();

        sfx.clip = Resources.Load<AudioClip>(experience.GetAudioAssetKey(audioName));
        // sfx.volume = experience.GetConfigurationValue<float>(data.audioVolumeConfigValue);
        sfx.loop = false;
        sfx.Play();
    }*/
}
