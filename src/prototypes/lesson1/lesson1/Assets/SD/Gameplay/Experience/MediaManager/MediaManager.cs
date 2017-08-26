using UnityEngine;
using System.Collections;
using System.Threading;
using UnityEngine.UI;
using System.Diagnostics;
using System.IO;

public class MediaManager : MonoBehaviour
{

    private VRExperience experience = null;

    [SerializeField] private MediaPlayerCtrl media;
    [SerializeField] private AudioSource audioLeft;
    [SerializeField] private AudioSource audioRight;
    [SerializeField] private MediaManagerData data;
    [SerializeField] private VRGameMenu menu;

    //[SerializeField] private LoadSubtitlePanel loadSubtitlePanel;
    [SerializeField] private LoadPanel loadPanel;

    [SerializeField] GameObject panelExt;
    [SerializeField] GameObject textInfo;
    private MeshRenderer meshPanel;
    private MeshRenderer meshTextInfo;
    private MeshRenderer meshPanelInteraction;

    private SubtitleReader subReader;
    private AudioManager audioManager;
    private string pathVideos = "/lesson1-data/videos/";
    //private ArrayList arrSubtitles = new ArrayList();
    private string[] arrSubtitles;

    private AudioSource sfx;
    private Stopwatch counterVideo;
    private Stopwatch counterAudio;
    private bool changeSub;
    private bool showUserInput;
    private bool pause;
    //[SerializeField] public GUIText theGuiText;
    //[SerializeField] private Text theText;
    [SerializeField] private TextMesh normalText;

    private int indiceAudio;

    // Use this for initialization

    void Start()
    {
        audioLeft = new AudioSource();
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

        //media.OnEnd += FinishLessonPart;
        //media.OnEnd += ManagerVideo;
        ManagerVideo();

    }

    void Awake()
    {
        if (media == null)
        {
            experience = VRExperience.Instance;
            counterVideo = new Stopwatch();
            counterVideo.Start();
            counterAudio = new Stopwatch();
            subReader = new SubtitleReader();
            audioManager = new AudioManager();
            media = FindObjectOfType<MediaPlayerCtrl>();
            if (media == null)
                throw new UnityException("No Media Player Ctrl object in scene");

            pause = false;
            showUserInput = false;
            indiceAudio = 0;
        }
    }


    // Update is called once per frame
    void Update()
    {
        try
        {
      
            if (!pause && !showUserInput)
            {
                long seconds = counterVideo.ElapsedMilliseconds;
                // search if duration is in last subtitle second (in miliseconds)

                DialogType dialogType = subReader.ReadSubtitleLine(seconds);

                if (dialogType != null)
                {
                    string theSub = dialogType.Text;

                    //Si la la frase es nueva pauso el video y reproduce la frase nuevamente
                    if (!theSub.Equals("") && theSub != normalText.text)
                    {
                        normalText.text = theSub;
                    }
                    if (dialogType.Pause)
                    {
                        //arrSubtitles = loadPanel.ArrayText();
                        ActiveObject();
                        pause = true;
                        counterAudio.Start();
                        //FinishLessonPart();

                    }
                    else if (dialogType.RequiredInput)
                    {
                        showUserInput = true;
                        normalText.text = " INPUT USUARIO ....";
                        Wait(5.0f);
                    }

                }
            }
            else if (pause)//se termino la sub leccion, muestro panel frontal y reproduzco audios
            {
                PauseMedia();
                counterVideo.Stop();
                long counter = counterAudio.ElapsedMilliseconds;
                if (counter >= 4000) //Espero x tiempo
                {
                    arrSubtitles = loadPanel.ArrayText();
                    if (indiceAudio < arrSubtitles.Length)
                    {
                      
                        string sub = arrSubtitles[indiceAudio];
                        
                        PlayAudio(sub);

                        counterAudio.Stop();
                        counterAudio.Reset();
                        counterAudio.Start();
						indiceAudio++;
                    }
                    else { 
                        indiceAudio = 0; //reseteo el contador
                        counterAudio.Stop();
                        counterAudio.Reset();
                        counterAudio.Start();
                        pause = false;
                        FinishLessonPart();
                    }
                }
            } else if (showUserInput)
            {
                //mostrar panel interaccion
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
        counterVideo.Stop();
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
        counterVideo.Start();
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


    private void FinishLessonPart()
    {
		pause = false;
        loadPanel.DeleteSub();
        DesactiveObject();
        counterVideo.Reset();
        counterVideo.Stop();
        counterVideo.Start();
        ManagerVideo();
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

            counterVideo.Reset();
            if (!videoName.Equals("End"))
            {
                subReader.RestFileReader(videoName);
                media.Load("file://" + Application.persistentDataPath + pathVideos + videoName);
                media.Play();
                counterVideo.Start();
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
                else
                {
                    UnityEngine.Debug.Log("Error en la reproduccion del audio. Audio no encontrado. texto: "+subtilte);
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

    private void Wait(float waitTime)
    {
        float time = Time.realtimeSinceStartup;

        while (Time.realtimeSinceStartup - time <= waitTime) ;
    }

    public void ActiveObject()
    {
        ActiveMeshRenderer(meshPanel, panelExt, true);
        ActiveMeshRenderer(meshTextInfo, textInfo, true);
    }

    public void DesactiveObject()
    {
        ActiveMeshRenderer(meshPanel, panelExt, false);
        ActiveMeshRenderer(meshTextInfo, textInfo, false);
    }

    private void ActiveMeshRenderer(MeshRenderer mesh, GameObject gameObj, bool v)
    {
        mesh = gameObj.GetComponent<MeshRenderer>();
        mesh.enabled = v;
    }
}
