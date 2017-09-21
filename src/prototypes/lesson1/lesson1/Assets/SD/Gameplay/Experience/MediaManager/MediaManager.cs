using UnityEngine;
using System.Collections;
using System.Threading;
using UnityEngine.UI;
using System.Diagnostics;
using Assets.AnswersLogic;
using System.IO;


public class MediaManager : MonoBehaviour {

    private VRExperience experience = null;

    [SerializeField] private MediaPlayerCtrl media;
    [SerializeField] private AudioSource audioLeft;
    [SerializeField] private AudioSource audioRight;
    [SerializeField] private MediaManagerData data;
	[SerializeField] private VRGameMenu menu;

	[SerializeField] private LoadPanel loadPanel;

    [SerializeField] GameObject panelExt;
	[SerializeField] GameObject textInfo;
	[SerializeField] GameObject panelSub;
	[SerializeField] GameObject panelInput;
    [SerializeField] GameObject panelQuestion;
    [SerializeField] GameObject panelAnswer;
    [SerializeField] GameObject panelHintButton;
    [SerializeField] GameObject panelHintText;
    [SerializeField] GameObject hintButton;
    [SerializeField] GameObject skipButton;
    [SerializeField] GameObject sphere;
	[SerializeField] GameObject keyboard;
    [SerializeField] Text keyboardInp;
	[SerializeField] TextMesh Sub;
    [SerializeField] TextMesh userAnswer;
    [SerializeField] TextMesh givenHint;
	[SerializeField] GameObject gifTick;
	[SerializeField] GameObject gifCross;


    [SerializeField] NavigationPanel navigationPanel;

    private SubtitleReader subReader;
    private AudioManager audioManager;
    private ProcessAnswer processAnswer;
    private string pathVideos = "/lesson1-data/videos/";
    //private ArrayList arrSubtitles = new ArrayList();
    private string[] arrSubtitles;
	private string[] arrayText;
	private Color originalColor;

    private AudioSource sfx;
    private Stopwatch counterVideo;
    private Stopwatch counterAudio;
	private Stopwatch counterDelay;
    private bool changeSub;
    private bool showUserInput;
    private bool pause;
    private bool finish;
    //[SerializeField] public GUIText theGuiText;
    //[SerializeField] private Text theText;
    [SerializeField] private TextMesh normalText;
	private bool answerOK = false;

    private int indiceAudio;
    private DialogType dialogType;
	private int i=-1;
    // Use this for initialization

    void Start()
    {
        audioLeft = new AudioSource();
        experience = VRExperience.Instance;
        dialogType = new DialogType();
        changeSub = false;
		experience.ResetIndice ();
		originalColor = Sub.color;

		//Inicializar variables
		InitializeVariables();

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

		if (audioRight != null) {
			audioRight.volume = experience.GetConfigurationValue<float> (data.videoVolumeConfigValue);
			audioRight.Play ();
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
        ManagerVideo();

    }

	void InitializeVariables()
	{
		changeSub = false;
		answerOK = false;
		showUserInput = false;
		pause = false;
		DesactiveObject(panelExt);
		DesactiveObject(textInfo);
		panelSub.SetActive(true);
		panelInput.SetActive(false);
		sphere.SetActive(false);
		keyboard.SetActive(false);
		panelAnswer.SetActive(false);
		panelQuestion.SetActive(false);
		panelHintButton.SetActive(false);
		panelHintText.SetActive(false);
		loadPanel.DeleteSub();
		Sub.text="";
		normalText.text = "";
		userAnswer.text = "";
		givenHint.text = "";
		indiceAudio = 0;
	}

    void Awake()
    {
        if (media == null)
        {
            experience = VRExperience.Instance;
            counterVideo = new Stopwatch();
            counterVideo.Start();
            counterAudio = new Stopwatch();
			counterDelay = new Stopwatch ();
            subReader = new SubtitleReader();
            audioManager = new AudioManager();
            processAnswer = new ProcessAnswer();
            dialogType = new DialogType();
            media = FindObjectOfType<MediaPlayerCtrl>();
            if (media == null)
                throw new UnityException("No Media Player Ctrl object in scene");

            pause = false;
            showUserInput = false;
            finish=false;
            indiceAudio = 0;
            dialogType = new DialogType();
        }
    }


    // Update is called once per frame
    void Update()
    {
        try
        {
      
            if (!pause && !showUserInput && !finish)
            {
                long seconds = counterVideo.ElapsedMilliseconds;
                // search if duration is in last subtitle second (in miliseconds)

                dialogType = subReader.ReadSubtitleLine(seconds);

                if (dialogType != null)
                {
                    string theSub = dialogType.Text;

                    //Si la la frase es nueva pauso el video y reproduce la frase nuevamente
                    if (!theSub.Equals("") && theSub != normalText.text)
                    {
                        normalText.text = theSub;
						answerOK = false;
						Sub.color = originalColor;
						gifTick.SetActive (false);
                    }
                    if (dialogType.Pause)
                    {
                        //arrSubtitles = loadPanel.ArrayText();

                        pause = true;
                        counterAudio.Start();

						counterDelay.Reset();
						counterDelay.Start();
                        //FinishLessonPart();

                    }
					else if (dialogType.RequiredInput && !answerOK)
                    {
						i=-1;
                        showUserInput = true;
						pause = false;
						panelInput.SetActive(true);
                        panelAnswer.SetActive(true);
                        panelQuestion.SetActive(true);
                        panelHintButton.SetActive(true);
                        panelHintText.SetActive(true);
						panelSub.SetActive(false);
                        PauseMedia();

						counterDelay.Reset();
						counterDelay.Start();
                        //normalText.text = " INPUT USUARIO ....";
                        //Wait(5.0f);
                    }else if (dialogType.Finish)
                    {
                        finish = true;
                        pause = false;
                        counterDelay.Reset();
                        counterDelay.Start();
                    }

                }
            }
			else if (pause && counterDelay.ElapsedMilliseconds>2000)//se termino la sub leccion, muestro panel frontal y reproduzco audios
            {
                PauseMedia();
				ActiveObject(panelExt);
				ActiveObject(textInfo);
				sphere.SetActive(true);

				panelSub.SetActive(false);
                counterVideo.Stop();

                long counter = counterAudio.ElapsedMilliseconds;
                if (counter >= 4000) //Espero x tiempo
                {
                    arrSubtitles = loadPanel.ArrayText();
                    if (indiceAudio < arrSubtitles.Length)
                    {

                        string sub = arrSubtitles[indiceAudio];

                        PlayAudio(sub);
						if(indiceAudio == 0)
						{
							arrayText = loadPanel.ArrayText();
						}
						loadPanel.colorSub(indiceAudio, arrayText);

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
				sphere.SetActive(true);
                //mostrar panel interaccion
            } else if (dialogType.Finish && counterDelay.ElapsedMilliseconds > 2000)
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

	public void KeyboardExitButton(){
        keyboardInp.text = "";
        keyboard.SetActive(false);
		panelSub.SetActive(false);
		panelInput.SetActive(true);
        panelAnswer.SetActive(true);
        panelQuestion.SetActive(true);
        panelHintButton.SetActive(true);
        hintButton.SetActive(true);
        skipButton.SetActive(true);
        panelHintText.SetActive(true);
		gifCross.SetActive (false);
		gifTick.SetActive (false);
    }

	public void KeyboardOKButton(){
		string answer = keyboardInp.text;
		validateAnswer (answer);
        panelSub.SetActive(false);
        panelInput.SetActive(true);
        panelAnswer.SetActive(true);
        panelQuestion.SetActive(true);
        panelHintButton.SetActive(true);
        panelHintText.SetActive(true);
        hintButton.SetActive(true);
        skipButton.SetActive(true);
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
        counterVideo.Reset();
        counterVideo.Stop();
        counterVideo.Start();
		DesactiveObject(panelExt);
		DesactiveObject(textInfo);
		sphere.SetActive(false);
		panelSub.SetActive(true);
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
				if(!videoName.Equals("Error"))
				{
                    int videoNumber = experience.GetIndice();
                    if (videoNumber == 4)
                    {
                        panelSub.SetActive(false);
                        panelAnswer.SetActive(true);
                        panelQuestion.SetActive(true);
                        panelHintButton.SetActive(true);
                        panelHintText.SetActive(true);
                    } else {
                        panelSub.SetActive(true);
                    }
					navigationPanel.materialOriginal();
					navigationPanel.colorPart();
					Sub.text = "";
					normalText.text = "";
                	subReader.RestFileReader(videoName);
                	media.Load("file://" + Application.persistentDataPath + pathVideos + videoName);
                	media.Play();
                	counterVideo.Start();
				}
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

	private void Wait (float waitTime)
	{
		float time = Time.realtimeSinceStartup;

		while (Time.realtimeSinceStartup - time <= waitTime);
	}

	public void ActiveObject(GameObject gameObj)
	{
		ActiveMeshRenderer(gameObj, true);
	}

	public void DesactiveObject(GameObject gameObj)
	{
		ActiveMeshRenderer(gameObj, false);
	}

	private void ActiveMeshRenderer(GameObject gameObj, bool v)
	{
		MeshRenderer mesh = gameObj.GetComponent<MeshRenderer>();
		mesh.enabled = v;
	}

    public void giveHint(TextMesh theQuestion) {
        string questionText = theQuestion.text;

		givenHint.text+= " "+ dialogType.Answers[0].ToString().Split(' ')[++i];
    }

	public void ExecuteSkip()
	{
		if (dialogType.RequiredInput) {
			pause = false;
			ResumeMedia ();
			showUserInput = false;
			answerOK = true;
			sphere.SetActive (false);

		}
	}


	public void SelectVideo(int indice)
	{
		try
		{
			if (experience == null)
			{
				experience = VRExperience.Instance;
			}
			indice--;
			string videoName = experience.SelectVideo(indice);

            counterVideo.Reset();
			if (!videoName.Equals("End"))
			{
				if(!videoName.Equals("Error"))
				{

					navigationPanel.materialOriginal();
					Sub.text="";
					normalText.text = "";
					DesactiveObject(panelExt);
					DesactiveObject(textInfo);
					panelInput.SetActive(false);
                    //TODO: NO DEJAR ESTO HARDCODEADO ARREGLAR
                    //SE ESTA IDENTIFICANDO SI EL VIDEO ES EL 5 PARA CAMBIAR LOS PANELES DE RESPUESTA
                    if (indice == 4)
                    {
                        panelSub.SetActive(false);
                        panelAnswer.SetActive(true);
                        panelQuestion.SetActive(true);
                        panelHintButton.SetActive(true);
                        panelHintText.SetActive(true);
                    }
                    else {
                        panelSub.SetActive(true);
                    }
                    //panelSub.SetActive(true);
                    sphere.SetActive(false);
					InitializeVariables();
					navigationPanel.colorPart();
					navigationPanel.OcultarPart();
					subReader.RestFileReader(videoName);
					media.Load("file://" + Application.persistentDataPath + pathVideos + videoName);
					media.Play();
                    counterVideo.Start();
				}
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

	public void validateAnswer(string answer)
	{
		keyboard.SetActive(false);
		//panelSub.SetActive(true);
		userAnswer.text = answer; // May be wrong, may be right. But we want to see what it is anyway.
		bool evaluatedAnswer = processAnswer.evaluateAnswer(answer, this.dialogType);

		if (evaluatedAnswer) {
			pause = false;
			ResumeMedia();
			showUserInput = false;
			answerOK = true;
			sphere.SetActive (false);
      		userAnswer.text = answer;
	
			userAnswer.color = Color.green;
			gifTick.SetActive (true);
			gifCross.SetActive (false);
		}
		else
		{
			userAnswer.text = answer; 
			userAnswer.color = Color.red;
			panelInput.SetActive (true);
			gifCross.SetActive (true);
		}
	}
		
}
