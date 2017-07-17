using UnityEngine;

using System.Collections;

public class MediaManager : MonoBehaviour {

    private VRExperience experience = null;
    
    [SerializeField] private MediaPlayerCtrl media;
    [SerializeField] private AudioSource audioLeft;
    [SerializeField] private AudioSource audioRight;
    [SerializeField] private MediaManagerData data;
    [SerializeField] private VRGameMenu menu;
    [SerializeField] private SubReader SubReader;
    [SerializeField] private string SubtitleText;

    private AudioSource sfx;

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
        media.OnEnd += FinishExperience;

        //Martin
        //PlayMovieInCtrl(data.videoAssetKey);
     
	}

    void Awake()
    {
        if (media == null)
        {
            media = FindObjectOfType<MediaPlayerCtrl>();
            if (media == null)
                throw new UnityException("No Media Player Ctrl object in scene");
        }
    }

    public void PlayMovieInCtrl(string path)
    {

        // media = GetComponent<MediaPlayerCtrl>();
        media.Load(path);
      
            // GetComponent<Renderer>().material.mainTexture = media.GetVideoTexture();
        media.Play();
    }

    // Update is called once per frame
    void Update ()
    {
        // search if duration is in last subtitle second (in miliseconds)
        int duration = media.GetDuration();
        SubtitleText = SubReader.readSubtitleLine(duration);


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
        experience.BackToMainMenu();
    }
}
