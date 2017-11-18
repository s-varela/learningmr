using UnityEngine;
using System.Collections;
using System.Threading;
using UnityEngine.UI;
using System.Diagnostics;
using Assets.AnswersLogic;
using System.IO;
using System.Text.RegularExpressions;
using System;
using Assets.Interaction;

public class MediaManager : MonoBehaviour {

    private VRExperience experience = null;

    [SerializeField] private MediaPlayerCtrl media;
    [SerializeField] private AudioSource audioLeft;
    [SerializeField] private AudioSource audioRight;
    [SerializeField] private MediaManagerData data;
	[SerializeField] private VRGameMenu menu;

	[SerializeField] private LoadPanel loadPanel;
	[SerializeField] GameObject panelInfo;
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
    [SerializeField] Material radioButtonSelected;
    [SerializeField] Material radioButtonNotSelected;
    [SerializeField] private TextMesh normalText;
    [SerializeField] NavigationPanel navigationPanel;
    [SerializeField] GameObject mediaDialogMenuPanel;
    VRDialogMenu mediaDialogMenu;

	[SerializeField] TextMesh textRespuesta;
	[SerializeField] GameObject PanelTextLAR;
	[SerializeField] GameObject GuiaPanelInfo;
	[SerializeField] GameObject GuiaPanelInteraccion;
	[SerializeField] GameObject PanelGuiaVidrioPanelInfo;
	[SerializeField] GameObject PanelGuiaVidrioPanelInteraccion;
	[SerializeField] GameObject BtnGuia;

    private const string TECLADO_RESPUESTA_VACIA = "Por favor, ingrese una respuesta.";
    private SubtitleReader subReader;
    private AudioManager audioManager;
    private ProcessAnswer processAnswer;
	private string[] arrayText;
	private Color originalColor;
    private AudioSource sfx;
    private Stopwatch counterVideo;
    private Stopwatch counterAudio;
	private Stopwatch counterDelay;
    private bool changeSub;
    private bool showUserInput;
    private bool listen;
    private bool finish;
    private bool isInPanelInfoMode = false;
	private bool isButtonGuiaOn = false;
   
	private bool answerOK = false;
	private bool menuPause = false;
    private bool emptyKeyboardAnswer = false;
    private int indiceAudio;
    private DialogType dialogType;
	private int i=-1;
    private bool skip = false;
    private ArrayList textForRepeat;
    private string selectedString;
    private int currentPage;
    private const int windows = 5;
    private bool wait;
	private bool firstTimePanelInfo = true;
	private bool firstTimePanelInteraccion = true;

    void Start()
    {
        selectedString = "";
        currentPage = 0;
        audioLeft = new AudioSource();
        experience = VRExperience.Instance;
        dialogType = new DialogType();
        changeSub = false;
		experience.ResetIndice ();
		originalColor = Sub.color;
        textForRepeat = new ArrayList();

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

        mediaDialogMenu = mediaDialogMenuPanel.GetComponent<VRDialogMenu>();
        mediaDialogMenu.OnAcceptClick += DialogAcceptHandle;
  
        menu.OnMenuShow += PauseMedia;
        menu.OnMenuHide += ResumeMedia;
        ManagerVideo();

    }

    private void DialogAcceptHandle()
    {
        ConfigDialogMode();
        mediaDialogMenuPanel.SetActive(false);
    }

    void InitializeVariables()
	{
		changeSub = false;
		answerOK = false;
		showUserInput = false;
		listen = false;
		panelInfo.SetActive(false);
		panelSub.SetActive(true);
		panelInput.SetActive(false);
		sphere.SetActive(false);
		keyboard.SetActive(false);
		panelAnswer.SetActive(false);
		panelQuestion.SetActive(false);
		panelHintButton.SetActive(false);
		panelHintText.SetActive(false);
		PanelTextLAR.SetActive (false);
		PanelGuiaVidrioPanelInfo.SetActive (false);
		PanelGuiaVidrioPanelInteraccion.SetActive (false);
		BtnGuia.SetActive (false);
		//loadPanel.DeleteSub();
		Sub.text="";
		normalText.text = "";
		userAnswer.text = "";
		givenHint.text = "";
		indiceAudio = 0;
        textForRepeat = new ArrayList();
		wait = false;
        currentPage = 0;
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

            listen = false;
            showUserInput = false;
            finish=false;
            indiceAudio = 0;
            dialogType = new DialogType();
            textForRepeat = new ArrayList();
        }
    }


    // Update is called once per frame
    void Update()
    {
        try
        {
			if (!menuPause && !wait)
            {
                if (IsDialogMode())
                {
                    long seconds = counterVideo.ElapsedMilliseconds;
                    // search if duration is in last subtitle second (in miliseconds)

                    dialogType = subReader.ReadSubtitleLine(seconds);

                    if (dialogType != null)
                    {
                        string theSub = dialogType.Text;

                        if (!theSub.Equals("") && theSub != normalText.text)
                        {
                            //arraylist con texto para repetir
                            textForRepeat.Add(theSub);
                            normalText.text = theSub;
                            answerOK = false;
                            Sub.color = originalColor;
                            gifTick.SetActive(false);
                        }

                        if (dialogType.Listen)
                        {
							//ActivePanelTextLAR();
                            ConfigListenMode();

                        }
                        else if (dialogType.RequiredInput && !answerOK)
                        {
                            ConfigInputMode();

                        }
                        else if (dialogType.Finish)
                        {
                            ConfigDialogFinishMode();
                        }

                    }
                }
                else if (IsListenMode())// Modo Listen, muestro panel frontal y reproduzco audios
                {
                    isInPanelInfoMode = true;
  
                    if (ElapsedAudioTime(5000))
                    {
                        try
                        {
							//ConfigListenMode();
                            RepeatDialog(); //Se reproducen los dialogos 
                        }
                        catch (System.Exception ex)
                        {
                            //TODO logger
                            UnityEngine.Debug.Log("[MediaManager][update] " + ex.Message);
                            normalText.text = "Error reproducir audio" + ex.Message;
                        }
                    }
                }
                else if (IsInputMode())
                {
                    isInPanelInfoMode = false;
                    PauseMedia();
                    EnableInterationMenu();
                }
                else if (IsFinishMode())
                {
                    isInPanelInfoMode = false;
                    FinishExperience();
                }
                else if (IsSkipMode())
                {
                    isInPanelInfoMode = false;
                    DisableInterationMenu();
                    ResumeMedia();
                    answerOK = true;
                    skip = false;
                    givenHint.text = "";
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

    private bool IsSkipMode()
    {
        return skip && ElapsedTime(500);
    }

    private bool IsFinishMode()
    {
        return dialogType.Finish && ElapsedTime(2000);
    }

    private bool IsInputMode()
    {
        return showUserInput && ElapsedTime(500) && !keyboard.activeSelf;
    }

    private bool IsListenMode()
    {
        return listen && ElapsedTime(2000);
    }

    private bool IsDialogMode()
    {
        return !listen && !showUserInput && !finish && !skip;
    }

    private void ConfigDialogMode()
    {
        showUserInput = false;
        listen = false;
        menuPause = false;
        wait = false;
        finish = false;
        skip = true;
        counterDelay.Reset();
        counterDelay.Start();
    }


    private void ConfigDialogFinishMode()
    {
        finish = true;
        listen = false;
        counterDelay.Reset();
        counterDelay.Start();
    }

    private void ConfigInputMode()
    {
        i = -1;
        showUserInput = true;
        listen = false;
        menuPause = false;
        wait = false;
        counterDelay.Reset();
        counterDelay.Start();
    }

	private void ActivePanelTextLAR()
	{
		listen = true;
		PauseMedia();
		sphere.SetActive(true);
		panelSub.SetActive(false);
		PanelTextLAR.SetActive (true);
	}

 
    private void ConfigListenMode()
    {
        /**Se oculta panel de informacion y se cargan los textos**/
        indiceAudio = 0;
        listen = true;
        PauseMedia();
        panelInfo.SetActive(true);
		sphere.SetActive(true);
		panelSub.SetActive(false);
		textRespuesta.text = "";
		PanelTextLAR.SetActive (true);

		Vector3 apagar = new Vector3(0.00001f,0.00001f,0.00001f);
		panelInfo.transform.localScale = apagar;

		if(currentPage == 0){
			GameObject BtnAnterior = GameObject.Find ("UI_BtnAnterior");
			BtnAnterior.transform.localScale = apagar;
		}
		DisablePanelInteration ();

        TextInfoFill();
        counterAudio.Start();
        counterDelay.Reset();
        counterDelay.Start();
    }

    private void RepeatDialog()
    {
		//DisablePanelInteration ();
		Vector3 panelInfoScale = new Vector3(3f,2f,0.008f);
		panelInfo.transform.localScale = panelInfoScale;

		PanelTextLAR.SetActive (false);

		int textRepeat = textForRepeat.Count - currentPage*windows;
		if (indiceAudio < windows && indiceAudio < textRepeat)
        {
            TextMesh textObject = GameObject.Find("TextInfo" + (indiceAudio + 1)).GetComponent<TextMesh>();
            //UI_RadioButton1
            GameObject r1 = GameObject.Find("UI_RadioButton1");
            r1.GetComponent<Renderer>().material = radioButtonNotSelected;

            //UI_RadioButton2
            GameObject r2 = GameObject.Find("UI_RadioButton2");
            r2.GetComponent<Renderer>().material = radioButtonNotSelected;

            //UI_RadioButton3
            GameObject r3 = GameObject.Find("UI_RadioButton3");
            r3.GetComponent<Renderer>().material = radioButtonNotSelected;

            //UI_RadioButton4
            GameObject r4 = GameObject.Find("UI_RadioButton4");
            r4.GetComponent<Renderer>().material = radioButtonNotSelected;

            //UI_RadioButton5
            GameObject r5 = GameObject.Find("UI_RadioButton5");
            r5.GetComponent<Renderer>().material = radioButtonNotSelected;

            //assign selected material to selected radio button and paint selected text in yellow
            GameObject radioSelected = GameObject.Find("UI_RadioButton" + (indiceAudio + 1));
            radioSelected.GetComponent<Renderer>().material = radioButtonSelected;
            textObject.color = Color.yellow;
            PlayAudio(textObject.text);
     
            if (indiceAudio > 0)
            {
                TextMesh textObjectBefore = GameObject.Find("TextInfo" + indiceAudio).GetComponent<TextMesh>();
                textObjectBefore.color = Color.white;
            }

            counterAudio.Stop();
            counterAudio.Reset();
            counterAudio.Start();
            indiceAudio++;
        }
        else //Se reprodujeron todos los audios, espero 
        {
            GameObject.Find("TextInfo1").GetComponent<TextMesh>().color = Color.yellow;
           	
			//UI_RadioButton1
            GameObject firstRad = GameObject.Find("UI_RadioButton1");
            firstRad.GetComponent<Renderer>().material = radioButtonSelected;

			//UI_RadioButton2
			GameObject r2 = GameObject.Find("UI_RadioButton2");
			r2.GetComponent<Renderer>().material = radioButtonNotSelected;

			//UI_RadioButton3
			GameObject r3 = GameObject.Find("UI_RadioButton3");
			r3.GetComponent<Renderer>().material = radioButtonNotSelected;

			//UI_RadioButton4
			GameObject r4 = GameObject.Find("UI_RadioButton4");
			r4.GetComponent<Renderer>().material = radioButtonNotSelected;

			//UI_RadioButton5
			GameObject r5 = GameObject.Find("UI_RadioButton5");
			r5.GetComponent<Renderer>().material = radioButtonNotSelected;
            TextMesh textObjectBefore = GameObject.Find("TextInfo" + indiceAudio).GetComponent<TextMesh>();
            textObjectBefore.color = Color.white;

            //activar botones interaccion
			EnablePanelInteration ();

            indiceAudio = 0; //reseteo el contador
            counterAudio.Stop();
            counterAudio.Reset();
            counterAudio.Start();
            listen = false;
            wait = true;
            isInPanelInfoMode = true;
            
        }
    }


    private bool ElapsedTime(int seconds)
    {
        return counterDelay.ElapsedMilliseconds > seconds;
    }

    private bool ElapsedAudioTime(int seconds)
    {
        return counterAudio.ElapsedMilliseconds > seconds;
    }

    private void StartDelayTime()
    {
        counterDelay.Reset();
        counterDelay.Start();
    }

    public void EnableInterationMenu()
    {
        sphere.SetActive(true);
        panelSub.SetActive(false);
        panelInput.SetActive(true);
        panelAnswer.SetActive(true);
     
        panelQuestion.SetActive(true);

        TextMesh textObject = GameObject.Find("QuestionText").GetComponent<TextMesh>();
        textObject.text = dialogType.Text; ;

        panelHintButton.SetActive(true);
        panelHintText.SetActive(true);

		BtnGuia.SetActive (true);

		if (firstTimePanelInteraccion) {
			ButtonGuiaOn();
			firstTimePanelInteraccion = false;
		}
    }

    public void DisableInterationMenu()
    {
        listen = false;
        showUserInput = false;
        sphere.SetActive(false);
        panelInput.SetActive(false);
        panelSub.SetActive(true);
        panelAnswer.SetActive(false);
        panelQuestion.SetActive(false);
        panelHintButton.SetActive(false);
        panelHintText.SetActive(false);
		userAnswer.text = "";
		givenHint.text = "";
		Sub.text = "";
		BtnGuia.SetActive (false);
    }

    public void KeyboardExitButton() {

        string answer = keyboardInp.text;

        if (isInPanelInfoMode) {

            KeyboardExitRepeatPanel();

        } else {

            keyboardInp.text = "";
            keyboard.SetActive(false);
            emptyKeyboardAnswer = false;


            if (!emptyKeyboardAnswer)
            {
                emptyKeyboardAnswer = false;
                panelSub.SetActive(false);
                panelInput.SetActive(true);
                panelAnswer.SetActive(true);
                panelQuestion.SetActive(true);
                panelHintButton.SetActive(true);
                hintButton.SetActive(true);
                skipButton.SetActive(true);
                panelHintText.SetActive(true);
                gifCross.SetActive(false);
                gifTick.SetActive(false);
            }

        }
    }

	public void KeyboardOKButton(){

        string answer = keyboardInp.text;
        answer = answer.Trim();
        answer = Regex.Replace(answer, @"\s+", " ");

        if (isInPanelInfoMode) {

            keyboardOKRepeatPanel();

        } else {

            if (answer.Equals("") || answer.Equals(TECLADO_RESPUESTA_VACIA))
            {
                emptyKeyboardAnswer = true;
            }
            else {
                emptyKeyboardAnswer = false;
            }

            if (!emptyKeyboardAnswer)
            {
                emptyKeyboardAnswer = false;
                ValidateAnswer(answer);
                panelSub.SetActive(false);
                panelInput.SetActive(true);
                panelAnswer.SetActive(true);
                panelQuestion.SetActive(true);
                panelHintButton.SetActive(true);
                panelHintText.SetActive(true);
                hintButton.SetActive(true);
                skipButton.SetActive(true);
            }
        }
    }

    public void KeyboardExitRepeatPanel()
    {
        keyboardInp.text = "";
        panelInfo.SetActive(true);
        keyboard.SetActive(false);
    }

    public void keyboardOKRepeatPanel() {

        panelInfo.SetActive(true);
        keyboard.SetActive(false);
        string answer = keyboardInp.text;
        answer = answer.Replace(".", string.Empty);

        if (answer.Equals("") || answer.Equals(TECLADO_RESPUESTA_VACIA))
        {
            emptyKeyboardAnswer = true;
        }
        else {
            emptyKeyboardAnswer = false;
        }

        if (!emptyKeyboardAnswer)
        {
            emptyKeyboardAnswer = false;
            ValidateAnswerRepeatPanel(answer);

        }
    }
     
    public void ValidateAnswerRepeatPanel(string answer)
    {
        radioButtonInteraction selectedRadio = radioButtonInteraction.Instance;
        int numberRadioSelected = selectedRadio.WhichRadioSelected();
        string selectedString = GameObject.Find("TextInfo" + numberRadioSelected).GetComponent<TextMesh>().text;

        keyboard.SetActive(false);

        answer = answer.Replace("!", string.Empty);
        answer = answer.Replace(".", string.Empty);
        answer = answer.Trim();
        answer = Regex.Replace(answer, @"\s+", " ");

        selectedString = selectedString.Replace("!", string.Empty);
        selectedString = selectedString.Replace(".", string.Empty);

        bool evaluatedAnswer = answer.ToLower().Contains(selectedString.ToLower());

        if (evaluatedAnswer)
        {
            listen = false;
            showUserInput = false;
            answerOK = true;
            GameObject.Find("TextInfo1").GetComponent<TextMesh>().color = Color.white;
            GameObject.Find("TextInfo2").GetComponent<TextMesh>().color = Color.white;
            GameObject.Find("TextInfo3").GetComponent<TextMesh>().color = Color.white;
            GameObject.Find("TextInfo4").GetComponent<TextMesh>().color = Color.white;
            GameObject.Find("TextInfo5").GetComponent<TextMesh>().color = Color.white;
            GameObject.Find("TextInfo" + numberRadioSelected).GetComponent<TextMesh>().color = Color.green;
            PlayAudio("correct");
        }
        else
        {
            GameObject.Find("TextInfo1").GetComponent<TextMesh>().color = Color.white;
            GameObject.Find("TextInfo2").GetComponent<TextMesh>().color = Color.white;
            GameObject.Find("TextInfo3").GetComponent<TextMesh>().color = Color.white;
            GameObject.Find("TextInfo4").GetComponent<TextMesh>().color = Color.white;
            GameObject.Find("TextInfo5").GetComponent<TextMesh>().color = Color.white;
            GameObject.Find("TextInfo" + numberRadioSelected).GetComponent<TextMesh>().color = Color.red;
            PlayAudio("incorrect");
        }
    }

    public void ValidateAnswerRepeatPanelVoice(string answer)
    {
        radioButtonInteraction selectedRadio = radioButtonInteraction.Instance;
        int numberRadioSelected = selectedRadio.WhichRadioSelected();
        string selectedString = GameObject.Find("TextInfo" + numberRadioSelected).GetComponent<TextMesh>().text;

        answer = answer.Trim();
        answer = Regex.Replace(answer, @"\s+", " ");

        selectedString = selectedString.Replace("!", string.Empty);
        selectedString = selectedString.Replace(".", string.Empty);
        selectedString = selectedString.Replace(",", string.Empty);
        selectedString = selectedString.Replace("?", string.Empty);

        bool evaluatedAnswer = answer.ToLower().Contains(selectedString.ToLower());

        if (evaluatedAnswer)
        {
            listen = false;
            showUserInput = false;
            answerOK = true;
            GameObject.Find("TextInfo1").GetComponent<TextMesh>().color = Color.white;
            GameObject.Find("TextInfo2").GetComponent<TextMesh>().color = Color.white;
            GameObject.Find("TextInfo3").GetComponent<TextMesh>().color = Color.white;
            GameObject.Find("TextInfo4").GetComponent<TextMesh>().color = Color.white;
            GameObject.Find("TextInfo5").GetComponent<TextMesh>().color = Color.white;
            GameObject.Find("TextInfo" + numberRadioSelected).GetComponent<TextMesh>().color = Color.green;
            PlayAudio("correct");
        }
        else
        {
            GameObject.Find("TextInfo1").GetComponent<TextMesh>().color = Color.white;
            GameObject.Find("TextInfo2").GetComponent<TextMesh>().color = Color.white;
            GameObject.Find("TextInfo3").GetComponent<TextMesh>().color = Color.white;
            GameObject.Find("TextInfo4").GetComponent<TextMesh>().color = Color.white;
            GameObject.Find("TextInfo5").GetComponent<TextMesh>().color = Color.white;
            GameObject.Find("TextInfo" + numberRadioSelected).GetComponent<TextMesh>().color = Color.red;
            PlayAudio("incorrect");
        }
    }

    
	private void MenuPause()
	{
		menuPause = true;
		media.Pause();
		counterVideo.Stop();
	}

	private void MenuResume()
	{
		menuPause = false;
		media.Play();
		counterVideo.Start();
	}

	private void PauseMedia()
    {
        media.Pause();
        counterVideo.Stop();
    }

    private void ResumeMedia()
    {
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
        //loadPanel.DeleteSub();
		listen = false;
        //loadPanel.DeleteSub();
        counterVideo.Reset();
        counterVideo.Stop();
        counterVideo.Start();
		panelInfo.SetActive(false);
		sphere.SetActive(false);
		panelSub.SetActive(true);
        textForRepeat.Clear();
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
					navigationPanel.materialOriginal();
					navigationPanel.colorPart();
                    InitializeVariables();
                    subReader.RestFileReader(videoName,experience.ResourcesPath+experience.MatedataPath);
                	media.Load("file://" + experience.ResourcesPath + experience.VideosPath + videoName);
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
		SetInactiveButtonGuia ();
        string questionText = theQuestion.text;

		givenHint.text+= " "+ dialogType.Answers[0].ToString().Split(' ')[++i];
    }

	public void ExecuteSkip()
	{
		SetInactiveButtonGuia();
        mediaDialogMenuPanel.SetActive(true);
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
					panelInfo.SetActive(false);
					panelInput.SetActive(false);
           
                    sphere.SetActive(false);
					InitializeVariables();
					navigationPanel.colorPart();
					navigationPanel.OcultarPart();

                    subReader.RestFileReader(videoName, experience.ResourcesPath + experience.MatedataPath);
                    media.Load("file://" + experience.ResourcesPath + experience.VideosPath + videoName);
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

	public void DisplayWarningMessage(string message)
	{
		userAnswer.text = message;
		userAnswer.color = Color.red;
	}

	public void ValidateAnswer(string answer)
	{
        keyboard.SetActive(false);
        //delete leading and final white-spaces 
        answer = answer.Trim();
        //substitute multiple spaces from within words for one space
        answer = Regex.Replace(answer, @"\s+", " ");

        bool evaluatedAnswer = processAnswer.evaluateAnswer(answer, this.dialogType);

		if (evaluatedAnswer) {
			listen = false;
			showUserInput = false;
			answerOK = true;
			sphere.SetActive (false);
      		userAnswer.text = answer;
            givenHint.text = "";
            userAnswer.color = Color.green;
			gifTick.SetActive (true);
			gifCross.SetActive (false);
			skip = true;
			StartDelayTime ();
		}
		else
		{
            userAnswer.text = answer;
            userAnswer.color = Color.red;
            panelInput.SetActive (true);
			gifCross.SetActive (true);
		}
	}

    public void selectRadioButton(GameObject radioSelected, int radioInt) {
        //paint all texts white and deselect all radio buttons

        //UI_RadioButton1
        GameObject r1 = GameObject.Find("UI_RadioButton1");
        r1.GetComponent<Renderer>().material = radioButtonNotSelected;
        GameObject.Find("TextInfo1").GetComponent<TextMesh>().color = Color.white;

        //UI_RadioButton2
        GameObject r2 = GameObject.Find("UI_RadioButton2");
        r2.GetComponent<Renderer>().material = radioButtonNotSelected;
        GameObject.Find("TextInfo2").GetComponent<TextMesh>().color = Color.white;

        //UI_RadioButton3
        GameObject r3 = GameObject.Find("UI_RadioButton3");
        r3.GetComponent<Renderer>().material = radioButtonNotSelected;
        GameObject.Find("TextInfo3").GetComponent<TextMesh>().color = Color.white;

        //UI_RadioButton4
        GameObject r4 = GameObject.Find("UI_RadioButton4");
        r4.GetComponent<Renderer>().material = radioButtonNotSelected;
        GameObject.Find("TextInfo4").GetComponent<TextMesh>().color = Color.white;

        //UI_RadioButton5
        GameObject r5 = GameObject.Find("UI_RadioButton5");
        r5.GetComponent<Renderer>().material = radioButtonNotSelected;
        GameObject.Find("TextInfo5").GetComponent<TextMesh>().color = Color.white;

        //assign selected material to selected radio button and paint selected text in yellow
        radioSelected.GetComponent<Renderer>().material = radioButtonSelected;
        string selectedString = GameObject.Find("TextInfo" + radioInt).GetComponent<TextMesh>().text;
        GameObject.Find("TextInfo" + radioInt).GetComponent<TextMesh>().color = Color.yellow;
    }

    //metodo repetir audio panel de resumen
    public void repeatAudio(string selectedString) {
        PlayAudio(selectedString);
    }

    //metodo proxima pagina panel de resumen
    public void NextPage() {
      
        currentPage += 1;
        if (currentPage * windows < textForRepeat.Count)
        {
			Vector3 prender = new Vector3(0.4f,0.8f,1f);
			GameObject BtnAnterior = GameObject.Find ("UI_BtnAnterior");
			BtnAnterior.transform.localScale = prender;
            ConfigListenMode();
            wait = false;
        }
        else {
            FinishLessonPart();
        }
      
    }

    //metodo pagina anterior panel de resumen
    public void PreviousPage()
    {
		if (currentPage > 0) {
			currentPage -= 1;
            ConfigListenMode();
            wait = false;
        }
    }


    //metodo rellenar panel de resumen al final de cada video
    public void TextInfoFill() {
		//primero activa todo los radioButton y los panelInfo
		Vector3 prender = new Vector3(0.6f,0.1f,1f);
		for (int i = 1; i <= windows; i++) {
			GameObject.Find("UI_RadioButton"+i).transform.localScale = prender;
			GameObject.Find ("PanelInfo"+i).transform.localScale = prender;
		}

        int indice = 0;
        for (int i = 0; i < windows; i ++) {
            TextMesh textObject = GameObject.Find("TextInfo"+(i+1)).GetComponent<TextMesh>();
            indice = currentPage * windows + i;

            if (indice < textForRepeat.Count)
            {
                string text = textForRepeat[indice].ToString();
                textObject.text = text;
                textObject.color = Color.white;
            }
            else {
                textObject.text = "";
            }
        }
		int textRepeat = textForRepeat.Count - currentPage * windows;
		Vector3 apagar = new Vector3(0.00001f,0.00001f,0.00001f);
		for (int i = textRepeat + 1; i <= windows; i++) {
			GameObject.Find("UI_RadioButton"+i).transform.localScale = apagar;
			GameObject.Find ("PanelInfo"+i).transform.localScale = apagar;
		}
    }

	public void DisablePanelInteration()
	{
		Vector3 apagar = new Vector3(0.00001f,0.00001f,0.00001f);
		GameObject InteraccionBasePanelInfo = GameObject.Find("UI_InteraccionBasePanelInfo");
		InteraccionBasePanelInfo.transform.localScale = apagar;

		GameObject Paginado = GameObject.Find("UI_Paginado");
		Paginado.transform.localScale = apagar;
	}

	public void EnablePanelInteration()
	{
		Vector3 interaccionScale = new Vector3(0.02f,0.09f,0.075f);
		Vector3 paginadoScale = new Vector3(0.03f,0.018f,0.075f);

		GameObject InteraccionBasePanelInfo = GameObject.Find("UI_InteraccionBasePanelInfo");
		InteraccionBasePanelInfo.transform.localScale = interaccionScale;
		GameObject Paginado = GameObject.Find("UI_Paginado");
		Paginado.transform.localScale = paginadoScale;

		BtnGuia.SetActive (true);

		if (firstTimePanelInfo) {
			ButtonGuiaOn();
			firstTimePanelInfo = false;
		}
	}

	public void ButtonGuia()
	{
		if (!isButtonGuiaOn) {
			ButtonGuiaOn ();
		} else {
			ButtonGuiaOff ();
		}
	}

	private void ButtonGuiaOn()
	{
		if (panelInfo.activeSelf) {
			GuiaPanelInfo.SetActive(true);
			//PanelGuiaVidrioPanelInfo.SetActive (true);
			isButtonGuiaOn = true;
		} else if (panelInput.activeSelf) {
			GuiaPanelInteraccion.SetActive(true);
			//PanelGuiaVidrioPanelInteraccion.SetActive (true);
			isButtonGuiaOn = true;
		}
	}

	private void ButtonGuiaOff()
	{
		if (panelInfo.activeSelf) {
			GuiaPanelInfo.SetActive(false);
			//PanelGuiaVidrioPanelInfo.SetActive (false);
			isButtonGuiaOn = false;
		} else if (panelInput.activeSelf) {
			GuiaPanelInteraccion.SetActive(false);
			//PanelGuiaVidrioPanelInteraccion.SetActive (false);
			isButtonGuiaOn = false;
		}
	}

	public void SetInactiveButtonGuia()
	{
		if (isButtonGuiaOn) {
			GuiaPanelInfo.SetActive(false);
			GuiaPanelInteraccion.SetActive(false);
			isButtonGuiaOn = false;
		}
	}
}
