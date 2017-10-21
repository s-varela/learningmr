using UnityEngine;
using System.Collections;
using System.Threading;
using UnityEngine.UI;
using System.Diagnostics;
using Assets.AnswersLogic;
using System.IO;
using System.Text.RegularExpressions;
using System;

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
    [SerializeField] NavigationPanel navigationPanel;
    [SerializeField] GameObject mediaDialogMenuPanel;
    VRMediaMenu mediaDialogMenu;

    private const string TECLADO_RESPUESTA_VACIA = "Por favor, ingrese una respuesta.";
    private SubtitleReader subReader;
    private AudioManager audioManager;
    private ProcessAnswer processAnswer;
    private string pathVideos = "/lesson1-data/videos/";
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
    [SerializeField] private TextMesh normalText;
	private bool answerOK = false;
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

    public event Action OnDialogShow;

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


        //mediaDialogMenu= GameObject.Find("SkipMenu").GetComponent<VRMediaMenu>();
      
        mediaDialogMenu = mediaDialogMenuPanel.GetComponent<VRMediaMenu>();
    
        mediaDialogMenu.OnAcceptClick += DialogAcceptHandle;
        mediaDialogMenu.OnCancelClick += DialogCancelHandle;
        mediaDialogMenu.transform.localScale = new Vector3(0, 0, 0); //Oculto panel de dialogo

        menu.OnMenuShow += PauseMedia;
        menu.OnMenuHide += ResumeMedia;
        ManagerVideo();

    }

    private void DialogCancelHandle()
    {
        givenHint.text = "CANCEL";
       // mediaDialogMenu.transform.localScale = new Vector3(0, 0, 0); //Oculto panel de dialogo
    }

    private void DialogAcceptHandle()
    {
        //skip = true;
        //showUserInput = false;
        givenHint.text = "ACCEPT";
        //mediaDialogMenu.transform.localScale = new Vector3(0, 0, 0); //Oculto panel de dialogo

    }

    private void DialogHidenHandle()
    {
        givenHint.text = "HIDEN";
    }

    private void DialogShowHandle()
    {
        givenHint.text = "SHOW";
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
		//loadPanel.DeleteSub();
		Sub.text="";
		normalText.text = "";
		userAnswer.text = "";
		givenHint.text = "";
		indiceAudio = 0;
        textForRepeat = new ArrayList();
		wait = false;
        currentPage = 0;
        //mediaDialogMenuPanel.SetActive(false);

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
            if (!wait)
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
                    sphere.SetActive(true);
                    panelSub.SetActive(false);
                    panelInfo.transform.localScale = new Vector3(3, 2, 0.008f); //Muestro el panel
  
                    if (ElapsedAudioTime(3000))
                    {
                        try
                        {
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
                    PauseMedia();
                    EnableInterationMenu();
                }
                else if (IsFinishMode())
                {
                    FinishExperience();
                }
                else if (IsSkipMode())
                {
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
        return skip && ElapsedTime(2000);
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
        counterDelay.Reset();
        counterDelay.Start();
    }

 
    private void ConfigListenMode()
    {
        /**Se oculta panel de informacion y se cargan los textos**/
        indiceAudio = 0;
        listen = true;
        PauseMedia();
        panelInfo.SetActive(true);
        panelInfo.transform.localScale = new Vector3(0, 0, 0); 

        TextInfoFill();
        counterAudio.Start();
        counterDelay.Reset();
        counterDelay.Start();
    }

    private void RepeatDialog()
    {
        if (indiceAudio < windows)
        {
            TextMesh textObject = GameObject.Find("TextInfo" + (indiceAudio + 1)).GetComponent<TextMesh>();
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
        else //Se reprodujeron todos los audios, epero 
        {
            TextMesh textObjectBefore = GameObject.Find("TextInfo" + indiceAudio).GetComponent<TextMesh>();
            textObjectBefore.color = Color.white;

            indiceAudio = 0; //reseteo el contador
            counterAudio.Stop();
            counterAudio.Reset();
            counterAudio.Start();
            listen = false;
            wait = true;
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
    }

    public void KeyboardExitButton(){

        string answer = keyboardInp.text;

        if (panelInfo.activeSelf)
        {
            KeyboardExitRepeatPanel();
        }
        else {

            keyboardInp.text = "";
            keyboard.SetActive(false);
            emptyKeyboardAnswer = false;
        }
        
        if (!emptyKeyboardAnswer) {
            emptyKeyboardAnswer = false;
            ValidateAnswer(answer);
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

	public void KeyboardOKButton(){

        if (panelInfo.activeSelf)
        {
            keyboardOKRepeatPanel();
        }
        else {

            string answer = keyboardInp.text;

            answer = answer.Trim();
            answer = Regex.Replace(answer, @"\s+", " ");

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
        keyboard.SetActive(false);
    }

    public void keyboardOKRepeatPanel() {
        string answer = keyboardInp.text;

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
        keyboard.SetActive(false);

        answer = answer.Trim();
        answer = Regex.Replace(answer,@"\s+", " ");

        bool evaluatedAnswer = processAnswer.evaluateAnswer(answer, this.dialogType);

        if (evaluatedAnswer)
        {
            listen = false;
            showUserInput = false;
            answerOK = true;
            sphere.SetActive(false);
            userAnswer.text = answer;
            givenHint.text = "";
            userAnswer.color = Color.green;
            gifTick.SetActive(true);
            gifCross.SetActive(false);
            skip = true;
            StartDelayTime();
        }
        else
        {
            userAnswer.text = answer;
            userAnswer.color = Color.red;
            panelInput.SetActive(true);
            gifCross.SetActive(true);
        }
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
        loadPanel.DeleteSub();
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
        givenHint.text = "PUTOOOOO"; 
       UnityEngine.Debug.Log("[MediaManager][ExecuteSkip] " + "Ejecutando Skip");
        //GameObject menuSkip= GameObject.Find("SkipMenu");

        mediaDialogMenu.transform.position = new Vector3(0.4f, -3.2f, 2.9f); 
        mediaDialogMenu.transform.localScale = new Vector3(1, 1, 1); //Muestro panel de dialogo
        //mediaDialogMenuPanel.SetActive(true);
        //menuSkip.SetActive(true);
       // mediaDialogMenuPanel.SetActive(true);
        //OnDialogShow();

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

    public void selectRadioButton(GameObject radioSelected) {
        UnityEngine.Debug.Log("[MM][selectRadiobutton][entro]");
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

        int radioInt = int.Parse(Regex.Match(radioSelected.name, @"\d+").Value);
        selectedString = GameObject.Find("TextInfo" + radioInt).GetComponent<TextMesh>().text;
        UnityEngine.Debug.Log("selectedstring: " + selectedString);
        radioSelected.GetComponent<Renderer>().material = radioButtonSelected;
    }

    //metodo repetir audio panel de resumen
    public void repeatAudio() {
        UnityEngine.Debug.Log("selectedstring repeat: " + selectedString);
        PlayAudio(selectedString);
    }

    public void RepeatAudio(TextMesh repeatSub) {
        PlayAudio(repeatSub.text);
    }

    //metodo proxima pagina panel de resumen
    public void NextPage() {
      
        currentPage += 1;
        if (currentPage * windows < textForRepeat.Count)
        {
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
        int indice = 0;
        UnityEngine.Debug.Log("[MediaManager][TextInfoFill] currentPage: " + currentPage);

        for (int i = 0; i < windows; i ++) {

            UnityEngine.Debug.Log("[MediaManager][TextInfoFill] buscando TextInfo"+i+1);
            TextMesh textObject = GameObject.Find("TextInfo"+(i+1)).GetComponent<TextMesh>();
            indice = currentPage * windows + i;

            UnityEngine.Debug.Log("[MediaManager][TextInfoFill] indice: " + indice);

            if (indice < textForRepeat.Count)
            {
                string text = textForRepeat[indice].ToString();
                textObject.text = text;
                textObject.color = Color.white;

                UnityEngine.Debug.Log("[MediaManager][TextInfoFill] text: " + text);
            }
            else {
                textObject.text = "";
            }
        }

    }

}
